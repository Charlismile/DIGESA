namespace DIGESA.Repositorios.Interfaces;

public interface IDatabaseProvider
{
    public string GetConnectionString();
    public bool GetEnvironment();
}