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

    public virtual DbSet<EstadoSolicitud> EstadoSolicitud { get; set; }

    public virtual DbSet<TbAcompanantePaciente> TbAcompanantePaciente { get; set; }

    public virtual DbSet<TbCorregimiento> TbCorregimiento { get; set; }

    public virtual DbSet<TbDeclaracionJurada> TbDeclaracionJurada { get; set; }

    public virtual DbSet<TbDistrito> TbDistrito { get; set; }

    public virtual DbSet<TbDocumentoMedico> TbDocumentoMedico { get; set; }

    public virtual DbSet<TbInstalacionSalud> TbInstalacionSalud { get; set; }

    public virtual DbSet<TbMedicoPaciente> TbMedicoPaciente { get; set; }

    public virtual DbSet<TbNombreProductoPaciente> TbNombreProductoPaciente { get; set; }

    public virtual DbSet<TbPaciente> TbPaciente { get; set; }

    public virtual DbSet<TbPacienteComorbilidad> TbPacienteComorbilidad { get; set; }

    public virtual DbSet<TbPacienteDiagnostico> TbPacienteDiagnostico { get; set; }

    public virtual DbSet<TbProvincia> TbProvincia { get; set; }

    public virtual DbSet<TbRegionSalud> TbRegionSalud { get; set; }

    public virtual DbSet<TbSolRegCannabis> TbSolRegCannabis { get; set; }

    public virtual DbSet<TbSolRegCannabisHistorial> TbSolRegCannabisHistorial { get; set; }

    public virtual DbSet<TbSolSecuencia> TbSolSecuencia { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EstadoSolicitud>(entity =>
        {
            entity.HasKey(e => e.Estado).HasName("PK__EstadoSo__36DF552EC1858A2D");

            entity.Property(e => e.Estado).ValueGeneratedNever();
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbAcompanantePaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbAcompa__3214EC076F610B4F");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Nacionalidad)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NumeroDocumento).HasMaxLength(100);
            entity.Property(e => e.Parentesco)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PrimerApellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PrimerNombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SegundoApellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SegundoNombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbAcompanantePaciente)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK__TbAcompan__Pacie__36B12243");
        });

        modelBuilder.Entity<TbCorregimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbCorreg__3214EC0720BCC5F1");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.NombreCorregimiento)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Distrito).WithMany(p => p.TbCorregimiento)
                .HasForeignKey(d => d.DistritoId)
                .HasConstraintName("FK__TbCorregi__Distr__2B3F6F97");
        });

        modelBuilder.Entity<TbDeclaracionJurada>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbDeclar__3214EC073C59DD2B");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Detalle)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.NombreDeclarante)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.SolRegCannabis).WithMany(p => p.TbDeclaracionJurada)
                .HasForeignKey(d => d.SolRegCannabisId)
                .HasConstraintName("FK__TbDeclara__SolRe__4AB81AF0");
        });

        modelBuilder.Entity<TbDistrito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbDistri__3214EC0787D1227B");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.NombreDistrito)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Provincia).WithMany(p => p.TbDistrito)
                .HasForeignKey(d => d.ProvinciaId)
                .HasConstraintName("FK__TbDistrit__Provi__286302EC");
        });

        modelBuilder.Entity<TbDocumentoMedico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbDocume__3214EC071825309D");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Categoria)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaSubidaUtc).HasColumnType("datetime");
            entity.Property(e => e.FileNameStored).HasMaxLength(200);
            entity.Property(e => e.NombreOriginal)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Url).HasMaxLength(300);

            entity.HasOne(d => d.Medico).WithMany(p => p.TbDocumentoMedico)
                .HasForeignKey(d => d.MedicoId)
                .HasConstraintName("FK__TbDocumen__Medic__5165187F");
        });

        modelBuilder.Entity<TbInstalacionSalud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbInstal__3214EC075134E723");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.NombreInstalacion)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbMedicoPaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbMedico__3214EC074E2F9E7C");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.MedicoDisciplina)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.MedicoIdoneidad)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MedicoInstalacion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PrimerApellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PrimerNombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Instalacion).WithMany(p => p.TbMedicoPaciente)
                .HasForeignKey(d => d.InstalacionId)
                .HasConstraintName("FK__TbMedico__Instal__4222D4EF");

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbMedicoPaciente)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK_TbMedicoPaciente_Paciente");

            entity.HasOne(d => d.Region).WithMany(p => p.TbMedicoPaciente)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("FK__TbMedico__Region__412EB0B6");
        });

        modelBuilder.Entity<TbNombreProductoPaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbNombre__3214EC0795EE4B9D");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CantidadConcentracion).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Concentracion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FormaFarmaceutica)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Parentesco)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbNombreProductoPaciente)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK__TbNombreP__Pacie__3C69FB99");
        });

        modelBuilder.Entity<TbPaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbPacien__3214EC073AA84CBD");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CorreoElectronico).HasMaxLength(200);
            entity.Property(e => e.DetDosisPaciente)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.DireccionExacta)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.DuracionTratamiento)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.InstalacionSalud)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.MotivoRequerimientoAcompanante)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Nacionalidad)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NumDocCedula).HasMaxLength(100);
            entity.Property(e => e.NumDocPasaporte).HasMaxLength(100);
            entity.Property(e => e.PrimerApellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PrimerNombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RegionSalud)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.SegundoApellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SegundoNombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Sexo)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TipoDiscapacidad)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ViaConsumoProducto)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Corregimiento).WithMany(p => p.TbPaciente)
                .HasForeignKey(d => d.CorregimientoId)
                .HasConstraintName("FK__TbPacient__Corre__31EC6D26");

            entity.HasOne(d => d.Distrito).WithMany(p => p.TbPaciente)
                .HasForeignKey(d => d.DistritoId)
                .HasConstraintName("FK__TbPacient__Distr__30F848ED");

            entity.HasOne(d => d.Instalacion).WithMany(p => p.TbPaciente)
                .HasForeignKey(d => d.InstalacionId)
                .HasConstraintName("FK__TbPacient__Insta__33D4B598");

            entity.HasOne(d => d.Provincia).WithMany(p => p.TbPaciente)
                .HasForeignKey(d => d.ProvinciaId)
                .HasConstraintName("FK__TbPacient__Provi__300424B4");

            entity.HasOne(d => d.Region).WithMany(p => p.TbPaciente)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("FK__TbPacient__Regio__32E0915F");
        });

        modelBuilder.Entity<TbPacienteComorbilidad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbPacien__3214EC075CBF7B1F");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DetalleTratamiento)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.NombreDiagnostico)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbPacienteComorbilidad)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK_TbPacienteComorbilidad_Paciente");
        });

        modelBuilder.Entity<TbPacienteDiagnostico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbPacien__3214EC074F07F45F");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.NombreDiagnostico)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbPacienteDiagnostico)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK__TbPacient__Pacie__398D8EEE");
        });

        modelBuilder.Entity<TbProvincia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbProvin__3214EC0791200421");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.NombreProvincia)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbRegionSalud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbRegion__3214EC074BD32811");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.NombreRegion)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbSolRegCannabis>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbSolReg__3214EC077603B6AD");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ComentarioRevision)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.CreadaPor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EstadoSolicitud)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");
            entity.Property(e => e.ModificadaPor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NumSolCompleta)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioRevisor)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbSolRegCannabis)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK__TbSolRegC__Pacie__44FF419A");
        });

        modelBuilder.Entity<TbSolRegCannabisHistorial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbSolReg__3214EC072D12416E");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Comentario)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.EstadoSolicitud)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioRevisor)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.SolRegCannabis).WithMany(p => p.TbSolRegCannabisHistorial)
                .HasForeignKey(d => d.SolRegCannabisId)
                .HasConstraintName("FK__TbSolRegC__SolRe__47DBAE45");
        });

        modelBuilder.Entity<TbSolSecuencia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbSolSec__3214EC07C381F601");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
