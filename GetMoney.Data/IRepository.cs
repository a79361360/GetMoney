using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Data
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : Entity
    {
        IUnitOfWork UnitOfWork { get; }
        void Add(TEntity item);
        void Merge(TEntity persisted, TEntity current);
        void Remove(TEntity item);
        TEntity Get(Guid id);
        IEnumerable<TEntity> GetAll();
    }
}
