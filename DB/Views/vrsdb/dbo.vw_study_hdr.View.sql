USE [vrsdb]
GO
/****** Object:  View [dbo].[vw_study_hdr]    Script Date: 20-08-2021 20:59:58 ******/
DROP VIEW [dbo].[vw_study_hdr]
GO
/****** Object:  View [dbo].[vw_study_hdr]    Script Date: 20-08-2021 20:59:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[vw_study_hdr]
		as 
		(select * from vrsarchive19db..study_hdr_archive
		 union
		select * from vrsarchive20db..study_hdr_archive)
GO
