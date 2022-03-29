using FarmaciaSimanEntrenamientoProductos.Infraestructura;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Infraestructure
{
    public class UnitOfWork : IUnitOfWork 
    {

        internal EntrenamientoProductosDbContext context;

     
        public UnitOfWork(EntrenamientoProductosDbContext _context)
        {
            context = _context;

        }

         
  

        public void Dispose()
        {
            context.Dispose();
        }
        public void Commit()
        {
            context.SaveChanges();
        }

        public DbSet<TEntity> Repository<TEntity>() where TEntity : class
        {
            DbSet<TEntity> dbSET = context.Set<TEntity>();
            return dbSET;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}