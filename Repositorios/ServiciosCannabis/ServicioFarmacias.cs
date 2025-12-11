using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.InterfacesCannabis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DIGESA.Repositorios.ServiciosCannabis
{
    public class ServicioFarmacias : IServicioFarmacias
    {
        private readonly DbContextDigesa _context;
        private readonly ILogger<ServicioFarmacias> _logger;

        public ServicioFarmacias(DbContextDigesa context, ILogger<ServicioFarmacias> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<FarmaciaAutorizadaViewModel> CrearFarmacia(FarmaciaAutorizadaViewModel farmacia, string usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ActualizarFarmacia(int farmaciaId, FarmaciaAutorizadaViewModel farmacia, string usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InactivarFarmacia(int farmaciaId, string usuarioId, string motivo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidarCarnetEnFarmacia(int farmaciaId, string codigoQR)
        {
            throw new NotImplementedException();
        }

        public Task<RegistroDispensacionViewModel> RegistrarDispensacion(int farmaciaId, string codigoQR, RegistroDispensacionViewModel dispensacion, string usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<List<FarmaciaAutorizadaViewModel>> ObtenerFarmaciasPorProvincia(int provinciaId)
        {
            throw new NotImplementedException();
        }

        public Task<List<FarmaciaAutorizadaViewModel>> ObtenerFarmaciasActivas()
        {
            throw new NotImplementedException();
        }

        public Task<FarmaciaAutorizadaViewModel> ObtenerFarmaciaPorCodigo(string codigoFarmacia)
        {
            throw new NotImplementedException();
        }

        public Task<ReporteDispensacionViewModel> GenerarReporteDispensacion(int farmaciaId, DateTime fechaInicio, DateTime fechaFin)
        {
            throw new NotImplementedException();
        }
    }
}