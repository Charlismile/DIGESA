namespace DIGESA.Repositorios.Interfaces;

public interface IBackgroundService
{
    /// <summary>
    /// Inicia el servicio de tareas en segundo plano
    /// </summary>
    Task StartAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Detiene el servicio de tareas en segundo plano
    /// </summary>
    Task StopAsync(CancellationToken cancellationToken);
}