using FarmaciaSimanEntrenamientoProductos.Creations.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Infraestructure.Map
{
    public class ZonasMap : IEntityTypeConfiguration<Zonas>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Zonas> builder)
        {
            builder.ToTable("Zonas");
            builder.HasKey(x => x.ZonaID);
            builder.Property(x => x.ZonaID).IsRequired();
            builder.Property(x => x.Zona);
        }
    }
}
