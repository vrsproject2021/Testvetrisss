USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_radiologist_assign_study_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_radiologist_assign_study_fetch]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_radiologist_assign_study_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_radiologist_assign_study_fetch : 
                  fetch studies to assign
** Created By   : Pavel Guha
** Created On   : 04/05/2021
*******************************************************/
CREATE procedure [dbo].[scheduler_radiologist_assign_study_fetch]
as
begin
	set nocount on 

	if(select data_type_string from  general_settings where control_code='ASNSTATENB')='Y'
		begin
			select sh.id,sh.study_uid,sh.status_last_updated_on,sh.priority_id,p.is_stat
			from study_hdr sh
			inner join sys_priority p on p.priority_id = sh.priority_id
			where sh.study_status_pacs=50
			and isnull(sh.dict_radiologist_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000'
			and isnull(sh.radiologist_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000'
			order by p.is_stat desc,sh.priority_id,sh.status_last_updated_on
		end
	else
		begin
			select sh.id,sh.study_uid,sh.status_last_updated_on
			from study_hdr sh
			inner join sys_priority p on p.priority_id = sh.priority_id
			where sh.study_status_pacs=50
			and isnull(sh.dict_radiologist_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000'
			and isnull(sh.radiologist_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000'
			and p.is_stat='N'
			and isnull(sh.merge_status,'N')='N'
			order by sh.status_last_updated_on
		end
	

	
	
	set nocount off
	
end



GO
