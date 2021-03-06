USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_study_file_count_check]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_study_file_count_check]
GO
/****** Object:  StoredProcedure [dbo].[case_list_study_file_count_check]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_study_file_count_check : fetch case list header
** Created By   : Pavel Guha
** Created On   : 14/12/2020
*******************************************************/
--exec case_list_study_file_count_check 'c6141fdc-f151-4ca7-aabb-81b99d46d72c'
CREATE procedure [dbo].[case_list_study_file_count_check]
    @id uniqueidentifier,	
	@actual_archived_file_count int,
	@pending_file_count int = 0 output,
	@error_code nvarchar(10) ='' output,
	@return_status int = 0 output
as
begin
	 set nocount on

	 declare @modality_id int,
	         @track_by nchar(1),
			 @img_count int,
			 @object_count int

			begin transaction
			if(select count(id) from study_hdr where id = @id)>0
				begin
					update study_hdr 
					set archive_file_count = @actual_archived_file_count
					where id = @id

					if(@@rowcount = 0)
						begin
							rollback transaction
							select @error_code='489',@return_status=0
							return 0
						end
					else
						begin
							commit transaction
						end

					select @modality_id        = modality_id,
					       @img_count          = img_count,
						   @object_count       = object_count
					from study_hdr 
					where id=@id
					
					select @track_by = track_by from modality where id = @modality_id

					if(@track_by ='I')
						begin
							if(@actual_archived_file_count < @img_count)
								begin
									set @pending_file_count = (@img_count - @actual_archived_file_count)
								end
							else
								begin
									set @pending_file_count = 0
								end
						end
					else if(@track_by ='O')
						begin
							if(@actual_archived_file_count < @object_count)
								begin
									set @pending_file_count = (@object_count - @actual_archived_file_count)
								end
							else
								begin
									set @pending_file_count = 0
								end
						end

				end
			else if(select count(id) from study_hdr_archive where id = @id)>0
				begin
					update study_hdr_archive 
					set archive_file_count = @actual_archived_file_count
					where id = @id

					if(@@rowcount = 0)
						begin
							rollback transaction
							select @error_code='489',@return_status=0
							return 0
						end
					else
						begin
							commit transaction
						end

					select @modality_id        = modality_id,
					       @img_count          = img_count,
						   @object_count       = object_count
					from study_hdr_archive
					where id=@id
					
					select @track_by = track_by from modality where id = @modality_id

					if(@track_by ='I')
						begin
							if(@actual_archived_file_count < @img_count)
								begin
									set @pending_file_count = (@img_count - @actual_archived_file_count)
								end
							else
								begin
									set @pending_file_count = 0
								end
						end
					else if(@track_by ='O')
						begin
							if(@actual_archived_file_count < @object_count)
								begin
									set @pending_file_count = (@object_count - @actual_archived_file_count)
								end
							else
								begin
									set @pending_file_count = 0
								end
						end

				end

	if(@pending_file_count > 0)
		begin
			select @error_code='490',@return_status=1
		end
	else
		begin
			select @error_code='491',@return_status=1
		end


	set nocount off
	return 1
end

GO
