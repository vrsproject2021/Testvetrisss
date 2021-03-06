USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_gl_code_map_modality_category_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_gl_code_map_modality_category_fetch]
GO
/****** Object:  StoredProcedure [dbo].[settings_gl_code_map_modality_category_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_gl_code_map_modality_category_fetch : 
				  fetch g/l code mapping with modality and category
** Created By   : Pavel Guha
** Created On   : 17/06/2020
*******************************************************/
--exec settings_gl_code_map_modality_category_fetch
create procedure [dbo].[settings_gl_code_map_modality_category_fetch]

as
begin
		set nocount on
		create table #tmp
		(
			row_id int identity(1,1),
			category_id int,
			category_name nvarchar(30),
			modality_id int,
			modality_name nvarchar(30),
			gl_code nvarchar(5)
		)

		insert into #tmp(category_id,category_name,modality_id,modality_name,gl_code)
		(select category_id,category_name=sc.name,modality_id,modality_name=m.name,
		       mgcl.gl_code
		from modality_gl_code_link mgcl
		inner join sys_study_category sc on sc.id = mgcl.category_id
		inner join modality m on m.id = mgcl.modality_id)
		order by category_name,modality_name
	

		select * from #tmp

		drop table #tmp
		set nocount off
end

GO
