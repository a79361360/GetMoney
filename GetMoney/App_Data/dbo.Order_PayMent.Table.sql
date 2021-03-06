USE [GetMoney]
GO
/****** Object:  Table [dbo].[Order_PayMent]    Script Date: 11/04/2016 01:21:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order_PayMent](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[OrderNo] [nvarchar](50) NOT NULL,
	[OrderListID] [int] NOT NULL,
	[StayPayNum] [int] NOT NULL,
	[StayPayTax] [int] NOT NULL,
	[RealPayNum] [int] NOT NULL,
	[PayDate] [date] NOT NULL,
	[AddDate] [datetime] NOT NULL,
	[State] [int] NOT NULL,
	[UserID] [int] NULL,
 CONSTRAINT [PK_PayMent] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_PayMent', @level2type=N'COLUMN',@level2name=N'OrderNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会单列表记录ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_PayMent', @level2type=N'COLUMN',@level2name=N'OrderListID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会费金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_PayMent', @level2type=N'COLUMN',@level2name=N'StayPayNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标息金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_PayMent', @level2type=N'COLUMN',@level2name=N'StayPayTax'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_PayMent', @level2type=N'COLUMN',@level2name=N'RealPayNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'需要支付的日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_PayMent', @level2type=N'COLUMN',@level2name=N'PayDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加记录时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_PayMent', @level2type=N'COLUMN',@level2name=N'AddDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态0为未付,1为已付,2未全付' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_PayMent', @level2type=N'COLUMN',@level2name=N'State'
GO
ALTER TABLE [dbo].[Order_PayMent] ADD  CONSTRAINT [DF_PayMent_StayPayNum]  DEFAULT ((0)) FOR [StayPayNum]
GO
ALTER TABLE [dbo].[Order_PayMent] ADD  CONSTRAINT [DF_PayMent_StayPayTax]  DEFAULT ((0)) FOR [StayPayTax]
GO
ALTER TABLE [dbo].[Order_PayMent] ADD  CONSTRAINT [DF_PayMent_RealPayNum]  DEFAULT ((0)) FOR [RealPayNum]
GO
ALTER TABLE [dbo].[Order_PayMent] ADD  CONSTRAINT [DF_PayMent_AddDate]  DEFAULT (getdate()) FOR [AddDate]
GO
ALTER TABLE [dbo].[Order_PayMent] ADD  CONSTRAINT [DF_PayMent_State]  DEFAULT ((0)) FOR [State]
GO
