USE [GetMoney]
GO
/****** Object:  StoredProcedure [dbo].[SP_PicthPayMent]    Script Date: 11/07/2016 18:09:20 ******/
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
CREATE PROCEDURE  [dbo].[SP_PicthPayMent]
	@Type int,							--1为添加待付款记录2为中标后更新标息部分金额3为填写缴费金额并修改状态
	@OrderNo nvarchar(50),				--会单号
	@OrderListID int = 0,				--会单记录列表ID
	@Userid int = 0,					--会员ID号
	@Money int = 0						--缴费金额
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRY
	--检查会单号是否已存在
	IF Not EXISTS(SELECT * FROM Orders WHERE OrderNo = @OrderNo)
		RETURN 9	
	--会费金额
	declare @peomoney int
	declare @AccrualMoney int = 0
	select @peomoney = PeoperMoney from Orders WHERE OrderNo = @OrderNo
	--开启事物，保持一致性
	BEGIN TRAN
		--Order_PayMent合并到Order_ListUsers表
		--IF @Type = 1
		--BEGIN
		--	Insert into Order_PayMent([OrderNo],[OrderListID],[UserID],[StayPayNum],[StayPayTax],[RealPayNum],[PayDate],[AddDate],[State])
		--	Select a.OrderNo,a.OrderListID,a.Userid,@peomoney,0,0,b.MeetDate,getdate(),0 from Order_ListUsers a inner join Order_Lists b on a.OrderListID=b.ID and a.OrderNo = @OrderNo
		--END
		IF @Type = 2
		BEGIN
			Select @AccrualMoney = Sum(AccrualMoney) from Order_Lists Where OrderNo=@OrderNo and Userid=@Userid and State='1'
			--中标以后清空当前记录以后当前用户所有填写的标金
			Update Order_ListUsers set StayPayTax=@AccrualMoney where OrderNo=@OrderNo and OrderListID>@OrderListID and Userid=@Userid
		END
		IF @Type = 3
		BEGIN
			declare @State int = 0
			--待缴金额
			Select @AccrualMoney = StayPayNum+StayPayTax from Order_ListUsers Where OrderNo=@OrderNo and OrderListID>@OrderListID and Userid=@Userid
			--缴费状态
			IF @Money >= @AccrualMoney
				SET @State = 1
			IF @AccrualMoney > @Money AND @Money > 0
				SET @State = 2
			Update Order_ListUsers set RealPayNum=@Money,PayState=@State where OrderNo=@OrderNo and OrderListID>@OrderListID and Userid=@Userid
		END
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
