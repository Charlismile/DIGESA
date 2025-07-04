// public class UsuarioService : IUsuarioService
// {
//     private readonly IUnitOfWork _unitOfWork;
//
//     public UsuarioService(IUnitOfWork unitOfWork)
//     {
//         _unitOfWork = unitOfWork;
//     }
//
//     public async Task<int> RegistrarUsuarioAsync(UsuarioDTO dto)
//     {
//         var existingUser = await _unitOfWork.Usuario.GetByEmailAsync(dto.Email);
//         if (existingUser != null)
//             throw new Exception("El correo ya está registrado.");
//
//         byte[] passwordHash, salt;
//         HashPassword(dto.Contraseña, out passwordHash, out salt);
//
//         var usuario = _mapper.Map<Usuario>(dto);
//         usuario.ContraseñaHash = passwordHash;
//         usuario.Salt = salt;
//
//         await _unitOfWork.Usuario.AddAsync(usuario);
//         await _unitOfWork.CompleteAsync();
//
//         return usuario.Id;
//     }
//
//     private void HashPassword(string password, out byte[] hash, out byte[] salt)
//     {
//         using var hmac = new System.Security.Cryptography.HMACSHA512();
//         salt = hmac.Key;
//         hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
//     }
// }