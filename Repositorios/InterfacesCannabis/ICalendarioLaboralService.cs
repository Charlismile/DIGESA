namespace DIGESA.Repositorios.InterfacesCannabis;

public interface ICalendarioLaboralService
{
    DateTime SumarDiasHabiles(DateTime fecha, int dias);
    int CalcularDiasHabiles(DateTime desde, DateTime hasta);
    bool EsDiaHabil(DateTime fecha);
}
