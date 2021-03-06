USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_archive_file_count_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_archive_file_count_update]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_archive_file_count_update]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_archive_file_count_update : 
                  update count of file(s) archived
** Created By   : Pavel Guha
** Created On   : 17/06/2019
*******************************************************/
--exec scheduler_log_save 1,0,'Test','',0
CREATE procedure [dbo].[scheduler_archive_file_count_update]
	@study_uid nvarchar(100),
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	set nocount on
	
	declare @strSQL varchar(max),
	        @db_name nvarchar(50)
	

	if(select count(id) from study_hdr where study_uid = @study_uid)>0
		begin
			update study_hdr set archive_file_count = archive_file_count + 1 where study_uid=@study_uid

			if(@@rowcount>0)
				begin
					select  @error_msg='',@return_type=1	
					return 1				
				end
			else
				begin
					select  @error_msg='Failed to update the archive file count of Study UID : ' + @study_uid,@return_type=0
					return 0
				end
		end
	else if(select count(id) from study_hdr_archive where study_uid = @study_uid)>0
		begin
			update study_hdr_archive set archive_file_count = archive_file_count + 1 where study_uid=@study_uid

			if(@@rowcount>0)
				begin
					select  @error_msg='',@return_type=1	
					return 1				
				end
			else
				begin
					select  @error_msg='Failed to update the archive file count of Study UID : ' + @study_uid,@return_type=0
					return 0
				end
		end
	else
		begin
			set @db_name=''
			exec common_get_study_database
				@study_uid  = @study_uid,
				@db_name    = @db_name output

			if(@db_name <>'vrsdb')
				begin
					set @strSQL = 'update ' + @db_name + '..study_hdr_archive set archive_file_count = archive_file_count + 1 where study_uid=''' + @study_uid + ''' '
					exec(@strSQL)

					if(@@rowcount>0)
						begin
							select  @error_msg='',@return_type=1	
							return 1				
						end
					else
						begin
							select  @error_msg='Failed to update the archive file count of Study UID : ' + @study_uid,@return_type=0
							return 0
						end
				end
		    
		end
	

	

	set nocount off
	
end


GO
