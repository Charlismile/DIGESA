using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CarnetController : ControllerBase
{
    private readonly ICarnetService _carnetService;
    private readonly IRenovacionService _renovacionService;
    private readonly DbContextDigesa _context;
    private readonly ILogger<CarnetController> _logger;

    public CarnetController(
        ICarnetService carnetService,
        IRenovacionService renovacionService,
        DbContextDigesa context,
        ILogger<CarnetController> logger)
    {
        _carnetService = carnetService;
        _renovacionService = renovacionService;
        _context = context;
        _logger = logger;
    }

    [HttpPost("generar/{solicitudId}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GenerarCarnet(int solicitudId, [FromQuery] bool incluirAcompanante = false)
    {
        try
        {
            var solicitud = await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .FirstOrDefaultAsync(s => s.Id == solicitudId);

            if (solicitud == null || solicitud.Paciente == null)
                return NotFound(new { success = false, message = "Solicitud no encontrada" });

            // Generar número de carnet
            var numeroCarnetResult = await _carnetService.GenerarNumeroCarnetAsync(
                solicitud.PacienteId.Value, 
                incluirAcompanante);

            if (!numeroCarnetResult.Success)
                return BadRequest(new { success = false, message = numeroCarnetResult.Message });

            // Asignar carnet a la solicitud
            var asignacionResult = await _carnetService.AsignarCarnetSolicitudAsync(
                solicitudId, 
                numeroCarnetResult.Data, 
                incluirAcompanante);

            if (!asignacionResult.Success)
                return BadRequest(new { success = false, message = asignacionResult.Message });

            // Generar PDF del carnet
            var carnetModel = new CarnetModel
            {
                NumeroCarnet = numeroCarnetResult.Data,
                NombreCompleto = $"{solicitud.Paciente.PrimerNombre} {solicitud.Paciente.PrimerApellido}",
                Documento = solicitud.Paciente.DocumentoCedula ?? solicitud.Paciente.DocumentoPasaporte ?? "N/A",
                Direccion = solicitud.Paciente.DireccionExacta ?? "N/A",
                FechaEmision = DateTime.Now,
                FechaVencimiento = DateTime.Now.AddYears(2),
                EsAcompanante = incluirAcompanante
            };

            var pdfResult = await _carnetService.GenerarCarnetPDFAsync(carnetModel, incluirAcompanante);

            if (!pdfResult.Success)
                return BadRequest(new { success = false, message = pdfResult.Message });

            return File(pdfResult.Data, "application/pdf", $"Carnet_{numeroCarnetResult.Data}.pdf");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error generando carnet para solicitud {solicitudId}");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("verificar/{numeroCarnet}")]
    [AllowAnonymous]
    public async Task<IActionResult> VerificarCarnet(string numeroCarnet)
    {
        try
        {
            var resultado = await _carnetService.VerificarCarnetValidoAsync(numeroCarnet);

            if (!resultado.Success)
            {
                return Ok(new { 
                    success = false, 
                    valido = false, 
                    mensaje = resultado.Message 
                });
            }

            // Obtener información adicional del carnet
            var solicitud = await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .FirstOrDefaultAsync(s => s.NumeroCarnet == numeroCarnet);

            if (solicitud == null)
                return Ok(new { 
                    success = false, 
                    valido = false, 
                    mensaje = "Carnet no encontrado" 
                });

            var esAcompanante = numeroCarnet.StartsWith("DGSP-A-");
            
            return Ok(new { 
                success = true,
                valido = true,
                numeroCarnet,
                paciente = $"{solicitud.Paciente?.PrimerNombre} {solicitud.Paciente?.PrimerApellido}",
                documento = solicitud.Paciente?.DocumentoCedula ?? solicitud.Paciente?.DocumentoPasaporte,
                fechaEmision = solicitud.FechaEmisionCarnet?.ToString("dd/MM/yyyy"),
                fechaVencimiento = solicitud.FechaVencimientoCarnet?.ToString("dd/MM/yyyy"),
                esAcompanante,
                diasRestantes = solicitud.FechaVencimientoCarnet.HasValue ? 
                    (solicitud.FechaVencimientoCarnet.Value - DateTime.Now).Days : 0,
                estado = solicitud.CarnetActivo == true ? "Activo" : "Inactivo"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error verificando carnet {numeroCarnet}");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("qr/{numeroCarnet}")]
    [AllowAnonymous]
    public IActionResult GenerarQR(string numeroCarnet)
    {
        try
        {
            // Generar un QR simple usando texto alternativo
            using var bitmap = new System.Drawing.Bitmap(200, 200);
            using var graphics = System.Drawing.Graphics.FromImage(bitmap);
            graphics.Clear(System.Drawing.Color.White);
            
            // Dibujar texto alternativo
            using var font = new System.Drawing.Font("Arial", 12);
            using var brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            var rect = new System.Drawing.RectangleF(10, 10, 180, 180);
            graphics.DrawString(numeroCarnet, font, brush, rect);
            
            using var ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            
            return File(ms.ToArray(), "image/png");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error generando QR para carnet {numeroCarnet}");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("renovacion/disponible/{pacienteId}")]
    [Authorize]
    public async Task<IActionResult> VerificarRenovacionDisponible(int pacienteId)
    {
        try
        {
            var resultado = await _renovacionService.VerificarDisponibilidadRenovacionAsync(pacienteId);
            return Ok(new { success = resultado.Success, data = resultado.Data, message = resultado.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error verificando renovación para paciente {pacienteId}");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost("renovacion/iniciar")]
    [Authorize]
    public async Task<IActionResult> IniciarRenovacion([FromBody] RenovacionSolicitudModel solicitud)
    {
        try
        {
            var resultado = await _renovacionService.IniciarRenovacionAsync(
                solicitud.SolicitudOriginalId, 
                solicitud.DocumentosMedicosIds);

            if (!resultado.Success)
                return BadRequest(new { success = false, message = resultado.Message });

            return Ok(new { 
                success = true, 
                renovacionId = resultado.Data,
                message = resultado.Message 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error iniciando renovación para solicitud {solicitud.SolicitudOriginalId}");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("renovaciones/pendientes")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> ObtenerRenovacionesPendientes()
    {
        try
        {
            var renovaciones = await _renovacionService.ObtenerRenovacionesPendientesAsync();
            return Ok(new { success = true, data = renovaciones });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo renovaciones pendientes");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost("renovacion/completar/{renovacionId}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> CompletarRenovacion(int renovacionId, [FromBody] CompletarRenovacionModel model)
    {
        try
        {
            var resultado = await _renovacionService.CompletarRenovacionAsync(
                renovacionId, 
                model.Aprobada, 
                model.Comentario);

            return Ok(new { success = resultado.Success, data = resultado.Data, message = resultado.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error completando renovación {renovacionId}");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}

public class CompletarRenovacionModel
{
    public bool Aprobada { get; set; }
    public string Comentario { get; set; } = string.Empty;
}