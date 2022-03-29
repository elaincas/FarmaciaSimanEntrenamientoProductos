using FarmaciaSimanEntrenamientoProductos.Creation.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmaciaSimanEntrenamientoProductos.Infraestructure.Map
{
    public class ProductosDetallesMap : IEntityTypeConfiguration<ProductosDetalles>
    {
        public void Configure(EntityTypeBuilder<ProductosDetalles> builder)
        {
            builder.ToTable("ProductosDetalles");
            builder.HasKey(x => x.ProductoDetalleID);
            builder.Property(x => x.ProductoDetalleID).IsRequired();
            builder.Property(x => x.ProductoID);
            builder.Property(x => x.Precio).HasColumnType("money");
            builder.Property(x => x.EstadoID);
            builder.Property(x => x.FechaIngreso).HasColumnType("datetime");
            builder.Property(x => x.UsuarioIngresa);
            builder.Property(x => x.Observacion);
            builder.Property(x => x.ZonaID);
            builder.Property(x => x.Activo).HasColumnType("bit");

            builder.HasOne(x => x.Producto).WithMany(x => x.ProductosDetalles).HasForeignKey(x => x.ProductoID);
            builder.HasOne(x => x.Estado).WithMany(x => x.ProductosDetalles).HasForeignKey(x => x.EstadoID);

        }
    }
}
