USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_new_data_synch_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_new_data_synch_save]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_new_data_synch_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_new_data_synch_save : save scheduler log
** Created By   : Pavel Guha
** Created On   : 12/04/2019
*******************************************************/
CREATE procedure [dbo].[scheduler_new_data_synch_save]
    @study_uid nvarchar(100),
	@study_date datetime=null,
	@received_date datetime=null,
	@accession_no nvarchar(20)=null,
	@reason nvarchar(500)=null,
	@institution_name nvarchar(100)=null,
	@manufacturer_name nvarchar(100)=null,
	@device_serial_no nvarchar(20)=null,
	@referring_physician nvarchar(200)=null,
	@patient_id nvarchar(20)=null,
	@patient_name nvarchar(100)=null,
	@patient_sex nvarchar(10)=null,
	@patient_dob datetime =null,
	@patient_age varchar(50)=null,
	@patient_weight decimal(12, 3)=null,
	@owner_name nvarchar(100)=null,
	@species nvarchar(30)=null,
	@breed nvarchar(50)=null,
	@modality nvarchar(50)=null,
	@body_part nvarchar(50)=null,
	@manufacturer_model_no nvarchar(100)=null,
	@sex_neutered nvarchar(30)=null,
	@img_count int =0,
	@study_desc nvarchar(500) =null,
	@modality_ae_title nvarchar(50) =null,
	@priority_id int=null,
	@object_count int=0,
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	set nocount on
	declare @id uniqueidentifier,
	        @species_id int,
			@breed_id uniqueidentifier,
			@modality_id int,
			@body_part_id int,
			@institution_id uniqueidentifier,
			@institution_code nvarchar(5),
			@cd int,
			@physician_id uniqueidentifier,
			@device_id uniqueidentifier,
			@patient_age_accepted nvarchar(50),
			@default_country_id int, 
			@patient_fname nvarchar(50),
			@patient_lname nvarchar(50),
			@owner_last_name nvarchar(100),
			@synch_flag int,
			@wt_uom nvarchar(5),
			@priority_id_pacs int,
			@finishing_datetime datetime,
			@finishing_time_hrs int,
			@synched_on datetime,
			@study_count int
			


	select @study_count = count(study_uid) from study_synch_dump where study_uid=@study_uid
	--select @study_count = @study_count + count(study_uid) from study_synch_dump where study_uid=@study_uid
	
	if(@study_count)=0
		begin
			set @synch_flag = 0

			if(select count(id) from scheduler_file_downloads where study_uid=@study_uid) >0
				begin
					select @id               = id,
					       @institution_code = institution_code
					from scheduler_file_downloads 
					where study_uid=@study_uid

					if(select req_action_created from scheduler_file_downloads where id = @id and study_uid=@study_uid)='N'
						begin
							set @synch_flag = 1
						end
					else
						begin
							set @synch_flag = 0
						end
				end	
			else
				begin
					set @synch_flag = 1
				end

			if(@synch_flag =1)
				begin
					begin transaction
					set @priority_id_pacs = isnull(@priority_id,0)
					insert into study_synch_dump(study_uid,study_date,received_date,accession_no,reason,
										institution_name,manufacturer_name,manufacturer_model_no,device_serial_no,modality_ae_title,referring_physician,
										patient_id,patient_name,patient_sex,patient_dob,patient_age,patient_weight,sex_neutered,
										owner_name,species,breed,modality,body_part,img_count,study_desc,priority_id,object_count,synched_on)
							     values(@study_uid,@study_date,@received_date,@accession_no,@reason,
										@institution_name,@manufacturer_name,@manufacturer_model_no,@device_serial_no,@modality_ae_title,@referring_physician,
										@patient_id,@patient_name,@patient_sex,@patient_dob,@patient_age,@patient_weight,@sex_neutered,
										@owner_name,@species,@breed,@modality,@body_part,@img_count,@study_desc,@priority_id_pacs,@object_count,getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to synch Study UID ' + @study_uid
							return 0
						end

					set @id = newid()
				    if(isnull(@study_date,'01jan1900')='01jan1900')
						begin
							set @study_date = getdate()
						end
				    if(isnull(@received_date,'01jan1900')='01jan1900')
						begin
							set @received_date = @study_date
						end
					
					set @patient_age_accepted = 0

					if(isnull(@patient_dob,'01jan1900') <>'01jan1900') 
						begin
							if(isnull(@patient_dob,'01jan1900') > isnull(@study_date,'01jan1900'))
								begin
									set @patient_age_accepted =  '0 year 0 month'
									set @patient_dob = @study_date -- null
								end
							else
								begin
									set @patient_age_accepted = dbo.CalculateAge(@patient_dob,@study_date)
								end
						end
					else
						begin
							set @patient_age_accepted = '0 year 0 month'
							set @patient_dob = @study_date --null
						end
					if(charindex(' ',@patient_name)>0)
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
					set @owner_last_name = @patient_lname

					--if(select count(priority_id) from sys_priority where priority_id=@priority_id_pacs)>0
					--	begin
					--		if(select is_active from sys_priority where priority_id=@priority_id_pacs) ='Y'
					--			begin
					--				set @priority_id=@priority_id_pacs
					--			end
					--		else
					--			begin
					--				set @priority_id=0
					--			end
					--	end
					--else
					--	begin
					--		set @priority_id=0
					--	end
					set @priority_id=0
					set @synched_on= getdate()
					select @finishing_time_hrs = finishing_time_hrs from sys_priority where priority_id = @priority_id_pacs
					set @finishing_time_hrs = isnull(@finishing_time_hrs,0)
					set @finishing_datetime = dateadd(HH,@finishing_time_hrs,@synched_on)

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
										 patient_weight_pacs,patient_weight,
										 owner_name_pacs,owner_first_name,owner_last_name,
										 species_pacs,breed_pacs,modality_pacs,body_part_pacs,study_status_pacs,
										 img_count_pacs,img_count,object_count,object_count_pacs,study_desc,
										 priority_id_pacs,priority_id,finishing_datetime,
										 synched_on,date_updated,status_last_updated_on)
								 values(@id,@study_uid,@study_date,@received_date,
										@accession_no,@accession_no,
										@reason,@reason,
										@institution_name,@manufacturer_name,@device_serial_no,@modality_ae_title,@referring_physician,
										@patient_id,@patient_id,
										@patient_name,@patient_name,@patient_fname,@patient_lname,
										@patient_sex,@patient_sex,
										@sex_neutered,@sex_neutered,
										@patient_dob,@patient_dob,
										@patient_age,@patient_age_accepted,
										@patient_weight,@patient_weight,
										@owner_name,@owner_name,@owner_last_name,
										@species,@breed,@modality,@body_part,0,
										@img_count,@img_count,0,@object_count,@study_desc,
										@priority_id_pacs,@priority_id,@finishing_datetime,
										@synched_on,getdate(),getdate())	

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to synch Study UID ' + @study_uid
							return 0
						end

					insert into sys_case_study_status_log(study_id,study_uid,status_id_from,status_id_to,date_updated)
			                                       values(@id,@study_uid,0,0,getdate())
					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to update status log of Study UID ' + @study_uid
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
										select @return_type=0,@error_msg='Failed to update species id for Study UID ' + @study_uid
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
									select @return_type=0,@error_msg='Failed to update breed id for Study UID ' + @study_uid
									return 0
								end
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
										select @return_type=0,@error_msg='Failed to update modality id for Study UID ' + @study_uid
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
										select @return_type=0,@error_msg='Failed to update body part id for Study UID ' + @study_uid
										return 0
									end
						end

					if(rtrim(ltrim(@institution_name))<>'')
						begin
							if(select count(id)
							   from institutions
							   where upper(name) = upper(@institution_name)
							   and is_active='Y') =0
								begin
									if(select count(inl.institution_id)
									   from institution_alt_name_link inl
									   inner join institutions i on i.id= inl.institution_id
									   where upper(inl.alternate_name) = upper(@institution_name)
									   and i.is_active='Y') =0
										begin
											set @institution_id = newid()
											select @default_country_id =  id from sys_country where is_default='Y'

											--select @cd = max(convert(int,code)) from institutions
											--set @cd = isnull(@cd,0) + 1
											--select @institution_code=replicate('0',5-len(convert(varchar,@cd)))+convert(varchar,@cd)
										
											insert into institutions(id,code,name,country_id,created_by,date_created) 
															 values (@institution_id,'',@institution_name,@default_country_id,'00000000-0000-0000-0000-000000000000',getdate())

											if(@@rowcount=0)
												begin
													rollback transaction
													select @return_type=0,@error_msg='Failed to create institution for Study UID ' + @study_uid
													return 0
												end
										end
									else
										begin
											select @institution_id=isnull((select institution_id 
											                               from institution_alt_name_link inl
																		   inner join institutions i on i.id= inl.institution_id
																		   where upper(inl.alternate_name) = upper(@institution_name)
																		   and i.is_active='Y'),'00000000-0000-0000-0000-000000000000')
										end
						        end
							  else
								begin
									select @institution_id=isnull((select id from institutions
									where upper(name) = upper(@institution_name)
									and is_active='Y'),'00000000-0000-0000-0000-000000000000')
								end

							if(@manufacturer_name<>'')
								begin
									if(select count(device_id) from  institution_device_link where upper(manufacturer)=upper(@manufacturer_name) and upper(modality)=upper(@modality) and upper(modality_ae_title)= upper(@modality_ae_title) and institution_id = @institution_id)=0
										begin
											set @device_id=newid()

											insert into institution_device_link(device_id,institution_id,manufacturer,modality,serial_no,modality_ae_title,created_by,date_created)
																		 values(@device_id,@institution_id,@manufacturer_name,@modality,@device_serial_no,@modality_ae_title,'00000000-0000-0000-0000-000000000000',getdate())

											if(@@rowcount=0)
												begin
													rollback transaction
													select @return_type=0,@error_msg='Failed to create manufucaturer details for Study UID ' + @study_uid
													return 0
												end
										end
									else
										begin
											select @wt_uom = isnull(weight_uom,'lbs') 
											from  institution_device_link 
											where upper(manufacturer)=upper(@manufacturer_name) 
											and upper(modality)=upper(@modality) 
											and upper(modality_ae_title)= upper(@modality_ae_title) 
											and institution_id = @institution_id

											update study_hdr
											set wt_uom = isnull(@wt_uom,'')
											where id =  @id

											if(@@rowcount=0)
												begin
													rollback transaction
													select @return_type=0,@error_msg='Failed to update weight uom for Study UID ' + @study_uid
													return 0
												end
											
										end
								end 
						end

					if(rtrim(ltrim(@referring_physician))<>'')
						begin
							set @referring_physician = replace(upper(@referring_physician),'  ',' ')
							if(select count(id) from physicians where upper(name) = upper(@referring_physician) and is_active = 'Y') >0
								begin
									select @physician_id=isnull((select top 1 id from physicians
																	where upper(name) = upper(@referring_physician)
																	and is_active = 'Y'),'00000000-0000-0000-0000-000000000000')
									update study_hdr set physician_id = @physician_id where id=@id

									if(@@rowcount=0)
										begin
											rollback transaction
											select @return_type=0,@error_msg='Failed to update physician id for Study UID ' + @study_uid
											return 0
										end

									if((@institution_name<>'') and (@referring_physician<>''))
										begin
											if(select count(institution_id) from institution_physician_link where institution_id=@institution_id and physician_id =@physician_id)=0
												begin
													insert into institution_physician_link(institution_id,physician_id,created_by,date_created)
																					values(@institution_id,@physician_id,'00000000-0000-0000-0000-000000000000',getdate())
												end
										end
								end
						end

					update study_hdr set institution_id = isnull(@institution_id,'00000000-0000-0000-0000-000000000000') where id=@id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to update institution id for Study UID ' + @study_uid
							return 0
						end

					exec common_study_user_activity_trail_save
						@study_hdr_id = @id,
						@study_uid    = @study_uid,
						@menu_id      = 0,
						@activity_text = 'Received From PACS',
						@activity_by   = '00000000-0000-0000-0000-000000000000',
						@error_code    = @error_msg output,
						@return_status = @return_type output

					if(@return_type=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to create activity log  of Study UID ' + @study_uid + '.'
							return 0
						end

					commit transaction
					 set @return_type=1
					 set @error_msg=''

				end
		end
	else
		begin
			 set @return_type=2
			 set @error_msg=''
		end

	set nocount off
	return 1

end


GO
