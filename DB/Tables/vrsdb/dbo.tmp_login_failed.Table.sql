USE [vrsdb]
GO
/****** Object:  Table [dbo].[tmp_login_failed]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[tmp_login_failed]
GO
/****** Object:  Table [dbo].[tmp_login_failed]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tmp_login_failed](
	[id] [uniqueidentifier] NOT NULL,
	[code] [nvarchar](10) NULL,
	[name] [nvarchar](200) NULL,
	[login_id] [nvarchar](50) NULL,
	[password] [nvarchar](200) NOT NULL,
	[password_entered] [nvarchar](200) NOT NULL,
	[email_id] [nvarchar](100) NULL,
	[contact_no] [nvarchar](20) NULL,
	[date_failed] [datetime] NULL,
 CONSTRAINT [PK_tmp_login_failed] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
