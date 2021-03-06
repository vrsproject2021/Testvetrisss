USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_study_delete]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_study_delete]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_study_delete]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_study_delete : save scheduler log
** Created By   : Pavel Guha
** Created On   : 30/04/2019
*******************************************************/
--exec scheduler_study_delete '2.25.51717228310870082330851092548469660061','',0
CREATE procedure [dbo].[scheduler_study_delete]
    @study_uid nvarchar(100),
	@institution_code nvarchar(5)='' output,
	@institution_name nvarchar(100)='' output,
	@PACSARCHIVEFLDR nvarchar(200)='' output,
	@FTPDLFLDRTMP nvarchar(200)='' output,
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	set nocount on
	declare @id uniqueidentifier,
	        @remarks nvarchar(250),
			@institution_id uniqueidentifier,
			@error_code nvarchar(10),
			@return_status int,
			@activity_text nvarchar(max)



    select @PACSARCHIVEFLDR = data_type_string from general_settings where control_code='PACSARCHIVEFLDR'
	select @FTPDLFLDRTMP = data_type_string from general_settings where control_code='FTPDLFLDRTMP'

	if(select count(study_uid) from scheduler_study_to_delete where study_uid = @study_uid)>0
		begin
			set @remarks='Synched 3 days before.No action taken by institution'
		end
	else
		begin
			set @remarks='Study not found in PACS'
		end

	if(isnull((select ih.approved
	           from invoice_hdr ih
			   inner join invoice_institution_dtls iid on iid.hdr_id = ih.id
	           where iid.study_uid=@study_uid),'N'))='Y'
				  begin
						select @error_msg ='Cannot delete Study UID : ' + @study_uid + ', It is already invoiced and approved',
						       @return_type =0
					    return 0
				  end

	begin transaction

	if(select count(id) from study_hdr where study_uid = @study_uid)>0
		begin
			
			select @id=id,
			       @institution_id = isnull(institution_id,'00000000-0000-0000-0000-000000000000')
			from study_hdr
			where study_uid = @study_uid

			set @id=isnull(@id,'00000000-0000-0000-0000-000000000000')

			select @institution_code = isnull(code,''),
			       @institution_name = isnull(name,'')
			from institutions
			where id= @institution_id

			insert into study_hdr_deleted(id,study_uid,study_date,received_date,synched_on,patient_name,institution_name,remarks,deleted_on)
			(select h.id,h.study_uid,h.study_date,h.received_date,h.synched_on,h.patient_name,isnull(i.name,isnull(h.institution_name_pacs,'')),@remarks,getdate()
			 from study_hdr h
			 left outer join institutions i on i.id = h.institution_id
			 where h.study_uid =@study_uid)

			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_msg ='Error deleting study details of Study UID : ' + @study_uid,
						   @return_type =0
					return 0
				end

			
			if(select count(study_hdr_id) from study_hdr_study_types where study_hdr_id=@id)>0
				begin
					delete from study_hdr_study_types
					where study_hdr_id=@id

					if(@@rowcount =0)
						begin
							rollback transaction
							select @error_msg ='Error deleting study type(s) of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end
				end
			
			if(select count(study_hdr_id) from study_hdr_documents where study_hdr_id=@id)>0
				begin
					delete from study_hdr_documents
					where study_hdr_id=@id

					if(@@rowcount =0)
						begin
							rollback transaction
							select @error_msg ='Error deleting documents of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end
				end

			if(select count(study_hdr_id) from study_hdr_dcm_files where study_hdr_id=@id)>0
				begin
					delete from study_hdr_dcm_files
					where study_hdr_id=@id

					if(@@rowcount =0)
						begin
							rollback transaction
							select @error_msg ='Error deleting DCM File(s) of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end
				end

			if(select count(study_hdr_id) from study_hdr_merged_studies where study_hdr_id=@id)>0
				begin
					delete from study_hdr_merged_studies
					where study_hdr_id=@id

					if(@@rowcount =0)
						begin
							rollback transaction
							select @error_msg ='Error deleting merged study(ies) of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end
				end

		    if(select count(study_id) from sys_case_study_status_log where study_id=@id)>0
			  begin
				 delete from sys_case_study_status_log where study_id = @id

				  if(@@rowcount =0)
						begin
							rollback transaction
							select @error_msg ='Error deleting status log of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end

				 
			  end

			if(select count(study_hdr_id) from study_hdr_dictated_reports where study_hdr_id=@id)>0
				begin
					delete from study_hdr_dictated_reports
					where study_hdr_id=@id

					if(@@rowcount =0)
						begin
							rollback transaction
							select @error_msg ='Error deleting dication report data of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end
				end

			if(select count(study_hdr_id) from study_hdr_prelim_reports where study_hdr_id=@id)>0
				begin
					delete from study_hdr_prelim_reports
					where study_hdr_id=@id

					if(@@rowcount =0)
						begin
							rollback transaction
							select @error_msg ='Error deleting preliminary report data of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end
				end

			if(select count(study_hdr_id) from study_hdr_final_reports where study_hdr_id=@id)>0
				begin
					delete from study_hdr_final_reports
					where study_hdr_id=@id

					if(@@rowcount =0)
						begin
							rollback transaction
							select @error_msg ='Error deleting final report data of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end
				end

			if(select count(study_hdr_id) from  study_report_addendums where study_hdr_id = @id )>0
				begin
					delete from study_report_addendums where study_hdr_id = @id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @error_msg ='Error deleting report addendum data of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end
				end

			if(select count(id) from study_hdr where id=@id)>0
				 begin
					
					 delete from study_hdr where id = @id

					 if(@@rowcount =0)
							begin
								
								--exec scheduler_study_delete '2.25.51717228310870082330851092548469660061','',0

								rollback transaction
								select @error_msg ='Error deleting Study UID : ' + @study_uid,
									   @return_type =0

									   print @error_msg
								return 0
							end

					exec common_study_user_activity_trail_save
						@study_hdr_id = @id,
						@study_uid    ='',
						@menu_id      = 0,
						@activity_text = 'Deleted',
						@activity_by   = '00000000-0000-0000-0000-000000000000',
						@error_code    = @error_msg output,
						@return_status = @return_type output

					if(@return_type=0)
						begin
							rollback transaction
							return 0
						end
				 end

			
		end
	else if(select count(id) from study_hdr_archive where study_uid = @study_uid)>0
		begin
			select @id=id,
			       @institution_id = isnull(institution_id,'00000000-0000-0000-0000-000000000000')
			from study_hdr_archive 
			where study_uid = @study_uid

			set @id=isnull(@id,'00000000-0000-0000-0000-000000000000')

			select @institution_code = isnull(code,''),
			       @institution_name = isnull(name,'')
			from institutions
			where id= @institution_id

			insert into study_hdr_deleted(id,study_uid,study_date,received_date,synched_on,patient_name,institution_name,remarks,deleted_on)
			(select h.id,h.study_uid,h.study_date,h.received_date,h.synched_on,h.patient_name,isnull(i.name,isnull(h.institution_name_pacs,'')),@remarks,getdate()
			 from study_hdr_archive h
			 left outer join institutions i on i.id = h.institution_id
			 where h.study_uid =@study_uid)

			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_msg ='Error deleting study details of Study UID : ' + @study_uid,
						   @return_type =0
					return 0
				end

			if(select count(study_hdr_id) from study_hdr_study_types_archive where study_hdr_id=@id)>0
				begin
					delete from study_hdr_study_types_archive
					where study_hdr_id=@id

					if(@@rowcount =0)
						begin
							rollback transaction
							select @error_msg ='Error deleting study type(s) of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end
				end

			if(select count(study_hdr_id) from study_hdr_documents_archive where study_hdr_id=@id)>0
				begin
					delete from study_hdr_documents
					where study_hdr_id=@id

					if(@@rowcount =0)
						begin
							rollback transaction
							select @error_msg ='Error deleting documents of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end
				end

			if(select count(study_hdr_id) from study_hdr_dcm_files_archive where study_hdr_id=@id)>0
				begin
					delete from study_hdr_dcm_files_archive
					where study_hdr_id=@id

					if(@@rowcount =0)
						begin
							rollback transaction
							select @error_msg ='Error deleting documents of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end
				end

			if(select count(study_hdr_id) from study_hdr_merged_studies_archive where study_hdr_id=@id)>0
				begin
					delete from study_hdr_merged_studies_archive
					where study_hdr_id=@id

					if(@@rowcount =0)
						begin
							rollback transaction
							select @error_msg ='Error deleting merged study(ies) of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end
				end

		    if(select count(study_id) from sys_case_study_status_log where study_id=@id)>0
			  begin
				 delete from sys_case_study_status_log where study_id = @id

				  if(@@rowcount =0)
						begin
							rollback transaction
							select @error_msg ='Error deleting status log of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end

				
			  end

            if(select count(study_hdr_id) from study_hdr_dictated_reports_archive where study_hdr_id=@id)>0
				begin
					delete from study_hdr_dictated_reports
					where study_hdr_id=@id

					if(@@rowcount =0)
						begin
							rollback transaction
							select @error_msg ='Error deleting dication report data of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end
				end

			if(select count(study_hdr_id) from study_hdr_prelim_reports_archive where study_hdr_id=@id)>0
				begin
					delete from study_hdr_prelim_reports_archive
					where study_hdr_id=@id

					if(@@rowcount =0)
						begin
							rollback transaction
							select @error_msg ='Error deleting preliminary report data of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end
				end

			if(select count(study_hdr_id) from study_hdr_final_reports_archive where study_hdr_id=@id)>0
				begin
					delete from study_hdr_final_reports_archive
					where study_hdr_id=@id

					if(@@rowcount =0)
						begin
							rollback transaction
							select @error_msg ='Error deleting final report data of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end
				end

			if(select count(study_hdr_id) from  study_report_addendums_archive where study_hdr_id = @id )>0
				begin
					delete from study_report_addendums where study_hdr_id = @id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @error_msg ='Error deleting report addendum data of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end
				end

			if(select count(id) from study_hdr_archive where id=@id)>0
				 begin
					 delete from study_hdr_archive where id = @id

					 if(@@rowcount =0)
							begin
								rollback transaction
								select @error_msg ='Error deleting Study UID : ' + @study_uid,
									   @return_type =0
								return 0
							end

					exec common_study_user_activity_trail_save
						@study_hdr_id = @id,
						@study_uid    ='',
						@menu_id      = 0,
						@activity_text = 'Deleted',
						@activity_by   = '00000000-0000-0000-0000-000000000000',
						@error_code    = @error_msg output,
						@return_status = @return_type output

					if(@return_type=0)
						begin
							rollback transaction
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
					select @error_msg ='Error deleting dump of Study UID : ' + @study_uid,
							@return_type =0
					return 0
					end
		end

	if(select count(id) from scheduler_file_downloads where study_uid = @study_uid)>0
		begin
			select @id=id
			from scheduler_file_downloads 
			where study_uid = @study_uid

			set @id=isnull(@id,'00000000-0000-0000-0000-000000000000')

			insert into study_hdr_deleted(id,study_uid,study_date,patient_name,institution_name,deleted_on)
			(select h.id,h.study_uid,h.study_date,rtrim(ltrim(isnull(h.patient_fname,'') + ' ' + isnull(h.patient_lname,''))),isnull(i.name,''),getdate()
			 from scheduler_file_downloads h
			 left outer join institutions i on i.id = h.institution_id
			 where h.study_uid =@study_uid)

			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_msg ='Error deleting study files of Study UID : ' + @study_uid,
						   @return_type =0
					return 0
				end

			
			if(select count(id) from scheduler_file_downloads_dtls where id=@id)>0
				 begin
					 delete from scheduler_file_downloads_dtls where id = @id

					 if(@@rowcount =0)
							begin
								rollback transaction
								select @error_msg ='Error deleting study files UID : ' + @study_uid,
									   @return_type =0
								return 0
							end
				 end
			
			if(select count(id) from scheduler_file_downloads where id=@id)>0
				 begin
					 delete from scheduler_file_downloads where id = @id

					 if(@@rowcount =0)
							begin
								rollback transaction
								select @error_msg ='Error deleting Study UID : ' + @study_uid,
									   @return_type =0
								return 0
							end
				 end
		end

	if(select count(id) from scheduler_img_file_downloads_grouped where study_uid = @study_uid)>0
		begin
			select @id=id
			from scheduler_img_file_downloads_grouped 
			where study_uid = @study_uid

			set @id=isnull(@id,'00000000-0000-0000-0000-000000000000')

			insert into study_hdr_deleted(id,study_uid,study_date,patient_name,institution_name,deleted_on)
			(select h.id,h.study_uid,h.study_date,rtrim(ltrim(isnull(h.patient_fname,'') + ' ' + isnull(h.patient_lname,''))),isnull(i.name,''),getdate()
			 from scheduler_img_file_downloads_grouped h
			 left outer join institutions i on i.id = h.institution_id
			 where h.study_uid =@study_uid)

			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_msg ='Error deleting study image files of Study UID : ' + @study_uid,
						   @return_type =0
					return 0
				end

			if(select count(study_hdr_id) from scheduler_img_file_downloads_grouped_docs where study_hdr_id=@id)>0
				begin
					 delete from scheduler_img_file_downloads_grouped_docs where study_hdr_id = @id

					 if(@@rowcount =0)
							begin
								rollback transaction
								select @error_msg ='Error deleting study image files of Study UID : ' + @study_uid,
									   @return_type =0
								return 0
							end
				 end

			if(select count(study_hdr_id) from scheduler_img_file_downloads_grouped_study_types where study_hdr_id=@id)>0
				begin
					
					 delete from scheduler_img_file_downloads_grouped_study_types where study_hdr_id = @id

					 if(@@rowcount = 0)
							begin
								
								rollback transaction
								select @error_msg ='Error deleting study image files study type of Study UID : ' + @study_uid,
									   @return_type =0

									print @error_msg
								return 0
							end
				 end

			if(select count(id) from scheduler_img_file_downloads_grouped_dtls where id=@id)>0
				 begin
					 delete from scheduler_img_file_downloads_grouped_dtls where id = @id

					 if(@@rowcount =0)
							begin
								rollback transaction
								select @error_msg ='Error deleting study image files study type of Study UID : ' + @study_uid,
									   @return_type =0
								return 0
							end
				 end
			
			if(select count(id) from scheduler_img_file_downloads_grouped where id=@id)>0
				 begin
					 delete from scheduler_img_file_downloads_grouped where id = @id

					 if(@@rowcount =0)
							begin
								
								rollback transaction
								select @error_msg ='Error deleting study image files header of Study UID : ' + @study_uid,
									   @return_type =0
								return 0
							end
				 end

			if(select count(id) from scheduler_img_file_downloads_ungrouped where grouped_id=@id)>0
				 begin
					 delete from scheduler_img_file_downloads_ungrouped where grouped_id = @id

					 if(@@rowcount =0)
							begin
								
								rollback transaction
								select @error_msg ='Error deleting study image files header (ungrouped) of Study UID : ' + @study_uid,
									   @return_type =0
								return 0
							end
				 end
	
		end

	if(select count(study_uid) from scheduler_study_to_delete where study_uid = @study_uid)>0
		begin
			delete from scheduler_study_to_delete where study_uid = @study_uid

			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_msg ='Error deleting record from deletion list for Study UID : ' + @study_uid,
						   @return_type =0
					return 0
				end
		end
    
	if(select count(study_uid) from dicom_router_files_received where study_uid=@study_uid)>0
		begin
			delete from dicom_router_files_received where study_uid = @study_uid

			if(@@rowcount =0)
				begin
					rollback transaction
					select @error_msg = 'Error deleting dicom router refence entrie for Study UID : ' + @study_uid,
					       @return_type =0
					return 0
				end
		end


		select @error_code='',@return_status=0
		set @activity_text='Deleted'
		exec common_study_user_activity_trail_save
			@study_hdr_id  = @id,
			@study_uid     = @study_uid,
			@menu_id       = 0,
			@activity_text = @activity_text,
			@activity_by   = '00000000-0000-0000-0000-000000000000',
			@error_code    = @error_code output,
			@return_status = @return_status output

		if(@return_status=0)
			begin
				rollback transaction
				return 0
			end
		
	

    commit transaction
	select @error_msg ='Study UID :' + @study_uid + ' deleted' ,@return_type = 1
	set nocount off
	
	return 1

end


GO
