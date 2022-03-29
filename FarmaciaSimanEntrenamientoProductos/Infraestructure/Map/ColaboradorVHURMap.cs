using FarmaciaSimanEntrenamientoProductos.Creation.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Insfraestructure.Map
{
    public class ColaboradorVHURMap : IEntityTypeConfiguration<ColaboradorVHUR>
    {
        public void Configure(EntityTypeBuilder<ColaboradorVHUR> builder)
        {
            builder.ToTable("ColaboradorVHUR");
            builder.HasKey(x => x.ColaboradorID);
            builder.Property(x => x.ColaboradorID).IsRequired();
            builder.Property(x => x.DescripcionPuesto);
            builder.Property(x => x.FechaIngreso).HasColumnType("datetime");
            builder.Property(x => x.Nombre);
            builder.Property(x => x.PuestoID);
            builder.Property(x => x.Activo).HasColumnType("bit");
        }
    }
}
