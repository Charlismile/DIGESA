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
        modelBuilder.UseCollation("Modern_Spanish_CI_AS");

        modelBuilder.Entity<ListaDiagnostico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ListaDia__3214EC0787C98F3E");

            entity.Property(e => e.IsActivo).HasDefaultValue(true);
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbAcompanantePaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbAcompa__3214EC07AE831989");

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
                .HasConstraintName("FK__TbAcompan__Pacie__17F790F9");
        });

        modelBuilder.Entity<TbCorregimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbCorreg__3214EC07FE3D07FE");

            entity.Property(e => e.NombreCorregimiento)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Distrito).WithMany(p => p.TbCorregimiento)
                .HasForeignKey(d => d.DistritoId)
                .HasConstraintName("FK__TbCorregi__Distr__18EBB532");
        });

        modelBuilder.Entity<TbDeclaracionJurada>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbDeclar__3214EC07890DF7EA");

            entity.Property(e => e.Detalle)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.NombreDeclarante)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.SolRegCannabis).WithMany(p => p.TbDeclaracionJurada)
                .HasForeignKey(d => d.SolRegCannabisId)
                .HasConstraintName("FK__TbDeclara__SolRe__19DFD96B");
        });

        modelBuilder.Entity<TbDistrito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbDistri__3214EC07CA5FE536");

            entity.Property(e => e.NombreDistrito)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Provincia).WithMany(p => p.TbDistrito)
                .HasForeignKey(d => d.ProvinciaId)
                .HasConstraintName("FK__TbDistrit__Provi__1AD3FDA4");
        });

        modelBuilder.Entity<TbDocumentoMedico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbDocume__3214EC0702147DCD");

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
                .HasConstraintName("FK__TbDocumen__Medic__1DB06A4F");
        });

        modelBuilder.Entity<TbEstadoSolicitud>(entity =>
        {
            entity.HasKey(e => e.Estado).HasName("PK__TbEstado__36DF552E7C4E170D");

            entity.Property(e => e.Estado).ValueGeneratedNever();
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbFormaFarmaceutica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbFormaF__3214EC07B82380CA");

            entity.Property(e => e.IsActivo).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<TbInstalacionSalud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbInstal__3214EC07DC3E22F5");

            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbMedicoPaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbMedico__3214EC07F5B94B2B");

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
                .HasConstraintName("FK__TbMedicoP__Insta__1F98B2C1");

            entity.HasOne(d => d.Region).WithMany(p => p.TbMedicoPaciente)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("FK__TbMedicoP__Regio__1EA48E88");
        });

        modelBuilder.Entity<TbNombreProductoPaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbNombre__3214EC07916D98F4");

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
                .HasConstraintName("FK__TbNombreP__Pacie__208CD6FA");
        });

        modelBuilder.Entity<TbPaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbPacien__3214EC071878FD74");

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
            entity.Property(e => e.TelefonoResidencial).HasMaxLength(15);
            entity.Property(e => e.TipoDiscapacidad)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Corregimiento).WithMany(p => p.TbPaciente)
                .HasForeignKey(d => d.CorregimientoId)
                .HasConstraintName("FK__TbPacient__Corre__236943A5");

            entity.HasOne(d => d.Distrito).WithMany(p => p.TbPaciente)
                .HasForeignKey(d => d.DistritoId)
                .HasConstraintName("FK__TbPacient__Distr__22751F6C");

            entity.HasOne(d => d.Instalacion).WithMany(p => p.TbPaciente)
                .HasForeignKey(d => d.InstalacionId)
                .HasConstraintName("FK__TbPacient__Insta__25518C17");

            entity.HasOne(d => d.Provincia).WithMany(p => p.TbPaciente)
                .HasForeignKey(d => d.ProvinciaId)
                .HasConstraintName("FK__TbPacient__Provi__2180FB33");

            entity.HasOne(d => d.Region).WithMany(p => p.TbPaciente)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("FK__TbPacient__Regio__245D67DE");
        });

        modelBuilder.Entity<TbPacienteComorbilidad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbPacien__3214EC070FF6F1B5");

            entity.Property(e => e.DetalleTratamiento)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.NombreDiagnostico)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbPacienteDiagnostico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbPacien__3214EC07A5CE3D12");

            entity.Property(e => e.NombreDiagnostico)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbPacienteDiagnostico)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK__TbPacient__Pacie__2645B050");
        });

        modelBuilder.Entity<TbProvincia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbProvin__3214EC078235366B");

            entity.Property(e => e.NombreProvincia)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbRegionSalud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbRegion__3214EC07217A1866");

            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbSolRegCannabis>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbSolReg__3214EC075DC8873F");

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
                .HasConstraintName("FK__TbSolRegC__Pacie__2739D489");
        });

        modelBuilder.Entity<TbSolRegCannabisHistorial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbSolReg__3214EC0729AF791A");

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
                .HasConstraintName("FK__TbSolRegC__SolRe__282DF8C2");
        });

        modelBuilder.Entity<TbSolSecuencia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbSolSec__3214EC0782CDE118");
        });

        modelBuilder.Entity<TbUnidades>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbUnidad__3214EC075A721EDF");

            entity.Property(e => e.IsActivo).HasDefaultValue(true);
            entity.Property(e => e.NombreUnidad)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbViaAdministracion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbViaAdm__3214EC0705E84A9D");

            entity.Property(e => e.IsActivo).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
