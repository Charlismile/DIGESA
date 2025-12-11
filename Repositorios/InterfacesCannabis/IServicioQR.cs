using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.InterfacesCannabis;

public interface IServicioQR
{
    Task<CodigoQRViewModel> GenerarCodigoQR(int solicitudId, string usuarioId);
    Task<bool> ValidarCodigoQR(string codigoQR, string ipOrigen = null);
    Task<CodigoQRViewModel> EscanearCodigoQR(string codigoQR, string escaneadoPor, string ipOrigen = null);
    Task<bool> InactivarCodigoQR(int codigoQRId, string usuarioId, string motivo);
    Task<List<CodigoQRViewModel>> ObtenerCodigosQRPorSolicitud(int solicitudId);
    Task<CodigoQRViewModel> ObtenerCodigoQRActivo(int solicitudId);
}