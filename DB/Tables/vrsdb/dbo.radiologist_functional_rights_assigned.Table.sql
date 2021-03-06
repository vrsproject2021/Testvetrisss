USE [vrsdb]
GO
/****** Object:  Table [dbo].[radiologist_functional_rights_assigned]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[radiologist_functional_rights_assigned]
GO
/****** Object:  Table [dbo].[radiologist_functional_rights_assigned]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[radiologist_functional_rights_assigned](
	[radiologist_id] [uniqueidentifier] NOT NULL,
	[right_code] [nvarchar](20) NOT NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
 CONSTRAINT [PK_radiologist_functional_rights_assigned] PRIMARY KEY CLUSTERED 
(
	[radiologist_id] ASC,
	[right_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
