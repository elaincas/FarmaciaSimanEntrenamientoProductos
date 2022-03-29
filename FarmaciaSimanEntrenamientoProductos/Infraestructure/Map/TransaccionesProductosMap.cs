using FarmaciaSimanEntrenamientoProductos.Creation.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntrenamientoProductos.Insfraestructure.Map
{
    public class TransaccionesProductosMap : IEntityTypeConfiguration<TransaccionesProductos>
    {
        public void Configure(EntityTypeBuilder<TransaccionesProductos> builder)
        {
            builder.ToTable("TransaccionesProductos");
            builder.HasKey(x => x.TransaccionesProductosID);
            builder.Property(x => x.TransaccionesProductosID).IsRequired();
            builder.Property(x => x.ProductoDetalleID);
            builder.Property(x=>x.ProductoPrestamoID);
            builder.Property(x => x.EstadoID);
            builder.Property(x => x.FechaCambioEstado);
            builder.Property(x => x.UsuarioID);
            builder.Property(x => x.Activo).HasColumnType("bit");
            builder.Property(x => x.ZonaID);

            builder.HasOne(x => x.Estado).WithMany(x => x.Transacciones).HasForeignKey(x => x.EstadoID);
            builder.HasOne(x => x.ProductoPrestamo).WithMany(x => x.Transacciones).HasForeignKey(x => x.ProductoPrestamoID);
            builder.HasOne(x => x.ProductoDetalle).WithMany(x => x.Transacciones).HasForeignKey(x => x.ProductoDetalleID);

        }
    }
}
