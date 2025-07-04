// [ApiController]
// [Route("api/estadosolicitud")]
// public class EstadoSolicitudController : ControllerBase
// {
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IMapper _mapper;
//
//     public EstadoSolicitudController(IUnitOfWork unitOfWork, IMapper mapper)
//     {
//         _unitOfWork = unitOfWork;
//         _mapper = mapper;
//     }
//
//     [HttpGet]
//     public async Task<IActionResult> GetAll()
//     {
//         var estados = await _unitOfWork.EstadoSolicitud.GetAllAsync();
//         return Ok(estados);
//     }
//
//     [HttpGet("{id}")]
//     public async Task<IActionResult> GetById(int id)
//     {
//         var estado = await _unitOfWork.EstadoSolicitud.GetByIdAsync(id);
//         if (estado == null) return NotFound();
//
//         return Ok(estado);
//     }
//
//     [HttpPost]
//     public async Task<IActionResult> Create(EstadoSolicitudDTO dto)
//     {
//         if (!ModelState.IsValid) return BadRequest(ModelState);
//
//         var estado = _mapper.Map<EstadoSolicitud>(dto);
//         await _unitOfWork.EstadoSolicitud.AddAsync(estado);
//         await _unitOfWork.CompleteAsync();
//
//         return CreatedAtAction(nameof(GetById), new { id = estado.Id }, estado);
//     }
//
//     [HttpPut("{id}")]
//     public async Task<IActionResult> Update(int id, EstadoSolicitudDTO dto)
//     {
//         if (id != dto.Id) return BadRequest("ID no coincide.");
//         if (!ModelState.IsValid) return BadRequest(ModelState);
//
//         var estadoExistente = await _unitOfWork.EstadoSolicitud.GetByIdAsync(id);
//         if (estadoExistente == null) return NotFound();
//
//         _mapper.Map(dto, estadoExistente);
//         await _unitOfWork.EstadoSolicitud.UpdateAsync(estadoExistente);
//         await _unitOfWork.CompleteAsync();
//
//         return NoContent();
//     }
//
//     [HttpDelete("{id}")]
//     public async Task<IActionResult> Delete(int id)
//     {
//         var estado = await _unitOfWork.EstadoSolicitud.GetByIdAsync(id);
//         if (estado == null) return NotFound();
//
//         await _unitOfWork.EstadoSolicitud.DeleteAsync(id);
//         await _unitOfWork.CompleteAsync();
//
//         return NoContent();
//     }
// }