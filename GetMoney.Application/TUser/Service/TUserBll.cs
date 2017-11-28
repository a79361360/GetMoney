using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GetMoney.Model;
using GetMoney.Data.TUser;
using GetMoney.Dal;
using GetMoney.Framework.Common;
using GetMoney.Common.Expand;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

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
        /// <summary>
        /// 使用EF来进行用户信息添加,现在基本没有使用这个方法来进行注册用户
        /// </summary>
        /// <param name="dto"></param>
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

        public bool EditTUser(TUserDto dto) {
            bool result = _dal.EditTUser(dto.id, dto.UserName, dto.NickName, dto.TrueName, dto.IdentityNum, dto.Phone, dto.TxUrl);
            return result;
        }
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="dto">用户信息对象</param>
        /// <param name="list">返回结果</param>
        public void RegTUser(TUserDto dto, out Dictionary<string, object> list)
        {
            _dal.AddTUserByProce(dto.UserName, dto.UserPwd, dto.BankPwd, dto.NickName, dto.TrueName, dto.IdentityNum, dto.Phone, dto.RegIP, dto.TxUrl, out list);
        }
        /// <summary>
        /// 翻页查询用户信息
        /// </summary>
        /// <param name="Total">总条数</param>
        /// <param name="pageSize">一页大小</param>
        /// <param name="pageIndex">当前页索引,首页为0</param>
        /// <param name="filter">条件</param>
        /// <returns></returns>
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
        /// <param name="pageIndex">当前页索引,首页为0</param>
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
        public TUserDto FindUserById(int id) {
            IList<TUserDto> list = DataTableToList.ModelConvertHelper<TUserDto>.ConvertToModel(_dal.FindUserById(id));
            if (list.Count > 0) {
                return list[0];
            }
            return null;
        }
        /// <summary>
        /// 单个移除用户信息(用户ID)
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
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
        /// <summary>
        /// 批量移出用户信息(用户ID列表)
        /// </summary>
        /// <param name="ids">用户ID列表</param>
        /// <returns></returns>
        public bool RemoveTUsers(IList<UListDto> ids)
        {
            bool result = false;
            if (ids.Count == 0)
            {
                throw new ArgumentNullException();
            }
            else
            {
                try
                {
                    foreach (UListDto item in ids)
                    {
                        int gid = item.id.ToInt32();        //用户ID
                        var _dto = _repostory.Get(gid);     //取得用户对象
                        _repostory.Remove(_dto);            //移除
                        _repostory.UnitOfWork.Commit();     //提交
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
        /// <summary>
        /// 添加好友,(用户ID,好友id列表)
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="list">好友列表对象</param>
        /// <returns>完成好友添加数量</returns>
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
        /// 通过账号和密码判断用户ID是否存在,存在返回用户ID,失败返回-1
        /// </summary>
        /// <param name="dto">用户对象</param>
        /// <returns>成功返回用户ID,失败返回-1</returns>
        public int VerifyTUsers(TUserDto dto) {
            int result = _dal.VerifyUserByUnamePwd(dto.UserName, dto.UserPwd);
            return result;
        }
        public string UpdateFileTx(string host, string userid)
        {
            string key = "ddzasdklfwne2394i23jovnfoirehnoi23";
            string pathx = "/DownLoad/File/Tx/";                                        //图片地址
            string filename = (userid.ToString() + key).MD5() + ".jpg";                 //图片名称 
            string vtime = "?v=" + DateTime.Now.ToUnixTimeStamp().ToString();           //用时间戳来做版本号
            string path = Common.CommonManager.FileObj.HttpUploadFile(pathx, filename); //返回完整的上传地址 
            if (!string.IsNullOrEmpty(path)) { 
                Dictionary<string, object> dic;
                _dal.UpdateUserTx(userid.ToInt32(), host + pathx + filename + vtime, out dic);
                if (Convert.ToInt32(dic["@ReturnValue"]) == 1)
                {
                    return host + pathx + filename + vtime;
                }
            }
            return "";
        }
        public string UpdateFileTx1(string host, string userid,int x,int y,int w,int h,Stream Img)
        {
            string key = "ddzasdklfwne2394i23jovnfoirehnoi23";
            string pathx = "/DownLoad/File/Tx/";                                        //图片地址
            string filename = (userid.ToString() + key).MD5() + ".jpg";                 //图片名称 
            string vtime = "?v=" + DateTime.Now.ToUnixTimeStamp().ToString();           //用时间戳来做版本号
            //byte[] pbyte = Crop(Img, w, h, x, y);
            byte[] pbyte = CropImage(Img, w, h, x, y);
            string path = Common.CommonManager.FileObj.UploadFileByByte(pathx+ filename, pbyte); //返回完整的上传地址
            if (!string.IsNullOrEmpty(path))
            {
                Dictionary<string, object> dic;
                _dal.UpdateUserTx(userid.ToInt32(), pathx + filename + vtime, out dic);
                if (Convert.ToInt32(dic["@ReturnValue"]) == 1)
                {
                    return host + pathx + filename + vtime;   //完整路径
                    //return pathx + filename + vtime;     //虚拟路径
                }
            }
            return "";
        }
        /// <summary>
        /// 剪裁图像
        /// </summary>
        /// <param name="Img"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public byte[] Crop(Stream Img, int Width, int Height, int X, int Y)
        {
            try
            {
                using (var OriginalImage = new Bitmap(Img))
                {
                    using (var bmp = new Bitmap(Width, Height, OriginalImage.PixelFormat))
                    {
                        bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);
                        using (Graphics Graphic = Graphics.FromImage(bmp))
                        {
                            Graphic.SmoothingMode = SmoothingMode.AntiAlias;
                            Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            Graphic.DrawImage(OriginalImage, new Rectangle(0, 0, Width, Height), X, Y, Width, Height,
                                              GraphicsUnit.Pixel);
                            var ms = new MemoryStream();
                            bmp.Save(ms, OriginalImage.RawFormat);
                            return ms.GetBuffer();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw (Ex);
            }
        }

        public byte[] CropImage(Stream content, int cropWidth, int cropHeight, int x, int y)
        {
            using (Bitmap sourceBitmap = new Bitmap(content))
            {

                // 将选择好的图片缩放
                //Bitmap bitSource = new Bitmap(sourceBitmap, sourceBitmap.Width, sourceBitmap.Height);
                Bitmap bitSource = new Bitmap(sourceBitmap, 602, 400);

                Rectangle cropRect = new Rectangle(x, y, cropWidth, cropHeight);

                using (Bitmap newBitMap = new Bitmap(cropWidth, cropHeight))
                {
                    newBitMap.SetResolution(sourceBitmap.HorizontalResolution, sourceBitmap.VerticalResolution);
                    using (Graphics g = Graphics.FromImage(newBitMap))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.CompositingQuality = CompositingQuality.HighQuality;

                        g.DrawImage(bitSource, new Rectangle(0, 0, newBitMap.Width, newBitMap.Height), cropRect, GraphicsUnit.Pixel);
                        var ms = new MemoryStream();
                        newBitMap.Save(ms, sourceBitmap.RawFormat);
                        return ms.GetBuffer();
                        //return GetBitmapBytes(newBitMap);
                    }
                }
            }
        }
    }
}
