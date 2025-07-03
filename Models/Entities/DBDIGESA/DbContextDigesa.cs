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

    public virtual DbSet<Contacto> Contacto { get; set; }

    public virtual DbSet<DecisionRevision> DecisionRevision { get; set; }

    public virtual DbSet<Diagnostico> Diagnostico { get; set; }

    public virtual DbSet<DocumentoAdjunto> DocumentoAdjunto { get; set; }

    public virtual DbSet<EstadoSolicitud> EstadoSolicitud { get; set; }

    public virtual DbSet<FormaFarmaceutica> FormaFarmaceutica { get; set; }

    public virtual DbSet<FrecuenciaAdministracion> FrecuenciaAdministracion { get; set; }

    public virtual DbSet<Medico> Medico { get; set; }

    public virtual DbSet<Paciente> Paciente { get; set; }

    public virtual DbSet<PacienteDiagnostico> PacienteDiagnostico { get; set; }

    public virtual DbSet<Revision> Revision { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<Solicitud> Solicitud { get; set; }

    public virtual DbSet<SolicitudDiagnostico> SolicitudDiagnostico { get; set; }

    public virtual DbSet<TipoContacto> TipoContacto { get; set; }

    public virtual DbSet<TipoProducto> TipoProducto { get; set; }

    public virtual DbSet<Tratamiento> Tratamiento { get; set; }

    public virtual DbSet<UnidadConcentracion> UnidadConcentracion { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    public virtual DbSet<ViaAdministracion> ViaAdministracion { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Acompanante>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Acompana__3214EC070667A45F");

            entity.HasIndex(e => new { e.TipoDocumento, e.NumeroDocumento }, "UQ_Acompanante_Documento").IsUnique();

            entity.Property(e => e.Nacionalidad).HasMaxLength(50);
            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
            entity.Property(e => e.NumeroDocumento).HasMaxLength(30);
            entity.Property(e => e.Parentesco).HasMaxLength(50);
            entity.Property(e => e.TipoDocumento).HasMaxLength(20);

            entity.HasOne(d => d.Paciente).WithMany(p => p.Acompanante)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK__Acompanan__Pacie__7755B73D");
        });

        modelBuilder.Entity<AuditoriaAccion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Auditori__3214EC07101295E2");

            entity.Property(e => e.AccionRealizada).HasMaxLength(500);
            entity.Property(e => e.FechaAccion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(50)
                .HasColumnName("IPAddress");
            entity.Property(e => e.NombreTablaAfectada).HasMaxLength(128);
            entity.Property(e => e.UserAgent).HasMaxLength(255);

            entity.HasOne(d => d.Usuario).WithMany(p => p.AuditoriaAccion)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Auditoria__Usuar__2AD55B43");
        });

        modelBuilder.Entity<Certificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Certific__3214EC070AC1424E");

            entity.HasIndex(e => e.CodigoCertificado, "IX_Certificacion_CodigoCertificado");

            entity.HasIndex(e => e.SolicitudId, "IX_Certificacion_SolicitudId");

            entity.HasIndex(e => e.SolicitudId, "UQ__Certific__85E95DC6194D6D1D").IsUnique();

            entity.HasIndex(e => e.CodigoCertificado, "UQ__Certific__AAE1A2C385C02771").IsUnique();

            entity.Property(e => e.CodigoCertificado).HasMaxLength(100);
            entity.Property(e => e.CodigoQr)
                .HasMaxLength(500)
                .HasColumnName("CodigoQR");
            entity.Property(e => e.EstadoCertificado)
                .HasMaxLength(20)
                .HasDefaultValue("Activa");
            entity.Property(e => e.FechaEmision)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaVencimiento).HasColumnType("datetime");
            entity.Property(e => e.Qrbase64).HasColumnName("QRBase64");
            entity.Property(e => e.RutaArchivoQr)
                .HasMaxLength(255)
                .HasColumnName("RutaArchivoQR");

            entity.HasOne(d => d.Solicitud).WithOne(p => p.Certificacion)
                .HasForeignKey<Certificacion>(d => d.SolicitudId)
                .HasConstraintName("FK__Certifica__Solic__1C873BEC");
        });

        modelBuilder.Entity<Contacto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contacto__3214EC0710AA51B5");

            entity.Property(e => e.PropietarioTipo).HasMaxLength(50);
            entity.Property(e => e.Valor).HasMaxLength(255);

            entity.HasOne(d => d.TipoContacto).WithMany(p => p.Contacto)
                .HasForeignKey(d => d.TipoContactoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contacto__TipoCo__6EC0713C");
        });

        modelBuilder.Entity<DecisionRevision>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Decision__3214EC07B430FA49");

            entity.HasIndex(e => e.Nombre, "UQ__Decision__75E3EFCF0E43C9FF").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Diagnostico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Diagnost__3214EC076D9F141E");

            entity.HasIndex(e => e.Nombre, "UQ_Diagnostico_Nombre").IsUnique();

            entity.HasIndex(e => e.CodigoCie10, "UQ__Diagnost__D3E9319F6A74B6D5").IsUnique();

            entity.Property(e => e.CodigoCie10)
                .HasMaxLength(10)
                .HasColumnName("CodigoCIE10");
            entity.Property(e => e.Descripcion).HasMaxLength(300);
            entity.Property(e => e.Nombre).HasMaxLength(150);
        });

        modelBuilder.Entity<DocumentoAdjunto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC073E7D6C3B");

            entity.HasIndex(e => e.SolicitudId, "IX_DocumentoAdjunto_SolicitudId");

            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.FechaSubida)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreArchivo).HasMaxLength(255);
            entity.Property(e => e.RutaAlmacenamiento).HasMaxLength(500);
            entity.Property(e => e.TipoContenidoMime)
                .HasMaxLength(100)
                .HasColumnName("TipoContenidoMIME");
            entity.Property(e => e.TipoDocumento).HasMaxLength(100);

            entity.HasOne(d => d.Solicitud).WithMany(p => p.DocumentoAdjunto)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK__Documento__Solic__2057CCD0");

            entity.HasOne(d => d.SubidoPorUsuario).WithMany(p => p.DocumentoAdjunto)
                .HasForeignKey(d => d.SubidoPorUsuarioId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Documento__Subid__214BF109");
        });

        modelBuilder.Entity<EstadoSolicitud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EstadoSo__3214EC0789E95686");

            entity.HasIndex(e => e.Nombre, "UQ__EstadoSo__75E3EFCFE01B6020").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<FormaFarmaceutica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FormaFar__3214EC070E70081C");

            entity.HasIndex(e => e.Nombre, "UQ__FormaFar__75E3EFCF2F83C8A6").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<FrecuenciaAdministracion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Frecuenc__3214EC0747F3F180");

            entity.HasIndex(e => e.Nombre, "UQ__Frecuenc__75E3EFCFC62CD14D").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Medico__3214EC074E06C452");

            entity.HasIndex(e => e.NombreCompleto, "IX_Medico_NombreCompleto");

            entity.HasIndex(e => e.NumeroRegistroIdoneidad, "IX_Medico_NumeroRegistroIdoneidad");

            entity.HasIndex(e => e.UsuarioId, "UQ__Medico__2B3DE7B909C47A6A").IsUnique();

            entity.HasIndex(e => e.NumeroRegistroIdoneidad, "UQ__Medico__489C33DD7B406326").IsUnique();

            entity.Property(e => e.Especialidad).HasMaxLength(500);
            entity.Property(e => e.InstalacionSalud).HasMaxLength(150);
            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
            entity.Property(e => e.NumeroRegistroIdoneidad).HasMaxLength(50);
            entity.Property(e => e.NumeroTelefono)
                .HasMaxLength(20)
                .HasDefaultValue("");

            entity.HasOne(d => d.Usuario).WithOne(p => p.Medico)
                .HasForeignKey<Medico>(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Medico__UsuarioI__7C1A6C5A");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Paciente__3214EC070E80387C");

            entity.HasIndex(e => e.CorreoElectronico, "IX_Paciente_CorreoElectronico").HasFilter("([CorreoElectronico] IS NOT NULL)");

            entity.HasIndex(e => e.NombreCompleto, "IX_Paciente_NombreCompleto");

            entity.HasIndex(e => e.NumeroDocumento, "IX_Paciente_NumeroDocumento");

            entity.HasIndex(e => e.TelefonoPersonal, "IX_Paciente_TelefonoPersonal").HasFilter("([TelefonoPersonal] IS NOT NULL)");

            entity.HasIndex(e => new { e.TipoDocumento, e.NumeroDocumento }, "UQ_Paciente_Documento").IsUnique();

            entity.Property(e => e.CorreoElectronico).HasMaxLength(100);
            entity.Property(e => e.DireccionResidencia)
                .HasMaxLength(300)
                .HasDefaultValue("");
            entity.Property(e => e.FechaNacimiento).HasColumnType("datetime");
            entity.Property(e => e.InstalacionSalud).HasMaxLength(150);
            entity.Property(e => e.MotivoRequerimientoAcompanante).HasMaxLength(250);
            entity.Property(e => e.Nacionalidad).HasMaxLength(50);
            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
            entity.Property(e => e.NumeroDocumento).HasMaxLength(30);
            entity.Property(e => e.RegionSalud).HasMaxLength(100);
            entity.Property(e => e.Sexo).HasMaxLength(20);
            entity.Property(e => e.TelefonoLaboral).HasMaxLength(20);
            entity.Property(e => e.TelefonoPersonal).HasMaxLength(20);
            entity.Property(e => e.TelefonoResidencial).HasMaxLength(20);
            entity.Property(e => e.TipoDiscapacidad).HasMaxLength(200);
            entity.Property(e => e.TipoDocumento).HasMaxLength(20);

            entity.HasOne(d => d.Usuario).WithMany(p => p.Paciente)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Paciente__Usuari__73852659");
        });

        modelBuilder.Entity<PacienteDiagnostico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Paciente__3214EC0760465867");

            entity.HasIndex(e => e.DiagnosticoId, "IX_PacienteDiagnostico_DiagnosticoId");

            entity.HasIndex(e => e.PacienteId, "IX_PacienteDiagnostico_PacienteId");

            entity.HasIndex(e => new { e.PacienteId, e.DiagnosticoId }, "UQ_PacienteDiagnostico_Paciente_Diagnostico").IsUnique();

            entity.Property(e => e.Observaciones).HasMaxLength(300);

            entity.HasOne(d => d.Diagnostico).WithMany(p => p.PacienteDiagnostico)
                .HasForeignKey(d => d.DiagnosticoId)
                .HasConstraintName("FK__PacienteD__Diagn__1209AD79");

            entity.HasOne(d => d.Paciente).WithMany(p => p.PacienteDiagnostico)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK__PacienteD__Pacie__11158940");
        });

        modelBuilder.Entity<Revision>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Revision__3214EC07F90712AA");

            entity.HasIndex(e => e.RevisorId, "IX_Revision_RevisorId");

            entity.HasIndex(e => e.SolicitudId, "IX_Revision_SolicitudId");

            entity.Property(e => e.FechaRevision)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TipoRevision).HasMaxLength(50);

            entity.HasOne(d => d.DecisionRevision).WithMany(p => p.Revision)
                .HasForeignKey(d => d.DecisionRevisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Revision__Decisi__2704CA5F");

            entity.HasOne(d => d.Revisor).WithMany(p => p.Revision)
                .HasForeignKey(d => d.RevisorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Revision__Reviso__2610A626");

            entity.HasOne(d => d.Solicitud).WithMany(p => p.Revision)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK__Revision__Solici__251C81ED");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rol__3214EC078E8A801F");

            entity.HasIndex(e => e.Nombre, "UQ__Rol__75E3EFCFFCF6CA22").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Solicitud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Solicitu__3214EC07A784C5E9");

            entity.HasIndex(e => e.EstadoSolicitudId, "IX_Solicitud_Estado");

            entity.HasIndex(e => e.FechaSolicitud, "IX_Solicitud_FechaSolicitud");

            entity.HasIndex(e => e.MedicoId, "IX_Solicitud_MedicoId");

            entity.HasIndex(e => e.PacienteId, "IX_Solicitud_PacienteId");

            entity.Property(e => e.FechaAprobacionRechazo).HasColumnType("datetime");
            entity.Property(e => e.FechaRecepcion).HasColumnType("datetime");
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Acompanante).WithMany(p => p.Solicitud)
                .HasForeignKey(d => d.AcompananteId)
                .HasConstraintName("FK__Solicitud__Acomp__0697FACD");

            entity.HasOne(d => d.EstadoSolicitud).WithMany(p => p.Solicitud)
                .HasForeignKey(d => d.EstadoSolicitudId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Solicitud__Estad__0880433F");

            entity.HasOne(d => d.FuncionarioRecibe).WithMany(p => p.Solicitud)
                .HasForeignKey(d => d.FuncionarioRecibeId)
                .HasConstraintName("FK__Solicitud__Funci__078C1F06");

            entity.HasOne(d => d.Medico).WithMany(p => p.Solicitud)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Solicitud__Medic__05A3D694");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Solicitud)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Solicitud__Pacie__04AFB25B");
        });

        modelBuilder.Entity<SolicitudDiagnostico>(entity =>
        {
            entity.HasKey(e => new { e.SolicitudId, e.DiagnosticoId }).HasName("PK__Solicitu__3C49881299E608A4");

            entity.Property(e => e.EsPrimario).HasDefaultValue(false);
            entity.Property(e => e.Observaciones).HasMaxLength(300);

            entity.HasOne(d => d.Diagnostico).WithMany(p => p.SolicitudDiagnostico)
                .HasForeignKey(d => d.DiagnosticoId)
                .HasConstraintName("FK__Solicitud__Diagn__0D44F85C");

            entity.HasOne(d => d.Solicitud).WithMany(p => p.SolicitudDiagnostico)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK__Solicitud__Solic__0C50D423");
        });

        modelBuilder.Entity<TipoContacto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TipoCont__3214EC07DCDDF5C6");

            entity.HasIndex(e => e.Nombre, "UQ__TipoCont__75E3EFCF12C9498A").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<TipoProducto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TipoProd__3214EC077766BD1E");

            entity.HasIndex(e => e.Nombre, "UQ__TipoProd__75E3EFCF2DF5D570").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Tratamiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tratamie__3214EC0790222EF2");

            entity.HasIndex(e => e.SolicitudId, "IX_Tratamiento_SolicitudId");

            entity.Property(e => e.CantidadPrescrita).HasMaxLength(100);
            entity.Property(e => e.ConcentracionCbd)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("ConcentracionCBD");
            entity.Property(e => e.ConcentracionOtroCannabinoide1).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ConcentracionOtroCannabinoide2).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ConcentracionThc)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("ConcentracionTHC");
            entity.Property(e => e.Dosis).HasMaxLength(100);
            entity.Property(e => e.FormaFarmaceutica).HasMaxLength(100);
            entity.Property(e => e.FrecuenciaAdministracion).HasMaxLength(100);
            entity.Property(e => e.NombreComercialProducto).HasMaxLength(150);
            entity.Property(e => e.NombreGenericoProducto).HasMaxLength(150);
            entity.Property(e => e.OtraFormaFarmaceuticaDescripcion).HasMaxLength(255);
            entity.Property(e => e.OtraUnidadCbddescripcion)
                .HasMaxLength(50)
                .HasColumnName("OtraUnidadCBDDescripcion");
            entity.Property(e => e.OtraUnidadOtroCannabinoide1Descripcion).HasMaxLength(50);
            entity.Property(e => e.OtraUnidadOtroCannabinoide2Descripcion).HasMaxLength(50);
            entity.Property(e => e.OtraUnidadThcdescripcion)
                .HasMaxLength(50)
                .HasColumnName("OtraUnidadTHCDescripcion");
            entity.Property(e => e.OtraViaAdministracionDescripcion).HasMaxLength(255);
            entity.Property(e => e.OtroCannabinode1).HasMaxLength(100);
            entity.Property(e => e.OtroCannabinode2).HasMaxLength(100);
            entity.Property(e => e.OtroProductoDescripcion).HasMaxLength(255);
            entity.Property(e => e.OtrosCannabinoides).HasMaxLength(200);
            entity.Property(e => e.UnidadCbd)
                .HasMaxLength(20)
                .HasColumnName("UnidadCBD");
            entity.Property(e => e.UnidadCbdid).HasColumnName("UnidadCBDId");
            entity.Property(e => e.UnidadThc)
                .HasMaxLength(20)
                .HasColumnName("UnidadTHC");
            entity.Property(e => e.UnidadThcid).HasColumnName("UnidadTHCId");
            entity.Property(e => e.ViaAdministracion).HasMaxLength(100);

            entity.HasOne(d => d.FormaFarmaceuticaNavigation).WithMany(p => p.Tratamiento)
                .HasForeignKey(d => d.FormaFarmaceuticaId)
                .HasConstraintName("FK_Tratamiento_FormaFarmaceutica");

            entity.HasOne(d => d.FrecuenciaAdministracionNavigation).WithMany(p => p.Tratamiento)
                .HasForeignKey(d => d.FrecuenciaAdministracionId)
                .HasConstraintName("FK_Tratamiento_FrecuenciaAdministracion");

            entity.HasOne(d => d.Solicitud).WithMany(p => p.Tratamiento)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK__Tratamien__Solic__14E61A24");

            entity.HasOne(d => d.TipoProducto).WithMany(p => p.Tratamiento)
                .HasForeignKey(d => d.TipoProductoId)
                .HasConstraintName("FK_Tratamiento_TipoProducto");

            entity.HasOne(d => d.UnidadCbdNavigation).WithMany(p => p.TratamientoUnidadCbdNavigation)
                .HasForeignKey(d => d.UnidadCbdid)
                .HasConstraintName("FK_Tratamiento_UnidadCBD");

            entity.HasOne(d => d.UnidadOtroCannabinoide1).WithMany(p => p.TratamientoUnidadOtroCannabinoide1)
                .HasForeignKey(d => d.UnidadOtroCannabinoide1Id)
                .HasConstraintName("FK_Tratamiento_UnidadOtroCannabinoide1");

            entity.HasOne(d => d.UnidadOtroCannabinoide2).WithMany(p => p.TratamientoUnidadOtroCannabinoide2)
                .HasForeignKey(d => d.UnidadOtroCannabinoide2Id)
                .HasConstraintName("FK_Tratamiento_UnidadOtroCannabinoide2");

            entity.HasOne(d => d.UnidadThcNavigation).WithMany(p => p.TratamientoUnidadThcNavigation)
                .HasForeignKey(d => d.UnidadThcid)
                .HasConstraintName("FK_Tratamiento_UnidadTHC");

            entity.HasOne(d => d.ViaAdministracionNavigation).WithMany(p => p.Tratamiento)
                .HasForeignKey(d => d.ViaAdministracionId)
                .HasConstraintName("FK_Tratamiento_ViaAdministracion");
        });

        modelBuilder.Entity<UnidadConcentracion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UnidadCo__3214EC072C7C0C46");

            entity.HasIndex(e => e.Nombre, "UQ__UnidadCo__75E3EFCFE78154C7").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC07F97849EA");

            entity.HasIndex(e => e.Email, "IX_Usuario_Email");

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D1053405C8D0F9").IsUnique();

            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.ContraseñaHash).HasMaxLength(64);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Salt).HasMaxLength(16);

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__RolId__6BE40491");
        });

        modelBuilder.Entity<ViaAdministracion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ViaAdmin__3214EC07FA817CB3");

            entity.HasIndex(e => e.Nombre, "UQ__ViaAdmin__75E3EFCF3E05F2D3").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
