using DIGESA.Data;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class DeclaracionJuradaService : IDeclaracionJuradaService
{
    private readonly DbContextDigesa _context;
    private readonly ILogger<DeclaracionJuradaService> _logger;

    public DeclaracionJuradaService(
        DbContextDigesa context,
        ILogger<DeclaracionJuradaService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ResultModel<DeclaracionJuradaModel>> GenerarDeclaracionAsync(int solicitudId)
    {
        try
        {
            var solicitud = await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .FirstOrDefaultAsync(s => s.Id == solicitudId);

            if (solicitud == null || solicitud.Paciente == null)
                return ResultModel<DeclaracionJuradaModel>.ErrorResult("Solicitud o paciente no encontrado");

            var declaracionText = ObtenerTextoDeclaracionJurada();
            
            var declaracion = new DeclaracionJuradaModel
            {
                SolicitudId = solicitudId,
                Detalle = declaracionText,
                Fecha = DateTime.Now,
                NombreDeclarante = $"{solicitud.Paciente.PrimerNombre} {solicitud.Paciente.PrimerApellido}",
                DocumentoIdentidad = !string.IsNullOrEmpty(solicitud.Paciente.DocumentoCedula)
                    ? solicitud.Paciente.DocumentoCedula
                    : solicitud.Paciente.DocumentoPasaporte ?? string.Empty,
                Aceptada = false
            };

            // Guardar en base de datos
            var declaracionEntity = new TbDeclaracionJurada
            {
                SolRegCannabisId = solicitudId,
                Detalle = declaracionText,
                Fecha = DateOnly.FromDateTime(DateTime.Now),
                NombreDeclarante = declaracion.NombreDeclarante,
                Aceptada = false
            };

            _context.TbDeclaracionJurada.Add(declaracionEntity);
            await _context.SaveChangesAsync();

            declaracion.Id = declaracionEntity.Id;

            return ResultModel<DeclaracionJuradaModel>.SuccessResult(
                declaracion, 
                "Declaración jurada generada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error generando declaración jurada para solicitud {solicitudId}");
            return ResultModel<DeclaracionJuradaModel>.ErrorResult(
                "Error al generar la declaración jurada", 
                new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<bool>> AceptarDeclaracionAsync(int declaracionId, string ipAceptacion = null)
    {
        try
        {
            var declaracion = await _context.TbDeclaracionJurada
                .FirstOrDefaultAsync(d => d.Id == declaracionId);

            if (declaracion == null)
                return ResultModel<bool>.ErrorResult("Declaración jurada no encontrada");

            if (declaracion.Aceptada == true)
                return ResultModel<bool>.ErrorResult("La declaración ya fue aceptada anteriormente");

            declaracion.Aceptada = true;
            declaracion.Fecha = DateOnly.FromDateTime(DateTime.Now);

            _context.TbDeclaracionJurada.Update(declaracion);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Declaración jurada {declaracionId} aceptada desde IP: {ipAceptacion}");
            return ResultModel<bool>.SuccessResult(true, "Declaración jurada aceptada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error aceptando declaración jurada {declaracionId}");
            return ResultModel<bool>.ErrorResult("Error al aceptar la declaración jurada");
        }
    }

    public async Task<ResultModel<DeclaracionJuradaModel>> ObtenerDeclaracionPorSolicitudAsync(int solicitudId)
    {
        try
        {
            var declaracion = await _context.TbDeclaracionJurada
                .Include(d => d.SolRegCannabis)
                    .ThenInclude(s => s.Paciente)
                .FirstOrDefaultAsync(d => d.SolRegCannabisId == solicitudId);

            if (declaracion == null)
                return ResultModel<DeclaracionJuradaModel>.ErrorResult("No se encontró declaración jurada para esta solicitud");

            var declaracionModel = new DeclaracionJuradaModel
            {
                Id = declaracion.Id,
                SolicitudId = declaracion.SolRegCannabisId ?? 0,
                Detalle = declaracion.Detalle ?? string.Empty,
                Fecha = declaracion.Fecha?.ToDateTime(TimeOnly.MinValue) ?? DateTime.MinValue,
                NombreDeclarante = declaracion.NombreDeclarante ?? string.Empty,
                Aceptada = declaracion.Aceptada ?? false
            };

            return ResultModel<DeclaracionJuradaModel>.SuccessResult(
                declaracionModel, 
                "Declaración jurada obtenida exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error obteniendo declaración jurada para solicitud {solicitudId}");
            return ResultModel<DeclaracionJuradaModel>.ErrorResult("Error al obtener la declaración jurada");
        }
    }

    public async Task<ResultModel<bool>> VerificarDeclaracionAceptadaAsync(int solicitudId)
    {
        try
        {
            var declaracion = await _context.TbDeclaracionJurada
                .FirstOrDefaultAsync(d => d.SolRegCannabisId == solicitudId && d.Aceptada == true);

            if (declaracion == null)
                return ResultModel<bool>.SuccessResult(false, "Declaración jurada no aceptada");

            return ResultModel<bool>.SuccessResult(true, "Declaración jurada aceptada");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error verificando declaración jurada para solicitud {solicitudId}");
            return ResultModel<bool>.ErrorResult("Error al verificar la declaración jurada");
        }
    }

    private string ObtenerTextoDeclaracionJurada()
    {
        return @"DECLARACIÓN JURADA

Yo, [Nombre Completo], con documento de identidad número [Documento], en mi condición de solicitante de la certificación para uso de cannabis medicinal, declaro bajo juramento lo siguiente:

1. Que toda la información proporcionada en la presente solicitud es veraz y completa.
2. Que comprendo que el cannabis medicinal debe ser utilizado únicamente con fines terapéuticos y bajo supervisión médica.
3. Que me comprometo a utilizar el producto exclusivamente para el tratamiento de la condición médica para la cual ha sido prescrito.
4. Que reconozco que el cannabis medicinal puede tener efectos secundarios y me comprometo a reportar cualquier efecto adverso a mi médico tratante.
5. Que entiendo que está prohibido conducir u operar maquinaria bajo los efectos incapacitantes del cannabis medicinal.
6. Que me comprometo a no compartir, vender o distribuir el cannabis medicinal a terceros.
7. Que acepto que el incumplimiento de estas condiciones puede resultar en la revocación de mi certificación.

DECLARACIÓN DE RESPONSABILIDAD:
El Ministerio de Salud no se hace responsable del uso que el paciente le dé al producto medicinal de cannabis ni de los efectos que estos puedan provocar. La responsabilidad del uso adecuado recae exclusivamente en el paciente y su médico tratante.

Fecha: " + DateTime.Now.ToString("dd/MM/yyyy") + @"

Firma del Declarante: ___________________________";
    }
}