USE [GetMoney]
GO
/****** Object:  StoredProcedure [dbo].[SP_FriendUser]    Script Date: 11/09/2016 02:00:43 ******/
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
CREATE PROCEDURE  [dbo].[SP_FriendUser]
	@Userid int,							--当前登入用户ID
	@Pcid int,								--用户ID
	@Type int								--1为添加好友,2为添加到黑名单,3为删除好友
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRY
		IF @Type = 1
		BEGIN
			--检查当前用户的此好友是否已经存在
			IF EXISTS(SELECT * FROM TUserFriends WHERE Userid = @Userid and Pcid = @Pcid)
				RETURN 9
			--检查Pcid的好友Userid是否已经存在
			IF EXISTS(SELECT * FROM TUserFriends WHERE Userid = @Pcid and Pcid = @Userid)
				RETURN 10
			--开启事物，保持一致性
			BEGIN TRAN
				--添加好友
				INSERT INTO [TUserFriends]([Userid],[Pcid],[Addtime],[State])
				VALUES(@Userid,@Pcid,GETDATE(),1)
				if(@Userid <> @Pcid)
				BEGIN
					--对方添加好友
					INSERT INTO [TUserFriends]([Userid],[Pcid],[Addtime],[State])
					VALUES(@Pcid,@Userid,GETDATE(),1)
				END
			--提交事物
			COMMIT TRAN
			RETURN 1
		END
		IF @Type = 2
		BEGIN
			UPDATE [TUserFriends] SET [State]=2 WHERE [Userid]=@Userid and [Pcid]=@Pcid
			RETURN 1
		END
		IF @Type = 3
		BEGIN
			DELETE [TUserFriends] WHERE [Userid]=@Userid and [Pcid]=@Pcid
			RETURN 1
		END
	END TRY
	BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN
	RETURN 10000--未知服务器错误
	END CATCH	
END
GO
