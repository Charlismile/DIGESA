// using DIGESA.Models.Entities.DBDIGESA;
// using DIGESA.Repositories.Interfaces;
//
// namespace DIGESA.Repositories.Services;
//
// public class UnitOfWork : IUnitOfWork
// {
//     private readonly DbContextDigesa _context;
//
//     public IPacienteRepository Pacientes { get; private set; }
//     public IMedicoRepository Medicos { get; private set; }
//     public IAcompananteRepository Acompanantes { get; private set; }
//     public ISolicitudRepository Solicitudes { get; private set; }
//     public IDiagnosticoRepository Diagnosticos { get; private set; }
//     public ITratamientoRepository Tratamientos { get; private set; }
//
//     public UnitOfWork(DbContextDigesa context)
//     {
//         _context = context;
//         Pacientes = new PacienteRepository(_context);
//         Medicos = new MedicoRepository(_context);
//         Acompanantes = new AcompananteRepository(_context);
//         Solicitudes = new SolicitudRepository(_context);
//         Diagnosticos = new DiagnosticoRepository(_context);
//         Tratamientos = new TratamientoRepository(_context);
//     }
//
//     public async Task<int> CompleteAsync()
//     {
//         return await _context.SaveChangesAsync();
//     }
//
//     public void Dispose()
//     {
//         _context.Dispose();
//     }
// }