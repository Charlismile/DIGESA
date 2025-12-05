using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface ICarnetService
{
    Task<ResultModel<string>> GenerarNumeroCarnetAsync(int pacienteId, bool esAcompanante = false);
    Task<ResultModel<byte[]>> GenerarCarnetPDFAsync(CarnetModel carnet, bool incluirAcompanante = false);
    Task<ResultModel<bool>> AsignarCarnetSolicitudAsync(int solicitudId, string numeroCarnet, bool esAcompanante = false);
    Task<ResultModel<bool>> VerificarCarnetValidoAsync(string numeroCarnet);
}