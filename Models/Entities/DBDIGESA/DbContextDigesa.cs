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

    public virtual DbSet<TbAuditoriaMedico> TbAuditoriaMedico { get; set; }

    public virtual DbSet<TbCodigoQr> TbCodigoQr { get; set; }

    public virtual DbSet<TbConfiguracionSistema> TbConfiguracionSistema { get; set; }

    public virtual DbSet<TbCorregimiento> TbCorregimiento { get; set; }

    public virtual DbSet<TbDeclaracionJurada> TbDeclaracionJurada { get; set; }

    public virtual DbSet<TbDistrito> TbDistrito { get; set; }

    public virtual DbSet<TbDocumentoAdjunto> TbDocumentoAdjunto { get; set; }

    public virtual DbSet<TbEstadoSolicitud> TbEstadoSolicitud { get; set; }

    public virtual DbSet<TbFarmaciaAutorizada> TbFarmaciaAutorizada { get; set; }

    public virtual DbSet<TbFormaFarmaceutica> TbFormaFarmaceutica { get; set; }

    public virtual DbSet<TbHistorialRenovacion> TbHistorialRenovacion { get; set; }

    public virtual DbSet<TbHistorialUsuario> TbHistorialUsuario { get; set; }

    public virtual DbSet<TbInstalacionSalud> TbInstalacionSalud { get; set; }

    public virtual DbSet<TbLogNotificaciones> TbLogNotificaciones { get; set; }

    public virtual DbSet<TbMedico> TbMedico { get; set; }

    public virtual DbSet<TbMedicoPaciente> TbMedicoPaciente { get; set; }

    public virtual DbSet<TbNombreProductoPaciente> TbNombreProductoPaciente { get; set; }

    public virtual DbSet<TbNotificacionVencimiento> TbNotificacionVencimiento { get; set; }

    public virtual DbSet<TbPaciente> TbPaciente { get; set; }

    public virtual DbSet<TbPacienteComorbilidad> TbPacienteComorbilidad { get; set; }

    public virtual DbSet<TbPacienteDiagnostico> TbPacienteDiagnostico { get; set; }

    public virtual DbSet<TbPlantillaEmail> TbPlantillaEmail { get; set; }

    public virtual DbSet<TbProvincia> TbProvincia { get; set; }

    public virtual DbSet<TbRegionSalud> TbRegionSalud { get; set; }

    public virtual DbSet<TbRegistroDispensacion> TbRegistroDispensacion { get; set; }

    public virtual DbSet<TbReporteGenerado> TbReporteGenerado { get; set; }

    public virtual DbSet<TbSolRegCannabis> TbSolRegCannabis { get; set; }

    public virtual DbSet<TbSolRegCannabisHistorial> TbSolRegCannabisHistorial { get; set; }

    public virtual DbSet<TbSolSecuencia> TbSolSecuencia { get; set; }

    public virtual DbSet<TbTipoDocumentoAdjunto> TbTipoDocumentoAdjunto { get; set; }

    public virtual DbSet<TbTransferenciaResponsabilidad> TbTransferenciaResponsabilidad { get; set; }

    public virtual DbSet<TbUnidades> TbUnidades { get; set; }

    public virtual DbSet<TbViaAdministracion> TbViaAdministracion { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DI-145465X;Database=DBDIGESA;User Id=carlos;Password=123456;Persist Security Info=False;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ListaDiagnostico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ListaDia__3214EC0794215911");

            entity.Property(e => e.IsActivo).HasDefaultValue(true);
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbAcompanantePaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbAcompa__3214EC07AFC5432F");

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
                .HasConstraintName("FK_TbAcompanantePaciente_TbPaciente");
        });

        modelBuilder.Entity<TbAprobacionTransferencia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbAproba__3214EC0768B3DEA4");

            entity.Property(e => e.Aprobada).HasDefaultValue(false);
            entity.Property(e => e.Comentario).HasMaxLength(500);
            entity.Property(e => e.FechaAprobacion).HasColumnType("datetime");
            entity.Property(e => e.NivelAprobacion).HasDefaultValue(1);
            entity.Property(e => e.UsuarioId).HasMaxLength(450);

            entity.HasOne(d => d.Transferencia).WithMany(p => p.TbAprobacionTransferencia)
                .HasForeignKey(d => d.TransferenciaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TbAprobac__Trans__3F115E1A");
        });

        modelBuilder.Entity<TbAuditoriaMedico>(entity =>
        {
            entity.Property(e => e.Comentarios).HasMaxLength(500);
            entity.Property(e => e.FechaAccion).HasColumnType("datetime");
            entity.Property(e => e.IpOrigen)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoAccion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioAccion).HasMaxLength(450);

            entity.HasOne(d => d.Medico).WithMany(p => p.TbAuditoriaMedico)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbAuditoriaMedico_TbMedico");
        });

        modelBuilder.Entity<TbCodigoQr>(entity =>
        {
            entity.ToTable("TbCodigoQR");

            entity.HasIndex(e => e.CodigoQr, "UQ_TbCodigoQR_Codigo").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CodigoQr)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("CodigoQR");
            entity.Property(e => e.Comentarios).HasMaxLength(500);
            entity.Property(e => e.FechaGeneracion).HasColumnType("datetime");
            entity.Property(e => e.FechaVencimiento).HasColumnType("datetime");
            entity.Property(e => e.UltimoEscaneadoPor)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UltimoEscaneo).HasColumnType("datetime");

            entity.HasOne(d => d.Solicitud).WithMany(p => p.TbCodigoQr)
                .HasForeignKey(d => d.SolicitudId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbCodigoQR_Solicitud");
        });

        modelBuilder.Entity<TbConfiguracionSistema>(entity =>
        {
            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.EsEditable).HasDefaultValue(true);
            entity.Property(e => e.Grupo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Valor).HasMaxLength(500);
        });

        modelBuilder.Entity<TbCorregimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbCorreg__3214EC079CF22324");

            entity.Property(e => e.NombreCorregimiento)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Distrito).WithMany(p => p.TbCorregimiento)
                .HasForeignKey(d => d.DistritoId)
                .HasConstraintName("FK__TbCorregi__Distr__09A971A2");
        });

        modelBuilder.Entity<TbDeclaracionJurada>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbDeclar__3214EC07CDD359CD");

            entity.Property(e => e.Detalle)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.NombreDeclarante)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.SolRegCannabis).WithMany(p => p.TbDeclaracionJurada)
                .HasForeignKey(d => d.SolRegCannabisId)
                .HasConstraintName("FK__TbDeclara__SolRe__0A9D95DB");
        });

        modelBuilder.Entity<TbDistrito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbDistri__3214EC07855FC95D");

            entity.Property(e => e.NombreDistrito)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Provincia).WithMany(p => p.TbDistrito)
                .HasForeignKey(d => d.ProvinciaId)
                .HasConstraintName("FK__TbDistrit__Provi__0B91BA14");
        });

        modelBuilder.Entity<TbDocumentoAdjunto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbDocume__3214EC0758A4AABF");

            entity.Property(e => e.Categoria)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaSubidaUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsValido).HasDefaultValue(false);
            entity.Property(e => e.NombreGuardado)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.NombreOriginal)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.SubidoPor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Url).HasMaxLength(300);

            entity.HasOne(d => d.SolRegCannabis).WithMany(p => p.TbDocumentoAdjunto)
                .HasForeignKey(d => d.SolRegCannabisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TbDocumen__SolRe__0C85DE4D");

            entity.HasOne(d => d.TipoDocumento).WithMany(p => p.TbDocumentoAdjunto)
                .HasForeignKey(d => d.TipoDocumentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TbDocumen__TipoD__0D7A0286");
        });

        modelBuilder.Entity<TbEstadoSolicitud>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__TbEstado__FBB0EDC1B27F94BB");

            entity.HasIndex(e => e.NombreEstado, "UQ__TbEstado__6CE50615E2886F9E").IsUnique();

            entity.Property(e => e.NombreEstado)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbFarmaciaAutorizada>(entity =>
        {
            entity.HasIndex(e => e.CodigoFarmacia, "UQ_TbFarmaciaAutorizada_Codigo").IsUnique();

            entity.HasIndex(e => e.Ruc, "UQ_TbFarmaciaAutorizada_RUC").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CodigoFarmacia)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FechaAutorizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaVencimientoAutorizacion).HasColumnType("datetime");
            entity.Property(e => e.NombreFarmacia)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Responsable)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Ruc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RUC");
            entity.Property(e => e.Telefono).HasMaxLength(15);
            entity.Property(e => e.UsuarioRegistro).HasMaxLength(450);

            entity.HasOne(d => d.Distrito).WithMany(p => p.TbFarmaciaAutorizada)
                .HasForeignKey(d => d.DistritoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbFarmaciaAutorizada_Distrito");

            entity.HasOne(d => d.Provincia).WithMany(p => p.TbFarmaciaAutorizada)
                .HasForeignKey(d => d.ProvinciaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbFarmaciaAutorizada_Provincia");
        });

        modelBuilder.Entity<TbFormaFarmaceutica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbFormaF__3214EC07E443B920");

            entity.Property(e => e.IsActivo).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<TbHistorialRenovacion>(entity =>
        {
            entity.Property(e => e.Comentarios).HasMaxLength(500);
            entity.Property(e => e.FechaRenovacion).HasColumnType("datetime");
            entity.Property(e => e.RazonRenovacion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioRenovador).HasMaxLength(450);

            entity.HasOne(d => d.SolicitudAnterior).WithMany(p => p.TbHistorialRenovacionSolicitudAnterior)
                .HasForeignKey(d => d.SolicitudAnteriorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbHistorialRenovacion_SolicitudAnterior");

            entity.HasOne(d => d.SolicitudNueva).WithMany(p => p.TbHistorialRenovacionSolicitudNueva)
                .HasForeignKey(d => d.SolicitudNuevaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbHistorialRenovacion_SolicitudNueva");
        });

        modelBuilder.Entity<TbHistorialUsuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbHistor__3214EC071257F06A");

            entity.Property(e => e.CambioPor).HasMaxLength(450);
            entity.Property(e => e.Comentario).HasMaxLength(500);
            entity.Property(e => e.EstadoAnterior)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EstadoNuevo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaCambio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TipoCambio)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioId).HasMaxLength(450);
        });

        modelBuilder.Entity<TbInstalacionSalud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbInstal__3214EC077628783A");

            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbLogNotificaciones>(entity =>
        {
            entity.Property(e => e.Destinatario).HasMaxLength(200);
            entity.Property(e => e.Error).HasMaxLength(500);
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaEnvio).HasColumnType("datetime");
            entity.Property(e => e.MetodoEnvio)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoNotificacion)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Solicitud).WithMany(p => p.TbLogNotificaciones)
                .HasForeignKey(d => d.SolicitudId)
                .HasConstraintName("FK_TbLogNotificaciones_Solicitud");
        });

        modelBuilder.Entity<TbMedico>(entity =>
        {
            entity.HasIndex(e => new { e.Verificado, e.Activo }, "IX_TbMedico_Verificado");

            entity.HasIndex(e => e.CodigoMedico, "UQ_TbMedico_CodigoMedico").IsUnique();

            entity.HasIndex(e => e.NumeroColegiatura, "UQ_TbMedico_NumeroColegiatura").IsUnique();

            entity.HasIndex(e => new { e.TipoDocumento, e.NumeroDocumento }, "UQ_TbMedico_NumeroDocumento").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CodigoMedico)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DireccionConsultorio)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Especialidad)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaVerificacion).HasColumnType("datetime");
            entity.Property(e => e.InstalacionPersonalizada)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.NumeroColegiatura)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NumeroDocumento).HasMaxLength(100);
            entity.Property(e => e.Observaciones).HasMaxLength(500);
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
            entity.Property(e => e.Subespecialidad)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.TelefonoConsultorio).HasMaxLength(15);
            entity.Property(e => e.TelefonoMovil).HasMaxLength(15);
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioRegistro).HasMaxLength(450);
            entity.Property(e => e.UsuarioVerificador).HasMaxLength(450);

            entity.HasOne(d => d.Distrito).WithMany(p => p.TbMedico)
                .HasForeignKey(d => d.DistritoId)
                .HasConstraintName("FK_TbMedico_Distrito");

            entity.HasOne(d => d.InstalacionSalud).WithMany(p => p.TbMedico)
                .HasForeignKey(d => d.InstalacionSaludId)
                .HasConstraintName("FK_TbMedico_InstalacionSalud");

            entity.HasOne(d => d.Provincia).WithMany(p => p.TbMedico)
                .HasForeignKey(d => d.ProvinciaId)
                .HasConstraintName("FK_TbMedico_Provincia");

            entity.HasOne(d => d.RegionSalud).WithMany(p => p.TbMedico)
                .HasForeignKey(d => d.RegionSaludId)
                .HasConstraintName("FK_TbMedico_RegionSalud");
        });

        modelBuilder.Entity<TbMedicoPaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbMedico__3214EC0761F30F34");

            entity.Property(e => e.DetalleMedico)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValue("Sin detalle");
            entity.Property(e => e.InstalacionPersonalizada)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.MedicoDisciplina)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.MedicoIdoneidad)
                .HasMaxLength(100)
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
                .HasConstraintName("FK__TbMedicoP__Insta__0F624AF8");

            entity.HasOne(d => d.Medico).WithMany(p => p.TbMedicoPaciente)
                .HasForeignKey(d => d.MedicoId)
                .HasConstraintName("FK_TbMedicoPaciente_TbMedico");

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbMedicoPaciente)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK_TbMedicoPaciente_TbPaciente");

            entity.HasOne(d => d.Region).WithMany(p => p.TbMedicoPaciente)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("FK__TbMedicoP__Regio__0E6E26BF");
        });

        modelBuilder.Entity<TbNombreProductoPaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbNombre__3214EC07E727A746");

            entity.Property(e => e.CantidadConcentracion).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DetDosisPaciente)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.DetDosisRescate)
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

            entity.HasOne(d => d.FormaFarmaceuticaNavigation).WithMany(p => p.TbNombreProductoPaciente)
                .HasForeignKey(d => d.FormaFarmaceuticaId)
                .HasConstraintName("FK_TbNombreProductoPaciente_FormaFarmaceutica");

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbNombreProductoPaciente)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK__TbNombreP__Pacie__114A936A");

            entity.HasOne(d => d.ProductoUnidadNavigation).WithMany(p => p.TbNombreProductoPaciente)
                .HasForeignKey(d => d.ProductoUnidadId)
                .HasConstraintName("FK_TbNombreProductoPaciente_TbUnidades");

            entity.HasOne(d => d.ViaAdministracion).WithMany(p => p.TbNombreProductoPaciente)
                .HasForeignKey(d => d.ViaAdministracionId)
                .HasConstraintName("FK_TbNombreProductoPaciente_ViaAdministracion");
        });

        modelBuilder.Entity<TbNotificacionVencimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbNotifi__3214EC071C1DF839");

            entity.HasIndex(e => e.SolRegCannabisId, "IX_TbNotificacionVencimiento_Solicitud");

            entity.Property(e => e.EmailEnviado).HasDefaultValue(false);
            entity.Property(e => e.FechaEnvio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TipoNotificacion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioNotificado).HasMaxLength(450);

            entity.HasOne(d => d.SolRegCannabis).WithMany(p => p.TbNotificacionVencimiento)
                .HasForeignKey(d => d.SolRegCannabisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbNotificacion_Solicitud");
        });

        modelBuilder.Entity<TbPaciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbPacien__3214EC07ABC46238");

            entity.HasIndex(e => new { e.TipoDocumento, e.DocumentoCedula, e.DocumentoPasaporte }, "IX_TbPaciente_Documento");

            entity.Property(e => e.CorreoElectronico).HasMaxLength(200);
            entity.Property(e => e.DireccionExacta)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.DocumentoCedula).HasMaxLength(100);
            entity.Property(e => e.DocumentoPasaporte).HasMaxLength(100);
            entity.Property(e => e.InstalacionPersonalizada)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.MotivoRequerimientoAcompanante)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Nacionalidad)
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
                .HasConstraintName("FK__TbPacient__Corre__17036CC0");

            entity.HasOne(d => d.Distrito).WithMany(p => p.TbPaciente)
                .HasForeignKey(d => d.DistritoId)
                .HasConstraintName("FK__TbPacient__Distr__160F4887");

            entity.HasOne(d => d.Instalacion).WithMany(p => p.TbPaciente)
                .HasForeignKey(d => d.InstalacionId)
                .HasConstraintName("FK__TbPacient__Insta__18EBB532");

            entity.HasOne(d => d.Provincia).WithMany(p => p.TbPaciente)
                .HasForeignKey(d => d.ProvinciaId)
                .HasConstraintName("FK__TbPacient__Provi__151B244E");

            entity.HasOne(d => d.Region).WithMany(p => p.TbPaciente)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("FK__TbPacient__Regio__17F790F9");
        });

        modelBuilder.Entity<TbPacienteComorbilidad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbPacien__3214EC0797FA3832");

            entity.Property(e => e.DetalleTratamiento)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.NombreDiagnostico)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbPacienteComorbilidad)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbPacienteComorbilidad_TbPaciente");
        });

        modelBuilder.Entity<TbPacienteDiagnostico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbPacien__3214EC079F8321BF");

            entity.Property(e => e.DetalleTratamiento)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.NombreDiagnostico)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbPacienteDiagnostico)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK__TbPacient__Pacie__1AD3FDA4");
        });

        modelBuilder.Entity<TbPlantillaEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbPlanti__3214EC07647D1D18");

            entity.Property(e => e.Activa).HasDefaultValue(true);
            entity.Property(e => e.Asunto)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FechaActualizacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TipoNotificacion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbProvincia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbProvin__3214EC073D75503B");

            entity.Property(e => e.NombreProvincia)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbRegionSalud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbRegion__3214EC076BD75312");

            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbRegistroDispensacion>(entity =>
        {
            entity.Property(e => e.Cantidad).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Comentarios).HasMaxLength(500);
            entity.Property(e => e.FarmaceuticoResponsable)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FechaDispensacion).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LoteProducto)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NumeroFactura)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Producto)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UnidadMedida)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioRegistro).HasMaxLength(450);

            entity.HasOne(d => d.Farmacia).WithMany(p => p.TbRegistroDispensacion)
                .HasForeignKey(d => d.FarmaciaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbRegistroDispensacion_Farmacia");

            entity.HasOne(d => d.Solicitud).WithMany(p => p.TbRegistroDispensacion)
                .HasForeignKey(d => d.SolicitudId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbRegistroDispensacion_Solicitud");
        });

        modelBuilder.Entity<TbReporteGenerado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbReport__3214EC075313D2C1");

            entity.Property(e => e.Descargado).HasDefaultValue(false);
            entity.Property(e => e.FechaGeneracion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GeneradoPor).HasMaxLength(450);
            entity.Property(e => e.NombreArchivo)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.RutaArchivo).HasMaxLength(500);
            entity.Property(e => e.TipoReporte)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbSolRegCannabis>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbSolReg__3214EC0745E552D5");

            entity.HasIndex(e => e.CarnetActivo, "IX_TbSolRegCannabis_CarnetActivo");

            entity.HasIndex(e => e.FechaVencimientoCarnet, "IX_TbSolRegCannabis_FechaVencimiento");

            entity.HasIndex(e => e.NumeroCarnet, "IX_TbSolRegCannabis_NumeroCarnet");

            entity.Property(e => e.CarnetActivo).HasDefaultValue(true);
            entity.Property(e => e.ComentarioRevision)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.CreadaPor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EsRenovacion).HasDefaultValue(false);
            entity.Property(e => e.FechaAprobacion).HasColumnType("datetime");
            entity.Property(e => e.FechaEmisionCarnet).HasColumnType("datetime");
            entity.Property(e => e.FechaInactivacion).HasColumnType("datetime");
            entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");
            entity.Property(e => e.FechaUltimaRenovacion).HasColumnType("datetime");
            entity.Property(e => e.FechaVencimientoCarnet).HasColumnType("datetime");
            entity.Property(e => e.FirmaDigitalUrl).HasMaxLength(500);
            entity.Property(e => e.FotoCarnetUrl).HasMaxLength(500);
            entity.Property(e => e.ModificadaPor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NumSolCompleta)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NumeroCarnet)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RazonInactivacion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioInactivador).HasMaxLength(450);
            entity.Property(e => e.UsuarioRevisor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.VersionCarnet).HasDefaultValue(1);

            entity.HasOne(d => d.EstadoSolicitud).WithMany(p => p.TbSolRegCannabis)
                .HasForeignKey(d => d.EstadoSolicitudId)
                .HasConstraintName("FK_TbSolRegCannabis_EstadoSolicitud");

            entity.HasOne(d => d.Paciente).WithMany(p => p.TbSolRegCannabis)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK__TbSolRegC__Pacie__1BC821DD");

            entity.HasOne(d => d.SolicitudPadre).WithMany(p => p.InverseSolicitudPadre)
                .HasForeignKey(d => d.SolicitudPadreId)
                .HasConstraintName("FK_TbSolRegCannabis_SolicitudPadre");
        });

        modelBuilder.Entity<TbSolRegCannabisHistorial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbSolReg__3214EC0719C6A514");

            entity.Property(e => e.Comentario)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioRevisor)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.EstadoSolicitudIdHistorialNavigation).WithMany(p => p.TbSolRegCannabisHistorial)
                .HasForeignKey(d => d.EstadoSolicitudIdHistorial)
                .HasConstraintName("FK_TbSolRegCannabisHistorial_EstadoSolicitudHistorial");

            entity.HasOne(d => d.SolRegCannabis).WithMany(p => p.TbSolRegCannabisHistorial)
                .HasForeignKey(d => d.SolRegCannabisId)
                .HasConstraintName("FK__TbSolRegC__SolRe__1DB06A4F");
        });

        modelBuilder.Entity<TbSolSecuencia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbSolSec__3214EC0718D1B159");
        });

        modelBuilder.Entity<TbTipoDocumentoAdjunto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbTipoDo__3214EC0703C04C07");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.EsObligatorio).HasDefaultValue(true);
            entity.Property(e => e.EsParaMayorEdad).HasDefaultValue(false);
            entity.Property(e => e.EsParaMenorEdad).HasDefaultValue(false);
            entity.Property(e => e.EsParaRenovacion).HasDefaultValue(false);
            entity.Property(e => e.EsRequisitoParaAcompanante).HasDefaultValue(false);
            entity.Property(e => e.EsRequisitoParaPaciente).HasDefaultValue(false);
            entity.Property(e => e.EsSoloMedico).HasDefaultValue(false);
            entity.Property(e => e.IsActivo).HasDefaultValue(true);
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbTransferenciaResponsabilidad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbTransf__3214EC07F3C1091D");

            entity.HasIndex(e => e.Estado, "IX_TbTransferenciaResponsabilidad_Estado");

            entity.Property(e => e.AprobadoPor).HasMaxLength(450);
            entity.Property(e => e.Comentario).HasMaxLength(500);
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaAprobacion).HasColumnType("datetime");
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UsuarioDestinoId).HasMaxLength(450);
            entity.Property(e => e.UsuarioOrigenId).HasMaxLength(450);

            entity.HasOne(d => d.SolRegCannabis).WithMany(p => p.TbTransferenciaResponsabilidad)
                .HasForeignKey(d => d.SolRegCannabisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbTransferencia_Solicitud");
        });

        modelBuilder.Entity<TbUnidades>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbUnidad__3214EC079C52C060");

            entity.Property(e => e.IsActivo).HasDefaultValue(true);
            entity.Property(e => e.NombreUnidad)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbViaAdministracion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbViaAdm__3214EC07B78E96F0");

            entity.Property(e => e.IsActivo).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
