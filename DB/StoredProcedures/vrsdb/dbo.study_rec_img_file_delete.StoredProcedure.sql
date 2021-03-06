USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[study_rec_img_file_delete]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[study_rec_img_file_delete]
GO
/****** Object:  StoredProcedure [dbo].[study_rec_img_file_delete]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : study_rec_img_file_delete : delete image file 
                  record
** Created By   : Pavel Guha
** Created On   : 26/09/2019
*******************************************************/
-- exec study_rec_img_file_delete '8372ee06-b7bd-4d4b-ab6a-0128f742b108'
CREATE PROCEDURE [dbo].[study_rec_img_file_delete] 
	@id uniqueidentifier,
	@updated_by uniqueidentifier,
	@menu_id int,
	@user_name nvarchar(500)='' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	set nocount on

	declare	@file_name nvarchar(250),
	        @remarks nvarchar(20)

	exec common_check_record_lock
		@menu_id       = @menu_id,
		@record_id     = @menu_id,
		@user_id       = @updated_by,
		@user_name     = @user_name output,
		@error_code    = @error_code output,
		@return_status = @return_status output

	if(@return_status=0)
		begin
			return 0
		end


	if(select count(id) from scheduler_img_file_downloads_ungrouped where id=@id)>0
		begin
			begin transaction	

			select @user_name = name from users where id=@updated_by
			set @remarks = 'Delete by user : ' + @user_name

			insert into image_files_deleted(id,file_name,import_session_id,institution_id,institution_code,institution_name,date_downloaded,
			                                remarks,deleted_by,date_deleted)
			                        (select id,file_name,import_session_id,institution_id,institution_code,institution_name,date_downloaded,
									        @remarks,@updated_by,getdate()
									 from scheduler_img_file_downloads_ungrouped
									 where id=@id)
			if(@@rowcount =0)
				begin
					rollback transaction
					select @return_status = 0,@error_code ='201'
					return 0
				end
			
			delete from scheduler_img_file_downloads_ungrouped where id=@id

			if(@@rowcount =0)
				begin
					rollback transaction
					select @return_status = 0,@error_code ='201'
					return 0
				end

			commit transaction
		end
	else
		begin
			select @return_status=0,@error_code='204'	
			return 0	
		end

select @return_status=1,@error_code='202'	
set nocount off
return 1

end

GO
