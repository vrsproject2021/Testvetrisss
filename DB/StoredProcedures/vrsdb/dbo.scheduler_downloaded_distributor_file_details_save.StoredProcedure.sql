USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_downloaded_distributor_file_details_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_downloaded_distributor_file_details_save]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_downloaded_distributor_file_details_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_downloaded_distributor_file_details_save : 
                  save file downloaded from distributor
** Created By   : Pavel Guha
** Created On   : 05/08/2021
*******************************************************/
CREATE procedure [dbo].[scheduler_downloaded_distributor_file_details_save]
    @study_uid nvarchar(100),
	@study_date datetime='01jan1900',
	@institution_code nvarchar(5),
	@institution_name nvarchar(100),
	@patient_id nvarchar(20) = null,
	@patient_fname nvarchar(40) = null,
	@patient_lname nvarchar(40) = null,
	@file_name nvarchar(250),
	@accession_no nvarchar(20)=null,
	@reason nvarchar(500)=null,
	@modality nvarchar(50)=null,
	@manufacturer_name nvarchar(100)=null,
	@device_serial_no nvarchar(20)=null,
	@manufacturer_model_no nvarchar(100)=null,
	@modality_ae_title nvarchar(50)=null,
	@referring_physician nvarchar(200)=null,
	@patient_sex nvarchar(10)=null,
	@patient_dob datetime=null,
	@patient_age varchar(50)=null,
	@priority_id int = 0,
	@import_session_id nvarchar(30) = null, 
	@is_manual nchar(1)='N',
	@series_uid nvarchar(100),
	@sop_instance_uid nvarchar(100),
	@delete_file nchar(1)='N' output,
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	set nocount on

	declare @institution_id uniqueidentifier,
			@inst_name nvarchar(100),
	        @file_count int,
			@id uniqueidentifier,
			@modality_id int,
			@device_id uniqueidentifier,
			@physician_id uniqueidentifier,
			@dcm_file_xfer_pacs_mode nchar(1),
			@finishing_datetime datetime,
			@finishing_time_hrs int,
			@is_add_on nchar(1),
			@received_via_dicom_router nchar(1),
			@send_to_pacs nchar(1),
			@study_found nchar(1),
			@error_code nvarchar(10),
			@return_status int,
			@activity_text nvarchar(max),
			@object_count_pacs int,
			@status_id int

	declare @patient_age_accepted nvarchar(50),
	        @owner_last_name nvarchar(100),
			@synched_on datetime,
			@patient_name nvarchar(100),
			@default_country_id int,
			@wt_uom nvarchar(5)

	declare @approve_for_pacs nchar(1),
			@approved_by uniqueidentifier,
			@date_approved datetime,
			@diff int

    set @send_to_pacs='Y'

	set @institution_id = isnull((select id 
	                              from institutions
							      where code = @institution_code),'00000000-0000-0000-0000-000000000000')

	set @institution_name = isnull((select name 
									from institutions
									where id = @institution_id),'')

	if(select count(study_uid) from study_synch_dump where study_uid=@study_uid)=0
		begin
			begin transaction

			set @patient_name = rtrim(ltrim(isnull(@patient_fname,'') + ' ' + isnull(@patient_lname,'')))
			insert into study_synch_dump(study_uid,study_date,received_date,accession_no,reason,
										institution_name,manufacturer_name,manufacturer_model_no,device_serial_no,modality_ae_title,referring_physician,
										patient_id,patient_name,patient_sex,patient_dob,patient_age,patient_weight,sex_neutered,
										owner_name,species,breed,modality,body_part,img_count,study_desc,priority_id,object_count,synched_on)
									values(@study_uid,@study_date,getdate(),@accession_no,@reason,
										@institution_name,@manufacturer_name,@manufacturer_model_no,@device_serial_no,@modality_ae_title,@referring_physician,
										@patient_id,@patient_name,@patient_sex,@patient_dob,@patient_age,0,'',
										'','','',@modality,'',1,'',@priority_id,1,getdate())

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

			set @patient_age_accepted = ''

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
			set @owner_last_name = @patient_lname

			set @synched_on= getdate()
			if(@priority_id =0) set @priority_id=20
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
								patient_weight_pacs,patient_weight,
								owner_name_pacs,owner_first_name,owner_last_name,
								species_pacs,breed_pacs,modality_pacs,body_part_pacs,study_status_pacs,
								img_count_pacs,img_count,object_count,object_count_pacs,study_desc,
								priority_id_pacs,priority_id,finishing_datetime,
								synched_on,sync_mode,date_updated,status_last_updated_on)
						values(@id,@study_uid,@study_date,@synched_on,
							@accession_no,@accession_no,
							@reason,@reason,
							@institution_name,@manufacturer_name,@device_serial_no,@modality_ae_title,@referring_physician,
							@patient_id,@patient_id,
							@patient_name,@patient_name,@patient_fname,@patient_lname,
							@patient_sex,@patient_sex,
							'','',
							@patient_dob,@patient_dob,
							@patient_age,@patient_age_accepted,
							0,0,
							'','',@owner_last_name,
							'','',@modality,'',0,
							1,1,0,1,'',
							@priority_id,@priority_id,@finishing_datetime,
							@synched_on,'FD',getdate(),getdate())	

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
							if(select count(device_id) from  institution_device_link 
								where upper(manufacturer)=upper(@manufacturer_name) 
								and upper(modality)=upper(@modality) 
								and upper(modality_ae_title)= upper(@modality_ae_title) 
								and institution_id = @institution_id)=0
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
				@activity_text = 'Received From File Distribution Service',
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
		end

	begin transaction
	set @delete_file='N'

	if(select count(study_uid) from dicom_router_files_received where study_uid=@study_uid  and file_series_uid=@series_uid and file_instance_no=@sop_instance_uid)=0
		begin
			insert into dicom_router_files_received(study_uid,import_session_id,institution_id,institution_code,
													file_name,file_type,file_series_uid,file_instance_no,date_received)
											values(@study_uid,@import_session_id,@institution_id,@institution_code,
													@file_name,'D',@series_uid,@sop_instance_uid,getdate())

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to import study file record - SUID ' + @study_uid
					return 0
				end
		end
	else
		begin
			rollback transaction
			select @return_type=0,@error_msg='' + @study_uid,@delete_file='Y'
			return 0
		end

	if(select count(study_uid) from scheduler_file_downloads where study_uid=@study_uid)=0
		begin
			if(select count(id) from study_hdr where study_uid=@study_uid)>0
				begin
					select @id = id from study_hdr where study_uid=@study_uid
					set @approve_for_pacs='Y'
					set @date_approved = getdate()
					set @study_found='Y'
				end
			else if(select count(id) from study_hdr_archive where study_uid=@study_uid)>0
				begin
					select @id = id from study_hdr_archive where study_uid=@study_uid
					set @approve_for_pacs='Y'
					set @date_approved = getdate()
					set @study_found='Y'
				end
			else
				begin
					set @id = newid()
					set @study_found='N'
					set @approve_for_pacs='Y'
					set @date_approved = null
				end

			
			insert into scheduler_file_downloads(id,study_uid,study_date,file_count,received_date,
												institution_id,institution_code,institution_name,
												patient_id,patient_fname,patient_lname,patient_sex,patient_dob,patient_age,
												accession_no,reason,modality,
												manufacturer_name,device_serial_no,manufacturer_model_no,modality_ae_title,
												referring_physician,priority_id,date_downloaded,approve_for_pacs,approved_by,date_approved,
												import_session_id,is_manual,study_found)
										values(@id,@study_uid,@study_date,1,getdate(),
												@institution_id,@institution_code,@institution_name,
												@patient_id,@patient_fname,@patient_lname,@patient_sex,@patient_dob,@patient_age,
												@accession_no,@reason,@modality,
												@manufacturer_name,@device_serial_no,@manufacturer_model_no,@modality_ae_title,
												@referring_physician,@priority_id,getdate(),@approve_for_pacs,'00000000-0000-0000-0000-000000000000',@date_approved,
												@import_session_id,@is_manual,@study_found)	
			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to create details of Study UID ' + @study_uid
					return 0
				end

			insert into scheduler_file_downloads_dtls(id,study_uid,file_name,series_uid,instance_no,import_session_id,sent_to_pacs)
												values(@id,@study_uid,@file_name,@series_uid,@sop_instance_uid,@import_session_id,@send_to_pacs)
			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to create file details of Study UID ' + @study_uid + ', File : ' + @file_name
					return 0
				end

			select @error_code='',@return_status=0
			set @activity_text='DICOM files - sync started'
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
				
		end
	else
		begin
				select @id= id 
				from scheduler_file_downloads
				where study_uid    = @study_uid
				--and institution_id = @institution_id

				if(select count(id) from scheduler_file_downloads_dtls where id=@id and study_uid=@study_uid and series_uid=@series_uid and instance_no=@sop_instance_uid) = 0
					begin
						insert into scheduler_file_downloads_dtls(id,study_uid,file_name,series_uid,instance_no,import_session_id,sent_to_pacs)
															values(@id,@study_uid,@file_name,@series_uid,@sop_instance_uid,@import_session_id,@send_to_pacs)

						if(@@rowcount=0)
							begin
								rollback transaction
								select @return_type=2,@error_msg='Failed to create file details of Study UID ' + @study_uid + ', File : ' + @file_name
								return 0
							end
					end
				else
					begin
						set @delete_file='Y'
					end
			
				select @file_count = count(file_name)
				from scheduler_file_downloads_dtls
				where id = @id

				update scheduler_file_downloads
				set study_date       = @study_date,
					file_count       = @file_count,
					date_downloaded  = getdate()
			    where study_uid    = @study_uid


				if(@@rowcount=0)
					begin
						rollback transaction
						select @return_type=0,@error_msg='Failed to update file count of Study UID ' + @study_uid
						return 0
					end
		  end

	set @file_count=0

	if(select count(id) from study_hdr where study_uid=@study_uid)>0
		begin
			select @id                = id
			from study_hdr 
			where study_uid = @study_uid

			select @file_count = (select count(file_name)
									from scheduler_file_downloads_dtls
									where id = @id)  
			
			update study_hdr
			set img_count_pacs    = @file_count,
			    img_count         = @file_count,
				object_count_pacs = @file_count,
				object_count      = @file_count
			where id = @id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to update file count of Study UID ' + @study_uid
					return 0
				end
		end
	else if(select count(id) from study_hdr_archive where study_uid=@study_uid)>0
		begin
			select @id                = id
			from study_hdr_archive 
			where study_uid=@study_uid
			
			select @file_count = (select count(file_name)
									from scheduler_file_downloads_dtls
									where id = @id)  
			
			update study_hdr_archive
			set img_count_pacs    = @file_count,
			    img_count         = @file_count,
				object_count_pacs = @file_count,
				object_count      = @file_count
			where id = @id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to update file count of Study UID ' + @study_uid
					return 0
				end
		  end

    commit transaction
	set @return_type=1
	set @error_msg=''

	

	set nocount off
	return 1

end


GO
