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

    public virtual DbSet<Acompanante> Acompanantes { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<AuditoriaAccion> AuditoriaAccions { get; set; }

    public virtual DbSet<Certificacion> Certificacions { get; set; }

    public virtual DbSet<Diagnostico> Diagnosticos { get; set; }

    public virtual DbSet<DocumentoAdjunto> DocumentoAdjuntos { get; set; }

    public virtual DbSet<FormaFarmaceutica> FormaFarmaceuticas { get; set; }

    public virtual DbSet<Medico> Medicos { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<PacienteDiagnostico> PacienteDiagnosticos { get; set; }

    public virtual DbSet<Revision> Revisions { get; set; }

    public virtual DbSet<Solicitud> Solicituds { get; set; }

    public virtual DbSet<SolicitudDiagnostico> SolicitudDiagnosticos { get; set; }

    public virtual DbSet<Tratamiento> Tratamientos { get; set; }

    public virtual DbSet<TratamientoCannabinoide> TratamientoCannabinoides { get; set; }

    public virtual DbSet<TratamientoFormaFarmaceutica> TratamientoFormaFarmaceuticas { get; set; }

    public virtual DbSet<TratamientoViaAdministracion> TratamientoViaAdministracions { get; set; }

    public virtual DbSet<UnidadMedidum> UnidadMedida { get; set; }

    public virtual DbSet<ViaAdministracion> ViaAdministracions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Acompanante>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Acompana__3214EC074A673DD5");

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

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AuditoriaAccion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Auditori__3214EC074534F96A");

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
            entity.Property(e => e.UsuarioId).HasMaxLength(450);

            entity.HasOne(d => d.Usuario).WithMany(p => p.AuditoriaAccions)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Auditoria__Usuar__06CD04F7");
        });

        modelBuilder.Entity<Certificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Certific__3214EC07A504BB0E");

            entity.ToTable("Certificacion");

            entity.HasIndex(e => e.CodigoCertificado, "UQ__Certific__AAE1A2C38C09D4C1").IsUnique();

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

            entity.HasOne(d => d.Solicitud).WithMany(p => p.Certificacions)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK_Certificacion_Solicitud");
        });

        modelBuilder.Entity<Diagnostico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Diagnost__3214EC076FE23E4B");

            entity.ToTable("Diagnostico");

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
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC07C9F54989");

            entity.ToTable("DocumentoAdjunto");

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

            entity.HasOne(d => d.Solicitud).WithMany(p => p.DocumentoAdjuntos)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK__Documento__Solic__75A278F5");

            entity.HasOne(d => d.SubidoPorUsuario).WithMany(p => p.DocumentoAdjuntos)
                .HasForeignKey(d => d.SubidoPorUsuarioId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Documento__Subid__76969D2E");
        });

        modelBuilder.Entity<FormaFarmaceutica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FormaFar__3214EC07BD81FEAC");

            entity.ToTable("FormaFarmaceutica");

            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Medico__3214EC071E998E65");

            entity.ToTable("Medico");

            entity.HasIndex(e => e.NumeroRegistroIdoneidad, "UQ_Medico_Idoneidad").IsUnique();

            entity.Property(e => e.CorreoElectronico).HasMaxLength(100);
            entity.Property(e => e.Disciplina).HasMaxLength(50);
            entity.Property(e => e.Especialidad).HasMaxLength(100);
            entity.Property(e => e.InstalacionSalud).HasMaxLength(150);
            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
            entity.Property(e => e.NumeroRegistroIdoneidad).HasMaxLength(50);
            entity.Property(e => e.NumeroTelefono).HasMaxLength(20);
            entity.Property(e => e.UsuarioId).HasMaxLength(450);

            entity.HasOne(d => d.Usuario).WithMany(p => p.Medicos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Medico_Usuario");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Paciente__3214EC07EC577B09");

            entity.ToTable("Paciente");

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

            entity.HasOne(d => d.Usuario).WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Paciente_Usuario");
        });

        modelBuilder.Entity<PacienteDiagnostico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Paciente__3214EC07AC1D66E3");

            entity.ToTable("PacienteDiagnostico");

            entity.HasIndex(e => new { e.PacienteId, e.DiagnosticoId }, "UQ_PacienteDiagnostico").IsUnique();

            entity.Property(e => e.DiagnosticoLibre).HasMaxLength(150);
            entity.Property(e => e.Observaciones).HasMaxLength(300);
            entity.Property(e => e.TratamientoRecibido).HasMaxLength(300);

            entity.HasOne(d => d.Diagnostico).WithMany(p => p.PacienteDiagnosticos)
                .HasForeignKey(d => d.DiagnosticoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PD_Diagnostico");

            entity.HasOne(d => d.Paciente).WithMany(p => p.PacienteDiagnosticos)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK_PD_Paciente");
        });

        modelBuilder.Entity<Revision>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Revision__3214EC07897DCB45");

            entity.ToTable("Revision");

            entity.Property(e => e.Decision).HasMaxLength(50);
            entity.Property(e => e.FechaRevision)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RevisorId).HasMaxLength(450);
            entity.Property(e => e.TipoRevision).HasMaxLength(50);

            entity.HasOne(d => d.Revisor).WithMany(p => p.Revisions)
                .HasForeignKey(d => d.RevisorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Revision__Reviso__7B5B524B");

            entity.HasOne(d => d.Solicitud).WithMany(p => p.Revisions)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK__Revision__Solici__7A672E12");
        });

        modelBuilder.Entity<Solicitud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Solicitu__3214EC07778FA50F");

            entity.ToTable("Solicitud");

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

            entity.HasOne(d => d.Acompanante).WithMany(p => p.Solicituds)
                .HasForeignKey(d => d.AcompananteId)
                .HasConstraintName("FK_Solicitud_Acompanante");

            entity.HasOne(d => d.FuncionarioRecibe).WithMany(p => p.Solicituds)
                .HasForeignKey(d => d.FuncionarioRecibeId)
                .HasConstraintName("FK_Solicitud_Funcionario");

            entity.HasOne(d => d.Medico).WithMany(p => p.Solicituds)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitud_Medico");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Solicituds)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitud_Paciente");
        });

        modelBuilder.Entity<SolicitudDiagnostico>(entity =>
        {
            entity.HasKey(e => new { e.SolicitudId, e.DiagnosticoId }).HasName("PK__Solicitu__3C4988124C0091A3");

            entity.ToTable("SolicitudDiagnostico");

            entity.Property(e => e.EsPrimario).HasDefaultValue(false);
            entity.Property(e => e.Observaciones).HasMaxLength(300);

            entity.HasOne(d => d.Diagnostico).WithMany(p => p.SolicitudDiagnosticos)
                .HasForeignKey(d => d.DiagnosticoId)
                .HasConstraintName("FK__Solicitud__Diagn__628FA481");

            entity.HasOne(d => d.Solicitud).WithMany(p => p.SolicitudDiagnosticos)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK__Solicitud__Solic__619B8048");
        });

        modelBuilder.Entity<Tratamiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tratamie__3214EC070D8E8EEC");

            entity.ToTable("Tratamiento");

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

            entity.HasOne(d => d.Solicitud).WithMany(p => p.Tratamientos)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK_Tratamiento_Solicitud");

            entity.HasOne(d => d.UnidadCbd).WithMany(p => p.TratamientoUnidadCbds)
                .HasForeignKey(d => d.UnidadCbdid)
                .HasConstraintName("FK_Tratamiento_UnidadCBD");

            entity.HasOne(d => d.UnidadThc).WithMany(p => p.TratamientoUnidadThcs)
                .HasForeignKey(d => d.UnidadThcid)
                .HasConstraintName("FK_Tratamiento_UnidadTHC");
        });

        modelBuilder.Entity<TratamientoCannabinoide>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tratamie__3214EC07ADCAC756");

            entity.ToTable("TratamientoCannabinoide");

            entity.Property(e => e.Observacion).HasMaxLength(100);
            entity.Property(e => e.Tipo).HasMaxLength(50);

            entity.HasOne(d => d.Tratamiento).WithMany(p => p.TratamientoCannabinoides)
                .HasForeignKey(d => d.TratamientoId)
                .HasConstraintName("FK_TC_Tratamiento");
        });

        modelBuilder.Entity<TratamientoFormaFarmaceutica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tratamie__3214EC07BAF0EE58");

            entity.ToTable("TratamientoFormaFarmaceutica");

            entity.HasOne(d => d.FormaFarmaceutica).WithMany(p => p.TratamientoFormaFarmaceuticas)
                .HasForeignKey(d => d.FormaFarmaceuticaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tratamien__Forma__6B24EA82");

            entity.HasOne(d => d.Tratamiento).WithMany(p => p.TratamientoFormaFarmaceuticas)
                .HasForeignKey(d => d.TratamientoId)
                .HasConstraintName("FK__Tratamien__Trata__6A30C649");
        });

        modelBuilder.Entity<TratamientoViaAdministracion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tratamie__3214EC074F3261F5");

            entity.ToTable("TratamientoViaAdministracion");

            entity.HasOne(d => d.Tratamiento).WithMany(p => p.TratamientoViaAdministracions)
                .HasForeignKey(d => d.TratamientoId)
                .HasConstraintName("FK__Tratamien__Trata__6E01572D");

            entity.HasOne(d => d.ViaAdministracion).WithMany(p => p.TratamientoViaAdministracions)
                .HasForeignKey(d => d.ViaAdministracionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tratamien__ViaAd__6EF57B66");
        });

        modelBuilder.Entity<UnidadMedidum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UnidadMe__3214EC075EFA3C6D");

            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Simbolo).HasMaxLength(20);
        });

        modelBuilder.Entity<ViaAdministracion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ViaAdmin__3214EC07040EC3E6");

            entity.ToTable("ViaAdministracion");

            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
