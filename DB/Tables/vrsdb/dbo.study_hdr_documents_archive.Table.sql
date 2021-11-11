USE [vrsdb]
GO
/****** Object:  Table [dbo].[study_hdr_documents_archive]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[study_hdr_documents_archive]
GO
/****** Object:  Table [dbo].[study_hdr_documents_archive]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[study_hdr_documents_archive](
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
 CONSTRAINT [PK_study_hdr_documents_archive] PRIMARY KEY CLUSTERED 
(
	[document_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
