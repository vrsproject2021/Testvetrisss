USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_req_action_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_req_action_save]
GO
/****** Object:  StoredProcedure [dbo].[case_list_req_action_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_req_action_save : update case list record
** Created By   : Pavel Guha
** Created On   : 11/04/2019
*******************************************************/
CREATE PROCEDURE [dbo].[case_list_req_action_save] 
    @id uniqueidentifier,
	@patient_id nvarchar(20),
	@patient_fname nvarchar(40),
	@patient_lname nvarchar(40),
	@patient_weight decimal(12,3),
	@patient_dob_accepted datetime,
	@patient_age_accepted nvarchar(50),
	@patient_sex nvarchar(10),
	@patient_sex_neutered nvarchar(30),
	@species_id int,
	@breed_id uniqueidentifier,
	@owner_first_name nvarchar(100)='',
	@owner_last_name nvarchar(100)='',
	@accession_no nvarchar(20) ='' output,
	@modality_id int,
	@reason_accepted nvarchar(2000),
	@img_count int,
	@img_count_accepted nchar(1),
	@institution_id uniqueidentifier = '00000000-0000-0000-0000-000000000000',
	@physician_id uniqueidentifier = '00000000-0000-0000-0000-000000000000',
	@TVP_studytypes as case_study_study_type readonly,
	@TVP_docs as case_study_doc_type readonly,
	@TVP_dcm as case_study_dcm_files readonly,
	@TVP_merged as case_study_merged readonly,
	@pacs_wb nchar(1),
	@wt_uom nvarchar(5),
	@priority_id int,
	@object_count int,
	@updated_by uniqueidentifier,
	@menu_id int,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@accn_no_generated nchar(1)='N' output,
	@studies_merged nchar(1)='X',
	@physician_note nvarchar(2000)='',
	@consult_applied nchar(1)='N',
	@category_id int,
	@sender_time_offset_mins int=0,
	@submit_priority nchar(1)='N',
	@patient_country_id int=0,
	@patient_state_id int=0,
	@patient_city nvarchar(100)='',
	@delv_time nvarchar(130) = '' output,
	@message_display nvarchar(500) = '' output,
	@institution_name nvarchar(100)='' output,
	@institution_code nvarchar(5) = '' output,
	@user_name nvarchar(130)='' output,
    @error_code nvarchar(500)='' output,
    @return_status int =0 output
