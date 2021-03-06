USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_manual_submission_file_delete]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_manual_submission_file_delete]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_manual_submission_file_delete]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_manual_submission_file_delete : 
				  delete file entry of manual submission file
** Created By   : Pavel Guha
** Created On   : 30/06/2020
*******************************************************/
--exec scheduler_settings_fetch
create procedure [dbo].[scheduler_manual_submission_file_delete]
	@file_id uniqueidentifier= '00000000-0000-0000-0000-000000000000',
	@error_msg nvarchar(500)='' output,
	@return_type int=0 output
as
begin
	set nocount on

	

	if (@file_id <>'00000000-0000-0000-0000-000000000000')
	 begin
		begin transaction

		delete from study_manual_upload_files where file_id=@file_id

		if(@@rowcount=0)
			begin
				rollback transaction
				select @error_msg='Failed to delete the file entry. File ID  :'+ CONVERT(nvarchar(36),@file_id),
				       @return_type=0
				return 0
			end

		commit transaction
		
	 end

	set nocount off
	select @error_msg='',@return_type=1
	return 1

end

GO
