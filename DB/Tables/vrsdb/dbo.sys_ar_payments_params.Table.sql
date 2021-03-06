USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_ar_payments_params]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_ar_payments_params]
GO
/****** Object:  Table [dbo].[sys_ar_payments_params]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_ar_payments_params](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ar_payments_year] [int] NOT NULL,
	[ar_payments_count] [int] NOT NULL,
	[ar_refunds_count] [int] NULL,
 CONSTRAINT [PK_sys_ar_payments_params] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
