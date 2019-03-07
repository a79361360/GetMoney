using GetMoney.Data.Order;
using GetMoney.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GetMoney.Framework.Common;
using GetMoney.Dal;
using System.Data.OleDb;
using System.Data;
using GetMoney.Common.Expand;

namespace GetMoney.Application
{
    public class OrderBll : IOrderBll
    {
        IOrderRepository _repostory;
        IOrderDal _dal;
        public OrderBll(IOrderRepository repostory,IOrderDal dal)
        {
            _repostory = repostory;
            _dal = dal;
        }
        public OrderBll()
        {
            _dal = new OrderDal();
        }
        /// <summary>
        /// 添加会单
        /// </summary>
        /// <param name="dto"></param>
        public void AddOrder(OrderDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException();
            var _unitOfWork = _repostory.UnitOfWork;
            var _build = OrderFactory.Create(
                dto.OrderNo,
                dto.PeoperNum,
                dto.PeoperMoney,
                (int)dto.MoneySendType,
                dto.MeetType,
                dto.MeetNum,
                dto.FirstExtraDate,
                dto.ExtraDate,
                dto.InputDate,
                dto.State,
                dto.Remark
                );
            _repostory.Add(_build);
            _unitOfWork.Commit();
        }

        public bool RemoveOrders(string[] ids)
        {
            bool result = false;
            if (ids.Length == 0)
            {
                throw new ArgumentNullException();
            }
            else
            {
                try
                {
                    foreach (var id in ids)
                    {
                        int gid = id.ToInt32();
                        var _dto = _repostory.Get(gid);
                        _repostory.Remove(_dto);
                        _repostory.UnitOfWork.Commit();
                    }
                    result = true;
                }
                catch
                {
                    result = false;
                    return result;
                }
            }
            return result;
        }

        public bool RemoveOrder(string id) {
            bool result = false;
            try
            {
                int gid = id.ToInt32();
                var _dto = _repostory.Get(gid);
                _repostory.Remove(_dto);
                _repostory.UnitOfWork.Commit();
                result = true;
            }
            catch {
                result = false;
            }
            return result;
        }


