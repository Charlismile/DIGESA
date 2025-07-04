using AutoMapper;
using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Paciente 
            CreateMap<Paciente, PacienteDTO>()
    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
    .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId))
    .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.NombreCompleto))
    .ForMember(dest => dest.TipoDocumento, opt => opt.MapFrom(src => src.TipoDocumento))
    .ForMember(dest => dest.NumeroDocumento, opt => opt.MapFrom(src => src.NumeroDocumento))
    .ForMember(dest => dest.Nacionalidad, opt => opt.MapFrom(src => src.Nacionalidad))
    .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNacimiento))
    .ForMember(dest => dest.Sexo, opt => opt.MapFrom(src => src.Sexo))
    .ForMember(dest => dest.InstalacionSalud, opt => opt.MapFrom(src => src.InstalacionSalud))
    .ForMember(dest => dest.RegionSalud, opt => opt.MapFrom(src => src.RegionSalud))
    .ForMember(dest => dest.RequiereAcompanante, opt => opt.MapFrom(src => src.RequiereAcompanante))
    .ForMember(dest => dest.MotivoRequerimientoAcompanante, opt => opt.MapFrom(src => src.MotivoRequerimientoAcompanante))
    .ForMember(dest => dest.TipoDiscapacidad, opt => opt.MapFrom(src => src.TipoDiscapacidad))
    .ForMember(dest => dest.DireccionResidencia, opt => opt.MapFrom(src => src.DireccionResidencia))
    .ForMember(dest => dest.TelefonoResidencial, opt => opt.MapFrom(src => src.TelefonoResidencial))
    .ForMember(dest => dest.TelefonoPersonal, opt => opt.MapFrom(src => src.TelefonoPersonal))
    .ForMember(dest => dest.TelefonoLaboral, opt => opt.MapFrom(src => src.TelefonoLaboral))
    .ForMember(dest => dest.CorreoElectronico, opt => opt.MapFrom(src => src.CorreoElectronico))
    .ForMember(dest => dest.FirmaBase64, opt => opt.MapFrom(src => src.FirmaBase64));

