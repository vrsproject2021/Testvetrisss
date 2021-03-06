USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_transciptionists_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_transciptionists_fetch_dtls]
GO
/****** Object:  StoredProcedure [dbo].[master_transciptionists_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_transciptionists_fetch_dtls : fetch transciptionists details
** Created By   : BK
** Created On   : 16/09/2020
*******************************************************/
--exec master_transciptionists_fetch_dtls '3BE93540-F0C9-4947-9E46-7A7B37F8D9FD',1,'11111111-1111-1111-1111-111111111111','',0
CREATE procedure [dbo].[master_transciptionists_fetch_dtls]
    @id uniqueidentifier,
    @menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	 set nocount on

	 declare @country_id int

	select code					= isnull(trn.code,''),
	       fname				= isnull(trn.fname,''),
		   lname				= isnull(trn.lname,''),
		   trn.name,
		   address_1			= isnull(trn.address_1,''),
		   address_2			= isnull(trn.address_2,''),
		   city					= isnull(trn.city,''),
	       state_id				= isnull(trn.state_id,0),
		   country_id			= isnull(trn.country_id,0),
		   zip					= isnull(trn.zip,''),
	       trn.email_id,
		   phone_no				= isnull(trn.phone_no,''),
		   mobile_no			= isnull(trn.mobile_no,''),
		   notification_pref    = isnull(trn.notification_pref,'B'),
		   login_user_id		= isnull(trn.login_user_id,'00000000-0000-0000-0000-000000000000'),
		   login_id				= isnull(trn.login_id,''),
		   login_pwd			= isnull(trn.login_pwd,''),
		   notes                = isnull(trn.notes,''),
	       trn.is_active 
	from transciptionists trn 
	where id=@id

	select @country_id = country_id
	from transciptionists 
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

	select id,name,is_default from sys_country  order by name
	select id,name from sys_states where country_id=@country_id order by name
		
	set nocount off
end

GO
