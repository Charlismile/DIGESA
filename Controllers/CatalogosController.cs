// using AutoMapper;
// using DIGESA.Repositories.Interfaces;
// using Microsoft.AspNetCore.Mvc;
//
// [ApiController]
// [Route("api/catalogos")]
// public class CatalogosController : ControllerBase
// {
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IMapper _mapper;
//
//     public CatalogosController(IUnitOfWork unitOfWork, IMapper mapper)
//     {
//         _unitOfWork = unitOfWork;
//         _mapper = mapper;
//     }
//
//     // GET /api/catalogos
//     [HttpGet]
//     public async Task<IActionResult> GetAllCatalogos()
//     {
//         var diagnosticos = await _unitOfWork.Diagnostico.GetAllAsync();
//         var formasFarmaceuticas = await _unitOfWork.FormaFarmaceutica.GetAllAsync();
//         var viasAdministracion = await _unitOfWork.ViaAdministracion.GetAllAsync();
//         var frecuencias = await _unitOfWork.FrecuenciaAdministracion.GetAllAsync();
//         var unidades = await _unitOfWork.UnidadConcentracion.GetAllAsync();
//         var tiposProducto = await _unitOfWork.TipoProducto.GetAllAsync();
//         var estadosSolicitud = await _unitOfWork.EstadoSolicitud.GetAllAsync();
//         var decisionesRevision = await _unitOfWork.DecisionRevision.GetAllAsync();
//         var roles = await _unitOfWork.Rol.GetAllAsync();
//
//         var response = new CatalogosResponseDTO
//         {
//             Diagnosticos = _mapper.Map<List<CatalogoDTO>>(diagnosticos),
//             FormasFarmaceuticas = _mapper.Map<List<CatalogoDTO>>(formasFarmaceuticas),
//             ViasAdministracion = _mapper.Map<List<CatalogoDTO>>(viasAdministracion),
//             FrecuenciasAdministracion = _mapper.Map<List<CatalogoDTO>>(frecuencias),
//             UnidadesConcentracion = _mapper.Map<List<CatalogoDTO>>(unidades),
//             TiposProducto = _mapper.Map<List<CatalogoDTO>>(tiposProducto),
//             EstadosSolicitud = _mapper.Map<List<CatalogoDTO>>(estadosSolicitud),
//             DecisionesRevision = _mapper.Map<List<CatalogoDTO>>(decisionesRevision),
//             Roles = _mapper.Map<List<CatalogoDTO>>(roles)
//         };
//
//         return Ok(response);
//     }
//
//     // GET /api/catalogos/diagnosticos
//     [HttpGet("diagnosticos")]
//     public async Task<IActionResult> GetDiagnosticos()
//     {
//         var diagnosticos = await _unitOfWork.Diagnostico.GetAllAsync();
//         var dtos = _mapper.Map<List<CatalogoDTO>>(diagnosticos);
//         return Ok(dtos);
//     }
//
//     // GET /api/catalogos/formasfarmaceuticas
//     [HttpGet("formasfarmaceuticas")]
//     public async Task<IActionResult> GetFormasFarmaceuticas()
//     {
//         var formas = await _unitOfWork.FormaFarmaceutica.GetAllAsync();
//         var dtos = _mapper.Map<List<CatalogoDTO>>(formas);
//         return Ok(dtos);
//     }
//
//     // GET /api/catalogos/viasadministracion
//     [HttpGet("viasadministracion")]
//     public async Task<IActionResult> GetViasAdministracion()
//     {
//         var vias = await _unitOfWork.ViaAdministracion.GetAllAsync();
//         var dtos = _mapper.Map<List<CatalogoDTO>>(vias);
//         return Ok(dtos);
//     }
//
//     // GET /api/catalogos/frecuencias
//     [HttpGet("frecuencias")]
//     public async Task<IActionResult> GetFrecuenciasAdministracion()
//     {
//         var frecuencias = await _unitOfWork.FrecuenciaAdministracion.GetAllAsync();
//         var dtos = _mapper.Map<List<CatalogoDTO>>(frecuencias);
//         return Ok(dtos);
//     }
//
//     // GET /api/catalogos/unidadesconcentracion
//     [HttpGet("unidadesconcentracion")]
//     public async Task<IActionResult> GetUnidadesConcentracion()
//     {
//         var unidades = await _unitOfWork.UnidadConcentracion.GetAllAsync();
//         var dtos = _mapper.Map<List<CatalogoDTO>>(unidades);
//         return Ok(dtos);
//     }
//
//     // GET /api/catalogos/tiposproducto
//     [HttpGet("tiposproducto")]
//     public async Task<IActionResult> GetTiposProducto()
//     {
//         var tipos = await _unitOfWork.TipoProducto.GetAllAsync();
//         var dtos = _mapper.Map<List<CatalogoDTO>>(tipos);
//         return Ok(dtos);
//     }
//
//     // GET /api/catalogos/estadosolicitud
//     [HttpGet("estadosolicitud")]
//     public async Task<IActionResult> GetEstadosSolicitud()
//     {
//         var estados = await _unitOfWork.EstadoSolicitud.GetAllAsync();
//         var dtos = _mapper.Map<List<CatalogoDTO>>(estados);
//         return Ok(dtos);
//     }
//
//     // GET /api/catalogos/decisionesrevision
//     [HttpGet("decisionesrevision")]
//     public async Task<IActionResult> GetDecisionesRevision()
//     {
//         var decisiones = await _unitOfWork.DecisionRevision.GetAllAsync();
//         var dtos = _mapper.Map<List<CatalogoDTO>>(decisiones);
//         return Ok(dtos);
//     }
//
//     // GET /api/catalogos/roles
//     [HttpGet("roles")]
//     public async Task<IActionResult> GetRoles()
//     {
//         var roles = await _unitOfWork.Rol.GetAllAsync();
//         var dtos = _mapper.Map<List<CatalogoDTO>>(roles);
//         return Ok(dtos);
//     }
// }