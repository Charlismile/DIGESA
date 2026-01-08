using DIGESA.Repositorios.InterfacesCannabis;

namespace DIGESA.Repositorios.ServiciosCannabis;

public class CalendarioLaboralService : ICalendarioLaboralService
{
    private static readonly HashSet<DateTime> FeriadosPanama = new()
    {
        new DateTime(2026, 1, 1),   // Año Nuevo
        new DateTime(2026, 1, 9),   // Mártires
        new DateTime(2026, 5, 1),   // Día del Trabajador
        new DateTime(2026, 11, 3),  // Separación
        new DateTime(2026, 11, 4),
        new DateTime(2026, 11, 5),
        new DateTime(2026, 11, 10),
        new DateTime(2026, 11, 28),
        new DateTime(2026, 12, 8),
        new DateTime(2026, 12, 25)
    };

    public bool EsDiaHabil(DateTime fecha)
    {
        return fecha.DayOfWeek != DayOfWeek.Saturday &&
               fecha.DayOfWeek != DayOfWeek.Sunday &&
               !FeriadosPanama.Contains(fecha.Date);
    }

    public DateTime SumarDiasHabiles(DateTime fecha, int dias)
    {
        var resultado = fecha;
        var contador = 0;

        while (contador < dias)
        {
            resultado = resultado.AddDays(1);
            if (EsDiaHabil(resultado))
                contador++;
        }

        return resultado;
    }

    public int CalcularDiasHabiles(DateTime desde, DateTime hasta)
    {
        var dias = 0;
        var fecha = desde.Date;

        while (fecha < hasta.Date)
        {
            fecha = fecha.AddDays(1);
            if (EsDiaHabil(fecha))
                dias++;
        }

        return dias;
    }
}
