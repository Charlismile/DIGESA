using DIGESA.DTOs;

namespace DIGESA.Repositorios.Interfaces;

public interface ISolicitudService
{
    Task<List<RegistroDto>> ObtenerInstalacionesSaludAsync();
    
    Task<List<RegistroDto>> ObtenerRegionesAsync();
    Task<int> CrearSolicitudAsync(RegistroDto dto);
    Task<List<RegistroDto>> ObtenerProvinciasAsync();
    Task<List<RegistroDto>> ObtenerDistritosPorProvinciaAsync(int provinciaId);
    Task<List<RegistroDto>> ObtenerCorregimientosPorDistritoAsync(int distritoId);
}