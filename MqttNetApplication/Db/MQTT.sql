USE [master]
GO

CREATE DATABASE [MQTT] 
GO

USE [MQTT]
GO
/****** Object:  Table [dbo].[iplogs]    Script Date: 17-09-2023 15:24:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[iplogs](
	[iplogsid] [bigint] IDENTITY(1,1) NOT NULL,
	[clientid] [nvarchar](50) NOT NULL,
	[clientip] [nvarchar](50) NULL,
	[timestamp] [datetime] NULL,
 CONSTRAINT [PK_iplogs] PRIMARY KEY CLUSTERED 
(
	[iplogsid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[logs]    Script Date: 17-09-2023 15:24:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[logs](
	[logid] [bigint] IDENTITY(1,1) NOT NULL,
	[logdate] [datetime] NULL,
	[clientid] [nvarchar](50) NULL,
	[topic] [nvarchar](max) NULL,
	[payload] [nvarchar](max) NULL,
	[qos] [nvarchar](50) NULL,
 CONSTRAINT [PK_logs] PRIMARY KEY CLUSTERED 
(
	[logid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[userlogs]    Script Date: 17-09-2023 15:24:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[userlogs](
	[userlogid] [bigint] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](50) NULL,
	[password] [nvarchar](50) NULL,
	[clientid] [nvarchar](50) NULL,
	[timestamps] [datetime] NULL,
	[clientip] [nvarchar](50) NULL,
 CONSTRAINT [PK_userlogs] PRIMARY KEY CLUSTERED 
(
	[userlogid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 17-09-2023 15:24:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[userId] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](50) NULL,
	[userpass] [nvarchar](50) NULL,
	[allowed] [bit] NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[users] ON 
GO
INSERT [dbo].[users] ([userId], [username], [userpass], [allowed]) VALUES (1, N'Valon', N'Test', 1)
GO
SET IDENTITY_INSERT [dbo].[users] OFF
GO
USE [master]
GO
ALTER DATABASE [MQTT] SET  READ_WRITE 
GO
