using FarmaciaSimanEntrenamientoProductos.Creations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Infraestructure.Map
{
    public class TrasladosDetalleMap : IEntityTypeConfiguration<TrasladosDetalle>
    {
        public void Configure(EntityTypeBuilder<TrasladosDetalle> builder)
        {
            builder.ToTable("TrasladosDetalles");
            builder.HasKey(x => x.TrasladoDetalleID);
            builder.Property(x => x.TrasladoID).IsRequired();
            builder.Property(x => x.TrasladoDetalleID);
            builder.Property(x => x.ProductoDetalleID);
            builder.Property(x => x.EstadoID);
            builder.Property(x => x.FechaCambioEstado);
            builder.Property(x => x.Observacion);

            builder.HasOne(x => x.ProductoDetalle).WithMany(x => x.TrasladosDetalles).HasForeignKey(x => x.ProductoDetalleID);
            builder.HasOne(x => x.EstadoProducto).WithMany(x => x.TrasladosDetalles).HasForeignKey(x => x.EstadoID);
            builder.HasOne(x => x.Traslado).WithMany(x => x.TrasladosDetalles).HasForeignKey(x => x.TrasladoID);



        }
    }
}
