namespace DIGESA.Repositorios.InterfacesCannabis;

public interface ICarnetVigenciaService
{
    DateTime CalcularFechaVencimiento(DateTime fechaEmision);
    DateTime CalcularFechaAlerta(DateTime fechaVencimiento);
}
