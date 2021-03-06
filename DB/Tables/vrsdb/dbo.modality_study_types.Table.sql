USE [vrsdb]
GO
/****** Object:  Table [dbo].[modality_study_types]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[modality_study_types]
GO
/****** Object:  Table [dbo].[modality_study_types]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[modality_study_types](
	[id] [uniqueidentifier] NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[modality_id] [int] NOT NULL,
	[is_active] [nchar](1) NULL CONSTRAINT [DF_modality_study_types_is_active]  DEFAULT (N'Y'),
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[validate_study_count] [nchar](1) NULL CONSTRAINT [DF_modality_study_types_validate_study_count]  DEFAULT (N'N'),
	[category_id] [int] NULL,
 CONSTRAINT [PK_modality_study_types] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
