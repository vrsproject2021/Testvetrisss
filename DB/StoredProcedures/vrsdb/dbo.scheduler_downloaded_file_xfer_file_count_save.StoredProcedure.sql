USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_downloaded_file_xfer_file_count_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_downloaded_file_xfer_file_count_save]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_downloaded_file_xfer_file_count_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_downloaded_file_xfer_file_count_save : 
                  save transfered file count
** Created By   : Pavel Guha
** Created On   : 29/07/2019
*******************************************************/
CREATE procedure [dbo].[scheduler_downloaded_file_xfer_file_count_save]
    @study_uid nvarchar(100),
	@institution_code nvarchar(5),
	@file_xfer_count int,
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	set nocount on

	declare @sent_to_pacs nchar(1),
	        @file_count int,
			@xfer_count int,
			@study_id uniqueidentifier

	begin transaction


	if(select count(study_uid) from scheduler_file_downloads where study_uid=@study_uid and institution_code = @institution_code)=0
		begin
			select @study_id = id,
				   @file_count = file_count,
			       @xfer_count = isnull(file_xfer_count,0)
			from scheduler_file_downloads
			where study_uid=@study_uid 
			and institution_code = @institution_code

			if(@file_count <> (@xfer_count + @file_xfer_count))
				begin
					set @sent_to_pacs = 'P' 
				end
			else if(@file_count = (@xfer_count + @file_xfer_count))
				begin
					set @sent_to_pacs = 'Y' 
				end

			update scheduler_file_downloads
			set file_xfer_count = @xfer_count + @file_xfer_count
			where id=@study_id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to update the transfered file count of Study UID ' + @study_uid
					return 0
				end

			update scheduler_file_downloads_dtls
			set sent_to_pacs    = @sent_to_pacs,
				date_sent       = getdate()
			where id=@study_id 
		  

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to update the transfered file count of Study UID ' + @study_uid
					return 0
				end
		end
	else
		begin

			
			rollback transaction
			select @return_type=0,@error_msg='Download record of Study UID ' + @study_uid + ' not found'
			return 0
				

		end

    commit transaction
	set @return_type=1
	set @error_msg=''

	set nocount off
	return 1

end


GO
