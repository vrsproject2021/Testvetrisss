USE [vrsdb]
GO
/****** Object:  Table [dbo].[institution_reg_modality_link]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[institution_reg_modality_link]
GO
/****** Object:  Table [dbo].[institution_reg_modality_link]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[institution_reg_modality_link](
	[modality_id] [int] NOT NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_institution_reg_modality_link] PRIMARY KEY CLUSTERED 
(
	[modality_id] ASC,
	[institution_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
