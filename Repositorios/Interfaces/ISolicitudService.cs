using DIGESA.DTOs;

namespace DIGESA.Repositorios.Interfaces;

public interface ISolicitudService
{
        Task<List<UbicacionDto>> ObtenerInstalacionesSaludAsync();
    
        Task<int> CrearSolicitudAsync(RegistroDto dto);
}
