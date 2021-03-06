USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[hk_study_status_audit_trail_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[hk_study_status_audit_trail_fetch]
GO
/****** Object:  StoredProcedure [dbo].[hk_study_status_audit_trail_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : hk_study_status_audit_trail_fetch : fetch status
                  auidit trail of a study
** Created By   : Pavel Guha
** Created On   : 28/05/2019
*******************************************************/
--exec hk_study_status_audit_trail_fetch 'F46533DF-79B1-49ED-AE66-064351289289'
create PROCEDURE [dbo].[hk_study_status_audit_trail_fetch] 
	@id uniqueidentifier
as
begin
	set nocount on

	
	select sl.status_id_from,from_status=ssp1.status_desc,sl.status_id_to,to_status=ssp2.status_desc,
	       sl.date_updated,updated_by = isnull(u.name,'System')
	from sys_case_study_status_log sl
	inner join sys_study_status_pacs ssp1 on ssp1.status_id = sl.status_id_from
	inner join sys_study_status_pacs ssp2 on ssp2.status_id = sl.status_id_to
	left outer join users u on u.id= sl.updated_by
	where sl.study_id = @id
	order by sl.date_updated

	set nocount off
end


GO
