using DIGESA.DTOs;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DIGESA.Components.Componente;

public partial class Solicitud : ComponentBase
{
    [Inject] public ISolicitudService SolicitudService { get; set; } = default!;
    [Inject] private ICommon _CommonService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private RegistroDto model = new();
    private List<UbicacionDto> regiones = new();
    private List<UbicacionDto> provincias = new();
    private List<UbicacionDto> distritos = new();
    private List<UbicacionDto> corregimientos = new();
    private List<UbicacionDto> instalaciones = new();
    
    private readonly Dictionary<string, string> condicionesMedicas = new()
    {
        {"alzheimer", "Alzheimer"},
        {"anorexia", "Anorexia"},
        {"arthritis", "Artritis"},
        {"autism", "Autismo"},
        {"cancer", "Cáncer"},
        {"depression", "Depresión"},
        {"ibd", "Enfermedad inflamatoria intestinal"},
        {"palliative", "Enfermedad incurable que requiera cuidado paliativo"},
        {"degenerative", "Enfermedades Degenerativas"},
        {"epilepsy", "Epilepsia"},
        {"fibromyalgia", "Fibromialgia"},
        {"glaucoma", "Glaucoma"},
        {"hepatitis", "Hepatitis C"},
        {"insomnia", "Insomnio"},
        {"spinal", "Lesiones de cordón espinal"},
        {"neuropathy", "Neuropatía periférica"},
        {"parkinson", "Parkinson"},
        {"hiv", "Relacionado a VIH positivo"},
        {"aids", "Síndrome de Inmunodeficiencia Adquirida (SIDA)"},
        {"ptsd", "Síndrome de Estrés Postraumático"},
        {"bipolar", "Trastorno Bipolar"},
        {"anxiety", "Trastornos de ansiedad"}
    };

    private readonly Dictionary<string, string> formasFarmaceuticas = new()
    {
        {"oils", "Aceites"},
        {"gel-caps", "Cápsulas de gel"},
        {"tablets", "Comprimido"},
        {"cream", "Crema"},
        {"extracts", "Extractos"},
        {"flower", "Flor procesada"},
        {"inhalers", "Inhaladores orales"},
        {"patches", "Parches transdérmicos"},
        {"pills", "Píldoras"},
        {"ointment", "Ungüento"},
        {"suppository", "Supositorio"},
        {"tinctures", "Tinturas/gotas orales"}
    };

    private readonly Dictionary<string, string> viasAdministracion = new()
    {
        {"oral", "Oral (deglutida)"},
        {"sublingual", "Sublingual"},
        {"inhaled", "Inhalada"},
        {"rectal", "Rectal"},
        {"topical", "Tópica (piel, oftálmica, ótica)"},
        {"subcutaneous", "Subcutánea"},
        {"intravenous", "Intravenosa"},
        {"intramuscular", "Intramuscular"},
        {"intradermal", "Intradérmica"}
    };

    private readonly string[] unidadesMedida = { "g", "mg", "ml", "mg/ml", "%" };

    protected override async Task OnInitializedAsync()
    {
        model.FechaSolicitud = DateTime.Today;
        model.TipoDocumento = "cedula";
        model.AcompananteTipoDocumento = "cedula";
        model.MedicoEspecialidad = "general";
        model.UnidadCBD = "mg";
        model.UnidadTHC = "mg";
        model.VecesAlDia = 1;

        regiones = (await _CommonService.ObtenerRegionesAsync())
            .Select(x => new UbicacionDto { Id = x.Id, Nombre = x.Nombre }).ToList();

        provincias = (await _CommonService.GetProvincias())
            .Select(x => new UbicacionDto { Id = x.Id, Nombre = x.Nombre }).ToList();

        instalaciones = (await _CommonService.ObtenerInstalacionesSaludAsync())
            .Select(x => new UbicacionDto { Id = x.Id, Nombre = x.Nombre }).ToList();

    }

    private async Task CargarDistritos(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int provinciaId))
        {
            model.PId = provinciaId;
            distritos = await _CommonService.GetDistritosPorProvincia();
            corregimientos.Clear();
            model.DId = null;
            model.CId = null;
        }
    }

    private async Task CargarCorregimientos(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int distritoId))
        {
            model.DId = distritoId;
            corregimientos = await _CommonService.GetCorregimientos(distritoId);
            model.CId = null;
        }
    }

    private void ToggleCondition(string conditionKey, bool isChecked)
    {
        if (isChecked)
        {
            if (!model.CondicionesMedicas.Contains(conditionKey))
                model.CondicionesMedicas.Add(conditionKey);
        }
        else
        {
            model.CondicionesMedicas.Remove(conditionKey);
        }
    }

    private void ToggleFormaFarmaceutica(string formaKey, bool isChecked)
    {
        if (isChecked)
        {
            if (!model.FormasFarmaceuticas.Contains(formaKey))
                model.FormasFarmaceuticas.Add(formaKey);
        }
        else
        {
            model.FormasFarmaceuticas.Remove(formaKey);
        }
    }

    private void ToggleViaAdministracion(string viaKey, bool isChecked)
    {
        if (isChecked)
        {
            if (!model.ViasAdministracion.Contains(viaKey))
                model.ViasAdministracion.Add(viaKey);
        }
        else
        {
            model.ViasAdministracion.Remove(viaKey);
        }
    }

    private async Task HandleValidSubmit()
    {
        try
        {
            // Aquí puedes agregar la lógica para enviar los datos a tu API/servicio
            // Por ejemplo: await PacienteService.RegistrarPaciente(model);
            
            // Simulación de envío
            await Task.Delay(1000);
            
            // Mostrar mensaje de éxito (puedes usar un componente de notificación)
            await Task.Run(() => Console.WriteLine("Formulario enviado exitosamente"));
            
            // Opcional: Limpiar el formulario o redirigir
            // NavigationManager.NavigateTo("/confirmacion");
        }
        catch (Exception ex)
        {
            // Manejar errores
            Console.WriteLine($"Error al enviar formulario: {ex.Message}");
        }
    }
    
    private List<CheckboxModel> checkboxes = new()
    {
        new CheckboxModel { Etiqueta = "CBD (Cannabidiol)" }
    };

    private void AgregarCheckbox()
    {
        if (checkboxes.Count < 3)
        {
            // Puedes personalizar etiquetas aquí si quieres variar cada checkbox
            checkboxes.Add(new CheckboxModel { Etiqueta = $"Otro Cannabinoide #{checkboxes.Count + 1}" });
        }
    }

    public class CheckboxModel
    {
        public string Etiqueta { get; set; } = string.Empty;
        public bool Seleccionado { get; set; } = false;
    }
}