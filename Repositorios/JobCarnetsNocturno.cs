using DIGESA.Repositorios.InterfacesCannabis;

namespace DIGESA.Repositorios;

public class JobCarnetsNocturno : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public JobCarnetsNocturno(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var ahora = DateTime.Now;

            if (ahora.Hour == 1) // 1:00 AM
            {
                using var scope = _scopeFactory.CreateScope();
                var servicio = scope.ServiceProvider.GetRequiredService<IServicioRenovaciones>();

                await servicio.InactivarCarnetsVencidos();
                await servicio.ProcesarRenovacionesAutomaticas();

                await Task.Delay(TimeSpan.FromHours(23), stoppingToken);
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            
        }
    }
}
