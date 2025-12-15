using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.InterfacesCannabis;

public interface IServicioQr
{
    Task<CodigoQrViewModel> GenerarCodigoQR(int solicitudId, string usuarioId);
    Task<bool> ValidarCodigoQR(string codigoQR, string ipOrigen = null);
    Task<CodigoQrViewModel> EscanearCodigoQR(string codigoQR, string escaneadoPor, string ipOrigen = null);
    Task<bool> InactivarCodigoQR(int codigoQRId, string usuarioId, string motivo);
    Task<List<CodigoQrViewModel>> ObtenerCodigosQRPorSolicitud(int solicitudId);
    Task<CodigoQrViewModel> ObtenerCodigoQRActivo(int solicitudId);
}


