USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_radiologist_self_assign]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_radiologist_self_assign]
GO
/****** Object:  StoredProcedure [dbo].[case_list_radiologist_self_assign]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_radiologist_assign : save
                  case report
** Created By   : Pavel Guha
** Created On   : 24/04/2020
*******************************************************/
CREATE procedure [dbo].[case_list_radiologist_self_assign]
	@radiologist_id uniqueidentifier,
	@xml_study ntext,
	@menu_id int,
    @updated_by uniqueidentifier,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@user_name nvarchar(250)='' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	
	set nocount on

	declare @id uniqueidentifier,
	        @study_uid nvarchar(100),
			@curr_radiologist_id uniqueidentifier,
			@curr_status_id int, 
	        @radiologist_name nvarchar(100),
	        @activity_text nvarchar(max),
			@hDoc int,
			@rowcount int,
			@counter int

	
	exec sp_xml_preparedocument @hDoc output,@xml_study 
	select @radiologist_name = name from radiologists where id = @radiologist_id

	set @activity_text = 'Readiing/Preliminary Radiologist ' + @radiologist_name  +  'assigned'

	set @counter = 1
	select  @rowcount=count(row_id)  
	from openxml(@hDoc,'study/row', 2)  
	with( row_id int )

	while(@counter <= @rowcount)
		begin
			begin transaction

			select @study_uid = study_uid
			from openxml(@hDoc,'study/row', 2)  
			with(
			      study_uid nvarchar(100),
				  row_id int 
				 ) xmlTemp where xmlTemp.row_id = @counter 

			select @id                  = id,
			       @curr_radiologist_id = isnull(radiologist_id,'00000000-0000-0000-0000-000000000000'),
				   @curr_status_id      = study_status_pacs
			from study_hdr
			where study_uid = @study_uid

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
					if(@error_code='033')
						begin
							select @error_code ='472'
						end
					rollback transaction
					return 0
				end

			if(@curr_status_id>=60)
				begin
					rollback transaction
					select @return_status=0,@error_code='473'
					return 0
				end

			if((@curr_radiologist_id <> @radiologist_id) and (@curr_radiologist_id <> '00000000-0000-0000-0000-000000000000'))
				begin
					rollback transaction
					select @return_status=0,@error_code='474'
					return 0
				end

			update study_hdr
			set radiologist_id   = @radiologist_id,
				radiologist_pacs = @radiologist_name,
				prelim_radiologist_id   = @radiologist_id,
				prelim_radiologist_pacs = @radiologist_name,
				manually_assigned       = 'S',
				pacs_wb                 = 'Y',
				rad_assigned_on         = getdate()
			where id = @id

		    if(@@rowcount = 0)
				begin
					rollback transaction
					select @return_status = 0,@error_code ='475'
					return 0
				end

			exec common_study_user_activity_trail_save
				@study_hdr_id  = @id,
				@study_uid     = @study_uid,
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
			set @counter = @counter + 1
		end


	
	set @return_status=1
	set @error_code='034'
	set nocount off
	return 1	

end


GO
