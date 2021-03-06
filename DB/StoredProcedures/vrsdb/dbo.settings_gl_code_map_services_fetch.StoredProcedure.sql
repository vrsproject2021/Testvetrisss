USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_gl_code_map_services_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_gl_code_map_services_fetch]
GO
/****** Object:  StoredProcedure [dbo].[settings_gl_code_map_services_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_gl_code_map_services_fetch : 
				  fetch g/l code mapping with services
** Created By   : Pavel Guha
** Created On   : 17/06/2020
*******************************************************/
--exec settings_gl_code_map_services_fetch
CREATE procedure [dbo].[settings_gl_code_map_services_fetch]

as
begin
		set nocount on
		
		create table #tmp
		(
			row_id int identity(1,1),
			service_id int,
			modality_id int,
			gl_code_default nvarchar(5),
			gl_code_after_hrs nvarchar(5)
		)

		insert into #tmp(service_id,modality_id,gl_code_default,gl_code_after_hrs)
		(select sgl.service_id,modality_id = sgl.modality_id,isnull(sgl.gl_code_default,''),isnull(sgl.gl_code_after_hrs,'')
		from service_gl_code_link sgl
		inner join services s on s.id = sgl.service_id
		left outer join modality m on m.id = sgl.modality_id
		where s.is_active='Y') 
		order by s.name

		select * from #tmp

		drop table #tmp

		set nocount off
end

GO
