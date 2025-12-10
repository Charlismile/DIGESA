namespace DIGESA.Models.CannabisModels;

public class ValidacionCarnetViewModel
{
    public string NumeroCarnet { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public bool CarnetActivo { get; set; }
    
    // Reglas de negocio
    public TimeSpan DuracionValidez => FechaVencimiento - FechaEmision;
    
    public bool EsValidoParaRenovacion
    {
        get
        {
            if (!CarnetActivo) return false;
            
            var tiempoRestante = FechaVencimiento - DateTime.Now;
            var maximoAntelacionRenovacion = TimeSpan.FromDays(60);
            var minimoAntelacionRenovacion = TimeSpan.FromDays(0);
            
            return tiempoRestante <= maximoAntelacionRenovacion && 
                   tiempoRestante >= minimoAntelacionRenovacion;
        }
    }
    
    public bool NecesitaRenovacionInmediata => 
        CarnetActivo && (FechaVencimiento - DateTime.Now).Days <= 7;
    
    public string EstadoCarnet
    {
        get
        {
            if (!CarnetActivo) return "Inactivo";
            if (DateTime.Now > FechaVencimiento) return "Vencido";
            if ((FechaVencimiento - DateTime.Now).Days <= 30) return "Por Vencer";
            return "Vigente";
        }
    }
}