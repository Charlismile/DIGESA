using DIGESA.Data;
using DIGESA.Models.Entities.DIGESA;
using DIGESA.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIGESA.Services
{
    public class FormService : IFormService
    {
        private readonly DbContextDigesa _context;

        public FormService(DbContextDigesa context)
        {
            _context = context;
        }

        /// <summary>
        /// Devuelve la lista de diagnósticos existentes, ordenados por Nombre.
        /// </summary>
        public async Task<List<Diagnostico>> ObtenerDiagnosticosAsync()
        {
            return await _context.Diagnosticos
                .OrderBy(d => d.Nombre)
                .ToListAsync();
        }

        /// <summary>
        /// Devuelve la lista de médicos existentes, ordenados por NombreCompleto.
        /// </summary>
        public async Task<List<Medico>> ObtenerMedicosAsync()
        {
            return await _context.Medicos
                .OrderBy(m => m.NombreCompleto)
                .ToListAsync();
        }

        /// <summary>
        /// Devuelve la lista de pacientes existentes, ordenados por NombreCompleto.
        /// </summary>
        public async Task<List<Paciente>> ObtenerPacientesAsync()
        {
            return await _context.Pacientes
                .OrderBy(p => p.NombreCompleto)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un paciente por Id, incluyendo sus Acompañantes y PacienteDiagnosticos (relación con Diagnostico).
        /// </summary>
        public async Task<Paciente?> ObtenerPacientePorIdAsync(int id)
        {
            return await _context.Pacientes
                .Include(p => p.Acompanantes)
                .Include(p => p.PacienteDiagnosticos)
                    .ThenInclude(pd => pd.Diagnosticos)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Obtiene un médico por su Id.
        /// </summary>
        public async Task<Medico?> ObtenerMedicoPorIdAsync(int id)
        {
            return await _context.Medicos
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
