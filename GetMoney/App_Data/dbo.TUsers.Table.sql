USE [GetMoney]
GO
/****** Object:  Table [dbo].[TUsers]    Script Date: 10/09/2016 09:51:16 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
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
ALTER TABLE [dbo].[TUsers] ADD  CONSTRAINT [DF_TUsers_Addtime]  DEFAULT (getdate()) FOR [Addtime]
GO
