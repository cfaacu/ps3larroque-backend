using Microsoft.EntityFrameworkCore;
using PS3Larroque.Domain.Entities;

namespace PS3Larroque.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductoLegacy> ProductosLegacy { get; set; }
        public DbSet<StockSucursal> StocksSucursal { get; set; }
        public DbSet<Preventa> Preventas { get; set; }
        public DbSet<PreventaItem> PreventaItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Para Postgres en Render (no obligatorio, pero prolijo):
            // deja explícito que usamos el schema "public"
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<ProductoLegacy>(entity =>
            {
                entity.ToTable("productos_legacy");
                entity.HasKey(e => e.Codigo);

                entity.Property(e => e.Codigo).HasColumnName("codigo");
                entity.Property(e => e.Consola).HasColumnName("consola");
                entity.Property(e => e.Descripcion).HasColumnName("descripcion");
                entity.Property(e => e.PrecioVentaNuevo).HasColumnName("precio_venta_nuevo");
                entity.Property(e => e.PrecioCompraUsado).HasColumnName("precio_compra_usado");
            });

            modelBuilder.Entity<StockSucursal>(entity =>
            {
                entity.ToTable("stock_sucursal");
                entity.HasKey(e => new { e.Codigo, e.Sucursal });

                entity.Property(e => e.Codigo).HasColumnName("codigo");
                entity.Property(e => e.Consola).HasColumnName("consola");
                entity.Property(e => e.Descripcion).HasColumnName("descripcion");
                entity.Property(e => e.Tipo).HasColumnName("tipo");
                entity.Property(e => e.Categoria).HasColumnName("categoria");
                entity.Property(e => e.Sucursal).HasColumnName("sucursal");
                entity.Property(e => e.Stock).HasColumnName("stock");
                entity.Property(e => e.Precio).HasColumnName("precio");
                entity.Property(e => e.ActualizadoEn).HasColumnName("actualizado_en");

                entity.HasOne(s => s.Producto)
                      .WithMany(p => p.Stocks)
                      .HasForeignKey(s => s.Codigo);
            });

            modelBuilder.Entity<Preventa>(entity =>
            {
                entity.ToTable("preventa");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Sucursal).HasColumnName("sucursal");
                entity.Property(e => e.Vendedor).HasColumnName("vendedor");
                entity.Property(e => e.FechaCreacion).HasColumnName("creada_en");
                entity.Property(e => e.Estado).HasColumnName("estado");
            });

            modelBuilder.Entity<PreventaItem>(entity =>
            {
                entity.ToTable("preventa_item");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.PreventaId).HasColumnName("preventa_id");
                entity.Property(e => e.Codigo).HasColumnName("codigo");
                entity.Property(e => e.Descripcion).HasColumnName("descripcion");
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");
                entity.Property(e => e.PrecioUnit).HasColumnName("precio_unit");

                entity.HasOne(e => e.Preventa)
                    .WithMany(p => p.Items)
                    .HasForeignKey(e => e.PreventaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
