SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[report_object](
	[id] [uniqueidentifier] NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[model] [nvarchar](MAX) NULL,
	[data_set_info] [nvarchar](max) NULL,
	[created_by] [uniqueidentifier] NULL,
	[created_on] [datetime] NULL,
	[last_modified_by] [uniqueidentifier] NULL,
	[last_modified_on] [datetime] NULL,
	[tabular_metadata] [nvarchar](max) NULL,
 CONSTRAINT [PK__data_set__3213E83F325B08EA] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
