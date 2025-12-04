using DIGESA.Data;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class UsuarioService : IUsuarioService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly DbContextDigesa _context;
    private readonly ILogger<UsuarioService> _logger;

    public UsuarioService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        DbContextDigesa context,
        ILogger<UsuarioService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _logger = logger;
    }

    public async Task<List<UsuarioAprobacionModel>> ObtenerUsuariosAsync(FiltroUsuariosModel filtros)
    {
        try
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(filtros.Estado))
            {
                if (filtros.Estado == "Aprobado")
                {
                    query = query.Where(u => u.IsAproved);
                }
                else if (filtros.Estado == "Pendiente")
                {
                    query = query.Where(u => !u.IsAproved);
                }
            }

            if (!string.IsNullOrEmpty(filtros.TerminoBusqueda))
            {
                var termino = filtros.TerminoBusqueda.ToLower();
                query = query.Where(u =>
                    (u.FirstName != null && u.FirstName.ToLower().Contains(termino)) ||
                    (u.LastName != null && u.LastName.ToLower().Contains(termino)) ||
                    (u.Email != null && u.Email.ToLower().Contains(termino)));
            }

            var usuarios = await query
                .OrderByDescending(u => u.CreatedOn)
                .ToListAsync();

            var resultado = new List<UsuarioAprobacionModel>();

            foreach (var usuario in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(usuario);
                var rolPrincipal = roles.FirstOrDefault() ?? "Sin rol";

                var usuarioModel = new UsuarioAprobacionModel
                {
                    Id = usuario.Id,
                    UserName = usuario.UserName ?? string.Empty,
                    Email = usuario.Email ?? string.Empty,
                    NombreCompleto = $"{usuario.FirstName} {usuario.LastName}".Trim(),
                    Rol = rolPrincipal,
                    IsAproved = usuario.IsAproved,
                    IsActive = usuario.LockoutEnd == null || usuario.LockoutEnd <= DateTimeOffset.Now,
                    CreatedOn = usuario.CreatedOn ?? DateTime.MinValue,
                    LastLoginDate = usuario.LastLoginDate,
                    MustChangePassword = usuario.MustChangePassword,
                    FechaAprobacion = null, // Buscar en historial si existe
                    AprobadoPor = "Sistema"
                };

                resultado.Add(usuarioModel);
            }

            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo usuarios");
            return new List<UsuarioAprobacionModel>();
        }
    }

    public async Task<ResultModel<bool>> CambiarEstadoUsuarioAsync(CambioEstadoUsuarioModel cambio, string usuarioModificador)
    {
        try
        {
            var usuario = await _userManager.FindByIdAsync(cambio.UsuarioId);
            if (usuario == null)
                return ResultModel<bool>.ErrorResult("Usuario no encontrado");

            switch (cambio.Estado.ToLower())
            {
                case "aprobar":
                    usuario.IsAproved = true;
                    break;

                case "rechazar":
                    usuario.IsAproved = false;
                    break;

                case "activar":
                    usuario.LockoutEnd = null;
                    break;

                case "desactivar":
                    usuario.LockoutEnd = DateTimeOffset.MaxValue;
                    break;

                default:
                    return ResultModel<bool>.ErrorResult("Estado no válido");
            }

            var resultado = await _userManager.UpdateAsync(usuario);
            if (!resultado.Succeeded)
            {
                var errores = resultado.Errors.Select(e => e.Description).ToList();
                return ResultModel<bool>.ErrorResult("Error al actualizar usuario", errores);
            }

            // Registrar en historial
            var historial = new TbHistorialUsuario
            {
                UsuarioId = usuario.Id,
                TipoCambio = cambio.Estado,
                EstadoAnterior = usuario.IsAproved ? "Aprobado" : "Pendiente",
                EstadoNuevo = cambio.Estado,
                FechaCambio = DateTime.Now,
                CambioPor = usuarioModificador,
                Comentario = $"Motivo: {cambio.Motivo}. Comentario: {cambio.Comentario}"
            };

            _context.TbHistorialUsuario.Add(historial);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Usuario {usuario.Id} actualizado a estado: {cambio.Estado}");
            return ResultModel<bool>.SuccessResult(true, $"Usuario {cambio.Estado} exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error cambiando estado del usuario {cambio.UsuarioId}");
            return ResultModel<bool>.ErrorResult("Error al cambiar el estado del usuario", new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<UsuarioModel>> ObtenerUsuarioPorIdAsync(string usuarioId)
    {
        try
        {
            var usuario = await _userManager.FindByIdAsync(usuarioId);
            if (usuario == null)
                return ResultModel<UsuarioModel>.ErrorResult("Usuario no encontrado");

            var roles = await _userManager.GetRolesAsync(usuario);
            var rolPrincipal = roles.FirstOrDefault() ?? "Sin rol";

            var usuarioModel = new UsuarioModel
            {
                Id = usuario.Id,
                UserName = usuario.UserName ?? string.Empty,
                Email = usuario.Email ?? string.Empty,
                NombreCompleto = $"{usuario.FirstName} {usuario.LastName}".Trim(),
                Rol = rolPrincipal,
                IsAproved = usuario.IsAproved,
                IsActive = usuario.LockoutEnd == null || usuario.LockoutEnd <= DateTimeOffset.Now,
                CreatedOn = usuario.CreatedOn ?? DateTime.MinValue,
                LastLoginDate = usuario.LastLoginDate,
                MustChangePassword = usuario.MustChangePassword
            };

            return ResultModel<UsuarioModel>.SuccessResult(usuarioModel, "Usuario obtenido exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error obteniendo usuario {usuarioId}");
            return ResultModel<UsuarioModel>.ErrorResult("Error al obtener el usuario", new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<bool>> ReasignarRolAsync(string usuarioId, string nuevoRol, string motivo)
    {
        try
        {
            var usuario = await _userManager.FindByIdAsync(usuarioId);
            if (usuario == null)
                return ResultModel<bool>.ErrorResult("Usuario no encontrado");

            var rolExiste = await _roleManager.RoleExistsAsync(nuevoRol);
            if (!rolExiste)
                return ResultModel<bool>.ErrorResult($"El rol '{nuevoRol}' no existe");

            var rolesActuales = await _userManager.GetRolesAsync(usuario);
            var removeResult = await _userManager.RemoveFromRolesAsync(usuario, rolesActuales);
            if (!removeResult.Succeeded)
            {
                var errores = removeResult.Errors.Select(e => e.Description).ToList();
                return ResultModel<bool>.ErrorResult("Error al remover roles actuales", errores);
            }

            var addResult = await _userManager.AddToRoleAsync(usuario, nuevoRol);
            if (!addResult.Succeeded)
            {
                var errores = addResult.Errors.Select(e => e.Description).ToList();
                return ResultModel<bool>.ErrorResult("Error al asignar nuevo rol", errores);
            }

            // Registrar en historial
            var historial = new TbHistorialUsuario
            {
                UsuarioId = usuario.Id,
                TipoCambio = "Cambio de Rol",
                EstadoAnterior = rolesActuales.FirstOrDefault() ?? "Sin rol",
                EstadoNuevo = nuevoRol,
                FechaCambio = DateTime.Now,
                CambioPor = "Sistema",
                Comentario = motivo
            };

            _context.TbHistorialUsuario.Add(historial);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Usuario {usuario.Id} reasignado a rol '{nuevoRol}'");
            return ResultModel<bool>.SuccessResult(true, $"Rol reasignado exitosamente a '{nuevoRol}'");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error reasignando rol al usuario {usuarioId}");
            return ResultModel<bool>.ErrorResult("Error al reasignar el rol", new List<string> { ex.Message });
        }
    }

    public async Task<List<HistorialUsuarioModel>> ObtenerHistorialUsuarioAsync(string usuarioId)
    {
        try
        {
            return await _context.TbHistorialUsuario
                .Where(h => h.UsuarioId == usuarioId)
                .OrderByDescending(h => h.FechaCambio)
                .Select(h => new HistorialUsuarioModel
                {
                    Id = h.Id,
                    UsuarioId = h.UsuarioId,
                    EstadoAnterior = h.EstadoAnterior ?? string.Empty,
                    EstadoNuevo = h.EstadoNuevo ?? string.Empty,
                    FechaCambio = h.FechaCambio ?? DateTime.MinValue,
                    CambioPor = h.CambioPor ?? string.Empty,
                    TipoCambio = h.TipoCambio ?? string.Empty,
                    Comentario = h.Comentario ?? string.Empty
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error obteniendo historial del usuario {usuarioId}");
            return new List<HistorialUsuarioModel>();
        }
    }

    public async Task<List<HistorialUsuarioModel>> ObtenerHistorialPorFechasAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        try
        {
            return await _context.TbHistorialUsuario
                .Where(h => h.FechaCambio >= fechaInicio && h.FechaCambio <= fechaFin)
                .OrderByDescending(h => h.FechaCambio)
                .Select(h => new HistorialUsuarioModel
                {
                    Id = h.Id,
                    UsuarioId = h.UsuarioId,
                    EstadoAnterior = h.EstadoAnterior ?? string.Empty,
                    EstadoNuevo = h.EstadoNuevo ?? string.Empty,
                    FechaCambio = h.FechaCambio ?? DateTime.MinValue,
                    CambioPor = h.CambioPor ?? string.Empty,
                    TipoCambio = h.TipoCambio ?? string.Empty,
                    Comentario = h.Comentario ?? string.Empty
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error obteniendo historial por fechas {fechaInicio} - {fechaFin}");
            return new List<HistorialUsuarioModel>();
        }
    }
}