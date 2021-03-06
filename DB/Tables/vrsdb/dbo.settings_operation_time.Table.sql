USE [vrsdb]
GO
/****** Object:  Table [dbo].[settings_operation_time]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[settings_operation_time]
GO
/****** Object:  Table [dbo].[settings_operation_time]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[settings_operation_time](
	[day_no] [int] NOT NULL,
	[day_name] [nvarchar](20) NOT NULL,
	[from_time] [nvarchar](5) NOT NULL,
	[till_time] [nvarchar](5) NOT NULL,
	[time_zone_id] [int] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[message_display] [nvarchar](500) NULL,
 CONSTRAINT [PK_settings_operation_time] PRIMARY KEY CLUSTERED 
(
	[day_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
