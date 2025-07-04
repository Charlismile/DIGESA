// [ApiController]
// [Route("api/[controller]")]
// public class SolicitudController : ControllerBase
// {
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IMapper _mapper;
//
//     public SolicitudController(IUnitOfWork unitOfWork, IMapper mapper)
//     {
//         _unitOfWork = unitOfWork;
//         _mapper = mapper;
//     }
//
//     [HttpGet]
//     public async Task<IActionResult> GetAll()
//     {
//         var solicitudes = await _unitOfWork.Solicitud.GetAllAsync();
//         return Ok(solicitudes);
//     }
//
//     [HttpGet("{id}")]
//     public async Task<IActionResult> GetById(int id)
//     {
//         var solicitud = await _unitOfWork.Solicitud.GetByIdAsync(id);
//         if (solicitud == null) return NotFound();
//
//         return Ok(solicitud);
//     }
//
//     [HttpPost]
//     public async Task<IActionResult> Create(SolicitudDTO dto)
//     {
//         if (!ModelState.IsValid) return BadRequest(ModelState);
//
//         var solicitud = _mapper.Map<Solicitud>(dto);
//         await _unitOfWork.Solicitud.AddAsync(solicitud);
//         await _unitOfWork.CompleteAsync();
//
//         return CreatedAtAction(nameof(GetById), new { id = solicitud.Id }, solicitud);
//     }
//
//     [HttpPut("{id}")]
//     public async Task<IActionResult> Update(int id, SolicitudDTO dto)
//     {
//         if (id != dto.Id) return BadRequest("ID no coincide.");
//         if (!ModelState.IsValid) return BadRequest(ModelState);
//
//         var solicitudExistente = await _unitOfWork.Solicitud.GetByIdAsync(id);
//         if (solicitudExistente == null) return NotFound();
//
//         _mapper.Map(dto, solicitudExistente);
//         await _unitOfWork.Solicitud.UpdateAsync(solicitudExistente);
//         await _unitOfWork.CompleteAsync();
//
//         return NoContent();
//     }
//
//     [HttpDelete("{id}")]
//     public async Task<IActionResult> Delete(int id)
//     {
//         var solicitud = await _unitOfWork.Solicitud.GetByIdAsync(id);
//         if (solicitud == null) return NotFound();
//
//         await _unitOfWork.Solicitud.DeleteAsync(id);
//         await _unitOfWork.CompleteAsync();
//
//         return NoContent();
//     }
// }