as
begin
	set nocount on
	set datefirst 1

	declare @counter int,
			@rowcount int,
			@salesperson_id uniqueidentifier,
			@study_uid nvarchar(100),
			@received_date datetime,
			@flg int,
			@rc int,
			@ctr int,
			@study_hdr_id uniqueidentifier,
			@suid nvarchar(100),
			@merge_status nchar(1),
			@merge_status_desc nvarchar(max),
			@patient_sex_pacs nvarchar(10),
			@patient_name_pacs nvarchar(100),
			@received_via_dicom_router nchar(1),
			@service_codes nvarchar(250),
			@finishing_datetime datetime,
			@finishing_time_hrs int,
			@beyond_hour_stat nchar(1),
			@final_rpt_release_datetime datetime,
			@final_rpt_release_hrs int,
			@FNLRPTAUTORELHR int,
			@def_asn_radiologist_id uniqueidentifier,
			@def_asn_radiologist_name nvarchar(200),
			@study_img_count int,
			@image_count int,
			@obj_count int,
			@is_stat nchar(1),
			@priority_charged nchar(1),
			@sync_mode nvarchar(5),
			@error_msg nvarchar(500)

	declare  @document_id uniqueidentifier,
			 @document_link nvarchar(100),
	         @document_name nvarchar(100),
			 @document_srl_no int,
			 @document_file_type nvarchar(5),
			 @document_file varbinary(max)

	declare @dcm_file_id uniqueidentifier,
	        @dcm_file_name nvarchar(100),
			@dcm_file_srl_no int,
			@dcm_file varbinary(max),
			@xfer nchar(1)

	declare @study_type_id uniqueidentifier,
	        @srl_no int,
			@field_code nvarchar(5),
			@file_count int

	declare	@beyond_operation_time nchar(1),
			@in_exp_list nchar(1)

	if(select count(id) from study_hdr where id = @id)>0
		begin
				if(select study_status_pacs from study_hdr where id = @id)>0
					begin
						select @error_code='494',@return_status=0
						return 0
					end
				
				select @institution_name = name,
				       @institution_code = code
				from institutions
				where id = @institution_id

				if(isnull(@institution_code,'') = '')
					begin
						select @error_code='127',@return_status=0
						return 0
					end

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
						return 0
					end

				set @beyond_hour_stat='N'
				select @FNLRPTAUTORELHR = data_type_string from general_settings where control_code='FNLRPTAUTORELHR'
				
				if(@pacs_wb = 'Y' and @submit_priority='N')
					begin
						exec common_check_operation_time
						    @priority_id             = @priority_id,
							@sender_time_offset_mins = @sender_time_offset_mins,
							@next_operation_time     = @user_name output,
							@delv_time               = @delv_time output,
							@display_message         = @message_display output,
							@beyond_hour_stat        = @beyond_hour_stat output,
							@error_code              = @error_code output,
							@return_status           = @return_status output
						
						if(@return_status=0)
							begin
								if(@is_stat='Y' and @in_exp_list='N')
									begin
										return 0
									end
								else
									begin
										set @submit_priority='Y'
									end
							end

					end
				

				begin transaction	

				set @accn_no_generated = 'N'
				--set @studies_merged    = 'N'

				select @study_uid                 = study_uid,
				       @received_date             = synched_on,
					   @service_codes             = isnull(service_codes,''),
					   @received_via_dicom_router = received_via_dicom_router,
					   @sync_mode                 = sync_mode
				from study_hdr
				where id = @id

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


				if(rtrim(ltrim(isnull(@accession_no,'')))<>'')
					begin
						if(select count(accession_no) from study_hdr where accession_no = @accession_no and study_uid <> @study_uid)>0
							begin
								set @accn_no_generated='Y'
								set @accession_no = right(@study_uid,15)
								set @accession_no =  REPLACE(@accession_no,'.','-')
							end
					end
				else
					begin
						set @accn_no_generated='Y'
						set @accession_no = right(@study_uid,15)
						set @accession_no =  REPLACE(@accession_no,'.','-')
					end

				select @salesperson_id = salesperson_id
				from institution_salesperson_link
				where institution_id = @institution_id

				select @def_asn_radiologist_id = id,
				       @def_asn_radiologist_name= name
				from radiologists
				where assign_merged_study = 'Y'

				select @def_asn_radiologist_id = isnull(@def_asn_radiologist_id,'00000000-0000-0000-0000-000000000000'),
				       @def_asn_radiologist_name = isnull(@def_asn_radiologist_name,'')

				if(@consult_applied = 'Y') 
					begin
						if(rtrim(ltrim(@service_codes))='')
							begin
								set @service_codes='CONSULT'
							end
						else
							begin
								if(charindex('CONSULT',rtrim(ltrim(@service_codes)))=0)
									begin
										set @service_codes=@service_codes + ',CONSULT'
									end
							end
					end
				

				select @finishing_time_hrs = finishing_time_hrs from sys_priority where priority_id = @priority_id
				set @finishing_time_hrs = isnull(@finishing_time_hrs,0)

				if(@is_stat='Y')
					  begin
							exec common_check_operation_time
								@priority_id             = @priority_id,
								@sender_time_offset_mins = @sender_time_offset_mins,
								@next_operation_time     = @user_name output,
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

				

				--if(isnull((select is_active from services where priority_id=@priority_id),'N'))='Y'
				--	begin
				--		set @priority_charged='Y'
				--	end
				--else
				--	begin
				--		set @priority_charged='N'
				--	end

				set @priority_charged='Y'
				
				 if(@received_via_dicom_router='Y') set @sync_mode ='DR'

				update study_hdr
				set     patient_id                 = @patient_id,
						patient_name               = @patient_lname + ' ' + @patient_fname,
						patient_fname              = @patient_fname,
						patient_lname              = @patient_lname,
						patient_country_id         = @patient_country_id,
						patient_state_id           = @patient_state_id,
						patient_city               = @patient_city,
						patient_weight             = @patient_weight,
						patient_dob_accepted       = @patient_dob_accepted,
						patient_age_accepted       = @patient_age_accepted,
						patient_sex                = @patient_sex,
						patient_sex_neutered       = @patient_sex_neutered,
						species_id                 = @species_id,
						breed_id                   = @breed_id,
						owner_first_name           = @owner_first_name,
						owner_last_name            = @owner_last_name,
						accession_no               = @accession_no,
						priority_id                = @priority_id,
						modality_id                = @modality_id,
						category_id                = @category_id,
						reason_accepted            = @reason_accepted,
						img_count_pacs             = @img_count,
						img_count                  = @img_count,
						--object_count               = 0,
						object_count_pacs          = @object_count,
						img_count_accepted         = @img_count_accepted,
						institution_id             = @institution_id,
						physician_id               = @physician_id,
						salesperson_id             = isnull(@salesperson_id,'00000000-0000-0000-0000-000000000000'),
						wt_uom                     = @wt_uom,
						physician_note             = @physician_note,
						consult_applied            = @consult_applied,
						service_codes              = @service_codes,
						sync_mode                  = isnull(@sync_mode,'PACS'),
						finishing_datetime         = @finishing_datetime,
						final_rpt_release_datetime = @final_rpt_release_datetime,
						beyond_hour_stat           = @beyond_hour_stat,
						priority_charged           = @priority_charged,
						updated_by                 = @updated_by,
						date_updated               = getdate()
				where id = @id 

				if(@@rowcount = 0)
					begin
						rollback transaction
						select @return_status = 0,@error_code ='035'
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
						set patient_weight_kgs = @patient_weight,
							patient_weight     = @patient_weight/0.454
						where id = @id 

						if(@@rowcount = 0)
							begin
								rollback transaction
								select @return_status = 0,@error_code ='035'
								return 0
							end
					end

	
				--Save study types
				create table #tmpWBT
				(
					srl_no int identity(1,1),
					field_code nvarchar(5)
				)

				insert into #tmpWBT(field_code) (select field_code from sys_pacs_query_fields where service_id=2 and display_index in (18,19,20,21))

				select @rc= @@rowcount

				delete from study_hdr_study_types  where study_hdr_id=@id

				if(select count(study_type_id) from @TVP_studytypes)>0
					begin
			
						select @rowcount = count(study_type_id),
								@counter = 1
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
				create table #tmpDocs
				(id uniqueidentifier not null,
				 document_id uniqueidentifier not null,
				 document_name nvarchar(100) not null,
				 document_srl_no int not null,
				 document_link nvarchar(100) not null,
				 document_file_type nvarchar(5) not null,
				 document_file varbinary(max) null)

				insert into #tmpDocs(id,document_id,document_name,document_srl_no,
										document_link,document_file_type,document_file)
									(select study_hdr_id,document_id,document_name,document_srl_no,
											document_link,document_file_type,document_file
										from study_hdr_documents
										where study_hdr_id = @id)

				delete from study_hdr_documents where study_hdr_id = @id

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

								if(@document_id = '00000000-0000-0000-0000-000000000000') set @document_id = newid() 
					
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
					

								if(select count(document_link) from #tmpDocs where document_link = @document_link)=0
									begin

										insert into #tmpDocs(id,document_id,document_name,document_srl_no,
																document_link,document_file_type,document_file)
													values (@id,@document_id,@document_name,@document_srl_no,
															@document_link,@document_file_type,@document_file)

										if(@@rowcount=0)
											begin
												rollback transaction
												select @error_code='036',@return_status=0
												return 0
											end
									end

								set @counter = @counter + 1
						end

					end

				create table #tmpDocsDel
				(row_id int identity(1,1) not null,
				 document_id uniqueidentifier not null)

				insert into #tmpDocsDel(document_id)
				(select document_id 
					from #tmpDocs
					where document_id not in (select document_id from @TVP_docs where document_id <> '00000000-0000-0000-0000-000000000000'
											union select document_id from #tmpDocs))

				select @rowcount = @@rowcount,
					   @counter  = 1

				while(@counter <= @rowcount)
					begin
						select @document_id          = document_id
						from #tmpDocsDel
						where row_id= @counter
					
						select  @document_link = document_link
						from study_hdr_documents
						where document_id   = @document_id
						and study_hdr_id    = @id

						delete from study_hdr_documents
						where document_id =@document_id

						if(@@rowcount=0)
							begin
								rollback transaction
								select @error_code='036',@return_status=0
								return 0
							end

						set @counter = @counter + 1
					end

				drop table #tmpDocsDel
				drop table #tmpDocs

				--save dcm files
				create table #tmpDCM
				(id uniqueidentifier not null,
				 dcm_file_id uniqueidentifier not null,
				 dcm_file_name nvarchar(100) not null,
				 dcm_file_srl_no int not null,
				 dcm_file varbinary(max) null)

				insert into #tmpDCM(id,dcm_file_id,dcm_file_name,dcm_file_srl_no,dcm_file)
									(select study_hdr_id,dcm_file_id,dcm_file_name,dcm_file_srl_no,dcm_file
									 from study_hdr_dcm_files
									 where study_hdr_id = @id)

				delete from study_hdr_dcm_files where study_hdr_id = @id

				set @xfer =@pacs_wb

				if(select count(dcm_file_id) from @TVP_dcm)>0
					begin
			
						select @rowcount = count(dcm_file_id),
							   @counter = 1
						from @TVP_dcm


						while(@counter <= @rowcount)
							begin
								select @dcm_file_id          = dcm_file_id,
									   @dcm_file_name        = dcm_file_name,
									   @dcm_file_srl_no      = dcm_file_srl_no,
									   @dcm_file             = dcm_file
								from @TVP_dcm
								where dcm_file_srl_no= @counter 

								if(@dcm_file_id = '00000000-0000-0000-0000-000000000000') set @dcm_file_id = newid() 
					
								insert into study_hdr_dcm_files(study_hdr_id,study_uid,dcm_file_id,dcm_file_name,dcm_file_srl_no,
																dcm_file,xfer,created_by,date_created)
														values (@id,@study_uid,@dcm_file_id,@dcm_file_name,@dcm_file_srl_no,
																@dcm_file,'N',@updated_by,getdate())

								if(@@rowcount=0)
									begin
										rollback transaction
										select @error_code='333',@return_status=0
										return 0
									end
					
								set @counter = @counter + 1
						end

						--update study_hdr
						--set img_count = isnull(img_count,0)  + (select count(dcm_file_id) from @TVP_dcm)
					end

				create table #tmpDCMDel
				(row_id int identity(1,1) not null,
				 dcm_file_id uniqueidentifier not null)

				insert into #tmpDCMDel(dcm_file_id)
				(select dcm_file_id 
				 from #tmpDCM
				 where dcm_file_id not in (select dcm_file_id from @TVP_dcm where dcm_file_id <> '00000000-0000-0000-0000-000000000000'
											union select dcm_file_id from #tmpDCM))

				select @rowcount = @@rowcount,
					   @counter  = 1

				while(@counter <= @rowcount)
					begin
						select @dcm_file_id          = dcm_file_id
						from #tmpDCMDel
						where row_id= @counter

						delete from study_hdr_dcm_files
						where dcm_file_id =@dcm_file_id

						if(@@rowcount=0)
							begin
								rollback transaction
								select @error_code='036',@return_status=0
								return 0
							end

						set @counter = @counter + 1
					end

				drop table #tmpDCMDel
				drop table #tmpDCM

				set @image_count=0
				set @obj_count=0

				delete from study_hdr_merged_studies where study_hdr_id=@id 
				select @rowcount = count(study_hdr_id),
						@counter = 1
				from @TVP_merged

				while(@counter <= @rowcount)
					begin
						
						select @study_hdr_id          = study_hdr_id,
								@suid                 = study_uid,
								@merge_status         = merge_compare_none,
								@study_img_count      = image_count
						from @TVP_merged
						where srl_no= @counter

						if((@merge_status ='M') or (@merge_status='C'))
							begin
								insert into study_hdr_merged_studies(study_hdr_id,study_id,study_uid,image_count,merge_compare_none,date_updated,updated_by)
										                values(@id,@study_hdr_id,@suid,@study_img_count,@merge_status,getdate(),@updated_by)
						
								if(@@rowcount=0)
									begin
										rollback transaction
										select @error_code='436',@return_status=0,@user_name=@suid
										return 0
									end
								select @image_count = @image_count + isnull(img_count,0),
								       @obj_count   = @obj_count + isnull(object_count,0)
								from study_hdr
								where id = @study_hdr_id
								 
								--Merge/Compare Study Types
								if(select count(study_type_id) from study_hdr_study_types 
									where study_hdr_id = @study_hdr_id
									and study_type_id not in (select study_type_id from study_hdr_study_types where study_hdr_id=@id))>0
										begin
											insert into study_hdr_study_types(study_hdr_id,study_type_id,srl_no,write_back_tag,updated_by,date_updated)
											(select @id,study_type_id,srl_no,write_back_tag,@updated_by,getdate()
												from study_hdr_study_types
												where study_hdr_id = @study_hdr_id
												and study_type_id not in (select study_type_id from study_hdr_study_types where study_hdr_id=@id))

											if(@@rowcount=0)
												begin
													rollback transaction
													select @error_code='166',@return_status=0
													return 0
												end
										end
			
								delete from study_hdr_study_types where study_hdr_id = @study_hdr_id

								--Merge/Compare Documents
								if(select count(document_id) from study_hdr_documents
									where study_hdr_id = @study_hdr_id
									and document_name not in (select document_name from study_hdr_documents where study_hdr_id=@id))>0
										begin
											insert into study_hdr_documents(study_hdr_id,document_id,document_name,document_srl_no,document_link,document_file_type,document_file,
																			created_by,date_created)
											(select @id,newid(),document_name,document_srl_no,document_link,document_file_type,document_file,
													@updated_by,getdate()
												from study_hdr_documents
												where study_hdr_id = @study_hdr_id
												and document_name not in (select document_name from study_hdr_documents where study_hdr_id=@id))

												if(@@rowcount=0)
												begin
													rollback transaction
													select @error_code='167',@return_status=0
													return 0
												end
										end

								delete from study_hdr_dcm_files where study_hdr_id = @study_hdr_id

								--Merge/Compare study dcm files
								if(select count(dcm_file_id)
									from study_hdr_dcm_files
									where study_hdr_id = @study_hdr_id
									and dcm_file_name not in (select dcm_file_name from study_hdr_dcm_files where study_hdr_id=@id))>0
										begin
											insert into study_hdr_dcm_files(study_hdr_id,study_uid,dcm_file_id,dcm_file_name,dcm_file_srl_no,dcm_file,
																			created_by,date_created)
											(select @id,@study_uid,newid(),dcm_file_name,dcm_file_srl_no,dcm_file,
													@updated_by,getdate()
												from study_hdr_dcm_files
												where study_hdr_id = @study_hdr_id
												and dcm_file_name not in (select dcm_file_name from study_hdr_dcm_files where study_hdr_id=@id))

											if(@@rowcount=0)
												begin
													rollback transaction
													select @error_code='343',@return_status=0
													return 0
												end

											update study_hdr
											set img_count = img_count + (select count(dcm_file_name) 
																			from study_hdr_dcm_files
																			where study_hdr_id = @study_hdr_id
																			and dcm_file_name not in (select dcm_file_name 
																									from study_hdr_dcm_files 
																									where study_hdr_id=@id) )
										end

								delete from study_hdr_dcm_files where study_hdr_id = @study_hdr_id

								--Merge/Compare downloaded dcm files
								if(select count(id) from scheduler_file_downloads_dtls where id=@study_hdr_id)>0
									begin
										insert into scheduler_file_downloads_dtls(id,study_uid,file_name,import_session_id) 
																			(select @id,@study_uid,file_name,import_session_id
																			from scheduler_file_downloads_dtls
																			where id = @study_hdr_id)

										select @file_count = @@rowcount

										if(@file_count=0)
											begin
												rollback transaction
												select @error_code='244',@return_status=0
												return 0
											end

										delete from scheduler_file_downloads_dtls where id = @study_hdr_id

										if(@@rowcount=0)
											begin
												rollback transaction
												select @error_code='244',@return_status=0
												return 0
											end

										delete from scheduler_file_downloads where id = @study_hdr_id

										if(@@rowcount=0)
											begin
												rollback transaction
												select @error_code='244',@return_status=0
												return 0
											end

										update scheduler_file_downloads
										set file_count = isnull(file_count,0) + @file_count
										where id = @id

										if(@@rowcount=0)
											begin
												rollback transaction
												select @error_code='244',@return_status=0
												return 0
											end
									end

								--Merge/Compare study
								if(@merge_status = 'M')
									begin
										set @merge_status_desc ='MERGED with Study UID : ' + @study_uid
									end
								else if(@merge_status = 'C')
									begin
										set @merge_status_desc ='COMPARE with Study UID : ' + @study_uid
									end

								exec common_study_user_activity_trail_save
										@study_hdr_id  = @study_hdr_id,
										@study_uid     ='',
										@menu_id       = @menu_id,
										@activity_text = @merge_status_desc,
										@session_id    = @session_id,
										@activity_by   = @updated_by,
										@error_code    = @error_code output,
										@return_status = @return_status output

								if(@return_status=0)
									begin
										rollback transaction
										return 0
									end

								update study_hdr
								set     patient_id                = @patient_id,
										patient_name              = @patient_lname + ' ' + @patient_fname,
										patient_fname             = @patient_fname,
										patient_lname             = @patient_lname,
										patient_weight            = @patient_weight,
										patient_dob_accepted      = @patient_dob_accepted,
										patient_age_accepted      = @patient_age_accepted,
										patient_sex               = @patient_sex,
										patient_sex_neutered      = @patient_sex_neutered,
										species_id                = @species_id,
										breed_id                  = @breed_id,
										owner_first_name          = @owner_first_name,
										owner_last_name           = @owner_last_name,
										accession_no              = @accession_no,
										priority_id               = @priority_id,
										modality_id               = @modality_id,
										category_id               = @category_id,
										reason_accepted           = @reason_accepted,
										img_count_accepted        = @img_count_accepted,
										institution_id            = @institution_id,
										physician_id              = @physician_id,
										salesperson_id            = isnull(@salesperson_id,'00000000-0000-0000-0000-000000000000'),
										wt_uom                    = @wt_uom,
										merge_status              = @merge_status,
										merge_status_desc         = @merge_status_desc,
										physician_note            = @physician_note,
										consult_applied           = @consult_applied,
										service_codes             = @service_codes,
										finishing_datetime        = @finishing_datetime,
										updated_by                = @updated_by,
										date_updated              = getdate()
								where id = @study_hdr_id

								if(@@rowcount=0)
									begin
										rollback transaction
										select @error_code='164',@return_status=0
										return 0
									end

								if(@wt_uom='lbs')
									begin
										update study_hdr
										set patient_weight_kgs = @patient_weight * 0.454
										where id = @study_hdr_id 

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
										set patient_weight_kgs = @patient_weight,
											patient_weight     = @patient_weight/0.454
										where id = @study_hdr_id 

										if(@@rowcount = 0)
											begin
												rollback transaction
												select @return_status = 0,@error_code ='035'
												return 0
											end
									end

								if(@received_via_dicom_router='Y' and (@merge_status = 'M' or @merge_status = 'C'))
									begin
										if(@pacs_wb='Y')
											begin
												--update study_hdr
												--set img_count = img_count + isnull((select sum(img_count) from study_hdr where id in (select study_id from study_hdr_merged_studies where study_hdr_id = @id)),0)
												--where id = @id

												update study_hdr
												set img_count = img_count + @image_count
												where id = @id

												insert into study_hdr_deleted(id,study_uid,study_date,received_date,synched_on,
																				patient_name,institution_name,remarks,
																				deleted_on,deleted_by)
																		(select sh.id,sh.study_uid,sh.study_date,sh.received_date,sh.synched_on,
																				sh.patient_name,isnull(i.name,''), 'Merged with study uid ' + @study_uid,
																				getdate(),@updated_by
																		 from study_hdr sh
																		 left outer join institutions i on i.id= sh.institution_id
																		 where sh.id = @study_hdr_id)

												if(@@rowcount=0)
													begin
														rollback transaction
														select @error_code='344',@return_status=0
														return 0
													end

												delete from study_hdr where id = @study_hdr_id

												if(@@rowcount=0)
													begin
														rollback transaction
														select @error_code='344',@return_status=0
														return 0
													end

												delete from study_synch_dump where study_uid = @suid

												if(@@rowcount=0)
													begin
														rollback transaction
														select @error_code='344',@return_status=0
														return 0
													end
										    end

									end
								else if(@received_via_dicom_router='N' and (@merge_status = 'M' or @merge_status = 'C'))
									begin
										if(@pacs_wb='Y')
											begin

												update study_hdr
												set     accession_no               = @accession_no,
														pacs_wb                    ='Y',
														study_status               = 2,
														study_status_pacs          = 50,
														radiologist_id             = @def_asn_radiologist_id,
													    radiologist_pacs           = @def_asn_radiologist_name,
														date_updated               = getdate(),
														updated_by                 = @updated_by,
														status_last_updated_on     = getdate()
												from study_hdr
												where id = @study_hdr_id

												if(@@rowcount=0)
													begin
														rollback transaction
														select @error_code='045',@return_status=0
														return 0
													end

												insert into sys_case_study_status_log(study_id,study_uid,status_id_from,status_id_to,date_updated,updated_by)
																			(select id,study_uid,study_status_pacs,10,getdate(),@updated_by
																			 from study_hdr 
																			 where id = @study_hdr_id)

												if(@@rowcount=0)
													begin
														rollback transaction
														select @error_code='142',@return_status=0
														return 0
													end

												update study_hdr
												set radiologist_id   = @def_asn_radiologist_id,
													radiologist_pacs = @def_asn_radiologist_name
												where id = @id

												if(@@rowcount=0)
													begin
														rollback transaction
														select @error_code='432',@return_status=0,@user_name=@study_uid
														return 0
													end

												--update study_hdr
												--set radiologist_id   = @def_asn_radiologist_id,
												--	radiologist_pacs = @def_asn_radiologist_name
												--where id = @study_hdr_id

												--if(@@rowcount=0)
												--	begin
												--		rollback transaction
												--		select @error_code='433',@return_status=0,@user_name=@study_uid
												--		return 0
												--	end
											end
									end
							end
						else if(@merge_status ='N')
							begin
								update study_hdr
								set     merge_status           = 'N',
										merge_status_desc      = 'User denied the MERGE/COMPARE',
										date_updated           = getdate(),
										updated_by             = @updated_by,
										status_last_updated_on = getdate()
								from study_hdr
								where id = @study_hdr_id

								if(@@rowcount=0)
									begin
										rollback transaction
										select @error_code='045',@return_status=0
										return 0
									end
							end
											  
						set @counter = @counter + 1
				  end
					

				if(@pacs_wb = 'Y')
					begin
						if(select rtrim(ltrim(isnull(code,'')))
						   from institutions
						   where id = @institution_id)=''
							begin
								rollback transaction
								select @return_status = 0,@error_code ='127'
								return 0
							end

						if(rtrim(ltrim(isnull(@accession_no,'')))='')
							begin
								set @accn_no_generated='Y'
								set @accession_no = right(@study_uid,15)
								set @accession_no =  REPLACE(@accession_no,'.','-')
							end

						--set @flg=0
						--while(@flg=0)

						if(select count(accession_no) from study_hdr where accession_no = @accession_no and study_uid <> @study_uid and isnull(merge_status,'N')='N')>0
							begin
								rollback transaction
								select @return_status = 0,@error_code ='126'
								return 0
							end

						update study_hdr
						set     accession_no               = @accession_no,
						        img_count                  = img_count +  (select count(dcm_file_id) from study_hdr_dcm_files where study_hdr_id=@id),
								pacs_wb                    ='Y',
								study_status               = 2,
								study_status_pacs          = 50,
								date_updated               = getdate(),
								updated_by                 = @updated_by,
								status_last_updated_on     = getdate()
						from study_hdr
						where id = @id 

						if(@@rowcount=0)
							begin
								rollback transaction
								select @error_code='045',@return_status=0
								return 0
							end


						--if(select count(study_hdr_id) from study_hdr_merged_studies where study_hdr_id = @id)>0
						--	begin
						--		update study_hdr
						--		set     accession_no               = @accession_no,
						--				pacs_wb                    ='Y',
						--				study_status               = 2,
						--				study_status_pacs          = 50,
						--				date_updated               = getdate(),
						--				updated_by                 = @updated_by,
						--				status_last_updated_on     = getdate()
						--		from study_hdr
						--		where id in (select study_id from study_hdr_merged_studies where study_hdr_id=@id) 

						--		if(@@rowcount=0)
						--			begin
						--				rollback transaction
						--				select @error_code='045',@return_status=0
						--				return 0
						--			end

								

								
						--	end
							
						select @study_uid                 = study_uid,
						       @patient_sex_pacs          = isnull(patient_sex_pacs,''),
							   @patient_name_pacs         = isnull(patient_name_pacs,''),
							   @received_via_dicom_router = received_via_dicom_router
						from study_hdr
						where id = @id

						insert into sys_case_study_status_log(study_id,study_uid,status_id_from,status_id_to,date_updated,updated_by)
												        values(@id,@study_uid,0,10,getdate(),@updated_by)

						if(@@rowcount=0)
							begin
								rollback transaction
								select @error_code='142',@return_status=0
								return 0
							end

						if(select count(id) 
						   from study_hdr 
						   where isnull(patient_name_pacs,'') = @patient_name_pacs
						   and isnull(patient_sex_pacs,'')= @patient_sex_pacs
						   and institution_id             = @institution_id
						   and study_status_pacs          = 0
						   and isnull(merge_status,'N')   ='N'
						   and study_uid <> @study_uid)>0
							begin
								--print '000'
								--print @studies_merged
								create table #tmpIDs
								(
									rec_id int identity(1,1),
									id uniqueidentifier,
									suid nvarchar(100),
									study_status_pacs int,
									study_status int,
									image_count int,
									object_count int
								)

								insert into #tmpIDs(id,suid,study_status_pacs,study_status,image_count,object_count)
								(select id,study_uid,study_status_pacs,study_status,img_count,object_count
								from study_hdr  
								where isnull(patient_name_pacs,'') = @patient_name_pacs
								and isnull(patient_sex_pacs,'') = @patient_sex_pacs
								and institution_id              = @institution_id
								and study_status_pacs           = 0
								and study_uid <> @study_uid)

								insert into study_hdr_merged_studies(study_hdr_id,study_id,study_uid,merge_compare_none,date_updated,updated_by)
								                              (select @id,id,suid,@studies_merged,getdate(),@updated_by from #tmpIDs)
								if(@@rowcount=0)
									begin
										rollback transaction
										select @error_code='437',@return_status=0,@user_name=@study_uid
										return 0
									end

								if((@studies_merged ='M') or (@studies_merged='C'))
									begin
										 select @rc=max(rec_id) from #tmpIDs
										--print '111'
										-- merge study types
										--if(select count(study_hdr_id) from study_hdr_study_types where study_hdr_id=@id) > 0
										--	begin
												set @ctr=1

												while(@ctr <= @rc)
													begin
														select @study_hdr_id = id
														from #tmpIDs
														where rec_id = @ctr

														if(select count(study_type_id) from study_hdr_study_types 
														   where study_hdr_id = @study_hdr_id
														   and study_type_id not in (select study_type_id from study_hdr_study_types where study_hdr_id=@id))>0
															begin
																insert into study_hdr_study_types(study_hdr_id,study_type_id,srl_no,write_back_tag,updated_by,date_updated)
																(select @id,study_type_id,srl_no,write_back_tag,@updated_by,getdate()
																 from study_hdr_study_types
																 where study_hdr_id = @study_hdr_id
																 and study_type_id not in (select study_type_id from study_hdr_study_types where study_hdr_id=@id))

																 if(@@rowcount=0)
																	begin
																		rollback transaction
																		select @error_code='166',@return_status=0
																		return 0
																	end
															end

														set @ctr = @ctr + 1
													end
											--end

										delete 
										from study_hdr_study_types
										where study_hdr_id in (select id from #tmpIDs)
										
										--merge study documents
										--if(select count(study_hdr_id) from study_hdr_documents where study_hdr_id=@id) > 0
										--	begin
												set @ctr=1

												while(@ctr <= @rc)
													begin
														select @study_hdr_id = id
														from #tmpIDs
														where rec_id = @ctr

														if(select count(document_id) from study_hdr_documents
														   where study_hdr_id = @study_hdr_id
														   and document_name not in (select document_name from study_hdr_documents where study_hdr_id=@id))>0
																begin
																	insert into study_hdr_documents(study_hdr_id,document_id,document_name,document_srl_no,document_link,document_file_type,document_file,
																									created_by,date_created)
																	(select @id,newid(),document_name,document_srl_no,document_link,document_file_type,document_file,
																			@updated_by,getdate()
																	 from study_hdr_documents
																	 where study_hdr_id = @study_hdr_id
																	 and document_name not in (select document_name from study_hdr_documents where study_hdr_id=@id))

																	 if(@@rowcount=0)
																		begin
																			rollback transaction
																			select @error_code='167',@return_status=0
																			return 0
																		end
																end

														set @ctr = @ctr + 1
													end
											--end

										delete 
										from study_hdr_documents
										where study_hdr_id in (select id from #tmpIDs)

										--merge study dcm files
										--if(select count(study_hdr_id) from study_hdr_dcm_files where study_hdr_id=@id) > 0
										--	begin
												set @ctr=1

												while(@ctr <= @rc)
													begin
														select @study_hdr_id = id
														from #tmpIDs
														where rec_id = @ctr

														if(select count(dcm_file_id)
														   from study_hdr_dcm_files
														   where study_hdr_id = @study_hdr_id
														   and dcm_file_name not in (select dcm_file_name from study_hdr_dcm_files where study_hdr_id=@id))>0
															 begin
																	insert into study_hdr_dcm_files(study_hdr_id,study_uid,dcm_file_id,dcm_file_name,dcm_file_srl_no,dcm_file,
																									created_by,date_created)
																	(select @id,@study_uid,newid(),dcm_file_name,dcm_file_srl_no,dcm_file,
																			@updated_by,getdate()
																	 from study_hdr_dcm_files
																	 where study_hdr_id = @study_hdr_id
																	 and dcm_file_name not in (select dcm_file_name from study_hdr_dcm_files where study_hdr_id=@id))

																	 if(@@rowcount=0)
																		begin
																			rollback transaction
																			select @error_code='343',@return_status=0
																			return 0
																		end

																	 update study_hdr
																	 set img_count = img_count + (select count(dcm_file_name) 
																									from study_hdr_dcm_files
																									where study_hdr_id = @study_hdr_id
																									and dcm_file_name not in (select dcm_file_name 
																															from study_hdr_dcm_files 
																															where study_hdr_id=@id) )
															end

														set @ctr = @ctr + 1
													end
											--end

										delete 
										from study_hdr_dcm_files
										where study_hdr_id in (select id from #tmpIDs)

										--if(select count(id) from scheduler_file_downloads where id = @id)>0
										--	begin
												set @ctr=1
												while(@ctr <= @rc)
													begin
														select @study_hdr_id = id
														from #tmpIDs
														where rec_id = @ctr

														if(select count(id) from scheduler_file_downloads_dtls where id=@study_hdr_id)>0
															begin
																insert into scheduler_file_downloads_dtls(id,study_uid,file_name,import_session_id) 
																						          (select @id,@study_uid,file_name,import_session_id
																						           from scheduler_file_downloads_dtls
																						           where id = @study_hdr_id)

																 select @file_count = @@rowcount

																 if(@file_count=0)
																	begin
																		rollback transaction
																		select @error_code='244',@return_status=0
																		return 0
																	end

																  delete from scheduler_file_downloads_dtls where id = @study_hdr_id

																  if(@@rowcount=0)
																	begin
																		rollback transaction
																		select @error_code='244',@return_status=0
																		return 0
																	end

																  delete from scheduler_file_downloads where id = @study_hdr_id

																  if(@@rowcount=0)
																	begin
																		rollback transaction
																		select @error_code='244',@return_status=0
																		return 0
																	end

																  update scheduler_file_downloads
																  set file_count = isnull(file_count,0) + @file_count
																  where id = @id

																  if(@@rowcount=0)
																	begin
																		rollback transaction
																		select @error_code='244',@return_status=0
																		return 0
																	end
															end

														set @ctr = @ctr + 1
													end
											--end
										
										--merge studies
										if(@studies_merged = 'M')
											begin
												set @merge_status_desc ='MERGED with Study UID : ' + @study_uid
											end
										else if(@studies_merged = 'C')
											begin
												set @merge_status_desc ='COMPARE with Study UID : ' + @study_uid
											end
										
										set @ctr=1
										while(@ctr <= @rc)
											begin
												select @study_hdr_id = id
												from #tmpIDs
												where rec_id = @ctr

												exec common_study_user_activity_trail_save
													@study_hdr_id = @study_hdr_id,
													@study_uid    ='',
													@menu_id       = @menu_id,
													@activity_text = @merge_status_desc,
													@session_id    = @session_id,
													@activity_by   = @updated_by,
													@error_code    = @error_code output,
													@return_status = @return_status output

											   if(@return_status=0)
												begin
													rollback transaction
													return 0
												end

											    set @ctr= @ctr + 1
											end

										update study_hdr
										set     patient_id                = @patient_id,
												patient_name              = @patient_fname + ' ' + @patient_lname,
												patient_fname             = @patient_fname,
												patient_lname             = @patient_lname,
												patient_weight            = @patient_weight,
												patient_dob_accepted      = @patient_dob_accepted,
												patient_age_accepted      = @patient_age_accepted,
												patient_sex               = @patient_sex,
												patient_sex_neutered      = @patient_sex_neutered,
												species_id                = @species_id,
												breed_id                  = @breed_id,
												owner_first_name          = @owner_first_name,
												owner_last_name           = @owner_last_name,
												accession_no              = @accession_no,
												priority_id               = @priority_id,
												modality_id               = @modality_id,
												category_id               = @category_id,
												reason_accepted           = @reason_accepted,
												img_count_accepted        = @img_count_accepted,
												institution_id            = @institution_id,
												physician_id              = @physician_id,
												salesperson_id            = isnull(@salesperson_id,'00000000-0000-0000-0000-000000000000'),
												wt_uom                    = @wt_uom,
												pacs_wb                   ='Y',
												study_status              = 2,
												study_status_pacs         = 50,
												merge_status              = @studies_merged,
												merge_status_desc         = @merge_status_desc,
												received_via_dicom_router ='N',
												physician_note            = @physician_note,
												consult_applied           = @consult_applied,
												service_codes             = @service_codes,
												finishing_datetime        = @finishing_datetime,
												updated_by                = @updated_by,
												date_updated              = getdate()
										where id in (select id from #tmpIDs)

										if(@@rowcount=0)
											begin
												rollback transaction
												select @error_code='164',@return_status=0
												return 0
											end

										insert into sys_case_study_status_log(study_id,study_uid,status_id_from,status_id_to,date_updated,updated_by)
																        (select id,suid,study_status_pacs,10,getdate(),@updated_by from #tmpIDs)

										if(@@rowcount=0)
											begin
												rollback transaction
												select @error_code='142',@return_status=0
												return 0
											end

										 if(@received_via_dicom_router='Y' and (@studies_merged = 'M' or @studies_merged = 'C'))
											begin
												update study_hdr
												set img_count = img_count + isnull((select sum(image_count) from #tmpIDs),0)
												where id = @id

												insert into study_hdr_deleted(id,study_uid,study_date,received_date,synched_on,
												                              patient_name,institution_name,remarks,
																			  deleted_on,deleted_by)
																		(select t.id,t.suid,sh.study_date,sh.received_date,sh.synched_on,
																		        sh.patient_name,isnull(i.name,''), 'Merged with study uid ' + @study_uid,
																				getdate(),@updated_by
																		 from #tmpIDs t
																		 inner join study_hdr sh on sh.id=t.id
																		 left outer join institutions i on i.id= sh.institution_id)

												if(@@rowcount=0)
													begin
														rollback transaction
														select @error_code='344',@return_status=0
														return 0
													end

												delete from study_hdr where id in (select id from #tmpIDs)

												if(@@rowcount=0)
													begin
														rollback transaction
														select @error_code='344',@return_status=0
														return 0
													end

												delete from study_synch_dump where study_uid in (select suid from #tmpIDs)

												if(@@rowcount=0)
													begin
														rollback transaction
														select @error_code='344',@return_status=0
														return 0
													end

											end
										 else if(@received_via_dicom_router='N' and (@studies_merged = 'M' or @studies_merged = 'C'))
											begin
												update study_hdr
												set radiologist_id   = @def_asn_radiologist_id,
													radiologist_pacs = @def_asn_radiologist_name
												where id = @id

												if(@@rowcount=0)
													begin
														rollback transaction
														select @error_code='432',@return_status=0,@user_name=@study_uid
														return 0
													end

												update study_hdr
												set radiologist_id   = @def_asn_radiologist_id,
													radiologist_pacs = @def_asn_radiologist_name
												where id in (select id from #tmpIDs)

												if(@@rowcount=0)
													begin
														rollback transaction
														select @error_code='433',@return_status=0,@user_name=@study_uid
														return 0
													end
											end
										

										if(select count(id) from scheduler_file_downloads where id = @id)>0
											begin
												update scheduler_file_downloads
												set approve_for_pacs ='Y',
												    approved_by      = @updated_by,
													date_approved    = getdate()
												where id = @id

												if(@@rowcount=0)
													begin
														rollback transaction
														select @error_code='242',@return_status=0
														return 0
													end
											end


									end
								else if(@studies_merged ='N')
									begin
										select @rc=max(rec_id) from #tmpIDs

										set @ctr=1

										while(@ctr <= @rc)
											begin
												select @study_hdr_id = id
												from #tmpIDs
												where rec_id = @ctr

												update study_hdr
												set    	merge_status           = @studies_merged,
														merge_status_desc      = 'User denied the MERGE/COMPARE',
														date_updated           = getdate(),
														updated_by             = @updated_by,
														status_last_updated_on = getdate()
												from study_hdr
												where id = @study_hdr_id

												if(@@rowcount=0)
													begin
														rollback transaction
														select @error_code='045',@return_status=0
														return 0
													end
												
												set @ctr = @ctr + 1 
											end

										update study_hdr
										set     accession_no           = @accession_no,
												pacs_wb                ='Y',
												study_status           = 2,
												study_status_pacs      = 50,
												date_updated           = getdate(),
												updated_by             = @updated_by,
												status_last_updated_on = getdate()
										from study_hdr
										where id = @id

										if(@@rowcount=0)
											begin
												rollback transaction
												select @error_code='045',@return_status=0
												return 0
											end

										if(select count(id) from scheduler_file_downloads where id = @id)>0
											begin
												update scheduler_file_downloads
												set approve_for_pacs ='Y',
												    approved_by      = @updated_by,
													date_approved    = getdate()
												where id = @id

												if(@@rowcount=0)
													begin
														rollback transaction
														select @error_code='242',@return_status=0
														return 0
													end
											end
									end
								else if(@studies_merged ='X')
									begin
										--print '222'
										rollback transaction
										select @error_code='165',@return_status=0
										return 0
									end

								drop table #tmpIDs
							end
						else
							begin
								if(select count(id) from scheduler_file_downloads where id = @id)>0
									begin
										update scheduler_file_downloads
										set approve_for_pacs ='Y',
											approved_by      = @updated_by,
											date_approved    = getdate()
										where id = @id

										if(@@rowcount=0)
											begin
												rollback transaction
												select @error_code='242',@return_status=0
												return 0
											end
									end
							end


					   exec common_study_user_activity_trail_save
							@study_hdr_id = @id,
							@study_uid    ='',
							@menu_id      = @menu_id,
							@activity_text = 'Submitted',
							@session_id    = @session_id,
							@activity_by   = @updated_by,
							@error_code    = @error_code output,
							@return_status = @return_status output

					   if(@return_status=0)
						begin
							rollback transaction
							return 0
						end

						select @obj_count = object_count from study_hdr where id=@id
						if(@object_count - @obj_count > 3)
							begin
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
							end
					end
				else
					begin
						exec common_study_user_activity_trail_save
							@study_hdr_id = @id,
							@study_uid    ='',
							@menu_id      = @menu_id,
							@activity_text = 'Saved',
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

				commit transaction
				select @error_code='022',@return_status=1

				
				/**********Generate STAT Study submit mail notification**********/
				
				if(@pacs_wb= 'Y' and (@priority_id=10 or @priority_id=30))
					begin
						exec notification_rule_0_create
							@id = @id,
							@error_msg = @error_msg output,
							@return_type = @return_status output
					end

				/**********Generate notification rule notifications**********/
				declare @email_count int,
				        @sms_count int

				set @email_count= 0
				set @sms_count  = 0
				exec scheduler_notification_rule_notification_create
					@email_count = @email_count output,
					@sms_count   = @sms_count output

				set nocount off
				return 1
		end
	else
		begin
			select @error_code='094',@return_status=0
			set nocount off
			return 0
		end
	
end

GO
