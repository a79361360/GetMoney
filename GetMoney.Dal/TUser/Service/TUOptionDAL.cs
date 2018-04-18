using GetMoney.Data;
using GetMoney.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GetMoney.Dal
{
    public class TUOptionDAL
    {
        SqlDal dal = new SqlDal();
        public DataTable ListPage(ref int Total, SqlPageParam Param)
        {
            DataTable dt = dal.PageResult(Param.TableName, Param.PrimaryKey, Param.Fields, Param.PageSize, Param.PageIndex, Param.Filter, Param.Group, Param.Order, ref Total);
            return dt;
        }
        public int SetTUOption(TUOptionDTO dto) {
            string sql = "INSERT INTO [TUOption]([userid],[title],[type],[content])VALUES(@userid,@title,@type,@content)";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@userid",SqlDbType.Int),
                new SqlParameter("@title",SqlDbType.NVarChar,50),
                new SqlParameter("@type",SqlDbType.Int),
                new SqlParameter("@content",SqlDbType.NVarChar,2000),
            };
            parameter[0].Value = dto.userid;
            parameter[1].Value = dto.title;
            parameter[2].Value = dto.type;
            parameter[3].Value = dto.content;
            return dal.IntExtSql(sql, parameter);
        }
        public DataTable FindTOptionById(int id) {
            string sql = "SELECT id,userid,type,title,content,rcontent FROM TUOption WHERE id=@id";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@id",SqlDbType.Int),
            };
            parameter[0].Value = id;
            return dal.ExtSql(sql, parameter);
        }
    }
}
