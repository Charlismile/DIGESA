namespace DIGESA.Models.CannabisModels;

public class EnumViewModel
{
    public enum TipoDocumento
    {
        Cedula = 1,
        Pasaporte = 2
    }

    
    public enum Sexo
    {
        Masculino = 1,
        Femenino = 2
    }

    
    public enum RequiereAcompanante
    {
        No = 0,
        Si = 1
    }
    
    public enum MotivoRequerimientoAcompanante
    {
        PacienteMenorEdad = 1,
        PacienteDiscapacidad = 2
    }
    
    public enum Parentesco
    {
        Madre = 1,
        Padre = 2,
        Tutor = 3
    }

    
    public enum MedicoDisciplina
    {
        General,
        Odontologo,
        Especialista
    }
    
    public enum NombreProductoE
    {
        CBD,
        THC,
        OTRO
    }
    
    public enum ConcentracionE
    {
        CBD,
        THC,
        OTRO
    }
    
    public enum UsaDosisRescate
    {
        No = 0,
        Si = 1
    }

    
    public enum TieneComorbilidad
    {
        Si,
        No
    }
}