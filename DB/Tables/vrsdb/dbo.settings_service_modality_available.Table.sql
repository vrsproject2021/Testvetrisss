USE [vrsdb]
GO
/****** Object:  Table [dbo].[settings_service_modality_available]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[settings_service_modality_available]
GO
/****** Object:  Table [dbo].[settings_service_modality_available]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[settings_service_modality_available](
	[service_id] [int] NOT NULL,
	[modality_id] [int] NOT NULL,
	[available] [nchar](1) NULL,
	[message_display] [nvarchar](500) NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_settings_service_modality_available] PRIMARY KEY CLUSTERED 
(
	[service_id] ASC,
	[modality_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
