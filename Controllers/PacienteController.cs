// using AutoMapper;
// using DIGESA.Models.Entities.DBDIGESA;
// using DIGESA.Repositories.Interfaces;
// using Microsoft.AspNetCore.Mvc;
//
// [ApiController]
// [Route("api/[controller]")]
// public class PacienteController : ControllerBase
// {
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IMapper _mapper;
//
//     public PacienteController(IUnitOfWork unitOfWork, IMapper mapper)
//     {
//         _unitOfWork = unitOfWork;
//         _mapper = mapper;
//     }
//
//     [HttpGet]
//     public async Task<IActionResult> GetAll()
//     {
//         var pacientes = await _unitOfWork.Paciente.GetAllAsync();
//         return Ok(pacientes);
//     }
//
//     [HttpGet("{id}")]
//     public async Task<IActionResult> GetById(int id)
//     {
//         var paciente = await _unitOfWork.Paciente.GetByIdAsync(id);
//         if (paciente == null) return NotFound();
//
//         return Ok(paciente);
//     }
//
//     [HttpPost]
//     public async Task<IActionResult> Create(PacienteDTO dto)
//     {
//         if (!ModelState.IsValid) return BadRequest(ModelState);
//
//         var paciente = _mapper.Map<Paciente>(dto);
//         await _unitOfWork.Paciente.AddAsync(paciente);
//         await _unitOfWork.CompleteAsync();
//
//         return CreatedAtAction(nameof(GetById), new { id = paciente.Id }, paciente);
//     }
//
//     [HttpPut("{id}")]
//     public async Task<IActionResult> Update(int id, PacienteDTO dto)
//     {
//         if (id != dto.Id) return BadRequest("ID no coincide.");
//         if (!ModelState.IsValid) return BadRequest(ModelState);
//
//         var pacienteExistente = await _unitOfWork.Paciente.GetByIdAsync(id);
//         if (pacienteExistente == null) return NotFound();
//
//         _mapper.Map(dto, pacienteExistente);
//         await _unitOfWork.Paciente.UpdateAsync(pacienteExistente);
//         await _unitOfWork.CompleteAsync();
//
//         return NoContent();
//     }
//
//     [HttpDelete("{id}")]
//     public async Task<IActionResult> Delete(int id)
//     {
//         var paciente = await _unitOfWork.Paciente.GetByIdAsync(id);
//         if (paciente == null) return NotFound();
//
//         await _unitOfWork.Paciente.DeleteAsync(id);
//         await _unitOfWork.CompleteAsync();
//
//         return NoContent();
//     }
// }