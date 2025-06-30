namespace DIGESA.Models.DTOs;

public class DiagnosticoRegistroDTO
{
    public bool Alzheimer { get; set; }
    public bool Epilepsia { get; set; }
    public bool SIDA { get; set; }
    public bool Anorexia { get; set; }
    public bool Fibromialgia { get; set; }
    public bool Artritis { get; set; }
    public bool Glaucoma { get; set; }
    public bool EstrésPostraumatico { get; set; }
    public bool Autismo { get; set; }
    public bool HepatitisC { get; set; }
    public string OtroDiagnostico { get; set; } = string.Empty;
}