USE [vrsdb]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'study_manual_upload_files', @level2type=N'COLUMN',@level2name=N'file_type'

GO
/****** Object:  Table [dbo].[study_manual_upload_files]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[study_manual_upload_files]
GO
/****** Object:  Table [dbo].[study_manual_upload_files]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[study_manual_upload_files](
	[file_id] [uniqueidentifier] NOT NULL,
	[session_id] [nvarchar](30) NOT NULL,
	[file_name] [nvarchar](250) NOT NULL,
	[file_content] [varbinary](max) NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[institution_code] [nvarchar](5) NOT NULL,
	[institution_name] [nvarchar](100) NOT NULL,
	[file_type] [nchar](1) NULL,
	[uploaded_by] [uniqueidentifier] NOT NULL,
	[date_uploaded] [datetime] NOT NULL,
 CONSTRAINT [PK_study_manual_upload_files] PRIMARY KEY CLUSTERED 
(
	[file_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[D]ICOM,[I]mage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'study_manual_upload_files', @level2type=N'COLUMN',@level2name=N'file_type'
GO
