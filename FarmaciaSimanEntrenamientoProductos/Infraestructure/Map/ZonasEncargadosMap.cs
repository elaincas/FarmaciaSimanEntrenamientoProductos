using FarmaciaSimanEntrenamientoProductos.Creations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Infraestructure.Map
{
    public class ZonasEncargadosMap : IEntityTypeConfiguration<ZonasEncargados>
    {
        public void Configure(EntityTypeBuilder<ZonasEncargados> builder)
        {
            builder.ToTable("ZonasEncargados");
            builder.HasKey(x => x.ZonaEncargadoID);
            builder.Property(x => x.ZonaID).IsRequired();
            builder.Property(x => x.ZonaEncargadoID);
            builder.Property(x => x.Descripcion).IsRequired();
            builder.Property(x => x.UsuarioEncargadoID);
            builder.Property(x => x.Activo);

        }
    }
}
