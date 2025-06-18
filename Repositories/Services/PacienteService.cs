using DIGESA.Models.DTOs;
using DIGESA.Models.Entities.DBDIGESA;
using Microsoft.EntityFrameworkCore;

public class PacienteService : IPacienteService
{
    private readonly DbContextDigesa _context;

    public PacienteService(DbContextDigesa context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Paciente>> GetAllAsync()
    {
        return await _context.Paciente.ToListAsync();
    }

    public async Task<Paciente?> GetByIdAsync(int id)
    {
        return await _context.Paciente.FindAsync(id);
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

            // ✅ Ahora sí puedes usar estas propiedades
            MotivoRequerimientoAcompanante = model.MotivoRequerimientoAcompanante,
            TipoDiscapacidad = model.TipoDiscapacidad
        };

        await _context.Paciente.AddAsync(paciente);
        await _context.SaveChangesAsync();

        if (model.RequiereAcompanante && model.Acompanante != null &&
            !string.IsNullOrEmpty(model.Acompanante.NombreCompleto))
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

    public async Task UpdateAsync(Paciente paciente)
    {
        _context.Paciente.Update(paciente);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var paciente = await _context.Paciente.FindAsync(id);
        if (paciente != null)
        {
            _context.Paciente.Remove(paciente);
            await _context.SaveChangesAsync();
        }
    }
}