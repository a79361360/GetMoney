USE [master]
GO
/****** Object:  Database [GetMoney]    Script Date: 2016/9/17 18:30:22 ******/
CREATE DATABASE [GetMoney]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'GetMoney', FILENAME = N'E:\VS2012\C#\DataBase\GetMoney.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'GetMoney_log', FILENAME = N'E:\VS2012\C#\DataBase\GetMoney_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [GetMoney] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GetMoney].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [GetMoney] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GetMoney] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GetMoney] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GetMoney] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GetMoney] SET ARITHABORT OFF 
GO
ALTER DATABASE [GetMoney] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [GetMoney] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [GetMoney] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GetMoney] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GetMoney] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GetMoney] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GetMoney] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GetMoney] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GetMoney] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GetMoney] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GetMoney] SET  DISABLE_BROKER 
GO
ALTER DATABASE [GetMoney] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [GetMoney] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GetMoney] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GetMoney] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GetMoney] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GetMoney] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [GetMoney] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [GetMoney] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [GetMoney] SET  MULTI_USER 
GO
ALTER DATABASE [GetMoney] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [GetMoney] SET DB_CHAINING OFF 
GO
ALTER DATABASE [GetMoney] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [GetMoney] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [GetMoney]
GO
/****** Object:  StoredProcedure [dbo].[SP_AddNewOrder]    Script Date: 2016/9/17 18:30:22 ******/
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
--SP_AddNewOrder '111111',20,'10002,10003,10004,10005,10006,10007,10008,10009,10010,10011,10012,10013,10014,10015,10016,10017,10018,10019,10020,10021',2000,1,1,1,'2016-09-20','20','19:00',1,'备注1'
-- =============================================
CREATE PROCEDURE  [dbo].[SP_AddNewOrder]
	@OrderNo NVARCHAR(50),					--会单号
	@PeoperNum int,							--互助会用户数量
	@Peoper NVARCHAR(500),					--互助会用户ID10001,10002,10003
	@PeoperMoney int,						--标准会费金额
	@MoneySendType int = 1,					--1全额发放2减掉利息后发放
	@MeetType int = 1,						--标会类型(1约定标会日期,2间隔30天标会)
	@MeetNum int = 1,						--每个月标会次数默认为1
	@FirstDate datetime,					--首次标会日期
	@MeetDate NVARCHAR(14) = N'',			--每个月标会日期(以字符串形式奖5个标会日期分开,以逗号隔开Len(14)例如:01,10,15)
	@MeetTime NVARCHAR(30) = N'',			--标会时间(具体的时间:例如晚上7点就是:    19:00)，只时间部分有用
	@State int = 1,							--互助会状态(1为正常,2为死会,3为险会)
	@Remark NVARCHAR(100) = N''				--
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
	IF @MeetType <> 1 and @MeetType <> 2
		RETURN 12
	--每月标会次数不正确
	IF @MeetNum < 1 or @MeetNum > 5
		RETURN 13
	--备注长度
	IF LEN(@Remark) > 100
		RETURN 14
	IF @MeetType=1
	BEGIN
		SET @MeetDate =  DATENAME(DD,@FirstDate)
		SET @MeetTime =  CONVERT(varchar(8), @FirstDate, 108)
	END
	--开启事物，保持一致性
	BEGIN TRAN
	--插入互助会单主表
	INSERT INTO [Orders]([OrderNo],[PeoperNum],[PeoperMoney],[MoneySendType],[MeetType],[MeetNum],[MeetDate],[MeetTime],[InputDate],[State],[Remark],[FirstDate])
    VALUES(@OrderNo,@PeoperNum,@PeoperMoney,@MoneySendType,@MeetType,@MeetNum,@MeetDate,@MeetTime,GETDATE(),@State,@Remark,@FirstDate)
	--根据会员数量生成同等数量的会单列表
	declare @index int = @PeoperNum
	select * into #tusers from SplitString(@Peoper,',',1)
	declare @userid int	--定义用户ID变量
	declare @listid int	--定义会单列表变量
	declare @yuedate datetime	--每月标会时间
	declare @i int = 0	--增加月份的索引
	
	while @index > 0
	begin
		set @yuedate = dateadd(month,@i,@FirstDate)
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
			INSERT INTO [Order_ListUsers]([OrderNo],[OrderListID],[Userid],[AccrualMoney],[Addtime],[Lastdate])
			VALUES(@OrderNo,@listid,@userid,0,GETDATE(),GETDATE())
			set @nextid = @nextid - 1
			set @uidex = @uidex + 1
		end
		set @index = @index - 1
		set @i = @i + 1
	end
	drop table #tusers
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
/****** Object:  StoredProcedure [dbo].[SP_AddNewUser]    Script Date: 2016/9/17 18:30:22 ******/
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
/****** Object:  StoredProcedure [dbo].[SP_FriendUser]    Script Date: 2016/9/17 18:30:22 ******/
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
				--对方添加好友
				INSERT INTO [TUserFriends]([Userid],[Pcid],[Addtime],[State])
				VALUES(@Pcid,@Userid,GETDATE(),1)
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
/****** Object:  StoredProcedure [dbo].[usp_PagingLarge]    Script Date: 2016/9/17 18:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXEC usp_PagingLarge @TableNames='MLY_CodeRecord',@PrimaryKey='id',@Fields='',@PageSize=20,@CurrentPage=1,@Filter='',@Group='',@Order='id',@RecordCount=1

--EXEC usp_PagingLarge @TableNames='MLY_CodeRecord inner join t_member on cast(MLY_CodeRecord.Userid as varchar(50))=t_member.username',@PrimaryKey='MLY_CodeRecord.id',@Fields='',@PageSize=20,@CurrentPage=0,@Filter='',@Group='',@Order='MLY_CodeRecord.id',@RecordCount=1
Create PROCEDURE [dbo].[usp_PagingLarge]
@TableNames VARCHAR(200),     --表名，可以是多个表，但不能用别名
@PrimaryKey VARCHAR(100),     --主键，可以为空，但@Order为空时该值不能为空
@Fields     VARCHAR(4000),         --要取出的字段，可以是多个表的字段，可以为空，为空表示select *
@PageSize INT,             --每页记录数
@CurrentPage INT,         --当前页，0表示第1页
@Filter VARCHAR(4000) = '',     --条件，可以为空，不用填 where
@Group VARCHAR(200) = '',     --分组依据，可以为空，不用填 group by
@Order VARCHAR(200) = '',    --排序，可以为空，为空默认按主键升序排列，不用填 order by
@RecordCount int OUTPUT             --总记录数,自己增加（总记录数）
AS
BEGIN
     DECLARE @SortColumn VARCHAR(200)
     DECLARE @Operator CHAR(2)
     DECLARE @SortTable VARCHAR(200)
     DECLARE @SortName VARCHAR(200)
     IF @Fields = ''
         SET @Fields = '*'
     IF @Filter = ''
         SET @Filter = 'Where 1=1'
     ELSE
         SET @Filter = 'Where ' +   @Filter
     IF @Group <>''
         SET @Group = 'GROUP BY ' + @Group

     IF @Order <> ''
     BEGIN
         DECLARE @pos1 INT, @pos2 INT
         SET @Order = REPLACE(REPLACE(@Order, ' asc', ' ASC'), ' desc', ' DESC')
         IF CHARINDEX(' DESC', @Order) > 0
             IF CHARINDEX(' ASC', @Order) > 0
             BEGIN
                 IF CHARINDEX(' DESC', @Order) < CHARINDEX(' ASC', @Order)
                     SET @Operator = '<='
                 ELSE
                     SET @Operator = '>='
             END
             ELSE
                 SET @Operator = '<='
         ELSE
             SET @Operator = '>='
         SET @SortColumn = REPLACE(REPLACE(REPLACE(@Order, ' ASC', ''), ' DESC', ''), ' ', '')
         SET @pos1 = CHARINDEX(',', @SortColumn)
         IF @pos1 > 0
             SET @SortColumn = SUBSTRING(@SortColumn, 1, @pos1-1)
         SET @pos2 = CHARINDEX('.', @SortColumn)
         IF @pos2 > 0
         BEGIN
             SET @SortTable = SUBSTRING(@SortColumn, 1, @pos2-1)
             IF @pos1 > 0 
                 SET @SortName = SUBSTRING(@SortColumn, @pos2+1, @pos1-@pos2-1)
             ELSE
                 SET @SortName = SUBSTRING(@SortColumn, @pos2+1, LEN(@SortColumn)-@pos2)
         END
         ELSE
         BEGIN
             SET @SortTable = @TableNames
             SET @SortName = @SortColumn
         END
     END
     ELSE
     BEGIN
         SET @SortColumn = @PrimaryKey
         SET @SortTable = @TableNames
         SET @SortName = @SortColumn
         SET @Order = @SortColumn
         SET @Operator = '>='
     END

     DECLARE @type varchar(50)
     DECLARE @prec int
     Select @type=t.name, @prec=c.prec
     FROM sysobjects o 
     JOIN syscolumns c on o.id=c.id
     JOIN systypes t on c.xusertype=t.xusertype
     Where o.name = @SortTable AND c.name = @SortName
     IF CHARINDEX('char', @type) > 0
     SET @type = @type + '(' + CAST(@prec AS varchar) + ')'

     DECLARE @TopRows INT
     SET @TopRows = @PageSize * @CurrentPage + 1
     print @TopRows
     print @Operator
     EXEC('
         DECLARE @SortColumnBegin ' + @type + '
         SET ROWCOUNT ' + @TopRows + '
         Select @SortColumnBegin=' + @SortColumn + ' FROM   ' + @TableNames + ' ' + @Filter + ' ' + @Group + ' orDER BY ' + @Order + '
         SET ROWCOUNT ' + @PageSize + '
         Select ' + @Fields + ' FROM   ' + @TableNames + ' ' + @Filter   + ' AND ' + @SortColumn + '' + @Operator + '@SortColumnBegin ' + @Group + ' orDER BY ' + @Order + '    
     ')    
        IF @RecordCount IS NULL
BEGIN
    DECLARE @sql nvarchar(4000)
    SET @sql=N'SELECT @RecordCount=COUNT(*)'
        +N' FROM '+@TableNames
        +N' '+@Filter
    EXEC sp_executesql @sql,N'@RecordCount int OUTPUT',@RecordCount OUTPUT
END


END

GO
/****** Object:  UserDefinedFunction [dbo].[SplitString]    Script Date: 2016/9/17 18:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[SplitString]
(
    @Input nvarchar(max),    @Separator nvarchar(max)=',', 
    @RemoveEmptyEntries bit=1 )
returns @TABLE table 
(
    [Id] int identity(1,1),
    [Value] nvarchar(max)
) 
as
begin 
    declare @Index int, @Entry nvarchar(max)
    set @Index = charindex(@Separator,@Input)

    while (@Index>0)
    begin
        set @Entry=ltrim(rtrim(substring(@Input, 1, @Index-1)))
        
        if (@RemoveEmptyEntries=0) or (@RemoveEmptyEntries=1 and @Entry<>'')
            begin
                insert into @TABLE([Value]) Values(@Entry)
            end

        set @Input = substring(@Input, @Index+datalength(@Separator)/2, len(@Input))
        set @Index = charindex(@Separator, @Input)
    end
    
    set @Entry=ltrim(rtrim(@Input))
    if (@RemoveEmptyEntries=0) or (@RemoveEmptyEntries=1 and @Entry<>'')
        begin
            insert into @TABLE([Value]) Values(@Entry)
        end

    return
end

GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 2016/9/17 18:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory2] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Cards]    Script Date: 2016/9/17 18:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cards](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CardCode] [int] NOT NULL,
	[CardName] [nvarchar](max) NULL,
	[CardBankType] [int] NOT NULL,
	[CardUseType] [int] NOT NULL,
	[CardAmount] [int] NOT NULL,
	[CardBillDate] [datetime] NOT NULL,
	[CardDelayDay] [int] NOT NULL,
	[CardInputDate] [datetime] NOT NULL,
	[Remark] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Cards] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OnlyNameTests]    Script Date: 2016/9/17 18:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OnlyNameTests](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[InputDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.OnlyNameTests] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Order_Lists]    Script Date: 2016/9/17 18:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Order_Lists](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[OrderNo] [nvarchar](50) NOT NULL,
	[Userid] [int] NOT NULL,
	[Addtime] [datetime] NOT NULL,
	[State] [char](1) NULL,
	[AccrualMoney] [int] NULL,
	[MeetDate] [date] NULL,
 CONSTRAINT [PK_Order_List] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Order_ListUsers]    Script Date: 2016/9/17 18:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order_ListUsers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[OrderNo] [nvarchar](50) NOT NULL,
	[OrderListID] [int] NOT NULL,
	[Userid] [int] NOT NULL,
	[AccrualMoney] [int] NOT NULL,
	[Addtime] [datetime] NOT NULL,
	[Lastdate] [datetime] NOT NULL,
 CONSTRAINT [PK_Order_ListUsers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Orders]    Script Date: 2016/9/17 18:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[OrderNo] [nvarchar](50) NOT NULL,
	[PeoperNum] [int] NOT NULL,
	[PeoperMoney] [int] NOT NULL,
	[MoneySendType] [int] NOT NULL,
	[MeetType] [int] NOT NULL,
	[MeetNum] [int] NOT NULL,
	[FirstDate] [datetime] NULL,
	[MeetDate] [nvarchar](14) NOT NULL,
	[MeetTime] [nvarchar](30) NOT NULL,
	[InputDate] [datetime] NOT NULL,
	[State] [int] NOT NULL,
	[Remark] [nvarchar](250) NULL,
 CONSTRAINT [PK_dbo.Orders] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TUserFriends]    Script Date: 2016/9/17 18:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TUserFriends](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Userid] [int] NOT NULL,
	[Pcid] [int] NOT NULL,
	[Addtime] [datetime] NOT NULL,
	[State] [char](1) NOT NULL,
 CONSTRAINT [PK_TUserFriend] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TUsers]    Script Date: 2016/9/17 18:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TUsers](
	[id] [int] IDENTITY(10000,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[UserPwd] [nvarchar](50) NOT NULL,
	[BankPwd] [nvarchar](50) NOT NULL,
	[NickName] [nvarchar](50) NOT NULL,
	[UserJb] [int] NOT NULL,
	[TrueName] [nvarchar](50) NULL,
	[IdentityNum] [nvarchar](18) NULL,
	[Phone] [nvarchar](16) NULL,
	[RegIP] [nvarchar](16) NULL,
	[TxUrl] [nvarchar](100) NULL,
	[State] [int] NOT NULL,
	[Addtime] [datetime] NOT NULL,
 CONSTRAINT [PK_TUsers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Order_Lists] ADD  CONSTRAINT [DF_Order_List_Addtime]  DEFAULT (getdate()) FOR [Addtime]
GO
ALTER TABLE [dbo].[Order_Lists] ADD  CONSTRAINT [DF_Order_List_AccrualMoney]  DEFAULT ((0)) FOR [AccrualMoney]
GO
ALTER TABLE [dbo].[Order_ListUsers] ADD  CONSTRAINT [DF_Order_ListUsers_Addtime]  DEFAULT (getdate()) FOR [Addtime]
GO
ALTER TABLE [dbo].[Order_ListUsers] ADD  CONSTRAINT [DF_Order_ListUsers_Lastdate]  DEFAULT (getdate()) FOR [Lastdate]
GO
ALTER TABLE [dbo].[TUserFriends] ADD  CONSTRAINT [DF_TUserFriend_Addtime]  DEFAULT (getdate()) FOR [Addtime]
GO
ALTER TABLE [dbo].[TUserFriends] ADD  CONSTRAINT [DF_TUserFriends_State]  DEFAULT ((1)) FOR [State]
GO
ALTER TABLE [dbo].[TUsers] ADD  CONSTRAINT [DF_TUsers_Addtime]  DEFAULT (getdate()) FOR [Addtime]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_Lists', @level2type=N'COLUMN',@level2name=N'OrderNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'夺标用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_Lists', @level2type=N'COLUMN',@level2name=N'Userid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'具体的标会日期时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_Lists', @level2type=N'COLUMN',@level2name=N'Addtime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单次标会状态(2未结束,1已结束)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_Lists', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'此次标会利息金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_Lists', @level2type=N'COLUMN',@level2name=N'AccrualMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'互助单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_ListUsers', @level2type=N'COLUMN',@level2name=N'OrderNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每月互助记录单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_ListUsers', @level2type=N'COLUMN',@level2name=N'OrderListID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最终夺标的用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_ListUsers', @level2type=N'COLUMN',@level2name=N'Userid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最终夺标的金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_ListUsers', @level2type=N'COLUMN',@level2name=N'AccrualMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最终一次修改标金的时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_ListUsers', @level2type=N'COLUMN',@level2name=N'Lastdate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'互助单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'OrderNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'PeoperNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每人会费金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'PeoperMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会费发放类型1全额发放,2减利息后发放' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MoneySendType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标会类型1约定日期2隔多长时间标会一次' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MeetType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每月标会次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MeetNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'首次标会日期时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'FirstDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标会日期(当每月多次标会次数时)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MeetDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标会时间(当每月多次标会次数时)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MeetTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1活会,2死会,3险会' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUserFriends', @level2type=N'COLUMN',@level2name=N'Userid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'好友ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUserFriends', @level2type=N'COLUMN',@level2name=N'Pcid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1正常好友2黑名单' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUserFriends', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户账号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUsers', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUsers', @level2type=N'COLUMN',@level2name=N'UserPwd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户银行密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUsers', @level2type=N'COLUMN',@level2name=N'BankPwd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户昵称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUsers', @level2type=N'COLUMN',@level2name=N'NickName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户金币' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUsers', @level2type=N'COLUMN',@level2name=N'UserJb'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户真实名字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUsers', @level2type=N'COLUMN',@level2name=N'TrueName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户身份证号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUsers', @level2type=N'COLUMN',@level2name=N'IdentityNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户手机号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUsers', @level2type=N'COLUMN',@level2name=N'Phone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUsers', @level2type=N'COLUMN',@level2name=N'RegIP'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'头像URL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUsers', @level2type=N'COLUMN',@level2name=N'TxUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户状态1正常,2临时,3禁用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUsers', @level2type=N'COLUMN',@level2name=N'State'
GO
USE [master]
GO
ALTER DATABASE [GetMoney] SET  READ_WRITE 
GO
