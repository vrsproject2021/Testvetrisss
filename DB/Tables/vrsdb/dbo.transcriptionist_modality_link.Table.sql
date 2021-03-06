USE [vrsdb]
GO
/****** Object:  Table [dbo].[transcriptionist_modality_link]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[transcriptionist_modality_link]
GO
/****** Object:  Table [dbo].[transcriptionist_modality_link]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[transcriptionist_modality_link](
	[transcriptionist_id] [uniqueidentifier] NOT NULL,
	[modality_id] [int] NOT NULL,
	[default_fee] [money] NULL CONSTRAINT [DF_transcriptionist_modality_link_default_fee]  DEFAULT ((0)),
	[addl_STAT_fee] [money] NULL CONSTRAINT [DF_transcriptionist_modality_link_addl_STAT_fee]  DEFAULT ((0)),
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_transcriptionist_modality_link] PRIMARY KEY CLUSTERED 
(
	[transcriptionist_id] ASC,
	[modality_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
