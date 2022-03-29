using FarmaciaSimanEntrenamientoProductos.Creation.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmaciaSimanEntrenamientoProductos.Infraestructure.Map
{
    public class SucursalesMap : IEntityTypeConfiguration<Sucursales>
    {
        public void Configure(EntityTypeBuilder<Sucursales> builder)
        {
            builder.ToTable("Sucursales");
            builder.HasKey(x => x.Codigo);
            builder.Property(x => x.Codigo).IsRequired();
            builder.Property(x => x.Contacto);
            builder.Property(x => x.Email);
            builder.Property(x => x.Sucursal);
            builder.Property(x => x.Telefono);
            builder.Property(x => x.Activa).HasColumnType("bit");
        }
    }
}
