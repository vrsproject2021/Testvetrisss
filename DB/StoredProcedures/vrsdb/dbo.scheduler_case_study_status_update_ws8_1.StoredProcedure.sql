USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_case_study_status_update_ws8_1]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_case_study_status_update_ws8_1]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_case_study_status_update_ws8_1]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_case_study_status_update_ws8 : update
                  case study status
** Created By   : Pavel Guha
** Created On   : 15/04/2019
*******************************************************/
CREATE procedure [dbo].[scheduler_case_study_status_update_ws8_1]
    @study_id uniqueidentifier,
	@study_uid nvarchar(100),
	@status_id int,
	@radiologist nvarchar(250)=null,
	@institution_name nvarchar(100)=null,
	@manufacturer_name nvarchar(100)=null,
	@manufacturer_model_no nvarchar(100)=null,
	@modality_ae_title nvarchar(50)=null,
	@image_count int,
	@object_count int,
	@service_codes nvarchar(250) = null,
	@prelim_radiologist_pacs nvarchar(250)=null,
	@final_radiologist_pacs nvarchar(250)=null,
	@modality_pacs nvarchar(30)= null,
	@rpt_approve_date datetime ='01jan1900',
	@rpt_record_date datetime ='01jan1900',
	@accession_no nvarchar(50)='',
	@patient_id nvarchar(50)='',
	@patient_name nvarchar(250)='',
	@patient_sex nvarchar(20)='',
	@patient_dob datetime='01jan1900',
	@referring_physician nvarchar(100)='',
	@species nvarchar(30) = '',
	@breed nvarchar(30) = '',
	@patient_age nvarchar(20),
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	set nocount on
	declare @existing_status_id int,
	        @existing_radiologist nvarchar(250),
			@existing_prelim_radiologist nvarchar(250),
			@existing_final_radiologist nvarchar(250),
			@existing_institution nvarchar(100),
	        @vrs_status_id int,
			@institution_id uniqueidentifier,
			@device_id uniqueidentifier,
			@modality nvarchar(30),
			@radiologist_id uniqueidentifier,
			@prelim_radiologist_id uniqueidentifier,
			@final_radiologist_id uniqueidentifier,
			@modality_id int,
			@consult_applied nchar(1)

	declare @patient_fname nvarchar(80),
			@patient_lname nvarchar(80),
			@physician_id uniqueidentifier,
			@species_id int,
			@breed_id uniqueidentifier,
			@study_date datetime


	if(select count(id) from study_hdr where id = @study_id) >0
		begin
				set @referring_physician = replace(@referring_physician,'  ','')
				set @patient_age = 0
				select @study_date = study_date from study_hdr where id=@study_id

				if(isnull(@patient_dob,'01jan1900') <>'01jan1900') 
					begin
						set @patient_age = dbo.CalculateAge(@patient_dob,@study_date)
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
				if(rtrim(ltrim(@institution_name))<>'')
					begin
						select @institution_id=isnull((select id from institutions
														where upper(name) = upper(rtrim(ltrim(@institution_name)))
														and is_active = 'Y'),'00000000-0000-0000-0000-000000000000')
					end
				if(rtrim(ltrim(@referring_physician))<>'' and isnull(@institution_id,'00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
					begin
						if(select count(physician_id) from institution_physician_link 
						    where (upper(physician_name) = upper(replace(@referring_physician,'  ',' ')) 
							or ((upper(physician_lname) + ' ' + upper(physician_fname) + upper(physician_credentials)) = upper(@referring_physician))
							or ((upper(physician_lname) + ' ' + upper(physician_fname)+ ' ' +  upper(physician_credentials)) = upper(replace(@referring_physician,'  ',' ')))
							or ((upper(physician_fname) + ' ' + upper(physician_lname) + ' ' + upper(physician_credentials)) = upper(@referring_physician))
															 or ((upper(physician_fname) + ' ' + upper(physician_lname) + upper(physician_credentials)) = upper(@referring_physician)))
							and institution_id = @institution_id)>0
							begin
								select @physician_id=isnull((select physician_id from institution_physician_link 
															 where (upper(physician_name) = upper(@referring_physician) 
															 or ((upper(physician_lname) + ' ' + upper(physician_fname) + upper(physician_credentials)) = upper(@referring_physician))
															 or ((upper(physician_lname) + ' ' + upper(physician_fname) + ' ' + upper(physician_credentials)) = upper(@referring_physician))
															 or ((upper(physician_fname) + ' ' + upper(physician_lname) + ' ' + upper(physician_credentials)) = upper(@referring_physician))
															 or ((upper(physician_fname) + ' ' + upper(physician_lname) + upper(physician_credentials)) = upper(@referring_physician)))
															 and institution_id = @institution_id),'00000000-0000-0000-0000-000000000000')

							end
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

							set @breed_id = isnull((select id from
													(select top 1 id,name
													from breed
													where species_id = @species_id
													and is_active='Y'
													and (upper(name)=upper(rtrim(ltrim(@breed)))
															or upper(name) like '%' + upper(rtrim(ltrim(@breed))) + '%'
															or code = upper(rtrim(ltrim(@breed)))
															or code like '%' + rtrim(ltrim(@breed)) + '%')order by name)t),'00000000-0000-0000-0000-000000000000')

					
					
					end

				update study_hdr
				set accession_no=@accession_no,
					patient_id= @patient_id,
					patient_name=@patient_name,
					patient_fname=@patient_fname,
					patient_lname=@patient_lname,
					patient_sex=@patient_sex,
					patient_dob_accepted=@patient_dob,
					patient_age_accepted= @patient_age,
					physician_id =@physician_id,
					species_id = isnull(@species_id,0),
					breed_id = isnull(@breed_id,'00000000-0000-0000-0000-000000000000')
				where study_uid=@study_uid
				
				select @existing_status_id = hdr.study_status_pacs,
					   @existing_radiologist = isnull(hdr.radiologist_pacs,''),
					   @existing_institution =isnull(i.name,''),
					   @modality = hdr.modality_pacs
				from study_hdr hdr
				inner join institutions i on i.id = hdr.institution_id
				where hdr.id = @study_id
				and hdr.study_uid = @study_uid
		end
	else
		begin
			select     @existing_status_id = hdr.study_status_pacs,
					   @existing_radiologist = isnull(hdr.radiologist_pacs,''),
					   @existing_institution =isnull(i.name,''),
					   @modality = hdr.modality_pacs
				from study_hdr_archive hdr
				inner join institutions i on i.id = hdr.institution_id
				where hdr.id = @study_id
				and hdr.study_uid = @study_uid
		end


	update study_synch_dump
	set deleted  = 'N'
	where study_uid = @study_uid
	
	update study_hdr
	set deleted  = 'N'
	where id      = @study_id
	and study_uid = @study_uid

	--if(@@rowcount=0)
	--	begin
	--		rollback transaction
	--		select @return_type=0,@error_msg='Failed to update delete flag for Study UID ' + @study_uid
	--		return 0
	--	end

	if(rtrim(ltrim(isnull(@radiologist,''))) <> '')
		begin
			set @radiologist = replace(rtrim(ltrim(isnull(@radiologist,''))),'  ','')
	
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
			where ( (rtrim(ltrim(upper(name))) = upper(@prelim_radiologist_pacs)) or
			        (rtrim(ltrim(upper(isnull(lname,'')))) + ' ' + rtrim(ltrim(upper(isnull(fname,'')))) + ' ' + rtrim(ltrim(upper(isnull(credentials,'')))) = upper(@prelim_radiologist_pacs)) )


			set @prelim_radiologist_id= isnull(@prelim_radiologist_id,'00000000-0000-0000-0000-000000000000')
		end

	if(rtrim(ltrim(isnull(@final_radiologist_pacs,''))) <> '')
		begin
			set @final_radiologist_pacs = replace(rtrim(ltrim(isnull(@final_radiologist_pacs,''))),'  ','')
	
			select @final_radiologist_id = id 
			from radiologists
			where ( (rtrim(ltrim(upper(name))) = upper(@final_radiologist_pacs)) or
			        (rtrim(ltrim(upper(isnull(lname,'')))) + ' ' + rtrim(ltrim(upper(isnull(fname,'')))) + ' ' + rtrim(ltrim(upper(isnull(credentials,'')))) = upper(@final_radiologist_pacs)) )

			set @final_radiologist_id= isnull(@final_radiologist_id,'00000000-0000-0000-0000-000000000000')
		end

	if(replace(rtrim(ltrim(upper(@existing_radiologist))),'  ', '')<>upper(@radiologist))
		begin
			begin transaction

			if(select count(id) from study_hdr where id = @study_id) >0
				begin
						update study_hdr
						set radiologist_pacs  = isnull(@radiologist,''),
							radiologist_id    = @radiologist_id
						where id      = @study_id
						and study_uid = @study_uid
				end
			else
				begin
						update study_hdr_archive
						set radiologist_pacs  = isnull(@radiologist,''),
							radiologist_id    = @radiologist_id
						where id      = @study_id
						and study_uid = @study_uid
				end

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to update radiologist for Study UID ' + @study_uid
					return 0
				end

			 commit transaction
			 set @return_type=1
			 set @error_msg=''
		end
	else
		begin
			 set @return_type=2
			 set @error_msg=''
		end

	--if(replace(rtrim(ltrim(upper(isnull(@existing_prelim_radiologist,'')))),'  ', '')<>upper(@prelim_radiologist_pacs))
	--	begin
			begin transaction

			if(select count(id) from study_hdr where id = @study_id) >0
				begin
					update study_hdr
					set prelim_radiologist_pacs  = isnull(@prelim_radiologist_pacs,''),
						prelim_radiologist_id    = @prelim_radiologist_id
					where id      = @study_id
					and study_uid = @study_uid
				end
			else
				begin
					update study_hdr_archive
					set prelim_radiologist_pacs  = isnull(@prelim_radiologist_pacs,''),
						prelim_radiologist_id    = @prelim_radiologist_id
					where id      = @study_id
					and study_uid = @study_uid
				end

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to update preliminary radiologist for Study UID ' + @study_uid
					return 0
				end
			else
				begin
						set @return_type=2
						set @error_msg=''
						commit transaction
				end

	--		 commit transaction
	--		 set @return_type=1
	--		 set @error_msg=''
	--	end
	--else
	--	begin
	--		 set @return_type=2
	--		 set @error_msg=''
	--	end

	
	--if(replace(rtrim(ltrim(upper(isnull(@existing_final_radiologist,'')))),'  ', '')<>upper(@final_radiologist_pacs))
	--	begin
			begin transaction
			if(select count(id) from study_hdr where id = @study_id) >0
				begin
					update study_hdr
					set final_radiologist_pacs  = isnull(@final_radiologist_pacs,''),
						final_radiologist_id    = @final_radiologist_id,
						rpt_approve_date        = @rpt_approve_date,
						rpt_record_date         = @rpt_record_date
					where id      = @study_id
					and study_uid = @study_uid
				end
			else
				begin
					update study_hdr_archive
					set final_radiologist_pacs  = isnull(@final_radiologist_pacs,''),
						final_radiologist_id    = @final_radiologist_id,
						rpt_approve_date        = @rpt_approve_date,
						rpt_record_date         = @rpt_record_date
					where id      = @study_id
					and study_uid = @study_uid
				end

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to update final radiologist for Study UID ' + @study_uid
					return 0
				end
		    else
				begin
						set @return_type=2
						set @error_msg=''
						commit transaction
				end

			 --commit transaction
			 --set @return_type=1
			 --set @error_msg=''
		--end
	--else
	--	begin
	--		 set @return_type=2
	--		 set @error_msg=''
	--	end

	--if(rtrim(ltrim(isnull(@modality_pacs,''))) <> '')
	--	begin
	--		begin transaction
	--		set @modality_id= isnull((select id from
	--								(select top 1 id,name
	--								from modality
	--								where is_active='Y'
	--								and (upper(name)=upper(rtrim(ltrim(@modality_pacs)))
	--										or upper(name) like '%' + upper(rtrim(ltrim(@modality_pacs))) + '%'
	--										or code = upper(rtrim(ltrim(@modality_pacs)))
	--										or code like '%' + rtrim(ltrim(@modality_pacs)) + '%'
	--										or upper(dicom_tag) = upper(rtrim(ltrim(@modality_pacs)))
	--										or upper(dicom_tag) like '%' + upper(rtrim(ltrim(@modality_pacs))) + '%') order by name)t),0)

	--			update study_hdr set modality_id = @modality_id where id=@study_id

	--			if(@@rowcount=0)
	--				begin
	--					rollback transaction
	--					select @return_type=0,@error_msg='Failed to update modality id for Study UID ' + @study_uid
	--					return 0
	--				end
	--			else
	--				begin
	--						set @return_type=2
	--						set @error_msg=''
	--						commit transaction
	--				end
	--	end

    if(@institution_name<>'')
		begin
			begin transaction 

			if(select count(id)
				from institutions
				where upper(name) = upper(@institution_name)
				and is_active = 'Y') =0
				begin 
					set @institution_id = newid()
					insert into institutions(id,name,created_by,date_created) values (@institution_id,@institution_name,'00000000-0000-0000-0000-000000000000',getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to create institution for Study UID ' + @study_uid
							return 0
						end
					
					if(select count(id) from study_hdr where id = @study_id) >0
						begin
							if(rtrim(ltrim(@existing_institution)) <> rtrim(ltrim(@institution_name)))
								begin
									update study_hdr set institution_id = @institution_id where id=@study_id

									if(@@rowcount=0)
										begin
											rollback transaction
											select @return_type=0,@error_msg='Failed to update institution id for Study UID ' + @study_uid
											return 0
										end
								end
						end
				end
			else
				begin
					select @institution_id=isnull((select id from institutions
													where upper(name) = upper(@institution_name)
													and is_active = 'Y'),'00000000-0000-0000-0000-000000000000')
					
					if(select count(id) from study_hdr where id = @study_id) >0
						begin
							update study_hdr set institution_id = @institution_id where id=@study_id

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to update institution id for Study UID ' + @study_uid
									return 0
								end
						end
				end

			if(@manufacturer_name<>'')
				begin
					if(select count(device_id) from  institution_device_link where upper(manufacturer)=upper(@manufacturer_name) and upper(modality)=upper(@modality) and upper(modality_ae_title)= upper(@modality_ae_title) and institution_id = @institution_id)=0
						begin
							set @device_id=newid()

							insert into institution_device_link(device_id,institution_id,manufacturer,modality,modality_ae_title,created_by,date_created)
															values(@device_id,@institution_id,@manufacturer_name,@modality,@modality_ae_title,'00000000-0000-0000-0000-000000000000',getdate())

								if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to create manufucaturer details for Study UID ' + @study_uid
									return 0
								end
						end
				end 

			commit transaction
		end
	else
		begin
			set @return_type=2
			set @error_msg=''
		end

	set @service_codes = upper(isnull(@service_codes,''))
	if(charindex('CONSULT',@service_codes) >0)
		begin
			set @consult_applied='Y'
		end
	else
		begin
			set @consult_applied='N'
		end

	begin transaction

	if(select count(id) from study_hdr where id = @study_id) >0
		begin
			update study_hdr 
			set img_count        = @image_count,
				object_count     = @object_count,
				service_codes    = isnull(@service_codes,''),
				consult_applied  = @consult_applied,
				rpt_approve_date = isnull(@rpt_approve_date,'01jan1900'),
				rpt_record_date  = isnull(@rpt_record_date,'01jan1900')
			where id=@study_id
		end
	else
		begin
			update study_hdr_archive
			set img_count        = @image_count,
				object_count     = @object_count,
				service_codes    = isnull(@service_codes,''),
				consult_applied  = @consult_applied,
				rpt_approve_date = isnull(@rpt_approve_date,'01jan1900'),
				rpt_record_date  = isnull(@rpt_record_date,'01jan1900')
			where id=@study_id
		end

	if(@@rowcount=0)
		begin
			rollback transaction
			select @return_type=0,@error_msg='Failed to update image count for Study UID ' + @study_uid
			return 0
		end
	else
		begin
				set @return_type=2
				set @error_msg=''
				commit transaction
		end

	if(select count(id) from study_hdr where id = @study_id) >0
		begin
			if(@existing_status_id<>@status_id)
				begin
					if(@status_id > @existing_status_id)
						begin
							begin transaction

							select @vrs_status_id = vrs_status_id
							from sys_study_status_pacs
							where status_id = @status_id

							update study_hdr
							set study_status_pacs      = @status_id,
								study_status           = @vrs_status_id,
								radiologist_pacs       = isnull(@radiologist,''),
								radiologist_id         = @radiologist_id,
								service_codes          = isnull(@service_codes,''),
								consult_applied        = @consult_applied,
								rpt_approve_date       = isnull(@rpt_approve_date,'01jan1900'),
				                rpt_record_date        = isnull(@rpt_record_date,'01jan1900'),
								status_last_updated_on = getdate()
							where id      = @study_id
							and study_uid = @study_uid

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to update status for Study UID ' + @study_uid
									return 0
								end

							insert into sys_case_study_status_log(study_id,study_uid,status_id_from,status_id_to,date_updated)
														  values(@study_id,@study_uid,@existing_status_id,@status_id,getdate())

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to update status log for Study UID ' + @study_uid
									return 0
								end

							 commit transaction
							 set @return_type=1
							 set @error_msg='Status of for Study UID ' + @study_uid + ' updated successfully'
						end
					else if(@status_id < @existing_status_id)
						begin
							if((@status_id=0 or @status_id=20) and @existing_status_id=50)
								begin
									update study_hdr
									set pacs_wb ='Y'
									where study_uid = @study_uid
									and id = @study_id

									if(@@rowcount=0)
										begin
											rollback transaction
											select @return_type=0,@error_msg='Failed to update status for Study UID ' + @study_uid
											return 0
										end
								end
						end
					else
						begin
							 set @return_type=2
							 set @error_msg=''
						end

				end
			else
				begin
					begin transaction

					update study_hdr
					set radiologist_pacs  = isnull(@radiologist,''),
						service_codes     = isnull(@service_codes,''),
						consult_applied   = @consult_applied,
						rpt_approve_date  = isnull(@rpt_approve_date,'01jan1900'),
				        rpt_record_date  = isnull(@rpt_record_date,'01jan1900')
					where id      = @study_id
					and study_uid = @study_uid

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to update status for Study UID ' + @study_uid
							return 0
						end

					commit transaction
				end
		end
	set nocount off
	return 1

end


GO
