USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_transfered_file_count_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_transfered_file_count_update]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_transfered_file_count_update]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_transfered_file_count_update : 
                  finalise downloaded file details
** Created By   : Pavel Guha
** Created On   : 02/08/2019
*******************************************************/
CREATE procedure [dbo].[scheduler_transfered_file_count_update]
	@id uniqueidentifier,
	@file_name nvarchar(250),
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	set nocount on

	declare @xfer_file_count int,
			@file_count int,
	        @study_uid nvarchar(100),
			@is_manual nchar(1),
			@received_via_dicom_router nchar(1),
			@diff int,
			@activity_text nvarchar(max),
			@error_code nvarchar(10),
			@return_status int

	begin transaction 

	select @is_manual = is_manual from scheduler_file_downloads where id=@id

	update scheduler_file_downloads_dtls
	set sent_to_pacs='Y',
	    date_sent   = getdate()
	where id = @id
	and file_name = @file_name

	if(@@rowcount=0)
		begin
			rollback transaction
			select @return_type=0,@error_msg='Failed to update sent to PACS of file ' + @file_name
			return 0
		end


	select @xfer_file_count = count(file_name)
	from scheduler_file_downloads_dtls
	where sent_to_pacs='Y'
	and id = @id

	select @study_uid = study_uid,
	       @file_count = file_count
	from scheduler_file_downloads
	where id = @id

	select @received_via_dicom_router = received_via_dicom_router
	from study_hdr
	where id = @id 

	if((@received_via_dicom_router = 'Y') or (@received_via_dicom_router = 'M'))
		begin
			update study_hdr
			set   received_via_dicom_router ='N'
			where id = @id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to update files received mode flag Study UID ' + @study_uid
					return 0
				end

		end

	if((@xfer_file_count>0) and (@received_via_dicom_router = 'M'))
		begin
			update study_hdr
			set   pacs_wb ='Y'
			where id = @id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to update PACS write back flag Study UID ' + @study_uid
					return 0
				end
		end

	update scheduler_file_downloads
	set file_xfer_count = @xfer_file_count
	where id = @id

	if(@@rowcount=0)
		begin
			rollback transaction
			select @return_type=0,@error_msg='1-Failed to update count of transfered file(s) to PACS of Study UID ' + @study_uid
			return 0
		end

	if(@xfer_file_count > @file_count)
		begin
			update scheduler_file_downloads
			set file_count = @xfer_file_count
			where id = @id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to update count of recived file(s) to PACS of Study UID ' + @study_uid
					return 0
				end
		end
	
	if(@xfer_file_count =1)
		begin
			select @error_code='',@return_status=0
			set @activity_text= 'Transfer of files to PACS started'
			exec common_study_user_activity_trail_save
				@study_hdr_id  = @id,
				@study_uid     = @study_uid,
				@menu_id       = 0,
				@activity_text = @activity_text,
				@activity_by   = '00000000-0000-0000-0000-000000000000',
				@error_code    = @error_code output,
				@return_status = @return_status output

			if(@return_status=0)
				begin
					rollback transaction
					return 0
				end

			  
		 end

	if(@xfer_file_count>0)
		begin
			  update study_hdr
			  set object_count = @xfer_file_count
			  where id=@id

			  if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='2-Failed to update count of transfered file(s) to PACS of Study UID ' + @study_uid
					return 0
				end
		end


	select @diff =  object_count_pacs - object_count from study_hdr where id = @id
	if(@diff>=0 and  @diff<=3)
		begin
			select @error_code='',@return_status=0
			set @activity_text= 'Transfer of files to PACS completed, count variance : ' + convert(varchar,@diff)
			exec common_study_user_activity_trail_save
				@study_hdr_id  = @id,
				@study_uid     = @study_uid,
				@menu_id       = 0,
				@activity_text = @activity_text,
				@activity_by   = '00000000-0000-0000-0000-000000000000',
				@error_code    = @error_code output,
				@return_status = @return_status output

			if(@return_status=0)
				begin
					rollback transaction
					return 0
				end
		end


	commit transaction
	set @return_type=1
	set @error_msg=''
	set nocount off
	return 1

end


GO
