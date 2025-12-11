using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.InterfacesCannabis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DIGESA.Repositorios.ServiciosCannabis
{
    public class ServicioMedicos : IServicioMedicos
    {
        private readonly DbContextDigesa _context;
        private readonly ILogger<ServicioMedicos> _logger;

        public ServicioMedicos(DbContextDigesa context, ILogger<ServicioMedicos> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<MedicoViewModel> CrearMedico(MedicoViewModel medico, string usuarioId)
        {
            try
            {
                var entidad = new TbMedico
                {
                    CodigoMedico = medico.CodigoMedico,
                    PrimerNombre = medico.PrimerNombre,
                    SegundoNombre = medico.SegundoNombre,
                    PrimerApellido = medico.PrimerApellido,
                    SegundoApellido = medico.SegundoApellido,
                    TipoDocumento = medico.TipoDocumento,
                    NumeroDocumento = medico.NumeroDocumento,
                    Especialidad = medico.Especialidad,
                    Subespecialidad = medico.Subespecialidad,
                    NumeroColegiatura = medico.NumeroColegiatura,
                    TelefonoConsultorio = medico.TelefonoConsultorio,
                    TelefonoMovil = medico.TelefonoMovil,
                    Email = medico.Email,
                    DireccionConsultorio = medico.DireccionConsultorio,
                    ProvinciaId = medico.ProvinciaId,
                    DistritoId = medico.DistritoId,
                    RegionSaludId = medico.RegionSaludId,
                    InstalacionSaludId = medico.InstalacionSaludId,
                    InstalacionPersonalizada = medico.InstalacionPersonalizada,
                    FechaRegistro = DateTime.Now,
                    UsuarioRegistro = usuarioId,
                    Activo = true,
                    Verificado = false
                };

                _context.TbMedico.Add(entidad);
                await _context.SaveChangesAsync();

                medico.Id = entidad.Id;
                return medico;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando médico");
                throw;
            }
        }

        // Implementar otros métodos según la interfaz...
        public Task<MedicoViewModel> ActualizarMedico(int medicoId, MedicoViewModel medico, string usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminarMedico(int medicoId, string usuarioId, string motivo)
        {
            throw new NotImplementedException();
        }

        public Task<MedicoViewModel> ObtenerMedicoPorId(int medicoId)
        {
            throw new NotImplementedException();
        }

        public Task<MedicoViewModel> ObtenerMedicoPorCodigo(string codigoMedico)
        {
            throw new NotImplementedException();
        }

        public Task<MedicoViewModel> ObtenerMedicoPorDocumento(string tipoDocumento, string numeroDocumento)
        {
            throw new NotImplementedException();
        }

        public Task<List<MedicoViewModel>> BuscarMedicos(string criterio, bool soloActivos = true)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerificarMedico(int medicoId, string usuarioVerificador, string observaciones)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RevocarVerificacion(int medicoId, string usuario, string motivo)
        {
            throw new NotImplementedException();
        }

        public Task<ReporteMedicosViewModel> GenerarReporteMedicos(DateTime? fechaInicio, DateTime? fechaFin)
        {
            throw new NotImplementedException();
        }

        public Task<EstadisticasMedicosViewModel> ObtenerEstadisticasMedicos()
        {
            throw new NotImplementedException();
        }

        public Task<List<AuditoriaMedicoViewModel>> ObtenerAuditoriaMedico(int medicoId)
        {
            throw new NotImplementedException();
        }

        public Task<List<AuditoriaMedicoViewModel>> ObtenerAuditoriaPorUsuario(string usuarioId)
        {
            throw new NotImplementedException();
        }
    }
}