USE [GetMoney]
GO
/****** Object:  Table [dbo].[Order_Lists]    Script Date: 10/09/2016 09:51:16 ******/
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
	[MeetDate] [datetime] NULL,
 CONSTRAINT [PK_Order_List] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
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
ALTER TABLE [dbo].[Order_Lists] ADD  CONSTRAINT [DF_Order_List_Addtime]  DEFAULT (getdate()) FOR [Addtime]
GO
ALTER TABLE [dbo].[Order_Lists] ADD  CONSTRAINT [DF_Order_List_AccrualMoney]  DEFAULT ((0)) FOR [AccrualMoney]
GO
