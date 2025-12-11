using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services
{
    public class CommonServices : ICommon
    {
        private readonly DbContextDigesa _context;
        private readonly IConfiguration _configuration;

        public CommonServices(DbContextDigesa context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> GetFakePassword()
        {
            string password = _configuration["FakePass"] ?? "";
            return await Task.FromResult(password);
        }

        public async Task<List<ListSustModel>> GetProvincias()
        {
            var lista = new List<ListSustModel>();
            try
            {
                lista = await _context.TbProvincia
                    .Select(x => new ListSustModel
                    {
                        Id = x.Id,
                        Name = x.NombreProvincia ?? "",
                        Active = true
                    })
                    .OrderBy(x => x.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log error
            }
            return lista;
        }

        public async Task<List<ListSustModel>> GetDistritos(int provinciaId)
        {
            var lista = new List<ListSustModel>();
            try
            {
                lista = await _context.TbDistrito
                    .Where(x => x.ProvinciaId == provinciaId)
                    .Select(x => new ListSustModel
                    {
                        Id = x.Id,
                        Name = x.NombreDistrito ?? "",
                        Active = true
                    })
                    .OrderBy(x => x.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log error
            }
            return lista;
        }

        public async Task<List<ListSustModel>> GetCorregimientos(int distritoId)
        {
            var lista = new List<ListSustModel>();
            try
            {
                lista = await _context.TbCorregimiento
                    .Where(x => x.DistritoId == distritoId)
                    .Select(x => new ListSustModel
                    {
                        Id = x.Id,
                        Name = x.NombreCorregimiento ?? "",
                        Active = true
                    })
                    .OrderBy(x => x.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log error
            }
            return lista;
        }

        public async Task<List<ListSustModel>> GetRegionesSalud()
        {
            var lista = new List<ListSustModel>();
            try
            {
                lista = await _context.TbRegionSalud
                    .Select(x => new ListSustModel
                    {
                        Id = x.Id,
                        Name = x.Nombre ?? "",
                        Active = true
                    })
                    .OrderBy(x => x.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log error
            }
            return lista;
        }

        public async Task<List<ListSustModel>> GetInstalacionesSalud(int regionId)
        {
            var lista = new List<ListSustModel>();
            try
            {
                var query = _context.TbInstalacionSalud.AsQueryable();
                
                if (regionId > 0)
                {
                    // Asumiendo que hay una relación entre región e instalación
                    // Ajustar según tu esquema
                }

                lista = await query
                    .Select(x => new ListSustModel
                    {
                        Id = x.Id,
                        Name = x.Nombre ?? "",
                        Active = true
                    })
                    .OrderBy(x => x.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log error
            }
            return lista;
        }
    }
}