using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DIGESA;

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

    public virtual DbSet<Certificacion> Certificacions { get; set; }

    public virtual DbSet<Diagnostico> Diagnosticos { get; set; }

    public virtual DbSet<DocumentoAdjunto> DocumentoAdjuntos { get; set; }

    public virtual DbSet<Medico> Medicos { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<RevisionMedica> RevisionMedicas { get; set; }

    public virtual DbSet<Solicitud> Solicituds { get; set; }

    public virtual DbSet<Tratamiento> Tratamientos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Acompanante>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Acompana__3214EC079F0BC3B2");

            entity.ToTable("Acompanante");

            entity.Property(e => e.Nacionalidad).HasMaxLength(50);
            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
            entity.Property(e => e.NumeroDocumento).HasMaxLength(30);
            entity.Property(e => e.Parentesco).HasMaxLength(50);
            entity.Property(e => e.TipoDocumento).HasMaxLength(20);

            entity.HasOne(d => d.Paciente).WithMany(p => p.Acompanantes)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Acompanan__Pacie__2C3393D0");
        });

        modelBuilder.Entity<Certificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Certific__3214EC07A8647649");

            entity.ToTable("Certificacion");

            entity.Property(e => e.CodigoQr)
                .HasMaxLength(500)
                .HasColumnName("CodigoQR");
            entity.Property(e => e.FechaEmision)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImagenQr).HasColumnName("ImagenQR");
            entity.Property(e => e.NombreArchivoQr)
                .HasMaxLength(255)
                .HasColumnName("NombreArchivoQR");
            entity.Property(e => e.VigenciaHasta).HasColumnType("datetime");

            entity.HasOne(d => d.Solicitud).WithMany(p => p.Certificacions)
                .HasForeignKey(d => d.SolicitudId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Certifica__Solic__4222D4EF");
        });

        modelBuilder.Entity<Diagnostico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Diagnost__3214EC077924C8F2");

            entity.ToTable("Diagnostico");

            entity.Property(e => e.Descripcion).HasMaxLength(300);
            entity.Property(e => e.EsOtro).HasDefaultValue(false);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<DocumentoAdjunto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC079B85A137");

            entity.ToTable("DocumentoAdjunto");

            entity.Property(e => e.FechaSubida)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreArchivo).HasMaxLength(255);
            entity.Property(e => e.RutaAlmacenamiento).HasMaxLength(500);
            entity.Property(e => e.TipoContenido).HasMaxLength(100);

            entity.HasOne(d => d.Solicitud).WithMany(p => p.DocumentoAdjuntos)
                .HasForeignKey(d => d.SolicitudId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Documento__Solic__45F365D3");
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Medico__3214EC078E73AB95");

            entity.ToTable("Medico");

            entity.HasIndex(e => e.NumeroRegistroIdoneidad, "UQ__Medico__489C33DD0181D016").IsUnique();

            entity.Property(e => e.Disciplina).HasMaxLength(50);
            entity.Property(e => e.Especialidad).HasMaxLength(100);
            entity.Property(e => e.InstalacionSalud).HasMaxLength(150);
            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
            entity.Property(e => e.NumeroRegistroIdoneidad).HasMaxLength(50);
            entity.Property(e => e.NumeroTelefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Paciente__3214EC072FF81FC5");

            entity.ToTable("Paciente");

            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.Genero).HasMaxLength(20);
            entity.Property(e => e.InstalacionSalud).HasMaxLength(150);
            entity.Property(e => e.MotivoRequerimiento).HasMaxLength(100);
            entity.Property(e => e.Nacionalidad).HasMaxLength(100);
            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
            entity.Property(e => e.NumeroDocumento).HasMaxLength(30);
            entity.Property(e => e.RegionSalud).HasMaxLength(100);
            entity.Property(e => e.RequiereAcompanante).HasDefaultValue(false);
            entity.Property(e => e.TelefonoLaboral).HasMaxLength(20);
            entity.Property(e => e.TelefonoPersonal).HasMaxLength(20);
            entity.Property(e => e.TelefonoResidencial).HasMaxLength(20);
            entity.Property(e => e.TipoDiscapacidad).HasMaxLength(200);
            entity.Property(e => e.TipoDocumento).HasMaxLength(20);

            entity.HasOne(d => d.Usuario).WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Paciente__Usuari__29572725");
        });

        modelBuilder.Entity<RevisionMedica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Revision__3214EC07D85D5904");

            entity.ToTable("RevisionMedica");

            entity.Property(e => e.FechaRevision)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Revisor).WithMany(p => p.RevisionMedicas)
                .HasForeignKey(d => d.RevisorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RevisionM__Revis__4AB81AF0");

            entity.HasOne(d => d.Solicitud).WithMany(p => p.RevisionMedicas)
                .HasForeignKey(d => d.SolicitudId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RevisionM__Solic__49C3F6B7");
        });

        modelBuilder.Entity<Solicitud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Solicitu__3214EC07D64042C1");

            entity.ToTable("Solicitud");

            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Medico).WithMany(p => p.Solicituds)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Solicitud__Medic__37A5467C");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Solicituds)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Solicitud__Pacie__36B12243");

            entity.HasMany(d => d.Diagnosticos).WithMany(p => p.Solicituds)
                .UsingEntity<Dictionary<string, object>>(
                    "SolicitudDiagnostico",
                    r => r.HasOne<Diagnostico>().WithMany()
                        .HasForeignKey("DiagnosticoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Solicitud__Diagn__3B75D760"),
                    l => l.HasOne<Solicitud>().WithMany()
                        .HasForeignKey("SolicitudId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Solicitud__Solic__3A81B327"),
                    j =>
                    {
                        j.HasKey("SolicitudId", "DiagnosticoId").HasName("PK__Solicitu__3C498812CC843399");
                        j.ToTable("SolicitudDiagnostico");
                    });
        });

        modelBuilder.Entity<Tratamiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tratamie__3214EC0749A03003");

            entity.ToTable("Tratamiento");

            entity.Property(e => e.ConcentracionCbd)
                .HasMaxLength(50)
                .HasColumnName("ConcentracionCBD");
            entity.Property(e => e.ConcentracionThc)
                .HasMaxLength(50)
                .HasColumnName("ConcentracionTHC");
            entity.Property(e => e.Dosis).HasMaxLength(100);
            entity.Property(e => e.FormaFarmaceutica).HasMaxLength(100);
            entity.Property(e => e.Frecuencia).HasMaxLength(100);
            entity.Property(e => e.ViaAdministracion).HasMaxLength(100);

            entity.HasOne(d => d.Solicitud).WithMany(p => p.Tratamientos)
                .HasForeignKey(d => d.SolicitudId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tratamien__Solic__3E52440B");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC072936E35D");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D1053498985F55").IsUnique();

            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.Contraseña).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Rol).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
