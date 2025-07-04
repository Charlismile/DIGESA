// [ApiController]
// [Route("api/decisionrevision")]
// public class DecisionRevisionController : ControllerBase
// {
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IMapper _mapper;
//
//     public DecisionRevisionController(IUnitOfWork unitOfWork, IMapper mapper)
//     {
//         _unitOfWork = unitOfWork;
//         _mapper = mapper;
//     }
//
//     [HttpGet]
//     public async Task<IActionResult> GetAll()
//     {
//         var decisiones = await _unitOfWork.DecisionRevision.GetAllAsync();
//         return Ok(decisiones);
//     }
//
//     [HttpGet("{id}")]
//     public async Task<IActionResult> GetById(int id)
//     {
//         var decision = await _unitOfWork.DecisionRevision.GetByIdAsync(id);
//         if (decision == null) return NotFound();
//
//         return Ok(decision);
//     }
//
//     [HttpPost]
//     public async Task<IActionResult> Create(DecisionRevisionDTO dto)
//     {
//         if (!ModelState.IsValid) return BadRequest(ModelState);
//
//         var decision = _mapper.Map<DecisionRevision>(dto);
//         await _unitOfWork.DecisionRevision.AddAsync(decision);
//         await _unitOfWork.CompleteAsync();
//
//         return CreatedAtAction(nameof(GetById), new { id = decision.Id }, decision);
//     }
//
//     [HttpPut("{id}")]
//     public async Task<IActionResult> Update(int id, DecisionRevisionDTO dto)
//     {
//         if (id != dto.Id) return BadRequest("ID no coincide.");
//         if (!ModelState.IsValid) return BadRequest(ModelState);
//
//         var decisionExistente = await _unitOfWork.DecisionRevision.GetByIdAsync(id);
//         if (decisionExistente == null) return NotFound();
//
//         _mapper.Map(dto, decisionExistente);
//         await _unitOfWork.DecisionRevision.UpdateAsync(decisionExistente);
//         await _unitOfWork.CompleteAsync();
//
//         return NoContent();
//     }
//
//     [HttpDelete("{id}")]
//     public async Task<IActionResult> Delete(int id)
//     {
//         var decision = await _unitOfWork.DecisionRevision.GetByIdAsync(id);
//         if (decision == null) return NotFound();
//
//         await _unitOfWork.DecisionRevision.DeleteAsync(id);
//         await _unitOfWork.CompleteAsync();
//
//         return NoContent();
//     }
// }