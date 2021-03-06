USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_priority_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_priority_update]
GO
/****** Object:  StoredProcedure [dbo].[case_list_priority_update]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_priority_update : update
                 priority
** Created By   : Pavel Guha
** Created On   : 02/03/2020
*******************************************************/
CREATE procedure [dbo].[case_list_priority_update]
    @study_id uniqueidentifier,
	@priority_id int,
	@updated_by uniqueidentifier,
    @error_code nvarchar(500)='' output,
    @return_status int =0 output
as
begin
	
	set nocount on
	set datefirst 1

	declare @finishing_datetime datetime,
			@finishing_time_hrs int,
			@received_date datetime,
			@current_datetime datetime,
			@final_rpt_release_datetime datetime,
			@is_stat nchar(1),
			@FNLRPTAUTORELHR int,
			@modality_id int,
			@species_id int,
			@institution_id uniqueidentifier

	declare @beyond_operation_time nchar(1),
			@in_exp_list nchar(1),
			@sender_time_offset_mins int,
			@next_operation_time nvarchar(130),
			@delv_time nvarchar(130),
			@display_message nvarchar(250)

    select @modality_id    = isnull(modality_id,0) ,
	       @species_id     = isnull(species_id,0),
	       @institution_id = isnull(institution_id,'00000000-0000-0000-0000-000000000000')
	from study_hdr 
	where id = @study_id

	select @is_stat = is_stat from sys_priority where priority_id=@priority_id

	set @in_exp_list='N'
	set @beyond_operation_time ='N'
	set @error_code =''
	set @return_status=0

	
	exec common_service_availability_check
		@species_id            = @species_id,
		@modality_id           = @modality_id,
		@institution_id        = @institution_id,
		@priority_id           = @priority_id,
		@beyond_operation_time = @beyond_operation_time output,
		@in_exp_list           = @in_exp_list output,
		@error_code            = @error_code output,
		@return_status         = @return_status output

	if(@return_status=0)
		begin
			return 0
		end
	
	
	begin transaction

	select @received_date= status_last_updated_on,
	       @current_datetime = getdate()
	from study_hdr 
	where id=@study_id


	select @finishing_time_hrs = finishing_time_hrs from sys_priority where priority_id = @priority_id
	set @finishing_time_hrs = isnull(@finishing_time_hrs,0)
	select @FNLRPTAUTORELHR = data_type_string from general_settings where control_code='FNLRPTAUTORELHR'
	select @sender_time_offset_mins = id from sys_us_time_zones where is_default='Y'

	exec common_check_operation_time
		@priority_id             = @priority_id,
		@sender_time_offset_mins = @sender_time_offset_mins,
		@submission_date         = @received_date,
		@next_operation_time     = @next_operation_time output,
		@delv_time               = @delv_time output,
		@display_message         = @display_message output,
		@error_code              = @error_code output,
		@return_status           = @return_status output


	if(@is_stat='N')
		begin
			set @finishing_datetime = dateadd(HH,@finishing_time_hrs,@received_date)
			set @final_rpt_release_datetime = dateadd(mi,(select final_report_release_time_mins from sys_priority where priority_id=@priority_id),getdate())
		end
	else if(@is_stat='Y')
		begin
			if(@in_exp_list='Y')
				begin
					set @finishing_datetime = dateadd(HH,@finishing_time_hrs,@received_date)
				end
			else
				begin
					set @finishing_datetime = convert(datetime,@delv_time)
				end
			
			select @final_rpt_release_datetime = dateadd(HH,@FNLRPTAUTORELHR,@received_date)
		end


	update study_hdr
	set priority_id=@priority_id,
		finishing_datetime = @finishing_datetime,
		final_rpt_release_datetime = @final_rpt_release_datetime,
	    pacs_wb='Y',
		updated_by = @updated_by,
		date_updated=getdate()
	where id = @study_id

   if(@@rowcount = 0)
		begin
			rollback transaction
			select @return_status = 0,@error_code ='035'
			return 0
		end
	
	commit transaction
	set @return_status=1
	set @error_code=''
	set nocount off
	return 1

end


GO
