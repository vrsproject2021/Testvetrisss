USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_downloaded_listener_file_details_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_downloaded_listener_file_details_save]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_downloaded_listener_file_details_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_downloaded_listener_file_details_save : 
                  save file downloaded from listener
** Created By   : Pavel Guha
** Created On   : 14/10/2020
*******************************************************/
CREATE procedure [dbo].[scheduler_downloaded_listener_file_details_save]
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
			@status_id int,
			@re_register nchar(1),
			@track_by nchar(1)

	-- Temporarirly Added
	declare @email_subject nvarchar(250),
		    @email_text varchar(8000),
		    @SUPPMAILID nvarchar(200),
			@SENDLFTOPACS nvarchar(200),
			@SUPPFWDSYNMAILID nvarchar(200),
			@MAILSVRUSRCODE nvarchar(200),
			@MAILSVRUSRPWD nvarchar(200),
			@email_log_id uniqueidentifier,
			@approve_for_pacs nchar(1),
			@approved_by uniqueidentifier,
			@date_approved datetime,
			@diff int


	select @SUPPMAILID = data_type_string
	from general_settings
	where control_code='SUPPMAILID'

	select @SUPPFWDSYNMAILID = data_type_string
	from general_settings
	where control_code='SUPPFWDSYNMAILID'

	select @SENDLFTOPACS = data_type_string
	from general_settings
	where control_code='SENDLFTOPACS'

   select @MAILSVRUSRCODE = data_type_string
   from general_settings
   where control_code ='MAILSVRUSRCODE'

   select @MAILSVRUSRPWD = data_type_string
   from general_settings
   where control_code ='MAILSVRUSRPWD'

	if(@SENDLFTOPACS='N') set @send_to_pacs='Y'
	else set @send_to_pacs='N'
	-- Temporarirly Added

	set @re_register='N'
	select @track_by = track_by from modality where id=@modality_id

	begin transaction

	

	set @institution_id = isnull((select id 
	                        from institutions
							where code = @institution_code),'00000000-0000-0000-0000-000000000000')

	set @institution_name = isnull((select name 
									from institutions
									where id = @institution_id),'')

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
	--else  if(select count(study_uid) from scheduler_file_downloads_dtls where study_uid=@study_uid  and series_uid=@series_uid and instance_no=@sop_instance_uid and sent_to_pacs='N')>0
	--	begin
	--		set @re_register='Y'
	--	end
	else
		begin
			rollback transaction
			select @return_type=0,@error_msg='' + @study_uid,@delete_file='Y'
			return 0
		end
	

	if(@re_register='N')
		begin
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
								select @return_type=0,@error_msg='Failed to create file details of Study UID ' + @study_uid
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
										select @return_type=2,@error_msg='Failed to create file details of Study UID ' + @study_uid
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
						--and institution_id = @institution_id

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
						select @id                = id,
							   @object_count_pacs = object_count_pacs,
							   @status_id         = study_status_pacs
						from study_hdr where study_uid=@study_uid

						select @file_count = (select count(file_name)
											 from scheduler_file_downloads_dtls
											 where id = @id)  
						select @file_count = @file_count + (select count(file_name)
														   from scheduler_img_file_downloads_grouped_dtls
														   where id = @id)
						--and file_name like '%_S1DXXX%'
			
						update study_hdr
						set object_count = @file_count
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
						select @id                = id,
							   @object_count_pacs = object_count_pacs,
							   @status_id         = study_status_pacs
						from study_hdr_archive where study_uid=@study_uid
			
						select @file_count = (select count(file_name)
											 from scheduler_file_downloads_dtls
											 where id = @id)  
						select @file_count = @file_count + (select count(file_name)
														   from scheduler_img_file_downloads_grouped_dtls
														   where id = @id)
						--and file_name like '%_S1DXXX%'
			
						update study_hdr_archive
						set object_count = @file_count
						where id = @id

						if(@@rowcount=0)
							begin
								rollback transaction
								select @return_type=0,@error_msg='Failed to update file count of Study UID ' + @study_uid
								return 0
							end
					end

				set @diff = @object_count_pacs - @file_count

				if(((@file_count>0) and (@file_count<=@object_count_pacs) and ((@file_count - @object_count_pacs)>= -3)) or (@file_count >= @object_count_pacs))
					begin
						if(@diff>=0 and @diff<=2)
							begin
								select @error_code='',@return_status=0
								set @activity_text='DICOM files - sync completed, count variance ' + convert(varchar,@object_count_pacs - @file_count)
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
					end

				if((@status_id>0 )and (@diff>3))
					begin
						if(Select count(email_log_id) from vrslogdb..email_log where study_hdr_id = @id and study_uid=@study_uid and email_type='SYFWDND')=0
							begin
								exec notification_study_file_sync_pending_create
									@id = @id,
									@error_msg = @error_msg output,
									@return_type = @return_type output

								if(@return_type=0)
									begin
										rollback transaction
									end
							end
					end
		end
	else
		begin
			update scheduler_file_downloads_dtls
			set sent_to_pacs=@send_to_pacs,
			    date_sent   = getdate()
			where study_uid=@study_uid  
			and series_uid=@series_uid 
			and instance_no=@sop_instance_uid 
			and sent_to_pacs='N'
		end

	
	--if((@status_id>0) and ((@file_count - @object_count_pacs)< -3))
	

	---- Temporarirly Added
	--if(@institution_id='9305A40D-706B-47B9-8E2F-A4422E462053')
	--	begin
	--		/******************************************************************************************************************
	--				Email Format
	--			******************************************************************************************************************/
	--			set @email_subject = 'DICOM files uploaded from institution :' + @institution_name

	--			set @email_text    = 'File Details :- \n\n'
	--			set @email_text    = @email_text + ' File Name          : ' + @file_name + '\n'
	--			set @email_text    = @email_text + ' Study UID          : ' + @study_uid +'\n'
	--			set @email_text    = @email_text + ' Institution Code   : ' + @institution_code + '\n'
	--			set @email_text    = @email_text + ' Institution Name   : ' + @institution_name + '\n'
	--			set @email_text    = @email_text + '\n\n'
	--			set @email_text    = @email_text + 'Please check whether it is available in both eRAD and VETRIS\n'
	--			set @email_text    = @email_text + 'If not, please check the corrupted tags of the file in the VETRIS, path is :\n'
	--			set @email_text    = @email_text + 'D:\\VRSApp\\PACS_ARCHIVE\\' + @institution_code + '_' + @institution_name + '_' + @study_uid + '\n'
	--			set @email_text    = @email_text + 'ALSO LOOK FOR DATE OF BIRTH TAG\n'
	--			set @email_text    = @email_text + '\n\n'
	--			set @email_text    = @email_text +'This is an automated message from VETRIS.Please do not reply to the message.\n'

	--		set @email_log_id=newid()	
	--		insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,email_subject,email_text,
	--                              study_uid,email_type,file_name)
	--			            values(@email_log_id,getdate(),@SUPPMAILID,'RAD Support',@email_subject,@email_text,
	--			                   @study_uid,'TEMP',@file_name)

	--		if(@@rowcount = 0)
	--			begin
	--				rollback transaction
	--				set @error_msg='Failed to create notification for file received for ' + isnull(@institution_name,'')
	--				set @return_type=0
	--				return 0
	--			end
	--	end
	---- Temporarirly Added


    commit transaction
	set @return_type=1
	set @error_msg=''

	exec scheduler_object_count_check

	set nocount off
	return 1

end


GO
