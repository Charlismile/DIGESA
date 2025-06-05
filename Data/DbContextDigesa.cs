using DIGESA.Models.Entities.DIGESA;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Data;

public partial class DbContextDigesa : DbContext
{
    public DbContextDigesa()
    {
    }

    public DbContextDigesa(DbContextOptions<DbContextDigesa> options)
        : base(options)
    {
    }

    public virtual DbSet<Acompanante> Acompanantes { get; set; }

    public virtual DbSet<AuditoriaAccion> AuditoriaAcciones { get; set; }

    public virtual DbSet<Certificacion> Certificaciones { get; set; }

    public virtual DbSet<Diagnostico> Diagnosticos { get; set; }

    public virtual DbSet<DocumentoAdjunto> DocumentoAdjuntos { get; set; }

    public virtual DbSet<Medico> Medicos { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<PacienteDiagnostico> PacienteDiagnosticos { get; set; }

    public virtual DbSet<Revision> Revisiones { get; set; }

    public virtual DbSet<Solicitud> Solicitudes { get; set; }

    public virtual DbSet<SolicitudDiagnostico> SolicitudDiagnosticos { get; set; }

    public virtual DbSet<Tratamiento> Tratamientos { get; set; }

    public virtual DbSet<TratamientoCannabinoide> TratamientoCannabinoides { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Acompanante>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Acompana__3214EC070781EC52");

            entity.ToTable("Acompanante");

            entity.HasIndex(e => new { e.TipoDocumento, e.NumeroDocumento }, "UQ_Acompanante_Documento").IsUnique();

            entity.Property(e => e.Nacionalidad).HasMaxLength(50);
            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
            entity.Property(e => e.NumeroDocumento).HasMaxLength(30);
            entity.Property(e => e.Parentesco).HasMaxLength(50);
            entity.Property(e => e.TipoDocumento).HasMaxLength(20);

            entity.HasOne(d => d.Paciente).WithMany(p => p.Acompanantes)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK_Acompanante_Paciente");
        });

        modelBuilder.Entity<AuditoriaAccion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Auditori__3214EC07629BD20C");

            entity.ToTable("AuditoriaAccion");

