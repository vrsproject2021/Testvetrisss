USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_case_study_write_back_status_change]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_case_study_write_back_status_change]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_case_study_write_back_status_change]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_case_study_status_update : update
                  case study status
** Created By   : Pavel Guha
** Created On   : 15/04/2019
*******************************************************/
CREATE procedure [dbo].[scheduler_case_study_write_back_status_change]
    @study_id uniqueidentifier,
	@study_uid nvarchar(100),
	@status_id int,
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	set nocount on

	declare @existing_status_id int,
	        @patient_fname nvarchar(80),
			@patient_lname nvarchar(80),
	        @patient_sex nvarchar(10),
			@institution_id uniqueidentifier,
			@institution_name nvarchar(100),
			@modality nvarchar(30),
	        @def_asn_radiologist_id uniqueidentifier,
			@def_asn_radiologist_name nvarchar(200),
			@activity_text nvarchar(max),
			@SUPPSYMRGMAILID nvarchar(200),
			@MAILSVRUSRCODE nvarchar(200),
			@MAILSVRUSRPWD nvarchar(200),
			@SMSSENDERNO nvarchar(200),
			@SUPPSYMRGSMS nvarchar(200),
			@rc int,
			@ctr int,
			@study_hdr_id uniqueidentifier,
			@suid nvarchar(100),
			@mdl nvarchar(30),
			@pname nvarchar(180),
			@inst_name nvarchar(100),
			@email_subject nvarchar(250),
			@email_text nvarchar(max),
			@sms_text nvarchar(max),
			@recipient_no nvarchar(100)

	

	begin transaction

	if(select count(id) from study_hdr where id=@study_id and study_uid = @study_uid)> 0
		begin
			select @existing_status_id = sh.study_status_pacs,
			       @patient_fname      = isnull(sh.patient_fname,''),
				   @patient_lname      = isnull(sh.patient_lname,''),
				   @patient_sex        = isnull(sh.patient_sex,''),
				   @institution_id     = isnull(sh.institution_id,'00000000-0000-0000-0000-000000000000'),
				   @institution_name   = isnull(i.name,''),
				   @modality           = isnull(m.name,'')
			from study_hdr sh
			left outer join institutions i on i.id = sh.institution_id
			left outer join modality m on m.id = sh.modality_id
			where sh.id = @study_id
			and sh.study_uid = @study_uid

			update study_hdr
			set pacs_wb ='N',
				study_status_pacs = @existing_status_id
			where study_uid = @study_uid
			and id = @study_id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to change the write back status to N for Study UID ' + @study_uid
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

			if(@status_id=50)
				begin
					 if(select count(id)		
						from study_hdr 
						where ((upper(isnull(patient_fname,'')) + ' ' + upper(isnull(patient_lname,'')) = upper(isnull(@patient_fname,'')) + ' ' + upper(isnull(@patient_lname,'')))
								or (upper(isnull(patient_lname,'')) + ' ' + upper(isnull(patient_fname,'')) = upper(isnull(@patient_lname,'')) + ' ' + upper(isnull(@patient_fname,''))))
						and isnull(patient_sex,'')  = @patient_sex
						and institution_id          = @institution_id
						and study_status_pacs       = 50
						and isnull(merge_status,'N')<>'N'
						and study_uid <> @study_uid)>0
							begin

								select @SUPPSYMRGMAILID = data_type_string from general_settings where control_code='SUPPSYMRGMAILID'
								select @MAILSVRUSRCODE = data_type_string from general_settings where control_code='MAILSVRUSRCODE'
								select @MAILSVRUSRPWD = data_type_string from general_settings where control_code='MAILSVRUSRPWD'
								select @SMSSENDERNO = data_type_string from general_settings where control_code='SMSSENDERNO'
								select @SUPPSYMRGSMS = data_type_string from general_settings where control_code='SUPPSYMRGSMS'

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
								set @email_subject = 'Merging of studies might be required in PACS for the patient ' +  isnull(@patient_fname,'') + ' ' + isnull(@patient_lname,'')								
								set @email_text    = 'Following study(ies) of the patient ' + isnull(@patient_fname,'') + ' ' + isnull(@patient_lname,'')	 + ' [Study UID : ' + @study_uid + '] qualify(ies) for merging in PACS'  

								while(@ctr<=@rc)
									begin
										select @study_hdr_id = study_id,
												@suid         = study_uid,
												@mdl          = modality,
												@inst_name    = institution,
												@pname        = patient_name
										from #tmpStudy 
										where rec_id = @ctr
												
										set @email_text    = @email_text + '\n\n'
										set @email_text    = @email_text + 'Study UID              : ' + @suid + '\n'
										set @email_text    = @email_text + 'Institution            : ' + @inst_name+'\n'
										set @email_text    = @email_text + 'Modality               : ' + @mdl + '\n'

										update study_hdr
										set status_last_updated_on =getdate()
										where study_uid = @suid
										and id = @study_hdr_id

										set @ctr = @ctr + 1
									end
										 

								set @email_text    = @email_text +'\n\nThis is an automated message from VETRIS.Please do not reply to the message.\n'

								insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,
												        email_subject,email_text,
														study_hdr_id,study_uid,email_type,sender_email_address,sender_email_password)
												values(newid(),getdate(),@SUPPSYMRGMAILID,'RAD Support',
														@email_subject,@email_text,
														@study_hdr_id,@suid,'MRGREQ',@MAILSVRUSRCODE,@MAILSVRUSRPWD)

								if(@@rowcount=0)
									begin
										rollback transaction
										select @return_type=0,@error_msg='Failed to generate merging required mail of Study UID :' + @study_uid
										return 0
									end

								drop table #tmpStudy

								if(rtrim(ltrim(@SMSSENDERNO))<>'' and rtrim(ltrim(@SUPPSYMRGSMS))<>'')
									begin

										set @sms_text ='Merge req. for ' + upper(isnull(@patient_fname,'')) + ' ' + upper(isnull(@patient_lname,'')) + ' of ' + @institution_name

										if(charindex(';', rtrim(ltrim(@SUPPSYMRGSMS))))>0
											begin
												
												select * into #tmpSMS
												from
												(select id,data from dbo.Split(rtrim(ltrim(@SUPPSYMRGSMS)),';')) t
												
												insert into vrslogdb..sms_log(sms_log_id,sms_log_datetime,sender_no,recipient_no,recipient_name,sms_text,study_hdr_id,study_uid,sequence_no)
															 (select newid(),getdate(),@SMSSENDERNO,data,'',@sms_text,@study_id,@study_uid,1
															  from #tmpSMS)

												if(@@rowcount=0)
													begin
														rollback transaction
														select @return_type=0,@error_msg='Failed to generate merging required sms of Study UID :' + @study_uid
														return 0
													end

												drop table #tmpSMS
											end
										else
											begin
												insert into vrslogdb..sms_log(sms_log_id,sms_log_datetime,sender_no,recipient_no,recipient_name,sms_text,study_hdr_id,study_uid,sequence_no)
															 (select newid(),getdate(),@SMSSENDERNO,@SUPPSYMRGSMS,'',@sms_text,@study_id,@study_uid,1)

												if(@@rowcount=0)
													begin
														rollback transaction
														select @return_type=0,@error_msg='Failed to generate merging required sms of Study UID :' + @study_uid
														return 0
													end
											end

										
									end
									
							end
				end
		end
	else if(select count(id) from study_hdr_archive where id=@study_id and study_uid = @study_uid)> 0
		begin

			update study_hdr_archive
			set pacs_wb ='N'
			where study_uid = @study_uid
			and id = @study_id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to change the write back status to N for Study UID ' + @study_uid
					return 0
				end

			--insert into sys_case_study_status_log(study_id,study_uid,status_id_from,status_id_to,date_updated)
			--				values(@study_id,@study_uid,@existing_status_id,@status_id,getdate())

			--if(@@rowcount=0)
			--	begin
			--		rollback transaction
			--		select @return_type=0,@error_msg='Failed to update status log for Study UID ' + @study_uid
			--		return 0
			--	end
		end

	commit transaction
	select @error_msg='Write back status of Study UID ' + @study_uid + ' changed to N',@return_type=1
	set nocount off
	return 1

end


GO
