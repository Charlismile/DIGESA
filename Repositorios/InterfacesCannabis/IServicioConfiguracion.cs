using DIGESA.Models.CannabisModels;
using DIGESA.Models.CannabisModels.Configuracion;

namespace DIGESA.Repositorios.InterfacesCannabis;

public interface IServicioConfiguracion
{
    Task<ConfiguracionCompletaViewModel> ObtenerConfiguracionCompleta();
    Task<ConfiguracionSistemaViewModel> ObtenerConfiguracion(string clave);
    Task<bool> GuardarConfiguracion(ConfiguracionCompletaViewModel configuracion);
    Task<int> ObtenerValorEntero(string clave, int valorDefecto = 0);
    Task<bool> ObtenerValorBooleano(string clave, bool valorDefecto = false);
    Task<string> ObtenerValorString(string clave, string valorDefecto = "");
}