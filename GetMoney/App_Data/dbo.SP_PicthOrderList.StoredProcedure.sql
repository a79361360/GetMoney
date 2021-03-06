USE [GetMoney]
GO
/****** Object:  StoredProcedure [dbo].[SP_PicthOrderList]    Script Date: 11/09/2016 01:53:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Qiuqiu
-- Create date: 2016-08-26
-- 会单列表相关操作
-- remark:
--	1:	操作成功
-- -1:	操作失败
-- CZType(操作类型)
-- 1:用户填写每月标金
-- 2:查询每月标金明细
-- 3:更新互助单记录并返回更新结果
-- 4:判断用户是否有权限写标金
-- 5:删除互助单
-- =============================================
--EXEC SP_PicthOrderList @CZType=1,@OrderID='201609172351402996089',@ListID=1,@Userid=10002
CREATE PROCEDURE  [dbo].[SP_PicthOrderList]
	@CZType Int,					--操作类型
	@OrderID varchar(50)=N'',		--互助单号
	@ListID Int=0,					--互助单记录号
	@Userid Int=0,					--明细ID号
	@Money Int=0					--标金金额
AS
BEGIN
	SET NOCOUNT ON;
	declare @curdate datetime	--当月互助单标会日期时间
	IF @CZType = 1
	BEGIN
		--当前互助单记录已结束，已结束的记录不允许再写标金了
		IF EXISTS(SELECT * FROM Order_Lists WHERE OrderNo=@OrderID and id=@ListID and State=1)
		BEGIN
			SELECT -10
			RETURN -10
		END
		--当前互助单记录已超过标会时间,超过标会时间的记录不允许再写标金了
		IF EXISTS(SELECT * FROM Order_Lists WHERE OrderNo=@OrderID AND id=@ListID AND DATEDIFF(second,MeetDate,GETDATE())>0)
		BEGIN
			SELECT -11
			RETURN -11
		END
		--更新当前用户的标金金额
		Update Order_ListUsers set AccrualMoney=@Money,Lastdate=GETDATE() where OrderNo=@OrderID and OrderListID=@ListID and Userid=@Userid
		SELECT 1
		RETURN 1
	END
	IF @CZType = 2
	BEGIN
		--当前互助单记录为已结束
		IF EXISTS(SELECT * FROM Order_Lists WHERE id=@ListID and State=1)
		BEGIN
			SELECT a.id,a.OrderNo,a.OrderListID,a.Userid,b.TrueName,a.AccrualMoney,a.Addtime,Convert(varchar(100),a.Lastdate,21) as Lastdate,a.StayPayNum,a.StayPayTax,a.RealPayNum,Convert(varchar(100),a.PayDate,21) PayDate,a.PayState from Order_ListUsers a left outer join TUsers b on a.Userid=b.id where a.OrderListID=@ListID
		END
	END
	IF @CZType = 3
	BEGIN
		--select a.OrderNo,b.id ListID,CAST(b.MeetDate as varchar(10)) +' '+ Convert(varchar(8),a.FirstDate,8) MeetDTime,b.Userid,b.AccrualMoney 
		--from Orders a inner join Order_Lists b on a.OrderNo=b.OrderNo and b.id=@ListID and 
		--and DATEDIFF(second,CAST(b.MeetDate as varchar(10)) +' '+ Convert(varchar(8),a.FirstDate,8),GETDATE()) > 0 
		--and b.State=2
		--当前互助单号,记录号的互助单记录处于未结束并且已过提交标金的时间
		IF EXISTS(SELECT * FROM Order_Lists WHERE OrderNo=@OrderID AND id=@ListID AND DATEDIFF(second,MeetDate,GETDATE())>0 and State=2)
		BEGIN
			Declare @FirstLID int = 0
			Declare @LastLID int = 0
			select top 1 @FirstLID = id from Order_Lists where OrderNo=@OrderID order by id
			select top 1 @LastLID = id from Order_Lists where OrderNo=@OrderID order by id desc
			INSERT INTO Log(cont)VALUES('FIRST'+CAST(@FirstLID AS VARCHAR(10))+'LAST'+CAST(@LastLID AS VARCHAR(10)))
			--如果是首月(免费给会头)
			--IF EXISTS(select * from Orders a inner join Order_Lists b on a.OrderNo=b.OrderNo and a.OrderNo=@OrderID and b.id=@ListID and Datediff(day,b.MeetDate,a.FirstDate)=0)
			IF @FirstLID = @ListID
			BEGIN
				SELECT @Userid = TouUserid from Orders where OrderNo=@OrderID
				Update Order_Lists set Userid=@Userid,AccrualMoney=0,State=1 where OrderNo=@OrderID and id=@ListID
			END
			--如果是最后月(免费给最后一个会脚)
			--ELSE IF EXISTS(select * from Orders a inner join Order_Lists b on a.OrderNo=b.OrderNo and a.OrderNo=@OrderID and b.id=@ListID and Datediff(day,b.MeetDate,a.FirstDate)=-(a.PeoperNum-1))
			ELSE IF @LastLID = @ListID
			BEGIN
				--先判断是否前面的那些记录都已经结束了
				declare @endnum int = 0
				select @endnum = Count(*) from Order_Lists where OrderNo=@OrderID and State=2
				IF @endnum = 1
				BEGIN
					--取得最后一个未进行过标会的用户ID
					SELECT distinct @Userid = Userid FROM Order_ListUsers WHERE OrderNo=@OrderID and Userid not in(
						select distinct Userid from Order_Lists where OrderNo=@OrderID and State=1	
					)
					Update Order_Lists set Userid=@Userid,AccrualMoney=0,State=1 where OrderNo=@OrderID and id=@ListID
				END
			END
			ELSE
			BEGIN
				declare @TopMoney int = 0
				declare @LowMoney int = 0
				select top 1 @Userid = Userid,@TopMoney=AccrualMoney,@LowMoney=a.LowestMoney from Orders a inner join Order_ListUsers b on a.OrderNo=b.OrderNo where b.OrderNo=@OrderID and b.OrderListID=@ListID order by AccrualMoney desc,Lastdate
				IF @Userid<>0 and @TopMoney<>0 and @TopMoney>@LowMoney
				BEGIN
					BEGIN TRY 
					BEGIN TRAN
						Update Order_Lists set Userid=@Userid,AccrualMoney=@TopMoney,State=1 where OrderNo=@OrderID and id=@ListID
						--中标以后清空当前记录以后当前用户所有填写的标金
						Update Order_ListUsers set AccrualMoney=0 where OrderNo=@OrderID and OrderListID>@ListID and Userid=@Userid
						--更新利息部分金额,待交会费就不仅是会费了，加上利息了
						Exec SP_PicthPayMent @Type=2,@OrderNo=@OrderID,@OrderListID=@ListID,@Userid=@Userid
					COMMIT TRAN 
					END TRY 
					BEGIN CATCH 
						ROLLBACK TRAN 
					END CATCH
				END
				ELSE
				BEGIN
					--订单异常
					Update Order_Lists set State=3 where OrderNo=@OrderID and id=@ListID
				END
			END
		END
		Select Userid,AccrualMoney,State from Order_Lists where OrderNo=@OrderID and id=@ListID
		--select a.OrderNo,b.id ListID,CAST(b.MeetDate as varchar(10)) +' '+ Convert(varchar(8),a.FirstDate,8) MeetDTime,
		--b.Userid,b.AccrualMoney,ROW_NUMBER() over (order by b.id) as 'total'
		--from Orders a inner join Order_Lists b on a.OrderNo=b.OrderNo and and b.id=@ListID
		--and DATEDIFF(second,CAST(b.MeetDate as varchar(10)) +' '+ Convert(varchar(8),a.FirstDate,8),GETDATE()) > 0
		--and b.State=2
	END
	IF @CZType = 4
	BEGIN
		--如果是首次就直接退出，不允许弹窗写标金
		IF EXISTS(select * from Orders a inner join Order_Lists b on a.OrderNo=b.OrderNo and a.OrderNo=@OrderID and b.id=@ListID and Datediff(day,b.MeetDate,a.FirstDate)=0)
		BEGIN
			select -1 result;
			RETURN
		END
		--如果是最后次就直接退出，不允许弹窗弹标金
		IF EXISTS(select * from Orders a inner join Order_Lists b on a.OrderNo=b.OrderNo and a.OrderNo=@OrderID and b.id=@ListID and Datediff(day,b.MeetDate,a.FirstDate)=-(a.PeoperNum-1))
		BEGIN
			select -1 result;
			RETURN
		END
		--当前互助单记录已超过标会时间,超过标会时间的记录不允许再写标金了
		IF EXISTS(SELECT * FROM Order_Lists WHERE OrderNo=@OrderID AND id=@ListID AND DATEDIFF(second,MeetDate,GETDATE())>0)
		BEGIN
			select -1 result;
			RETURN
		END
		declare @allnum int = 0		--共有几次
		declare @usednum int = 0	--已使用几次
		--当前用户有几次标会资格
		select @allnum = count(*) from Order_ListUsers where OrderNo=@OrderID and OrderListID=@ListID and Userid=@Userid
		--已经成功夺标次数
		SELECT @usednum = COUNT(*) from Order_Lists where Userid=@Userid and state=1
		if @usednum < @allnum
			select 1 result;
		else
			select -1 result;
	END
	IF @CZType = 5
	BEGIN
		IF EXISTS( select * from Orders where OrderNo=@OrderID)
		begin
			BEGIN TRY
			--开启事物，保持一致性
			BEGIN TRAN DelOrder
				Delete Orders where OrderNo=@OrderID
				Delete Order_Lists where OrderNo=@OrderID
				Delete Order_ListUsers where OrderNo=@OrderID
			--提交事物
			COMMIT TRAN DelOrder
			select 1 result;
			END TRY
			BEGIN CATCH
			IF @@TRANCOUNT > 0
				print error_message()
				ROLLBACK TRAN
				select -1 result;
			END CATCH
		end
	END
END

--select a.ID,a.OrderNo,convert(varchar(10),a.MeetDate) MeetDate,a.Userid,b.TrueName,a.AccrualMoney,a.State 
--from Order_Lists a left outer join TUsers b on a.Userid=b.id where a.OrderNo=201609172351402996089
GO
