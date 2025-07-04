// using AutoMapper;
// using DIGESA.Models.Entities.DBDIGESA;
// using DIGESA.Repositories.Interfaces;
// using Microsoft.AspNetCore.Mvc;
//
// [ApiController]
// [Route("api/usuario")]
// public class UsuarioController : ControllerBase
// {
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IMapper _mapper;
//
//     public UsuarioController(IUnitOfWork unitOfWork, IMapper mapper)
//     {
//         _unitOfWork = unitOfWork;
//         _mapper = mapper;
//     }
//
//     [HttpGet]
//     public async Task<IActionResult> GetAll()
//     {
//         var usuarios = await _unitOfWork.Usuario.GetAllAsync();
//         return Ok(usuarios);
//     }
//
//     [HttpGet("{id}")]
//     public async Task<IActionResult> GetById(int id)
//     {
//         var usuario = await _unitOfWork.Usuario.GetByIdAsync(id);
//         if (usuario == null) return NotFound();
//
//         return Ok(usuario);
//     }
//
//     [HttpPost]
//     public async Task<IActionResult> Create(UsuarioDTO dto)
//     {
//         if (!ModelState.IsValid) return BadRequest(ModelState);
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
//         return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
//     }
//
//     private void HashPassword(string password, out byte[] hash, out byte[] salt)
//     {
//         using var hmac = new System.Security.Cryptography.HMACSHA512();
//         salt = hmac.Key;
//         hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
//     }
// }