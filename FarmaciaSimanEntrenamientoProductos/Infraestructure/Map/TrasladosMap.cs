using FarmaciaSimanEntrenamientoProductos.Creations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Infraestructure.Map
{
    public class TrasladosMap : IEntityTypeConfiguration<Traslados>
    {
        public void Configure(EntityTypeBuilder<Traslados> builder)
        {
            builder.ToTable("Traslados");
            builder.HasKey(x => x.TrasladoID);
            builder.Property(x => x.TrasladoID).IsRequired();
            builder.Property(x => x.ZonaIDEnvia);
            builder.Property(x => x.ZonaIDRecibe);
            builder.Property(x => x.Fecha);
            builder.Property(x => x.UsuarioID);
            builder.Property(x => x.Activo).HasColumnType("bit");

        }
    }
}
