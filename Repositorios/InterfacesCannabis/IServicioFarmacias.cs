using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.InterfacesCannabis;

public interface IServicioFarmacias
{
    Task<FarmaciaAutorizadaViewModel> CrearFarmacia(FarmaciaAutorizadaViewModel farmacia, string usuarioId);
    Task<bool> ActualizarFarmacia(int farmaciaId, FarmaciaAutorizadaViewModel farmacia, string usuarioId);
    Task<bool> InactivarFarmacia(int farmaciaId, string usuarioId, string motivo);
    Task<bool> ValidarCarnetEnFarmacia(int farmaciaId, string codigoQR);
    Task<RegistroDispensacionViewModel> RegistrarDispensacion(int farmaciaId, string codigoQR, RegistroDispensacionViewModel dispensacion, string usuarioId);
    Task<List<FarmaciaAutorizadaViewModel>> ObtenerFarmaciasPorProvincia(int provinciaId);
    Task<List<FarmaciaAutorizadaViewModel>> ObtenerFarmaciasActivas();
    Task<FarmaciaAutorizadaViewModel> ObtenerFarmaciaPorCodigo(string codigoFarmacia);
    Task<ReporteDispensacionViewModel> GenerarReporteDispensacion(int farmaciaId, DateTime fechaInicio, DateTime fechaFin);
}