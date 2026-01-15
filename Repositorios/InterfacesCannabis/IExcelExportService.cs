using DIGESA.Data;
using DIGESA.Models.CannabisModels.Listados;

namespace DIGESA.Repositorios.InterfacesCannabis;

public interface IExcelExportService
{
    Task<byte[]> ExportSolicitudesAsync(
        List<PacienteListadoViewModel> data,
        string? estado,
        string? documento);

    Task<byte[]> ExportUsuariosAsync(List<ApplicationUser> usuarios);
}
