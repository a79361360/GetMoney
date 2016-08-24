using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Data
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 在容器里提交所有的更改
        /// </summary>
        void Commit();

        /// <summary>
        /// 在容器里提交所有的更改
        /// </summary>
        void CommitAndRefreshChanges();

        /// <summary>
        /// 回滚所有没有存储到数据库中的更改
        /// </summary>
        void RollbackChanges();
    }
}
