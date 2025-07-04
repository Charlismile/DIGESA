// [ApiController]
// [Route("api/[controller]")]
// public class DiagnosticoController : ControllerBase
// {
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IMapper _mapper;
//
//     public DiagnosticoController(IUnitOfWork unitOfWork, IMapper mapper)
//     {
//         _unitOfWork = unitOfWork;
//         _mapper = mapper;
//     }
//
//     [HttpGet]
//     public async Task<IActionResult> GetAll()
//     {
//         var diagnosticos = await _unitOfWork.Diagnostico.GetAllAsync();
//         return Ok(diagnosticos);
//     }
//
//     [HttpGet("{id}")]
//     public async Task<IActionResult> GetById(int id)
//     {
//         var diagnostico = await _unitOfWork.Diagnostico.GetByIdAsync(id);
//         if (diagnostico == null) return NotFound();
//
//         return Ok(diagnostico);
//     }
//
//     [HttpPost]
//     public async Task<IActionResult> Create(DiagnosticoDTO dto)
//     {
//         if (!ModelState.IsValid) return BadRequest(ModelState);
//
//         var diagnostico = _mapper.Map<Diagnostico>(dto);
//         await _unitOfWork.Diagnostico.AddAsync(diagnostico);
//         await _unitOfWork.CompleteAsync();
//
//         return CreatedAtAction(nameof(GetById), new { id = diagnostico.Id }, diagnostico);
//     }
//
//     [HttpPut("{id}")]
//     public async Task<IActionResult> Update(int id, DiagnosticoDTO dto)
//     {
//         if (id != dto.Id) return BadRequest("ID no coincide.");
//         if (!ModelState.IsValid) return BadRequest(ModelState);
//
//         var diagnosticoExistente = await _unitOfWork.Diagnostico.GetByIdAsync(id);
//         if (diagnosticoExistente == null) return NotFound();
//
//         _mapper.Map(dto, diagnosticoExistente);
//         await _unitOfWork.Diagnostico.UpdateAsync(diagnosticoExistente);
//         await _unitOfWork.CompleteAsync();
//
//         return NoContent();
//     }
//
//     [HttpDelete("{id}")]
//     public async Task<IActionResult> Delete(int id)
//     {
//         var diagnostico = await _unitOfWork.Diagnostico.GetByIdAsync(id);
//         if (diagnostico == null) return NotFound();
//
//         await _unitOfWork.Diagnostico.DeleteAsync(id);
//         await _unitOfWork.CompleteAsync();
//
//         return NoContent();
//     }
// }