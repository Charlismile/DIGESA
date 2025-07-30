namespace DIGESA.DTOs;

public class UbicacionDto
{
    //REGION SALUD
    public int RId { get; set; }
    public string NombreRegion { get; set; } = string.Empty;

    //PROVINCIA
    public int PId { get; set; }   
    public string NombreProvincia { get; set; } = string.Empty;
    
    //DISTRITO
    public int DId { get; set; }
    public string NombreDistrito { get; set; } = string.Empty;
    public int ProvinciaId { get; set; }
    
    //CORREGIMIENTO
    public int CId { get; set; }
    public string NombreCorregimiento { get; set; } = string.Empty;
    public int DistritoId { get; set; }
    
    //INSTALACION SALUD
    public int IId { get; set; }
    public string NombreInstalacion { get; set; } = string.Empty;
}