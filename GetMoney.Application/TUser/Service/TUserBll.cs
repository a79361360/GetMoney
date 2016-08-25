using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GetMoney.Model.Model;
using GetMoney.Data.TUser;
using GetMoney.Dal;
using GetMoney.Framework.Common;

namespace GetMoney.Application
{
    public class TUserBll : ITUserBll
    {
        ITUserRepository _repostory;
        ITUserDal _dal;
        public TUserBll(ITUserRepository repostory, ITUserDal dal)
        {
            _repostory = repostory;
            _dal = dal;
        }
        public void AddTUser(TUserDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException();
            var _unitOfWork = _repostory.UnitOfWork;
            var _build = TUserFactory.Create(
                dto.UserName,
                dto.NickName,
                dto.UserJb,
                dto.IdentityNum,
                dto.Phone,
                dto.TxUrl,
                dto.State,
                dto.Addtime
                );
            _repostory.Add(_build);
            _unitOfWork.Commit();
        }

        public IList<TUserDto> ListTUserPage(ref int Total, int pageSize, int pageIndex)
        {
            SqlPageParam param = new SqlPageParam();
            param.TableName = "TUsers";
            param.PrimaryKey = "Userid";
            param.Fields = "Userid,UserName,NickName,UserJb,IdentityNum,Phone,TxUrl,State,Addtime";
            param.PageSize = pageSize;
            param.PageIndex = pageIndex;
            param.Filter = "";
            param.Group = "";
            param.Order = "Userid";
            IList<TUserDto> list = DataTableToList.ModelConvertHelper<TUserDto>.ConvertToModel(_dal.ListUserPage(ref Total, param));
            return list;
        }

        public bool RemoveTUser(string id)
        {
            bool result = false;
            try
            {
                int gid = id.ToInt32();
                var _dto = _repostory.Get(gid);
                _repostory.Remove(_dto);
                _repostory.UnitOfWork.Commit();
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public bool RemoveTUsers(string[] ids)
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
    }
}
