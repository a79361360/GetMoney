using GetMoney.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GetMoney.Dal.Notice
{
    public class NoticeDal
    {
        SqlDal dal = new SqlDal();
        /// <summary>
        /// 取得最近一条公告
        /// </summary>
        /// <returns></returns>
        public DataTable GetNotice() {
            string sql = "SELECT TOP 1 ID,Title,ContentTxt,AddTime FROM TNotice ORDER BY ID DESC";
            return dal.ExtSql(sql);
        }
    }
}
