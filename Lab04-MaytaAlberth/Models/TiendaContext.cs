using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Lab04_MaytaAlberth.Models;

public partial class TiendaContext : DbContext
{
    public TiendaContext()
    {
    }

    public TiendaContext(DbContextOptions<TiendaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Detallesorden> Detallesordens { get; set; }

    public virtual DbSet<Ordene> Ordenes { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=tiendadb;user=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.32-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId).HasName("PRIMARY");

            entity.ToTable("categorias");

            entity.Property(e => e.CategoriaId)
                .HasColumnType("int(11)")
                .HasColumnName("CategoriaID");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PRIMARY");

            entity.ToTable("clientes");

            entity.Property(e => e.ClienteId)
                .HasColumnType("int(11)")
                .HasColumnName("ClienteID");
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Detallesorden>(entity =>
        {
            entity.HasKey(e => e.DetalleId).HasName("PRIMARY");

            entity.ToTable("detallesorden");

            entity.HasIndex(e => e.OrdenId, "FK_DetallesOrden_Ordenes");

            entity.HasIndex(e => e.ProductoId, "FK_DetallesOrden_Productos");

            entity.Property(e => e.DetalleId)
                .HasColumnType("int(11)")
                .HasColumnName("DetalleID");
            entity.Property(e => e.Cantidad).HasColumnType("int(11)");
            entity.Property(e => e.OrdenId)
                .HasColumnType("int(11)")
                .HasColumnName("OrdenID");
            entity.Property(e => e.Precio).HasPrecision(10, 2);
            entity.Property(e => e.ProductoId)
                .HasColumnType("int(11)")
                .HasColumnName("ProductoID");

            entity.HasOne(d => d.Orden).WithMany(p => p.Detallesordens)
                .HasForeignKey(d => d.OrdenId)
                .HasConstraintName("FK_DetallesOrden_Ordenes");

            entity.HasOne(d => d.Producto).WithMany(p => p.Detallesordens)
                .HasForeignKey(d => d.ProductoId)
                .HasConstraintName("FK_DetallesOrden_Productos");
        });

        modelBuilder.Entity<Ordene>(entity =>
        {
            entity.HasKey(e => e.OrdenId).HasName("PRIMARY");

            entity.ToTable("ordenes");

            entity.HasIndex(e => e.ClienteId, "FK_Ordenes_Clientes");

            entity.Property(e => e.OrdenId)
                .HasColumnType("int(11)")
                .HasColumnName("OrdenID");
            entity.Property(e => e.ClienteId)
                .HasColumnType("int(11)")
                .HasColumnName("ClienteID");
            entity.Property(e => e.FechaOrden)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime");
            entity.Property(e => e.Total).HasPrecision(10, 2);

            entity.HasOne(d => d.Cliente).WithMany(p => p.Ordenes)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("FK_Ordenes_Clientes");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.PagoId).HasName("PRIMARY");

            entity.ToTable("pagos");

            entity.HasIndex(e => e.OrdenId, "FK_Pagos_Ordenes");

            entity.Property(e => e.PagoId)
                .HasColumnType("int(11)")
                .HasColumnName("PagoID");
            entity.Property(e => e.FechaPago)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime");
            entity.Property(e => e.MetodoPago).HasMaxLength(50);
            entity.Property(e => e.Monto).HasPrecision(10, 2);
            entity.Property(e => e.OrdenId)
                .HasColumnType("int(11)")
                .HasColumnName("OrdenID");

            entity.HasOne(d => d.Orden).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.OrdenId)
                .HasConstraintName("FK_Pagos_Ordenes");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("PRIMARY");

            entity.ToTable("productos");

            entity.HasIndex(e => e.CategoriaId, "FK_Productos_Categorias");

            entity.Property(e => e.ProductoId)
                .HasColumnType("int(11)")
                .HasColumnName("ProductoID");
            entity.Property(e => e.CategoriaId)
                .HasColumnType("int(11)")
                .HasColumnName("CategoriaID");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasPrecision(10, 2);
            entity.Property(e => e.Stock).HasColumnType("int(11)");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaId)
                .HasConstraintName("FK_Productos_Categorias");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
