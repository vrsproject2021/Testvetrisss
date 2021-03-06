USE [vrsdb]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'modality', @level2type=N'COLUMN',@level2name=N'invoice_by'

GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'modality', @level2type=N'COLUMN',@level2name=N'track_by'

GO
/****** Object:  Table [dbo].[modality]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[modality]
GO
/****** Object:  Table [dbo].[modality]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[modality](
	[id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[code] [nvarchar](10) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[dicom_tag] [nvarchar](50) NOT NULL,
	[is_active] [nchar](1) NULL CONSTRAINT [DF_modality_is_active]  DEFAULT (N'Y'),
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[track_by] [nchar](1) NULL,
	[invoice_by] [nchar](1) NULL,
	[file_receive_path] [nvarchar](250) NULL,
 CONSTRAINT [PK_modality] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[I]mage Count,[O]bject Count' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'modality', @level2type=N'COLUMN',@level2name=N'track_by'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[I]mage Count,[M]inute Count,[B]ody Part Count' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'modality', @level2type=N'COLUMN',@level2name=N'invoice_by'
GO
