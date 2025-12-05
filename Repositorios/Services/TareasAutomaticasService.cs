using DIGESA.Repositorios.Interfaces;
using Microsoft.Extensions.Hosting;

namespace DIGESA.Repositorios.Services;

public class TareasAutomaticasService : BackgroundService
{
    private readonly ILogger<TareasAutomaticasService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private Timer? _timerRecordatorios;
    private Timer? _timerInactivacion;

    public TareasAutomaticasService(
        ILogger<TareasAutomaticasService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Servicio de tareas automáticas iniciado");

        // Programar tareas
        // Recordatorios: Ejecutar diariamente a las 8:00 AM
        var ahora = DateTime.Now;
        var proximaEjecucionRecordatorios = ahora.Date.AddDays(1).AddHours(8);
        var tiempoHastaRecordatorios = proximaEjecucionRecordatorios - ahora;
        
        _timerRecordatorios = new Timer(
            async _ => await EjecutarRecordatoriosAsync(),
            null,
            tiempoHastaRecordatorios,
            TimeSpan.FromDays(1));

        // Inactivación: Ejecutar diariamente a las 9:00 AM
        var proximaEjecucionInactivacion = ahora.Date.AddDays(1).AddHours(9);
        var tiempoHastaInactivacion = proximaEjecucionInactivacion - ahora;
        
        _timerInactivacion = new Timer(
            async _ => await EjecutarInactivacionAsync(),
            null,
            tiempoHastaInactivacion,
            TimeSpan.FromDays(1));

        // Mantener el servicio activo
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }

    private async Task EjecutarRecordatoriosAsync()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var renovacionService = scope.ServiceProvider.GetRequiredService<IRenovacionService>();
            
            await renovacionService.ProcesarRecordatoriosVencimientoAsync();
            _logger.LogInformation("Recordatorios de vencimiento procesados");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ejecutando recordatorios automáticos");
        }
    }

    private async Task EjecutarInactivacionAsync()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var renovacionService = scope.ServiceProvider.GetRequiredService<IRenovacionService>();
            
            await renovacionService.InactivarCarnetsVencidosAsync();
            _logger.LogInformation("Carnets vencidos inactivados");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ejecutando inactivación automática");
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Servicio de tareas automáticas detenido");
        
        _timerRecordatorios?.Change(Timeout.Infinite, 0);
        _timerInactivacion?.Change(Timeout.Infinite, 0);
        
        await base.StopAsync(stoppingToken);
    }
}