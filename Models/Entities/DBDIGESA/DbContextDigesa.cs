using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class DbContextDigesa : DbContext
{
    public DbContextDigesa()
    {
    }

    public DbContextDigesa(DbContextOptions<DbContextDigesa> options)
        : base(options)
    {
    }

    public virtual DbSet<Acompanante> Acompanante { get; set; }

    public virtual DbSet<AuditoriaAccion> AuditoriaAccion { get; set; }

    public virtual DbSet<Certificacion> Certificacion { get; set; }

    public virtual DbSet<DocumentoAdjunto> DocumentoAdjunto { get; set; }

    public virtual DbSet<FormaFarmaceutica> FormaFarmaceutica { get; set; }

    public virtual DbSet<Medico> Medico { get; set; }

    public virtual DbSet<Paciente> Paciente { get; set; }

    public virtual DbSet<Revision> Revision { get; set; }

    public virtual DbSet<Solicitud> Solicitud { get; set; }

    public virtual DbSet<Tratamiento> Tratamiento { get; set; }

    public virtual DbSet<TratamientoFormaFarmaceutica> TratamientoFormaFarmaceutica { get; set; }

    public virtual DbSet<TratamientoViaAdministracion> TratamientoViaAdministracion { get; set; }

    public virtual DbSet<UnidadMedida> UnidadMedida { get; set; }

    public virtual DbSet<ViaAdministracion> ViaAdministracion { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Acompanante>(entity =>
        {
            entity.Property(e => e.AcompananteActivo)
                .HasDefaultValue(true)
                .HasColumnName("Acompanante_Activo");
            entity.Property(e => e.AcompananteFechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Acompanante_FechaRegistro");
            entity.Property(e => e.AcompananteNacionalidad)
                .HasMaxLength(50)
                .HasColumnName("Acompanante_Nacionalidad");
            entity.Property(e => e.AcompananteNombreCompleto)
                .HasMaxLength(150)
                .HasColumnName("Acompanante_NombreCompleto");
            entity.Property(e => e.AcompananteNumeroDocumento)
                .HasMaxLength(30)
                .HasColumnName("Acompanante_NumeroDocumento");
            entity.Property(e => e.AcompanantePacienteId).HasColumnName("Acompanante_PacienteId");
            entity.Property(e => e.AcompananteParentesco)
                .HasMaxLength(50)
                .HasColumnName("Acompanante_Parentesco");
            entity.Property(e => e.AcompananteTipoDocumento)
                .HasMaxLength(20)
                .HasColumnName("Acompanante_TipoDocumento");

            entity.HasOne(d => d.AcompanantePaciente).WithMany(p => p.Acompanante)
                .HasForeignKey(d => d.AcompanantePacienteId)
                .HasConstraintName("FK_Acompanante_Paciente");
        });

        modelBuilder.Entity<AuditoriaAccion>(entity =>
        {
            entity.Property(e => e.AuditoriaAccionAccionRealizada)
                .HasMaxLength(500)
                .HasColumnName("AuditoriaAccion_AccionRealizada");
            entity.Property(e => e.AuditoriaAccionDetallesAdicionales).HasColumnName("AuditoriaAccion_DetallesAdicionales");
            entity.Property(e => e.AuditoriaAccionFechaAccion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("AuditoriaAccion_FechaAccion");
            entity.Property(e => e.AuditoriaAccionIpaddress)
                .HasMaxLength(50)
                .HasColumnName("AuditoriaAccion_IPAddress");
            entity.Property(e => e.AuditoriaAccionRegistroAfectadoId).HasColumnName("AuditoriaAccion_RegistroAfectadoId");
            entity.Property(e => e.AuditoriaAccionTablaAfectada)
                .HasMaxLength(128)
                .HasColumnName("AuditoriaAccion_TablaAfectada");
            entity.Property(e => e.AuditoriaAccionUserAgent)
                .HasMaxLength(255)
                .HasColumnName("AuditoriaAccion_UserAgent");
            entity.Property(e => e.AuditoriaAccionUsuarioId)
                .HasMaxLength(450)
                .HasColumnName("AuditoriaAccion_UsuarioId");
        });

        modelBuilder.Entity<Certificacion>(entity =>
        {
            entity.HasIndex(e => e.CertificacionCodigoCertificado, "UIX_Certificacion_Codigo").IsUnique();

            entity.Property(e => e.CertificacionCodigoCertificado)
                .HasMaxLength(100)
                .HasColumnName("Certificacion_CodigoCertificado");
            entity.Property(e => e.CertificacionCodigoQr)
                .HasMaxLength(500)
                .HasColumnName("Certificacion_CodigoQR");
            entity.Property(e => e.CertificacionEstado)
                .HasMaxLength(20)
                .HasDefaultValue("Vigente")
                .HasColumnName("Certificacion_Estado");
            entity.Property(e => e.CertificacionFechaEmision)
                .HasColumnType("datetime")
                .HasColumnName("Certificacion_FechaEmision");
            entity.Property(e => e.CertificacionFechaVencimiento)
                .HasColumnType("datetime")
                .HasColumnName("Certificacion_FechaVencimiento");
            entity.Property(e => e.CertificacionRutaArchivoQr)
                .HasMaxLength(255)
                .HasColumnName("Certificacion_RutaArchivoQR");
            entity.Property(e => e.CertificacionSolicitudId).HasColumnName("Certificacion_SolicitudId");

            entity.HasOne(d => d.CertificacionSolicitud).WithMany(p => p.Certificacion)
                .HasForeignKey(d => d.CertificacionSolicitudId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Certificacion_Solicitud");
        });

        modelBuilder.Entity<DocumentoAdjunto>(entity =>
        {
            entity.Property(e => e.DocumentoAdjuntoDescripcion)
                .HasMaxLength(500)
                .HasColumnName("DocumentoAdjunto_Descripcion");
            entity.Property(e => e.DocumentoAdjuntoFechaSubida)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DocumentoAdjunto_FechaSubida");
            entity.Property(e => e.DocumentoAdjuntoNombreArchivo)
                .HasMaxLength(255)
                .HasColumnName("DocumentoAdjunto_NombreArchivo");
            entity.Property(e => e.DocumentoAdjuntoRutaAlmacenamiento)
                .HasMaxLength(500)
                .HasColumnName("DocumentoAdjunto_RutaAlmacenamiento");
            entity.Property(e => e.DocumentoAdjuntoSolicitudId).HasColumnName("DocumentoAdjunto_SolicitudId");
            entity.Property(e => e.DocumentoAdjuntoSubidoPorUsuarioId)
                .HasMaxLength(450)
                .HasColumnName("DocumentoAdjunto_SubidoPorUsuarioId");
            entity.Property(e => e.DocumentoAdjuntoTipoContenidoMime)
                .HasMaxLength(100)
                .HasColumnName("DocumentoAdjunto_TipoContenidoMIME");
            entity.Property(e => e.DocumentoAdjuntoTipoDocumento)
                .HasMaxLength(100)
                .HasColumnName("DocumentoAdjunto_TipoDocumento");

            entity.HasOne(d => d.DocumentoAdjuntoSolicitud).WithMany(p => p.DocumentoAdjunto)
                .HasForeignKey(d => d.DocumentoAdjuntoSolicitudId)
                .HasConstraintName("FK_DocumentoAdjunto_Solicitud");
        });

        modelBuilder.Entity<FormaFarmaceutica>(entity =>
        {
            entity.HasIndex(e => e.FormaFarmaceuticaNombre, "UIX_FormaFarmaceutica_Nombre").IsUnique();

            entity.Property(e => e.FormaFarmaceuticaNombre)
                .HasMaxLength(100)
                .HasColumnName("FormaFarmaceutica_Nombre");
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasIndex(e => e.MedicoNumeroRegistroIdoneidad, "UIX_Medico_RegistroIdoneidad").IsUnique();

            entity.Property(e => e.MedicoCorreoElectronico)
                .HasMaxLength(100)
                .HasColumnName("Medico_CorreoElectronico");
            entity.Property(e => e.MedicoDisciplina)
                .HasMaxLength(50)
                .HasColumnName("Medico_Disciplina");
            entity.Property(e => e.MedicoEspecialidad)
                .HasMaxLength(100)
                .HasColumnName("Medico_Especialidad");
            entity.Property(e => e.MedicoFechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Medico_FechaRegistro");
            entity.Property(e => e.MedicoInstalacionSalud)
                .HasMaxLength(150)
                .HasColumnName("Medico_InstalacionSalud");
            entity.Property(e => e.MedicoNombreCompleto)
                .HasMaxLength(150)
                .HasColumnName("Medico_NombreCompleto");
            entity.Property(e => e.MedicoNumeroRegistroIdoneidad)
                .HasMaxLength(50)
                .HasColumnName("Medico_NumeroRegistroIdoneidad");
            entity.Property(e => e.MedicoNumeroTelefono)
                .HasMaxLength(20)
                .HasColumnName("Medico_NumeroTelefono");
            entity.Property(e => e.UsuarioId).HasMaxLength(450);
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasIndex(e => new { e.PacienteTipoDocumento, e.PacienteNumeroDocumento }, "UIX_Paciente_NumeroDocumento").IsUnique();

            entity.Property(e => e.PacienteCorreoElectronico)
                .HasMaxLength(100)
                .HasColumnName("Paciente_CorreoElectronico");
            entity.Property(e => e.PacienteDireccionResidencia)
                .HasMaxLength(300)
                .HasColumnName("Paciente_DireccionResidencia");
            entity.Property(e => e.PacienteEstado)
                .HasMaxLength(50)
                .HasDefaultValue("Activo")
                .HasColumnName("Paciente_Estado");
            entity.Property(e => e.PacienteFechaNacimiento).HasColumnName("Paciente_FechaNacimiento");
            entity.Property(e => e.PacienteFechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Paciente_FechaRegistro");
            entity.Property(e => e.PacienteInstalacionSalud)
                .HasMaxLength(150)
                .HasColumnName("Paciente_InstalacionSalud");
            entity.Property(e => e.PacienteMotivoRequerimientoAcompanante)
                .HasMaxLength(250)
                .HasColumnName("Paciente_MotivoRequerimientoAcompanante");
            entity.Property(e => e.PacienteNacionalidad)
                .HasMaxLength(50)
                .HasColumnName("Paciente_Nacionalidad");
            entity.Property(e => e.PacienteNombreCompleto)
                .HasMaxLength(150)
                .HasColumnName("Paciente_NombreCompleto");
            entity.Property(e => e.PacienteNumeroDocumento)
                .HasMaxLength(30)
                .HasColumnName("Paciente_NumeroDocumento");
            entity.Property(e => e.PacienteRegionSalud)
                .HasMaxLength(100)
                .HasColumnName("Paciente_RegionSalud");
            entity.Property(e => e.PacienteRequiereAcompanante).HasColumnName("Paciente_RequiereAcompanante");
            entity.Property(e => e.PacienteSexo)
                .HasMaxLength(20)
                .HasColumnName("Paciente_Sexo");
            entity.Property(e => e.PacienteTelefonoLaboral)
                .HasMaxLength(20)
                .HasColumnName("Paciente_TelefonoLaboral");
            entity.Property(e => e.PacienteTelefonoPersonal)
                .HasMaxLength(20)
                .HasColumnName("Paciente_TelefonoPersonal");
            entity.Property(e => e.PacienteTelefonoResidencial)
                .HasMaxLength(20)
                .HasColumnName("Paciente_TelefonoResidencial");
            entity.Property(e => e.PacienteTipoDiscapacidad)
                .HasMaxLength(200)
                .HasColumnName("Paciente_TipoDiscapacidad");
            entity.Property(e => e.PacienteTipoDocumento)
                .HasMaxLength(20)
                .HasColumnName("Paciente_TipoDocumento");
            entity.Property(e => e.UsuarioId).HasMaxLength(450);
        });

        modelBuilder.Entity<Revision>(entity =>
        {
            entity.Property(e => e.RevisionDecision)
                .HasMaxLength(50)
                .HasColumnName("Revision_Decision");
            entity.Property(e => e.RevisionEstado)
                .HasMaxLength(50)
                .HasDefaultValue("Finalizada")
                .HasColumnName("Revision_Estado");
            entity.Property(e => e.RevisionFechaRevision)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Revision_FechaRevision");
            entity.Property(e => e.RevisionObservaciones).HasColumnName("Revision_Observaciones");
            entity.Property(e => e.RevisionRevisorId)
                .HasMaxLength(450)
                .HasColumnName("Revision_RevisorId");
            entity.Property(e => e.RevisionSolicitudId).HasColumnName("Revision_SolicitudId");
            entity.Property(e => e.RevisionTipoRevision)
                .HasMaxLength(50)
                .HasColumnName("Revision_TipoRevision");

            entity.HasOne(d => d.RevisionSolicitud).WithMany(p => p.Revision)
                .HasForeignKey(d => d.RevisionSolicitudId)
                .HasConstraintName("FK_Revision_Solicitud");
        });

        modelBuilder.Entity<Solicitud>(entity =>
        {
            entity.Property(e => e.SolicitudAceptaTerminos).HasColumnName("Solicitud_AceptaTerminos");
            entity.Property(e => e.SolicitudEstado)
                .HasMaxLength(50)
                .HasDefaultValue("Pendiente")
                .HasColumnName("Solicitud_Estado");
            entity.Property(e => e.SolicitudFechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Solicitud_FechaSolicitud");
            entity.Property(e => e.SolicitudMedicoId).HasColumnName("Solicitud_MedicoId");
            entity.Property(e => e.SolicitudObservaciones).HasColumnName("Solicitud_Observaciones");
            entity.Property(e => e.SolicitudPacienteId).HasColumnName("Solicitud_PacienteId");

            entity.HasOne(d => d.SolicitudMedico).WithMany(p => p.Solicitud)
                .HasForeignKey(d => d.SolicitudMedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitud_Medico");

            entity.HasOne(d => d.SolicitudPaciente).WithMany(p => p.Solicitud)
                .HasForeignKey(d => d.SolicitudPacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitud_Paciente");
        });

        modelBuilder.Entity<Tratamiento>(entity =>
        {
            entity.Property(e => e.TratamientoCantidadPrescrita)
                .HasMaxLength(100)
                .HasColumnName("Tratamiento_CantidadPrescrita");
            entity.Property(e => e.TratamientoConcentracionCbd)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Tratamiento_ConcentracionCBD");
            entity.Property(e => e.TratamientoConcentracionThc)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Tratamiento_ConcentracionTHC");
            entity.Property(e => e.TratamientoDosis)
                .HasMaxLength(100)
                .HasColumnName("Tratamiento_Dosis");
            entity.Property(e => e.TratamientoDuracionTratamientoDias).HasColumnName("Tratamiento_DuracionTratamientoDias");
            entity.Property(e => e.TratamientoEstado)
                .HasMaxLength(50)
                .HasDefaultValue("En proceso")
                .HasColumnName("Tratamiento_Estado");
            entity.Property(e => e.TratamientoFechaFinEstimada).HasColumnName("Tratamiento_FechaFinEstimada");
            entity.Property(e => e.TratamientoFechaInicioTratamientoPrevista).HasColumnName("Tratamiento_FechaInicioTratamientoPrevista");
            entity.Property(e => e.TratamientoFrecuenciaAdministracion)
                .HasMaxLength(100)
                .HasColumnName("Tratamiento_FrecuenciaAdministracion");
            entity.Property(e => e.TratamientoInstruccionesAdicionales).HasColumnName("Tratamiento_InstruccionesAdicionales");
            entity.Property(e => e.TratamientoNombreComercialProducto)
                .HasMaxLength(150)
                .HasColumnName("Tratamiento_NombreComercialProducto");
            entity.Property(e => e.TratamientoNombreGenericoProducto)
                .HasMaxLength(150)
                .HasColumnName("Tratamiento_NombreGenericoProducto");
            entity.Property(e => e.TratamientoOtrosCannabinoides)
                .HasMaxLength(200)
                .HasColumnName("Tratamiento_OtrosCannabinoides");
            entity.Property(e => e.TratamientoSolicitudId).HasColumnName("Tratamiento_SolicitudId");
            entity.Property(e => e.TratamientoUnidadCbdid).HasColumnName("Tratamiento_UnidadCBDId");
            entity.Property(e => e.TratamientoUnidadThcid).HasColumnName("Tratamiento_UnidadTHCId");

            entity.HasOne(d => d.TratamientoSolicitud).WithMany(p => p.Tratamiento)
                .HasForeignKey(d => d.TratamientoSolicitudId)
                .HasConstraintName("FK_Tratamiento_Solicitud");
        });

        modelBuilder.Entity<TratamientoFormaFarmaceutica>(entity =>
        {
            entity.Property(e => e.FormaFarmaceuticaId).HasColumnName("FormaFarmaceutica_Id");
            entity.Property(e => e.TratamientoFormaFarmaceuticaFechaAsignacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("TratamientoFormaFarmaceutica_FechaAsignacion");
            entity.Property(e => e.TratamientoId).HasColumnName("Tratamiento_Id");

            entity.HasOne(d => d.FormaFarmaceutica).WithMany(p => p.TratamientoFormaFarmaceutica)
                .HasForeignKey(d => d.FormaFarmaceuticaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TFF_FormaFarmaceutica");

            entity.HasOne(d => d.Tratamiento).WithMany(p => p.TratamientoFormaFarmaceutica)
                .HasForeignKey(d => d.TratamientoId)
                .HasConstraintName("FK_TFF_Tratamiento");
        });

        modelBuilder.Entity<TratamientoViaAdministracion>(entity =>
        {
            entity.Property(e => e.TratamientoId).HasColumnName("Tratamiento_Id");
            entity.Property(e => e.TratamientoViaAdminFechaAsignacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("TratamientoViaAdmin_FechaAsignacion");
            entity.Property(e => e.ViaAdministracionId).HasColumnName("ViaAdministracion_Id");

            entity.HasOne(d => d.Tratamiento).WithMany(p => p.TratamientoViaAdministracion)
                .HasForeignKey(d => d.TratamientoId)
                .HasConstraintName("FK_TVA_Tratamiento");

            entity.HasOne(d => d.ViaAdministracion).WithMany(p => p.TratamientoViaAdministracion)
                .HasForeignKey(d => d.ViaAdministracionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TVA_ViaAdministracion");
        });

        modelBuilder.Entity<UnidadMedida>(entity =>
        {
            entity.HasIndex(e => e.UnidadMedidaNombre, "UIX_UnidadMedida_Nombre").IsUnique();

            entity.Property(e => e.UnidadMedidaNombre)
                .HasMaxLength(50)
                .HasColumnName("UnidadMedida_Nombre");
            entity.Property(e => e.UnidadMedidaSimbolo)
                .HasMaxLength(20)
                .HasColumnName("UnidadMedida_Simbolo");
        });

        modelBuilder.Entity<ViaAdministracion>(entity =>
        {
            entity.HasIndex(e => e.ViaAdministracionNombre, "UIX_ViaAdministracion_Nombre").IsUnique();

            entity.Property(e => e.ViaAdministracionNombre)
                .HasMaxLength(100)
                .HasColumnName("ViaAdministracion_Nombre");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
