USE [vrsdb]
GO
/****** Object:  Table [dbo].[SysReports]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[SysReports]
GO
/****** Object:  Table [dbo].[SysReports]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysReports](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Category] [nvarchar](150) NULL,
	[Draft] [bit] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[LastModifiedBy] [uniqueidentifier] NULL,
	[LastModifiedOn] [datetime] NULL,
	[Report] [nvarchar](max) NULL,
 CONSTRAINT [PK__SYSREPORTS__3213E83F325B08EA] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
