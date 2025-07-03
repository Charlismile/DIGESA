using AutoMapper;
using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Paciente -> PacienteRegistroDTO (para lectura)
            CreateMap<Paciente, PacienteRegistroDTO>()
                .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNacimiento))
                .ForMember(dest => dest.TelefonoResidencial, opt => opt.MapFrom(src => src.TelefonoResidencial))
                .ForMember(dest => dest.TelefonoLaboral, opt => opt.MapFrom(src => src.TelefonoLaboral))
                .ForMember(dest => dest.CorreoElectronico, opt => opt.MapFrom(src => src.CorreoElectronico));

            // PacienteRegistroDTO -> Paciente (para escritura)
            CreateMap<PacienteRegistroDTO, Paciente>()
                .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNacimiento.Value))
                .ForMember(dest => dest.TelefonoResidencial, opt => opt.MapFrom(src => src.TelefonoResidencial ?? ""))
                .ForMember(dest => dest.TelefonoLaboral, opt => opt.MapFrom(src => src.TelefonoLaboral ?? ""))
                .ForMember(dest => dest.CorreoElectronico, opt => opt.MapFrom(src => src.CorreoElectronico ?? ""))
                .ForMember(dest => dest.RequiereAcompanante, opt => opt.MapFrom(src => src.RequiereAcompanante))
                .ForMember(dest => dest.MotivoRequerimientoAcompanante, opt => opt.MapFrom(src => src.MotivoRequerimientoAcompanante ?? ""))
                .ForMember(dest => dest.TipoDiscapacidad, opt => opt.MapFrom(src => src.TipoDiscapacidad ?? ""));

            // Médico
            CreateMap<MedicoDTO, Medico>()
                .ForMember(dest => dest.UsuarioId, opt => opt.Ignore())
                .ForMember(dest => dest.FirmaBase64, opt => opt.Ignore());

            // Diagnóstico
            CreateMap<DiagnosticoSeleccionadoDTO, Diagnostico>()
                .ForMember(dest => dest.EsOtro, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.CodigoCIE10)))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => 
                    string.IsNullOrWhiteSpace(src.Observaciones) ? 
                        $"Diagnóstico creado automáticamente - {src.Nombre}" : 
                        src.Observaciones))
                .ForMember(dest => dest.CodigoCie10, opt => opt.MapFrom(src => src.CodigoCIE10 ?? ""));

            // Tratamiento
            CreateMap<TratamientoDTO, Tratamiento>()
                .ForMember(dest => dest.NombreGenericoProducto, opt => opt.Ignore())
                .ForMember(dest => dest.NombreComercialProducto, opt => opt.Ignore())
                .ForMember(dest => dest.FormaFarmaceutica, opt => opt.Ignore())
                .ForMember(dest => dest.ViaAdministracion, opt => opt.Ignore())
                .ForMember(dest => dest.FrecuenciaAdministracion, opt => opt.Ignore())
                .ForMember(dest => dest.UnidadCbd, opt => opt.Ignore())
                .ForMember(dest => dest.UnidadThc, opt => opt.Ignore());
        }
    }
}