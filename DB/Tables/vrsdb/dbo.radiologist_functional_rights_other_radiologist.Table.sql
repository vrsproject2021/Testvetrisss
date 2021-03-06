USE [vrsdb]
GO
/****** Object:  Table [dbo].[radiologist_functional_rights_other_radiologist]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[radiologist_functional_rights_other_radiologist]
GO
/****** Object:  Table [dbo].[radiologist_functional_rights_other_radiologist]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[radiologist_functional_rights_other_radiologist](
	[radiologist_id] [uniqueidentifier] NOT NULL,
	[other_radiologist_id] [uniqueidentifier] NOT NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
 CONSTRAINT [PK_radiologist_functional_rights_other_radiologist] PRIMARY KEY CLUSTERED 
(
	[radiologist_id] ASC,
	[other_radiologist_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
