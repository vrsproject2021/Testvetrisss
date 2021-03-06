USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_day_end_ungrouped_image_record_delete]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_day_end_ungrouped_image_record_delete]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_day_end_ungrouped_image_record_delete]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_day_end_ungrouped_image_record_delete : delete
                  umgrouped image file record
** Created By   : Pavel Guha
** Created On   : 04/05/2020
*******************************************************/
CREATE procedure [dbo].[scheduler_day_end_ungrouped_image_record_delete]
    @id uniqueidentifier,
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	set nocount on

	declare @file_name nvarchar(250)
	
	if(select count(id) from scheduler_img_file_downloads_ungrouped where id=@id)>0
		begin
			begin transaction

			insert into image_files_deleted(id,file_name,import_session_id,institution_id,institution_code,institution_name,date_downloaded,
			                                remarks,deleted_by,date_deleted)
			                        (select id,file_name,import_session_id,institution_id,institution_code,institution_name,date_downloaded,
									        'Received 3 days before.No action taken by institution','00000000-0000-0000-0000-000000000000',getdate()
									 from scheduler_img_file_downloads_ungrouped
									 where id=@id)
			if(@@rowcount =0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to log the deletion of the image file record of file ' + isnull(@file_name,'')
					return 0
				end

			select @file_name=file_name from scheduler_img_file_downloads_ungrouped where id = @id

			delete from scheduler_img_file_downloads_ungrouped where id = @id

			if(@@rowcount =0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to delete the image file record of file ' + isnull(@file_name,'')
					return 0
				end

			delete from scheduler_image_files_to_delete where id=@id
				
			commit transaction
		end
	
	select @return_type=1,@error_msg=''
	set nocount off
	return 1

end


GO
