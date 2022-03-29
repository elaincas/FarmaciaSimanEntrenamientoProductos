using FarmaciaSimanEntrenamientoProductos.Creation.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmaciaSimanEntrenamientoProductos.Infraestructure.Map
{
    public class ProductosMap : IEntityTypeConfiguration<Productos>
    {
        public void Configure(EntityTypeBuilder<Productos> builder)
        {
            builder.ToTable("Productos");
            builder.HasKey(x => x.ProductoID);
            builder.Property(x => x.ProductoID).IsRequired();
            builder.Property(x => x.Descripcion).HasMaxLength(50);
            builder.Property(x => x.Activo).HasColumnType("bit");
        }
    }
}
