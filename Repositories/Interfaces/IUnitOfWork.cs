namespace DIGESA.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IPacienteRepository Pacientes { get; }
    IMedicoRepository Medicos { get; }
    IAcompananteRepository Acompanantes { get; }
    ISolicitudRepository Solicitudes { get; }
    IDiagnosticoRepository Diagnosticos { get; }
    ITratamientoRepository Tratamientos { get; }

    // Más repositorios según tus entidades...

    Task<int> CompleteAsync(); // Guarda todos los cambios en la DB
}