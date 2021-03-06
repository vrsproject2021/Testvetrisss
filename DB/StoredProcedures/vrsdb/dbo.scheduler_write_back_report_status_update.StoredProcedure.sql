USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_write_back_report_status_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_write_back_report_status_update]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_write_back_report_status_update]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_write_back_report_status_update : update
                  report write back status
** Created By   : Pavel Guha
** Created On   : 31/07/2020
*******************************************************/
CREATE procedure [dbo].[scheduler_write_back_report_status_update]
    @study_id uniqueidentifier,
	@study_uid nvarchar(100),
	@status_id int,
	@is_addendum nchar(1),
	@addendum_srl int=0,
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	set nocount on

		

	begin transaction

	if(@status_id=60)
		begin
			if(select count(study_hdr_id) from study_hdr_dictated_reports where study_hdr_id=@study_id and study_uid = @study_uid)> 0
				begin
					update study_hdr_dictated_reports
					set pacs_wb ='N'
					where study_uid = @study_uid
					and study_hdr_id = @study_id
				end
			else
				begin
					update study_hdr_dictated_reports_archive
					set pacs_wb ='N'
					where study_uid = @study_uid
					and study_hdr_id = @study_id
				end
		end
	else if(@status_id=80)
		begin
			if(select count(study_hdr_id) from study_hdr_prelim_reports where study_hdr_id=@study_id and study_uid = @study_uid)> 0
				begin
					update study_hdr_prelim_reports
					set pacs_wb ='N'
					where study_uid = @study_uid
					and study_hdr_id = @study_id
				end
			else
				begin
					update study_hdr_prelim_reports_archive
					set pacs_wb ='N'
					where study_uid = @study_uid
					and study_hdr_id = @study_id
				end
		end
	else if(@status_id=100)
		begin
			if(@is_addendum ='N')
				begin
					if(select count(study_hdr_id) from study_hdr_final_reports where study_hdr_id=@study_id and study_uid = @study_uid)> 0
						begin
							update study_hdr_final_reports
							set pacs_wb ='N'
							where study_uid = @study_uid
							and study_hdr_id = @study_id
						end
					else
						begin
							update study_hdr_final_reports_archive
							set pacs_wb ='N'
							where study_uid = @study_uid
							and study_hdr_id = @study_id
						end
			    end
			else if(@is_addendum ='Y')
				begin
					if(select count(study_hdr_id) from study_report_addendums where study_hdr_id=@study_id and study_uid = @study_uid)> 0
						begin
							update study_report_addendums
							set pacs_wb ='N'
							where study_uid  = @study_uid
							and study_hdr_id = @study_id
							and addendum_srl = @addendum_srl
						end
					else
						begin
							update study_report_addendums_archive
							set pacs_wb ='N'
							where study_uid  = @study_uid
							and study_hdr_id = @study_id
							and addendum_srl = @addendum_srl
						end
			    end
		end


	if(@@rowcount=0)
		begin
			rollback transaction
			select @return_type=0,@error_msg='Failed to change the report write back status to N for Study UID ' + @study_uid
			return 0
		end

	commit transaction
	select @error_msg='Report write back status of Study UID ' + @study_uid + ' changed to N',@return_type=1
	set nocount off
	return 1

end


GO
