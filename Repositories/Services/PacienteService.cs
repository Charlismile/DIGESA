using DIGESA.Models.Entities.DBDIGESA;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

public class PacienteService : IPacienteService
{
    private readonly IPacienteRepository _pacienteRepo;
    private readonly IMedicoRepository _medicoRepo;
    private readonly IDiagnosticoRepository _diagnosticoRepo;
    private readonly ITratamientoRepository _tratamientoRepo;
    private readonly IAcompananteRepository _acompananteRepo;
    private readonly ISolicitudRepository _solicitudRepo; // Nuevo repo
    private readonly ISolicitudDiagnosticoRepository _solicitudDiagnosticoRepo; // Nuevo repo
    private readonly DbContextDigesa _context;
    private readonly ILogger<PacienteService> _logger;
    private readonly IMapper _mapper; // Agregado para usar AutoMapper

    public PacienteService(
        IPacienteRepository pacienteRepo,
        IMedicoRepository medicoRepo,
        IDiagnosticoRepository diagnosticoRepo,
        ITratamientoRepository tratamientoRepo,
        IAcompananteRepository acompananteRepo,
        ISolicitudRepository solicitudRepo,
        ISolicitudDiagnosticoRepository solicitudDiagnosticoRepo,
        DbContextDigesa context,
        IMapper mapper,
        ILogger<PacienteService> logger)
    {
        _pacienteRepo = pacienteRepo;
        _medicoRepo = medicoRepo;
        _diagnosticoRepo = diagnosticoRepo;
        _tratamientoRepo = tratamientoRepo;
        _acompananteRepo = acompananteRepo;
        _solicitudRepo = solicitudRepo;
        _solicitudDiagnosticoRepo = solicitudDiagnosticoRepo;
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<int> CreateAsync(PacienteRegistroDTO model)
{
    var transaction = await _context.Database.BeginTransactionAsync();
    try
    {
        // var errores = model.Validate();
        // if (errores.Count > 0)
        //     throw new ValidationException(string.Join(", ", errores));

        // Mapear paciente
        var paciente = _mapper.Map<Paciente>(model);
        await _pacienteRepo.AddAsync(paciente);
        await _context.SaveChangesAsync(); // Guardamos para obtener Id

        // Acompañante (si aplica)
        if (model.RequiereAcompanante && model.Acompanante != null)
        {
            var acompanante = _mapper.Map<Acompanante>(model.Acompanante);
            acompanante.PacienteId = paciente.Id;
            await _acompananteRepo.AddAsync(acompanante);
            await _context.SaveChangesAsync();
        }

        // Médico
        var medicoExiste = await _medicoRepo.ExistePorDocumento(model.Medico.RegistroIdoneidad);
        if (medicoExiste)
        {
            throw new ValidationException("El médico ya está registrado.");
        }

        var medico = _mapper.Map<Medico>(model.Medico);
        await _medicoRepo.AddAsync(medico);
        await _context.SaveChangesAsync();

        // Solicitud - CORRECTAMENTE CREADA DESDE CERO
        var solicitud = new Solicitud
        {
            PacienteId = paciente.Id,
            MedicoId = medico.Id,
            FechaSolicitud = DateTime.Now,
            EstadoSolicitudId = 1 // Pendiente
        };

        await _solicitudRepo.AddAsync(solicitud);
        await _context.SaveChangesAsync();

        // Diagnósticos
        foreach (var diag in model.Diagnosticos)
        {
            var diagnostico = await _diagnosticoRepo.GetByNombreOrCodigoAsync(diag.Nombre, diag.CodigoCIE10);

            if (diagnostico == null)
            {
                diagnostico = _mapper.Map<Diagnostico>(diag);
                diagnostico.Descripcion = diag.Observaciones ?? $"Diagnóstico creado automáticamente - {diag.Nombre}";
                await _diagnosticoRepo.GetOrCreateAsync(diagnostico);
            }

            var solicitudDiagnostico = new SolicitudDiagnostico
            {
                SolicitudId = solicitud.Id,
                DiagnosticoId = diagnostico.Id,
                EsPrimario = model.Diagnosticos.FirstOrDefault()?.CodigoCIE10 == diag.CodigoCIE10,
                Observaciones = diag.Observaciones
            };

            await _solicitudDiagnosticoRepo.AddAsync(solicitudDiagnostico);
        }

        await _context.SaveChangesAsync();

        // Tratamiento
        var tratamiento = _mapper.Map<Tratamiento>(model.Tratamiento);
        tratamiento.SolicitudId = solicitud.Id;

        await _tratamientoRepo.AddAsync(tratamiento);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();
        return paciente.Id;
    }
    catch (ValidationException ex)
    {
        await transaction.RollbackAsync();
        _logger.LogWarning(ex, "Validación fallida en registro de paciente");
        throw;
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
        _logger.LogError(ex, "Error al registrar paciente");
        throw;
    }
}
}