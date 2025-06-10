using DIGESA.Models.DTOs;
using DIGESA.Models.Entities.DBDIGESA;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly DbContextDigesa _context;

    public PacientesController(DbContextDigesa context)
    {
        _context = context;
    }

    // GET: api/pacientes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PacienteDTO>>> GetPacientes()
    {
        var pacientes = await _context.Pacientes
            .Select(p => new PacienteDTO
            {
                Id = p.Id,
                NombreCompleto = p.NombreCompleto,
                TipoDocumento = p.TipoDocumento,
                NumeroDocumento = p.NumeroDocumento,
                Nacionalidad = p.Nacionalidad,
                FechaNacimiento = p.FechaNacimiento,
                Sexo = p.Sexo,
                DireccionResidencia = p.DireccionResidencia,
                TelefonoResidencial = p.TelefonoResidencial,
                TelefonoPersonal = p.TelefonoPersonal,
                TelefonoLaboral = p.TelefonoLaboral,
                CorreoElectronico = p.CorreoElectronico,
                InstalacionSalud = p.InstalacionSalud,
                RegionSalud = p.RegionSalud,
                RequiereAcompanante = p.RequiereAcompanante,
                MotivoRequerimientoAcompanante = p.MotivoRequerimientoAcompanante,
                TipoDiscapacidad = p.TipoDiscapacidad
            })
            .ToListAsync();

        return Ok(pacientes);
    }

    // GET: api/pacientes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PacienteDTO>> GetPaciente(int id)
    {
        var paciente = await _context.Pacientes
            .Where(p => p.Id == id)
            .Select(p => new PacienteDTO
            {
                Id = p.Id,
                NombreCompleto = p.NombreCompleto,
                TipoDocumento = p.TipoDocumento,
                NumeroDocumento = p.NumeroDocumento,
                Nacionalidad = p.Nacionalidad,
                FechaNacimiento = p.FechaNacimiento,
                Sexo = p.Sexo,
                DireccionResidencia = p.DireccionResidencia,
                TelefonoResidencial = p.TelefonoResidencial,
                TelefonoPersonal = p.TelefonoPersonal,
                TelefonoLaboral = p.TelefonoLaboral,
                CorreoElectronico = p.CorreoElectronico,
                InstalacionSalud = p.InstalacionSalud,
                RegionSalud = p.RegionSalud,
                RequiereAcompanante = p.RequiereAcompanante,
                MotivoRequerimientoAcompanante = p.MotivoRequerimientoAcompanante,
                TipoDiscapacidad = p.TipoDiscapacidad
            })
            .FirstOrDefaultAsync();

        if (paciente == null)
        {
            return NotFound();
        }

        return Ok(paciente);
    }
    // POST: api/pacientes
        [HttpPost]
        public async Task<ActionResult<PacienteDTO>> CreatePaciente(PacienteDTO dto)
        {
            var paciente = new Paciente
            {
                NombreCompleto = dto.NombreCompleto,
                TipoDocumento = dto.TipoDocumento,
                NumeroDocumento = dto.NumeroDocumento,
                Nacionalidad = dto.Nacionalidad,
                FechaNacimiento = dto.FechaNacimiento,
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

            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();

            var createdDto = new PacienteDTO
            {
                Id = paciente.Id,
                NombreCompleto = paciente.NombreCompleto,
                TipoDocumento = paciente.TipoDocumento,
                NumeroDocumento = paciente.NumeroDocumento,
                Nacionalidad = paciente.Nacionalidad,
                FechaNacimiento = paciente.FechaNacimiento,
                Sexo = paciente.Sexo,
                DireccionResidencia = paciente.DireccionResidencia,
                TelefonoResidencial = paciente.TelefonoResidencial,
                TelefonoPersonal = paciente.TelefonoPersonal,
                TelefonoLaboral = paciente.TelefonoLaboral,
                CorreoElectronico = paciente.CorreoElectronico,
                InstalacionSalud = paciente.InstalacionSalud,
                RegionSalud = paciente.RegionSalud,
                RequiereAcompanante = paciente.RequiereAcompanante,
                MotivoRequerimientoAcompanante = paciente.MotivoRequerimientoAcompanante,
                TipoDiscapacidad = paciente.TipoDiscapacidad
            };

            return CreatedAtAction(nameof(GetPaciente), new { id = paciente.Id }, createdDto);
        }
// PUT: api/pacientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaciente(int id, PacienteDTO dto)
        {
            var paciente = await _context.Pacientes.FindAsync(id);

            if (paciente == null)
            {
                return NotFound();
            }

            paciente.NombreCompleto = dto.NombreCompleto;
            paciente.TipoDocumento = dto.TipoDocumento;
            paciente.NumeroDocumento = dto.NumeroDocumento;
            paciente.Nacionalidad = dto.Nacionalidad;
            paciente.FechaNacimiento = dto.FechaNacimiento;
            paciente.Sexo = dto.Sexo;
            paciente.DireccionResidencia = dto.DireccionResidencia;
            paciente.TelefonoResidencial = dto.TelefonoResidencial;
            paciente.TelefonoPersonal = dto.TelefonoPersonal;
            paciente.TelefonoLaboral = dto.TelefonoLaboral;
            paciente.CorreoElectronico = dto.CorreoElectronico;
            paciente.InstalacionSalud = dto.InstalacionSalud;
            paciente.RegionSalud = dto.RegionSalud;
            paciente.RequiereAcompanante = dto.RequiereAcompanante;
            paciente.MotivoRequerimientoAcompanante = dto.MotivoRequerimientoAcompanante;
            paciente.TipoDiscapacidad = dto.TipoDiscapacidad;

            _context.Pacientes.Update(paciente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/pacientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaciente(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }

            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("seguimiento/{documento}")]
        public async Task<ActionResult<PacienteSeguimientoDTO>> GetSeguimiento(string documento)
        {
            var paciente = await _context.Pacientes
                .Where(p => p.NumeroDocumento == documento)
                .Select(p => new PacienteSeguimientoDTO
                {
                    Id = p.Id,
                    NombreCompleto = p.NombreCompleto,
                    NumeroDocumento = p.NumeroDocumento,
                    FechaRegistro = p.FechaRegistro,
                    EstadoSolicitud = p.EstadoSolicitud ?? "Pendiente",
                    Aprobado = p.EstadoSolicitud == "Aprobado"
                })
                .FirstOrDefaultAsync();

            if (paciente == null)
                return NotFound("Solicitud no encontrada");

            return Ok(paciente);
        }
}

   