using DIGESA.Repositorios.InterfacesCannabis;

namespace DIGESA.Repositorios.ServiciosCannabis;

public class TareasAutomaticasService : BackgroundService
{
    private readonly ILogger<TareasAutomaticasService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _intervaloNotificaciones = TimeSpan.FromHours(6);
    private readonly TimeSpan _intervaloInactivaciones = TimeSpan.FromDays(1);

    public TareasAutomaticasService(
        ILogger<TareasAutomaticasService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Servicio de tareas automáticas iniciado");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                
                var servicioRenovaciones = scope.ServiceProvider
                    .GetRequiredService<IServicioRenovaciones>();

                var servicioConfiguracion = scope.ServiceProvider
                    .GetRequiredService<IServicioConfiguracion>();

                var config = await servicioConfiguracion.ObtenerConfiguracionCompleta();

                // 1. Enviar notificaciones de vencimiento (cada 6 horas)
                if (config.NotificarPorEmail || config.NotificarPorSMS)
                {
                    await servicioRenovaciones.EnviarNotificacionesVencimiento();
                    _logger.LogInformation("Notificaciones de vencimiento procesadas");
                }

                // 2. Inactivar carnets vencidos (una vez al día a medianoche)
                if (DateTime.Now.Hour == 0 && config.AutoInactivarDias > 0)
                {
                    await servicioRenovaciones.InactivarCarnetsVencidos();
                    _logger.LogInformation("Carnets vencidos inactivados");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando tareas automáticas");
            }

            // Esperar hasta la próxima ejecución
            await Task.Delay(_intervaloNotificaciones, stoppingToken);
        }
    }
}