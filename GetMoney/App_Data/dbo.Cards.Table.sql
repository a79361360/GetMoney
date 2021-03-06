USE [GetMoney]
GO
/****** Object:  Table [dbo].[Cards]    Script Date: 10/09/2016 09:51:16 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
