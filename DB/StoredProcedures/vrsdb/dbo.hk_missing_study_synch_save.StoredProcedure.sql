USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[hk_missing_study_synch_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[hk_missing_study_synch_save]
GO
/****** Object:  StoredProcedure [dbo].[hk_missing_study_synch_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : hk_missing_study_synch_save : save missing study
** Created By   : Pavel Guha
** Created On   : 18/07/2019
*******************************************************/
/*
exec hk_missing_study_synch_save '1.2.826.0.1.3680043.2.93.2.363610615.3890.1577194765.562646','24Dec2019 02:49:51','24Dec2019 08:39:27','0191224024951-2',
'Liver Enzymes elevated.',
'Veterinary Ultrasound Services','Sonoscape','','Glass Sharon  DVM','','LAWS CODY JUNIOR','F','01May2005','14 y',8,'Laws Randy','Canine','Pomeranian','US','',
'','YES',49,'','',20,'Waller Kenneth   DVM, MS, DACVR',100,'Abdomen - Complete','','','','Alex Shapiro','3.632',50,
'11111111-1111-1111-1111-111111111111',29,'',0
*/
CREATE procedure [dbo].[hk_missing_study_synch_save]
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
	@sex_neutered nvarchar(30),
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
	@object_count int = 0,
	@physician_note nvarchar(2000)=null,
	@prelim_radiologist_pacs nvarchar(250)=null,
	@final_radiologist_pacs nvarchar(250)=null,
	@updated_by uniqueidentifier,
	@menu_id int,
	@error_code nvarchar(1000) = '' output,
	@return_status int = 0 output
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
			@vrs_status_id int,
			@srl int,
			@study_type_id uniqueidentifier,
			@wb_tag nvarchar(4),
			@salesperson_id uniqueidentifier,
			@radiologist_id uniqueidentifier,
			@prelim_radiologist_id uniqueidentifier,
			@final_radiologist_id uniqueidentifier,
			@finishing_datetime datetime,
			@finishing_time_hrs int,
			@synched_on datetime

	declare @rpt_type nvarchar(20),
	        @gen_mail nchar(1),
			@recipient_address nvarchar(500),
			@recipient_name nvarchar(200),
			@VRSPACSLINKURL nvarchar(200),
			@study_types varchar(max),
			@email_subject varchar(250),
			@email_text nvarchar(max)
	
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
										@patient_id,@patient_name,@patient_sex,@patient_dob,@patient_age,@patient_weight_lbs,@sex_neutered,
										@owner_name,@species,@breed,@modality,@body_part,@img_count,@study_desc,@priority_id,@object_count,getdate())

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status=0,@error_code='140'
					return 0
				end

		   set @id = newid()

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
				end
			else
				begin
					set @patient_fname = @patient_name
					set @patient_lname = ''
				end

		  select @vrs_status_id = vrs_status_id
		  from sys_study_status_pacs
		  where status_id = @study_status_pacs

		   set @owner_last_name = @patient_lname
		   set @radiologist_id = '00000000-0000-0000-0000-000000000000'
		   set @prelim_radiologist_id = '00000000-0000-0000-0000-000000000000'
		   set @final_radiologist_id = '00000000-0000-0000-0000-000000000000'

		   if(rtrim(ltrim(isnull(@radiologist,''))) <> '')
				begin
					select @radiologist_id = id 
					from radiologists
					where rtrim(ltrim(upper(isnull(lname,'')))) + ' ' + rtrim(ltrim(upper(isnull(fname,'')))) + ' ' + rtrim(ltrim(upper(isnull(credentials,'')))) = upper(@radiologist)


					set @radiologist_id= isnull(@radiologist_id,'00000000-0000-0000-0000-000000000000')
				end

		   if(rtrim(ltrim(isnull(@prelim_radiologist_pacs,''))) <> '')
				begin
					set @prelim_radiologist_pacs = replace(rtrim(ltrim(isnull(@prelim_radiologist_pacs,''))),'  ','')
	
					select @prelim_radiologist_id = id 
					from radiologists
					where ( (rtrim(ltrim(upper(name))) = upper(isnull(@prelim_radiologist_pacs,''))) or
			        (rtrim(ltrim(upper(isnull(lname,'')))) + ' ' + rtrim(ltrim(upper(isnull(fname,'')))) + ' ' + rtrim(ltrim(upper(isnull(credentials,'')))) = upper(isnull(@prelim_radiologist_pacs,''))) )


					set @prelim_radiologist_id= isnull(@prelim_radiologist_id,'00000000-0000-0000-0000-000000000000')
				end

		   if(rtrim(ltrim(isnull(@final_radiologist_pacs,''))) <> '')
				begin
					set @final_radiologist_pacs = replace(rtrim(ltrim(isnull(@final_radiologist_pacs,''))),'  ','')
	
					select @final_radiologist_id = id 
					from radiologists
					where ( (rtrim(ltrim(upper(name))) = upper(isnull(@final_radiologist_pacs,''))) or
							(rtrim(ltrim(upper(isnull(lname,'')))) + ' ' + rtrim(ltrim(upper(isnull(fname,'')))) + ' ' + rtrim(ltrim(upper(isnull(credentials,'')))) = upper(isnull(@final_radiologist_pacs,''))) )


					set @final_radiologist_id= isnull(@final_radiologist_id,'00000000-0000-0000-0000-000000000000')
				end

			set @synched_on=getdate()
			select @finishing_time_hrs = finishing_time_hrs from sys_priority where priority_id = @priority_id
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
								 patient_weight_pacs,patient_weight,patient_weight_kgs,wt_uom,
								 owner_name_pacs,owner_first_name,owner_last_name,
								 species_pacs,breed_pacs,modality_pacs,body_part_pacs,study_status_pacs,study_status,
								 img_count_pacs,img_count,img_count_accepted,study_desc,radiologist_pacs,radiologist_id,physician_note,
								 prelim_radiologist_pacs,prelim_radiologist_id,final_radiologist_pacs,final_radiologist_id,
								 priority_id_pacs,priority_id,object_count,synched_on,finishing_datetime,
								 updated_by,date_updated,status_last_updated_on)
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
								@patient_weight_lbs,@patient_weight_lbs,@patient_weight_kgs,'lbs',
								@owner_name,@owner_name,@owner_last_name,
								@species,@breed,@modality,@body_part,@study_status_pacs,@vrs_status_id,
								@img_count,@img_count,'Y',@study_desc,@radiologist,@radiologist_id,@physician_note,
								@prelim_radiologist_pacs,@prelim_radiologist_id,@final_radiologist_pacs,@final_radiologist_id,
								@priority_id,@priority_id,@object_count,@synched_on,@finishing_datetime,
								@updated_by,getdate(),getdate())	

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status=0,@error_code='Failed to synch Study UID ' + @study_uid
					return 0
				end

			if(@modality <> '')
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
								select @return_status=0,@error_code='145'
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

							 insert into study_hdr_study_types(study_hdr_id,study_type_id,srl_no,write_back_tag,updated_by,date_updated)
														values(@id,@study_type_id,@srl,@wb_tag,@updated_by,getdate())

							 if(@@rowcount=0)
								begin
									rollback transaction
									select @return_status=0,@error_code='152'
									return 0
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

							 insert into study_hdr_study_types(study_hdr_id,study_type_id,srl_no,write_back_tag,updated_by,date_updated)
														values(@id,@study_type_id,@srl,@wb_tag,@updated_by,getdate())

							 if(@@rowcount=0)
								begin
									rollback transaction
									select @return_status=0,@error_code='153'
									return 0
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

							 insert into study_hdr_study_types(study_hdr_id,study_type_id,srl_no,write_back_tag,updated_by,date_updated)
														values(@id,@study_type_id,@srl,@wb_tag,@updated_by,getdate())

							 if(@@rowcount=0)
								begin
									rollback transaction
									select @return_status=0,@error_code='154'
									return 0
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

							 insert into study_hdr_study_types(study_hdr_id,study_type_id,srl_no,write_back_tag,updated_by,date_updated)
														values(@id,@study_type_id,@srl,@wb_tag,@updated_by,getdate())

							 if(@@rowcount=0)
								begin
									rollback transaction
									select @return_status=0,@error_code='155'
									return 0
								end
						end
				end

			insert into sys_case_study_status_log(study_id,study_uid,status_id_from,status_id_to,date_updated)
			                               values(@id,@study_uid,@study_status_pacs,@study_status_pacs,getdate())
			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status=0,@error_code='142'
					return 0
				end

			if(@species <>'')
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
								select @return_status=0,@error_code='143'
								return 0
							end

					 set @breed_id = isnull((select id from
												(select top 1 id,name
												from breed
												where species_id = @species_id
												and is_active='Y'
												and (upper(name)=upper(@breed)
													   or upper(name) like '%' + upper(@breed) + '%'
													   or code = upper(@breed)
													   or code like '%' + @breed + '%')order by name)t),'00000000-0000-0000-0000-000000000000')

					update study_hdr set breed_id = @breed_id where id=@id

				    if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='144'
							return 0
						end
					
				end
			
			if(@body_part <> '')
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
								select @return_status=0,@error_code='146'
								return 0
							end
				end

			if(@institution_name<>'')
				begin
					
					if(select count(id)
					   from institutions
					   where upper(name) = upper(@institution_name)
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
									select @return_status=0,@error_code='147' 
									return 0
								end
						end
					else
						begin
							
							select @institution_id=isnull((select id from institutions
															where upper(name) = upper(@institution_name)
															and is_active = 'Y'),'00000000-0000-0000-0000-000000000000')

							

							update study_hdr set institution_id = @institution_id where id=@id

							if(@@rowcount=0)
									begin
										rollback transaction
										select @return_status=0,@error_code='149'
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
											select @return_status=0,@error_code='150'
											return 0
										end
								end
						end 
				end

			if(@referring_physician<>'' and isnull(@institution_id,'00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
					begin
					
						if(select count(physician_id) from institution_physician_link 
						    where institution_id = @institution_id
							and (upper(physician_name) = upper(replace(@referring_physician,'  ',' ')) 
							or ((upper(physician_fname) + ' ' + upper(physician_lname) + upper(physician_credentials)) = upper(@referring_physician))
							or ((upper(physician_fname) + ' ' + upper(physician_lname) + ' ' +upper(physician_credentials)) = upper(replace(@referring_physician,'  ',' '))
							or ((upper(physician_lname) + ' ' + upper(physician_fname) + upper(physician_credentials)) = upper(@referring_physician))
							or ((upper(physician_lname) + ' ' + upper(physician_fname) + ' ' +upper(physician_credentials)) = upper(replace(@referring_physician,'  ',' '))))))>0
							
							begin
								
								select @physician_id=isnull((select physician_id from institution_physician_link 
															 where (upper(physician_name) = upper(@referring_physician) 
															 or ((upper(physician_lname) + ' ' + upper(physician_fname) + upper(physician_credentials)) = upper(@referring_physician)))
															 and institution_id = @institution_id),'00000000-0000-0000-0000-000000000000')

								


								update study_hdr set physician_id = @physician_id where id=@id

								if(@@rowcount=0)
									begin
										rollback transaction
										select @return_status=0,@error_code='148'
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
														select @return_status=0,@error_code='151'
														return 0
													end
											end
									end
							end
					end
				

			if(isnull(@sales_person,'')<>'')
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
										select @return_status=0,@error_code='156'
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
														select @return_status=0,@error_code='151'
														return 0
													end
											end
									end
							end
						
					end
			
			set @gen_mail='N'
		    if(@study_status_pacs = 80)
				begin
					set @gen_mail='Y'
					set @rpt_type='Preliminary'
				end
			else if(@study_status_pacs = 100)
				begin
					set @gen_mail='Y'
					set @rpt_type='Final'
				end

			if(@gen_mail='Y')
				begin
			
					select @recipient_address = isnull(physician_email,''),
						   @recipient_name    = isnull(physician_name,'')
					from institution_physician_link
					where institution_id = isnull(@institution_id,'00000000-0000-0000-0000-000000000000')
					and physician_id = isnull(@physician_id,'00000000-0000-0000-0000-000000000000')

					if(isnull(@recipient_address,'') <> '')
						begin
								select @patient_name = patient_name
								from study_hdr
								where id= @id
								and study_uid = @study_uid

								select @owner_name = rtrim(ltrim(owner_first_name + ' ' + owner_last_name))
								from study_hdr
								where id= @id
								and study_uid = @study_uid

								select @VRSPACSLINKURL = data_type_string
								from general_settings
								where control_code ='VRSPACSLINKURL'

								set @VRSPACSLINKURL = @VRSPACSLINKURL + convert(varchar(36),@id)

								set @study_types=''
								select @study_types = @study_types +  convert(varchar,st.name) + ','
								from study_hdr_study_types shst
								inner join  modality_study_types st on st.id= shst.study_type_id
								where shst.study_hdr_id=@id 

								if(isnull(@study_types,'')<>'') select @study_types = substring(@study_types,1,len(@study_types)-1)
						

								set @email_subject = @rpt_type + ' Report Available For ' + isnull(@patient_name,'')
						
								set @email_text    = @rpt_type + ' report is available for :- \n\n'
								set @email_text    = @email_text + ' Patient    : ' + @patient_name + '\n'
								--set @email_text    = @email_text + ' Owner      : ' + @owner_name + '\n'
								set @email_text    = @email_text + ' Species    : ' + isnull((select name from species where id = (select species_id from study_hdr where id=@id and study_uid = @study_uid)),'') + '\n'
								set @email_text    = @email_text + ' Breed      : ' + isnull((select name from breed where id = (select breed_id from study_hdr where id=@id and study_uid = @study_uid)),'') + '\n'
								set @email_text    = @email_text + ' Modality   : ' + isnull((select name from modality where id = (select modality_id from study_hdr where id=@id and study_uid = @study_uid)),'') + '\n'
								set @email_text    = @email_text + ' Study Type : ' + isnull(@study_types,'') + '\n'
								set @email_text    = @email_text + '\n\n'
								set @email_text    = @email_text +'<a href=''' + @VRSPACSLINKURL + ''' target=''_blank''>Click here to view the report</a>\n\n'
								set @email_text    = @email_text +'This is an automated message from VETRIS.Please do not reply to the message.\n'


								insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,
													            email_subject,email_text,study_hdr_id,study_uid,date_updated)
												         values(newid(),getdate(),@recipient_address,@recipient_name,
													            @email_subject,@email_text,@id,@study_uid,getdate())

								if(@@rowcount=0)
									begin
										rollback transaction
										select @return_status=0,@error_code='Failed to generate email record for Study UID ' + @study_uid
										return 0
									end

								--if(@rpt_type = 'Preliminary')
								--	begin
								--		update study_hdr_prelim_reports
								--		set report_text   = @report_text,
								--			email_updated ='Y'
								--		where study_hdr_id = @id 
								--		and study_uid =@study_uid

								--		if(@@rowcount=0)
								--			begin
								--				rollback transaction
								--				select @return_status=0,@error_code='Failed to update report for Study UID ' + @study_uid
								--				return 0
								--			end

								--		update study_hdr set prelim_rpt_updated = 'Y' where id =@id

								--		if(@@rowcount=0)
								--			begin
								--				rollback transaction
								--				select @return_status=0,@error_code='Failed to update status for report synch for Study UID ' + @study_uid
								--				return 0
								--			end
								--	end
								--else if(@rpt_type = 'Final')
								--	begin
								--		update study_hdr_final_reports
								--		set report_text   = @report_text,
								--			email_updated ='Y'
								--		where study_hdr_id = @id 
								--		and study_uid =@study_uid

								--		if(@@rowcount=0)
								--			begin
								--				rollback transaction
								--				select @return_status=0,@error_code='Failed to update report for Study UID ' + @study_uid
								--				return 0
								--			end

								--		 update study_hdr set final_rpt_updated = 'Y' where id =@id

								--		 if(@@rowcount=0)
								--			begin
								--				rollback transaction
								--				select @return_status=0,@error_code='Failed to update status for report synch for Study UID ' + @study_uid
								--				return 0
								--			end
								--	end
						end
			

		end
			
			 commit transaction
			 set @return_status=1
			 set @error_code='141'

		end
	else
		begin
			 set @return_status=0
			 set @error_code='139'
		end

   

	set nocount off
	return 1

end


GO