CreateMap<PacienteDTO, Paciente>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId))
    .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.NombreCompleto))
    .ForMember(dest => dest.TipoDocumento, opt => opt.MapFrom(src => src.TipoDocumento))
    .ForMember(dest => dest.NumeroDocumento, opt => opt.MapFrom(src => src.NumeroDocumento))
    .ForMember(dest => dest.Nacionalidad, opt => opt.MapFrom(src => src.Nacionalidad))
    .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNacimiento ?? DateTime.Now))
    .ForMember(dest => dest.Sexo, opt => opt.MapFrom(src => src.Sexo))
    .ForMember(dest => dest.InstalacionSalud, opt => opt.MapFrom(src => src.InstalacionSalud))
    .ForMember(dest => dest.RegionSalud, opt => opt.MapFrom(src => src.RegionSalud))
    .ForMember(dest => dest.RequiereAcompanante, opt => opt.MapFrom(src => src.RequiereAcompanante))
    .ForMember(dest => dest.MotivoRequerimientoAcompanante, opt => opt.MapFrom(src => src.MotivoRequerimientoAcompanante))
    .ForMember(dest => dest.TipoDiscapacidad, opt => opt.MapFrom(src => src.TipoDiscapacidad))
    .ForMember(dest => dest.DireccionResidencia, opt => opt.MapFrom(src => src.DireccionResidencia))
    .ForMember(dest => dest.TelefonoResidencial, opt => opt.MapFrom(src => src.TelefonoResidencial))
    .ForMember(dest => dest.TelefonoPersonal, opt => opt.MapFrom(src => src.TelefonoPersonal))
    .ForMember(dest => dest.TelefonoLaboral, opt => opt.MapFrom(src => src.TelefonoLaboral))
    .ForMember(dest => dest.CorreoElectronico, opt => opt.MapFrom(src => src.CorreoElectronico))
    .ForMember(dest => dest.FirmaBase64, opt => opt.MapFrom(src => src.FirmaBase64));

            //AuditoriaAccion
            CreateMap<AuditoriaAccion, AuditoriaAccionDTO>()
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId))
                .ForMember(dest => dest.AccionRealizada, opt => opt.MapFrom(src => src.AccionRealizada))
                .ForMember(dest => dest.NombreTablaAfectada, opt => opt.MapFrom(src => src.NombreTablaAfectada))
                .ForMember(dest => dest.RegistroAfectadoId, opt => opt.MapFrom(src => src.RegistroAfectadoId))
                .ForMember(dest => dest.DetallesAdicionales, opt => opt.MapFrom(src => src.DetallesAdicionales))
                .ForMember(dest => dest.FechaAccion, opt => opt.MapFrom(src => src.FechaAccion))
                .ForMember(dest => dest.Ipaddress, opt => opt.MapFrom(src => src.Ipaddress))
                .ForMember(dest => dest.UserAgent, opt => opt.MapFrom(src => src.UserAgent));
            
            //Certificacion
            CreateMap<Certificacion, CertificacionDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SolicitudId, opt => opt.MapFrom(src => src.SolicitudId))
                .ForMember(dest => dest.CodigoCertificado, opt => opt.MapFrom(src => src.CodigoCertificado))
                .ForMember(dest => dest.CodigoQR, opt => opt.MapFrom(src => src.CodigoQr))
                .ForMember(dest => dest.RutaArchivoQR, opt => opt.MapFrom(src => src.RutaArchivoQr))
                .ForMember(dest => dest.QRBase64, opt => opt.MapFrom(src => src.Qrbase64))
                .ForMember(dest => dest.FechaEmision, opt => opt.MapFrom(src => src.FechaEmision))
                .ForMember(dest => dest.FechaVencimiento, opt => opt.MapFrom(src => src.FechaVencimiento))
                .ForMember(dest => dest.EstadoCertificado, opt => opt.MapFrom(src => src.EstadoCertificado));

            CreateMap<CertificacionDTO, Certificacion>()
                .ForMember(dest => dest.SolicitudId, opt => opt.MapFrom(src => src.SolicitudId))
                .ForMember(dest => dest.CodigoCertificado, opt => opt.MapFrom(src => src.CodigoCertificado))
                .ForMember(dest => dest.CodigoQr, opt => opt.MapFrom(src => src.CodigoQR))
                .ForMember(dest => dest.RutaArchivoQr, opt => opt.MapFrom(src => src.RutaArchivoQR))
                .ForMember(dest => dest.Qrbase64, opt => opt.MapFrom(src => src.QRBase64))
                .ForMember(dest => dest.FechaEmision, opt => opt.MapFrom(src => src.FechaEmision))
                .ForMember(dest => dest.FechaVencimiento, opt => opt.MapFrom(src => src.FechaVencimiento))
                .ForMember(dest => dest.EstadoCertificado, opt => opt.MapFrom(src => src.EstadoCertificado));
            
            //Contacto
            CreateMap<Contacto, ContactoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PropietarioTipo, opt => opt.MapFrom(src => src.PropietarioTipo))
                .ForMember(dest => dest.PropietarioId, opt => opt.MapFrom(src => src.PropietarioId))
                .ForMember(dest => dest.TipoContactoId, opt => opt.MapFrom(src => src.TipoContactoId))
                .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.Valor));

            CreateMap<ContactoDTO, Contacto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // O usar .Ignore() si se crea nuevo
                .ForMember(dest => dest.PropietarioTipo, opt => opt.MapFrom(src => src.PropietarioTipo))
                .ForMember(dest => dest.PropietarioId, opt => opt.MapFrom(src => src.PropietarioId))
                .ForMember(dest => dest.TipoContactoId, opt => opt.MapFrom(src => src.TipoContactoId))
                .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.Valor));
            
            //Decision revision
            CreateMap<DecisionRevision, DecisionRevisionDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));

            CreateMap<DecisionRevisionDTO, DecisionRevision>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Se ignora para creación
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));
            
            // Diagnostico
            CreateMap<Diagnostico, DiagnosticoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CodigoCie10, opt => opt.MapFrom(src => src.CodigoCie10))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.EsOtro, opt => opt.MapFrom(src => src.EsOtro));

            CreateMap<DiagnosticoDTO, Diagnostico>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Para creación
                .ForMember(dest => dest.CodigoCie10, opt => opt.MapFrom(src => src.CodigoCie10))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.EsOtro, opt => opt.MapFrom(src => src.EsOtro));
            
            //Documento Adjunto
            CreateMap<DocumentoAdjunto, DocumentoAdjuntoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SolicitudId, opt => opt.MapFrom(src => src.SolicitudId))
                .ForMember(dest => dest.NombreArchivo, opt => opt.MapFrom(src => src.NombreArchivo))
                .ForMember(dest => dest.TipoDocumento, opt => opt.MapFrom(src => src.TipoDocumento))
                .ForMember(dest => dest.TipoContenidoMIME, opt => opt.MapFrom(src => src.TipoContenidoMime))
                .ForMember(dest => dest.RutaAlmacenamiento, opt => opt.MapFrom(src => src.RutaAlmacenamiento))
                .ForMember(dest => dest.FechaSubida, opt => opt.MapFrom(src => src.FechaSubida))
                .ForMember(dest => dest.SubidoPorUsuarioId, opt => opt.MapFrom(src => src.SubidoPorUsuarioId))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion));

            CreateMap<DocumentoAdjuntoDTO, DocumentoAdjunto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Se ignora al crear
                .ForMember(dest => dest.SolicitudId, opt => opt.MapFrom(src => src.SolicitudId))
                .ForMember(dest => dest.NombreArchivo, opt => opt.MapFrom(src => src.NombreArchivo))
                .ForMember(dest => dest.TipoDocumento, opt => opt.MapFrom(src => src.TipoDocumento))
                .ForMember(dest => dest.TipoContenidoMime, opt => opt.MapFrom(src => src.TipoContenidoMIME))
                .ForMember(dest => dest.RutaAlmacenamiento, opt => opt.MapFrom(src => src.RutaAlmacenamiento))
                .ForMember(dest => dest.FechaSubida, opt => opt.MapFrom(src => src.FechaSubida ?? DateTime.Now))
                .ForMember(dest => dest.SubidoPorUsuarioId, opt => opt.MapFrom(src => src.SubidoPorUsuarioId))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion));
            
            //Estado Solicitud
            CreateMap<EstadoSolicitud, EstadoSolicitudDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));

            CreateMap<EstadoSolicitudDTO, EstadoSolicitud>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Se ignora al crear
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));
            
            //Forma Farmaceutica
            CreateMap<FormaFarmaceutica, FormaFarmaceuticaDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));

            CreateMap<FormaFarmaceuticaDTO, FormaFarmaceutica>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Se ignora al crear
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));
            
            //Frecuencia Administracion
            CreateMap<FrecuenciaAdministracion, FrecuenciaAdministracionDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));

            CreateMap<FrecuenciaAdministracionDTO, FrecuenciaAdministracion>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Se ignora al crear
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));
            
            // Medico
            CreateMap<Medico, MedicoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId))
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.NombreCompleto))
                .ForMember(dest => dest.Especialidad, opt => opt.MapFrom(src => src.Especialidad))
                .ForMember(dest => dest.RegistroIdoneidad, opt => opt.MapFrom(src => src.NumeroRegistroIdoneidad))
                .ForMember(dest => dest.InstalacionSalud, opt => opt.MapFrom(src => src.InstalacionSalud))
                .ForMember(dest => dest.NumeroTelefono, opt => opt.MapFrom(src => src.NumeroTelefono))
                .ForMember(dest => dest.EsMedicoEspecialista, opt => opt.MapFrom(src => src.EsMedicoEspecialista))
                .ForMember(dest => dest.FirmaBase64, opt => opt.MapFrom(src => src.FirmaBase64));

            CreateMap<MedicoDTO, Medico>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Se ignora al crear
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId))
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.NombreCompleto))
                .ForMember(dest => dest.Especialidad, opt => opt.MapFrom(src => src.Especialidad))
                .ForMember(dest => dest.NumeroRegistroIdoneidad, opt => opt.MapFrom(src => src.RegistroIdoneidad))
                .ForMember(dest => dest.InstalacionSalud, opt => opt.MapFrom(src => src.InstalacionSalud))
                .ForMember(dest => dest.NumeroTelefono, opt => opt.MapFrom(src => src.NumeroTelefono))
                .ForMember(dest => dest.EsMedicoEspecialista, opt => opt.MapFrom(src => src.EsMedicoEspecialista))
                .ForMember(dest => dest.FirmaBase64, opt => opt.MapFrom(src => src.FirmaBase64));
            
            //Paciente Diagnostico
            CreateMap<PacienteDiagnostico, PacienteDiagnosticoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
                .ForMember(dest => dest.DiagnosticoId, opt => opt.MapFrom(src => src.DiagnosticoId))
                .ForMember(dest => dest.FechaDiagnostico, opt => opt.MapFrom(src => src.FechaDiagnostico))
                .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
                .ForMember(dest => dest.NombreDiagnostico, opt => opt.MapFrom(src => src.Diagnostico.Nombre));

            CreateMap<PacienteDiagnosticoDTO, PacienteDiagnostico>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Se ignora al crear
                .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
                .ForMember(dest => dest.DiagnosticoId, opt => opt.MapFrom(src => src.DiagnosticoId))
                .ForMember(dest => dest.FechaDiagnostico, opt => opt.MapFrom(src => src.FechaDiagnostico))
                .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones));
            
            //Revision
            CreateMap<Revision, RevisionDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SolicitudId, opt => opt.MapFrom(src => src.SolicitudId))
                .ForMember(dest => dest.RevisorId, opt => opt.MapFrom(src => src.RevisorId))
                .ForMember(dest => dest.TipoRevision, opt => opt.MapFrom(src => src.TipoRevision))
                .ForMember(dest => dest.FechaRevision, opt => opt.MapFrom(src => src.FechaRevision))
                .ForMember(dest => dest.DecisionRevisionId, opt => opt.MapFrom(src => src.DecisionRevisionId))
                .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
                .ForMember(dest => dest.NombreRevisor, opt => opt.MapFrom(src => src.Revisor.Nombre + " " + src.Revisor.Apellido))
                .ForMember(dest => dest.DecisionNombre, opt => opt.MapFrom(src => src.DecisionRevision.Nombre));

            CreateMap<RevisionDTO, Revision>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Se ignora al crear
                .ForMember(dest => dest.SolicitudId, opt => opt.MapFrom(src => src.SolicitudId))
                .ForMember(dest => dest.RevisorId, opt => opt.MapFrom(src => src.RevisorId))
                .ForMember(dest => dest.TipoRevision, opt => opt.MapFrom(src => src.TipoRevision))
                .ForMember(dest => dest.FechaRevision, opt => opt.MapFrom(src => src.FechaRevision ?? DateTime.Now))
                .ForMember(dest => dest.DecisionRevisionId, opt => opt.MapFrom(src => src.DecisionRevisionId))
                .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones));
            
            // rol
            CreateMap<Rol, RolDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));

            CreateMap<RolDTO, Rol>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Se ignora al crear
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));
            
            //Solicitud
            CreateMap<Solicitud, SolicitudDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
                .ForMember(dest => dest.MedicoId, opt => opt.MapFrom(src => src.MedicoId))
                .ForMember(dest => dest.AcompananteId, opt => opt.MapFrom(src => src.AcompananteId))
                .ForMember(dest => dest.FechaSolicitud, opt => opt.MapFrom(src => src.FechaSolicitud))
                .ForMember(dest => dest.EstadoSolicitudId, opt => opt.MapFrom(src => src.EstadoSolicitudId))
                .ForMember(dest => dest.MotivoSolicitud, opt => opt.MapFrom(src => src.MotivoSolicitud))
                .ForMember(dest => dest.FuncionarioRecibeId, opt => opt.MapFrom(src => src.FuncionarioRecibeId))
                .ForMember(dest => dest.FechaRecepcion, opt => opt.MapFrom(src => src.FechaRecepcion))
                .ForMember(dest => dest.FechaAprobacionRechazo, opt => opt.MapFrom(src => src.FechaAprobacionRechazo))
                .ForMember(dest => dest.ObservacionesRevision, opt => opt.MapFrom(src => src.ObservacionesRevision))
                .ForMember(dest => dest.FirmaBase64, opt => opt.MapFrom(src => src.FirmaBase64))

                // Campos adicionales para mostrar info en UI
                .ForMember(dest => dest.NombrePaciente, opt => opt.MapFrom(src => src.Paciente.NombreCompleto))
                .ForMember(dest => dest.NombreMedico, opt => opt.MapFrom(src => src.Medico.NombreCompleto))
                .ForMember(dest => dest.NombreFuncionario, opt => opt.MapFrom(src => src.FuncionarioRecibe != null ? $"{src.FuncionarioRecibe.Nombre} {src.FuncionarioRecibe.Apellido}" : ""))
                .ForMember(dest => dest.EstadoNombre, opt => opt.MapFrom(src => src.EstadoSolicitud.Nombre));

            CreateMap<SolicitudDTO, Solicitud>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Se ignora al crear
                .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
                .ForMember(dest => dest.MedicoId, opt => opt.MapFrom(src => src.MedicoId))
                .ForMember(dest => dest.AcompananteId, opt => opt.MapFrom(src => src.AcompananteId))
                .ForMember(dest => dest.FechaSolicitud, opt => opt.MapFrom(src => src.FechaSolicitud ?? DateTime.Now))
                .ForMember(dest => dest.EstadoSolicitudId, opt => opt.MapFrom(src => src.EstadoSolicitudId))
                .ForMember(dest => dest.MotivoSolicitud, opt => opt.MapFrom(src => src.MotivoSolicitud))
                .ForMember(dest => dest.FuncionarioRecibeId, opt => opt.MapFrom(src => src.FuncionarioRecibeId))
                .ForMember(dest => dest.FechaRecepcion, opt => opt.MapFrom(src => src.FechaRecepcion))
                .ForMember(dest => dest.FechaAprobacionRechazo, opt => opt.MapFrom(src => src.FechaAprobacionRechazo))
                .ForMember(dest => dest.ObservacionesRevision, opt => opt.MapFrom(src => src.ObservacionesRevision))
                .ForMember(dest => dest.FirmaBase64, opt => opt.MapFrom(src => src.FirmaBase64));
                        
            //Solicitud Diagnostico
            CreateMap<SolicitudDiagnostico, SolicitudDiagnosticoDTO>()
                .ForMember(dest => dest.SolicitudId, opt => opt.MapFrom(src => src.SolicitudId))
                .ForMember(dest => dest.DiagnosticoId, opt => opt.MapFrom(src => src.DiagnosticoId))
                .ForMember(dest => dest.EsPrimario, opt => opt.MapFrom(src => src.EsPrimario))
                .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
                .ForMember(dest => dest.NombreDiagnostico, opt => opt.MapFrom(src => src.Diagnostico.Nombre));

            CreateMap<SolicitudDiagnosticoDTO, SolicitudDiagnostico>()
                .ForMember(dest => dest.SolicitudId, opt => opt.MapFrom(src => src.SolicitudId))
                .ForMember(dest => dest.DiagnosticoId, opt => opt.MapFrom(src => src.DiagnosticoId))
                .ForMember(dest => dest.EsPrimario, opt => opt.MapFrom(src => src.EsPrimario))
                .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones));
            
            //TipoContacto
            CreateMap<TipoContacto, TipoContactoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));

            CreateMap<TipoContactoDTO, TipoContacto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Se ignora al crear
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));
            
            //TipoProducto
            CreateMap<TipoProducto, TipoProductoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));

            CreateMap<TipoProductoDTO, TipoProducto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Se ignora al crear
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));
            
            // Diagnóstico
            CreateMap<DiagnosticoSeleccionadoDTO, Diagnostico>()
                .ForMember(dest => dest.EsOtro, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.CodigoCIE10)))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => 
                    string.IsNullOrWhiteSpace(src.Observaciones) ? 
                        $"Diagnóstico creado automáticamente - {src.Nombre}" : 
                        src.Observaciones))
                .ForMember(dest => dest.CodigoCie10, opt => opt.MapFrom(src => src.CodigoCIE10 ?? ""));

            // Tratamiento
            CreateMap<Tratamiento, TratamientoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SolicitudId, opt => opt.MapFrom(src => src.SolicitudId))
                .ForMember(dest => dest.NombreGenericoProducto, opt => opt.MapFrom(src => src.NombreGenericoProducto))
                .ForMember(dest => dest.NombreComercialProducto, opt => opt.MapFrom(src => src.NombreComercialProducto))
                .ForMember(dest => dest.FormaFarmaceutica, opt => opt.MapFrom(src => src.FormaFarmaceutica))
                .ForMember(dest => dest.ConcentracionCBD, opt => opt.MapFrom(src => src.ConcentracionCbd))
                .ForMember(dest => dest.UnidadCBD, opt => opt.MapFrom(src => src.UnidadCbd))
                .ForMember(dest => dest.ConcentracionTHC, opt => opt.MapFrom(src => src.ConcentracionThc))
                .ForMember(dest => dest.UnidadTHC, opt => opt.MapFrom(src => src.UnidadThc))
                .ForMember(dest => dest.OtrosCannabinoides, opt => opt.MapFrom(src => src.OtrosCannabinoides))
                .ForMember(dest => dest.ViaAdministracion, opt => opt.MapFrom(src => src.ViaAdministracion))
                .ForMember(dest => dest.Dosis, opt => opt.MapFrom(src => src.Dosis))
                .ForMember(dest => dest.FrecuenciaAdministracion, opt => opt.MapFrom(src => src.FrecuenciaAdministracion))
                .ForMember(dest => dest.DuracionTratamientoDias, opt => opt.MapFrom(src => src.DuracionTratamientoDias))
                .ForMember(dest => dest.CantidadPrescrita, opt => opt.MapFrom(src => src.CantidadPrescrita))
                .ForMember(dest => dest.InstruccionesAdicionales, opt => opt.MapFrom(src => src.InstruccionesAdicionales))
                .ForMember(dest => dest.FechaInicioTratamientoPrevista, opt => opt.MapFrom(src => src.FechaInicioTratamientoPrevista))
                .ForMember(dest => dest.TipoProductoId, opt => opt.MapFrom(src => src.TipoProductoId))
                .ForMember(dest => dest.OtroProductoDescripcion, opt => opt.MapFrom(src => src.OtroProductoDescripcion))
                .ForMember(dest => dest.FormaFarmaceuticaId, opt => opt.MapFrom(src => src.FormaFarmaceuticaId))
                .ForMember(dest => dest.OtraFormaFarmaceuticaDescripcion, opt => opt.MapFrom(src => src.OtraFormaFarmaceuticaDescripcion))
                .ForMember(dest => dest.UnidadCBDId, opt => opt.MapFrom(src => src.UnidadCbdid))
                .ForMember(dest => dest.OtraUnidadCBDDescripcion, opt => opt.MapFrom(src => src.OtraUnidadCbddescripcion))
                .ForMember(dest => dest.OtraUnidadTHCDescripcion, opt => opt.MapFrom(src => src.OtraUnidadThcdescripcion))
                .ForMember(dest => dest.OtroCannabinode1, opt => opt.MapFrom(src => src.OtroCannabinode1))
                .ForMember(dest => dest.ConcentracionOtroCannabinoide1, opt => opt.MapFrom(src => src.ConcentracionOtroCannabinoide1))
                .ForMember(dest => dest.UnidadOtroCannabinoide1Id, opt => opt.MapFrom(src => src.UnidadOtroCannabinoide1Id))
                .ForMember(dest => dest.OtraUnidadOtroCannabinoide1Descripcion, opt => opt.MapFrom(src => src.OtraUnidadOtroCannabinoide1Descripcion))
                .ForMember(dest => dest.OtroCannabinode2, opt => opt.MapFrom(src => src.OtroCannabinode2))
                .ForMember(dest => dest.ConcentracionOtroCannabinoide2, opt => opt.MapFrom(src => src.ConcentracionOtroCannabinoide2))
                .ForMember(dest => dest.UnidadOtroCannabinoide2Id, opt => opt.MapFrom(src => src.UnidadOtroCannabinoide2Id))
                .ForMember(dest => dest.OtraUnidadOtroCannabinoide2Descripcion, opt => opt.MapFrom(src => src.OtraUnidadOtroCannabinoide2Descripcion))
                .ForMember(dest => dest.ViaAdministracionId, opt => opt.MapFrom(src => src.ViaAdministracionId))
                .ForMember(dest => dest.OtraViaAdministracionDescripcion, opt => opt.MapFrom(src => src.OtraViaAdministracionDescripcion))
                .ForMember(dest => dest.FrecuenciaAdministracionId, opt => opt.MapFrom(src => src.FrecuenciaAdministracionId))
                .ForMember(dest => dest.DuracionMeses, opt => opt.MapFrom(src => src.DuracionMeses))
                .ForMember(dest => dest.DuracionDiasExtra, opt => opt.MapFrom(src => src.DuracionDiasExtra));

            CreateMap<TratamientoDTO, Tratamiento>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SolicitudId, opt => opt.MapFrom(src => src.SolicitudId))
                .ForMember(dest => dest.NombreGenericoProducto, opt => opt.MapFrom(src => src.NombreGenericoProducto))
                .ForMember(dest => dest.NombreComercialProducto, opt => opt.MapFrom(src => src.NombreComercialProducto))
                .ForMember(dest => dest.FormaFarmaceutica, opt => opt.MapFrom(src => src.FormaFarmaceutica))
                .ForMember(dest => dest.ConcentracionCbd, opt => opt.MapFrom(src => src.ConcentracionCBD))
                .ForMember(dest => dest.UnidadCbd, opt => opt.MapFrom(src => src.UnidadCBD))
                .ForMember(dest => dest.ConcentracionThc, opt => opt.MapFrom(src => src.ConcentracionTHC))
                .ForMember(dest => dest.UnidadThc, opt => opt.MapFrom(src => src.UnidadTHC))
                .ForMember(dest => dest.OtrosCannabinoides, opt => opt.MapFrom(src => src.OtrosCannabinoides))
                .ForMember(dest => dest.ViaAdministracion, opt => opt.MapFrom(src => src.ViaAdministracion))
                .ForMember(dest => dest.Dosis, opt => opt.MapFrom(src => src.Dosis))
                .ForMember(dest => dest.FrecuenciaAdministracion, opt => opt.MapFrom(src => src.FrecuenciaAdministracion))
                .ForMember(dest => dest.DuracionTratamientoDias, opt => opt.MapFrom(src => src.DuracionTratamientoDias))
                .ForMember(dest => dest.CantidadPrescrita, opt => opt.MapFrom(src => src.CantidadPrescrita))
                .ForMember(dest => dest.InstruccionesAdicionales, opt => opt.MapFrom(src => src.InstruccionesAdicionales))
                .ForMember(dest => dest.FechaInicioTratamientoPrevista, opt => opt.MapFrom(src => src.FechaInicioTratamientoPrevista))
                .ForMember(dest => dest.TipoProductoId, opt => opt.MapFrom(src => src.TipoProductoId))
                .ForMember(dest => dest.OtroProductoDescripcion, opt => opt.MapFrom(src => src.OtroProductoDescripcion))
                .ForMember(dest => dest.FormaFarmaceuticaId, opt => opt.MapFrom(src => src.FormaFarmaceuticaId))
                .ForMember(dest => dest.OtraFormaFarmaceuticaDescripcion, opt => opt.MapFrom(src => src.OtraFormaFarmaceuticaDescripcion))
                .ForMember(dest => dest.UnidadCbdid, opt => opt.MapFrom(src => src.UnidadCBDId))
                .ForMember(dest => dest.OtraUnidadCbddescripcion, opt => opt.MapFrom(src => src.OtraUnidadCBDDescripcion))
                .ForMember(dest => dest.OtraUnidadThcdescripcion, opt => opt.MapFrom(src => src.OtraUnidadTHCDescripcion))
                .ForMember(dest => dest.OtroCannabinode1, opt => opt.MapFrom(src => src.OtroCannabinode1))
                .ForMember(dest => dest.ConcentracionOtroCannabinoide1, opt => opt.MapFrom(src => src.ConcentracionOtroCannabinoide1))
                .ForMember(dest => dest.UnidadOtroCannabinoide1Id, opt => opt.MapFrom(src => src.UnidadOtroCannabinoide1Id))
                .ForMember(dest => dest.OtraUnidadOtroCannabinoide1Descripcion, opt => opt.MapFrom(src => src.OtraUnidadOtroCannabinoide1Descripcion))
                .ForMember(dest => dest.OtroCannabinode2, opt => opt.MapFrom(src => src.OtroCannabinode2))
                .ForMember(dest => dest.ConcentracionOtroCannabinoide2, opt => opt.MapFrom(src => src.ConcentracionOtroCannabinoide2))
                .ForMember(dest => dest.UnidadOtroCannabinoide2Id, opt => opt.MapFrom(src => src.UnidadOtroCannabinoide2Id))
                .ForMember(dest => dest.OtraUnidadOtroCannabinoide2Descripcion, opt => opt.MapFrom(src => src.OtraUnidadOtroCannabinoide2Descripcion))
                .ForMember(dest => dest.ViaAdministracionId, opt => opt.MapFrom(src => src.ViaAdministracionId))
                .ForMember(dest => dest.OtraViaAdministracionDescripcion, opt => opt.MapFrom(src => src.OtraViaAdministracionDescripcion))
                .ForMember(dest => dest.FrecuenciaAdministracionId, opt => opt.MapFrom(src => src.FrecuenciaAdministracionId))
                .ForMember(dest => dest.DuracionMeses, opt => opt.MapFrom(src => src.DuracionMeses))
                .ForMember(dest => dest.DuracionDiasExtra, opt => opt.MapFrom(src => src.DuracionDiasExtra));
            
            //UnidadConcentracion
            CreateMap<UnidadConcentracion, UnidadConcentracionDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));

            CreateMap<UnidadConcentracionDTO, UnidadConcentracion>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Se ignora al crear
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));
            
            //Usuario
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.Apellido))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.RolId, opt => opt.MapFrom(src => src.RolId))
                .ForMember(dest => dest.FirmaBase64, opt => opt.MapFrom(src => src.FirmaBase64));

            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.Apellido))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ContraseñaHash, opt => opt.Ignore()) // Se maneja en servicio
                .ForMember(dest => dest.Salt, opt => opt.Ignore()) // Se genera en servicio
                .ForMember(dest => dest.RolId, opt => opt.MapFrom(src => src.RolId))
                // .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => src.FechaRegistro ?? DateTime.Now))
                .ForMember(dest => dest.FirmaBase64, opt => opt.MapFrom(src => src.FirmaBase64));
            
            //Via Administracion
            CreateMap<ViaAdministracion, ViaAdministracionDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));

            CreateMap<ViaAdministracionDTO, ViaAdministracion>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Se ignora al crear
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));
            
            
            
            
            
            
            
        }
    }
}