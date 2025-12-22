namespace DIGESA.Models.CannabisModels.Medicos;

public class EstadisticasMedicosViewModel
{
    public int TotalMedicos { get; set; }
    public int MedicosVerificados { get; set; }
    public int MedicosActivos { get; set; }
    public int MedicosInactivos { get; set; }
    public int MedicosEspecialistasCannabis { get; set; }

    public int Neurologos { get; set; }
    public int Oncologos { get; set; }
    public int Psiquiatras { get; set; }
    public int MedDolor { get; set; }
    public int Pediatras { get; set; }
    public int Otros { get; set; }

    public int Panama { get; set; }
    public int Colon { get; set; }
    public int Chiriqui { get; set; }
    public int Veraguas { get; set; }
    public int Cocle { get; set; }
    public int BocasDelToro { get; set; }
    public int Herrera { get; set; }
    public int LosSantos { get; set; }
    public int Darien { get; set; }
    public int GunaYala { get; set; }
    public int NgabeBugle { get; set; }
    public int EmberaWounaan { get; set; }

    public int RegistrosEsteMes { get; set; }
    public int RegistrosMesAnterior { get; set; }
    public decimal VariacionPorcentual { get; set; }

    public int TotalPacientesAtendidos { get; set; }
    public decimal PromedioPacientesPorMedico { get; set; }
    public int TopMedicoPacientes { get; set; }
    public string TopMedicoNombre { get; set; }
}