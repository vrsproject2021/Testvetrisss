USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_study_correction_delete]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_study_correction_delete]
GO
/****** Object:  StoredProcedure [dbo].[ar_study_correction_delete]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_study_correction_delete : 
                  delete study
** Created By   : Pavel Guha 
** Created On   : 31/01/2020
*******************************************************/
--exec ar_study_correction_delete '3225fd47-3ff5-4863-8e89-22e960263eb3','ba128821-10b3-4794-9f72-49a8cb575769',56,'11111111-1111-1111-1111-111111111111','','',0
CREATE procedure [dbo].[ar_study_correction_delete]
	@billing_cycle_id uniqueidentifier,
	@study_id uniqueidentifier,
	@menu_id          int,
    @updated_by       uniqueidentifier,
	@archive_folder   nvarchar(500) = '' output,
    @user_name        nvarchar(500) = '' output,
    @error_code       nvarchar(10)='' output,
    @return_status    int =0 output
as
	begin
		set nocount on

		declare @institution_id uniqueidentifier,
		        @billing_account_id uniqueidentifier,
				@study_uid nvarchar(5),
				@deleted_by nvarchar(100),
			    @inst_code nvarchar(10),
			    @inst_name nvarchar(100)

		
		 exec common_check_record_lock_ui
				@menu_id       = @menu_id,
				@record_id     = @billing_cycle_id,
				@user_id       = @updated_by,
				@user_name     = @user_name output,
				@error_code    = @error_code output,
				@return_status = @return_status output
		
		if(@return_status=0)
			begin
				return 0
			end

		select @deleted_by= name from users where id=@updated_by

		
		begin transaction

		select @study_uid = study_uid,
		       @institution_id = institution_id
		from study_hdr
		where id = @study_id

		select @inst_code=code,
			   @inst_name = name
		from institutions
		where id = isnull(@institution_id,'00000000-0000-0000-0000-000000000000')

	    select @archive_folder = data_type_string from general_settings where control_code='PACSARCHIVEFLDR'

	    select @archive_folder = rtrim(ltrim(@archive_folder)) + '/' + rtrim(ltrim(isnull(@inst_code,''))) + '_' + rtrim(ltrim(isnull(@inst_name,''))) + '_' + @study_uid

		if(select count(id) from study_hdr where id = @study_id)>0
			begin
				insert into study_hdr_deleted(id,study_uid,study_date,received_date,synched_on,patient_name,institution_name,remarks,deleted_on,deleted_by)
				(select h.id,h.study_uid,h.study_date,h.received_date,h.synched_on,h.patient_name,isnull(i.name,''),'Deleted for invoice amendment by ' + @deleted_by,getdate(),@updated_by
				 from study_hdr h
				 left outer join institutions i on i.id = h.institution_id
				 where h.id =@study_id)

				if(@@rowcount=0)
					begin
						rollback transaction
						select @error_code ='122',@return_status =0
						return 0
					end
			
				if(select count(study_hdr_id) from study_hdr_documents where study_hdr_id=@study_id)>0
					begin
						delete from study_hdr_documents
						where study_hdr_id=@study_id

						if(@@rowcount =0)
							begin
								rollback transaction
								select @error_code ='249',@return_status =0
								return 0
							end
					end

				if(select count(study_hdr_id) from study_hdr_study_types where study_hdr_id=@study_id)>0
					begin
						delete from study_hdr_study_types
						where study_hdr_id=@study_id

						if(@@rowcount =0)
							begin
								rollback transaction
								select @error_code ='251',@return_status =0
								return 0
							end
					end

				if(select count(study_hdr_id) from study_hdr_dcm_files where study_hdr_id=@study_id)>0
					begin
						delete from study_hdr_dcm_files
						where study_hdr_id=@study_id

						if(@@rowcount =0)
							begin
								rollback transaction
								select @error_code = '330',@return_status =0
								return 0
							end
					end

				if(select count(study_hdr_id) from study_hdr_merged_studies where study_hdr_id=@study_id)>0
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
								select @error_code ='250', @return_status =0
								return 0
							end
				 
				  end

				if(select count(study_hdr_id) from study_hdr_dictated_reports where study_hdr_id=@study_id)>0
					begin
						delete from study_hdr_dictated_reports
						where study_hdr_id=@study_id

						if(@@rowcount =0)
							begin
								rollback transaction
								select @error_code ='394',
										@return_status =0
								return 0
							end
					end

				if(select count(study_hdr_id) from study_hdr_prelim_reports where study_hdr_id=@study_id)>0
					begin
						delete from study_hdr_prelim_reports
						where study_hdr_id=@study_id

						if(@@rowcount =0)
							begin
								rollback transaction
								select @error_code ='281',@return_status =0
								return 0
							end
					end

				if(select count(study_hdr_id) from  study_report_addendums where study_hdr_id = @study_id )>0
					begin
						delete from study_report_addendums where study_hdr_id = @study_id

						if(@@rowcount=0)
							begin
								rollback transaction
								select @error_code ='373',@return_status =0
								return 0
							end
					end

				if(select count(study_hdr_id) from study_hdr_final_reports where study_hdr_id=@study_id)>0
					begin
						delete from study_hdr_final_reports
						where study_hdr_id=@study_id

						if(@@rowcount =0)
							begin
								rollback transaction
								select @error_code ='282', @return_status =0
								return 0
							end
					end

				if(select count(id) from study_hdr where id=@study_id)>0
					 begin
					
						 delete from study_hdr where id = @study_id

						 if(@@rowcount =0)
								begin
									rollback transaction
									select @error_code ='122', @return_status =0
									return 0
								end
					 end
			end
		else if(select count(id) from study_hdr_archive where id = @study_id)>0
			begin
				insert into study_hdr_deleted(id,study_uid,study_date,received_date,synched_on,patient_name,institution_name,remarks,deleted_on,deleted_by)
				(select h.id,h.study_uid,h.study_date,h.received_date,h.synched_on,h.patient_name,isnull(i.name,''),'Deleted for invoice amendment by ' + @deleted_by,getdate(),@updated_by
				 from study_hdr_archive h
				 left outer join institutions i on i.id = h.institution_id
				 where h.id =@study_id)

				if(@@rowcount=0)
					begin
						rollback transaction
						select @error_code ='122',@return_status =0
						return 0
					end

				if(select count(study_hdr_id) from study_hdr_documents_archive where study_hdr_id=@study_id)>0
					begin
						delete from study_hdr_documents_archive
						where study_hdr_id=@study_id

						if(@@rowcount =0)
							begin
								rollback transaction
								select @error_code ='249',
									   @return_status =0
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
								select @error_code ='251',
									   @return_status =0
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

				if(select count(study_hdr_id) from study_hdr_merged_studies_archive where study_hdr_id=@study_id)>0
					begin
						delete from study_hdr_merged_studies_archive
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
								select @error_code ='250',@return_status =0
								return 0
							end

				
				  end

				if(select count(study_hdr_id) from study_hdr_dictated_reports_archive where study_hdr_id=@study_id)>0
					begin
						delete from study_hdr_dictated_reports
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
								select @error_code ='281',@return_status =0
								return 0
							end
					end

				if(select count(study_hdr_id) from  study_report_addendums_archive where study_hdr_id = @study_id )>0
					begin
						delete from study_report_addendums where study_hdr_id = @study_id

						if(@@rowcount=0)
							begin
								rollback transaction
								select @error_code ='373',@return_status =0
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
								select @error_code ='282',@return_status =0
								return 0
							end
					end

				if(select count(id) from study_hdr_archive where id=@study_id)>0
					 begin
						 delete from study_hdr_archive where id = @study_id

						 if(@@rowcount =0)
								begin
									rollback transaction
									select @error_code ='122', @return_status =0
									return 0
								end
					 end
			end

		if(select count(study_uid) from study_synch_dump where study_uid=@study_uid)>0
				begin
					delete from study_synch_dump where study_uid=@study_uid

					 if(@@rowcount =0)
						 begin
							rollback transaction
							select @error_code ='283',@return_status =0
							return 0
						 end
				end

		if(select count(id) from scheduler_file_downloads where id = @study_id)>0
			begin
				--insert into study_hdr_deleted(id,study_uid,study_date,patient_name,institution_name,deleted_on)
				--(select h.id,h.study_uid,h.study_date,rtrim(ltrim(isnull(h.patient_fname,'') + ' ' + isnull(h.patient_lname,''))),isnull(i.name,''),getdate()
				-- from scheduler_file_downloads h
				-- left outer join institutions i on i.id = h.institution_id
				-- where h.id =@study_id)

				--if(@@rowcount=0)
				--	begin
				--		rollback transaction
				--		select @error_code ='252',@return_status =0
				--		return 0
				--	end

				if(select count(id) from scheduler_file_downloads_dtls where id=@study_id)>0
					 begin
						 delete from scheduler_file_downloads_dtls where id = @study_id

						 if(@@rowcount =0)
								begin
									rollback transaction
									select @error_code ='252', @return_status =0
									return 0
								end
					 end
			
				if(select count(id) from scheduler_file_downloads where id=@study_id)>0
					 begin
						 delete from scheduler_file_downloads where id = @study_id

						 if(@@rowcount =0)
								begin
									rollback transaction
									select @error_code ='252',@return_status =0
									return 0
								end
					 end
			end

		if(select count(id) from scheduler_img_file_downloads_grouped where id = @study_id)>0
			begin
				
				--insert into study_hdr_deleted(id,study_uid,study_date,patient_name,institution_name,deleted_on)
				--(select h.id,h.study_uid,h.study_date,rtrim(ltrim(isnull(h.patient_fname,'') + ' ' + isnull(h.patient_lname,''))),isnull(i.name,''),getdate()
				-- from scheduler_img_file_downloads_grouped h
				-- left outer join institutions i on i.id = h.institution_id
				-- where h.id =@study_id)

				--if(@@rowcount=0)
				--	begin
				--		rollback transaction
				--		select @error_code ='284',@return_status =0
				--		return 0
				--	end

				if(select count(study_hdr_id) from scheduler_img_file_downloads_grouped_docs where study_hdr_id=@study_id)>0
					begin
						 delete from scheduler_img_file_downloads_grouped_docs where study_hdr_id = @study_id

						 if(@@rowcount =0)
								begin
									rollback transaction
									select @error_code ='284',@return_status =0
									return 0
								end
					 end

				if(select count(study_hdr_id) from scheduler_img_file_downloads_grouped_study_types where study_hdr_id=@study_id)>0
					begin
					
						 delete from scheduler_img_file_downloads_grouped_study_types where study_hdr_id = @study_id

						 if(@@rowcount = 0)
								begin
								
									rollback transaction
									select @error_code ='284',@return_status =0
									return 0
								end
					 end

				if(select count(id) from scheduler_file_downloads_dtls where id=@study_id)>0
					 begin
						 delete from scheduler_img_file_downloads_grouped where id = @study_id

						 if(@@rowcount =0)
								begin
									rollback transaction
									select @error_code ='284',@return_status =0
									return 0
								end
					 end
			
				if(select count(id) from scheduler_img_file_downloads_grouped where id=@study_id)>0
					 begin
						 delete from scheduler_img_file_downloads_grouped where id = @study_id

						 if(@@rowcount =0)
								begin
								
									rollback transaction
									select @error_code ='284',@return_status =0
									return 0
								end
					 end

				if(select count(id) from scheduler_img_file_downloads_ungrouped where grouped_id=@study_id)>0
					 begin
						 delete from scheduler_img_file_downloads_ungrouped where grouped_id = @study_id

						 if(@@rowcount =0)
								begin
									rollback transaction
									select @error_code ='285',@return_status =0
									return 0
								end
					 end

				if(select count(study_uid) from dicom_router_files_received where study_uid=@study_uid)>0
					begin
						delete from dicom_router_files_received where study_uid = @study_uid

						if(@@rowcount =0)
							begin
								rollback transaction
								select @error_code = '452',@return_status =0
								return 0
							end
					end
	
			end

		if(select count(study_hdr_id) from ar_amended_rates where study_hdr_id = @study_id)>0
			begin
				
				delete from ar_amended_rates where study_hdr_id = @study_id

				if(@@rowcount =0)
					begin
						rollback transaction
						select @error_code ='439',@return_status =0
						return 0
					end
	
			end

		if(select count(id) from invoice_institution_dtls where study_id = @study_id and billing_cycle_id=@billing_cycle_id)>0
			begin
				select @billing_account_id = billing_account_id
				from institutions
				where id = @institution_id

				set @user_name=''
				set @error_code =''
				set @return_status = 0

				exec ar_study_correction_invoice_reprocess
					@billing_cycle_id   = @billing_cycle_id,
					@billing_account_id = @billing_account_id,
					@menu_id            = 45,
					@user_id            = @updated_by,
					@user_name          = @user_name output,
					@error_code         = @error_code output,
					@return_status      = @return_status output

				if(@return_status=0)
					begin
						rollback transaction
						return 0
					end

			end
		
		commit transaction

	    set @return_status=1
	    set @error_code='287'
		set nocount off
		return 1
	end
GO
