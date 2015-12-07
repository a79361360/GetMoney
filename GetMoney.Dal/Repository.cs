using GetMoney.Data;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Dal
{
    public class Repository<T>
    : IRepository<T> where T : Entity
    {
        IQueryableUnitOfWork _UnitOfWork;
        public Repository(IQueryableUnitOfWork unitOfWork)
        {
            if (unitOfWork == (IQueryableUnitOfWork)null)
                throw new ArgumentNullException();
            _UnitOfWork = unitOfWork;
        }
        public IUnitOfWork UnitOfWork
        {
            get { return _UnitOfWork; }
        }

        public void Add(T item)
        {
            if (item != (T)null)
            {
                CreateSet().Add(item);
            }
            else
            {
                //添加日志功能
            }
        }

        public void Merge(T persisted, T current)
        {
            throw new NotImplementedException();
        }

        public void Remove(T item)
        {
            throw new NotImplementedException();
        }

        public T Get(int id)
        {
            if (id != 0)
                return CreateSet().SingleOrDefault(p => p.id == id);
            else
                return null;
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        IDbSet<T> CreateSet()
        {
            return _UnitOfWork.CreateSet<T>();
        }


        public T Get(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
