using Microsoft.AspNetCore.Mvc;
using DIGESA.Data;
using DIGESA.Models.Entities.DBDIGESA;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly DbContextDigesa _context;

        public PacienteController(DbContextDigesa context)
        {
            _context = context;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] PacienteRegistroDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Guardar paciente
                var paciente = new Paciente
                {
                    NombreCompleto = dto.NombreCompleto,
                    TipoDocumento = dto.TipoDocumento,
                    NumeroDocumento = dto.NumeroDocumento,
                    Nacionalidad = dto.Nacionalidad,
                    FechaNacimiento = dto.FechaNacimiento.Value,
                    Sexo = dto.Sexo,
                    DireccionResidencia = dto.DireccionResidencia,
                    TelefonoResidencial = dto.TelefonoResidencial,
                    TelefonoPersonal = dto.TelefonoPersonal,
                    TelefonoLaboral = dto.TelefonoLaboral,
                    CorreoElectronico = dto.CorreoElectronico,
                    InstalacionSalud = dto.InstalacionSalud,
                    RegionSalud = dto.RegionSalud,
                    RequiereAcompanante = dto.RequiereAcompanante,
                    MotivoRequerimientoAcompanante = dto.MotivoRequerimientoAcompanante,
                    TipoDiscapacidad = dto.TipoDiscapacidad
                };

                await _context.Paciente.AddAsync(paciente);
                await _context.SaveChangesAsync();

                // Guardar médico
                var medico = new Medico
                {
                    NombreCompleto = dto.Medico.NombreCompleto,
                    Especialidad = dto.Medico.Especialidad,
                    NumeroRegistroIdoneidad = dto.Medico.RegistroIdoneidad,
                    InstalacionSalud = dto.Medico.InstalacionSalud,
                    NumeroTelefono = dto.Medico.NumeroTelefono,
                    // EsMedicoEspecialista = dto.Medico.EsEspecialista
                };

                await _context.Medico.AddAsync(medico);
                await _context.SaveChangesAsync();

                // Guardar solicitud
                var solicitud = new Solicitud
                {
                    PacienteId = paciente.Id,
                    MedicoId = medico.Id
                };

                await _context.Solicitud.AddAsync(solicitud);
                await _context.SaveChangesAsync();

                // Guardar diagnósticos
                foreach (var diag in dto.Diagnosticos)
                {
                    var diagnostico = await _context.Diagnostico
                        .FirstOrDefaultAsync(d => d.Nombre == diag.Nombre || d.CodigoCie10 == diag.CodigoCIE10);

                    if (diagnostico == null)
                    {
                        diagnostico = new Diagnostico
                        {
                            Nombre = diag.Nombre,
                            CodigoCie10 = diag.CodigoCIE10,
                            Descripcion = diag.Observaciones ?? $"Diagnóstico creado automáticamente - {diag.Nombre}",
                            EsOtro = string.IsNullOrEmpty(diag.CodigoCIE10)
                        };
                        await _context.Diagnostico.AddAsync(diagnostico);
                        await _context.SaveChangesAsync();
                    }

                    await _context.SolicitudDiagnostico.AddAsync(new SolicitudDiagnostico
                    {
                        SolicitudId = solicitud.Id,
                        DiagnosticoId = diagnostico.Id,
                        EsPrimario = dto.Diagnosticos.First().CodigoCIE10 == diag.CodigoCIE10
                    });
                }

                // Guardar tratamiento
                var tratamiento = new Tratamiento
                {
                    SolicitudId = solicitud.Id,
                    ConcentracionCbd = dto.Tratamiento.ConcentracionCBD,
                    ConcentracionThc = dto.Tratamiento.ConcentracionTHC,
                    OtrosCannabinoides = dto.Tratamiento.OtrosCannabinoides,
                    Dosis = dto.Tratamiento.Dosis,
                    FrecuenciaAdministracion = dto.Tratamiento.FrecuenciaAdministracion,
                    DuracionTratamientoDias = dto.Tratamiento.DuracionTratamientoDias,
                    CantidadPrescrita = dto.Tratamiento.CantidadPrescrita,
                    InstruccionesAdicionales = dto.Tratamiento.InstruccionesAdicionales,

                    FormaFarmaceuticaId = dto.Tratamiento.FormaFarmaceuticaId,
                    ViaAdministracionId = dto.Tratamiento.ViaAdministracionId,
                    FrecuenciaAdministracionId = dto.Tratamiento.FrecuenciaAdministracionId,
                    UnidadCbdid = dto.Tratamiento.UnidadCBDId,
                    UnidadThcid = dto.Tratamiento.UnidadTHCId,
                    UnidadOtroCannabinoide1Id = dto.Tratamiento.UnidadOtroCannabinoide1Id,
                    UnidadOtroCannabinoide2Id = dto.Tratamiento.UnidadOtroCannabinoide2Id,
                    TipoProductoId = dto.Tratamiento.TipoProductoId
                };

                await _context.Tratamiento.AddAsync(tratamiento);
                await _context.SaveChangesAsync();

                // Guardar acompañante (si es requerido)
                if (dto.RequiereAcompanante && dto.Acompanante != null)
                {
                    var acompanante = new Acompanante
                    {
                        PacienteId = paciente.Id,
                        NombreCompleto = dto.Acompanante.NombreCompleto,
                        TipoDocumento = dto.Acompanante.TipoDocumento,
                        NumeroDocumento = dto.Acompanante.NumeroDocumento,
                        Nacionalidad = dto.Acompanante.Nacionalidad,
                        Parentesco = dto.Acompanante.Parentesco,
                        EsPacienteMenorEdad = dto.Acompanante.EsPacienteMenorEdad,
                        EsPacienteMayorDiscapacidad = dto.Acompanante.EsPacienteMayorDiscapacidad
                    };

                    await _context.Acompanante.AddAsync(acompanante);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return Ok(new
                {
                    Id = paciente.Id,
                    Mensaje = "Paciente y solicitud registrados con éxito"
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Error = "Error interno", Detalles = ex.Message });
            }
        }
    }
}