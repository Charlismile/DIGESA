// using DIGESA.Models.Entities.DBDIGESA;
//
// public class SolicitudRepository : ISolicitudRepository
// {
//     private readonly DbContextDigesa _context;
//
//     public SolicitudRepository(DbContextDigesa context) => _context = context;
//
//     public async Task AddAsync(Solicitud solicitud)
//     {
//         await _context.Solicitud.AddAsync(solicitud);
//         await _context.SaveChangesAsync();
//     }
// }