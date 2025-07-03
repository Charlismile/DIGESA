using DIGESA.Models.Entities.DBDIGESA;

public interface ISolicitudRepository
{
    Task AddAsync(Solicitud solicitud);
}