USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_archive_study_delete]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_archive_study_delete]
GO
/****** Object:  StoredProcedure [dbo].[case_list_archive_study_delete]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_archive_study_delete : delete
				  study from archive
** Created By   : Pavel Guha
** Created On   : 13 May 2020
*******************************************************/
CREATE procedure [dbo].[case_list_archive_study_delete]
    @study_id uniqueidentifier,
	@study_uid nvarchar(100),
	@updated_by uniqueidentifier,
	@menu_id int,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	
	set nocount on
	declare @deleted_by nvarchar(100),
	        @remarks nvarchar(250),
			@inst_id uniqueidentifier,
			@inst_code nvarchar(10),
			@inst_name nvarchar(100),
			@archive_folder nvarchar(500)

	
	begin transaction

	

	select  folder = (select data_type_string from general_settings where control_code='FTPDLFLDRTMP'),
			file_name 
	from scheduler_file_downloads_dtls where id=@study_id

	select @inst_id = institution_id from study_hdr_archive where id=@study_id and study_uid=@study_uid

	select @inst_code=code,
	       @inst_name = name
	from institutions
	where id = isnull(@inst_id,'00000000-0000-0000-0000-000000000000')

	select @archive_folder = data_type_string from general_settings where control_code='PACSARCHIVEFLDR'

	select archive_folder = rtrim(ltrim(@archive_folder)) + '/' + rtrim(ltrim(isnull(@inst_code,''))) + '_' + rtrim(ltrim(isnull(@inst_name,''))) + '_' + @study_uid

	select @deleted_by = name
	from users
	where id=@updated_by

	select @remarks = 'Deleted by ' + @deleted_by + ' from archive'

	insert into study_hdr_deleted(id,study_uid,study_date,received_date,synched_on,patient_name,institution_name,remarks,deleted_on,deleted_by)
	(select h.id,h.study_uid,h.study_date,h.received_date,h.synched_on,h.patient_name,isnull(i.name,isnull(h.institution_name_pacs,'')),@remarks,getdate(),@updated_by
	 from study_hdr_archive h
	 left outer join institutions i on i.id = h.institution_id
	 where h.id = @study_id 
	 and h.study_uid =@study_uid)

	 if(@@rowcount=0)
		begin
			rollback transaction
			select @return_status=0,@error_code='122'
			return 0
		end

	if(select count(study_hdr_id) from study_hdr_documents_archive where study_hdr_id=@study_id)>0
		begin
			delete from study_hdr_documents_archive
			where study_hdr_id=@study_id

			if(@@rowcount =0)
				begin
					rollback transaction
					select @error_code = '249',@return_status =0
					return 0
				end
		end

	if(select count(study_hdr_id) from study_hdr_dcm_files_archive where study_hdr_id=@study_id)>0
		begin
			delete from study_hdr_dcm_files_archive
			where study_hdr_id=@study_id

			if(@@rowcount =0)
				begin
					rollback transaction
					select @error_code = '330',@return_status =0
					return 0
				end
		end

	if(select count(study_hdr_id) from study_hdr_study_types_archive where study_hdr_id=@study_id)>0
		begin
			delete from study_hdr_study_types_archive
			where study_hdr_id=@study_id

			if(@@rowcount =0)
				begin
					rollback transaction
					select @error_code = '251',@return_status =0
					return 0
				end
		end

	if(select count(study_hdr_id) from study_hdr_merged_studies_archive where study_hdr_id=@study_id)>0
		begin
			delete from study_hdr_merged_studies
			where study_hdr_id=@study_id

			if(@@rowcount =0)
				begin
					rollback transaction
					select @error_code = '435',@return_status =0
					return 0
				end
		end

	if(select count(study_id) from sys_case_study_status_log where study_id=@study_id)>0
		begin
			delete from sys_case_study_status_log where study_id = @study_id

			if(@@rowcount =0)
				begin
					rollback transaction
					select @error_code = '250',@return_status =0
					return 0
				end

				 
		end

	if(select count(study_hdr_id) from study_hdr_dictated_reports_archive where study_hdr_id=@study_id)>0
		begin
			delete from study_hdr_dictated_reports_archive
			where study_hdr_id=@study_id

			if(@@rowcount =0)
				begin
					rollback transaction
					select @error_code ='394',
							@return_status =0
					return 0
				end
		end

	if(select count(study_hdr_id) from study_hdr_prelim_reports_archive where study_hdr_id=@study_id)>0
		begin
			delete from study_hdr_prelim_reports_archive
			where study_hdr_id=@study_id

			if(@@rowcount =0)
				begin
					rollback transaction
					select @error_code ='345',
							@return_status =0
					return 0
				end
		end

	if(select count(study_hdr_id) from  study_report_addendums_archive where study_hdr_id = @study_id )>0
		begin
			delete from study_report_addendums_archive where study_hdr_id = @study_id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_code ='346' ,
							@return_status =0
					return 0
				end
		end

	if(select count(study_hdr_id) from study_hdr_final_reports_archive where study_hdr_id=@study_id)>0
		begin
			delete from study_hdr_final_reports_archive
			where study_hdr_id=@study_id

			if(@@rowcount =0)
				begin
					rollback transaction
					select @error_code ='347',
							@return_status =0
					return 0
				end
		end


	if(select count(id) from study_hdr_archive where id=@study_id)>0
		begin
					
			delete from study_hdr_archive where id = @study_id

			if(@@rowcount =0)
				begin
					rollback transaction
					select @error_code = '122',@return_status =0
					return 0
				end

			exec common_study_user_activity_trail_save
				@study_hdr_id  = @study_id,
				@study_uid     ='',
				@menu_id       = @menu_id,
				@activity_text = 'Deleted',
				@session_id    = @session_id,
				@activity_by   = @updated_by,
				@error_code    = @error_code output,
				@return_status = @return_status output

			if(@return_status=0)
				begin
					rollback transaction
					return 0
				end
		end

	if(select count(study_uid) from study_synch_dump where study_uid=@study_uid)>0
		begin
			delete from study_synch_dump where study_uid=@study_uid

				if(@@rowcount =0)
					begin
					rollback transaction
					select @error_code = '122',@return_status =0
					return 0
					end
		end

	if(select count(id) from scheduler_file_downloads_dtls where id=@study_id)>0
		begin
			delete from scheduler_file_downloads_dtls where id = @study_id

			if(@@rowcount =0)
				begin
					rollback transaction
					select @error_code = '252',@return_status =0
					return 0
				end
		end
			
	if(select count(id) from scheduler_file_downloads where id=@study_id)>0
		begin
			delete from scheduler_file_downloads where id = @study_id

			if(@@rowcount =0)
				begin
					rollback transaction
					select @error_code = '252',@return_status =0
					return 0
				end
		end

	if(select count(id) from scheduler_img_file_downloads_ungrouped where grouped_id=@study_id)>0
		begin
			delete from scheduler_img_file_downloads_ungrouped where grouped_id=@study_id

			if(@@rowcount =0)
				begin
					rollback transaction
					select @error_code = '252',@return_status =0
					return 0
				end
		end
	
	if(select count(study_hdr_id) from scheduler_img_file_downloads_grouped_docs where study_hdr_id=@study_id)>0
		begin
			delete from scheduler_img_file_downloads_grouped_docs where study_hdr_id = @study_id

			if(@@rowcount =0)
				begin
					rollback transaction
					select @error_code = '252',@return_status =0
					return 0
				end
		end

	if(select count(study_hdr_id) from scheduler_img_file_downloads_grouped_study_types where study_hdr_id=@study_id)>0
		begin
			delete from scheduler_img_file_downloads_grouped_study_types where study_hdr_id = @study_id

			if(@@rowcount =0)
				begin
					rollback transaction
					select @error_code = '252',@return_status =0
					return 0
				end
		end

	if(select count(id) from scheduler_img_file_downloads_grouped_dtls where id=@study_id)>0
		begin
			delete from scheduler_img_file_downloads_grouped_dtls where id = @study_id

			if(@@rowcount =0)
				begin
					rollback transaction
					select @error_code = '252',@return_status =0
					return 0
				end
		end

	if(select count(id) from scheduler_img_file_downloads_grouped where id=@study_id)>0
		begin
			delete from scheduler_img_file_downloads_grouped where id = @study_id

			if(@@rowcount =0)
				begin
					rollback transaction
					select @error_code = '252',@return_status =0
					return 0
				end
		end

	if(select count(study_uid) from dicom_router_files_received where study_uid=@study_uid)>0
		begin
			delete from dicom_router_files_received where study_uid = @study_uid

			if(@@rowcount =0)
				begin
					rollback transaction
					select @error_code = '252',@return_status =0
					return 0
				end
		end

	commit transaction
	set @return_status=1
	set @error_code=''
	set nocount off
	return 1

end


GO
