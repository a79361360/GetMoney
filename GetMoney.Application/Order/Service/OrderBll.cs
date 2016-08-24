using GetMoney.Data.Order;
using GetMoney.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GetMoney.Framework.Common;
using GetMoney.Dal;

namespace GetMoney.Application.Order
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
                dto.MoneySendType,
                dto.MeetType,
                dto.MeetNum,
                dto.MeetDate,
                dto.MeetTime,
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
            param.Fields = "id,OrderNo,PeoperNum,PeoperMoney,InputDate,Remark,MoneySendType,MeetType,MeetNum,MeetDate,MeetTime,State";
            param.PageSize = pageSize;
            param.PageIndex = pageIndex;
            param.Filter = "";
            param.Group = "";
            param.Order = "id";
            IList<OrderDto> list = DataTableToList.ModelConvertHelper<OrderDto>.ConvertToModel(_dal.ListOrderPage(ref Total, param));
            return list;
        }
    }
}
