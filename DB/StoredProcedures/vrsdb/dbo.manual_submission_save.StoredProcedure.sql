USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[manual_submission_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[manual_submission_save]
GO
/****** Object:  StoredProcedure [dbo].[manual_submission_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : manual_submission_save : save manual submission
** Created By   : Pavel Guha
** Created On   : 30/06/2020
*******************************************************/
CREATE PROCEDURE [dbo].[manual_submission_save] 
	@study_uid nvarchar(100),
	@series_instance_uid nvarchar(100) = '',
	@series_no nvarchar(100) = '',
	@session_id nvarchar(30),
	@study_date datetime,
	@patient_id nvarchar(20),
	@patient_fname nvarchar(40),
	@patient_lname nvarchar(40),
	@patient_weight decimal(12,3),
	@wt_uom nvarchar(5),
	@patient_dob datetime,
	@patient_age nvarchar(50),
	@patient_sex nvarchar(10),
	@patient_spayed_neutered nvarchar(30),
	@species_id int,
	@breed_id uniqueidentifier,
	@owner_first_name nvarchar(100)='',
	@owner_last_name nvarchar(100)='',
	@accession_no nvarchar(20) ='' output,
	@institution_id uniqueidentifier = '00000000-0000-0000-0000-000000000000',
	@physician_id uniqueidentifier = '00000000-0000-0000-0000-000000000000',
	@modality_id int,
	@priority_id int,
	@reason nvarchar(2000),
	@physician_note nvarchar(2000)='',
	@consult_applied nchar(1)='N',
	@category_id int,
	@TVP_studytypes as case_study_study_type readonly,
	@TVP_dcm as manual_submission_dcm_files readonly,
	@TVP_img as manual_submission_img_files readonly,
	@TVP_docs as case_study_doc_type readonly,
	@updated_by uniqueidentifier,
	@sender_time_offset_mins int=0,
	@submit_priority nchar(1)='N',
	@patient_country_id int =0,
	@patient_state_id int =0,
	@patient_city nvarchar(100)='',
	@user_session_id uniqueidentifier = '00000000-0000-0000-0000-000000000000',
	@next_operation_time nvarchar(130) = '' output,
	@delv_time nvarchar(130) = '' output,
	@message_display nvarchar(500) = '' output,
	@error_text nvarchar(300)='' output,
    @error_code nvarchar(500)='' output,
    @return_status int =0 output
as
begin
	set nocount on
	set datefirst 1

	declare @counter int,
			@rowcount int,
			@institution_name nvarchar(100),
			@institution_code nvarchar(5),
			@species_name nvarchar(30),
			@breed_name nvarchar(30),
			@modality_name nvarchar(30),
			@patient_name nvarchar(100),
			@owner_name nvarchar(100),
			@referring_physician nvarchar(100),
			@img_count int,
			@salesperson_id uniqueidentifier,
			@synched_on datetime,
			@finishing_datetime datetime,
			@finishing_time_hrs int,
			@final_rpt_release_datetime datetime,
			@final_rpt_release_hrs int,
			@FNLRPTAUTORELHR int,
			@service_codes nvarchar(250),
			@rc int,
			@id uniqueidentifier,
			@patient_id_srl int,
			@beyond_hour_stat nchar(1),
			@is_stat nchar(1),
			@priority_charged nchar(1),
			@error_msg nvarchar(500)

	declare @study_type_id uniqueidentifier,
	        @srl_no int,
			@field_code nvarchar(5),
			@file_count int

	declare @dcm_file_id uniqueidentifier,
			@dcm_file_name nvarchar(100),
			@dcm_file_srl_no int,
			@dcm_file varbinary(max)

	declare  @img_file_id uniqueidentifier,
			 @img_file_name nvarchar(100),
			 @img_file_srl_no int,
			 @img_file varbinary(max),
			 @file_name nvarchar(250)

	declare  @document_id uniqueidentifier,
			 @document_link nvarchar(100),
	         @document_name nvarchar(100),
			 @document_srl_no int,
			 @document_file_type nvarchar(5),
			 @document_file varbinary(max)

	declare	@beyond_operation_time nchar(1),
			@in_exp_list nchar(1)

	if(select rtrim(ltrim(isnull(code,'')))
	from institutions
	where id = @institution_id)=''
		begin
			select @return_status = 0,@error_code ='127'
			return 0
		end

	begin transaction	

	if(select count(study_uid) from study_synch_dump where study_uid=@study_uid)=0
		begin

		    select @is_stat = is_stat from sys_priority where priority_id=@priority_id
			set @is_stat = isnull(@is_stat,'N')
	
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
					rollback transaction
					return 0
				end

			 set @beyond_hour_stat='N'
			 select @FNLRPTAUTORELHR = data_type_string from general_settings where control_code='FNLRPTAUTORELHR'

			 if(@submit_priority='N')
				begin
					exec common_check_operation_time
						@priority_id             = @priority_id,
						@sender_time_offset_mins = @sender_time_offset_mins,
						@next_operation_time     = @next_operation_time output,
						@delv_time               = @delv_time output,
						@display_message         = @message_display output,
						@beyond_hour_stat        = @beyond_hour_stat output,
						@error_code              = @error_code output,
						@return_status           = @return_status output
						
					if(@return_status=0)
						begin
							if(@is_stat='Y' and @in_exp_list='N')
								begin
									rollback transaction
									return 0
								end
							else
								begin
									set @submit_priority='Y'
								end
						end

				end
			

			  select @institution_name = name,
				     @institution_code = code,
					 @patient_id_srl   = patient_id_srl
			  from institutions
			  where id = @institution_id

			  set @id= newid()
			  select @species_name = name from species where id = @species_id
			  select @breed_name = name from breed where id = @breed_id
			  select @modality_name = name from modality where id=@modality_id
			  select @referring_physician = name from physicians where id = @physician_id

			  if(@patient_fname like '%[^a-zA-Z0-9.&( )_-]%')
					begin
						rollback transaction
						select @error_code='238',@return_status=0
						return 0
					end
			  if(@patient_lname like '%[^a-zA-Z0-9.&( )_-]%')
					begin
						rollback transaction
						select @error_code='239',@return_status=0
						return 0
					end
			  if(@owner_first_name like '%[^a-zA-Z0-9.&( )_-]%')
					begin
						rollback transaction
						select @error_code='240',@return_status=0
						return 0
					end
			  if(@owner_last_name like '%[^a-zA-Z0-9.&( )_-]%')
					begin
						rollback transaction
						select @error_code='241',@return_status=0
						return 0
					end

			  if(rtrim(ltrim(isnull(@accession_no,'')))='')
				begin
					if(select count(accession_no) from study_hdr where accession_no = @accession_no and study_uid <> @study_uid)>0
						begin
							set @accession_no = right(@study_uid,15)
							set @accession_no =  REPLACE(@accession_no,'.','-')
						end
				end

			  set @patient_name = rtrim(ltrim(@patient_fname)) + ' ' + rtrim(ltrim(@patient_lname))
			  set @owner_name = rtrim(ltrim(@owner_first_name)) + ' ' + rtrim(ltrim(@owner_last_name))
			  select @img_count =count(dcm_file_name) from @TVP_dcm
			  select @img_count = @img_count + count(img_file_name) from @TVP_img

			  select @salesperson_id = salesperson_id
			  from institution_salesperson_link
			  where institution_id = @institution_id

			  if(@consult_applied = 'Y') 
				begin
					set @service_codes='CONSULT'
				end

			  set @synched_on= getdate()

			  insert into study_synch_dump(study_uid,study_date,received_date,accession_no,reason,
										   institution_name,manufacturer_name,manufacturer_model_no,device_serial_no,modality_ae_title,referring_physician,
										   patient_id,patient_name,patient_sex,patient_dob,patient_age,patient_weight,sex_neutered,
										   owner_name,species,breed,modality,body_part,img_count,study_desc,priority_id,object_count,synched_on)
							        values(@study_uid,@study_date,@synched_on,@accession_no,@reason,
										   @institution_name,'','','','',@referring_physician,
										   @patient_id,@patient_name,@patient_sex,@patient_dob,@patient_age,@patient_weight,@patient_spayed_neutered,
										   @owner_name,@species_name,@breed_name,@modality_name,'',@img_count,'',@priority_id,@img_count,getdate())

			  if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status=0,@error_code='340'
					return 0
				end

			  
			  select @finishing_time_hrs = finishing_time_hrs from sys_priority where priority_id = @priority_id
			  set @finishing_time_hrs = isnull(@finishing_time_hrs,0)

					  

			  if(@is_stat='Y')
					begin
						exec common_check_operation_time
							@priority_id             = @priority_id,
							@sender_time_offset_mins = @sender_time_offset_mins,
							@next_operation_time     = @next_operation_time output,
							@delv_time               = @delv_time output,
							@beyond_hour_stat        = @beyond_hour_stat output,
							@error_code              = @error_code output,
							@return_status           = @return_status output

					  if(@submit_priority='Y' and @in_exp_list='N')  
							begin
								set @finishing_datetime = convert(datetime,@delv_time)
							end
						else
							begin
								set @finishing_datetime = dateadd(HH,@finishing_time_hrs,getdate())
							end

						set @final_rpt_release_datetime = dateadd(mi,(select final_report_release_time_mins from sys_priority where priority_id=@priority_id),getdate())
					end
				else
					begin
					    select @finishing_datetime = dateadd(HH,@finishing_time_hrs,getdate())
						select @final_rpt_release_datetime = dateadd(HH,@FNLRPTAUTORELHR,getdate())
					end

			 -- if(isnull((select is_active from services where priority_id=@priority_id),'N'))='Y'
				--begin
					set @priority_charged='Y'
				--end
			 -- else
				--begin
				--	set @priority_charged='N'
				--end

              insert into study_hdr(id,study_uid,study_date,received_date,
									accession_no_pacs,accession_no,
									reason_pacs,reason_accepted,physician_note,
									institution_id,physician_id,
									institution_name_pacs,manufacturer_name,device_serial_no,modality_ae_title,referring_physician_pacs,
									patient_id_pacs,patient_id,
									patient_name_pacs,patient_name,patient_fname,patient_lname,
									patient_sex_pacs,patient_sex,
									patient_sex_neutered_pacs,patient_sex_neutered,
									patient_country_id,patient_state_id,patient_city,
									patient_dob_pacs,patient_dob_accepted,
									patient_age_pacs,patient_age_accepted,
									patient_weight_pacs,patient_weight,wt_uom,
									owner_name_pacs,owner_first_name,owner_last_name,
									species_id,breed_id,modality_id,category_id,
									species_pacs,breed_pacs,modality_pacs,body_part_pacs,study_status_pacs,study_status,
									img_count_pacs,img_count,object_count,object_count_pacs,study_desc,
									priority_id_pacs,priority_id,finishing_datetime,final_rpt_release_datetime,received_via_dicom_router,sync_mode,
									consult_applied,service_codes,beyond_hour_stat,priority_charged,
									synched_on,updated_by,date_updated,status_last_updated_on)
							values(@id,@study_uid,@study_date,@synched_on,
								   @accession_no,@accession_no,
								   @reason,@reason,@physician_note,
								   @institution_id,@physician_id,
								   @institution_name,'','','',@referring_physician,
								   @patient_id,@patient_id,
								   @patient_name,@patient_name,@patient_fname,@patient_lname,
								   @patient_sex,@patient_sex,
								   @patient_spayed_neutered,@patient_spayed_neutered,
								   @patient_country_id,@patient_state_id,@patient_city,
								   @patient_dob,@patient_dob,
								   @patient_age,@patient_age,
								   @patient_weight,@patient_weight,@wt_uom,
								   @owner_name,@owner_name,@owner_last_name,
								   @species_id,@breed_id,@modality_id,@category_id,
								   @species_name,@breed_name,@modality_name,'',50,2,
								   @img_count,@img_count,0,@img_count,'',
								   @priority_id,@priority_id,@finishing_datetime,@final_rpt_release_datetime,'M','MS',
								   @consult_applied,@service_codes,@beyond_hour_stat,@priority_charged,
								   @synched_on,@updated_by,getdate(),getdate())	

			  if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status=0,@error_code='035'
					return 0
				end

			  if(@wt_uom='lbs')
					begin
						update study_hdr
						set patient_weight_kgs = @patient_weight * 0.454
						where id = @id 

						if(@@rowcount = 0)
							begin
								rollback transaction
								select @return_status = 0,@error_code ='035'
								return 0
							end
					end
			  else if(@wt_uom='kgs')
				   begin
						update study_hdr
						set patient_weight_kgs = @patient_weight
						where id = @id 

						if(@@rowcount = 0)
							begin
								rollback transaction
								select @return_status = 0,@error_code ='035'
								return 0
							end
				   end

			--save Study Types
			create table #tmpWBT
			(
				srl_no int identity(1,1),
				field_code nvarchar(5)
			)
				
			--save DICOM files
			if(select count(dcm_file_name) from @TVP_dcm)>0
				begin
					select @rowcount = count(dcm_file_srl_no),
							@counter = 1
					from @TVP_dcm

					while(@counter <= @rowcount)
						begin
							select 	@dcm_file_name        = dcm_file_name,
									@dcm_file_srl_no      = dcm_file_srl_no,
									@dcm_file             = dcm_file
							from @TVP_dcm
							where dcm_file_srl_no= @counter 

							set @dcm_file_id = newid() 
							set @dcm_file_name = REPLACE(@dcm_file_name,'''','')
					
							insert into study_manual_upload_files(file_id,session_id,file_name,file_content,file_type,
																	institution_id,institution_code,institution_name,
																	uploaded_by,date_uploaded)
															values (@dcm_file_id,@session_id,@dcm_file_name,@dcm_file,'D',
																	@institution_id,@institution_code,@institution_name,
																	@updated_by,getdate())

							if(@@rowcount=0)
								begin
									rollback transaction
									select @error_code='334',@return_status=0
									return 0
								end

					
					
							set @counter = @counter + 1
					end

				end

			--save image files
			if(select count(img_file_name) from @TVP_img)>0
				begin
					insert into scheduler_img_file_downloads_grouped(id,study_uid,study_date,file_count,
																	 institution_id,institution_code,institution_name,
																	 patient_id,patient_fname,patient_lname,
																	 modality_id,category_id,modality,series_instance_uid,series_no,
																	 accession_no,reason,physician_id,patient_dob,patient_age,patient_sex,
																	 spayed_neutered,patient_weight,wt_uom,owner_first_name,owner_last_name,
																	 species_id,breed_id,priority_id,salesperson_id,physician_note,consult_applied,is_manual,
																	 created_by,date_created,approve_for_pacs,approved_by,date_approved)
															values(@id,@study_uid,@study_date,@img_count,
																	@institution_id,@institution_code,@institution_name,
																	@patient_id,@patient_fname,@patient_lname,
																	@modality_id,@category_id,@modality_name,@series_instance_uid,@series_no,
																	@accession_no,@reason,@physician_id,@patient_dob,@patient_age,@patient_sex,
																	@patient_spayed_neutered,@patient_weight,@wt_uom,@owner_first_name,@owner_last_name,
																	@species_id,@breed_id,@priority_id,isnull(@salesperson_id,'00000000-0000-0000-0000-000000000000'),@physician_note,@consult_applied,'Y',
																	@updated_by,@synched_on,'Y',@updated_by,getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							select @error_code='066',@return_status=0,@error_text=@img_file_name
							return 0
						end

			
					select @rowcount = count(img_file_srl_no),
							@counter = 1
					from @TVP_img

					select * from @TVP_img

					while(@counter <= @rowcount)
						begin
							select 	@img_file_name        = img_file_name,
									@img_file_srl_no      = img_file_srl_no,
									@img_file             = img_file
							from @TVP_img
							where img_file_srl_no= @counter 


							set @img_file_id = newid() 
							set @img_file_name = replace(@img_file_name,' ','_')
							set @img_file_name = REPLACE(@img_file_name,'''','')
					
							insert into study_manual_upload_files(file_id,session_id,file_name,file_content,file_type,
																	institution_id,institution_code,institution_name,
																	uploaded_by,date_uploaded)
															values (@img_file_id,@session_id,@img_file_name,@img_file,'I',
																	@institution_id,@institution_code,@institution_name,
																	@updated_by,getdate())

							if(@@rowcount=0)
								begin
									rollback transaction
									select @error_code='334',@return_status=0
									return 0
								end

						    set @file_name = @institution_code + '_' + @session_id + '_' + replace(@institution_name,' ','_') + '_' + @img_file_name
							set @file_name = replace(@file_name,' ','_')
							set @file_name = replace(@file_name,',','')
							set @file_name = replace(@file_name,'(','')
							set @file_name = replace(@file_name,')','')
							set @file_name = replace(@file_name,'''','')
							set @file_name = replace(@file_name,'"','')
							set @file_name = replace(@file_name,'/','_')
							set @file_name = replace(@file_name,'\','_')
							set @file_name = replace(@file_name,'#','')
							set @file_name = replace(@file_name,'&','')
							set @file_name = replace(@file_name,'@','')
							set @file_name = replace(@file_name,'?','')
							set @file_name = replace(@file_name,'__','_')

							while(charindex('__',@file_name))>0
								begin
									set @file_name = replace(@file_name,'__','_')
								end

							insert into scheduler_img_file_downloads_grouped_dtls(id,ungrouped_id,study_uid,file_name,series_instance_uid,series_no,import_session_id)
													       values(@id,'00000000-0000-0000-0000-000000000000',@study_uid,@file_name,@series_instance_uid,@series_no,@session_id)
					                                              

							if(@@rowcount=0)
								begin
									rollback transaction
									select @error_code='066',@return_status=0,@error_text=@img_file_name
									return 0
								end

							set @counter = @counter + 1
						end

				end

			insert into #tmpWBT(field_code) values ('DSCR')
			insert into #tmpWBT(field_code) values ('UDF4')
			insert into #tmpWBT(field_code) values ('UDF7')
			insert into #tmpWBT(field_code) values ('UDF9')

			select @rc= count(field_code) from #tmpWBT
			if(select count(study_type_id) from @TVP_studytypes)>0
				begin
					select @rowcount = count(study_type_id),
						   @counter  = 1
					from @TVP_studytypes

					while(@counter <= @rowcount)
						begin
							select @study_type_id  = study_type_id,
								   @srl_no         = srl_no
							from @TVP_studytypes
							where srl_no= @counter 

							insert into study_hdr_study_types(study_hdr_id,study_type_id,srl_no,updated_by,date_updated)
															values (@id,@study_type_id,@srl_no,@updated_by,getdate())

							if(@@rowcount=0)
								begin
									rollback transaction
									select @error_code='061',@return_status=0
									return 0
								end

							if(@rc>0)
							   begin
									select @field_code = field_code from #tmpWBT where srl_no = @counter

									if(isnull(@field_code,'')<>'')
										begin
											update study_hdr_study_types
											set write_back_tag = @field_code
											where study_hdr_id=@id
											and study_type_id = @study_type_id

											if(@@rowcount=0)
												begin
													rollback transaction
													select @error_code='062',@return_status=0
													return 0
												end
										end
							   end

							set @counter = @counter + 1
						end
				end

			drop table #tmpWBT


            --save documents
			if(select count(document_id) from @TVP_docs)>0
			   begin
					select @rowcount = count(document_id),
						   @counter = 1
				    from @TVP_docs

					while(@counter <= @rowcount)
						begin
							select @document_id          = document_id,
								   @document_name        = document_name,
								   @document_srl_no      = document_srl_no,
								   @document_link        = document_link,
								   @document_file_type   = document_file_type,
								   @document_file        = document_file
							from @TVP_docs
							where document_srl_no= @counter 

							set @document_id = newid() 

							insert into study_hdr_documents(study_hdr_id,document_id,document_name,document_srl_no,
																document_link,document_file_type,document_file,created_by,date_created)
															values (@id,@document_id,@document_name,@document_srl_no,
																	@document_link,@document_file_type,@document_file,@updated_by,getdate())

							if(@@rowcount=0)
								begin
									rollback transaction
									select @error_code='036',@return_status=0
									return 0
								end

							set @counter = @counter + 1
						end
			   end

			insert into sys_case_study_status_log(study_id,study_uid,status_id_from,status_id_to,date_updated,updated_by)
										values(@id,@study_uid,10,50,getdate(),@updated_by)

			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_code='142',@return_status=0
					return 0
				end


			if(@patient_id = @institution_code + '-' + convert(varchar,@patient_id_srl + 1))
				begin
					update institutions
					set patient_id_srl = patient_id_srl + 1
					where id = @institution_id
				end

			exec common_study_user_activity_trail_save
				@study_hdr_id = @id,
				@study_uid    = @study_uid,
				@menu_id      = 0,
				@activity_text = 'Created Via Manual Submission',
				@session_id    = @user_session_id,
				@activity_by   = @updated_by,
				@error_code    = @error_code output,
				@return_status = @return_status output

		   if(@return_status=0)
			begin
				rollback transaction
				return 0
			end

		   exec notification_study_file_sync_pending_create
				@id = @id,
				@error_msg = @error_msg output,
				@return_type = @return_status output

			if(@return_status=0)
				begin
					rollback transaction
					select @error_code='487',@return_status=0
					return 0
				end

			/**********Generate STAT Study submit mail notification**********/
			if(@priority_id=10 or @priority_id=30)
				begin
					exec notification_rule_0_create
						@id = @id,
						@error_msg = @error_msg output,
						@return_type = @return_status output

					--if(@return_status=0)
					--	begin
					--		rollback transaction
					--		select @error_code='487',@return_status=0
					--		return 0
					--	end
			    end

				/**********Generate notification rule notifications**********/
				declare @email_count int,
				        @sms_count int

				set @email_count= 0
				set @sms_count  = 0
				exec scheduler_notification_rule_notification_create
					@email_count = @email_count output,
					@sms_count   = @sms_count output
		end
	else
		begin
			rollback transaction
			select @error_code='139',@return_status=0
			set nocount off
			return 0
		end

	commit transaction
	select @error_code='335',@return_status=1
	set nocount off
	return 1
	
end

GO
