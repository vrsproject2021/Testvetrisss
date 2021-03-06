USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_user_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_user_fetch_dtls]
GO
/****** Object:  StoredProcedure [dbo].[settings_user_fetch_dtls]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_user_fetch_dtls : fetch user details
** Created By   : Pavel Guha
** Created On   : 24/04/2019
*******************************************************/
--exec settings_user_fetch_dtls '3BE93540-F0C9-4947-9E46-7A7B37F8D9FD',1,'11111111-1111-1111-1111-111111111111','',0
CREATE procedure [dbo].[settings_user_fetch_dtls]
    @id uniqueidentifier,
    @menu_id int,
    @user_id uniqueidentifier,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	 set nocount on

	 declare @activity_text nvarchar(max)

	select u.code,u.name,u.email_id,contact_no=isnull(u.contact_no,''),u.user_role_id,user_role_code = ur.code,
		   login_id = isnull(u.login_id,''), password = isnull(u.password,''),
		   pacs_user_id=isnull(u.pacs_user_id,''),pacs_password=isnull(u.pacs_password,''),
		   u.allow_manual_submission,
		   u.allow_dashboard_view,
		   case 
		      when ur.code='IU' then isnull(i.name,'') else '' 
	       end institution_name,
	       case 
	         when ur.code='AU' then isnull(ba.name,'') else '' 
           end billing_account_name,
		   u.is_active 
	from users  u
	inner join user_roles ur on ur.id = u.user_role_id
	left outer join institution_user_link iul on iul.user_id = u.id
	left outer join institutions i on i.id = iul.institution_id
	left outer join billing_account ba on ba.login_user_id = u.id
	where u.id=@id

	
	if(@id<>'00000000-0000-0000-0000-000000000000')
		begin
			
				if(select count(record_id) from sys_record_lock_ui where record_id=@id and menu_id=@menu_id)=0
					begin
						exec common_lock_record_ui
							@menu_id       = @menu_id,
							@record_id     = @id,
							@user_id       = @user_id,
							@session_id    = @session_id,
							@error_code    = @error_code output,
							@return_status = @return_status output	
						
						if(@return_status=0)
							begin
								return 0
							end

						set @activity_text =  'Locked record => ' + (select name from users where id=@id)
						exec common_user_activity_log
							@user_id       = @user_id,
							@activity_text = @activity_text,
							@menu_id       = @menu_id,
							@session_id    = @session_id,
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

	select id,name from user_roles where is_visible='Y' order by name

		
	set nocount off
end

GO
