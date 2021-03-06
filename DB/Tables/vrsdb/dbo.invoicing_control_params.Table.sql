USE [vrsdb]
GO
/****** Object:  Table [dbo].[invoicing_control_params]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[invoicing_control_params]
GO
/****** Object:  Table [dbo].[invoicing_control_params]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[invoicing_control_params](
	[control_code] [nvarchar](20) NOT NULL,
	[data_value_char] [nvarchar](2000) NULL CONSTRAINT [DF_invoicing_control_params_data_value_char]  DEFAULT (''),
	[data_value_int] [int] NULL CONSTRAINT [DF_invoicing_control_params_data_value_int]  DEFAULT ((0)),
	[data_value_dec] [decimal](12, 2) NULL CONSTRAINT [DF_invoicing_control_params_data_value_dec]  DEFAULT ((0)),
	[value_type] [nvarchar](5) NOT NULL,
	[ui_prefix] [nvarchar](5) NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_invoicing_control_params] PRIMARY KEY CLUSTERED 
(
	[control_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
