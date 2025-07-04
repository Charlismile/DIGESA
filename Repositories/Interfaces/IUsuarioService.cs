namespace DIGESA.Repositories.Interfaces;

public interface IUsuarioService
{
    Task<int> RegistrarUsuarioAsync(UsuarioDTO dto);
}