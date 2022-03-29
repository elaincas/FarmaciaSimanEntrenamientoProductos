using FarmaciaSimanEntrenamientoProductos.Creation.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmaciaSimanEntrenamientoProductos.Infraestructure.Map
{
    public class EstadoMap : IEntityTypeConfiguration<EstadosProductos>
    {
        public void Configure(EntityTypeBuilder<EstadosProductos> builder)
        {
            builder.ToTable("EstadosProductos");
            builder.HasKey(x => x.EstadoID);
            builder.Property(x => x.EstadoID).IsRequired();
            builder.Property(x => x.Descripcion).HasMaxLength(50);
        }
    }
}
