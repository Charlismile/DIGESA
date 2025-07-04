using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositories.Interfaces
{
    public interface ISolicitudDiagnosticoRepository : IRepository<SolicitudDiagnostico>
    {
        Task<List<SolicitudDiagnostico>> GetBySolicitudAsync(int solicitudId);
    }
}