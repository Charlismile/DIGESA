using DIGESA.Repositorios.Interfaces;

namespace DIGESA.Repositorios.Services;

// Servicio para recordatorios automáticos programados
public class RecordatorioVencimientoService : BackgroundService
{
    private readonly ILogger<RecordatorioVencimientoService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public RecordatorioVencimientoService(
        ILogger<RecordatorioVencimientoService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Servicio de recordatorios iniciado");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Ejecutar diario a las 8:00 AM
                var ahora = DateTime.Now;
                var horaEjecucion = new DateTime(ahora.Year, ahora.Month, ahora.Day, 8, 0, 0);
                
                if (ahora >= horaEjecucion)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var notificacionService = scope.ServiceProvider.GetRequiredService<INotificacionService>();
                    await notificacionService.EnviarNotificacionesPendientesAsync();
                    _logger.LogInformation("Recordatorios enviados");
                }

                // Esperar 1 hora antes de volver a verificar
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio de recordatorios");
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}