namespace DIGESA.Repositorios.InterfacesCannabis;

public interface IValidator<T>
{
    List<string> Validate(T model);
}