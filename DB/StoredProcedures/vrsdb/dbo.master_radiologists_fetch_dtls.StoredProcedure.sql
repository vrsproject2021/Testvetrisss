USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologists_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_radiologists_fetch_dtls]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologists_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_radiologists_fetch_dtls : fetch radiologists details
** Created By   : BK
** Created On   : 24/07/2019
*******************************************************/
--exec master_radiologists_fetch_dtls '3BE93540-F0C9-4947-9E46-7A7B37F8D9FD',1,'11111111-1111-1111-1111-111111111111','',0
CREATE procedure [dbo].[master_radiologists_fetch_dtls]
    @id uniqueidentifier,
    @menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	 set nocount on

	 declare @country_id int

	select code					= isnull(rd.code,''),
	       fname				= isnull(rd.fname,''),
		   lname				= isnull(rd.lname,''),
		   rd.name,
		   credentials			= isnull(rd.credentials,''),--Added on 12th SEP 2019 @BK
		   address_1			= isnull(rd.address_1,''),
		   address_2			= isnull(rd.address_2,''),
		   city					= isnull(rd.city,''),
	       state_id				= isnull(rd.state_id,0),
		   country_id			= isnull(rd.country_id,0),
		   zip					= isnull(rd.zip,''),
	       rd.email_id,
		   phone_no				= isnull(rd.phone_no,''),
		   mobile_no			= isnull(rd.mobile_no,''),
		   identity_color       = isnull(rd.identity_color,'#FFFFFF'),
	       timezone_id			= isnull(rd.timezone_id,isnull((select id from sys_us_time_zones where is_default='Y'),0)),
		   notification_pref    = isnull(rd.notification_pref,'B'),
		   login_user_id		= isnull(rd.login_user_id,'00000000-0000-0000-0000-000000000000'),
		   login_id				= isnull(rd.login_id,''),
		   login_pwd			= isnull(rd.login_pwd,''),
		   pacs_user_id  		= isnull(rd.pacs_user_id,''),
		   pacs_user_pwd		= isnull(rd.pacs_user_pwd,''),
		   signage              = isnull(rd.signage,''),
		   schedule_view        = isnull(rd.schedule_view,'O'),
		   notes                = isnull(rd.notes,''),
		   transcription_required = isnull(rd.transcription_required,'N'),
		   acct_group_id          = isnull(rd.acct_group_id,0),
		   assign_merged_study    = isnull(rd.assign_merged_study,'N'),
		   max_wu_per_hr          = isnull(rd.max_wu_per_hr,0),
	       rd.is_active 
	from radiologists rd 
	where id=@id

	select @country_id = country_id
	from radiologists 
	where id=@id
	
	
	if(@id<>'00000000-0000-0000-0000-000000000000')
		begin
			
				if(select count(record_id) from sys_record_lock_ui where record_id=@id and menu_id=@menu_id)=0
					begin
						exec common_lock_record_ui
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

	select id,name from sys_country  order by name
	select id,name from sys_states where country_id=@country_id order by name
	select id,name from users where is_active = 'Y' and id not in ('11111111-1111-1111-1111-111111111111','22222222-2222-2222-2222-222222222222') order by name
	select id,name from sys_radiologist_group order by display_order
	select id,
			case
				  when gmt_diff<0 then name + ' (GMT' + convert(varchar(10) ,(gmt_diff_mins/60) ) + ':' + replicate('0',2-len(convert(varchar(10) ,(abs(gmt_diff_mins%60))))) + convert(varchar(10) ,(abs(gmt_diff_mins%60))) + ')'
				      else name + ' (GMT+' + convert(varchar(10) ,(gmt_diff_mins/60) ) + ':' + replicate('0',2-len(convert(varchar(10) ,(abs(gmt_diff_mins%60))))) + convert(varchar(10) ,(abs(gmt_diff_mins%60))) + ')'
			end name,
		   is_default 
	from sys_us_time_zones 
	order by gmt_diff,name
		
	set nocount off
end

GO
