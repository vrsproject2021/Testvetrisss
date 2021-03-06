USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_notification_unassigned_study_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_notification_unassigned_study_fetch]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_notification_unassigned_study_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_notification_unassigned_study_fetch : 
                  fetch unassigned studies
** Created By   : Pavel Guha
** Created On   : 24/05/2021
*******************************************************/
create procedure [dbo].[scheduler_notification_unassigned_study_fetch]
as
begin
	set nocount on 

	declare @UNSNTHTSTD int

	if(select data_type_string from  general_settings where control_code='SCHCASVCENBL')='Y'
		begin
			select @UNSNTHTSTD = data_type_number from general_settings where control_code= 'SCHCASVCENBL'
			if(@UNSNTHTSTD)>0
				begin
					if(select data_type_string from  general_settings where control_code='ASNSTATENB')='Y'
						begin
							select sh.id,sh.study_uid,sh.status_last_updated_on,sh.priority_id,p.is_stat
							from study_hdr sh
							inner join sys_priority p on p.priority_id = sh.priority_id
							where sh.study_status_pacs=50
							and isnull(sh.dict_radiologist_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000'
							and isnull(sh.radiologist_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000'
							and datediff(mi,sh.status_last_updated_on,getdate())>=@UNSNTHTSTD
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
				            and datediff(mi,sh.status_last_updated_on,getdate())>=@UNSNTHTSTD
							order by sh.status_last_updated_on
						end
				end
			else
				begin
					select id,study_uid,status_last_updated_on,priority_id from study_hdr where id ='00000000-0000-0000-0000-000000000000'
				end
		end
	else
		begin
			select id,study_uid,status_last_updated_on,priority_id from study_hdr where id ='00000000-0000-0000-0000-000000000000'
		end

	set nocount off
	
end



GO
