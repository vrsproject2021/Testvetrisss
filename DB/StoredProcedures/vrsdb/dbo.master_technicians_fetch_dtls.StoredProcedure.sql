USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_technicians_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_technicians_fetch_dtls]
GO
/****** Object:  StoredProcedure [dbo].[master_technicians_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_technicians_fetch_dtls : fetch technicians details
** Created By   : BK
** Created On   : 25/07/2019
*******************************************************/
--exec master_technicians_fetch_dtls '3BE93540-F0C9-4947-9E46-7A7B37F8D9FD',1,'11111111-1111-1111-1111-111111111111','',0
CREATE procedure [dbo].[master_technicians_fetch_dtls]
    @id uniqueidentifier,
    @menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	 set nocount on

	 declare @country_id int

	select code					=isnull(tec.code,''),
	       fname				= isnull(tec.fname,''),
		   lname				= isnull(tec.lname,''),
		   tec.name,
		   address_1			= isnull(tec.address_1,''),
		   address_2			=isnull(tec.address_2,''),
		   city					=isnull(tec.city,''),
	       state_id				= isnull(tec.state_id,0),
		   country_id			=isnull(tec.country_id,0),
		   zip					=isnull(tec.zip,''),
	       tec.email_id,
		   phone_no				=isnull(tec.phone_no,''),
		   mobile_no			=isnull(tec.mobile_no,''),
		   notification_pref    = isnull(tec.notification_pref,'B'),
		   login_user_id		=isnull(tec.login_user_id,'00000000-0000-0000-0000-000000000000'),
		   login_id				= isnull(tec.login_id,''),
		   login_pwd			= isnull(tec.login_pwd,''),
		   default_fee			= isnull(default_fee,0),
	       tec.is_active 
	from technicians tec 
	where id=@id

	select @country_id = country_id
	from technicians 
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
		
	set nocount off
end

GO
