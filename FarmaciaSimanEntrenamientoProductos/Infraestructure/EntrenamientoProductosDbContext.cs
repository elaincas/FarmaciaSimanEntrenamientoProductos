
using EntrenamientoProductos.Insfraestructure.Map;
using FarmaciaSimanEntrenamientoProductos.Creation.Entities;
using FarmaciaSimanEntrenamientoProductos.Creations.Entities;
using FarmaciaSimanEntrenamientoProductos.Infraestructure;
using FarmaciaSimanEntrenamientoProductos.Infraestructure.Map;
using FarmaciaSimanEntrenamientoProductos.Insfraestructure.Map;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Infraestructura
{
    public class EntrenamientoProductosDbContext : DbContext
    {
        public EntrenamientoProductosDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<EstadosProductos> EstadoProductos { get; set; }
        public DbSet<ProductosDetalles> ProductosDetalle { get; set; }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<ProductosPrestamos> ProductoPrestamo { get; set; }
        public DbSet<Sucursales> Sucursales { get; set; }
        public DbSet<UsuarioInformacionGeneral> UsuarioInformacionGeneral { get; set; }
        public DbSet<ColaboradorVHUR> ColaboradorVhur { get; set; }
        public DbSet<TransaccionesProductos> TransaccionesProductos { get; set; }
        public DbSet<ZonasEncargados> ZonasEncargados { get; set; }
        public DbSet<Zonas> Zonas { get; set; }
        public DbSet<spAuxiliarProductos> spAuxiliarProductos { get; set; }
        public DbSet<Traslados> Traslados { get; set; }
        public DbSet<TrasladosDetalle> TrasladosDetalle { get; set; }
        public DbSet<LogEventos> LogEventos { get; set; }

        public void Commit()
        {
            SaveChanges();
        }



        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<EstadosProductos>(new EstadoMap());
            modelBuilder.ApplyConfiguration<ProductosDetalles>(new ProductosDetallesMap());
            modelBuilder.ApplyConfiguration<Productos>(new ProductosMap());
            modelBuilder.ApplyConfiguration<ProductosPrestamos>(new ProductosPrestamosMap());
            modelBuilder.ApplyConfiguration<TransaccionesProductos>(new TransaccionesProductosMap());
            modelBuilder.ApplyConfiguration<Traslados>(new TrasladosMap());
            modelBuilder.ApplyConfiguration<TrasladosDetalle>(new TrasladosDetalleMap());
            modelBuilder.ApplyConfiguration<ColaboradorVHUR>(new ColaboradorVHURMap());
            modelBuilder.ApplyConfiguration<LogEventos>(new LogEventosMap());
            modelBuilder.ApplyConfiguration<Sucursales>(new SucursalesMap());
            modelBuilder.ApplyConfiguration<UsuarioInformacionGeneral>(new UsuarioInformacionGeneralMap());
            modelBuilder.ApplyConfiguration<Zonas>(new ZonasMap());
            modelBuilder.ApplyConfiguration<ZonasEncargados>(new ZonasEncargadosMap());
        }




    }
}
