USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_billing_account_fetch_dtls]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_billing_account_fetch_dtls : fetch 
                  billing account details
** Created By   : Pavel Guha
** Created On   : 22/10/2019
*******************************************************/
--exec master_billing_account_fetch_dtls '00000000-0000-0000-0000-000000000000',42,'11111111-1111-1111-1111-111111111111','',0
CREATE procedure [dbo].[master_billing_account_fetch_dtls]
    @id uniqueidentifier,
    @menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	 set nocount on

	 declare @country_id int

	select code                = isnull(ba.code,''),ba.name,
		   address_1           = isnull(ba.address_1,''),
		   address_2           = isnull(ba.address_2,''),
		   city                = isnull(ba.city,''),
	       state_id            = isnull(ba.state_id,0),
		   state_name          = isnull(s.name,''),
		   country_id		   = isnull(ba.country_id,0),
		   coutry_name         = isnull(c.name,''),
		   country_id          = isnull(ba.country_id,0),
		   zip                 = isnull(ba.zip,''),
	       email_id            = isnull(ba.email_id,''),
		   phone_no            = isnull(ba.phone_no,''),
		   fax_no              = isnull(ba.fax_no,''),
	       contact_person_name = isnull(ba.contact_person_name,''),contact_person_mobile= isnull(ba.contact_person_mobile,''),contact_person_email_id= isnull(ba.contact_person_email_id,''),
		   salesperson_id      = isnull(ba.salesperson_id,'00000000-0000-0000-0000-000000000000'),
		   commission_1st_yr   = isnull(ba.commission_1st_yr,0),
		   commission_2nd_yr   = isnull(ba.commission_2nd_yr,0),
		   ba.login_id,
		   ba.login_pwd,
		   user_email_id       = isnull(ba.user_email_id,''),
		   user_mobile_no      = isnull(ba.user_mobile_no,''),
		   notification_pref   = isnull(ba.notification_pref,'B'),
		   accountant_name     = isnull(ba.accountant_name,''),
		   discount_per        = isnull(ba.discount_per,0),
	       ba.is_active
	from billing_account ba 
	left outer join sys_states s on s.id = ba.state_id
	left outer join sys_country c on c.id = ba.country_id
	where ba.id=@id

	select @country_id = country_id
	from billing_account
	where id=@id

	if(isnull(@country_id,0))=0
		begin
			select @country_id=id
			from sys_country
			where is_default='Y'
		end
	
	
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
	select id,name from salespersons where is_active='Y' order by name
	
		
	set nocount off
end

GO
