// using System.Collections.Generic;
// using System.Threading.Tasks;
// using DIGESA.Models.Entities.DBDIGESA;
// using Microsoft.EntityFrameworkCore;
//
// public class PacienteRepository : IPacienteRepository
// {
//     private readonly DbContextDigesa _context;
//
//     public PacienteRepository(DbContextDigesa context)
//     {
//         _context = context;
//     }
//
//     public async Task<Paciente> GetByIdAsync(int id)
//     {
//         return await _context.Paciente.FindAsync(id);
//     }
//
//     public async Task<List<Paciente>> GetAllAsync()
//     {
//         return await _context.Paciente.ToListAsync();
//     }
//
//     public async Task AddAsync(Paciente paciente)
//     {
//         await _context.Paciente.AddAsync(paciente);
//         await _context.SaveChangesAsync();
//     }
//
//     public async Task UpdateAsync(Paciente paciente)
//     {
//         _context.Paciente.Update(paciente);
//         await _context.SaveChangesAsync();
//     }
//
//     public async Task DeleteAsync(int id)
//     {
//         var paciente = await GetByIdAsync(id);
//         if (paciente != null)
//         {
//             _context.Paciente.Remove(paciente);
//             await _context.SaveChangesAsync();
//         }
//     }
// }