USE [vrslogdb]
GO
/****** Object:  Table [dbo].[sys_scheduler_log]    Script Date: 20-08-2021 20:49:16 ******/
DROP TABLE [dbo].[sys_scheduler_log]
GO
/****** Object:  Table [dbo].[sys_scheduler_log]    Script Date: 20-08-2021 20:49:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[sys_scheduler_log](
	[is_error] [bit] NULL,
	[service_id] [int] NULL,
	[log_date] [datetime] NULL,
	[log_message] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
