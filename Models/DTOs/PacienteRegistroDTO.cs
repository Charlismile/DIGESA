public class PacienteRegistroDTO
{
    // Datos personales del paciente
    public string NombreCompleto { get; set; }
    public string TipoDocumento { get; set; } // Cédula, Pasaporte
    public string NumeroDocumento { get; set; }
    public string Nacionalidad { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public string Sexo { get; set; } // Femenino, Masculino
    public string DireccionResidencia { get; set; }

    // Contacto
    public string TelefonoResidencial { get; set; }
    public string TelefonoPersonal { get; set; }
    public string TelefonoLaboral { get; set; }
    public string CorreoElectronico { get; set; }

    // Instalación de salud
    public string InstalacionSalud { get; set; }
    public string RegionSalud { get; set; }

    // Acompañante
    public bool RequiereAcompanante { get; set; }
    public AcompananteDTO Acompanante { get; set; }

    // Médico tratante
    public MedicoDTO Medico { get; set; }

    // Diagnósticos
    public DiagnosticosDTO Diagnostico { get; set; }

    // Otras enfermedades
    public OtrasEnfermedadesDTO OtrasEnfermedades { get; set; }

    // Tratamiento
    public TerapiaDTO Terapia { get; set; }

    // Validación de negocio
    public List<string> Validate()
    {
        var errores = new List<string>();

        if (string.IsNullOrWhiteSpace(NombreCompleto))
            errores.Add("El nombre completo del paciente es obligatorio.");

        if (string.IsNullOrWhiteSpace(TipoDocumento))
            errores.Add("Debe seleccionar un tipo de documento.");

        if (string.IsNullOrWhiteSpace(NumeroDocumento))
            errores.Add("El número de documento es obligatorio.");

        if (!FechaNacimiento.HasValue)
            errores.Add("La fecha de nacimiento es obligatoria.");
        else if (FechaNacimiento.Value.Date > DateTime.Now.Date)
            errores.Add("La fecha de nacimiento no puede ser futura.");

        if (string.IsNullOrWhiteSpace(Sexo))
            errores.Add("Debe seleccionar el sexo del paciente.");

        if (string.IsNullOrWhiteSpace(DireccionResidencia))
            errores.Add("La dirección de residencia es obligatoria.");

        if (string.IsNullOrWhiteSpace(InstalacionSalud))
            errores.Add("La instalación de salud donde es atendido es obligatoria.");

        if (string.IsNullOrWhiteSpace(RegionSalud))
            errores.Add("La región de salud es obligatoria.");

        // Validar requerimiento de acompañante
        if (RequiereAcompanante)
        {
            if (Acompanante == null || string.IsNullOrWhiteSpace(Acompanante.NombreCompleto))
                errores.Add("Debe ingresar el nombre completo del acompañante.");
            if (string.IsNullOrWhiteSpace(Acompanante.TipoDocumento))
                errores.Add("Debe seleccionar el tipo de documento del acompañante.");
            if (string.IsNullOrWhiteSpace(Acompanante.NumeroDocumento))
                errores.Add("El número de documento del acompañante es obligatorio.");
            if (string.IsNullOrWhiteSpace(Acompanante.Nacionalidad))
                errores.Add("La nacionalidad del acompañante es obligatoria.");
            if (string.IsNullOrWhiteSpace(Acompanante.Parentesco))
                errores.Add("Debe seleccionar el parentesco del acompañante.");
        }

        // Validar médico tratante
        if (Medico == null)
            errores.Add("Los datos del médico tratante son obligatorios.");
        else
        {
            if (string.IsNullOrWhiteSpace(Medico.NombreCompleto))
                errores.Add("Debe ingresar el nombre completo del médico tratante.");
            if (string.IsNullOrWhiteSpace(Medico.Disciplina))
                errores.Add("Debe seleccionar la disciplina del médico.");
            if (string.IsNullOrWhiteSpace(Medico.RegistroIdoneidad))
                errores.Add("El número de registro de idoneidad del médico es obligatorio.");
            if (string.IsNullOrWhiteSpace(Medico.InstalacionSalud))
                errores.Add("La instalación de salud del médico es obligatoria.");
        }

        // Validar diagnósticos
        if (Diagnostico == null)
            errores.Add("Debe seleccionar al menos un diagnóstico.");
        else
        {
            if (!Diagnostico.AnySelected() && string.IsNullOrWhiteSpace(Diagnostico.OtroDiagnostico))
                errores.Add("Debe seleccionar al menos un diagnóstico o especificar uno personalizado.");
        }

        // Validar tratamiento
        if (Terapia == null)
            errores.Add("Debe proporcionar los datos del tratamiento.");
        else
        {
            if (string.IsNullOrWhiteSpace(Terapia.Dosis))
                errores.Add("La dosis del tratamiento es obligatoria.");
            if (string.IsNullOrWhiteSpace(Terapia.FrecuenciaAdministracion))
                errores.Add("La frecuencia de administración es obligatoria.");
            if (Terapia.DuracionTratamientoDias <= 0)
                errores.Add("La duración del tratamiento debe ser mayor a cero días.");
        }

        return errores;
    }
}

// Clases auxiliares

public class AcompananteDTO
{
    public string NombreCompleto { get; set; }
    public string TipoDocumento { get; set; }
    public string NumeroDocumento { get; set; }
    public string Nacionalidad { get; set; }
    public string Parentesco { get; set; }
}

public class MedicoDTO
{
    public string NombreCompleto { get; set; }
    public string Disciplina { get; set; } // Médico general, Odontólogo, Especialista
    public string RegistroIdoneidad { get; set; }
    public string NumeroTelefono { get; set; }
    public string InstalacionSalud { get; set; }
}

public class DiagnosticosDTO
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

    public string OtroDiagnostico { get; set; }

    public bool AnySelected()
    {
        return Alzheimer || Epilepsia || SIDA || Anorexia || Fibromialgia ||
               Artritis || Glaucoma || EstrésPostraumatico || Autismo || HepatitisC;
    }
}

public class OtrasEnfermedadesDTO
{
    public string Diagnostico { get; set; }
    public string Tratamiento { get; set; }
}

public class TerapiaDTO
{
    public decimal ConcentracionCBD { get; set; }
    public decimal ConcentracionTHC { get; set; }
    public string OtrosCannabinoides { get; set; }
    public string Dosis { get; set; }
    public string FrecuenciaAdministracion { get; set; }
    public int DuracionTratamientoDias { get; set; }
    public string CantidadPrescrita { get; set; }
    public string InstruccionesAdicionales { get; set; }
}