using DIGESA.Models.Entities.DBDIGESA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador,Médico")] // Solo accesible para estos roles
public class PruebaDbController : ControllerBase
{
    private readonly DbContextDigesa _context;

    public PruebaDbController(DbContextDigesa context)
    {
        _context = context;
    }

    // GET: api/pruebadb
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Paciente>>> GetPacientes()
    {
        var pacientes = await _context.Paciente
            .Select(p => new Paciente
            {
                Id = p.Id,
                NombreCompleto = p.NombreCompleto,
                TipoDocumento = p.TipoDocumento,
                NumeroDocumento = p.NumeroDocumento
            })
            .ToListAsync();

        return Ok(pacientes);
    }

    // GET: api/pruebadb/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Paciente>> GetPacientePorId(int id)
    {
        var paciente = await _context.Paciente
            .Where(p => p.Id == id)
            .Select(p => new Paciente
            {
                Id = p.Id,
                NombreCompleto = p.NombreCompleto,
                TipoDocumento = p.TipoDocumento,
                NumeroDocumento = p.NumeroDocumento,
                Nacionalidad = p.Nacionalidad,
                Sexo = p.Sexo
            })
            .FirstOrDefaultAsync();

        if (paciente == null)
        {
            return NotFound($"No se encontró ningún paciente con ID {id}");
        }

        return Ok(paciente);
    }
}