// Archivo: SolicitudService.cs

using DIGESA.DTOs;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services
{
    public class SolicitudService : ISolicitudService
    {
        private readonly DbContextDigesa _context;

        public SolicitudService(DbContextDigesa context)
        {
            _context = context;
        }
        
        //ubicacion
        public async Task<List<RegistroDto>> ObtenerProvinciasAsync()
        {
            return await _context.Provincia
                .Select(p => new RegistroDto
                {
                    PId = p.Id,
                    NombreProvincia = p.NombreProvincia
                })
                .OrderBy(p => p.NombreProvincia)
                .ToListAsync();
        }

        public async Task<List<RegistroDto>> ObtenerDistritosPorProvinciaAsync(int provinciaId)
        {
            return await _context.Distrito
                .Where(d => d.ProvinciaId == provinciaId)
                .Select(d => new RegistroDto
                {
                    DId = d.Id,
                    NombreDistrito = d.NombreDistrito,
                    ProvinciaId = d.ProvinciaId
                })
                .OrderBy(d => d.NombreDistrito)
                .ToListAsync();
        }

        public async Task<List<RegistroDto>> ObtenerCorregimientosPorDistritoAsync(int distritoId)
        {
            return await _context.Corregimiento
                .Where(c => c.DistritoId == distritoId)
                .Select(c => new RegistroDto
                {
                    CId = c.Id,
                    NombreCorregimiento = c.NombreCorregimiento,
                    DistritoId = c.DistritoId
                })
                .OrderBy(c => c.NombreCorregimiento)
                .ToListAsync();
        }

        public async Task<int> CrearSolicitudAsync(RegistroDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Buscar o crear Paciente
                var paciente = await _context.Paciente
                    .FirstOrDefaultAsync(p => p.TipoDocumento == dto.TipoDocumento && p.NumeroDocumento == dto.NumeroDocumento);

                if (paciente == null)
                {
                    paciente = new Paciente
                    {
                        NombreCompleto = dto.NombreCompleto,
                        TipoDocumento = dto.TipoDocumento,
                        NumeroDocumento = dto.NumeroDocumento,
                        Nacionalidad = dto.Nacionalidad,
                        FechaNacimiento = dto.FechaNacimiento ?? DateTime.Now,
                        Sexo = dto.Sexo,
                        InstalacionSalud = dto.InstalacionSalud,
                        RegionSalud = dto.RegionSalud,
                        RequiereAcompanante = dto.RequiereAcompanante,
                        MotivoRequerimientoAcompanante = string.Join("; ",
                            (dto.EsPacienteMenorEdad ? "Menor de edad" : null),
                            (dto.EsPacienteMayorDiscapacidad ? $"Mayor con discapacidad: {dto.DiscapacidadDescripcion}" : null)
                        ).Trim(';', ' '),
                        TipoDiscapacidad = dto.DiscapacidadDescripcion,
                        DireccionExacta = dto.DireccionExacta,
                        TelefonoResidencial = dto.TelefonoResidencial,
                        TelefonoPersonal = dto.TelefonoPersonal,
                        TelefonoLaboral = dto.TelefonoLaboral,
                        CorreoElectronico = dto.CorreoElectronico,
                        FirmaBase64 = dto.FirmaBase64
                    };

                    _context.Paciente.Add(paciente);
                    await _context.SaveChangesAsync();
                }

                // 2. Buscar o crear Médico
                var medico = await _context.Medico
                    .FirstOrDefaultAsync(m => m.NumeroRegistroIdoneidad == dto.MedicoNumeroRegistroIdoneidad);

                if (medico == null)
                {
                    medico = new Medico
                    {
                        NombreCompleto = dto.MedicoNombreCompleto,
                        EsMedicoEspecialista = dto.EsMedicoEspecialista,
                        Especialidad = dto.EsMedicoEspecialista ? dto.MedicoEspecialidad : "Médico General",
                        NumeroRegistroIdoneidad = dto.MedicoNumeroRegistroIdoneidad,
                        InstalacionSalud = dto.MedicoInstalacionSalud,
                        NumeroTelefono = dto.MedicoNumeroTelefono,
                        FirmaBase64 = dto.FirmaBase64
                    };

                    _context.Medico.Add(medico);
                    await _context.SaveChangesAsync();
                }

                // 3. Acompañante (si aplica)
                Acompanante? acompanante = null;
                if (dto.RequiereAcompanante)
                {
                    acompanante = await _context.Acompanante
                        .FirstOrDefaultAsync(a => a.PacienteId == paciente.Id);

                    if (acompanante == null)
                    {
                        acompanante = new Acompanante
                        {
                            PacienteId = paciente.Id,
                            NombreCompleto = dto.AcompananteNombreCompleto,
                            TipoDocumento = dto.AcompananteTipoDocumento,
                            NumeroDocumento = dto.AcompananteNumeroDocumento,
                            Nacionalidad = dto.AcompananteNacionalidad,
                            Parentesco = dto.AcompananteParentesco,
                            EsPacienteMenorEdad = dto.EsPacienteMenorEdad,
                            EsPacienteMayorDiscapacidad = dto.EsPacienteMayorDiscapacidad,
                            DiscapacidadDescripcion = dto.DiscapacidadDescripcion,
                            FirmaBase64 = dto.FirmaBase64
                        };

                        _context.Acompanante.Add(acompanante);
                        await _context.SaveChangesAsync();
                    }
                }

                // 4. Crear Solicitud
                var solicitud = new Solicitud
                {
                    PacienteId = paciente.Id,
                    MedicoId = medico.Id,
                    AcompananteId = acompanante?.Id,
                    FechaSolicitud = DateTime.Now,
                    EstadoSolicitudId = 1,
                    MotivoSolicitud = "Solicitud inicial de cannabis medicinal",
                    FirmaBase64 = dto.FirmaBase64
                };

                _context.Solicitud.Add(solicitud);
                await _context.SaveChangesAsync();

                // 5. Crear Tratamiento
                var tratamiento = new Tratamiento
                {
                    SolicitudId = solicitud.Id,
                    FormaFarmaceuticaId = dto.FormaFarmaceuticaId,
                    OtraFormaFarmaceuticaDescripcion = dto.OtraFormaFarmaceuticaDescripcion,
                    ViaAdministracionId = dto.ViaAdministracionId,
                    OtraViaAdministracionDescripcion = dto.OtraViaAdministracionDescripcion,
                    Dosis = dto.Dosis,
                    FrecuenciaAdministracionId = dto.FrecuenciaAdministracionId,
                    DuracionMeses = dto.DuracionMeses,
                    DuracionDiasExtra = dto.DuracionDiasExtra,
                    ConcentracionCbd = dto.ConcentracionCBD,
                    ConcentracionThc = dto.ConcentracionTHC,
                    OtrosCannabinoides = string.Join("; ",
                        (dto.UsaCBD ? $"CBD: {dto.ConcentracionCBD} {dto.OtraUnidadCBDDescripcion}" : null),
                        (dto.UsaTHC ? $"THC: {dto.ConcentracionTHC} {dto.OtraUnidadTHCDescripcion}" : null),
                        (dto.UsaOtroCannabinoide ? dto.OtroCannabinoideEspecifique : null)
                    ).Trim(';', ' ')
                };

                _context.Tratamiento.Add(tratamiento);
                await _context.SaveChangesAsync();

                // 6. Diagnósticos principales
                foreach (var diagId in dto.DiagnosticoIds)
                {
                    await _context.SolicitudDiagnostico.AddAsync(new SolicitudDiagnostico
                    {
                        SolicitudId = solicitud.Id,
                        DiagnosticoId = diagId,
                        EsPrimario = true
                    });
                }

                // 7. Comorbilidades
                foreach (var comorb in dto.Comorbilidades)
                {
                    if (comorb.DiagnosticoId.HasValue)
                    {
                        await _context.SolicitudDiagnostico.AddAsync(new SolicitudDiagnostico
                        {
                            SolicitudId = solicitud.Id,
                            DiagnosticoId = comorb.DiagnosticoId.Value,
                            EsPrimario = false,
                            Observaciones = comorb.TratamientoRecibido
                        });

                        var existe = await _context.PacienteDiagnostico
                            .AnyAsync(pd => pd.PacienteId == paciente.Id && pd.DiagnosticoId == comorb.DiagnosticoId);

                        if (!existe)
                        {
                            await _context.PacienteDiagnostico.AddAsync(new PacienteDiagnostico
                            {
                                PacienteId = paciente.Id,
                                DiagnosticoId = comorb.DiagnosticoId.Value,
                                Observaciones = comorb.TratamientoRecibido
                            });
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return solicitud.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}