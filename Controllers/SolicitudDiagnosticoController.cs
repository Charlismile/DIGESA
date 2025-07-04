// [ApiController]
// [Route("api/solicituddiagnostico")]
// public class SolicitudDiagnosticoController : ControllerBase
// {
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IMapper _mapper;
//
//     public SolicitudDiagnosticoController(IUnitOfWork unitOfWork, IMapper mapper)
//     {
//         _unitOfWork = unitOfWork;
//         _mapper = mapper;
//     }
//
//     [HttpGet("{solicitudId}")]
//     public async Task<IActionResult> GetDiagnosticos(int solicitudId)
//     {
//         var diagnosticos = await _unitOfWork.SolicitudDiagnostico.GetBySolicitudAsync(solicitudId);
//         return Ok(diagnosticos);
//     }
//
//     [HttpPost]
//     public async Task<IActionResult> AddDiagnostico(SolicitudDiagnosticoDTO dto)
//     {
//         if (!ModelState.IsValid) return BadRequest(ModelState);
//
//         var diag = _mapper.Map<SolicitudDiagnostico>(dto);
//         await _unitOfWork.SolicitudDiagnostico.AddAsync(diag);
//         await _unitOfWork.CompleteAsync();
//
//         return CreatedAtAction(nameof(GetDiagnosticos), new { solicitudId = dto.SolicitudId }, diag);
//     }
//
//     [HttpDelete("{solicitudId}/{diagnosticoId}")]
//     public async Task<IActionResult> RemoveDiagnostico(int solicitudId, int diagnosticoId)
//     {
//         await _unitOfWork.SolicitudDiagnostico.DeleteAsync(solicitudId, diagnosticoId);
//         await _unitOfWork.CompleteAsync();
//
//         return NoContent();
//     }
// }