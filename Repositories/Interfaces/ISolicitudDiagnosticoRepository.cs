using DIGESA.Models.Entities.DBDIGESA;

public interface ISolicitudDiagnosticoRepository
{
    Task AddAsync(SolicitudDiagnostico solicitudDiagnostico);
}