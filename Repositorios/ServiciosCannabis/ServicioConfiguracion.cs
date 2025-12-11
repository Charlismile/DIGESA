using System.Collections.Concurrent;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.InterfacesCannabis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DIGESA.Repositorios.ServiciosCannabis
{
    public class ServicioConfiguracion : IServicioConfiguracion
    {
        private readonly DbContextDigesa _context;
        private readonly ILogger<ServicioConfiguracion> _logger;
        private static readonly ConcurrentDictionary<string, ConfiguracionSistemaViewModel> _cache = new();

        public ServicioConfiguracion(DbContextDigesa context, ILogger<ServicioConfiguracion> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ConfiguracionCompletaViewModel> ObtenerConfiguracionCompleta()
        {
            try
            {
                var configuraciones = await _context.TbConfiguracionSistema.ToListAsync();

                var configCompleta = new ConfiguracionCompletaViewModel();

                foreach (var config in configuraciones)
                {
                    var configVm = MapToViewModel(config);
                    
                    switch (config.Clave)
                    {
                        case "DiasVigenciaCarnet":
                            configCompleta.DiasVigenciaCarnet = configVm.ValorEntero;
                            break;
                        case "DiasAntesNotificar":
                            configCompleta.DiasAntesNotificar = configVm.ValorEntero;
                            break;
                        case "DiasGraciaRenovacion":
                            configCompleta.DiasGraciaRenovacion = configVm.ValorEntero;
                            break;
                        case "AutoInactivarDias":
                            configCompleta.AutoInactivarDias = configVm.ValorEntero;
                            break;
                        case "MaximoRenovaciones":
                            configCompleta.MaximoRenovaciones = configVm.ValorEntero;
                            break;
                        case "NotificarPorEmail":
                            configCompleta.NotificarPorEmail = configVm.ValorBooleano;
                            break;
                        case "NotificarPorSMS":
                            configCompleta.NotificarPorSMS = configVm.ValorBooleano;
                            break;
                        case "ItemsPorPagina":
                            configCompleta.ItemsPorPagina = configVm.ValorEntero;
                            break;
                        case "EmailRemitente":
                            configCompleta.EmailRemitente = configVm.Valor;
                            break;
                    }
                }

                return configCompleta;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo configuración completa");
                return new ConfiguracionCompletaViewModel();
            }
        }

        public async Task<ConfiguracionSistemaViewModel> ObtenerConfiguracion(string clave)
        {
            try
            {
                if (_cache.TryGetValue(clave, out var configCache))
                    return configCache;

                var config = await _context.TbConfiguracionSistema
                    .FirstOrDefaultAsync(c => c.Clave == clave);

                if (config == null)
                {
                    config = await CrearConfiguracionPorDefecto(clave);
                }

                var configVm = MapToViewModel(config);
                _cache[clave] = configVm;

                return configVm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo configuración para clave {Clave}", clave);
                return new ConfiguracionSistemaViewModel { Clave = clave, Valor = "0" };
            }
        }

        public async Task<int> ObtenerValorEntero(string clave, int valorDefecto = 0)
        {
            var config = await ObtenerConfiguracion(clave);
            return config.ValorEntero > 0 ? config.ValorEntero : valorDefecto;
        }

        public async Task<bool> ObtenerValorBooleano(string clave, bool valorDefecto = false)
        {
            var config = await ObtenerConfiguracion(clave);
            return config.ValorBooleano;
        }

        public async Task<string> ObtenerValorString(string clave, string valorDefecto = "")
        {
            var config = await ObtenerConfiguracion(clave);
            return !string.IsNullOrEmpty(config.Valor) ? config.Valor : valorDefecto;
        }

        public async Task<bool> GuardarConfiguracion(ConfiguracionCompletaViewModel configuracion)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                var configuraciones = configuracion.ToConfiguracionesList();
                
                foreach (var config in configuraciones)
                {
                    var entidad = await _context.TbConfiguracionSistema
                        .FirstOrDefaultAsync(c => c.Clave == config.Clave);

                    if (entidad == null)
                    {
                        entidad = new TbConfiguracionSistema
                        {
                            Clave = config.Clave,
                            Valor = config.Valor,
                            Descripcion = config.Descripcion,
                            Grupo = config.Grupo,
                            EsEditable = config.EsEditable
                        };
                        _context.TbConfiguracionSistema.Add(entidad);
                    }
                    else if (entidad.EsEditable)
                    {
                        entidad.Valor = config.Valor;
                        entidad.Descripcion = config.Descripcion;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                
                _cache.Clear();
                
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error guardando configuración");
                return false;
            }
        }

        private async Task<TbConfiguracionSistema> CrearConfiguracionPorDefecto(string clave)
        {
            var configPorDefecto = GetConfiguracionPorDefecto(clave);
            
            if (configPorDefecto == null)
                return null;

            _context.TbConfiguracionSistema.Add(configPorDefecto);
            await _context.SaveChangesAsync();
            
            return configPorDefecto;
        }

        private TbConfiguracionSistema GetConfiguracionPorDefecto(string clave)
        {
            return clave switch
            {
                "DiasVigenciaCarnet" => new TbConfiguracionSistema 
                { 
                    Clave = clave, 
                    Valor = "730", 
                    Descripcion = "Días de vigencia del carnet",
                    Grupo = "Vencimientos",
                    EsEditable = false 
                },
                "DiasAntesNotificar" => new TbConfiguracionSistema 
                { 
                    Clave = clave, 
                    Valor = "30", 
                    Descripcion = "Días antes para notificar vencimiento",
                    Grupo = "Notificaciones",
                    EsEditable = true 
                },
                "DiasGraciaRenovacion" => new TbConfiguracionSistema 
                { 
                    Clave = clave, 
                    Valor = "90", 
                    Descripcion = "Días de gracia después del vencimiento",
                    Grupo = "Renovaciones",
                    EsEditable = true 
                },
                "MaximoRenovaciones" => new TbConfiguracionSistema 
                { 
                    Clave = clave, 
                    Valor = "10", 
                    Descripcion = "Máximo número de renovaciones permitidas",
                    Grupo = "Renovaciones",
                    EsEditable = false 
                },
                "AutoInactivarDias" => new TbConfiguracionSistema 
                { 
                    Clave = clave, 
                    Valor = "0", 
                    Descripcion = "Días después del vencimiento para autoinactivar",
                    Grupo = "Vencimientos",
                    EsEditable = true 
                },
                "NotificarPorEmail" => new TbConfiguracionSistema 
                { 
                    Clave = clave, 
                    Valor = "true", 
                    Descripcion = "Habilitar notificaciones por email",
                    Grupo = "Notificaciones",
                    EsEditable = true 
                },
                _ => null
            };
        }

        private ConfiguracionSistemaViewModel MapToViewModel(TbConfiguracionSistema entity)
        {
            return new ConfiguracionSistemaViewModel
            {
                Id = entity.Id,
                Clave = entity.Clave,
                Valor = entity.Valor,
                Descripcion = entity.Descripcion,
                Grupo = entity.Grupo,
                EsEditable = entity.EsEditable
            };
        }
    }
}