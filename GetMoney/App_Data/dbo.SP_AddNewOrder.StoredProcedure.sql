USE [GetMoney]
GO
/****** Object:  StoredProcedure [dbo].[SP_AddNewOrder]    Script Date: 11/09/2016 01:53:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Qiuqiu
-- Create date: 2016-08-26
-- 添加新会单
-- remark:
--	1:	操作成功
-- -1:	操作失败
--SP_AddNewOrder '333333',20,'10234,10235,10236,10237,10238,10239,10240,10241,10242,10243,10244,10245,10246,10247,10248,10249,10250,10251,10252,10253',2000,35,10234,1,
--4,3,'2016-09-05 19:00:00','2016-09-25 12:00:00',
--'2016-09-25 12:00:00|2016-10-24 12:00:00|2016-11-06 11:00:00|2016-09-25 12:00:00','20','19:00',1,'备注1'
-- =============================================
CREATE PROCEDURE  [dbo].[SP_AddNewOrder]
	@OrderNo NVARCHAR(50),					--会单号
	@PeoperNum int,							--互助会用户数量
	@Peoper NVARCHAR(500),					--互助会用户ID10001,10002,10003
	@PeoperMoney int,						--标准会费金额
	@LowestMoney int,						--最低标会金额
	@TouUserid int,							--会头用户ID
	@MoneySendType int = 1,					--1全额发放2减掉利息后发放
	@MeetType int = 1,						--标会类型(1约定标会日期,2每月都加标,3隔N月加标,4自定义日期加标)
	@MeetNum int = 1,						--每N月标会次数默认为1(仅@MeetType为3使用,当作N)
	@MeetExtNum int = 1,					--每个月标会次数默认为1(仅@MeetType为3使用,当作N)
	@FirstDate datetime,					--首次标会日期
	@FirstExtraDate datetime=null,			--首次加标标会日期(@MeetType为2,3使用)
	@ExtraDate NVARCHAR(1000)=null,				--加标日期列表(仅@MeetType为4使用)
	@Address NVARCHAR(250)=null,			--标会地址
	@State int = 1,							--互助会状态(1为正常,2为死会,3为险会)
	@Remark NVARCHAR(1000) = N''			--
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRY
	--检查会单是否已存在
	IF EXISTS(SELECT * FROM Orders WHERE OrderNo = @OrderNo)
		RETURN 10	
	--会款发放方式不正确
	IF @MoneySendType <> 1 and @MoneySendType <> 2
		RETURN 11
	--标会类型不正确
	IF @MeetType not in(1,2,3,4)
		RETURN 12
	--每月标会次数不正确
	IF @MeetNum < 1
		RETURN 13
	--备注长度
	IF LEN(@Remark) > 100 and LEN(@ExtraDate) > 1000
		RETURN 14
	--为加标类型时，首次加标时间不能小于首次标会时间
	IF (@MeetType=2 or @MeetType=3) AND @FirstExtraDate IS NOT NULL
	BEGIN
		IF @FirstExtraDate < @FirstDate
			RETURN 15
	END
	IF @MeetType=1
	BEGIN
		SET @FirstExtraDate = GETDATE()
	END
	--开启事物，保持一致性
	BEGIN TRAN
	--插入互助会单主表
	INSERT INTO [Orders]([OrderNo],[PeoperNum],[PeoperMoney],[LowestMoney],[TouUserid],[MoneySendType],[MeetType],[MeetNum],[MeetExtNum],[FirstExtraDate],[ExtraDate],[InputDate],[Address],[State],[Remark],[FirstDate])
    VALUES(@OrderNo,@PeoperNum,@PeoperMoney,@LowestMoney,@TouUserid,@MoneySendType,@MeetType,@MeetNum,@MeetExtNum,@FirstExtraDate,@ExtraDate,GETDATE(),@Address,@State,@Remark,@FirstDate)
	--根据会员数量生成同等数量的会单列表
	declare @index int = @PeoperNum	--用户个数
	select * into #tusers from SplitString(@Peoper,',',1)	--用户列表
	declare @userid int	--定义用户ID变量
	declare @listid int	--定义会单列表ID变量
	declare @yuedate datetime	--每月标会时间
	declare @i int = 0	--与用户个数对应的循环次数
	
	while @index > 0
	begin
		--是否自定义加标
		--if @MeetType = 4 and @ExtraDate is not null and @i = 0
		--begin
		--	--自定义加标时间写入临时表
		--	select * into #extrad from SplitString(@ExtraDate,'|',1)
		--	--自定义时间多少个
		--	declare @zdycount int = 0
		--	select @zdycount = count(*) from #extrad
		--	--数量直接超过用户数(等于也不行,最起码得有一期是正常期,会头)
		--	if @zdycount >= @PeoperNum
		--	begin
		--		RAISERROR('自定义加标时间过多',16,1)
		--	end
		--	--插入会单列表
		--	INSERT INTO [Order_Lists]([OrderNo],[Userid],[Addtime],[State],[AccrualMoney],[MeetDate])
		--	SELECT @OrderNo,0,GETDATE(),2,0,Value from #extrad
		--	--VALUES(@OrderNo,0,GETDATE(),2,0,@yuedate)
		--	--最大的会单列表ID
		--	select @listid = Max(id) from Order_Lists
		--	--多少个用户
		--	declare @znextid int = @PeoperNum
		--	declare @zuidex int = 1
		--	--开始循环
		--	while @znextid>0
		--	begin
		--		select @userid = Value from #tusers where Id = @znextid
		--		INSERT INTO [Order_ListUsers]([OrderNo],[OrderListID],[Userid],[AccrualMoney],[Addtime],[Lastdate])
		--		VALUES(@OrderNo,@listid,@userid,0,GETDATE(),GETDATE())
		--		set @znextid = @znextid - 1
		--		set @zuidex = @zuidex + 1
		--	end
		--	set @index = @PeoperNum - @zdycount
		--end
		set @yuedate = dateadd(month,@i*@MeetNum,@FirstDate)
		--插入会单列表
		INSERT INTO [Order_Lists]([OrderNo],[Userid],[Addtime],[State],[AccrualMoney],[MeetDate])
		VALUES(@OrderNo,0,GETDATE(),2,0,@yuedate)
		--最大的会单列表ID
		select @listid = Max(id) from Order_Lists
		declare @nextid int = @PeoperNum
		declare @uidex int = 1
		while @nextid>0
		begin
			select @userid = Value from #tusers where Id = @uidex
			INSERT INTO [Order_ListUsers]([OrderNo],[OrderListID],[Userid],[AccrualMoney],[Addtime],[Lastdate],[StayPayNum])
			VALUES(@OrderNo,@listid,@userid,0,GETDATE(),GETDATE(),@PeoperMoney)
			set @nextid = @nextid - 1
			set @uidex = @uidex + 1
		end
		set @index = @index - 1
		--是否每月都加标
		if @MeetType = 2 and @FirstExtraDate is not null
		begin
			set @yuedate = dateadd(month,@i,@FirstExtraDate)
			--插入会单列表
			INSERT INTO [Order_Lists]([OrderNo],[Userid],[Addtime],[State],[AccrualMoney],[MeetDate])
			VALUES(@OrderNo,0,GETDATE(),2,0,@yuedate)
			--最大的会单列表ID
			select @listid = Max(id) from Order_Lists
			set @nextid = @PeoperNum
			set @uidex = 1
			while @nextid>0
			begin
				select @userid = Value from #tusers where Id = @uidex
				INSERT INTO [Order_ListUsers]([OrderNo],[OrderListID],[Userid],[AccrualMoney],[Addtime],[Lastdate],[StayPayNum])
				VALUES(@OrderNo,@listid,@userid,0,GETDATE(),GETDATE(),@PeoperMoney)
				set @nextid = @nextid - 1
				set @uidex = @uidex + 1
			end
			--如果是加标需要将时间减1
			set @index = @index - 1
		end
		--是否每隔N月都加标
		insert into log(cont)values(cast(@i % @MeetExtNum as varchar(20)))
		if @MeetType = 3 and @FirstExtraDate is not null and @i % @MeetExtNum = 0 and @index > 0
		begin
			set @yuedate = dateadd(month,(@i*@MeetNum/@MeetExtNum)*@MeetExtNum,@FirstExtraDate)
			insert into log(cont)values('比较是多少'+Convert(varchar(20),@yuedate,120)+''+cast(@i % @MeetExtNum as varchar(20))+''+CAST(@index as varchar(20)))
			--插入会单列表
			INSERT INTO [Order_Lists]([OrderNo],[Userid],[Addtime],[State],[AccrualMoney],[MeetDate])
			VALUES(@OrderNo,0,GETDATE(),2,0,@yuedate)
			--最大的会单列表ID
			select @listid = Max(id) from Order_Lists
			set @nextid = @PeoperNum
			set @uidex = 1
			while @nextid>0
			begin
				select @userid = Value from #tusers where Id = @uidex
				INSERT INTO [Order_ListUsers]([OrderNo],[OrderListID],[Userid],[AccrualMoney],[Addtime],[Lastdate],[StayPayNum])
				VALUES(@OrderNo,@listid,@userid,0,GETDATE(),GETDATE(),@PeoperMoney)
				set @nextid = @nextid - 1
				set @uidex = @uidex + 1
			end
			--如果是加标需要将时间减1
			set @index = @index - 1
		end
		set @i = @i + 1
	end
	drop table #tusers
	--添加缴费记录
	Exec SP_PicthPayMent @Type=1,@OrderNo=@OrderNo
	--提交事物
	COMMIT TRAN
	RETURN 1
	END TRY
	BEGIN CATCH
	IF @@TRANCOUNT > 0
		print error_message()
		ROLLBACK TRAN
	RETURN 10000--未知服务器错误
	END CATCH
END
GO
