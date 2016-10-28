using GetMoney.Data.Order;
using GetMoney.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GetMoney.Framework.Common;
using GetMoney.Dal;

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
            param.TableName = "Orders";
            param.PrimaryKey = "id";
            param.Fields = "id,OrderNo,PeoperNum,PeoperMoney,InputDate,Remark,MoneySendType,MeetType,MeetNum,LowestMoney,FirstExtraDate,ExtraDate,State";
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
            _dal.CreateOrder(dto.OrderNo, dto.PeoperNum, dto.PeoperIds, dto.PeoperMoney, dto.LowestMoney, dto.TouUserid, (int)dto.MoneySendType, dto.MeetType, dto.MeetNum, dto.FirstDate, dto.FirstExtraDate, dto.ExtraDate,dto.Address, dto.Remark, out dic);
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
    }
}
