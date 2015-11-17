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
        public DBContextOfUnitWork()
            : base("name=SQLConnString")
        {

        }
        //public DBContextOfUnitWork()
        //    : base(ConfigurationManager.ConnectionStrings["SQLConnString"].ConnectionString)
        //{

        //}
        public IDbSet<TEntity> CreateSet<TEntity>() where TEntity : class{
            return base.Set<TEntity>();
        }
        
        public void Attach<TEntity>(TEntity item) where TEntity : class
        {
            throw new NotImplementedException();
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
            base.OnModelCreating(modelBuilder);
        }
    }
}
