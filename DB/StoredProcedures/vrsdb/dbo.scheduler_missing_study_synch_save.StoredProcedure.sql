USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_missing_study_synch_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_missing_study_synch_save]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_missing_study_synch_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_missing_study_synch_save : save synched 
                  missing study
** Created By   : Pavel Guha
** Created On   : 23/07/2019
*******************************************************/
CREATE procedure [dbo].[scheduler_missing_study_synch_save]
    @study_uid nvarchar(100),
	@study_date datetime,
	@received_date datetime,
	@accession_no nvarchar(20),
	@reason nvarchar(2000),
	@institution_name nvarchar(100),
	@manufacturer_name nvarchar(100),
	@device_serial_no nvarchar(20),
	@referring_physician nvarchar(200),
	@patient_id nvarchar(20),
	@patient_name nvarchar(100),
	@patient_sex nvarchar(10),
	@patient_dob datetime,
	@patient_age varchar(50),
	@patient_weight_lbs decimal(12, 3),
	@owner_name nvarchar(100),
	@species nvarchar(30),
	@breed nvarchar(50),
	@modality nvarchar(50),
	@body_part nvarchar(50),
	@manufacturer_model_no nvarchar(100),
	@spayed_neutered nvarchar(30),
	@img_count int =0,
	@study_desc nvarchar(500),
	@modality_ae_title nvarchar(50),
	@priority_id int,
	@radiologist nvarchar(250)=null,
	@study_status_pacs int,
	@study_type_1 nvarchar(50)=null,
	@study_type_2 nvarchar(50)=null,
	@study_type_3 nvarchar(50)=null,
	@study_type_4 nvarchar(50)=null,
	@sales_person nvarchar(100)=null,
    @patient_weight_kgs decimal(12, 3),
	@object_count int=0,
	@physician_note nvarchar(2000),
	@service_codes nvarchar(250)='',
	@submit_on datetime = null,
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	set nocount on
	set datefirst 1

	declare @id uniqueidentifier,
	        @species_id int,
			@breed_id uniqueidentifier,
			@modality_id int,
			@body_part_id int,
			@institution_id uniqueidentifier,
			@institution_code nvarchar(5),
			@patient_country_id int,
			@patient_state_id int,
			@patient_city nvarchar(100),
			@cd int,
			@physician_id uniqueidentifier,
			@device_id uniqueidentifier,
			@patient_age_accepted nvarchar(50),
			@default_country_id int, 
			@patient_fname nvarchar(80),
			@patient_lname nvarchar(80),
			@owner_first_name nvarchar(50),
			@owner_last_name nvarchar(50),
			@vrs_status_id int,
			@srl int,
			@study_type_id uniqueidentifier,
			@wb_tag nvarchar(4),
			@salesperson_id uniqueidentifier,
			@radiologist_id uniqueidentifier,
			@updated_by uniqueidentifier,
			@finishing_datetime datetime,
			@finishing_time_hrs int,
			@synched_on datetime,
			@consult_applicable nchar(1),
			@category_id int,
			@def_asn_radiologist_id uniqueidentifier,
			@def_asn_radiologist_name nvarchar(200),
			@activity_text nvarchar(max),
			@SUPPSYMRGMAILID nvarchar(200),
			@MAILSVRUSRCODE nvarchar(200),
			@MAILSVRUSRPWD nvarchar(200),
			@rc int,
			@ctr int,
			@study_hdr_id uniqueidentifier,
			@suid nvarchar(100),
			@mdl nvarchar(30),
			@pname nvarchar(200),
			@inst_name nvarchar(100),
			@email_subject nvarchar(250),
			@email_text nvarchar(max),
			@is_stat nchar(1),
			@final_rpt_release_datetime datetime,
			@final_rpt_release_hrs int,
			@FNLRPTAUTORELHR int,
			@sender_time_offset_mins int,
			@next_operation_time nvarchar(130),
			@delv_time nvarchar(130),
			@display_message nvarchar(250)

	declare	@beyond_operation_time nchar(1),
			@in_exp_list nchar(1)

	set @updated_by ='00000000-0000-0000-0000-000000000000'

	
	if(select count(study_uid) from study_synch_dump where study_uid=@study_uid)=0
		begin
			begin transaction
			set @referring_physician = replace(@referring_physician,'  ','')
			set @radiologist = replace(@radiologist,'  ','')

			insert into study_synch_dump(study_uid,study_date,received_date,accession_no,reason,
										institution_name,manufacturer_name,manufacturer_model_no,device_serial_no,modality_ae_title,referring_physician,
										patient_id,patient_name,patient_sex,patient_dob,patient_age,patient_weight,sex_neutered,
										owner_name,species,breed,modality,body_part,img_count,study_desc,priority_id,object_count,synched_on)
							     values(@study_uid,@study_date,@received_date,@accession_no,@reason,
										@institution_name,@manufacturer_name,@manufacturer_model_no,@device_serial_no,@modality_ae_title,@referring_physician,
										@patient_id,@patient_name,@patient_sex,@patient_dob,@patient_age,@patient_weight_lbs,@spayed_neutered,
										@owner_name,@species,@breed,@modality,@body_part,@img_count,@study_desc,@priority_id,@object_count,getdate())

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to synch Study UID ' + @study_uid
					return 0
				end

			if(select count(id) from scheduler_file_downloads where study_uid=@study_uid)=0
				begin
					select @id = newid()
				end
			else
				begin
					select @id = id from scheduler_file_downloads where study_uid=@study_uid
				end

		   set @patient_age_accepted = 0
		   if(isnull(@patient_dob,'01jan1900') <>'01jan1900') 
				begin
					set @patient_age_accepted = dbo.CalculateAge(@patient_dob,@study_date)
					--set @patient_age_accepted = datediff(Y,@patient_dob,getdate())
				end

		   if(CHARINDEX(' ',@patient_name)>0)
				begin
					set @patient_fname = RIGHT(@patient_name, CHARINDEX(' ', REVERSE(@patient_name))-1)
					set @patient_lname = LEFT(@patient_name, CHARINDEX(' ', @patient_name)-1)
					set @patient_name = rtrim(ltrim(@patient_fname + ' ' + @patient_lname))
				end
		    else
				begin
					set @patient_fname = @patient_name
					set @patient_lname = ''
				end

		   select @vrs_status_id = vrs_status_id
		   from sys_study_status_pacs
		   where status_id = @study_status_pacs

		   if(CHARINDEX(' ',@owner_name)>0)
				begin
					set @owner_name = RIGHT(@owner_name, CHARINDEX(' ', REVERSE(@owner_name))-1)
					set @owner_last_name = LEFT(@owner_name, CHARINDEX(' ', @owner_name)-1)
					set @owner_first_name = rtrim(ltrim(@owner_name + ' ' + @owner_name))
				end
			else
				begin
					set @owner_first_name = @owner_name
					set @owner_last_name = ''
				end
		   --set @owner_last_name = @patient_lname
		   set @radiologist_id = '00000000-0000-0000-0000-000000000000'

		   if(rtrim(ltrim(isnull(@radiologist,''))) <> '')
				begin
					select @radiologist_id = id 
					from radiologists
					where rtrim(ltrim(upper(isnull(lname,'')))) + ' ' + rtrim(ltrim(upper(isnull(fname,'')))) + ' ' + rtrim(ltrim(upper(isnull(credentials,'')))) = upper(@radiologist)


					set @radiologist_id= isnull(@radiologist_id,'00000000-0000-0000-0000-000000000000')
				end


		  -- select @synched_on = synched_on from study_synch_dump where study_uid=@study_uid
		  -- if(year(@synched_on) <=1900)
				--begin
				--	while(year(@synched_on) <=1900)
				--		begin
				--			set @synched_on= getdate()
				--		end
				--end
		   if(year(isnull(@submit_on,'01Jan1900'))<=1900) select @submit_on = getdate()
		   select @finishing_time_hrs = finishing_time_hrs from sys_priority where priority_id = @priority_id
		   set @finishing_time_hrs = isnull(@finishing_time_hrs,0)
		   select @FNLRPTAUTORELHR = data_type_string from general_settings where control_code='FNLRPTAUTORELHR'
		   select @is_stat = is_stat from sys_priority where priority_id=@priority_id
		   select @sender_time_offset_mins = id from sys_us_time_zones where is_default='Y'
		   
		   insert into study_hdr(id,study_uid,study_date,received_date,
								 accession_no_pacs,accession_no,
								 reason_pacs,reason_accepted,
								 institution_name_pacs,manufacturer_name,device_serial_no,modality_ae_title,referring_physician_pacs,
								 patient_id_pacs,patient_id,
								 patient_name_pacs,patient_name,patient_fname,patient_lname,
								 patient_sex_pacs,patient_sex,
								 patient_sex_neutered_pacs,patient_sex_neutered,
								 patient_dob_pacs,patient_dob_accepted,
								 patient_age_pacs,patient_age_accepted,
								 patient_weight_pacs,patient_weight,patient_weight_kgs,wt_uom,
								 owner_name_pacs,owner_first_name,owner_last_name,
								 species_pacs,breed_pacs,modality_pacs,body_part_pacs,study_status_pacs,study_status,
								 img_count_pacs,img_count,img_count_accepted,study_desc,radiologist_pacs,radiologist_id,physician_note,
								 priority_id_pacs,priority_id,object_count,object_count_pacs,synched_on,
								 service_codes,updated_by,date_updated,status_last_updated_on)
						 values(@id,@study_uid,@study_date,@received_date,
								@accession_no,@accession_no,
								@reason,@reason,
								@institution_name,@manufacturer_name,@device_serial_no,@modality_ae_title,@referring_physician,
								@patient_id,@patient_id,
								@patient_name,@patient_name,@patient_fname,@patient_lname,
								@patient_sex,@patient_sex,
								@spayed_neutered,@spayed_neutered,
								@patient_dob,@patient_dob,
								@patient_age,@patient_age_accepted,
								@patient_weight_lbs,@patient_weight_lbs,@patient_weight_kgs,'lbs',
								@owner_name,@owner_first_name,@owner_last_name,
								@species,@breed,@modality,@body_part,@study_status_pacs,@vrs_status_id,
								@img_count,@img_count,'Y',@study_desc,@radiologist,@radiologist_id,@physician_note,
								@priority_id,@priority_id,0,@object_count,getdate(),
								upper(isnull(@service_codes,'')),@updated_by,getdate(),getdate())	

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to synch Study UID ' + @study_uid
					return 0
				end

			if(rtrim(ltrim(@modality)) <> '')
				begin
					set @modality_id= isnull((select id from
											(select top 1 id,name
											from modality
											where is_active='Y'
											and (upper(name)=upper(@modality)
												   or upper(name) like '%' + upper(@modality) + '%'
												   or code = upper(@modality)
												   or code like '%' + @modality + '%'
												   or upper(dicom_tag) = upper(@modality)
												   or upper(dicom_tag) like '%' + upper(@modality) + '%') order by name)t),0)

					 update study_hdr set modality_id = @modality_id where id=@id

					 if(@@rowcount=0)
							begin
								rollback transaction
								select @return_type=0,@error_msg='Failed to update the modality of Study UID :' + @study_uid
								return 0
							end
				end

			set @srl=0
			if(rtrim(ltrim(isnull(@study_type_1,'')))<>'')
				begin
					set @study_type_id= isnull((select id 
					                           from modality_study_types 
											   where upper(name)=upper(rtrim(ltrim(@study_type_1)))
											   and modality_id = isnull(@modality_id,0)
											   and is_active='Y'),'00000000-0000-0000-0000-000000000000')

					if(@study_type_id<>'00000000-0000-0000-0000-000000000000')
						begin
							 set @srl = @srl + 1

							 if(@srl=1) set @wb_tag='DSCR'
							 else if(@srl=2) set @wb_tag='UDF4'
							 else if(@srl=3) set @wb_tag='UDF7'
							 else if(@srl=4) set @wb_tag='UDF9'

							 if(select count(study_hdr_id) from study_hdr_study_types where study_hdr_id = @id and study_type_id=@study_type_id)=0
								begin
									 insert into study_hdr_study_types(study_hdr_id,study_type_id,srl_no,write_back_tag,updated_by,date_updated)
																values(@id,@study_type_id,@srl,@wb_tag,@updated_by,getdate())

									 if(@@rowcount=0)
										begin
											rollback transaction
											select @return_type=0,@error_msg='Failed to synch the study type ' + convert(varchar,@srl) +  ' of Study UID :' + @study_uid
											return 0
										end
								end
						end
				end
			if(rtrim(ltrim(isnull(@study_type_2,'')))<>'')
				begin
					set @study_type_id= isnull((select id 
					                           from modality_study_types 
											   where upper(name)=upper(rtrim(ltrim(@study_type_2)))
											   and modality_id = isnull(@modality_id,0)
											   and is_active='Y'),'00000000-0000-0000-0000-000000000000')

					if(@study_type_id<>'00000000-0000-0000-0000-000000000000')
						begin
							 set @srl = @srl + 1

							 if(@srl=1) set @wb_tag='DSCR'
							 else if(@srl=2) set @wb_tag='UDF4'
							 else if(@srl=3) set @wb_tag='UDF7'
							 else if(@srl=4) set @wb_tag='UDF9'

							 if(select count(study_hdr_id) from study_hdr_study_types where study_hdr_id = @id and study_type_id=@study_type_id)=0
								begin
									 insert into study_hdr_study_types(study_hdr_id,study_type_id,srl_no,write_back_tag,updated_by,date_updated)
																values(@id,@study_type_id,@srl,@wb_tag,@updated_by,getdate())

									 if(@@rowcount=0)
										begin
											rollback transaction
											select @return_type=0,@error_msg='Failed to synch the study type ' + convert(varchar,@srl) +  ' of Study UID :' + @study_uid
											return 0
										end
								end
						end
				end
			if(rtrim(ltrim(isnull(@study_type_3,'')))<>'')
				begin
					set @study_type_id= isnull((select id 
					                           from modality_study_types 
											   where upper(name)=upper(rtrim(ltrim(@study_type_3)))
											   and modality_id = isnull(@modality_id,0)
											   and is_active='Y'),'00000000-0000-0000-0000-000000000000')

					if(@study_type_id<>'00000000-0000-0000-0000-000000000000')
						begin
							 set @srl = @srl + 1

							 if(@srl=1) set @wb_tag='DSCR'
							 else if(@srl=2) set @wb_tag='UDF4'
							 else if(@srl=3) set @wb_tag='UDF7'
							 else if(@srl=4) set @wb_tag='UDF9'

							 if(select count(study_hdr_id) from study_hdr_study_types where study_hdr_id = @id and study_type_id=@study_type_id)=0
								begin
									 insert into study_hdr_study_types(study_hdr_id,study_type_id,srl_no,write_back_tag,updated_by,date_updated)
																values(@id,@study_type_id,@srl,@wb_tag,@updated_by,getdate())

									 if(@@rowcount=0)
										begin
											rollback transaction
											select @return_type=0,@error_msg='Failed to synch the study type ' + convert(varchar,@srl) +  ' of Study UID :' + @study_uid
											return 0
										end
								end
						end
				end
			if(rtrim(ltrim(isnull(@study_type_4,'')))<>'')
				begin
					set @study_type_id= isnull((select id 
					                           from modality_study_types 
											   where upper(name)=upper(rtrim(ltrim(@study_type_4)))
											   and modality_id = isnull(@modality_id,0)
											   and is_active='Y'),'00000000-0000-0000-0000-000000000000')

					if(@study_type_id<>'00000000-0000-0000-0000-000000000000')
						begin
							 set @srl = @srl + 1

							 if(@srl=1) set @wb_tag='DSCR'
							 else if(@srl=2) set @wb_tag='UDF4'
							 else if(@srl=3) set @wb_tag='UDF7'
							 else if(@srl=4) set @wb_tag='UDF9'

							 if(select count(study_hdr_id) from study_hdr_study_types where study_hdr_id = @id and study_type_id=@study_type_id)=0
								begin
									 insert into study_hdr_study_types(study_hdr_id,study_type_id,srl_no,write_back_tag,updated_by,date_updated)
																values(@id,@study_type_id,@srl,@wb_tag,@updated_by,getdate())

									 if(@@rowcount=0)
										begin
											rollback transaction
											select @return_type=0,@error_msg='Failed to synch the study type ' + convert(varchar,@srl) +  ' of Study UID :' + @study_uid
											return 0
										end
								end
						end
				end

			if(@study_status_pacs=50)
				begin
					insert into sys_case_study_status_log(study_id,study_uid,status_id_from,status_id_to,date_updated)
												   values(@id,@study_uid,@study_status_pacs,@study_status_pacs,@submit_on)
				end
			else
				begin
					insert into sys_case_study_status_log(study_id,study_uid,status_id_from,status_id_to,date_updated)
												   values(@id,@study_uid,@study_status_pacs,@study_status_pacs,getdate())
				end

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to update the status log of Study UID :' + @study_uid
					return 0
				end

			if(rtrim(ltrim(@species)) <>'')
				begin
					set @species_id= isnull((select id from
											(select top 1 id,name
											from species
											where is_active='Y'
											and (upper(name)=upper(@species)
												   or upper(name) like '%' + upper(@species) + '%'
												   or code = upper(@species)
												   or code like '%' + upper(@species) + '%')order by name)t),0)
												   
	                 update study_hdr set species_id = @species_id where id=@id

					 if(@@rowcount=0)
							begin
								rollback transaction
								select @return_type=0,@error_msg='Failed to update the species of Study UID :' + @study_uid
								return 0
							end

					 set @breed_id = isnull((select id from
												(select top 1 id,name
												from breed
												where species_id = @species_id
												and is_active='Y'
												and (upper(name)=upper(rtrim(ltrim(@breed)))
													   or upper(name) like '%' + upper(rtrim(ltrim(@breed))) + '%'
													   or code = upper(rtrim(ltrim(@breed)))
													   or code like '%' + rtrim(ltrim(@breed)) + '%')order by name)t),'00000000-0000-0000-0000-000000000000')

					update study_hdr set breed_id = @breed_id where id=@id

				    if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to update the breed of Study UID :' + @study_uid
							return 0
						end
					
				end
			
			if(rtrim(ltrim(@body_part)) <> '')
				begin
					set @body_part_id = isnull((select id from
											(select top 1 id,name
											from body_part
											where is_active='Y'
											and (upper(name)=upper(@body_part)
												   or upper(name) like '%' + upper(@body_part) + '%'
												   or code = upper(@body_part)
												   or code like '%' + upper(@body_part) + '%') order by name)t),0)

			
					 update study_hdr set body_part_id = @body_part_id where id=@id

					 if(@@rowcount=0)
							begin
								rollback transaction
								select @return_type=0,@error_msg='Failed to update the body part of Study UID :' + @study_uid
								return 0
							end
				end

			if(rtrim(ltrim(@institution_name))<>'')
				begin
					if(select count(id)
					   from institutions
					   where (upper(name) = upper(@institution_name) or upper(code)= @institution_name)
					   and is_active = 'Y') =0
						begin
							set @institution_id = newid()
							select @default_country_id =  id from sys_country where is_default='Y'

							select @cd = max(convert(int,code)) from institutions
							set @cd = isnull(@cd,0) + 1
							select @institution_code=replicate('0',5-len(convert(varchar,@cd)))+convert(varchar,@cd)

							insert into institutions(id,code,name,country_id,created_by,date_created) 
							                 values (@institution_id,@institution_code,@institution_name,@default_country_id,'00000000-0000-0000-0000-000000000000',getdate())

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to create the institution name of Study UID :' + @study_uid
									return 0
								end
						end
					else
						begin
							select @institution_id=isnull((select id from institutions
															where (upper(name) = upper(@institution_name) or upper(code)= @institution_name)
															and is_active = 'Y'),'00000000-0000-0000-0000-000000000000')

							select @patient_country_id = country_id,
							       @patient_state_id   = state_id,
								   @patient_city       = isnull(city,'')
							from institutions 
							where id=@institution_id

							update study_hdr 
							set institution_id = @institution_id ,
							    patient_country_id = isnull(@patient_country_id,0),
								patient_state_id = isnull(@patient_state_id,0),
								patient_city  = isnull(@patient_city,'')
							where id=@id

							if(@@rowcount=0)
									begin
										rollback transaction
										select @return_type=0,@error_msg='Failed to update the institution name of Study UID :' + @study_uid
										return 0
									end
						end

					if(@manufacturer_name<>'')
						begin
							if(select count(device_id) from  institution_device_link where upper(manufacturer)=upper(@manufacturer_name) and upper(modality)=upper(@modality) and upper(modality_ae_title)= upper(@modality_ae_title) and institution_id = @institution_id)=0
								begin
									set @device_id=newid()

									insert into institution_device_link(device_id,institution_id,manufacturer,modality,serial_no,modality_ae_title,created_by,date_created)
																 values(@device_id,@institution_id,@manufacturer_name,@modality,@device_serial_no,@modality_ae_title,@updated_by,getdate())

										if(@@rowcount=0)
										begin
											rollback transaction
											select @return_type=0,@error_msg='150'
											return 0
										end
								end
						end 
				end

			if(rtrim(ltrim(@referring_physician))<>'' and isnull(@institution_id,'00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
					begin
						if(select count(physician_id) from institution_physician_link ipl
						   inner join physicians p on p.id = ipl.physician_id
						    where (upper(ipl.physician_name) = upper(replace(@referring_physician,'  ',' '))
							or (upper(p.code)) = upper(replace(@referring_physician,'  ',' '))
							or ((upper(ipl.physician_lname) + ' ' + upper(ipl.physician_fname) + upper(ipl.physician_credentials)) = upper(@referring_physician))
							or ((upper(ipl.physician_lname) + ' ' + upper(ipl.physician_fname)+ ' ' +  upper(ipl.physician_credentials)) = upper(replace(@referring_physician,'  ',' '))))
							and ipl.institution_id = @institution_id)>0
							begin
								select @physician_id=isnull((select physician_id from institution_physician_link ipl
								                             inner join physicians p on p.id = ipl.physician_id
															 where (upper(ipl.physician_name) = upper(replace(@referring_physician,'  ',' ')) 
															 or (upper(p.code)) = upper(replace(@referring_physician,'  ',' '))
															 or ((upper(ipl.physician_lname) + ' ' + upper(ipl.physician_fname) + upper(ipl.physician_credentials)) = upper(@referring_physician))
							                                 or ((upper(ipl.physician_lname) + ' ' + upper(ipl.physician_fname)+ ' ' +  upper(ipl.physician_credentials)) = upper(replace(@referring_physician,'  ',' '))))
															 and ipl.institution_id = @institution_id),'00000000-0000-0000-0000-000000000000')


								update study_hdr set physician_id = @physician_id where id=@id

								if(@@rowcount=0)
									begin
										rollback transaction
										select @return_type=0,@error_msg='Failed to update the physician of Study UID :' + @study_uid
										return 0
									end

								if((@institution_name<>'') and (@referring_physician<>''))
									begin
										if(select count(institution_id) from institution_physician_link where institution_id=@institution_id and physician_id =@physician_id)=0
											begin
												insert into institution_physician_link(institution_id,physician_id,created_by,date_created)
													                            values(@institution_id,@physician_id,@updated_by,getdate())

												if(@@rowcount =0)
													begin
														rollback transaction
														select @return_type=0,@error_msg='Failed to link the institution and physician of Study UID :' + @study_uid
														return 0
													end
											end
									end
							end
					end
				--end
			if(rtrim(ltrim(isnull(@sales_person,'')))<>'')
					begin
						if(select count(id) from salespersons where upper(name) = upper(@sales_person) and is_active = 'Y') >0
							begin
								select @salesperson_id=isnull((select id from salespersons
																where upper(name) = upper(@sales_person)
																and is_active = 'Y'),'00000000-0000-0000-0000-000000000000')


								update study_hdr set salesperson_id = @salesperson_id where id=@id

								if(@@rowcount=0)
									begin
										rollback transaction
										select @return_type=0,@error_msg='Failed to update the sales person of Study UID :' + @study_uid
										return 0
									end

								if((@institution_name<>'') and (@salesperson_id<>'00000000-0000-0000-0000-000000000000'))
									begin
										if(select count(institution_id) from institution_salesperson_link where institution_id=@institution_id and salesperson_id =@salesperson_id)=0
											begin
												insert into institution_salesperson_link(institution_id,salesperson_id,created_by,date_created)
													                              values(@institution_id,@salesperson_id,@updated_by,getdate())

												if(@@rowcount =0)
													begin
														rollback transaction
														select @return_type=0,@error_msg='Failed to update the institution and sales person of Study UID :' + @study_uid
														return 0
													end
											end
									end
							end
						
					end
			
			select @consult_applicable= consult_applicable
			from institutions
			where id = isnull(@institution_id,'00000000-0000-0000-0000-000000000000')

			set @consult_applicable=isnull(@consult_applicable,'N')
			set @category_id=0

			if(@consult_applicable='Y')
				begin
					if(charindex('CONSULT',upper(isnull(@service_codes,''))) >0)
						begin
							set @category_id=3
						end
					else
						begin
							set @category_id=0
						end
				end

			if(@category_id=0)
				begin
					if(select count(shst.study_type_id)
					  from modality_study_types mst
					  inner join study_hdr_study_types shst on shst.study_type_id = mst.id
					  where shst.study_hdr_id = @id
					  and mst.category_id=2)>0
						begin
							set @category_id =2
						end
					else
						begin
							set @category_id =1
						end
				end

			update study_hdr
			set category_id = @category_id
			where id=@id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to set the category of Study UID :' + @study_uid
					return 0
				end

		    set @in_exp_list='N'
			set @beyond_operation_time ='N'
			set @error_msg =''
			set @return_type=0

			exec common_service_availability_check
				@species_id            = @species_id,
				@modality_id           = @modality_id,
				@institution_id        = @institution_id,
				@priority_id           = @priority_id,
				@beyond_operation_time = @beyond_operation_time output,
				@in_exp_list           = @in_exp_list output,
				@error_code            = @error_msg output,
				@return_status         = @return_type output

			if(@return_type=0)
				begin
				    rollback transaction
					select @return_type=0,@error_msg='Failure checking the service availability Study UID ' + @study_uid + '.'
					return 0
				end

		   

		   if(@is_stat='Y')
				begin
					exec common_check_operation_time
						@priority_id             = @priority_id,
						@sender_time_offset_mins = @sender_time_offset_mins,
						@submission_date         = @submit_on,
						@next_operation_time     = @next_operation_time output,
						@delv_time               = @delv_time output,
						@display_message         = @display_message output,
						@error_code              = @error_msg output,
						@return_status           = @return_type output

					if(@in_exp_list='Y')
						begin
							set @finishing_datetime = dateadd(HH,@finishing_time_hrs,getdate())
						end
					else
						begin
							set @finishing_datetime = convert(datetime,@delv_time)
						end

					set @final_rpt_release_datetime = dateadd(mi,(select final_report_release_time_mins from sys_priority where priority_id=@priority_id),getdate())
				end
		   else if(@is_stat='N')
				begin
					set @finishing_datetime = dateadd(HH,@finishing_time_hrs,@submit_on)
					select @final_rpt_release_datetime = dateadd(HH,@FNLRPTAUTORELHR,@submit_on)
				end

			update study_hdr
			set finishing_datetime = @finishing_datetime,
			    final_rpt_release_datetime = @final_rpt_release_datetime
			where id=@id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to set the finishing/report releasing datetime of Study UID :' + @study_uid
					return 0
				end

			exec common_study_user_activity_trail_save
				@study_hdr_id = @id,
				@study_uid    = @study_uid,
				@menu_id      = 0,
				@activity_text = 'Received From PACS (Missing Study)',
				@activity_by   = '00000000-0000-0000-0000-000000000000',
				@error_code    = @error_msg output,
				@return_status = @return_type output

			if(@return_type=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to create activity log  of Study UID ' + @study_uid + '.'
					return 0
				end

			if(@study_status_pacs=50)
				begin
					if(select count(id)		
						   from study_hdr 
						   where ((upper(isnull(patient_fname,'')) + ' ' + upper(isnull(patient_lname,'')) = upper(isnull(@patient_fname,'')) + ' ' + upper(isnull(@patient_lname,'')))
						          or (upper(isnull(patient_lname,'')) + ' ' + upper(isnull(patient_fname,'')) = upper(isnull(@patient_lname,'')) + ' ' + upper(isnull(@patient_fname,''))))
						   and isnull(patient_sex,'')  = @patient_sex
						   and institution_id          = @institution_id
						   and study_status_pacs       = 50
						   and study_uid <> @study_uid)>0
							begin
								select @def_asn_radiologist_id   = id,
								       @def_asn_radiologist_name = name
								from radiologists
								where assign_merged_study = 'Y'
								and is_active='Y'

								if(isnull(@def_asn_radiologist_id,'00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
									begin
										select @SUPPSYMRGMAILID = data_type_string from general_settings where control_code='SUPPSYMRGMAILID'
										select @MAILSVRUSRCODE = data_type_string from general_settings where control_code='MAILSVRUSRCODE'
										select @MAILSVRUSRPWD = data_type_string from general_settings where control_code='MAILSVRUSRPWD'

										create table #tmpStudy
										(
											rec_id int identity(1,1),
											study_id uniqueidentifier,
											study_uid nvarchar(100),
											patient_name nvarchar(200),
											modality nvarchar(30),
											institution nvarchar(100)
										)

										insert into #tmpStudy(study_id,study_uid,patient_name,modality,institution)
										(select sh.id,sh.study_uid,isnull(sh.patient_name,''),isnull(m.name,''),isnull(i.name,'')
										 from study_hdr sh
										 left outer join modality m on m.id = sh.modality_id
										 left outer join institutions i on i.id = sh.institution_id
										 where ((upper(isnull(sh.patient_fname,'')) + ' ' + upper(isnull(sh.patient_lname,'')) = upper(isnull(@patient_fname,'')) + ' ' + upper(isnull(@patient_lname,'')))
												or (upper(isnull(sh.patient_lname,'')) + ' ' + upper(isnull(sh.patient_fname,'')) = upper(isnull(@patient_lname,'')) + ' ' + upper(isnull(@patient_fname,''))))
										and isnull(sh.patient_sex,'')  = @patient_sex
										and sh.institution_id          = @institution_id
										and sh.study_status_pacs       = 50
										and sh.study_uid <> @study_uid)

										select @rc=@@rowcount,@ctr=1

										while(@ctr<=@rc)
											begin
												select @study_hdr_id = study_id,
												       @suid         = study_uid,
													   @mdl          = modality,
													   @inst_name    = institution,
													   @pname        = patient_name
											    from #tmpStudy 
												where rec_id = @ctr

												update study_hdr
												set radiologist_id   = @def_asn_radiologist_id,
													radiologist_pacs = @def_asn_radiologist_name
												where id = @study_hdr_id

												if(@@rowcount=0)
													begin
														rollback transaction
														select @return_type=0,@error_msg='Failed to assign default radiologist for merged/comparison of Study UID :' + @suid
														return 0
													end

												set @activity_text = 'Default radiologist ' + @def_asn_radiologist_name + ' assigned'
												exec common_study_user_activity_trail_save
													@study_hdr_id  = @study_hdr_id,
													@study_uid     = '',
													@menu_id       = 0,
													@activity_text = @activity_text,
													@activity_by   = @updated_by,
													@error_code    = @error_msg output,
													@return_status = @return_type output

												if(@return_type=0)
													begin
														rollback transaction
														select @return_type=0,@error_msg='Failed to create activity log for default radiologist assignment of Study UID ' + @suid
														return 0
													end

												set @email_subject = 'Default radiologist assigned to study of patient ' +  @pname
												
												set @email_text    = 'Default radiologist assigned to study of patient ' + @pname + ' as it has been merged in PACS\n\n'
												set @email_text    = @email_text + 'Radiologist Assigned   : ' + @def_asn_radiologist_name + '\n'
												set @email_text    = @email_text + 'Study UID              : ' + @suid + '\n'
												set @email_text    = @email_text + 'Institution            : ' + @inst_name+'\n'
												set @email_text    = @email_text + 'Modality               : ' + @mdl + '\n'
												set @email_text    = @email_text + '\n\n'
												set @email_text    = @email_text +'This is an automated message from VETRIS.Please do not reply to the message.\n'

												insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,
												                      email_subject,email_text,
																	  study_hdr_id,study_uid,email_type,sender_email_address,sender_email_password)
																values(newid(),getdate(),@SUPPSYMRGMAILID,'RAD Support',
																       @email_subject,@email_text,
																       @study_hdr_id,@suid,'MGSYRDASN',@MAILSVRUSRCODE,@MAILSVRUSRPWD)

												if(@@rowcount=0)
													begin
														rollback transaction
														select @return_type=0,@error_msg='Failed to generate default radiologist assignment mail for merged/comparison of Study UID :' + @suid
														return 0
													end

												set @ctr = @ctr + 1
											end

										 drop table #tmpStudy
									end
							end
				end

					
			 commit transaction
			 set @return_type=1
			 set @error_msg='Study UID :' + @study_uid + ' synched successfully'

		end
	else
		begin
			
			--set @return_type=1
			-- set @error_msg='Study UID :' + @study_uid + ' synched successfully'
			 set @return_type=0
			 set @error_msg='Study UID :' + @study_uid + ' already exists'
		end

   

	set nocount off
	return 1

end


GO
