USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_case_study_status_update_ws8]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_case_study_status_update_ws8]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_case_study_status_update_ws8]    Script Date: 28-09-2021 19:36:35 ******/
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
CREATE procedure [dbo].[scheduler_case_study_status_update_ws8]
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
	@dict_radiologist_pacs nvarchar(250)=null,
	@prelim_radiologist_pacs nvarchar(250)=null,
	@final_radiologist_pacs nvarchar(250)=null,
	@modality_pacs nvarchar(30)= null,
	@rpt_approve_date datetime ='01jan1900',
	@rpt_record_date datetime ='01jan1900',
	@additional_field nvarchar(5)='',
	@additional_field_value nvarchar(2000)='',
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	set nocount on
	declare @existing_status_id int,
	        @existing_radiologist nvarchar(250),
			@existing_radiologist_id uniqueidentifier,
			@existing_dict_radiologist nvarchar(250),
			@existing_prelim_radiologist nvarchar(250),
			@existing_final_radiologist nvarchar(250),
			@existing_final_radiologist_id uniqueidentifier,
			@existing_prelim_radiologist_id uniqueidentifier, 
			@existing_institution nvarchar(100),
			@existing_institution_code nvarchar(10),
	        @vrs_status_id int,
			@institution_id uniqueidentifier,
			@device_id uniqueidentifier,
			@modality nvarchar(30),
			@radiologist_id uniqueidentifier,
			@dict_radiologist_id uniqueidentifier,
			@prelim_radiologist_id uniqueidentifier,
			@final_radiologist_id uniqueidentifier,
			@modality_id int,
			@consult_applied nchar(1),
			@category_id int,
			@addl_field_db nvarchar(30),
			@addl_field_db_type nchar(1),
			@addl_field_alias nvarchar(100),
			@addl_field_tbl_name nvarchar(50),
			@dict_rpt_count int,
			@prelim_rpt_count int,
			@final_rpt_count int,
			@transcription_finishing_time_mins int,
			@transcription_finishing_datetime datetime,
			@priority_id int,
			@strSQL varchar(8000)


	if(select count(id) from study_hdr where id = @study_id) >0
		begin
			if(select pacs_wb from study_hdr where id = @study_id) ='Y'
				begin
					select @return_type=0,@error_msg='Write back pending for Study UID ' + @study_uid 
					return 0
				end

				select @existing_status_id = hdr.study_status_pacs,
					   @existing_radiologist = isnull(hdr.radiologist_pacs,''),
					   @existing_radiologist_id = isnull(hdr.radiologist_id,'00000000-0000-0000-0000-000000000000'),
					   @existing_institution =isnull(i.name,''),
					   @existing_institution_code = isnull(i.code,''),
					   @modality = hdr.modality_pacs
				from study_hdr hdr
				inner join institutions i on i.id = hdr.institution_id
				where hdr.id = @study_id
				and hdr.study_uid = @study_uid
		end
	else if(select count(id) from study_hdr_archive where id = @study_id) >0
		begin
			select     @existing_status_id = hdr.study_status_pacs,
					   @existing_radiologist = isnull(hdr.radiologist_pacs,''),
					   @existing_radiologist_id = isnull(hdr.radiologist_id,'00000000-0000-0000-0000-000000000000'),
					   @existing_institution =isnull(i.name,''),
					   @existing_institution_code = isnull(i.code,''),
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
	
	if(rtrim(ltrim(isnull(@dict_radiologist_pacs,''))) <> '')
		begin
			set @dict_radiologist_pacs = replace(rtrim(ltrim(isnull(@dict_radiologist_pacs,''))),'  ','')
	
			select @dict_radiologist_id = id 
			from radiologists
			where ( (rtrim(ltrim(upper(name))) = upper(isnull(@dict_radiologist_pacs,''))) or
			        (rtrim(ltrim(upper(isnull(lname,'')))) + ' ' + rtrim(ltrim(upper(isnull(fname,'')))) + ' ' + rtrim(ltrim(upper(isnull(credentials,'')))) = upper(isnull(@dict_radiologist_pacs,''))) )


			set @dict_radiologist_id= isnull(@dict_radiologist_id,'00000000-0000-0000-0000-000000000000')
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


	if(@existing_status_id <60)
		begin
			if(replace(rtrim(ltrim(upper(@existing_radiologist))),'  ', '')<>upper(@radiologist))
				begin
					if(isnull(@existing_radiologist_id,'00000000-0000-0000-0000-000000000000')='00000000-0000-0000-0000-000000000000')
						begin
							begin transaction
			
							if(select count(id) from study_hdr where id = @study_id) >0
								begin
										select @existing_prelim_radiologist_id = isnull(radiologist_id,'00000000-0000-0000-0000-000000000000')
										from study_hdr 
										where id = @study_id

										if(isnull(@existing_radiologist_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000')
											begin
												update study_hdr
												set radiologist_pacs  = isnull(@radiologist,''),
													radiologist_id    = isnull(@radiologist_id,'00000000-0000-0000-0000-000000000000')
												where id      = @study_id
												and study_uid = @study_uid

												if(@@rowcount=0)
													begin
														rollback transaction
														select @return_type=0,@error_msg='Failed to update radiologist for Study UID ' + @study_uid
														return 0
													end
											end
								end
							else if(select count(id) from study_hdr_archive where id = @study_id) >0
								begin
										select @existing_prelim_radiologist_id = isnull(radiologist_id,'00000000-0000-0000-0000-000000000000')
										from study_hdr_archive 
										where id = @study_id

										if(isnull(@existing_radiologist_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000')
											begin
												update study_hdr_archive
												set radiologist_pacs  = isnull(@radiologist,''),
													radiologist_id    = isnull(@radiologist_id,'00000000-0000-0000-0000-000000000000')
												where id      = @study_id
												and study_uid = @study_uid

												if(@@rowcount=0)
													begin
														rollback transaction
														select @return_type=0,@error_msg='Failed to update radiologist for Study UID ' + @study_uid
														return 0
													end
											end
								end

							 commit transaction
						end

					 set @return_type=1
					 set @error_msg=''
				end
			else
				begin
					 set @return_type=2
					 set @error_msg=''
				end
		end

	select @dict_rpt_count = count(report_id) from study_hdr_dictated_reports where study_hdr_id = @study_id 
	select @dict_rpt_count = @dict_rpt_count + count(report_id) from study_hdr_dictated_reports_archive where study_hdr_id = @study_id 
	select @prelim_rpt_count = count(report_id) from study_hdr_prelim_reports where study_hdr_id = @study_id 
	select @prelim_rpt_count = @prelim_rpt_count + count(report_id) from study_hdr_prelim_reports_archive where study_hdr_id = @study_id 
	select @final_rpt_count = count(report_id) from study_hdr_final_reports where study_hdr_id = @study_id 
	select @final_rpt_count = @final_rpt_count + count(report_id) from study_hdr_final_reports_archive where study_hdr_id = @study_id 

	begin transaction


	if(select count(id) from study_hdr where id = @study_id) >0
		begin
			if(@dict_rpt_count=0 or @prelim_rpt_count=0)
				begin
					update study_hdr
					set dict_radiologist_pacs  = isnull(@dict_radiologist_pacs,''),
						dict_radiologist_id    = isnull(@dict_radiologist_id,'00000000-0000-0000-0000-000000000000'),
						rpt_record_date        = @rpt_record_date
					where id      = @study_id
					and study_uid = @study_uid

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to update dictation radiologist for Study UID ' + @study_uid
							return 0
						end
				end
		end
	else if(select count(id) from study_hdr_archive where id = @study_id) >0
		begin
			if(@dict_rpt_count=0 or @prelim_rpt_count=0)
				begin
					update study_hdr_archive
					set dict_radiologist_pacs  = isnull(@dict_radiologist_pacs,''),
						dict_radiologist_id    = isnull(@dict_radiologist_id,'00000000-0000-0000-0000-000000000000'),
						rpt_record_date        = @rpt_record_date
					where id      = @study_id
					and study_uid = @study_uid

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to update dictation radiologist for Study UID ' + @study_uid
							return 0
						end
				end
		end

	set @return_type=2
	set @error_msg=''
	commit transaction

	


	--if(replace(rtrim(ltrim(upper(isnull(@existing_prelim_radiologist,'')))),'  ', '')<>upper(@prelim_radiologist_pacs))
	--	begin
	    begin transaction

		if(@existing_status_id <80)		
			begin
				if(select count(id) from study_hdr where id = @study_id) >0
					begin
						if(@dict_rpt_count=0 or @prelim_rpt_count=0)
							begin
								update study_hdr
								set prelim_radiologist_pacs  = isnull(@prelim_radiologist_pacs,''),
									prelim_radiologist_id    = isnull(@prelim_radiologist_id,'00000000-0000-0000-0000-000000000000')
								where id      = @study_id
								and study_uid = @study_uid

								if(@@rowcount=0)
									begin
										rollback transaction
										select @return_type=0,@error_msg='Failed to update preliminary radiologist for Study UID ' + @study_uid
										return 0
									end
							end

						if(select isnull(radiologist_id,'00000000-0000-0000-0000-000000000000') from study_hdr where id=@study_id) = '00000000-0000-0000-0000-000000000000'
							begin
								update study_hdr
								set radiologist_pacs  = isnull(@prelim_radiologist_pacs,''),
									radiologist_id    = isnull(@prelim_radiologist_id,'00000000-0000-0000-0000-000000000000')
								where id      = @study_id
								and study_uid = @study_uid

								if(@@rowcount=0)
									begin
										rollback transaction
										select @return_type=0,@error_msg='Failed to update radiologist for Study UID ' + @study_uid
										return 0
									end
							end
					
					end
				else if(select count(id) from study_hdr_archive where id = @study_id) >0
					begin
						if(@dict_rpt_count=0 or @prelim_rpt_count=0)
							begin
								update study_hdr_archive
								set prelim_radiologist_pacs  = isnull(@prelim_radiologist_pacs,''),
									prelim_radiologist_id    = isnull(@prelim_radiologist_id,'00000000-0000-0000-0000-000000000000')
								where id      = @study_id
								and study_uid = @study_uid

								if(@@rowcount=0)
									begin
										rollback transaction
										select @return_type=0,@error_msg='Failed to update preliminary radiologist for Study UID ' + @study_uid
										return 0
									end
							end
					end
			end

			set @return_type=2
			set @error_msg=''
			commit transaction

	--		 commit transaction
	--		 set @return_type=1
	--		 set @error_msg=''
	--	end
	--else
	--	begin
	--		 set @return_type=2
	--		 set @error_msg=''
	--	end

	
	if(replace(rtrim(ltrim(upper(isnull(@existing_final_radiologist,'')))),'  ', '')<>upper(@final_radiologist_pacs))
		begin
			begin transaction
			if(select count(id) from study_hdr where id = @study_id) >0
				begin
					select @existing_final_radiologist_id = final_radiologist_id from study_hdr where id=@study_id
					if(@final_rpt_count>0  and isnull(@existing_final_radiologist_id,'00000000-0000-0000-0000-000000000000')='00000000-0000-0000-0000-000000000000')
						begin
							update study_hdr
							set final_radiologist_pacs  = isnull(@final_radiologist_pacs,''),
								final_radiologist_id    = isnull(@final_radiologist_id,'00000000-0000-0000-0000-000000000000'),
								rpt_approve_date        = @rpt_approve_date	
							where id      = @study_id
							and study_uid = @study_uid

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to update final radiologist for Study UID ' + @study_uid
									return 0
								end
							
						end
				end
			else if(select count(id) from study_hdr_archive where id = @study_id) >0
				begin
					select @existing_final_radiologist_id = final_radiologist_id from study_hdr_archive where id=@study_id
					if(@final_rpt_count>0  and isnull(@existing_final_radiologist_id,'00000000-0000-0000-0000-000000000000')='00000000-0000-0000-0000-000000000000')
						begin
							update study_hdr_archive
							set final_radiologist_pacs  = isnull(@final_radiologist_pacs,''),
								final_radiologist_id    = isnull(@final_radiologist_id,'00000000-0000-0000-0000-000000000000'),
								rpt_approve_date        = @rpt_approve_date
							where id      = @study_id
							and study_uid = @study_uid

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to update final radiologist for Study UID ' + @study_uid
									return 0
								end
							
						end
				end

			set @return_type=2
			set @error_msg=''
			commit transaction

			 
		end
	else
		begin
			 set @return_type=2
			 set @error_msg=''
		end

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
				where (upper(name) = upper(@institution_name) or upper(code) = upper(@institution_name)) 
				and is_active = 'Y') =0
				begin 
					if(select count(inl.institution_id)
						from institution_alt_name_link inl
						inner join institutions i on i.id= inl.institution_id
						where (upper(inl.alternate_name) = upper(@institution_name) or upper(code) = upper(@institution_name)) 
						and i.is_active='Y') =0
							begin
								set @institution_id = newid()
								insert into institutions(id,name,created_by,date_created) values (@institution_id,@institution_name,'00000000-0000-0000-0000-000000000000',getdate())

								if(@@rowcount=0)
									begin
										rollback transaction
										select @return_type=0,@error_msg='Failed to create institution for Study UID ' + @study_uid
										return 0
									end
							end
					else
						    begin
									select @institution_id=isnull((select top 1 institution_id 
											                       from institution_alt_name_link inl
																   inner join institutions i on i.id= inl.institution_id
																   where (upper(inl.alternate_name) = upper(@institution_name) or upper(code) = upper(@institution_name)) 
																   and i.is_active='Y'),'00000000-0000-0000-0000-000000000000')
						    end	
					
					
					
					if(rtrim(ltrim(@existing_institution)) <> rtrim(ltrim(@institution_name)) or rtrim(ltrim(@existing_institution_code)) <> rtrim(ltrim(@institution_name)))
						begin
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
							else if(select count(id) from study_hdr_archive where id = @study_id) >0
								begin
									update study_hdr_archive set institution_id = @institution_id where id=@study_id

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
					select @institution_id=isnull((select top 1 id from institutions
													where (upper(name) = upper(@institution_name) or upper(code) = upper(@institution_name))
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
					else if(select count(id) from study_hdr_archive where id = @study_id) >0
						begin
							update study_hdr_archive set institution_id = @institution_id where id=@study_id

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
			set @category_id =3
		end
	else
		begin
			set @consult_applied='N'
			if(select count(id) from study_hdr where id = @study_id) >0
				begin
					if(select count(st.study_type_id) 
					   from study_hdr_study_types st
					   inner join modality_study_types mst on mst.id= st.study_type_id
					   where st.study_hdr_id = @study_id
					   and mst.category_id=2) >0
						begin
							set @category_id = 2 
						end
					else
						begin
							set @category_id = 1
						end
				end
			
		end

	begin transaction

	if(select count(id) from study_hdr where id = @study_id) >0
		begin
			update study_hdr 
			set img_count             = @image_count,
				object_count_pacs     = @object_count,
				service_codes         = isnull(@service_codes,''),
				consult_applied       = @consult_applied,
				category_id           = isnull(@category_id,0),
				--rpt_approve_date = isnull(@rpt_approve_date,'01jan1900'),
				rpt_record_date  = isnull(@rpt_record_date,'01jan1900')
			where id=@study_id
		end
	else if(select count(id) from study_hdr_archive where id = @study_id) >0
		begin
			update study_hdr_archive
			set img_count        = @image_count,
				object_count     = @object_count,
				service_codes    = isnull(@service_codes,''),
				consult_applied  = @consult_applied,
				--rpt_approve_date = isnull(@rpt_approve_date,'01jan1900'),
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
								--radiologist_pacs       = isnull(@radiologist,''),
								--radiologist_id         = isnull(@radiologist_id,'00000000-0000-0000-0000-000000000000'),
								service_codes          = isnull(@service_codes,''),
								consult_applied        = @consult_applied,
								--rpt_approve_date       = isnull(@rpt_approve_date,'01jan1900'),
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

							if(@status_id=60)
								begin
									select @priority_id = isnull(priority_id,0) from study_hdr where id=@study_id
									if(@priority_id >0)
										begin
											select @transcription_finishing_time_mins = transcription_finishing_time_mins from sys_priority where priority_id=@priority_id
											select @transcription_finishing_datetime = dateadd(mi,@transcription_finishing_time_mins,getdate())

											update study_hdr set transcription_finishing_datetime=@transcription_finishing_datetime where id=@study_id

											if(@@rowcount=0)
												begin
													rollback transaction
													select @return_type=0,@error_msg='Failed to update transcription finishing date and time for Study UID ' + @study_uid
													return 0
												end
										end
								end



							 commit transaction
							 set @return_type=1
							 set @error_msg='Status of for Study UID ' + @study_uid + ' updated successfully'
						end
					--else if(@status_id < @existing_status_id)
					--	begin
					--		if((@status_id=0 or @status_id=20) and @existing_status_id=50)
					--			begin
					--				update study_hdr
					--				set pacs_wb ='Y'
					--				where study_uid = @study_uid
					--				and id = @study_id

					--				if(@@rowcount=0)
					--					begin
					--						rollback transaction
					--						select @return_type=0,@error_msg='Failed to update status for Study UID ' + @study_uid
					--						return 0
					--					end
					--			end
					--	end
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
					set 
						--radiologist_pacs  = isnull(@radiologist,''),
						service_codes     = isnull(@service_codes,''),
						consult_applied   = @consult_applied,
						--rpt_approve_date  = isnull(@rpt_approve_date,'01jan1900'),
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


	--update additional field
	
	if(isnull(@additional_field,'')<>'')
		begin
			begin transaction
			
			if(@additional_field = 'PMAL') 
				begin
					set @addl_field_db ='reason_accepted'
					set @addl_field_alias ='History / Reason For Study'
					set @addl_field_db_type='C'
				end
		
			if(select count(id) from study_hdr where id = @study_id) >0
				begin
					set @addl_field_tbl_name = 'study_hdr'
				end
			else if(select count(id) from study_hdr_archive where id = @study_id) >0
				begin
					set @addl_field_tbl_name = 'study_hdr_archive'
				end

			if(@addl_field_db_type='C')
				begin
					--set @additional_field_value=replace(@additional_field_value,char(39),'')
					set @strSQL ='update ' + @addl_field_tbl_name + ' set ' + @addl_field_db + ' = ''' + @additional_field_value + ''' where id=''' + convert(varchar(36),@study_id) +''''
				end
			else if(@addl_field_db_type='N')
				begin
					set @strSQL ='update ' + @addl_field_tbl_name + ' set ' + @addl_field_db + ' = ' + @additional_field_value + ' where id=''' + convert(varchar(36),@study_id) +''''
				end
			
			--set @error_msg=''
			--set @return_type=0

			--exec scheduler_log_save
			--	 @is_error    = 0,
			--	 @service_id  = 3,
			--	 @log_message = @strSQL,
			--	 @error_msg   = @error_msg,
			--	 @return_type = @return_type	


			exec(@strSQL)

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to update ' + @addl_field_alias + ' for Study UID ' + @study_uid
					return 0
				end
			else
				begin
						set @return_type=2
						set @error_msg=''
						commit transaction
				end
		end


	set nocount off
	return 1

end


GO
