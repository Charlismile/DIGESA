using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositories.Interfaces
{
    public interface ISolicitudRepository : IRepository<Solicitud>
    {
        Task<Solicitud?> GetByPacienteIdAsync(int pacienteId);
    }
}