USE [GetMoney]
GO
/****** Object:  Table [dbo].[TUserFriends]    Script Date: 10/09/2016 09:51:16 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUserFriends', @level2type=N'COLUMN',@level2name=N'Userid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'好友ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUserFriends', @level2type=N'COLUMN',@level2name=N'Pcid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1正常好友2黑名单' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TUserFriends', @level2type=N'COLUMN',@level2name=N'State'
GO
ALTER TABLE [dbo].[TUserFriends] ADD  CONSTRAINT [DF_TUserFriend_Addtime]  DEFAULT (getdate()) FOR [Addtime]
GO
ALTER TABLE [dbo].[TUserFriends] ADD  CONSTRAINT [DF_TUserFriends_State]  DEFAULT ((1)) FOR [State]
GO
