using GetMoney.Dal;
using GetMoney.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Application.TUser.Service
{
    public class TUOptionBLL
    {
        TUOptionDAL dal = new TUOptionDAL();
        public int AddTUOption(int userid, string title, int type, string content)
        {
            TUOptionDTO dto = new TUOptionDTO();
            dto.title = title;dto.type = type;dto.content = content;
            int result = dal.SetTUOption(dto);
            return result;
        }
        public TUOptionDTO FindTOptionById(int id) {
            IList<TUOptionDTO> list = DataTableToList.ModelConvertHelper<TUOptionDTO>.ConvertToModel(dal.FindTOptionById(id));
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }
        public IList<TUOptionDTO> ListTUOptionPage(ref int Total, int pageSize, int pageIndex, string filter)
        {
            SqlPageParam param = new SqlPageParam();
            param.TableName = "TUOption";
            param.PrimaryKey = "id";
            param.Fields = "[id],[userid],[title],[type],[content],[rcontent],Convert(varchar(19),[addtime],120) addtime";
            param.PageSize = pageSize;
            param.PageIndex = pageIndex;
            param.Filter = filter;
            param.Group = "";
            param.Order = "id";
            IList<TUOptionDTO> list = DataTableToList.ModelConvertHelper<TUOptionDTO>.ConvertToModel(dal.ListPage(ref Total, param));
            return list;
        }
    }
}
