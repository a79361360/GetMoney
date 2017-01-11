using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GetMoney.Model;
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
                dto.UserPwd,
                dto.BankPwd,
                dto.NickName,
                dto.UserJb,
                dto.TrueName,
                dto.IdentityNum,
                dto.Phone,
                dto.RegIP,
                dto.TxUrl,
                dto.State,
                dto.Addtime
                );
            _repostory.Add(_build);
            _unitOfWork.Commit();
        }
        public void RegTUser(TUserDto dto, out Dictionary<string, object> list)
        {
            _dal.AddTUserByProce(dto.UserName, dto.UserPwd, dto.BankPwd, dto.NickName, dto.TrueName, dto.IdentityNum, dto.Phone, dto.RegIP, dto.TxUrl, out list);
        }

        public IList<TUserDto> ListTUserPage(ref int Total, int pageSize, int pageIndex)
        {
            SqlPageParam param = new SqlPageParam();
            param.TableName = "TUsers";
            param.PrimaryKey = "Userid";
            param.Fields = "id,UserName,NickName,UserJb,IdentityNum,Phone,TxUrl,State,Addtime";
            param.PageSize = pageSize;
            param.PageIndex = pageIndex;
            param.Filter = "";
            param.Group = "";
            param.Order = "Userid";
            IList<TUserDto> list = DataTableToList.ModelConvertHelper<TUserDto>.ConvertToModel(_dal.ListUserPage(ref Total, param));
            return list;
        }
        public IList<TUserDto> ListTUserPage(ref int Total, int pageSize, int pageIndex,string filter)
        {
            SqlPageParam param = new SqlPageParam();
            param.TableName = "TUsers";
            param.PrimaryKey = "id";
            param.Fields = "id,UserName,NickName,TrueName,UserJb,IdentityNum,Phone,TxUrl,State,Addtime";
            param.PageSize = pageSize;
            param.PageIndex = pageIndex;
            param.Filter = filter;
            param.Group = "";
            param.Order = "id";
            IList<TUserDto> list = DataTableToList.ModelConvertHelper<TUserDto>.ConvertToModel(_dal.ListUserPage(ref Total, param));
            return list;
        }
        /// <summary>
        /// 翻页查询当前用户好友
        /// </summary>
        /// <param name="Total">总条数</param>
        /// <param name="pageSize">一页大小</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        public IList<FriendDto> ListFriendPage(ref int Total, int pageSize, int pageIndex, string filter)
        {
            SqlPageParam param = new SqlPageParam();
            param.TableName = "TUserFriends inner join TUsers on TUserFriends.Pcid = TUsers.id";
            param.PrimaryKey = "TUserFriends.id";
            param.Fields = "TUserFriends.Userid,TUserFriends.Pcid,TUsers.UserName,TUsers.NickName,TrueName,TUsers.Phone";
            param.PageSize = pageSize;
            param.PageIndex = pageIndex;
            param.Filter = filter;
            param.Group = "";
            param.Order = "TUserFriends.id";
            IList<FriendDto> list = DataTableToList.ModelConvertHelper<FriendDto>.ConvertToModel(_dal.ListUserPage(ref Total, param));
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

        public int AddTUserFriend(int userid, IList<UListDto> list)
        {
            Dictionary<string, object> dic;
            int result = 0;
            foreach (UListDto item in list)
            {
                dic = new Dictionary<string, object>();
                _dal.MakeTUserFriend(userid, item.id, 1,out dic);
                if (Convert.ToInt32(dic["@ReturnValue"]) == 1) {
                    result++;
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int VerifyTUsers(TUserDto dto) {
            int result = _dal.VerifyUserByUnamePwd(dto.UserName, dto.UserPwd);
            return result;
        }
    }
}
