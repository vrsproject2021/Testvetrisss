USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_salesperson_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_salesperson_fetch_dtls]
GO
/****** Object:  StoredProcedure [dbo].[master_salesperson_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_salesperson_fetch_dtls : fetch salesperson details
** Created By   : Pavel Guha
** Created On   : 21/05/2019
*******************************************************/
--exec master_salesperson_fetch_dtls '3BE93540-F0C9-4947-9E46-7A7B37F8D9FD',1,'11111111-1111-1111-1111-111111111111','',0
CREATE procedure [dbo].[master_salesperson_fetch_dtls]
    @id uniqueidentifier,
    @menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	 set nocount on

	 declare @country_id int

	select code=isnull(sp.code,''),
	       fname = isnull(sp.fname,''),
		   lname = isnull(sp.lname,''),
		   sp.name,
		   address_1 = isnull(sp.address_1,''),address_2=isnull(sp.address_2,''),city=isnull(sp.city,''),
	       state_id = isnull(sp.state_id,0),country_id=isnull(sp.country_id,0),zip=isnull(sp.zip,''),
	       sp.email_id,phone_no=isnull(sp.phone_no,''),mobile_no=isnull(sp.mobile_no,''),
		   notification_pref = isnull(sp.notification_pref,'B'),
		   login_id = isnull(sp.login_id,''),
		   login_pwd = isnull(sp.login_pwd,''),
		   pacs_user_id = isnull(sp.pacs_user_id,''),
		   pacs_password = isnull(sp.pacs_password,''),
	       sp.is_active 
	from salespersons sp 
	where id=@id

	select @country_id = country_id
	from salespersons 
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
	select id,name from institutions where is_active='Y' order by name
	select id,name from users where is_active = 'Y' and id not in ('11111111-1111-1111-1111-111111111111','22222222-2222-2222-2222-222222222222') order by name
		
	set nocount off
end

GO
