USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_img_obj_count_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_img_obj_count_update]
GO
/****** Object:  StoredProcedure [dbo].[case_list_img_obj_count_update]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_img_obj_count_update : update
                  image/object count of a study
** Created By   : Pavel Guha
** Created On   : 11/04/2019
*******************************************************/
CREATE PROCEDURE [dbo].[case_list_img_obj_count_update] 
    @id uniqueidentifier,
	@img_count int,
	@object_count int,
	@updated_by uniqueidentifier,
	@menu_id int,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @error_code nvarchar(500)='' output,
    @return_status int =0 output
as
begin
	set nocount on


	if(select count(id) from study_hdr where id = @id)>0
		begin
				
				begin transaction	

				update study_hdr
				set     img_count_pacs             = @img_count,
						img_count                  = @img_count,
						object_count_pacs          = @object_count
				where id = @id 

				if(@@rowcount = 0)
					begin
						rollback transaction
						select @return_status = 0,@error_code ='505'
						return 0
					end

	
				exec common_study_user_activity_trail_save
						@study_hdr_id = @id,
						@study_uid    ='',
						@menu_id      = @menu_id,
						@activity_text = 'Updated image/object count',
						@session_id    = @session_id,
						@activity_by   = @updated_by,
						@error_code    = @error_code output,
						@return_status = @return_status output

				if(@return_status=0)
					begin
						rollback transaction
						return 0
					end

				commit transaction
				select @error_code='',@return_status=1
				set nocount off

				return 1
		end
	else if(select count(id) from study_hdr_archive where id = @id)>0
		begin
				
				begin transaction	

				update study_hdr_archive
				set     img_count_pacs             = @img_count,
						img_count                  = @img_count,
						object_count_pacs          = @object_count
				where id = @id 

				if(@@rowcount = 0)
					begin
						rollback transaction
						select @return_status = 0,@error_code ='505'
						return 0
					end

	
				exec common_study_user_activity_trail_save
						@study_hdr_id = @id,
						@study_uid    ='',
						@menu_id      = @menu_id,
						@activity_text = 'Updated image/object count',
						@session_id    = @session_id,
						@activity_by   = @updated_by,
						@error_code    = @error_code output,
						@return_status = @return_status output

				if(@return_status=0)
					begin
						rollback transaction
						return 0
					end

				commit transaction
				select @error_code='',@return_status=1
				set nocount off

				return 1
		end
	else
		begin
			select @error_code='094',@return_status=0
			set nocount off
			return 0
		end
	
end

GO
