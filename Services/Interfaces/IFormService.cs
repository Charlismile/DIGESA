using DIGESA.Models.Entities.DIGESA;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIGESA.Services.Interfaces
{
    public interface IFormService
    {
        /// <summary>
        /// Obtiene todos los diagnósticos ordenados por Nombre.
        /// </summary>
        Task<List<Diagnostico>> ObtenerDiagnosticosAsync();

        /// <summary>
        /// Obtiene todos los médicos ordenados por NombreCompleto.
        /// </summary>
        Task<List<Medico>> ObtenerMedicosAsync();

        /// <summary>
        /// Obtiene todos los pacientes ordenados por NombreCompleto.
        /// </summary>
        Task<List<Paciente>> ObtenerPacientesAsync();

        /// <summary>
        /// Obtiene un paciente completo (incluyendo Acompañantes y PacienteDiagnosticos).
        /// </summary>
        Task<Paciente?> ObtenerPacientePorIdAsync(int id);

        /// <summary>
        /// Obtiene un médico por su Id.
        /// </summary>
        Task<Medico?> ObtenerMedicoPorIdAsync(int id);
    }
}