USE [GetMoney]
GO
/****** Object:  StoredProcedure [dbo].[SP_AddNewUser]    Script Date: 10/09/2016 09:51:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Qiuqiu
-- Create date: 2016-08-26
-- 添加新用户
-- remark:
--	1:	操作成功
-- -1:	操作失败
-- =============================================
CREATE PROCEDURE  [dbo].[SP_AddNewUser]
	@UserName NVARCHAR(20),							--玩家名
	@Password NVARCHAR(50),							--密码，应为MD5后的值
	@BankPwd NVARCHAR(50),							--银行密码，应为MD5后的值
	@NickName NVARCHAR(20)= N'',					--昵称
	@TrueName NVARCHAR(20)= N'',					--真实姓名
	@IdentityNum NVARCHAR(20) = N'',				--身份证件号码
	@Phone NVARCHAR(12) = N'',						--电话
	@RegIP NVARCHAR(16) = N'',						--注册IP
	@TxUrl NVARCHAR(100) = N'',						--头像ID，默认值为第一张
	@State int = 2,									--玩家状态(1为正常,2为临时,3为禁用)
	@Userid INT = 0 OUTPUT							--用户ID
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRY
	--检查玩家账号是否已存在
	IF EXISTS(SELECT * FROM TUsers WHERE UserName = @UserName)
		RETURN 9	
	--检查玩家手机是否已存在
	IF @Phone <> ''
		IF EXISTS(SELECT * FROM TUsers WHERE Phone = @Phone)
			return 6
	--完善信息用为玩家为状态为正常
	IF @NickName <> '' and @TrueName <> '' and @IdentityNum <> '' and @Phone <> '' and @RegIP <> '' and @TxUrl <> ''
		Set @State=1
	--开启事物，保持一致性
	BEGIN TRAN
	--插入用户
	INSERT INTO [TUsers]([UserName],[UserPwd],[BankPwd],[NickName],[UserJb],[TrueName],[IdentityNum],[Phone],[RegIP],[TxUrl],[State],[Addtime])
	VALUES (@UserName,@Password,@BankPwd,@NickName,0,@TrueName,@IdentityNum,@Phone,@RegIP,@TxUrl,@State,GETDATE())
	SELECT @Userid=MAX(id) from TUsers
	--提交事物
	COMMIT TRAN
	RETURN 1
	END TRY
	BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN
	RETURN 10000--未知服务器错误
	END CATCH
END
GO
