// using DIGESA.Models.CannabisModels;
// using DIGESA.Repositorios.Interfaces;
// using Microsoft.AspNetCore.Components;
// using Microsoft.AspNetCore.Components.Web;
//
// namespace DIGESA.Components.Pages.Public;
//
// public partial class Verificacion : ComponentBase
// {
//     [Inject]
//     public IPaciente PacienteService { get; set; } = default!;
//
//     private string documentoBusqueda = string.Empty;
//     private EstadoSolicitudViewModel? pacienteEstado;
//     private PacienteViewModel? pacienteDetalle;
//     private bool busquedaRealizada;
//     private bool pacienteEncontrado;
//     private bool loading;
//
//     private async Task HandleKeyPress(KeyboardEventArgs e)
//     {
//         if (e.Key == "Enter")
//         {
//             await BuscarPaciente();
//         }
//     }
//
//     private async Task BuscarPaciente()
//     {
//         if (string.IsNullOrWhiteSpace(documentoBusqueda))
//             return;
//
//         loading = true;
//         busquedaRealizada = false;
//         pacienteEncontrado = false;
//         pacienteEstado = null;
//         pacienteDetalle = null;
//
//         try
//         {
//             pacienteEstado = await PacienteService.GetEstadoPacienteAsync(documentoBusqueda);
//             pacienteDetalle = await PacienteService.BuscarPorDocumentoAsync(documentoBusqueda);
//             pacienteEncontrado = pacienteEstado is not null && !string.IsNullOrEmpty(pacienteEstado.Documento);
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"Error al buscar paciente: {ex.Message}");
//         }
//         finally
//         {
//             loading = false;
//             busquedaRealizada = true;
//         }
//     }
//
//     // Métodos auxiliares para mostrar texto amigable
//     private string GetNombreCompleto()
//     {
//         if (pacienteDetalle == null) return string.Empty;
//         
//         var nombres = new List<string> 
//         { 
//             pacienteDetalle.PrimerNombre ?? "",
//             pacienteDetalle.SegundoNombre ?? "",
//             pacienteDetalle.PrimerApellido ?? "",
//             pacienteDetalle.SegundoApellido ?? ""
//         };
//         
//         return string.Join(" ", nombres.Where(n => !string.IsNullOrEmpty(n)));
//     }
//
//     private string GetTipoDocumentoTexto()
//     {
//         return pacienteDetalle?.TipoDocumento switch
//         {
//             TipoDocumento.Cedula => "Cédula",
//             TipoDocumento.Pasaporte => "Pasaporte",
//             _ => "No especificado"
//         };
//     }
//
//     private string GetSexoTexto()
//     {
//         return pacienteDetalle?.Sexo switch
//         {
//             Sexo.Masculino => "Masculino",
//             Sexo.Femenino => "Femenino",
//             _ => "No especificado"
//         };
//     }
//
//     private string GetMotivoAcompananteTexto()
//     {
//         return pacienteDetalle?.MotivoRequerimientoAcompanante switch
//         {
//             MotivoRequerimientoAcompanante.PacienteMenorEdad => "Paciente menor de edad",
//             MotivoRequerimientoAcompanante.PacienteDiscapacidad => "Paciente con discapacidad",
//             _ => "No especificado"
//         };
//     }
// }