        public IList<OrderDto> ListOrderPage(ref int Total, int pageSize, int pageIndex)
        {
            SqlPageParam param = new SqlPageParam();
            param.TableName = "Orders";
            param.PrimaryKey = "id";
            param.Fields = "id,OrderNo,PeoperNum,PeoperMoney,InputDate,Remark,MoneySendType,MeetType,MeetNum,FirstExtraDate,ExtraDate,State";
            param.PageSize = pageSize;
            param.PageIndex = pageIndex;
            param.Filter = "";
            param.Group = "";
            param.Order = "id";
            IList<OrderDto> list = DataTableToList.ModelConvertHelper<OrderDto>.ConvertToModel(_dal.ListOrderPage(ref Total, param));
            return list;
        }
        public IList<OrderDto> ListOrderPage(ref int Total, int pageSize, int pageIndex, string filter)
        {
            SqlPageParam param = new SqlPageParam();
            param.TableName = "View_OrderUser";
            param.PrimaryKey = "id";
            param.Fields = "id,OrderNo,PeoperNum,PeoperMoney,MoneySendType,MeetType,MeetNum,FirstDate,CONVERT(varchar(19),FirstDate,120) FirstDate1,InputDate,State,Remark,LowestMoney,TouUserid,TouTrueName,FirstExtraDate,ExtraDate,Address,MeetExtNum";
            param.PageSize = pageSize;
            param.PageIndex = pageIndex;
            param.Filter = filter;
            param.Group = "";
            param.Order = "id";
            IList<OrderDto> list = DataTableToList.ModelConvertHelper<OrderDto>.ConvertToModel(_dal.ListOrderPage(ref Total, param));
            foreach (var item in list) {
                item.MSType = Enum.GetName(typeof(MnSdTypeEnum), (int)item.MoneySendType);
            }
            return list;
        }
        public OrderDto GetOrderByOrderID(string OrderID) {
            OrderDto dto = DataTableToList.ModelConvertHelper<OrderDto>.ConvertToModel(_dal.GetOrderByOrderID(OrderID))[0];
            return dto;
        }
        public int CreateOrder(OrderDto dto)
        {
            dto.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");    //互助单号
            Dictionary<string, object> dic;
            int result = 0;
            _dal.CreateOrder(dto.OrderNo, dto.PeoperNum, dto.PeoperIds, dto.PeoperMoney, dto.LowestMoney, dto.TouUserid, (int)dto.MoneySendType, dto.MeetType, dto.MeetNum, dto.Meetextnum, dto.FirstDate, dto.FirstExtraDate, dto.ExtraDate,dto.Address, dto.Remark, out dic);
            if (Convert.ToInt32(dic["@ReturnValue"]) == 1)
            {
                result = 1;
            }
            return result;
        }
        public string ListToString(IList<UListDto> list) {
            IList<int> intlist = new List<int>();
            foreach (var item in list) {
                intlist.Add(item.id);
            }
            string result = string.Join(",", intlist.ToArray());
            return result;
        }
        public IList<OrderListDto> OrderLists(string No) {
            IList<OrderListDto> list = DataTableToList.ModelConvertHelper<OrderListDto>.ConvertToModel(_dal.OrderLists(No));
            //存在已结束未更新结果的记录
            int num = 0;
            foreach (var item in list) {
                if (item.State == "2" && (DateTime.Now > Convert.ToDateTime(item.MeetDate))) {
                    UpdateOrderListState(No, item.ID.ToString());
                    num++;
                }
            }
            if (num > 0) {
                list = DataTableToList.ModelConvertHelper<OrderListDto>.ConvertToModel(_dal.OrderLists(No));
            }
            return list;
        }
        public IList<OrderListUserDto> OrderListUser(string OrderListID) {
            IList<OrderListUserDto> list = DataTableToList.ModelConvertHelper<OrderListUserDto>.ConvertToModel(_dal.OrderListUser(OrderListID));
            return list;
        }
        public OrderListDto UpdateOrderListState(string OrderNo, string OrderListID) {
            OrderListDto dto = DataTableToList.ModelConvertHelper<OrderListDto>.ConvertToModel(_dal.UpdateOrderListState(OrderNo, OrderListID))[0];
            return dto;
        }
        public int UpdateOrderListUserMoney(string OrderNo, string OrderListID, int Userid, int Money) {
            int result = _dal.UpdateOrderListUserMoney(OrderNo, OrderListID, Userid, Money);
            return result;
        }
        public OrderListUserDto GetOrderListUserPrvMoney(int Userid, string OrderNo, string OrderListID)
        {
            OrderListUserDto dto = DataTableToList.ModelConvertHelper<OrderListUserDto>.ConvertToModel(_dal.GetOrderListUserPrvMoney(Userid, OrderNo, OrderListID))[0];
            return dto;
        }
        public int VerUserUpdateMoney(int Userid, string OrderNo, string OrderListID) {
            int result = _dal.VerUserUpdateMoney(Userid, OrderNo, OrderListID);
            return result;
        }
        public int DelOrder(string OrderNo) {
            int result = _dal.DelOrder(OrderNo);
            return result;
        }
        /// <summary>
        /// 今天开标的用户信息
        /// </summary>
        /// <returns></returns>
        public IList<OrderListUserDto> FindCurOrderList() {
            IList<OrderListUserDto> list = DataTableToList.ModelConvertHelper<OrderListUserDto>.ConvertToModel(_dal.FindCurOrderList());
            return list;
        }
        public IList<OrderListUserDto> FindListOrder(string orderno, int userid) {
            IList<OrderListUserDto> list = DataTableToList.ModelConvertHelper<OrderListUserDto>.ConvertToModel(_dal.FindListOrder(orderno, userid));
            return list;
        }
        public IList<OrderListUserDto> FindListOrder(string orderno)
        {
            IList<OrderListUserDto> list = DataTableToList.ModelConvertHelper<OrderListUserDto>.ConvertToModel(_dal.FindListOrder(orderno));
            return list;
        }
        public int CreateOrderByImport(int touuid,int filetype) {
            string pathx = "/DownLoad/OrderImport/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";                                        //图片地址
            string suffix = filetype == 1 ? ".txt" : ".xlsx";
            string filename = TxtHelp.MD5(touuid.ToString() + DateTime.Now.ToUnixTimeStamp() + "321645abcdef") + suffix;                 //图片名称
            string path = Common.CommonManager.FileObj.HttpUploadFile(pathx, filename); //返回完整的上传地址 
            if (!string.IsNullOrEmpty(path)) {
                var dt = GetExcelDatatable(path);
                if (dt.Rows.Count > 0) {
                    DataRow dr = dt.Rows[0];
                    OrderDto dto = new OrderDto();
                    IList<UListDto> list = new List<UListDto>();    //会员列表
                    int pepernum = 0;                               //会员人数
                    foreach (DataRow item in dt.Rows) {
                        UListDto dto_1 = new UListDto();
                        dto_1.id = Convert.ToInt32(item["会员列表"]);
                        list.Add(dto_1);
                        pepernum++;
                    }
                    dto.PeoperNum = pepernum;                           //会员人数
                    dto.PeoperIds = ListToString(list);                 //会员ids
                    if (dr["会费金额"] != null && dr["会费金额"].ToString().IsInt())
                        dto.PeoperMoney = Convert.ToInt32(dr["会费金额"]);  //会费金额
                    if (dr["最低标息"] != null && dr["最低标息"].ToString().IsInt())
                        dto.LowestMoney = Convert.ToInt32(dr["最低标息"]);  //最低标息
                    dto.Remark = dr["备注"].ToString();                 //备注
                    dto.TouUserid = touuid;                             //会头
                    dto.MoneySendType = (MnSdTypeEnum)Convert.ToInt32(StrToInt(dr["放款类型"].ToString()));
                    dto.MeetType = Convert.ToInt32(StrToInt(dr["标会类型"].ToString()));
                    dto.MeetNum = Convert.ToInt32(StrToInt(dr["标会频率"].ToString()));
                    dto.Meetextnum = Convert.ToInt32(StrToInt(dr["加标频率"].ToString()));
                    //if (dto.MeetType == 1 || dto.MeetType == 2) {
                    //    dto.MeetNum = 1;
                    //}
                    dto.Address = dr["标会地址"].ToString();
                    dto.FirstDate = Convert.ToDateTime(dr["首次标会日期时间"]);
                    if (!string.IsNullOrEmpty(dr["首次加标日期时间"].ToString()))
                    {
                        dto.FirstExtraDate = Convert.ToDateTime(dr["首次加标日期时间"].ToString());
                    }
                    int result = CreateOrder(dto);
                    return result;
                }
            }
            return -1;  //失败
        }
        /// <summary>
        /// 将中文转换成数字类型
        /// </summary>
        /// <param name="LName"></param>
        /// <returns></returns>
        protected int StrToInt(string LName) {
            switch (LName)
            {
                case "会款总额+利息":
                    return 1;
                case "会款总额-利息":
                    return 2;
                case "每N月标会一次":
                    return 1;
                case "每N次标会后加标一次":
                    return 3;
                case "1个月":
                    return 1;
                case "2个月":
                    return 2;
                case "3个月":
                    return 3;
                case "4个月":
                    return 4;
                case "5个月":
                    return 5;
                case "6个月":
                    return 6;
                case "没有加标":
                    return 0;
                case "每1个正常标后加标1次":
                    return 1;
                case "每2个正常标后加标1次":
                    return 2;
                case "每3个正常标后加标1次":
                    return 3;
                case "每4个正常标后加标1次":
                    return 4;
                case "每5个正常标后加标1次":
                    return 5;
                default:
                    return -1;
            }
        }
        //2：Excel数据导入Datable
        //@param fileUrl 服务器文件路径
        //@return System.Data.DataTable dt 
        protected System.Data.DataTable GetExcelDatatable(string fileUrl)
        {
            //office2007之前 仅支持.xls
            //const string cmdText = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;IMEX=1';";
            //支持.xls和.xlsx，即包括office2010等版本的   HDR=Yes代表第一行是标题，不是数据；
            const string cmdText = "Provider=Microsoft.Ace.OleDb.12.0;Data Source={0};Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";

            System.Data.DataTable dt = null;
            //建立连接
            OleDbConnection conn = new OleDbConnection(string.Format(cmdText, fileUrl));

            //打开连接
            if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }


            System.Data.DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            //获取Excel的第一个Sheet名称
            string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString().Trim();

            //查询sheet中的数据
            string strSql = "select * from [" + sheetName + "]";
            OleDbDataAdapter da = new OleDbDataAdapter(strSql, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dt = ds.Tables[0];

            return dt;


        }

    }
}
