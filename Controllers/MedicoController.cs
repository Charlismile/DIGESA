// [ApiController]
// [Route("api/[controller]")]
// public class MedicoController : ControllerBase
// {
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IMapper _mapper;
//
//     public MedicoController(IUnitOfWork unitOfWork, IMapper mapper)
//     {
//         _unitOfWork = unitOfWork;
//         _mapper = mapper;
//     }
//
//     [HttpGet]
//     public async Task<IActionResult> GetAll()
//     {
//         var medicos = await _unitOfWork.Medico.GetAllAsync();
//         return Ok(medicos);
//     }
//
//     [HttpGet("{id}")]
//     public async Task<IActionResult> GetById(int id)
//     {
//         var medico = await _unitOfWork.Medico.GetByIdAsync(id);
//         if (medico == null) return NotFound();
//
//         return Ok(medico);
//     }
//
//     [HttpPost]
//     public async Task<IActionResult> Create(MedicoDTO dto)
//     {
//         if (!ModelState.IsValid) return BadRequest(ModelState);
//
//         var medico = _mapper.Map<Medico>(dto);
//         await _unitOfWork.Medico.AddAsync(medico);
//         await _unitOfWork.CompleteAsync();
//
//         return CreatedAtAction(nameof(GetById), new { id = medico.Id }, medico);
//     }
//
//     [HttpPut("{id}")]
//     public async Task<IActionResult> Update(int id, MedicoDTO dto)
//     {
//         if (id != dto.Id) return BadRequest("ID no coincide.");
//         if (!ModelState.IsValid) return BadRequest(ModelState);
//
//         var medicoExistente = await _unitOfWork.Medico.GetByIdAsync(id);
//         if (medicoExistente == null) return NotFound();
//
//         _mapper.Map(dto, medicoExistente);
//         await _unitOfWork.Medico.UpdateAsync(medicoExistente);
//         await _unitOfWork.CompleteAsync();
//
//         return NoContent();
//     }
//
//     [HttpDelete("{id}")]
//     public async Task<IActionResult> Delete(int id)
//     {
//         var medico = await _unitOfWork.Medico.GetByIdAsync(id);
//         if (medico == null) return NotFound();
//
//         await _unitOfWork.Medico.DeleteAsync(id);
//         await _unitOfWork.CompleteAsync();
//
//         return NoContent();
//     }
// }