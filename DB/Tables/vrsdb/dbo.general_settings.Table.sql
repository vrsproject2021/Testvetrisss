USE [vrsdb]
GO
/****** Object:  Table [dbo].[general_settings]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[general_settings]
GO
/****** Object:  Table [dbo].[general_settings]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[general_settings](
	[control_code] [nvarchar](20) NOT NULL,
	[data_type_number] [int] NULL CONSTRAINT [DF_general_settings_data_type_number]  DEFAULT ((0)),
	[data_type_string] [nvarchar](200) NULL CONSTRAINT [DF_general_settings_data_type_string]  DEFAULT (''),
	[data_type_decimal] [decimal](12, 2) NULL CONSTRAINT [DF_general_settings_data_type_decimal]  DEFAULT ((0)),
	[updated_by] [uniqueidentifier] NOT NULL,
	[date_updated] [datetime] NOT NULL,
	[group_id] [int] NULL,
	[data_type] [nchar](1) NULL,
	[control_desc] [nvarchar](250) NULL,
	[is_password] [nchar](1) NULL,
	[ui_control] [nvarchar](5) NULL,
	[ui_value_list] [nvarchar](max) NULL,
	[group_display_index] [int] NULL,
 CONSTRAINT [PK_general_settings] PRIMARY KEY CLUSTERED 
(
	[control_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
