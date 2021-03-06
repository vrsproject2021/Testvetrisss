USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_case_notification_rule_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_case_notification_rule_fetch_dtls]
GO
/****** Object:  StoredProcedure [dbo].[settings_case_notification_rule_fetch_dtls]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_case_notification_rule_fetch_dtls : 
                   fetch case notification rule header
** Created By   : Pavel Guha
** Created On   : 01/10/2019
*******************************************************/
--exec settings_case_notification_rule_fetch_dtls 2,40,'11111111-1111-1111-1111-111111111111','',0
CREATE procedure [dbo].[settings_case_notification_rule_fetch_dtls]
    @id int,
    @menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	 set nocount on

	

	select hdr.rule_no,hdr.rule_desc,hdr.pacs_status_id,sp.status_desc,hdr.priority_id,p.priority_desc,
		   time_ellapsed_hr = replicate('0',2- len(convert(varchar,convert(int,hdr.time_ellapsed_mins/60)))) + convert(varchar,convert(int,hdr.time_ellapsed_mins/60)),
		   time_ellapsed_mins = replicate('0',2-len(convert(varchar,(hdr.time_ellapsed_mins - ((convert(int,hdr.time_ellapsed_mins/60))*60))))) + convert(varchar,(hdr.time_ellapsed_mins - ((convert(int,hdr.time_ellapsed_mins/60))*60))),
		   time_left_hr = replicate('0',2- len(convert(varchar,convert(int,hdr.time_left_mins/60)))) + convert(varchar,convert(int,hdr.time_left_mins/60)),
		   time_left_mins = replicate('0',2-len(convert(varchar,(hdr.time_left_mins - ((convert(int,hdr.time_left_mins/60))*60))))) + convert(varchar,(hdr.time_left_mins - ((convert(int,hdr.time_left_mins/60))*60))),
	       notify_by_time = isnull(hdr.notify_by_time,'E'),
		   hdr.is_active
	from case_notification_rule_hdr hdr  
	inner join sys_study_status_pacs sp on sp.status_id = hdr.pacs_status_id
	inner join sys_priority p on p.priority_id = hdr.priority_id
	where rule_no=@id

	
	
	if(@id>0)
		begin
			
				if(select count(record_id) from sys_record_lock where record_id=@id and menu_id=@menu_id)=0
					begin
						exec common_lock_record
							@menu_id       = @menu_id,
							@record_id     = @id,
							@user_id       = @user_id,
							@error_code    = @error_code output,
							@return_status = @return_status output	
						
						if(@return_status=0)
							begin
								return 0
							end
					end
				
		end
    else
		begin
			if(select count(record_id) from sys_record_lock_ui where user_id=@user_id)>0
			    begin
				  delete from sys_record_lock_ui where user_id=@user_id
				  delete from sys_record_lock where user_id=@user_id
			    end
		end

	select status_id,status_desc from sys_study_status_pacs order by status_id
	select priority_id,priority_desc from sys_priority where is_active = 'Y' order by priority_id
		
	set nocount off
end

GO
