USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_notification_invoice_sending_create]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_notification_invoice_sending_create]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_notification_invoice_sending_create]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_case_study_notification_create : 
                  create invoice sending notification
** Created By   : Pavel Guha
** Created On   : 26/05/2020
*******************************************************/
--exec scheduler_notification_invoice_sending_create '54EADB22-EAF3-40C9-8E62-8BE7C0BEADD8','586953D4-2B3D-44DD-9349-B1C1B2701246','BillingAccountInvoice_Animal_Medical_center_-_Spay_and_Neuter_00651_AUGUST2020.pdf','A54E0B66-B4D6-45A5-860D-A4B39CAA3559','',0

CREATE procedure [dbo].[scheduler_notification_invoice_sending_create]
	@billing_account_id uniqueidentifier,
	@billing_cycle_id uniqueidentifier,
	@file_name nvarchar(4000)= null,
	@invoice_hdr_id uniqueidentifier,
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
   set nocount on
 
   declare  @email_subject nvarchar(250),
		    @email_text varchar(8000)
		    
   declare @SENDMAILID nvarchar(200),
           @SENDMAILPWD nvarchar(200),
		   @DEFCCMAILID nvarchar(200),
		   @COMPNAME nvarchar(200),
		   @MAILSVRUSRCODE nvarchar(200),
		   @MAILSVRUSRPWD nvarchar(200),
		   @SUPPMAILID nvarchar(200),
		   @INVMAILTXT nvarchar(2000),
		   @VRSLOGINKURL nvarchar(200)

   declare @acct_user_id nvarchar(100),
		   @user_email_id nvarchar(100),
		   @recepient_name nvarchar(100),
		   @billing_cycle_name nvarchar(50),
		   @billing_account_name nvarchar(100),
		   @billing_account_code nvarchar(5),
		   @dtFrom nvarchar(30),
		   @dtTill nvarchar(30),
		   @dtDue nvarchar(30),
		   @invoice_no nvarchar(30),
		   @invoice_date datetime


   select @acct_user_id = isnull(login_user_id,'00000000-0000-0000-0000-000000000000'),
          @recepient_name = isnull(login_id,'')
   from billing_account
   where id = @billing_account_id

   select @user_email_id   = email_id
   from users 
   where id = @acct_user_id

   begin transaction
  print @user_email_id
  if(isnull(@user_email_id,'')<>'')
	begin
		select @SENDMAILID = data_value_char from invoicing_control_params where control_code ='SENDMAILID'
	    select @SENDMAILPWD = data_value_char from invoicing_control_params where control_code ='SENDMAILPWD'
	    select @DEFCCMAILID = data_value_char from invoicing_control_params where control_code ='DEFCCMAILID'
	    select @COMPNAME = data_value_char from invoicing_control_params where control_code ='COMPNAME'
		select @INVMAILTXT = data_value_char from invoicing_control_params where control_code ='INVMAILTXT'
		select @VRSLOGINKURL = data_type_string from general_settings where control_code='VRSLOGINKURL'

		select @billing_cycle_name = name from billing_cycle where id= @billing_cycle_id
		select @dtFrom = convert(varchar,date_from,107) from billing_cycle where id= @billing_cycle_id
		select @dtTill = convert(varchar,date_till,107) from billing_cycle where id= @billing_cycle_id
		select @dtDue = convert(varchar,invoice_due_date,107) from invoice_hdr where billing_cycle_id= @billing_cycle_id and billing_account_id=@billing_account_id

		select @invoice_no =  invoice_no,
		       @invoice_date = invoice_date
		from invoice_hdr
		where id = @invoice_hdr_id

		set @email_subject = dbo.InitCap(@COMPNAME)  + ' : Invoice for ' + @billing_cycle_name + ' (from ' + @dtFrom + ' to ' + @dtTill + ')'
		

		--set @email_text    = 'Dear Valued Client,<br/><br/>'
		--set @email_text    = @email_text + ' Please find attached Invoice for the period ' +  @dtFrom + ' to ' + @dtTill 
		--set @email_text    = @email_text + ' <br/><br/><br/>'
		--set @email_text    = @email_text + ' Regards,<br/>'
		--set @email_text    = @email_text + ' VETS CHOICE RADIOLOGY <br/>'
		set @email_text = @INVMAILTXT
		set @email_text = replace(@email_text,'[FROM_DATE]',@dtFrom)
		set @email_text = replace(@email_text,'[TILL_DATE]',@dtTill)
		set @email_text = replace(@email_text,'[DUE_DATE]',@dtDue)
		set @email_text = replace(@email_text,'[PAYMENT_URL]',@VRSLOGINKURL +'?aid=' + convert(varchar(36),@billing_account_id))
		

		insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,cc_address,email_subject,email_text,
							  invoice_hdr_id,file_name,email_type,sender_email_address,sender_email_password)
				      values(newid(),getdate(),@user_email_id,@recepient_name,@DEFCCMAILID,@email_subject,@email_text,
				             @invoice_hdr_id,@file_name,'ACCTINV',@SENDMAILID,@SENDMAILPWD)

		if(@@rowcount =0)
			begin
				rollback transaction
				select @return_type=0,@error_msg='Failed to create notification for invoice # ' + @invoice_no + ' dtd ' + convert(varchar(11),@invoice_date,106)
				return 0
			end

		update invoice_hdr 
		set pick_for_mail='N' 
		where id=@invoice_hdr_id

		if(@@rowcount =0)
			begin
				rollback transaction
				select @return_type=0,@error_msg='Failed to update mail sending status for invoice # ' + @invoice_no + ' dtd ' + convert(varchar(11),@invoice_date,106)
				return 0
			end
		
	end
  else
	begin
		if(select count(email_log_id) from vrslogdb..email_log where invoice_hdr_id=@invoice_hdr_id and email_type='CONFBA')=0
			begin
				select @MAILSVRUSRCODE = data_type_string from general_settings where control_code ='MAILSVRUSRCODE'
				select @MAILSVRUSRPWD = data_type_string from general_settings where control_code ='MAILSVRUSRPWD'
				select @SUPPMAILID = data_type_string from general_settings where control_code ='SUPPMAILID'

				select @billing_account_name = name,
					   @billing_account_code = code
				from billing_account
				where id = @billing_account_id

				set @email_subject = 'Billing account update required'
				set @email_text    = '<br/>'
				set @email_text    = @email_text + ' Configuration of billing account <b>' +  @billing_account_name + ' (' + @billing_account_code + ')</b> is pending.<br/>' 
				set @email_text    = @email_text + ' Please update the mandatory fields and login credentials of this billing account.'
				set @email_text    = @email_text + ' <br/><br/>'
				set @email_text    = @email_text + ' This is an automated message from VETRIS.Please do not reply to the message. <br/>'

				insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,email_subject,email_text,invoice_hdr_id,email_type,sender_email_address,sender_email_password)
							   values(newid(),getdate(),@SUPPMAILID,'RAD Support',@email_subject,@email_text,@invoice_hdr_id,'CONFBA',@MAILSVRUSRCODE,@MAILSVRUSRPWD)

				if(@@rowcount = 0)
					begin
						rollback transaction
						set @error_msg='Failed to create notification to update billing account '+  @billing_account_name + ' (' + @billing_account_code + ')'
						set @return_type=0
						return 0
					end
		    end
	end


	commit transaction
	select @return_type=1,@error_msg=''
	
	set nocount off
	return 1
end

GO
