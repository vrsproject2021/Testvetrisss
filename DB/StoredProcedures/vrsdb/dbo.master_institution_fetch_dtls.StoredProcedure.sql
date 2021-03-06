USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_institution_fetch_dtls]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_institution_fetch_dtls : fetch case list header
** Created By   : Pavel Guha
** Created On   : 24/04/2019
*******************************************************/
--exec master_institution_fetch_dtls '3BE93540-F0C9-4947-9E46-7A7B37F8D9FD',1,'11111111-1111-1111-1111-111111111111','',0
CREATE procedure [dbo].[master_institution_fetch_dtls]
    @id uniqueidentifier,
    @menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	 set nocount on

	 declare @country_id int

	select code						= isnull(i.code,''),i.name,
		   address_1				= isnull(i.address_1,''),
		   address_2				= isnull(i.address_2,''),
		   city						= isnull(i.city,''),
	       state_id					= isnull(i.state_id,0),
		   state_name               = isnull(s.name,''),
		   country_id				= isnull(i.country_id,0),
		   coutry_name              = isnull(c.name,''),
		   zip						= isnull(i.zip,''),
	       email_id					= isnull(i.email_id,''),
		   phone_no					= isnull(i.phone_no,''),
		   mobile_no				= isnull(i.mobile_no,''),
	       contact_person_name		= isnull(i.contact_person_name,''),contact_person_mobile= isnull(i.contact_person_mobile,''),
		   notes					= isnull(i.notes,''),
		   salesperson_id			= isnull(spl.salesperson_id,'00000000-0000-0000-0000-000000000000'),
		   commission_1st_yr		= isnull(spl.commission_1st_yr,0),-- Added on 4th SEP 2019 @BK
		   commission_2nd_yr		= isnull(spl.commission_2nd_yr,0),-- Added on 4th SEP 2019 @BK
		   discount_per			    = isnull(i.discount_per,0),
		   accountant_name          = isnull(i.accountant_name,''),-- Added on 3rd SEP 2019 @BK
		   business_source_id       = isnull(i.business_source_id,0),
		   link_existing_bill_acct  = isnull(i.link_existing_bill_acct,'N'),
		   billing_account_id       = isnull(i.billing_account_id,'00000000-0000-0000-0000-000000000000'),
		   billing_acct_name        = isnull(ba.name,''),
		   i.format_dcm_files,
		   dcm_file_xfer_pacs_mode  = isnull(i.dcm_file_xfer_pacs_mode,'N'),
		   study_img_manual_receive_path = isnull(i.study_img_manual_receive_path,''),
		   consult_applicable       = isnull(i.consult_applicable,'N'),
		   storage_applicable       = isnull(i.storage_applicable,'N'),
		   custom_report            = isnull(i.custom_report,'N'),
		   i.logo_img,
		   image_content_type       = isnull(i.image_content_type,''),
		   xfer_files_compress      = isnull(i.xfer_files_compress,'N'),
		   fax_rpt                  = isnull(i.fax_rpt,'N'),
		   fax_no                   = isnull(i.fax_no,''),
		   rpt_format               = isnull(i.rpt_format,'P'),
	       i.is_active
	from institutions i 
	left outer join institution_salesperson_link spl on spl.institution_id = i.id
	left outer join billing_account ba on ba.id = i.billing_account_id
	left outer join sys_states s on s.id = i.state_id
	left outer join sys_country c on c.id = i.country_id
	where i.id=@id

	select @country_id = country_id
	from institutions 
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
	select id,name from physicians where is_active='Y' order by name
	select id,name from salespersons where is_active='Y' order by name
	select USRUPDURL = data_type_string from general_settings where control_code='USRUPDURL'
	select id,name from business_sources where is_active='Y' order by name
	select id,name from billing_account where is_active='Y' order by name
		
	set nocount off
end

GO
