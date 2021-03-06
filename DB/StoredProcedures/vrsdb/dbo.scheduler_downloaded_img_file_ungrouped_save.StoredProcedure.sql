USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_downloaded_img_file_ungrouped_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_downloaded_img_file_ungrouped_save]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_downloaded_img_file_ungrouped_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_downloaded_img_file_ungrouped_save : 
                  save downloaded ungroup image file details
** Created By   : Pavel Guha
** Created On   : 19/08/2019
*******************************************************/
CREATE procedure [dbo].[scheduler_downloaded_img_file_ungrouped_save]
	@institution_code nvarchar(5),
	@file_name nvarchar(250),
	@import_session_id nvarchar(30) = null, 
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	set nocount on

	declare @institution_id uniqueidentifier,
			@institution_name nvarchar(100),
			@id uniqueidentifier

	begin transaction

	set @institution_id = isnull((select id 
	                              from institutions
								  where code = @institution_code),'00000000-0000-0000-0000-000000000000')

	set @institution_name = isnull((select name 
	                               from institutions
								   where id = @institution_id),'')


	if(@institution_id <>  '00000000-0000-0000-0000-000000000000')
		begin 
			if(select count(id) from scheduler_img_file_downloads_ungrouped where file_name=@file_name and institution_id=@institution_id and grouped='N')=0
				begin
			
				   set @id = newid()
			   	   insert into scheduler_img_file_downloads_ungrouped(id,file_name,institution_id,institution_code,institution_name,date_downloaded,import_session_id)
															  values(@id,@file_name,@institution_id,@institution_code,@institution_name,getdate(),@import_session_id)	

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to save download details of file ' + @file_name
							return 0
						end
				end
			else
				begin
					rollback transaction
					select @return_type=0,@error_msg='File : ' + @file_name + ' already exists for institution ' + @institution_name + ' and not yet grouped'
					return 0
				end
	     end
    else
		begin
			rollback transaction
			select @return_type=0,@error_msg='Institution code ' + @institution_code + ' for File : ' + @file_name + ' does not exist'
			return 0
		end

    commit transaction
	set @return_type=1
	set @error_msg=''

	set nocount off
	return 1

end


GO
