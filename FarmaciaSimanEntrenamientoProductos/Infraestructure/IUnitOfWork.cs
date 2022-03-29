using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Infraestructure
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void Dispose();
        DbSet<TEntity> Repository<TEntity>() where TEntity : class;
        void SaveChanges();

    }
}
