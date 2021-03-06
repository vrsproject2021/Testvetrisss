USE [vrsarchive20db]
GO
/****** Object:  Table [dbo].[scheduler_img_file_downloads_grouped_docs]    Script Date: 21-09-2021 17:15:31 ******/
DROP TABLE [dbo].[scheduler_img_file_downloads_grouped_docs]
GO
/****** Object:  Table [dbo].[scheduler_img_file_downloads_grouped_docs]    Script Date: 21-09-2021 17:15:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[scheduler_img_file_downloads_grouped_docs](
	[study_hdr_id] [uniqueidentifier] NOT NULL,
	[document_id] [uniqueidentifier] NOT NULL,
	[document_name] [nvarchar](100) NOT NULL,
	[document_srl_no] [int] NOT NULL,
	[document_link] [nvarchar](100) NOT NULL,
	[document_file_type] [nvarchar](5) NOT NULL,
	[document_file] [varbinary](max) NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_scheduler_img_file_downloads_grouped_docs] PRIMARY KEY CLUSTERED 
(
	[document_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
