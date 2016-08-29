using GetMoney.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Configuration;
using GetMoney.Dal.Configuration;

namespace GetMoney.Dal
{
    public class DBContextOfUnitWork
          : DbContext, IQueryableUnitOfWork
    {
        //public DBContextOfUnitWork()
        //    : base("GetMoney")
        //{

        //}
        public DBContextOfUnitWork()
            : base("server=127.0.0.1,1433;database=GetMoney;user id=sa;password=123;MultipleActiveResultSets=true;")
        {

        }
        public IDbSet<TEntity> CreateSet<TEntity>() where TEntity : class{
            return base.Set<TEntity>();
        }
        
        public void Attach<TEntity>(TEntity item) where TEntity : class
        {
            base.Entry<TEntity>(item).State = (EntityState)System.Data.EntityState.Unchanged;
        }

        public void SetModified<TEntity>(TEntity item) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            base.SaveChanges();
        }

        public void CommitAndRefreshChanges()
        {
            throw new NotImplementedException();
        }

        public void RollbackChanges()
        {
            throw new NotImplementedException();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new OnlyNameTestConfiguration());
            modelBuilder.Configurations.Add(new CardConfiguration());
            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Configurations.Add(new TUserConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
