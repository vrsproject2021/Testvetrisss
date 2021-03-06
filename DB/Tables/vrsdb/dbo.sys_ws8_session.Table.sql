USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_ws8_session]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_ws8_session]
GO
/****** Object:  Table [dbo].[sys_ws8_session]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_ws8_session](
	[session_id] [nvarchar](30) NOT NULL,
	[date_created] [datetime] NOT NULL,
	[created_by] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

GO
