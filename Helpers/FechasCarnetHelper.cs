namespace DIGESA.Helpers;

public static class FechasCarnetHelper
{
    private const int VigenciaAnios = 2;
    private const int DiasHabilesAlerta = 30;

    public static DateTime CalcularFechaVencimiento(DateTime fechaEmision)
    {
        return fechaEmision.AddYears(VigenciaAnios).Date;
    }

    public static bool EstaVencido(DateTime fechaVencimiento)
    {
        return DateTime.Today >= fechaVencimiento.Date;
    }

    public static bool EstaProximoAVencer(DateTime fechaVencimiento)
    {
        if (EstaVencido(fechaVencimiento))
            return false;

        var hoy = DateTime.Today;
        int diasHabiles = 0;
        var fechaIteracion = hoy;

        while (fechaIteracion < fechaVencimiento)
        {
            if (fechaIteracion.DayOfWeek != DayOfWeek.Saturday &&
                fechaIteracion.DayOfWeek != DayOfWeek.Sunday)
            {
                diasHabiles++;
            }

            if (diasHabiles > DiasHabilesAlerta)
                return false;

            fechaIteracion = fechaIteracion.AddDays(1);
        }

        return diasHabiles <= DiasHabilesAlerta;
    }
    public static int CalcularEdad(DateTime fechaNacimiento)
    {
        var hoy = DateTime.Today;
        var edad = hoy.Year - fechaNacimiento.Year;

        if (fechaNacimiento.Date > hoy.AddYears(-edad))
            edad--;

        return edad;
    }

}