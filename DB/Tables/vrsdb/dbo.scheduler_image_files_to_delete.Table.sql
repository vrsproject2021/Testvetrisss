USE [vrsdb]
GO
/****** Object:  Table [dbo].[scheduler_image_files_to_delete]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[scheduler_image_files_to_delete]
GO
/****** Object:  Table [dbo].[scheduler_image_files_to_delete]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scheduler_image_files_to_delete](
	[id] [uniqueidentifier] NOT NULL,
	[file_name] [nvarchar](250) NOT NULL,
	[import_session_id] [nvarchar](30) NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[date_downloaded] [datetime] NOT NULL,
	[date_created] [datetime] NOT NULL,
 CONSTRAINT [PK_scheduler_image_files_to_delete] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
