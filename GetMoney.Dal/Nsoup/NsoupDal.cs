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
        public bool AddTitle(int type, string title) {
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
        public bool AddTitleDetail(int type, int titleid, string imgname, string imgurl) {
            string sql = "INSERT INTO Nsoup_ImgDetail(Type,TitleId,ImgName,ImgUrl)VALUES(@Type,@TitleId,@ImgName,@ImgUrl)";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Type",SqlDbType.Int),
                new SqlParameter("@TitleId",SqlDbType.Int),
                new SqlParameter("@ImgName",SqlDbType.NVarChar,50),
                new SqlParameter("@ImgUrl",SqlDbType.NVarChar,250)
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
        /// <summary>
        /// 根据titleid返回列表
        /// </summary>
        /// <param name="type">图片类型</param>
        /// <param name="titleid">titleid</param>
        /// <returns></returns>
        public DataTable ListDetailTitle(int type, int titleid) {
            string sql = "select id,TitleName,ImgUrl from V_NsoupImg where Type=@type and TitleId=@titleid";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@type",SqlDbType.Int),
                new SqlParameter("@titleid",SqlDbType.Int)
            };
            parameter[0].Value = type;
            parameter[1].Value = titleid;
            DataTable dt = dal.ExtSql(sql, parameter);
            return dt;
        }
        public DataTable ListDetailPage(ref int Total, SqlPageParam param)
        {
            DataTable dt = dal.PageResult(param.TableName, param.PrimaryKey, param.Fields, param.PageSize, param.PageIndex, param.Filter, param.Group, param.Order, ref Total);
            return dt;
        }
        public int AddBankBin(int id, string bankName, string bankNameEn, string cardName, string cardType, long bin, int nLength, int binLength, long issueid)
        { 
            string sql = "INSERT INTO [TBankBin]([id],[bankName],[bankNameEn],[cardName],[cardType],[bin],[nLength],[binLength],[issueid])";
            sql += "VALUES(@id,@bankName,@bankNameEn,@cardName,@cardType,@bin,@nLength,@binLength,@issueid)";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@id",SqlDbType.Int),
                new SqlParameter("@bankName",SqlDbType.NVarChar,50),
                new SqlParameter("@bankNameEn",SqlDbType.NVarChar,20),
                new SqlParameter("@cardName",SqlDbType.NVarChar,50),
                new SqlParameter("@cardType",SqlDbType.NVarChar,20),
                new SqlParameter("@bin",SqlDbType.BigInt),
                new SqlParameter("@nLength",SqlDbType.Int),
                new SqlParameter("@binLength",SqlDbType.Int),
                new SqlParameter("@issueid",SqlDbType.BigInt),
            };
            parameter[0].Value = id;
            parameter[1].Value = bankName;
            parameter[2].Value = bankNameEn;
            parameter[3].Value = cardName;
            parameter[4].Value = cardType;
            parameter[5].Value = bin;
            parameter[6].Value = nLength;
            parameter[7].Value = binLength;
            parameter[8].Value = issueid;
            int result = dal.IntExtSql(sql, parameter);
            return result;
        }
    }
}
