using GetMoney.Data;
using GetMoney.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GetMoney.Dal.TUser.Service
{
    public class TUOptionDAL
    {
        SqlDal dal = new SqlDal();
        public int SetTUOption(TUOptionDTO dto) {
            string sql = "INSERT INTO [TUOption]([title],[type],[content])VALUES(@title,@type,@content)";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@title",SqlDbType.NVarChar,50),
                new SqlParameter("@type",SqlDbType.Int),
                new SqlParameter("@content",SqlDbType.NVarChar,2000),
            };
            parameter[0].Value = dto.title;
            parameter[1].Value = dto.type;
            parameter[2].Value = dto.content;
            return dal.IntExtSql(sql, parameter);
        }
        public DataTable FindTOptionById(int id) {
            string sql = "SELECT type,title,content,rcontent FROM TUOption WHERE id=@id";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@id",SqlDbType.Int),
            };
            parameter[0].Value = id;
            return dal.ExtSql(sql, parameter);
        }
    }
}
