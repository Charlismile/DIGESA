// using DIGESA.Repositorios.InterfacesCannabis;
//
// namespace DIGESA.Repositorios.ServiciosCannabis;
//
// public class CarnetVigenciaService : ICarnetVigenciaService
// {
//     private readonly ICalendarioLaboralService _calendario;
//
//     public CarnetVigenciaService(ICalendarioLaboralService calendario)
//     {
//         _calendario = calendario;
//     }
//
//     public DateTime CalcularFechaVencimiento(DateTime fechaEmision)
//         => fechaEmision.AddYears(2);
//
//     public DateTime CalcularFechaAlerta(DateTime fechaVencimiento)
//         => _calendario.RestarDiasHabiles(fechaVencimiento, 30);
// }
