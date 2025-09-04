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

    public virtual DbSet<ListaDiagnostico> ListaDiagnostico { get; set; }

    public virtual DbSet<TbAcompanantePaciente> TbAcompanantePaciente { get; set; }

    public virtual DbSet<TbCorregimiento> TbCorregimiento { get; set; }

    public virtual DbSet<TbDeclaracionJurada> TbDeclaracionJurada { get; set; }

    public virtual DbSet<TbDistrito> TbDistrito { get; set; }

    public virtual DbSet<TbDocumentoMedico> TbDocumentoMedico { get; set; }

    public virtual DbSet<TbEstadoSolicitud> TbEstadoSolicitud { get; set; }

    public virtual DbSet<TbFormaFarmaceutica> TbFormaFarmaceutica { get; set; }

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

    public virtual DbSet<TbUnidades> TbUnidades { get; set; }

    public virtual DbSet<TbViaAdministracion> TbViaAdministracion { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ListaDiagnostico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ListaDia__3214EC073A493EE6");

            entity.Property(e => e.IsActivo).HasDefaultValue(true);
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbAcompanantePaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbAcompa__3214EC07D7E1287B");

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
            entity.Property(e => e.TelefonoMovil).HasMaxLength(15);
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbAcompanantePaciente)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK__TbAcompan__Pacie__04E4BC85");
        });

        modelBuilder.Entity<TbCorregimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbCorreg__3214EC07326C99E6");

            entity.Property(e => e.NombreCorregimiento)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Distrito).WithMany(p => p.TbCorregimiento)
                .HasForeignKey(d => d.DistritoId)
                .HasConstraintName("FK__TbCorregi__Distr__05D8E0BE");
        });

        modelBuilder.Entity<TbDeclaracionJurada>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbDeclar__3214EC07726A234F");

            entity.Property(e => e.Detalle)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.NombreDeclarante)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.SolRegCannabis).WithMany(p => p.TbDeclaracionJurada)
                .HasForeignKey(d => d.SolRegCannabisId)
                .HasConstraintName("FK__TbDeclara__SolRe__06CD04F7");
        });

        modelBuilder.Entity<TbDistrito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbDistri__3214EC074B465C51");

            entity.Property(e => e.NombreDistrito)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Provincia).WithMany(p => p.TbDistrito)
                .HasForeignKey(d => d.ProvinciaId)
                .HasConstraintName("FK__TbDistrit__Provi__07C12930");
        });

        modelBuilder.Entity<TbDocumentoMedico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbDocume__3214EC0713E55AFE");

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
                .HasConstraintName("FK__TbDocumen__Medic__0A9D95DB");
        });

        modelBuilder.Entity<TbEstadoSolicitud>(entity =>
        {
            entity.HasKey(e => e.Estado).HasName("PK__TbEstado__36DF552EC7C72BD8");

            entity.Property(e => e.Estado).ValueGeneratedNever();
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbFormaFarmaceutica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbFormaF__3214EC07D91E09F5");

            entity.Property(e => e.IsActivo).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<TbInstalacionSalud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbInstal__3214EC0743962010");

            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbMedicoPaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbMedico__3214EC07116D16E6");

            entity.Property(e => e.DetalleMedico)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValue("Sin detalle");
            entity.Property(e => e.MedicoDisciplina)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.MedicoIdoneidad)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MedicoInstalacion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.MedicoTelefono).HasMaxLength(15);
            entity.Property(e => e.PrimerApellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PrimerNombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Instalacion).WithMany(p => p.TbMedicoPaciente)
                .HasForeignKey(d => d.InstalacionId)
                .HasConstraintName("FK__TbMedicoP__Insta__0C85DE4D");

            entity.HasOne(d => d.Region).WithMany(p => p.TbMedicoPaciente)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("FK__TbMedicoP__Regio__0B91BA14");
        });

        modelBuilder.Entity<TbNombreProductoPaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbNombre__3214EC07C70370D8");

            entity.Property(e => e.CantidadConcentracion).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DetDosisPaciente)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.FormaFarmaceutica)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.NombreComercialProd)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.NombreConcentracion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ProductoUnidad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ViaConsumoProducto)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbNombreProductoPaciente)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK__TbNombreP__Pacie__0D7A0286");
        });

        modelBuilder.Entity<TbPaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbPacien__3214EC0763C96278");

            entity.Property(e => e.CorreoElectronico).HasMaxLength(200);
            entity.Property(e => e.DireccionExacta)
                .HasMaxLength(300)
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
            entity.Property(e => e.TelefonoLaboral).HasMaxLength(15);
            entity.Property(e => e.TelefonoPersonal).HasMaxLength(15);
            entity.Property(e => e.TipoDiscapacidad)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Corregimiento).WithMany(p => p.TbPaciente)
                .HasForeignKey(d => d.CorregimientoId)
                .HasConstraintName("FK__TbPacient__Corre__10566F31");

            entity.HasOne(d => d.Distrito).WithMany(p => p.TbPaciente)
                .HasForeignKey(d => d.DistritoId)
                .HasConstraintName("FK__TbPacient__Distr__0F624AF8");

            entity.HasOne(d => d.Instalacion).WithMany(p => p.TbPaciente)
                .HasForeignKey(d => d.InstalacionId)
                .HasConstraintName("FK__TbPacient__Insta__123EB7A3");

            entity.HasOne(d => d.Provincia).WithMany(p => p.TbPaciente)
                .HasForeignKey(d => d.ProvinciaId)
                .HasConstraintName("FK__TbPacient__Provi__0E6E26BF");

            entity.HasOne(d => d.Region).WithMany(p => p.TbPaciente)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("FK__TbPacient__Regio__114A936A");
        });

        modelBuilder.Entity<TbPacienteComorbilidad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbPacien__3214EC07F9CBC101");

            entity.Property(e => e.DetalleTratamiento)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.NombreDiagnostico)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbPacienteDiagnostico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbPacien__3214EC076F1C117B");

            entity.Property(e => e.NombreDiagnostico)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbPacienteDiagnostico)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK__TbPacient__Pacie__1332DBDC");
        });

        modelBuilder.Entity<TbProvincia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbProvin__3214EC07DEBEFE7F");

            entity.Property(e => e.NombreProvincia)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbRegionSalud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbRegion__3214EC073E447860");

            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbSolRegCannabis>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbSolReg__3214EC0714056226");

            entity.Property(e => e.ComentarioRevision)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.CreadaPor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EstadoSolicitud)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaAprobacion).HasColumnType("datetime");
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
                .HasConstraintName("FK__TbSolRegC__Pacie__14270015");
        });

        modelBuilder.Entity<TbSolRegCannabisHistorial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbSolReg__3214EC07173C6F09");

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
                .HasConstraintName("FK__TbSolRegC__SolRe__151B244E");
        });

        modelBuilder.Entity<TbSolSecuencia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbSolSec__3214EC07FC4B29BC");
        });

        modelBuilder.Entity<TbUnidades>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbUnidad__3214EC0728F3088F");

            entity.Property(e => e.IsActivo).HasDefaultValue(true);
            entity.Property(e => e.NombreUnidad)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbViaAdministracion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbViaAdm__3214EC077C94E533");

            entity.Property(e => e.IsActivo).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
