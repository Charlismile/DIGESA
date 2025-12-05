using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class InactivacionAutomaticaService : IInactivacionService
{
    private readonly DbContextDigesa _context;
    private readonly IEmailService _emailService;
    private readonly ILogger<InactivacionAutomaticaService> _logger;

    public InactivacionAutomaticaService(
        DbContextDigesa context,
        IEmailService emailService,
        ILogger<InactivacionAutomaticaService> logger)
    {
        _context = context;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<ResultModel<int>> InactivarCarnetsVencidos2AniosAsync()
    {
        try
        {
            var fechaLimite = DateTime.Now.AddYears(-2);
            var carnetsVencidos = await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .Where(s => s.CarnetActivo == true && 
                           s.FechaVencimientoCarnet.HasValue && 
                           s.FechaVencimientoCarnet.Value < fechaLimite)
                .ToListAsync();

            var contador = 0;

            foreach (var carnet in carnetsVencidos)
            {
                carnet.CarnetActivo = false;
                contador++;

                // Registrar en historial
                var historial = new TbSolRegCannabisHistorial
                {
                    SolRegCannabisId = carnet.Id,
                    Comentario = "Carnet inactivado automáticamente por vencimiento de 2 años",
                    UsuarioRevisor = "Sistema",
                    FechaCambio = DateOnly.FromDateTime(DateTime.Now),
                    EstadoSolicitudIdHistorial = 5 // Estado "Inactivo"
                };
                _context.TbSolRegCannabisHistorial.Add(historial);

                // Enviar notificación
                if (!string.IsNullOrEmpty(carnet.Paciente?.CorreoElectronico))
                {
                    await _emailService.EnviarCorreoAsync(
                        carnet.Paciente.CorreoElectronico,
                        "Carnet de Cannabis Medicinal inactivado",
                        $@"Estimado/a {carnet.Paciente.PrimerNombre},

Su carnet de cannabis medicinal ha sido inactivado automáticamente 
debido a que ha superado los 2 años de vigencia.

<b>Número de Carnet:</b> {carnet.NumeroCarnet}
<b>Fecha de Inactivación:</b> {DateTime.Now:dd/MM/yyyy}

Para reactivar su carnet, deberá realizar una nueva solicitud completa 
en nuestro sistema.

Atentamente,
DIGESA - Dirección General de Salud"
                    );
                }
            }

            if (contador > 0)
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Inactivados {contador} carnets con más de 2 años de vencimiento");
            }

            return ResultModel<int>.SuccessResult(contador, 
                contador > 0 ? 
                $"Inactivados {contador} carnets vencidos por más de 2 años" :
                "No se encontraron carnets para inactivar");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inactivando carnets vencidos por 2 años");
            return ResultModel<int>.ErrorResult("Error al inactivar carnets vencidos", 
                new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<List<CarnetVencidoModel>>> ObtenerCarnetsProximosInactivacionAsync()
    {
        try
        {
            var fechaLimite = DateTime.Now.AddYears(-2);
            var fechaAdvertencia = fechaLimite.AddDays(30); // 30 días antes de inactivación

            var carnets = await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .Where(s => s.CarnetActivo == true && 
                           s.FechaVencimientoCarnet.HasValue && 
                           s.FechaVencimientoCarnet.Value < fechaAdvertencia)
                .Select(s => new CarnetVencidoModel
                {
                    Id = s.Id,
                    NumeroCarnet = s.NumeroCarnet ?? "N/A",
                    PacienteNombre = $"{s.Paciente.PrimerNombre} {s.Paciente.PrimerApellido}",
                    PacienteDocumento = s.Paciente.DocumentoCedula ?? s.Paciente.DocumentoPasaporte ?? "N/A",
                    FechaVencimiento = s.FechaVencimientoCarnet.Value,
                    DiasDesdeVencimiento = (int)(DateTime.Now - s.FechaVencimientoCarnet.Value).TotalDays,
                    ProximoInactivacion = s.FechaVencimientoCarnet.Value.AddYears(2),
                    DiasParaInactivacion = (int)(s.FechaVencimientoCarnet.Value.AddYears(2) - DateTime.Now).TotalDays
                })
                .OrderBy(c => c.ProximoInactivacion)
                .ToListAsync();

            return ResultModel<List<CarnetVencidoModel>>.SuccessResult(carnets, 
                $"Encontrados {carnets.Count} carnets próximos a inactivación");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo carnets próximos a inactivación");
            return ResultModel<List<CarnetVencidoModel>>.ErrorResult(
                "Error al obtener carnets próximos a inactivación", 
                new List<string> { ex.Message });
        }
    }
}

public class CarnetVencidoModel
{
    public int Id { get; set; }
    public string NumeroCarnet { get; set; } = string.Empty;
    public string PacienteNombre { get; set; } = string.Empty;
    public string PacienteDocumento { get; set; } = string.Empty;
    public DateTime FechaVencimiento { get; set; }
    public int DiasDesdeVencimiento { get; set; }
    public DateTime ProximoInactivacion { get; set; }
    public int DiasParaInactivacion { get; set; }
}