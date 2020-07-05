using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VM.Core.Contracts;

namespace VM.Infrastructure.DataAccess
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public readonly ExchangeDbContext _context;
        private bool _disposed;

        public GenericRepository(ExchangeDbContext context)
        {
            _context = context;
            _disposed = false;
        }

        public virtual async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> Insert(TEntity entity)
        {
            return (await _context.AddAsync(entity)).Entity;
        }

        public virtual async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
        
        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}