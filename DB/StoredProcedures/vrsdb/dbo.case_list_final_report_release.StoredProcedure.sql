USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_final_report_release]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_final_report_release]
GO
/****** Object:  StoredProcedure [dbo].[case_list_final_report_release]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_final_report_release : release
                  final report
** Created By   : Pavel Guha
** Created On   : 06/02/2021
*******************************************************/
CREATE procedure [dbo].[case_list_final_report_release]
    @study_id uniqueidentifier,
	@menu_id int,
	@updated_by uniqueidentifier,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@FNLRPTMANUALRELMIN int = 0 output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	
	set nocount on


	begin transaction


	if(select final_rpt_released from study_hdr where id=@study_id)='N'
		begin
			select @FNLRPTMANUALRELMIN = data_type_number from general_settings where control_code='FNLRPTMANUALRELMIN'

			update study_hdr
			set final_rpt_release_datetime = dateadd(mi,@FNLRPTMANUALRELMIN,getdate()),
				final_rpt_released_on      = getdate(),
				final_rpt_released_by      = @updated_by
			where id = @study_id

			if(@@rowcount = 0)
				begin
					rollback transaction
					select @return_status = 0,@error_code ='450'
					return 0
				end
		end
	else
		begin
			rollback transaction
			select @return_status = 0,@error_code ='448'
			return 0
		end

	  exec common_study_user_activity_trail_save
			@study_hdr_id = @study_id,
			@study_uid    ='',
			@menu_id      = @menu_id,
			@activity_text = 'Final report released',
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
	set @return_status=1
	set @error_code='449'
	set nocount off
	return 1

end


GO
