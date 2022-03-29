using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Infraestructure
{
    public interface IEntrenamientoProductoContext : IDisposable
    {
      
            DbSet<TEntity> Set<TEntity>() where TEntity : class;

            Task<int> SaveChangesAsync();
        }

        
}
