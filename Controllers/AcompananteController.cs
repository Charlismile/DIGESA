// [ApiController]
// [Route("api/[controller]")]
// public class AcompananteController : ControllerBase
// {
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IMapper _mapper;
//
//     public AcompananteController(IUnitOfWork unitOfWork, IMapper mapper)
//     {
//         _unitOfWork = unitOfWork;
//         _mapper = mapper;
//     }
//
//     [HttpGet]
//     public async Task<IActionResult> GetAll()
//     {
//         var acompanantes = await _unitOfWork.Acompanante.GetAllAsync();
//         return Ok(acompanantes);
//     }
//
//     [HttpGet("{id}")]
//     public async Task<IActionResult> GetById(int id)
//     {
//         var acompanante = await _unitOfWork.Acompanante.GetByIdAsync(id);
//         if (acompanante == null) return NotFound();
//
//         return Ok(acompanante);
//     }
//
//     [HttpPost]
//     public async Task<IActionResult> Create(AcompananteDTO dto)
//     {
//         if (!ModelState.IsValid) return BadRequest(ModelState);
//
//         var acompanante = _mapper.Map<Acompanante>(dto);
//         await _unitOfWork.Acompanante.AddAsync(acompanante);
//         await _unitOfWork.CompleteAsync();
//
//         return CreatedAtAction(nameof(GetById), new { id = acompanante.Id }, acompanante);
//     }
//
//     [HttpPut("{id}")]
//     public async Task<IActionResult> Update(int id, AcompananteDTO dto)
//     {
//         if (id != dto.Id) return BadRequest("ID no coincide.");
//         if (!ModelState.IsValid) return BadRequest(ModelState);
//
//         var acompananteExistente = await _unitOfWork.Acompanante.GetByIdAsync(id);
//         if (acompananteExistente == null) return NotFound();
//
//         _mapper.Map(dto, acompananteExistente);
//         await _unitOfWork.Acompanante.UpdateAsync(acompananteExistente);
//         await _unitOfWork.CompleteAsync();
//
//         return NoContent();
//     }
//
//     [HttpDelete("{id}")]
//     public async Task<IActionResult> Delete(int id)
//     {
//         var acompanante = await _unitOfWork.Acompanante.GetByIdAsync(id);
//         if (acompanante == null) return NotFound();
//
//         await _unitOfWork.Acompanante.DeleteAsync(id);
//         await _unitOfWork.CompleteAsync();
//
//         return NoContent();
//     }
// }