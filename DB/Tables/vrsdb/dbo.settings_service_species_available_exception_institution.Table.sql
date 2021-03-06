USE [vrsdb]
GO
/****** Object:  Table [dbo].[settings_service_species_available_exception_institution]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[settings_service_species_available_exception_institution]
GO
/****** Object:  Table [dbo].[settings_service_species_available_exception_institution]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[settings_service_species_available_exception_institution](
	[service_id] [int] NOT NULL,
	[species_id] [int] NOT NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[after_hours] [nchar](1) NOT NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
 CONSTRAINT [PK_settings_service_species_available_exception_institution] PRIMARY KEY CLUSTERED 
(
	[service_id] ASC,
	[species_id] ASC,
	[institution_id] ASC,
	[after_hours] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
