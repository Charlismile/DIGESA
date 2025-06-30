using DIGESA.Models.DTOs;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositories.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly DbContextDigesa _context;

        public PacienteService(DbContextDigesa context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(PacienteRegistroDTO model)
        {
            // Registrar al paciente
            var paciente = new Paciente
            {
                PacienteNombreCompleto = model.NombreCompleto,
                PacienteTipoDocumento = model.TipoDocumento,
                PacienteNumeroDocumento = model.NumeroDocumento,
                PacienteNacionalidad = model.Nacionalidad,
                PacienteFechaNacimiento = model.FechaNacimiento,
                PacienteSexo = model.Sexo,
                PacienteDireccionResidencia = model.DireccionResidencia,
                PacienteTelefonoResidencial = model.TelefonoResidencial,
                PacienteTelefonoPersonal = model.TelefonoPersonal,
                PacienteTelefonoLaboral = model.TelefonoLaboral,
                PacienteCorreoElectronico = model.CorreoElectronico,
                PacienteInstalacionSalud = model.InstalacionSalud,
                PacienteRegionSalud = model.RegionSalud,
                PacienteRequiereAcompanante = model.RequiereAcompanante,
                PacienteMotivoRequerimientoAcompanante = model.MotivoRequerimientoAcompanante,
                PacienteTipoDiscapacidad = model.TipoDiscapacidad
            };

            await _context.Paciente.AddAsync(paciente);
            await _context.SaveChangesAsync();

            // Si requiere acompañante, registrar también
            if (model.RequiereAcompanante && model.Acompanante != null &&
                !string.IsNullOrEmpty(model.Acompanante.NombreCompleto))
            {
                var acompanante = new Acompanante
                {
                    AcompanantePacienteId = paciente.PacienteId,
                    AcompananteNombreCompleto = model.Acompanante.NombreCompleto,
                    AcompananteTipoDocumento = model.Acompanante.TipoDocumento,
                    AcompananteNumeroDocumento = model.Acompanante.NumeroDocumento,
                    AcompananteNacionalidad = model.Acompanante.Nacionalidad,
                    AcompananteParentesco = model.Acompanante.Parentesco
                };

                await _context.Acompanante.AddAsync(acompanante);
                await _context.SaveChangesAsync();
            }

            return paciente.PacienteId;
        }

        public async Task<Paciente?> GetByIdAsync(int id)
        {
            return await _context.Paciente.FindAsync(id);
        }

        public async Task<IEnumerable<Paciente>> GetAllAsync()
        {
            return await _context.Paciente.ToListAsync();
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
}