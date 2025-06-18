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

    public virtual DbSet<Diagnostico> Diagnostico { get; set; }

    public virtual DbSet<DocumentoAdjunto> DocumentoAdjunto { get; set; }

    public virtual DbSet<FormaFarmaceutica> FormaFarmaceutica { get; set; }

    public virtual DbSet<Medico> Medico { get; set; }

    public virtual DbSet<Paciente> Paciente { get; set; }

    public virtual DbSet<PacienteDiagnostico> PacienteDiagnostico { get; set; }

    public virtual DbSet<Revision> Revision { get; set; }

    public virtual DbSet<Solicitud> Solicitud { get; set; }

    public virtual DbSet<SolicitudDiagnostico> SolicitudDiagnostico { get; set; }

    public virtual DbSet<Tratamiento> Tratamiento { get; set; }

    public virtual DbSet<TratamientoCannabinoide> TratamientoCannabinoide { get; set; }

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
            entity.HasKey(e => e.Id).HasName("PK__Acompana__3214EC075DAF836C");

            entity.HasIndex(e => new { e.TipoDocumento, e.NumeroDocumento }, "UQ_Acompanante_Documento").IsUnique();

            entity.Property(e => e.Nacionalidad).HasMaxLength(50);
            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
            entity.Property(e => e.NumeroDocumento).HasMaxLength(30);
            entity.Property(e => e.Parentesco).HasMaxLength(50);
            entity.Property(e => e.TipoDocumento).HasMaxLength(20);

            entity.HasOne(d => d.Paciente).WithMany(p => p.Acompanante)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK_Acompanante_Paciente");
        });

        modelBuilder.Entity<AuditoriaAccion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Auditori__3214EC07E59971FD");

            entity.Property(e => e.AccionRealizada).HasMaxLength(500);
            entity.Property(e => e.FechaAccion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(50)
                .HasColumnName("IPAddress");
            entity.Property(e => e.NombreTablaAfectada).HasMaxLength(128);
            entity.Property(e => e.UserAgent).HasMaxLength(255);
            entity.Property(e => e.UsuarioId).HasMaxLength(450);
        });

        modelBuilder.Entity<Certificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Certific__3214EC07E5CDB8CD");

            entity.HasIndex(e => e.CodigoCertificado, "UQ__Certific__AAE1A2C378CE674B").IsUnique();

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

            entity.HasOne(d => d.Solicitud).WithMany(p => p.Certificacion)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK_Certificacion_Solicitud");
        });

        modelBuilder.Entity<Diagnostico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Diagnost__3214EC07A9410CBF");

            entity.HasIndex(e => e.CodigoCie10, "UQ_Diagnostico_Codigo").IsUnique();

            entity.HasIndex(e => e.Nombre, "UQ_Diagnostico_Nombre").IsUnique();

            entity.Property(e => e.CodigoCie10)
                .HasMaxLength(10)
                .HasColumnName("CodigoCIE10");
            entity.Property(e => e.Descripcion).HasMaxLength(300);
            entity.Property(e => e.Nombre).HasMaxLength(150);
        });

        modelBuilder.Entity<DocumentoAdjunto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC07C2E731A4");

            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.FechaSubida)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreArchivo).HasMaxLength(255);
            entity.Property(e => e.RutaAlmacenamiento).HasMaxLength(500);
            entity.Property(e => e.SubidoPorUsuarioId).HasMaxLength(450);
            entity.Property(e => e.TipoContenidoMime)
                .HasMaxLength(100)
                .HasColumnName("TipoContenidoMIME");
            entity.Property(e => e.TipoDocumento).HasMaxLength(100);

            entity.HasOne(d => d.Solicitud).WithMany(p => p.DocumentoAdjunto)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK__Documento__Solic__0E6E26BF");
        });

        modelBuilder.Entity<FormaFarmaceutica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FormaFar__3214EC07BA8DB969");

            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Medico__3214EC0719EC784E");

            entity.HasIndex(e => e.NumeroRegistroIdoneidad, "UQ_Medico_Idoneidad").IsUnique();

            entity.Property(e => e.CorreoElectronico).HasMaxLength(100);
            entity.Property(e => e.Disciplina).HasMaxLength(50);
            entity.Property(e => e.Especialidad).HasMaxLength(100);
            entity.Property(e => e.InstalacionSalud).HasMaxLength(150);
            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
            entity.Property(e => e.NumeroRegistroIdoneidad).HasMaxLength(50);
            entity.Property(e => e.NumeroTelefono).HasMaxLength(20);
            entity.Property(e => e.UsuarioId).HasMaxLength(450);
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Paciente__3214EC07E652C9FC");

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
            entity.Property(e => e.UsuarioId).HasMaxLength(450);
        });

        modelBuilder.Entity<PacienteDiagnostico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Paciente__3214EC07AF4D704D");

            entity.HasIndex(e => new { e.PacienteId, e.DiagnosticoId }, "UQ_PacienteDiagnostico").IsUnique();

            entity.Property(e => e.DiagnosticoLibre).HasMaxLength(150);
            entity.Property(e => e.Observaciones).HasMaxLength(300);
            entity.Property(e => e.TratamientoRecibido).HasMaxLength(300);

            entity.HasOne(d => d.Diagnostico).WithMany(p => p.PacienteDiagnostico)
                .HasForeignKey(d => d.DiagnosticoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PD_Diagnostico");

            entity.HasOne(d => d.Paciente).WithMany(p => p.PacienteDiagnostico)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK_PD_Paciente");
        });

        modelBuilder.Entity<Revision>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Revision__3214EC07D5F14A85");

            entity.Property(e => e.Decision).HasMaxLength(50);
            entity.Property(e => e.FechaRevision)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RevisorId).HasMaxLength(450);
            entity.Property(e => e.TipoRevision).HasMaxLength(50);

            entity.HasOne(d => d.Solicitud).WithMany(p => p.Revision)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK__Revision__Solici__114A936A");
        });

        modelBuilder.Entity<Solicitud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Solicitu__3214EC07375E08A4");

            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaAprobacionRechazo).HasColumnType("datetime");
            entity.Property(e => e.FechaRecepcion).HasColumnType("datetime");
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FuncionarioRecibeId).HasMaxLength(450);
            entity.Property(e => e.NombreFirmante).HasMaxLength(150);

            entity.HasOne(d => d.Acompanante).WithMany(p => p.Solicitud)
                .HasForeignKey(d => d.AcompananteId)
                .HasConstraintName("FK_Solicitud_Acompanante");

            entity.HasOne(d => d.Medico).WithMany(p => p.Solicitud)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitud_Medico");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Solicitud)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitud_Paciente");
        });

        modelBuilder.Entity<SolicitudDiagnostico>(entity =>
        {
            entity.HasKey(e => new { e.SolicitudId, e.DiagnosticoId }).HasName("PK__Solicitu__3C49881275443A2D");

            entity.Property(e => e.EsPrimario).HasDefaultValue(false);
            entity.Property(e => e.Observaciones).HasMaxLength(300);

            entity.HasOne(d => d.Diagnostico).WithMany(p => p.SolicitudDiagnostico)
                .HasForeignKey(d => d.DiagnosticoId)
                .HasConstraintName("FK__Solicitud__Diagn__160F4887");

            entity.HasOne(d => d.Solicitud).WithMany(p => p.SolicitudDiagnostico)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK__Solicitud__Solic__151B244E");
        });

        modelBuilder.Entity<Tratamiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tratamie__3214EC07F9697A67");

            entity.Property(e => e.CantidadPrescrita).HasMaxLength(100);
            entity.Property(e => e.ConcentracionCbd)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("ConcentracionCBD");
            entity.Property(e => e.ConcentracionThc)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("ConcentracionTHC");
            entity.Property(e => e.Dosis).HasMaxLength(100);
            entity.Property(e => e.FrecuenciaAdministracion).HasMaxLength(100);
            entity.Property(e => e.NombreComercialProducto).HasMaxLength(150);
            entity.Property(e => e.NombreGenericoProducto).HasMaxLength(150);
            entity.Property(e => e.OtrosCannabinoides).HasMaxLength(200);
            entity.Property(e => e.UnidadCbdid).HasColumnName("UnidadCBDId");
            entity.Property(e => e.UnidadThcid).HasColumnName("UnidadTHCId");

            entity.HasOne(d => d.Solicitud).WithMany(p => p.Tratamiento)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK_Tratamiento_Solicitud");

            entity.HasOne(d => d.UnidadCbd).WithMany(p => p.TratamientoUnidadCbd)
                .HasForeignKey(d => d.UnidadCbdid)
                .HasConstraintName("FK_Tratamiento_UnidadCBD");

            entity.HasOne(d => d.UnidadThc).WithMany(p => p.TratamientoUnidadThc)
                .HasForeignKey(d => d.UnidadThcid)
                .HasConstraintName("FK_Tratamiento_UnidadTHC");
        });

        modelBuilder.Entity<TratamientoCannabinoide>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tratamie__3214EC07C3AB5B8E");

            entity.Property(e => e.Observacion).HasMaxLength(100);
            entity.Property(e => e.Tipo).HasMaxLength(50);

            entity.HasOne(d => d.Tratamiento).WithMany(p => p.TratamientoCannabinoide)
                .HasForeignKey(d => d.TratamientoId)
                .HasConstraintName("FK_TC_Tratamiento");
        });

        modelBuilder.Entity<TratamientoFormaFarmaceutica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tratamie__3214EC073E615F15");

            entity.HasOne(d => d.FormaFarmaceutica).WithMany(p => p.TratamientoFormaFarmaceutica)
                .HasForeignKey(d => d.FormaFarmaceuticaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tratamien__Forma__1BC821DD");

            entity.HasOne(d => d.Tratamiento).WithMany(p => p.TratamientoFormaFarmaceutica)
                .HasForeignKey(d => d.TratamientoId)
                .HasConstraintName("FK__Tratamien__Trata__1AD3FDA4");
        });

        modelBuilder.Entity<TratamientoViaAdministracion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tratamie__3214EC075B5453B6");

            entity.HasOne(d => d.Tratamiento).WithMany(p => p.TratamientoViaAdministracion)
                .HasForeignKey(d => d.TratamientoId)
                .HasConstraintName("FK__Tratamien__Trata__1CBC4616");

            entity.HasOne(d => d.ViaAdministracion).WithMany(p => p.TratamientoViaAdministracion)
                .HasForeignKey(d => d.ViaAdministracionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tratamien__ViaAd__1DB06A4F");
        });

        modelBuilder.Entity<UnidadMedida>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UnidadMe__3214EC074500B66D");

            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Simbolo).HasMaxLength(20);
        });

        modelBuilder.Entity<ViaAdministracion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ViaAdmin__3214EC0762A91733");

            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
