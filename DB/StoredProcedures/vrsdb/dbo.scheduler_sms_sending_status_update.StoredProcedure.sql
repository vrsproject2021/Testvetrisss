USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_sms_sending_status_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_sms_sending_status_update]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_sms_sending_status_update]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_sms_sending_status_update : update
                  SMS sending text
** Created By   : Pavel Guha
** Created On   : 01/05/2019
*******************************************************/
--exec scheduler_settings_fetch
CREATE procedure [dbo].[scheduler_sms_sending_status_update]
	@sms_log_id uniqueidentifier= '00000000-0000-0000-0000-000000000000',
	@message_sid nvarchar(200) = '',
	@processed nchar(1) ,
	@error_msg nvarchar(500)='' output,
	@return_type int=0 output
as
begin
	declare @study_hdr_id uniqueidentifier,
			@study_uid nvarchar(100),
			@recipient_no nvarchar(100),
			@activity_text nvarchar(max),
			@sms_type nvarchar(10)

	if (@sms_log_id <>'00000000-0000-0000-0000-000000000000')
	 begin
		begin transaction

		update vrslogdb..sms_log
		set sms_processed     = @processed, 
		    sms_processed_time= getdate(),
			message_sid       = @message_sid,
			attempts          =  attempts + 1
		where sms_log_id=@sms_log_id

		if(@@rowcount=0)
			begin
				rollback transaction
				select @error_msg='Failed to update sms processing status. SMS Log ID  :'+ CONVERT(nvarchar(36),@sms_log_id),
				       @return_type=0
				return 0
			end

	   select @sms_type = sms_type from vrslogdb..sms_log where sms_log_id = @sms_log_id

       if(@processed='Y' and @sms_type='RPT')
		begin
			select  @study_hdr_id      = study_hdr_id,
					@study_uid         = study_uid,
					@recipient_no     = recipient_no
			from vrslogdb..sms_log
			where sms_log_id=@sms_log_id

			if(select upper(sms_text) from vrslogdb..sms_log where sms_log_id = @sms_log_id) like '%FINAL REPORT AVAILABLE FOR%'
				begin
						
						set @activity_text = 'Final report text sent to ' + @recipient_no

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
								select @return_type=0,@error_msg='Failed to create activity log (sms) of final report release of ' + @study_uid + '.'
								return 0
							end

					end
			else if(select upper(sms_text) from vrslogdb..sms_log where sms_log_id = @sms_log_id) like '%PRELIMINARY REPORT AVAILABLE FOR%'
				begin
						
						set @activity_text = 'Preliminary report text sent to ' + @recipient_no

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
								select @return_type=0,@error_msg='Failed to create activity log (sms) of final report release of ' + @study_uid + '.'
								return 0
							end

					end
				
		end


	
		commit transaction
		select @error_msg='',@return_type=1
		return 1
	 end

end

GO
