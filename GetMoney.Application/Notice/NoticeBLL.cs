using GetMoney.Dal.Notice;
using GetMoney.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Application.Notice
{
    public class NoticeBll
    {
        NoticeDal dal = new NoticeDal();
        /// <summary>
        /// 取得最近一条公告
        /// </summary>
        /// <returns></returns>
        public TNoticeDto GetNotice() {
            IList<TNoticeDto> list = DataTableToList.ModelConvertHelper<TNoticeDto>.ConvertToModel(dal.GetNotice());
            if (list.Count > 0) {
                return list[0];
            }
            return null;
        }
    }
}
