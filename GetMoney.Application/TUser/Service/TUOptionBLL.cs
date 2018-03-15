using GetMoney.Dal.TUser.Service;
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
        public int AddTUOption(string title,int type,string content) {
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
    }
}
