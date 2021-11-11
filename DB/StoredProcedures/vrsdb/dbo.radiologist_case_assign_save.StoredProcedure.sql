USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_case_assign_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[radiologist_case_assign_save]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_case_assign_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : radiologist_case_assign_save : save scheduler log
** Created By   : Pavel Guha
** Created On   : 21/04/2020
*******************************************************/
CREATE procedure [dbo].[radiologist_case_assign_save]
    @id uniqueidentifier,
	@type nchar(1),
	@radiologist_id uniqueidentifier,
	@menu_id int,
    @updated_by uniqueidentifier,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@user_name nvarchar(250)='' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	
	set nocount on

	declare @radiologist_name nvarchar(100),
	        @activity_text nvarchar(max)
				
	begin transaction
	exec common_check_record_lock_ui
		@menu_id       = @menu_id,
		@record_id     = @id,
		@user_id       = @updated_by,
		@session_id    = @session_id,
		@user_name     = @user_name output,
		@error_code    = @error_code output,
		@return_status = @return_status output

	if(@return_status=0)
		begin
			rollback transaction
			return 0
		end

	select @radiologist_name = name from radiologists where id = @radiologist_id

	if(@type='P')
		begin
			update study_hdr
			set radiologist_id   = @radiologist_id,
				radiologist_pacs = @radiologist_name,
			    dict_radiologist_id   = @radiologist_id,
				dict_radiologist_pacs = @radiologist_name,
				manually_assigned       = 'Y',
				pacs_wb                 = 'Y',
				rad_assigned_on         = getdate()
			where id = @id
		end
	else
		begin
			update study_hdr
			set final_radiologist_id   = @radiologist_id,
				final_radiologist_pacs = @radiologist_name,
				manually_assigned      = 'Y',
				pacs_wb                = 'Y',
				rad_assigned_on         = getdate()
			where id = @id
		end

	

	if(@@rowcount = 0)
		begin
			rollback transaction
			select @return_status = 0,@error_code ='035'
			return 0
		end

  --  if(@type='P')
		--begin
		--	if(select isnull(radiologist_id,'00000000-0000-0000-0000-000000000000') from study_hdr where id=@id) = '00000000-0000-0000-0000-000000000000'
		--		begin
		--			update study_hdr
		--			set radiologist_id   = @radiologist_id,
		--			    radiologist_pacs = @radiologist_name
		--			where id = @id

		--			if(@@rowcount = 0)
		--				begin
		--					rollback transaction
		--					select @return_status = 0,@error_code ='035'
		--					return 0
		--				end
		--		end

			
		--end

	if(@type='P') set @activity_text = 'Readiing/Preliminary Radiologist ' + @radiologist_name 
	else set @activity_text = 'Final Radiologist ' + @radiologist_name

	set @activity_text =  @activity_text + ' assigned'

	exec common_study_user_activity_trail_save
		@study_hdr_id  = @id,
		@study_uid     ='',
		@menu_id       = @menu_id,
		@activity_text = @activity_text,
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
	select @error_code='022',@return_status=1
	set nocount off
	return 1

end


GO
