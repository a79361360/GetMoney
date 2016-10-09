USE [GetMoney]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 10/09/2016 09:51:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[OrderNo] [nvarchar](50) NOT NULL,
	[PeoperNum] [int] NOT NULL,
	[PeoperMoney] [int] NOT NULL,
	[LowestMoney] [int] NOT NULL,
	[TouUserid] [int] NOT NULL,
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
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
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_LowestMoney]  DEFAULT ((0)) FOR [LowestMoney]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_TouUserid]  DEFAULT ((0)) FOR [TouUserid]
GO
