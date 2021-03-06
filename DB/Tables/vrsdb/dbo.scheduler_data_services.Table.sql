USE [vrsdb]
GO
/****** Object:  Table [dbo].[scheduler_data_services]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[scheduler_data_services]
GO
/****** Object:  Table [dbo].[scheduler_data_services]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scheduler_data_services](
	[service_id] [int] NOT NULL,
	[service_name] [nvarchar](50) NOT NULL,
	[frequency] [int] NULL CONSTRAINT [DF_scheduler_data_services_frequency]  DEFAULT ((0)),
	[updated_on] [datetime] NULL,
 CONSTRAINT [PK_scheduler_data_services] PRIMARY KEY CLUSTERED 
(
	[service_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
