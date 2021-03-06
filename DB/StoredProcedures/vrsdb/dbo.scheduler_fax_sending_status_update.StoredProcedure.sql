USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_fax_sending_status_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_fax_sending_status_update]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_fax_sending_status_update]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_fax_sending_status_update : update
                  fax sending text
** Created By   : Pavel Guha
** Created On   : 03/02/2020
*******************************************************/
CREATE procedure [dbo].[scheduler_fax_sending_status_update]
	@id uniqueidentifier= '00000000-0000-0000-0000-000000000000',
	@processed nchar(1) ,
	@error_msg nvarchar(500)='' output,
	@return_type int=0 output
as
begin
	declare @study_hdr_id uniqueidentifier,
			@study_uid nvarchar(100),
			@recipient_no nvarchar(100),
			@activity_text nvarchar(max),
			@fax_type nvarchar(10),
			@report_type nvarchar(1)

	if (@id <>'00000000-0000-0000-0000-000000000000')
	 begin
		begin transaction

		update vrslogdb..fax_log
		set fax_sent     = @processed, 
		    fax_sent_time= getdate()
		where id=@id

		if(@@rowcount=0)
			begin
				rollback transaction
				select @error_msg='Failed to update fax processing status. Fax Log ID  :'+ CONVERT(nvarchar(36),@id),
				       @return_type=0
				return 0
			end

		select @fax_type    = fax_type,
		       @report_type = isnull(report_type,'')
		from vrslogdb..fax_log 
		where id = @id

       if(@processed='Y' and @fax_type='RPT')
		begin
			
				select @study_hdr_id      = study_hdr_id,
						@study_uid         = study_uid,
						@recipient_no     = recipient_no
				from vrslogdb..fax_log
				where id=@id

				if( @report_type='F')
					begin
							set @activity_text = 'Final report faxed to ' + @recipient_no

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
									select @return_type=0,@error_msg='Failed to create activity log (fax) of final report release of ' + @study_uid + '.'
									return 0
								end
					end
				else if( @report_type='P')
					begin
							set @activity_text = 'Preliminary report faxed to ' + @recipient_no

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
									select @return_type=0,@error_msg='Failed to create activity log (fax) of final report release of ' + @study_uid + '.'
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
