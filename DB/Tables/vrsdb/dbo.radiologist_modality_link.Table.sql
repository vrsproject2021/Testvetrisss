USE [vrsdb]
GO
/****** Object:  Table [dbo].[radiologist_modality_link]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[radiologist_modality_link]
GO
/****** Object:  Table [dbo].[radiologist_modality_link]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[radiologist_modality_link](
	[radiologist_id] [uniqueidentifier] NOT NULL,
	[modality_id] [int] NOT NULL,
	[updated_by] [uniqueidentifier] NOT NULL,
	[date_updated] [datetime] NOT NULL,
	[prelim_fee] [money] NULL CONSTRAINT [DF_radiologist_modality_link_prelim_fee]  DEFAULT ((0)),
	[final_fee] [money] NULL CONSTRAINT [DF_radiologist_modality_link_final_fee]  DEFAULT ((0)),
	[addl_STAT_fee] [money] NULL CONSTRAINT [DF_radiologist_modality_link_addl_STAT_fee]  DEFAULT ((0)),
	[work_unit] [int] NULL CONSTRAINT [DF_radiologist_modality_link_work_unit]  DEFAULT ((0)),
 CONSTRAINT [PK_radiologist_modality_link] PRIMARY KEY CLUSTERED 
(
	[radiologist_id] ASC,
	[modality_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
