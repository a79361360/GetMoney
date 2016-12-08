using GetMoney.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GetMoney.Dal.Nsoup
{
    public class NsoupDal
    {
        SqlDal dal = new SqlDal();
        /// <summary>
        /// 取得TitleID的最大值
        /// </summary>
        /// <returns></returns>
        public int MaxTitleId() {
            string sql = "SELECT ISNULL(MAX(id),0) FROM Nsoup_ImgTitle";
            return Convert.ToInt32(dal.ExtScalarSql(sql));
        }
        /// <summary>
        /// 添加Nsoup图片类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool AddTitle(int type,string title) {
            string sql = "INSERT INTO Nsoup_ImgTitle(Type,Title)VALUES(@Type,@Title)";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Type",SqlDbType.Int),
                new SqlParameter("@Title",SqlDbType.NVarChar,150)
            };
            parameter[0].Value = type;
            parameter[1].Value = title;
            int result = dal.IntExtSql(sql, parameter);
            if (result > 0)
            { return true; }
            else { return false; }
        }
        /// <summary>
        /// 添加Nsoup图片类型,并直接返回Ttitleid最大值
        /// </summary>
        /// <param name="type">100无码101唯美</param>
        /// <param name="title">标题</param>
        /// <returns></returns>
        public int CreateTitle(int type, string title)
        {
            if (AddTitle(type, title)) {
                return MaxTitleId();
            }
            return -1;
        }
        /// <summary>
        /// 抓取图片的名细插入数据库
        /// </summary>
        /// <param name="type">100无码101唯美</param>
        /// <param name="titleid">标题ID</param>
        /// <param name="imgurl">图片地址</param>
        /// <returns></returns>
        public bool AddTitleDetail(int type,int titleid,string imgname,string imgurl) {
            string sql = "INSERT INTO Nsoup_ImgDetail(Type,TitleId,ImgName,ImgUrl)VALUES(@Type,@TitleId,@ImgName,@ImgUrl)";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Type",SqlDbType.Int),
                new SqlParameter("@TitleId",SqlDbType.Int),
                new SqlParameter("@ImgUrl",SqlDbType.NVarChar,250),
                new SqlParameter("@ImgName",SqlDbType.NVarChar,50)
            };
            parameter[0].Value = type;
            parameter[1].Value = titleid;
            parameter[2].Value = imgname;
            parameter[3].Value = imgurl;
            int result = dal.IntExtSql(sql, parameter);
            if (result > 0)
            { return true; }
            else { return false; }
        }

        public DataTable ListDetailPage(ref int Total, SqlPageParam param)
        {
            DataTable dt = dal.PageResult(param.TableName, param.PrimaryKey, param.Fields, param.PageSize, param.PageIndex, param.Filter, param.Group, param.Order, ref Total);
            return dt;
        }
    }
}
