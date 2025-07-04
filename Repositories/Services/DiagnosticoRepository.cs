// using DIGESA.Models.Entities.DBDIGESA;
// using Microsoft.EntityFrameworkCore;
// using System.Threading.Tasks;
//
// public class DiagnosticoRepository : IDiagnosticoRepository
// {
//     private readonly DbContextDigesa _context;
//
//     public DiagnosticoRepository(DbContextDigesa context)
//     {
//         _context = context;
//     }
//
//     public async Task<Diagnostico> GetOrCreateAsync(Diagnostico diagnostico)
//     {
//         var existing = await _context.Diagnostico
//             .FirstOrDefaultAsync(d => d.Nombre == diagnostico.Nombre || d.CodigoCie10 == diagnostico.CodigoCie10);
//
//         if (existing != null)
//             return existing;
//
//         await _context.Diagnostico.AddAsync(diagnostico);
//         await _context.SaveChangesAsync();
//         return diagnostico;
//     }
//
//     public async Task<Diagnostico> GetByNombreOrCodigoAsync(string nombre, string codigoCie10)
//     {
//         return await _context.Diagnostico
//             .FirstOrDefaultAsync(d => d.Nombre == nombre || d.CodigoCie10 == codigoCie10);
//     }
// }