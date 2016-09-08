using GetMoney.Dal;
using GetMoney.Data.OnlyNameTest;
using GetMoney.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Application.OnlyNameTest
{
    public class OnlyNameTestBll:IOnlyNameTestBll
    {
        IOnlyNameTestRepository _repostory;
        ITUserDal _dal;
        public OnlyNameTestBll(IOnlyNameTestRepository repostory, ITUserDal dal)
        {
            _repostory = repostory;
            _dal = dal;
        }
        public void Add(OnlyNameTestDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException();
            var _unitOfWork = _repostory.UnitOfWork;
            var _factory = OnlyNameTestFactory.CreateOnly(
                dto.Name,
                dto.InputDate
                );
            _repostory.Add(_factory);
            _unitOfWork.Commit();
        }
    }
}
