USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_process_mail_param_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_process_mail_param_fetch]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_process_mail_param_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_process_mail_param_fetch : 
                  fetch invoicing mail parameters 
				  if locked by other user
** Created By   : Pavel Guha 
** Created On   : 12/11/2019
*******************************************************/

CREATE procedure [dbo].[invoicing_process_mail_param_fetch]
	@billing_cycle_id uniqueidentifier,
	@billing_account_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@institution_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@mail_type nchar(1),
	@user_id uniqueidentifier
as
	begin
		set nocount on
		declare @due_date datetime

		select  @due_date = invoice_due_date
		from invoice_hdr
		where billing_account_id=@billing_account_id 
		and billing_cycle_id = @billing_cycle_id



		--billing cycle
		select name,
			   date_from,
			   date_till,
			   due_date = @due_date
		from billing_cycle
		where id = @billing_cycle_id

		-- mail to
		if(@mail_type ='A')
			begin
				select contact_person_email_id 
				from billing_account_contacts
				where billing_account_id = @billing_account_id
				and isnull(contact_person_email_id,'') <> ''
			end
		else
			begin
				select contact_person_email_id  = isnull(email_id,'')
				from institutions
				where id = @institution_id
			end

		-- mail cc
		select email_id= isnull(email_id,'')
		from salespersons
		where id = isnull((select salesperson_id from billing_account where id= @billing_account_id),'00000000-0000-0000-0000-000000000000')
		union
		select email_id = data_value_char from invoicing_control_params where control_code = 'DEFCCMAILID'


		--sender name
		select name from users where id = @user_id
		
		--company
		select company_name     = (select dbo.InitCap(data_value_char) from invoicing_control_params where control_code ='COMPNAME'),
			   sender_email_id  = (select data_value_char from invoicing_control_params where control_code ='SENDMAILID'),
		       mail_server      = (select data_type_string from general_settings where control_code='MAILSVRNAME'),
			   mail_server_port = (select data_type_number from general_settings where control_code='MAILSVRPORT'),
		       mail_user_code   = (select data_value_char from invoicing_control_params where control_code ='SENDMAILID'),
			   mail_user_pwd    = (select data_value_char from invoicing_control_params where control_code ='SENDMAILPWD'),
			   mail_ssl_enabled = (select data_type_string from general_settings where control_code='MAILSSLENABLED'),
			   mail_text        = (select data_value_char from invoicing_control_params where control_code ='INVMAILTXT'),
		       login_url        = (select data_type_string from general_settings where control_code='VRSLOGINKURL')

		set nocount off
		
	end
GO
