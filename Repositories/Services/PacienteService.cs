using DIGESA.Models.DTOs;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Services.Interfaces;

public class PacienteService : IPacienteService
{
    private readonly DbContextDigesa _context;

    public PacienteService(DbContextDigesa context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Paciente>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<int> CreateAsync(PacienteRegistroDTO model)
    {
        var paciente = new Paciente
        {
            NombreCompleto = model.NombreCompleto,
            TipoDocumento = model.TipoDocumento,
            NumeroDocumento = model.NumeroDocumento,
            Nacionalidad = model.Nacionalidad,
            FechaNacimiento = model.FechaNacimiento,
            Sexo = model.Sexo,
            DireccionResidencia = model.DireccionResidencia,
            TelefonoResidencial = model.TelefonoResidencial,
            TelefonoPersonal = model.TelefonoPersonal,
            TelefonoLaboral = model.TelefonoLaboral,
            CorreoElectronico = model.CorreoElectronico,
            InstalacionSalud = model.InstalacionSalud,
            RegionSalud = model.RegionSalud,
            RequiereAcompanante = model.RequiereAcompanante,
            MotivoRequerimientoAcompanante = model.MotivoRequerimientoAcompanante,
            TipoDiscapacidad = model.TipoDiscapacidad
        };

        await _context.Paciente.AddAsync(paciente);
        await _context.SaveChangesAsync();

        if (model.RequiereAcompanante && model.Acompanante != null && !string.IsNullOrEmpty(model.Acompanante.NombreCompleto))
        {
            var acompanante = new Acompanante
            {
                PacienteId = paciente.Id,
                NombreCompleto = model.Acompanante.NombreCompleto,
                TipoDocumento = model.Acompanante.TipoDocumento,
                NumeroDocumento = model.Acompanante.NumeroDocumento,
                Nacionalidad = model.Acompanante.Nacionalidad,
                Parentesco = model.Acompanante.Parentesco
            };

            await _context.Acompanante.AddAsync(acompanante);
            await _context.SaveChangesAsync();
        }

        return paciente.Id;
    }
}