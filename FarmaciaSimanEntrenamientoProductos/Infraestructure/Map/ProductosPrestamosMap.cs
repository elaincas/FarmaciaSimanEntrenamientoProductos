using FarmaciaSimanEntrenamientoProductos.Creation.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmaciaSimanEntrenamientoProductos.Infraestructure.Map
{
    public class ProductosPrestamosMap : IEntityTypeConfiguration<ProductosPrestamos>
    {
        public void Configure(EntityTypeBuilder<ProductosPrestamos> builder)
        {
            builder.ToTable("ProductosPrestamos");
            builder.HasKey(x => x.ProductoPrestamoID);
            builder.Property(x => x.ProductoPrestamoID).IsRequired();
            builder.Property(x => x.FechaPrestamo).HasColumnType("datetime");
            builder.Property(x => x.UsuarioIngresaDevolucionID);
            builder.Property(x => x.UsuarioPrestaID);
            builder.Property(x => x.FechaDevolucion).HasColumnType("datetime");
            builder.Property(x => x.Activo);
            builder.Property(x => x.ValeId);
            builder.Property(x => x.UsuarioIngresaVale);

        }
    }
}
