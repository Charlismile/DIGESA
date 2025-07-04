// using DIGESA.Models.Entities.DBDIGESA;
// using Microsoft.EntityFrameworkCore;
// using System.Threading.Tasks;
//
// public class MedicoRepository : IMedicoRepository
// {
//     private readonly DbContextDigesa _context;
//
//     public MedicoRepository(DbContextDigesa context)
//     {
//         _context = context;
//     }
//
//     public async Task<Medico> GetByIdAsync(int id)
//     {
//         return await _context.Medico.FindAsync(id);
//     }
//
//     public async Task AddAsync(Medico medico)
//     {
//         await _context.Medico.AddAsync(medico);
//         await _context.SaveChangesAsync();
//     }
//
//     public async Task<bool> ExistePorDocumento(string numeroRegistro)
//     {
//         return await _context.Medico
//             .AnyAsync(m => m.NumeroRegistroIdoneidad == numeroRegistro);
//     }
//
//     public async Task UpdateAsync(Medico medico)
//     {
//         _context.Medico.Update(medico);
//         await _context.SaveChangesAsync();
//     }
// }