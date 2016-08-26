using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetMoney.Data
{
    public class SqlConn : IDisposable
    {
        protected SqlConnection MSqlConn;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SqlConn()
        {
            MSqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SQLConnString"].ConnectionString);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (MSqlConn != null)
            {
                MSqlConn.Close();
                MSqlConn.Dispose();
            }
        }
    }
}
