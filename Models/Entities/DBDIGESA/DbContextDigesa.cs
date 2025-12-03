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

    public virtual DbSet<TbAprobacionTransferencia> TbAprobacionTransferencia { get; set; }

    public virtual DbSet<TbCorregimiento> TbCorregimiento { get; set; }

    public virtual DbSet<TbDeclaracionJurada> TbDeclaracionJurada { get; set; }

    public virtual DbSet<TbDistrito> TbDistrito { get; set; }

    public virtual DbSet<TbDocumentoAdjunto> TbDocumentoAdjunto { get; set; }

    public virtual DbSet<TbEstadoSolicitud> TbEstadoSolicitud { get; set; }

    public virtual DbSet<TbFormaFarmaceutica> TbFormaFarmaceutica { get; set; }

    public virtual DbSet<TbHistorialUsuario> TbHistorialUsuario { get; set; }

    public virtual DbSet<TbInstalacionSalud> TbInstalacionSalud { get; set; }

    public virtual DbSet<TbMedicoPaciente> TbMedicoPaciente { get; set; }

    public virtual DbSet<TbNombreProductoPaciente> TbNombreProductoPaciente { get; set; }

    public virtual DbSet<TbNotificacionVencimiento> TbNotificacionVencimiento { get; set; }

    public virtual DbSet<TbPaciente> TbPaciente { get; set; }

    public virtual DbSet<TbPacienteComorbilidad> TbPacienteComorbilidad { get; set; }

    public virtual DbSet<TbPacienteDiagnostico> TbPacienteDiagnostico { get; set; }

    public virtual DbSet<TbPlantillaEmail> TbPlantillaEmail { get; set; }

    public virtual DbSet<TbProvincia> TbProvincia { get; set; }

    public virtual DbSet<TbRegionSalud> TbRegionSalud { get; set; }

    public virtual DbSet<TbReporteGenerado> TbReporteGenerado { get; set; }

    public virtual DbSet<TbSolRegCannabis> TbSolRegCannabis { get; set; }

    public virtual DbSet<TbSolRegCannabisHistorial> TbSolRegCannabisHistorial { get; set; }

    public virtual DbSet<TbSolSecuencia> TbSolSecuencia { get; set; }

    public virtual DbSet<TbTipoDocumentoAdjunto> TbTipoDocumentoAdjunto { get; set; }

    public virtual DbSet<TbTransferenciaResponsabilidad> TbTransferenciaResponsabilidad { get; set; }

    public virtual DbSet<TbUnidades> TbUnidades { get; set; }

    public virtual DbSet<TbViaAdministracion> TbViaAdministracion { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ListaDiagnostico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ListaDia__3214EC0794215911");

            entity.Property(e => e.IsActivo).HasDefaultValue(true);
        });

        modelBuilder.Entity<TbAcompanantePaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbAcompa__3214EC07AFC5432F");

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbAcompanantePaciente).HasConstraintName("FK_TbAcompanantePaciente_TbPaciente");
        });

        modelBuilder.Entity<TbAprobacionTransferencia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbAproba__3214EC0768B3DEA4");

            entity.Property(e => e.Aprobada).HasDefaultValue(false);
            entity.Property(e => e.NivelAprobacion).HasDefaultValue(1);

            entity.HasOne(d => d.Transferencia).WithMany(p => p.TbAprobacionTransferencia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TbAprobac__Trans__3F115E1A");
        });

        modelBuilder.Entity<TbCorregimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbCorreg__3214EC079CF22324");

            entity.HasOne(d => d.Distrito).WithMany(p => p.TbCorregimiento).HasConstraintName("FK__TbCorregi__Distr__09A971A2");
        });

        modelBuilder.Entity<TbDeclaracionJurada>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbDeclar__3214EC07CDD359CD");

            entity.HasOne(d => d.SolRegCannabis).WithMany(p => p.TbDeclaracionJurada).HasConstraintName("FK__TbDeclara__SolRe__0A9D95DB");
        });

        modelBuilder.Entity<TbDistrito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbDistri__3214EC07855FC95D");

            entity.HasOne(d => d.Provincia).WithMany(p => p.TbDistrito).HasConstraintName("FK__TbDistrit__Provi__0B91BA14");
        });

        modelBuilder.Entity<TbDocumentoAdjunto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbDocume__3214EC0758A4AABF");

            entity.Property(e => e.FechaSubidaUtc).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsValido).HasDefaultValue(false);

            entity.HasOne(d => d.SolRegCannabis).WithMany(p => p.TbDocumentoAdjunto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TbDocumen__SolRe__0C85DE4D");

            entity.HasOne(d => d.TipoDocumento).WithMany(p => p.TbDocumentoAdjunto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TbDocumen__TipoD__0D7A0286");
        });

        modelBuilder.Entity<TbEstadoSolicitud>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__TbEstado__FBB0EDC1B27F94BB");
        });

        modelBuilder.Entity<TbFormaFarmaceutica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbFormaF__3214EC07E443B920");

            entity.Property(e => e.IsActivo).HasDefaultValue(true);
        });

        modelBuilder.Entity<TbHistorialUsuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbHistor__3214EC071257F06A");

            entity.Property(e => e.FechaCambio).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<TbInstalacionSalud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbInstal__3214EC077628783A");
        });

        modelBuilder.Entity<TbMedicoPaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbMedico__3214EC0761F30F34");

            entity.Property(e => e.DetalleMedico).HasDefaultValue("Sin detalle");

            entity.HasOne(d => d.Instalacion).WithMany(p => p.TbMedicoPaciente).HasConstraintName("FK__TbMedicoP__Insta__0F624AF8");

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbMedicoPaciente).HasConstraintName("FK_TbMedicoPaciente_TbPaciente");

            entity.HasOne(d => d.Region).WithMany(p => p.TbMedicoPaciente).HasConstraintName("FK__TbMedicoP__Regio__0E6E26BF");
        });

        modelBuilder.Entity<TbNombreProductoPaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbNombre__3214EC07E727A746");

            entity.HasOne(d => d.FormaFarmaceuticaNavigation).WithMany(p => p.TbNombreProductoPaciente).HasConstraintName("FK_TbNombreProductoPaciente_FormaFarmaceutica");

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbNombreProductoPaciente).HasConstraintName("FK__TbNombreP__Pacie__114A936A");

            entity.HasOne(d => d.ProductoUnidadNavigation).WithMany(p => p.TbNombreProductoPaciente).HasConstraintName("FK_TbNombreProductoPaciente_TbUnidades");

            entity.HasOne(d => d.ViaAdministracion).WithMany(p => p.TbNombreProductoPaciente).HasConstraintName("FK_TbNombreProductoPaciente_ViaAdministracion");
        });

        modelBuilder.Entity<TbNotificacionVencimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbNotifi__3214EC071C1DF839");

            entity.Property(e => e.EmailEnviado).HasDefaultValue(false);
            entity.Property(e => e.FechaEnvio).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.SolRegCannabis).WithMany(p => p.TbNotificacionVencimiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbNotificacion_Solicitud");
        });

        modelBuilder.Entity<TbPaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbPacien__3214EC07ABC46238");

            entity.HasOne(d => d.Corregimiento).WithMany(p => p.TbPaciente).HasConstraintName("FK__TbPacient__Corre__17036CC0");

            entity.HasOne(d => d.Distrito).WithMany(p => p.TbPaciente).HasConstraintName("FK__TbPacient__Distr__160F4887");

            entity.HasOne(d => d.Instalacion).WithMany(p => p.TbPaciente).HasConstraintName("FK__TbPacient__Insta__18EBB532");

            entity.HasOne(d => d.Provincia).WithMany(p => p.TbPaciente).HasConstraintName("FK__TbPacient__Provi__151B244E");

            entity.HasOne(d => d.Region).WithMany(p => p.TbPaciente).HasConstraintName("FK__TbPacient__Regio__17F790F9");
        });

        modelBuilder.Entity<TbPacienteComorbilidad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbPacien__3214EC0797FA3832");

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbPacienteComorbilidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbPacienteComorbilidad_TbPaciente");
        });

        modelBuilder.Entity<TbPacienteDiagnostico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbPacien__3214EC079F8321BF");

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbPacienteDiagnostico).HasConstraintName("FK__TbPacient__Pacie__1AD3FDA4");
        });

        modelBuilder.Entity<TbPlantillaEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbPlanti__3214EC07647D1D18");

            entity.Property(e => e.Activa).HasDefaultValue(true);
            entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<TbProvincia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbProvin__3214EC073D75503B");
        });

        modelBuilder.Entity<TbRegionSalud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbRegion__3214EC076BD75312");
        });

        modelBuilder.Entity<TbReporteGenerado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbReport__3214EC075313D2C1");

            entity.Property(e => e.Descargado).HasDefaultValue(false);
            entity.Property(e => e.FechaGeneracion).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<TbSolRegCannabis>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbSolReg__3214EC0745E552D5");

            entity.Property(e => e.CarnetActivo).HasDefaultValue(true);
            entity.Property(e => e.EsRenovacion).HasDefaultValue(false);

            entity.HasOne(d => d.EstadoSolicitud).WithMany(p => p.TbSolRegCannabis).HasConstraintName("FK_TbSolRegCannabis_EstadoSolicitud");

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbSolRegCannabis).HasConstraintName("FK__TbSolRegC__Pacie__1BC821DD");
        });

        modelBuilder.Entity<TbSolRegCannabisHistorial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbSolReg__3214EC0719C6A514");

            entity.HasOne(d => d.EstadoSolicitudIdHistorialNavigation).WithMany(p => p.TbSolRegCannabisHistorial).HasConstraintName("FK_TbSolRegCannabisHistorial_EstadoSolicitudHistorial");

            entity.HasOne(d => d.SolRegCannabis).WithMany(p => p.TbSolRegCannabisHistorial).HasConstraintName("FK__TbSolRegC__SolRe__1DB06A4F");
        });

        modelBuilder.Entity<TbSolSecuencia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbSolSec__3214EC0718D1B159");
        });

        modelBuilder.Entity<TbTipoDocumentoAdjunto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbTipoDo__3214EC0703C04C07");

            entity.Property(e => e.EsObligatorio).HasDefaultValue(true);
            entity.Property(e => e.EsParaMayorEdad).HasDefaultValue(false);
            entity.Property(e => e.EsParaMenorEdad).HasDefaultValue(false);
            entity.Property(e => e.EsParaRenovacion).HasDefaultValue(false);
            entity.Property(e => e.EsRequisitoParaAcompanante).HasDefaultValue(false);
            entity.Property(e => e.EsRequisitoParaPaciente).HasDefaultValue(false);
            entity.Property(e => e.EsSoloMedico).HasDefaultValue(false);
            entity.Property(e => e.IsActivo).HasDefaultValue(true);
        });

        modelBuilder.Entity<TbTransferenciaResponsabilidad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbTransf__3214EC07F3C1091D");

            entity.Property(e => e.Estado).HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaSolicitud).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.SolRegCannabis).WithMany(p => p.TbTransferenciaResponsabilidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbTransferencia_Solicitud");
        });

        modelBuilder.Entity<TbUnidades>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbUnidad__3214EC079C52C060");

            entity.Property(e => e.IsActivo).HasDefaultValue(true);
        });

        modelBuilder.Entity<TbViaAdministracion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbViaAdm__3214EC07B78E96F0");

            entity.Property(e => e.IsActivo).HasDefaultValue(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
