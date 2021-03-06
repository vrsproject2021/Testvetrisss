USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_archive_report_addendum_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_archive_report_addendum_update]
GO
/****** Object:  StoredProcedure [dbo].[case_list_archive_report_addendum_update]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_archive_report_addendum_update : update
                  resport addendum for srchived study
** Created By   : Pavel Guha
** Created On   : 13/07/2020
*******************************************************/
CREATE procedure [dbo].[case_list_archive_report_addendum_update]
    @id uniqueidentifier,
	@TVP_addendums as case_study_report_addendum readonly,
	@updated_by uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	
	set nocount on

	declare @study_uid nvarchar(100),
	        @report_id uniqueidentifier,
			@srl_no int,
			@addendum_text nvarchar(max),
			@addendum_text_html nvarchar(max),
			@rowcount int,
			@counter int

	select @study_uid = study_uid from study_hdr_archive where id=@id
	select @report_id = report_id from study_hdr_final_reports_archive where study_hdr_id = @id

	begin transaction

	--ADDENDUMS
	select @rowcount = count(srl_no),
		   @counter = 1
	from @TVP_addendums

	while(@counter <= @rowcount)
		begin
			select @srl_no             = srl_no,
				   @addendum_text      = addendum_text,
				   @addendum_text_html = @addendum_text_html
			from @TVP_addendums
			where srl_no = @counter

			---- for notifying report creation by email
			if(select count(addendum_srl) from study_report_addendums_archive where study_hdr_id = @id and study_uid =@study_uid and addendum_srl=@srl_no)=0
				begin
					select @report_id = report_id from study_hdr_final_reports where study_hdr_id = @id and study_uid =@study_uid

					insert into study_report_addendums_archive(report_id,addendum_srl,study_hdr_id,study_uid,addendum_text,addendum_text_html,
														created_by,date_created,archived_by,date_archived)
												values(@report_id,@srl_no,@id,@study_uid,@addendum_text,@addendum_text_html,
													   @updated_by,getdate(), @updated_by,getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='348'
							return 0
						end
				end
			else
				begin
					update study_report_addendums_archive
					set addendum_text      = @addendum_text,
					    addendum_text_html = @addendum_text_html,
						updated_by         = @updated_by,
						date_updated       = getdate()
					where addendum_srl=@srl_no
					and study_hdr_id = @id 
					and study_uid =@study_uid

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='348'
							return 0
						end
						
				end
	
			set @counter = @counter + 1
		end

	commit transaction
	select @return_status=1,@error_code='349'
	set nocount off
	return 1

end


GO
