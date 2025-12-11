using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.InterfacesCannabis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DIGESA.Repositorios.ServiciosCannabis
{
    public class ServicioQR : IServicioQR
    {
        private readonly DbContextDigesa _context;
        private readonly ILogger<ServicioQR> _logger;

        public ServicioQR(DbContextDigesa context, ILogger<ServicioQR> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<CodigoQRViewModel> GenerarCodigoQR(int solicitudId, string usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidarCodigoQR(string codigoQR, string ipOrigen = null)
        {
            throw new NotImplementedException();
        }

        public Task<CodigoQRViewModel> EscanearCodigoQR(string codigoQR, string escaneadoPor, string ipOrigen = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InactivarCodigoQR(int codigoQRId, string usuarioId, string motivo)
        {
            throw new NotImplementedException();
        }

        public Task<List<CodigoQRViewModel>> ObtenerCodigosQRPorSolicitud(int solicitudId)
        {
            throw new NotImplementedException();
        }

        public Task<CodigoQRViewModel> ObtenerCodigoQRActivo(int solicitudId)
        {
            throw new NotImplementedException();
        }
    }
}