            entity.Property(e => e.AccionRealizada).HasMaxLength(500);
            entity.Property(e => e.FechaAccion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(50)
                .HasColumnName("IPAddress");
            entity.Property(e => e.NombreTablaAfectada).HasMaxLength(128);
            entity.Property(e => e.UserAgent).HasMaxLength(255);

            entity.HasOne(d => d.Usuarios).WithMany(p => p.AuditoriaAcciones)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_AuditoriaAccion_Usuario");
        });

        modelBuilder.Entity<Certificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Certific__3214EC07CC73C6B6");

            entity.ToTable("Certificacion");

            entity.HasIndex(e => e.CodigoCertificado, "IX_Certificacion_CodigoCertificado");

            entity.HasIndex(e => e.SolicitudId, "IX_Certificacion_SolicitudId");

            entity.HasIndex(e => e.SolicitudId, "UQ__Certific__85E95DC65D93D55B").IsUnique();

            entity.HasIndex(e => e.CodigoCertificado, "UQ__Certific__AAE1A2C30EF52C63").IsUnique();

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
            entity.Property(e => e.RutaArchivoQr)
                .HasMaxLength(255)
                .HasColumnName("RutaArchivoQR");

            entity.HasOne(d => d.Solicitudes).WithOne(p => p.Certificacion)
                .HasForeignKey<Certificacion>(d => d.SolicitudId)
                .HasConstraintName("FK_Certificacion_Solicitud");
        });

        modelBuilder.Entity<Diagnostico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Diagnost__3214EC078B4908F5");

            entity.ToTable("Diagnostico");

            entity.HasIndex(e => e.Nombre, "UQ_Diagnostico_Nombre").IsUnique();

            entity.HasIndex(e => e.CodigoCie10, "UQ__Diagnost__D3E9319FAE340D91").IsUnique();

            entity.Property(e => e.CodigoCie10)
                .HasMaxLength(10)
                .HasColumnName("CodigoCIE10");
            entity.Property(e => e.Descripcion).HasMaxLength(300);
            entity.Property(e => e.Nombre).HasMaxLength(150);
        });

        modelBuilder.Entity<DocumentoAdjunto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC07452E94C0");

            entity.ToTable("DocumentoAdjunto");

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

            entity.HasOne(d => d.Solicitudes).WithMany(p => p.DocumentoAdjuntos)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK_DocumentoAdjunto_Solicitud");

            entity.HasOne(d => d.SubidoPorUsuario).WithMany(p => p.DocumentoAdjuntos)
                .HasForeignKey(d => d.SubidoPorUsuarioId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_DocumentoAdjunto_Usuario");
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Medico__3214EC075F86DCFD");

            entity.ToTable("Medico");

            entity.HasIndex(e => e.NombreCompleto, "IX_Medico_NombreCompleto");

            entity.HasIndex(e => e.NumeroRegistroIdoneidad, "IX_Medico_NumeroRegistroIdoneidad");

            entity.HasIndex(e => e.UsuarioId, "UQ__Medico__2B3DE7B9FA2280CF").IsUnique();

            entity.HasIndex(e => e.NumeroRegistroIdoneidad, "UQ__Medico__489C33DDD3AE4B7A").IsUnique();

            entity.Property(e => e.CorreoElectronico).HasMaxLength(100);
            entity.Property(e => e.Disciplina).HasMaxLength(50);
            entity.Property(e => e.Especialidad).HasMaxLength(100);
            entity.Property(e => e.InstalacionSalud).HasMaxLength(150);
            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
            entity.Property(e => e.NumeroRegistroIdoneidad).HasMaxLength(50);
            entity.Property(e => e.NumeroTelefono).HasMaxLength(20);

            entity.HasOne(d => d.Usuarios).WithOne(p => p.Medico)
                .HasForeignKey<Medico>(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Medico_Usuario");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Paciente__3214EC07000EA0E7");

            entity.ToTable("Paciente");

            entity.HasIndex(e => e.NombreCompleto, "IX_Paciente_NombreCompleto");

            entity.HasIndex(e => e.NumeroDocumento, "IX_Paciente_NumeroDocumento");

            entity.HasIndex(e => new { e.TipoDocumento, e.NumeroDocumento }, "UQ_Paciente_Documento").IsUnique();

            entity.Property(e => e.CorreoElectronico).HasMaxLength(100);
            entity.Property(e => e.DireccionResidencia).HasMaxLength(300);
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

            entity.HasOne(d => d.Usuarios).WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Paciente_Usuario");
        });

        modelBuilder.Entity<PacienteDiagnostico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Paciente__3214EC079D2E26BA");

            entity.ToTable("PacienteDiagnostico");

            entity.HasIndex(e => e.DiagnosticoId, "IX_PacienteDiagnostico_DiagnosticoId");

            entity.HasIndex(e => e.PacienteId, "IX_PacienteDiagnostico_PacienteId");

            entity.HasIndex(e => new { e.PacienteId, e.DiagnosticoId }, "UQ_PacienteDiagnostico_Paciente_Diagnostico").IsUnique();

            entity.Property(e => e.DiagnosticoLibre).HasMaxLength(150);
            entity.Property(e => e.Observaciones).HasMaxLength(300);
            entity.Property(e => e.TratamientoRecibido).HasMaxLength(300);

            entity.HasOne(d => d.Diagnosticos).WithMany(p => p.PacienteDiagnosticos)
                .HasForeignKey(d => d.DiagnosticoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PacienteDiagnostico_Diagnostico");

            entity.HasOne(d => d.Pacientes).WithMany(p => p.PacienteDiagnosticos)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK_PacienteDiagnostico_Paciente");
        });

        modelBuilder.Entity<Revision>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Revision__3214EC0715726975");

            entity.ToTable("Revision");

            entity.HasIndex(e => e.RevisorId, "IX_Revision_RevisorId");

            entity.HasIndex(e => e.SolicitudId, "IX_Revision_SolicitudId");

            entity.Property(e => e.Decision).HasMaxLength(50);
            entity.Property(e => e.FechaRevision)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TipoRevision).HasMaxLength(50);

            entity.HasOne(d => d.Revisor).WithMany(p => p.Revisiones)
                .HasForeignKey(d => d.RevisorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Revision_Revisor");

            entity.HasOne(d => d.Solicitudes).WithMany(p => p.Revisiones)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK_Revision_Solicitud");
        });

        modelBuilder.Entity<Solicitud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Solicitu__3214EC0759314E04");

            entity.ToTable("Solicitud");

            entity.HasIndex(e => e.Estado, "IX_Solicitud_Estado");

            entity.HasIndex(e => e.FechaSolicitud, "IX_Solicitud_FechaSolicitud");

            entity.HasIndex(e => e.MedicoId, "IX_Solicitud_MedicoId");

            entity.HasIndex(e => e.PacienteId, "IX_Solicitud_PacienteId");

            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaAprobacionRechazo).HasColumnType("datetime");
            entity.Property(e => e.FechaRecepcion).HasColumnType("datetime");
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreFirmante).HasMaxLength(150);

            entity.HasOne(d => d.Acompanante).WithMany(p => p.Solicitudes)
                .HasForeignKey(d => d.AcompananteId)
                .HasConstraintName("FK_Solicitud_Acompanante");

            entity.HasOne(d => d.FuncionarioRecibe).WithMany(p => p.Solicitudes)
                .HasForeignKey(d => d.FuncionarioRecibeId)
                .HasConstraintName("FK_Solicitud_Funcionario");

            entity.HasOne(d => d.Medico).WithMany(p => p.Solicitudes)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitud_Medico");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Solicitudes)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitud_Paciente");
        });

        modelBuilder.Entity<SolicitudDiagnostico>(entity =>
        {
            entity.HasKey(e => new { e.SolicitudId, e.DiagnosticoId }).HasName("PK__Solicitu__3C4988123C68F207");

            entity.ToTable("SolicitudDiagnostico");

            entity.Property(e => e.EsPrimario).HasDefaultValue(false);
            entity.Property(e => e.Observaciones).HasMaxLength(300);

            entity.HasOne(d => d.Diagnosticos).WithMany(p => p.SolicitudDiagnosticos)
                .HasForeignKey(d => d.DiagnosticoId)
                .HasConstraintName("FK_SolicitudDiagnostico_Diagnostico");

            entity.HasOne(d => d.Solicitudes).WithMany(p => p.SolicitudDiagnosticos)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK_SolicitudDiagnostico_Solicitud");
        });

        modelBuilder.Entity<Tratamiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tratamie__3214EC072DE06B77");

            entity.ToTable("Tratamiento");

            entity.HasIndex(e => e.SolicitudId, "IX_Tratamiento_SolicitudId");

            entity.Property(e => e.CannabinoidesSeleccionados).HasMaxLength(300);
            entity.Property(e => e.CantidadPrescrita).HasMaxLength(100);
            entity.Property(e => e.ConcentracionCbd)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("ConcentracionCBD");
            entity.Property(e => e.ConcentracionThc)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("ConcentracionTHC");
            entity.Property(e => e.Dosis).HasMaxLength(100);
            entity.Property(e => e.FormaFarmaceutica).HasMaxLength(100);
            entity.Property(e => e.FormaFarmaceuticaExtra).HasMaxLength(100);
            entity.Property(e => e.FrecuenciaAdministracion).HasMaxLength(100);
            entity.Property(e => e.NombreComercialProducto).HasMaxLength(150);
            entity.Property(e => e.NombreGenericoProducto).HasMaxLength(150);
            entity.Property(e => e.OtrosCannabinoides).HasMaxLength(200);
            entity.Property(e => e.UnidadCbd)
                .HasMaxLength(20)
                .HasColumnName("UnidadCBD");
            entity.Property(e => e.UnidadThc)
                .HasMaxLength(20)
                .HasColumnName("UnidadTHC");
            entity.Property(e => e.ViaAdministracion).HasMaxLength(100);

            entity.HasOne(d => d.Solicitudes).WithMany(p => p.Tratamientos)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK_Tratamiento_Solicitud");
        });

        modelBuilder.Entity<TratamientoCannabinoide>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tratamie__3214EC0795FBC710");

            entity.ToTable("TratamientoCannabinoide");

            entity.Property(e => e.Observacion).HasMaxLength(100);
            entity.Property(e => e.Tipo).HasMaxLength(50);

            entity.HasOne(d => d.Tratamientos).WithMany(p => p.TratamientoCannabinoides)
                .HasForeignKey(d => d.TratamientoId)
                .HasConstraintName("FK_TratamientoCannabinoide_Tratamiento");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC078DAED041");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Email, "IX_Usuario_Email");

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D105341BF81BD5").IsUnique();

            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.ContraseñaHash).HasMaxLength(64);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Rol).HasMaxLength(50);
            entity.Property(e => e.Salt).HasMaxLength(16);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
