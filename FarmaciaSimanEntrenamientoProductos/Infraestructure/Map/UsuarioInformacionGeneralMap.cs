using FarmaciaSimanEntrenamientoProductos.Creation.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmaciaSimanEntrenamientoProductos.Infraestructure.Map
{
    public class UsuarioInformacionGeneralMap : IEntityTypeConfiguration<UsuarioInformacionGeneral>
    {
        public void Configure(EntityTypeBuilder<UsuarioInformacionGeneral> builder)
        {
            builder.ToTable("UsuarioInformacionGeneral");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).IsRequired();
            builder.Property(x => x.Perfil_ID);
            builder.Property(x => x.Usuario);
            builder.Property(x => x.Clave).HasColumnType("varbinary(50)");
            builder.Property(x => x.Activo);
            builder.Property(x => x.AplicacionId);

        }
    }
}
