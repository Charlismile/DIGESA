using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.BDUbicaciones;

public partial class DbUbicacionPanama : DbContext
{
    public DbUbicacionPanama()
    {
    }

    public DbUbicacionPanama(DbContextOptions<DbUbicacionPanama> options)
        : base(options)
    {
    }

    public virtual DbSet<Corregimiento> Corregimiento { get; set; }

    public virtual DbSet<Distrito> Distrito { get; set; }

    public virtual DbSet<InstalacionSalud> InstalacionSalud { get; set; }

    public virtual DbSet<Provincia> Provincia { get; set; }

    public virtual DbSet<RegionSalud> RegionSalud { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:UbicacionConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Corregimiento>(entity =>
        {
            entity.HasKey(e => e.CorregimientoId).HasName("PK__Corregim__3214EC07D5083803");

            entity.HasIndex(e => new { e.CodigoCorregimiento, e.DistritoId }, "UQ_Corregimiento_Codigo").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(100);

            entity.HasOne(d => d.Distrito).WithMany(p => p.Corregimiento)
                .HasForeignKey(d => d.DistritoId)
                .HasConstraintName("FK__Corregimi__Distr__31EC6D26");
        });

        modelBuilder.Entity<Distrito>(entity =>
        {
            entity.HasKey(e => e.DistritoId).HasName("PK__Distrito__3214EC07A244901D");

            entity.HasIndex(e => new { e.CodigoDistrito, e.ProvinciaId }, "UQ_Distrito_Codigo").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(100);

            entity.HasOne(d => d.Provincia).WithMany(p => p.Distrito)
                .HasForeignKey(d => d.ProvinciaId)
                .HasConstraintName("FK__Distrito__Provin__2E1BDC42");
        });

        modelBuilder.Entity<InstalacionSalud>(entity =>
        {
            entity.HasKey(e => e.InstalacionId).HasName("PK__Instalac__14B99F7A2F07A6D7");

            entity.Property(e => e.InstalacionId)
                .ValueGeneratedNever()
                .HasColumnName("instalacion_Id");
            entity.Property(e => e.Instalacion)
                .HasMaxLength(255)
                .HasColumnName("instalacion");
        });

        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.HasKey(e => e.ProvinciaId).HasName("PK__Provinci__3214EC07AEBA6305");

            entity.HasIndex(e => e.CodigoProvincia, "UQ_Provincia_Codigo").IsUnique();

            entity.HasIndex(e => e.Nombre, "UQ_Provincia_Nombre").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(100);

            entity.HasOne(d => d.RegionSalud).WithMany(p => p.Provincia)
                .HasForeignKey(d => d.RegionSaludId)
                .HasConstraintName("FK__Provincia__Regio__2A4B4B5E");
        });

        modelBuilder.Entity<RegionSalud>(entity =>
        {
            entity.HasKey(e => e.RegionSaludId).HasName("PK__RegionSa__3214EC070C5BE500");

            entity.HasIndex(e => e.Nombre, "UQ__RegionSa__75E3EFCFAD432D6D").IsUnique();

            entity.HasIndex(e => e.CodigoRegion, "UQ__RegionSa__7F1F2F9BE656A3B2").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
