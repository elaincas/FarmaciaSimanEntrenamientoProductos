using FarmaciaSimanEntrenamientoProductos.Creations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Infraestructure.Map
{
    
        public class LogEventosMap : IEntityTypeConfiguration<LogEventos>
        {
            public void Configure(EntityTypeBuilder<LogEventos> builder)
            {
                builder.ToTable("LogEventos");
                builder.HasKey(c => c.Id);
                builder.Property(c => c.Id).IsRequired();
                builder.Property(c => c.Descripcion);
                builder.Property(c => c.Fecha);
                builder.Property(c => c.Pantalla);
                builder.Property(c => c.VersionSistema);

            }
    }
}
