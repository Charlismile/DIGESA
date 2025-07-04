// using AutoMapper;
// using DIGESA.Models.Entities.DBDIGESA;
// using DIGESA.Repositories.Interfaces;
// using Microsoft.AspNetCore.Mvc;
//
// [ApiController]
// [Route("api/[controller]")]
// public class TratamientoController : ControllerBase
// {
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IMapper _mapper;
//
//     public TratamientoController(IUnitOfWork unitOfWork, IMapper mapper)
//     {
//         _unitOfWork = unitOfWork;
//         _mapper = mapper;
//     }
//
//     [HttpGet("{solicitudId}")]
//     public async Task<IActionResult> GetBySolicitud(int solicitudId)
//     {
//         var tratamiento = await _unitOfWork.Tratamiento.GetBySolicitudAsync(solicitudId);
//         if (tratamiento == null) return NotFound();
//
//         return Ok(tratamiento);
//     }
//
//     [HttpPost]
//     public async Task<IActionResult> Create(TratamientoDTO dto)
//     {
//         if (!ModelState.IsValid) return BadRequest(ModelState);
//
//         var tratamiento = _mapper.Map<Tratamiento>(dto);
//         await _unitOfWork.Tratamiento.AddAsync(tratamiento);
//         await _unitOfWork.CompleteAsync();
//
//         return CreatedAtAction(nameof(GetBySolicitud), new { solicitudId = tratamiento.SolicitudId }, tratamiento);
//     }
//
//     [HttpPut("{id}")]
//     public async Task<IActionResult> Update(int id, TratamientoDTO dto)
//     {
//         if (!ModelState.IsValid) return BadRequest(ModelState);
//
//         var tratamientoExistente = await _unitOfWork.Tratamiento.GetByIdAsync(id);
//         if (tratamientoExistente == null) return NotFound();
//
//         _mapper.Map(dto, tratamientoExistente);
//         await _unitOfWork.Tratamiento.UpdateAsync(tratamientoExistente);
//         await _unitOfWork.CompleteAsync();
//
//         return NoContent();
//     }
//
//     [HttpDelete("{id}")]
//     public async Task<IActionResult> Delete(int id)
//     {
//         var tratamiento = await _unitOfWork.Tratamiento.GetByIdAsync(id);
//         if (tratamiento == null) return NotFound();
//
//         await _unitOfWork.Tratamiento.DeleteAsync(id);
//         await _unitOfWork.CompleteAsync();
//
//         return NoContent();
//     }
// }