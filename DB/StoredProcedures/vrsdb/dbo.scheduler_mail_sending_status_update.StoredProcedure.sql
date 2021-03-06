USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_mail_sending_status_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_mail_sending_status_update]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_mail_sending_status_update]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 
** Procedure    : scheduler_settings_fetch : fetch scheduler settings
** Created By   : Pavel Guha
** Created On   : 21/04/2019
*******************************************************/
--exec scheduler_settings_fetch
CREATE procedure [dbo].[scheduler_mail_sending_status_update]
	@email_log_id uniqueidentifier= '00000000-0000-0000-0000-000000000000',
	@error_msg nvarchar(500)='' output,
	@return_type int=0 output
as
begin
	set nocount on

	declare @email_type nvarchar(10),
	        @invoice_hdr_id uniqueidentifier,
			@study_hdr_id uniqueidentifier,
			@study_uid nvarchar(100),
			@recipient_address nvarchar(500),
			@activity_text nvarchar(max)

	if (@email_log_id <>'00000000-0000-0000-0000-000000000000')
	 begin
		begin transaction

		update vrslogdb..email_log
		set email_processed='Y', 
		    email_processed_time=getdate()
		where email_log_id=@email_log_id

		if(@@rowcount=0)
			begin
				rollback transaction
				select @error_msg='Failed to send mail. Email Log ID  :'+ CONVERT(nvarchar(36),@email_log_id),
				       @return_type=0
				return 0
			end

		select @email_type = email_type from vrslogdb..email_log where email_log_id = @email_log_id

		if(isnull(@email_type,'')= 'ACCTINV')
			begin
				select @invoice_hdr_id = isnull(invoice_hdr_id,'00000000-0000-0000-0000-000000000000')
				from vrslogdb..email_log 
				where email_log_id = @email_log_id

				if (@invoice_hdr_id <>'00000000-0000-0000-0000-000000000000')
					begin
						if(select count(id) from invoice_hdr where id=@invoice_hdr_id)>0
							begin
								update invoice_hdr
								set pick_for_mail='N'
								where id = @invoice_hdr_id

								if(@@rowcount=0)
									begin
										rollback transaction
										select @error_msg='Failed to update mail sending status of invoice. Email Log ID  :'+ CONVERT(nvarchar(36),@email_log_id),
											   @return_type=0
										return 0
									end
							end
					end
			end
		else if(isnull(@email_type,'')= 'RPT')
			begin
				select @study_hdr_id      = study_hdr_id,
						@study_uid         = study_uid,
						@recipient_address = recipient_address
				from vrslogdb..email_log
				where email_log_id=@email_log_id

				if(select upper(email_subject) from vrslogdb..email_log where email_log_id = @email_log_id) like '%FINAL REPORT AVAILABLE FOR%'
					begin
						set @activity_text = 'Final report mailed to ' + @recipient_address

						exec common_study_user_activity_trail_save
							@study_hdr_id = @study_hdr_id,
							@study_uid    = @study_uid,
							@menu_id      = 0,
							@activity_text = @activity_text,
							@activity_by   = '00000000-0000-0000-0000-000000000000',
							@error_code    = @error_msg output,
							@return_status = @return_type output

						if(@return_type=0)
							begin
								rollback transaction
								select @return_type=0,@error_msg='Failed to create activity log (email) of final report release of ' + @study_uid + '.'
								return 0
							end
					end
				else if(select upper(email_subject) from vrslogdb..email_log where email_log_id = @email_log_id) like '%PRELIMINARY REPORT AVAILABLE FOR%'
					begin
						set @activity_text = 'Preliminary report mailed to ' + @recipient_address

						exec common_study_user_activity_trail_save
							@study_hdr_id = @study_hdr_id,
							@study_uid    = @study_uid,
							@menu_id      = 0,
							@activity_text = @activity_text,
							@activity_by   = '00000000-0000-0000-0000-000000000000',
							@error_code    = @error_msg output,
							@return_status = @return_type output

						if(@return_type=0)
							begin
								rollback transaction
								select @return_type=0,@error_msg='Failed to create activity log (email) of final report release of ' + @study_uid + '.'
								return 0
							end
					end
			end
	
		commit transaction
		
	 end

	set nocount off
	select @error_msg='',@return_type=1
	return 1

end

GO
