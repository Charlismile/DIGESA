using DIGESA.Data;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class ConfiguracionSistemaService : IConfiguracionSistemaService
{
    private readonly DbContextDigesa _context;
    private readonly ILogger<ConfiguracionSistemaService> _logger;

    public ConfiguracionSistemaService(
        DbContextDigesa context,
        ILogger<ConfiguracionSistemaService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ResultModel<ConfiguracionNotificacionModel>> ObtenerConfiguracionNotificacionesAsync()
    {
        try
        {
            // Por ahora, retornamos configuración por defecto
            // En el futuro, podrías almacenar esto en una tabla de configuración
            var config = new ConfiguracionNotificacionModel
            {
                Id = 1,
                DiasRecordatorio1 = 30,
                DiasRecordatorio2 = 15,
                DiasRecordatorio3 = 7,
                NotificarAntesVencimiento = true,
                NotificarVencimiento = true,
                NotificarInactivacion = true,
                ActivarInactivacionAutomatica = true,
                DiasInactivacionDespuesVencimiento = 0,
                AsuntoRecordatorio = "Recordatorio: Carnet próximo a vencer",
                AsuntoVencimiento = "Aviso: Carnet vencido",
                AsuntoInactivacion = "Aviso: Carnet inactivado"
            };

            return ResultModel<ConfiguracionNotificacionModel>.SuccessResult(
                config, 
                "Configuración obtenida exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo configuración de notificaciones");
            return ResultModel<ConfiguracionNotificacionModel>.ErrorResult("Error al obtener la configuración");
        }
    }

    public async Task<ResultModel<ConfiguracionNotificacionModel>> ActualizarConfiguracionNotificacionesAsync(
        ConfiguracionNotificacionModel configuracion)
    {
        try
        {
            // Validar valores
            if (configuracion.DiasRecordatorio1 <= 0 || 
                configuracion.DiasRecordatorio2 <= 0 || 
                configuracion.DiasRecordatorio3 <= 0)
            {
                return ResultModel<ConfiguracionNotificacionModel>.ErrorResult(
                    "Los días de recordatorio deben ser mayores a 0");
            }

            // Aquí podrías guardar en la base de datos
            // Por ahora, solo retornamos éxito

            _logger.LogInformation("Configuración de notificaciones actualizada");
            return ResultModel<ConfiguracionNotificacionModel>.SuccessResult(
                configuracion, 
                "Configuración actualizada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando configuración de notificaciones");
            return ResultModel<ConfiguracionNotificacionModel>.ErrorResult("Error al actualizar la configuración");
        }
    }

    public async Task<ResultModel<string>> ObtenerDeclaracionJuradaTextoAsync()
    {
        try
        {
            var texto = @"El Ministerio de Salud no se hace responsable del uso que el paciente le dé al producto medicinal de cannabis ni de los efectos que estos puedan provocar.";

            return ResultModel<string>.SuccessResult(texto, "Texto de declaración jurada obtenido");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo texto de declaración jurada");
            return ResultModel<string>.ErrorResult("Error al obtener el texto de declaración jurada");
        }
    }

    public async Task<ResultModel<List<ParametroSistemaModel>>> ObtenerParametrosSistemaAsync()
    {
        try
        {
            var parametros = new List<ParametroSistemaModel>
            {
                new() { 
                    Codigo = "VIGENCIA_CARNET", 
                    Nombre = "Vigencia del carnet (años)", 
                    Valor = "2",
                    Tipo = "Entero",
                    Descripcion = "Período de validez del carnet en años"
                },
                new() { 
                    Codigo = "DIAS_RENOVACION_ANTES", 
                    Nombre = "Días para renovar antes del vencimiento", 
                    Valor = "60",
                    Tipo = "Entero",
                    Descripcion = "Días antes del vencimiento que puede iniciar renovación"
                },
                new() { 
                    Codigo = "EMAIL_SOPORTE", 
                    Nombre = "Email de soporte", 
                    Valor = "cannabis@digesa.gob.pa",
                    Tipo = "Texto",
                    Descripcion = "Email para contacto de soporte"
                },
                new() { 
                    Codigo = "MAX_DOCUMENTOS_MB", 
                    Nombre = "Tamaño máximo de documentos (MB)", 
                    Valor = "10",
                    Tipo = "Entero",
                    Descripcion = "Tamaño máximo por documento en megabytes"
                }
            };

            return ResultModel<List<ParametroSistemaModel>>.SuccessResult(
                parametros, 
                "Parámetros del sistema obtenidos");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo parámetros del sistema");
            return ResultModel<List<ParametroSistemaModel>>.ErrorResult("Error al obtener los parámetros");
        }
    }
}