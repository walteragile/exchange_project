using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VM.Core.Contracts
{
    public interface IRepository<TEntity> : IDisposable
    {
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null);
        Task<TEntity> Insert(TEntity entity);
        Task<int> SaveAsync();
    }
}