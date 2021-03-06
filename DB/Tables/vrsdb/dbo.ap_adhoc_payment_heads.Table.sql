USE [vrsdb]
GO
/****** Object:  Table [dbo].[ap_adhoc_payment_heads]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[ap_adhoc_payment_heads]
GO
/****** Object:  Table [dbo].[ap_adhoc_payment_heads]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_adhoc_payment_heads](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [nvarchar](5) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[is_active] [nchar](1) NULL,
	[created_by] [uniqueidentifier] NULL,
	[date_created] [datetime] NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_ap_adhoc_payment_heads] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
