USE [vrsdb]
GO
/****** Object:  Table [dbo].[modality_gl_code_link]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[modality_gl_code_link]
GO
/****** Object:  Table [dbo].[modality_gl_code_link]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[modality_gl_code_link](
	[modality_id] [int] NOT NULL,
	[category_id] [int] NOT NULL,
	[gl_code] [nvarchar](5) NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_modality_gl_code_link] PRIMARY KEY CLUSTERED 
(
	[modality_id] ASC,
	[category_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
