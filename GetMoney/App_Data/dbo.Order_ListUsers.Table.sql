USE [GetMoney]
GO
/****** Object:  Table [dbo].[Order_ListUsers]    Script Date: 11/07/2016 18:09:19 ******/
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
	[StayPayNum] [int] NOT NULL,
	[StayPayTax] [int] NOT NULL,
	[RealPayNum] [int] NOT NULL,
	[PayDate] [datetime] NOT NULL,
	[PayState] [int] NOT NULL,
	[Addtime] [datetime] NOT NULL,
	[Lastdate] [datetime] NOT NULL,
 CONSTRAINT [PK_Order_ListUsers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'互助单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_ListUsers', @level2type=N'COLUMN',@level2name=N'OrderNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每月互助记录单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_ListUsers', @level2type=N'COLUMN',@level2name=N'OrderListID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最终夺标的用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_ListUsers', @level2type=N'COLUMN',@level2name=N'Userid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最终夺标的金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_ListUsers', @level2type=N'COLUMN',@level2name=N'AccrualMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'待支付会费金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_ListUsers', @level2type=N'COLUMN',@level2name=N'StayPayNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'待支付标息金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_ListUsers', @level2type=N'COLUMN',@level2name=N'StayPayTax'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'实际用户支付金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_ListUsers', @level2type=N'COLUMN',@level2name=N'RealPayNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'实现支付时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_ListUsers', @level2type=N'COLUMN',@level2name=N'PayDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态0为未付,1为已付,2未全付' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_ListUsers', @level2type=N'COLUMN',@level2name=N'PayState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最终一次修改标金的时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order_ListUsers', @level2type=N'COLUMN',@level2name=N'Lastdate'
GO
ALTER TABLE [dbo].[Order_ListUsers] ADD  CONSTRAINT [DF_Order_ListUsers_StayPayNum]  DEFAULT ((0)) FOR [StayPayNum]
GO
ALTER TABLE [dbo].[Order_ListUsers] ADD  CONSTRAINT [DF_Order_ListUsers_StayPayTax]  DEFAULT ((0)) FOR [StayPayTax]
GO
ALTER TABLE [dbo].[Order_ListUsers] ADD  CONSTRAINT [DF_Order_ListUsers_RealPayNum]  DEFAULT ((0)) FOR [RealPayNum]
GO
ALTER TABLE [dbo].[Order_ListUsers] ADD  CONSTRAINT [DF_Order_ListUsers_PayDate]  DEFAULT (getdate()) FOR [PayDate]
GO
ALTER TABLE [dbo].[Order_ListUsers] ADD  CONSTRAINT [DF_Order_ListUsers_PayState]  DEFAULT ((0)) FOR [PayState]
GO
ALTER TABLE [dbo].[Order_ListUsers] ADD  CONSTRAINT [DF_Order_ListUsers_Addtime]  DEFAULT (getdate()) FOR [Addtime]
GO
ALTER TABLE [dbo].[Order_ListUsers] ADD  CONSTRAINT [DF_Order_ListUsers_Lastdate]  DEFAULT (getdate()) FOR [Lastdate]
GO
