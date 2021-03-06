USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_report_addendum_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_report_addendum_save]
GO
/****** Object:  StoredProcedure [dbo].[case_list_report_addendum_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_report_addendum_save : save
                  case report
** Created By   : Pavel Guha
** Created On   : 24/04/2020
*******************************************************/
/*
exec case_list_report_addendum_save
'70e5f2c4-343c-4aa5-b6d2-812af6f802e7',
'Study: Thorax, two images
Radiographic findings:
1. Cardiac silhouette dorsally displaces the trachea and is convex in left atrial contour. (VHS - 12.2)
2. Normal pulmonary vasculature and parenchyma
3. The caudoventral margins of the hepatic silhouette are mildly elongated and convex in contour 
4. Narrowing of the cervical trachea by a dorsal soft tissue opaque membrane
5. No other abnormalities are identified
Conclusions:
1. Compensated left sided cardiac enlargement, consistent with myxomatous mitral valvular disease.  Echocardiography and EKG may be useful in further evaluation of cardiac function.
2. Metabolic, inflammatory or less likely infiltrative hepatopathy
3. Redundant tracheal membrane',
'Study: Thorax, two images<br/>Radiographic findings:<br/>1. Cardiac silhouette dorsally displaces the trachea and is convex in left atrial contour. (VHS - 12.2)<br/>2. Normal pulmonary vasculature and parenchyma<br/>3. The caudoventral margins of the hepatic silhouette are mildly elongated and convex in contour <br/>4. Narrowing of the cervical trachea by a dorsal soft tissue opaque membrane<br/>5. No other abnormalities are identified<br/>Conclusions:<br/>1. Compensated left sided cardiac enlargement, consistent with myxomatous mitral valvular disease. &nbsp;Echocardiography and EKG may be useful in further evaluation of cardiac function.<br/>2. Metabolic, inflammatory or less likely infiltrative hepatopathy<br/>3. Redundant tracheal membrane',
4,'Repeat images collimated down to area of concern may be helpful for further evaluation.','Testing archive addendum','Testing archive addendum','Y',
'21199081-fdc2-4416-8a1b-a96e217f00c5',76,'240fa805-2d70-4688-b90c-07eb8c69496c','','',0
*/
CREATE procedure [dbo].[case_list_report_addendum_save]
	@id uniqueidentifier ='00000000-0000-0000-0000-000000000000',
	@report_text ntext=null,
	@report_text_html ntext=null,
	@disclaimer_reason_id int =0,
	@disclaimer_text ntext = null,
	@addendum_text ntext=null,
	@addendum_text_html ntext=null,
	@mark_to_teach nchar(1)='N',
	@updated_by uniqueidentifier,
	@menu_id int,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@user_name nvarchar(130)='' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	
	set nocount on

	declare @study_uid nvarchar(100),
	        @addendum_srl int,
			@report_id uniqueidentifier,
			@institution_id uniqueidentifier,
			@physician_id uniqueidentifier,
			@activity_text nvarchar(max),
			@final_rpt_release_datetime datetime,
			@strSQL varchar(max),
	        @db_name nvarchar(50)

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
			return 0
		end

	begin transaction

	if(select count(id) from study_hdr where id = @id)>0
		begin
			select @study_uid                  = study_uid    
			from study_hdr 
			where id=@id

			if(select count(study_hdr_id) from study_hdr_final_reports where study_hdr_id=@id)>0
				begin
					update study_hdr_final_reports
					set report_text          = @report_text,
						report_text_html     = @report_text_html,
						disclaimer_reason_id = @disclaimer_reason_id,
						disclaimer_text      = @disclaimer_text,
						updated_by           = @updated_by,
						date_updated         = getdate()
					where study_hdr_id=@id
				end
			else
				begin
					insert into study_hdr_final_reports(report_id,study_hdr_id,study_uid,report_text,report_text_html,pacs_wb,disclaimer_reason_id,disclaimer_text,rating_reason_id,created_by,date_created)
									             values(newid(),@id,@study_uid,@report_text,@report_text_html,'Y',@disclaimer_reason_id,@disclaimer_text,'00000000-0000-0000-0000-000000000000',@updated_by,getdate())
				end

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status=0,@error_code='035'
					return 0
				end

			if(isnull(convert(nvarchar(max),@addendum_text),'')<> '')
				begin
					select @addendum_srl = max(addendum_srl)
					from study_report_addendums
					where study_hdr_id=@id

					set @addendum_srl = isnull(@addendum_srl,0) + 1

					select @report_id = report_id
					from study_hdr_final_reports
					where study_hdr_id = @id

					insert into study_report_addendums(report_id,addendum_srl,study_hdr_id,study_uid,
													   addendum_text,addendum_text_html,pacs_wb,
													   created_by,date_created)
												values(@report_id,@addendum_srl,@id,@study_uid,
													   @addendum_text,@addendum_text_html,'Y',
													   @updated_by,getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='035'
							return 0
						end
				end

			update study_hdr
			set mark_to_teach = @mark_to_teach
			where id=@id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status=0,@error_code='035'
					return 0
				end

			exec common_study_mis_data_update
					@id            = @id,
					@updated_by    = @updated_by,
					@error_code    = @error_code output,
					@return_status = @return_status output

			if(@return_status=0)
				begin
					rollback transaction
					return 0
				end
		end
	else if(select count(id) from study_hdr_archive where id = @id)>0
		begin
			select @study_uid = study_uid from study_hdr_archive where id=@id

			if(select count(study_hdr_id) from study_hdr_final_reports_archive where study_hdr_id=@id)>0
				begin
					update study_hdr_final_reports_archive
					set report_text          = @report_text,
						report_text_html     = @report_text_html,
						disclaimer_reason_id = @disclaimer_reason_id,
						disclaimer_text      = @disclaimer_text,
						updated_by           = @updated_by,
						date_updated         = getdate()
					where study_hdr_id=@id
				end
            else
				begin
					insert into study_hdr_final_reports_archive(report_id,study_hdr_id,study_uid,report_text,report_text_html,pacs_wb,disclaimer_reason_id,disclaimer_text,rating_reason_id,created_by,date_created)
									                     values(newid(),@id,@study_uid,@report_text,@report_text_html,'Y',@disclaimer_reason_id,@disclaimer_text,'00000000-0000-0000-0000-000000000000',@updated_by,getdate())
				end

		    if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status=0,@error_code='035'
					return 0
				end

			if(isnull(convert(nvarchar(max),@addendum_text),'')<> '')
				begin
					select @addendum_srl = max(addendum_srl)
					from study_report_addendums_archive
					where study_hdr_id=@id

					set @addendum_srl = isnull(@addendum_srl,0) + 1

					select @report_id = report_id
					from study_hdr_final_reports_archive
					where study_hdr_id = @id

					insert into study_report_addendums_archive(report_id,addendum_srl,study_hdr_id,study_uid,
															   addendum_text,addendum_text_html,pacs_wb,
															   created_by,date_created,archived_by, date_archived)
													   values(@report_id,@addendum_srl,@id,@study_uid,
															  @addendum_text,@addendum_text_html,'Y',
															  @updated_by,getdate(),@updated_by,getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='035'
							return 0
						end
				end

			update study_hdr_archive
			set mark_to_teach = @mark_to_teach
			where id=@id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status=0,@error_code='035'
					return 0
				end

			exec common_study_mis_data_update
					@id            = @id,
					@updated_by    = @updated_by,
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
			set @db_name=''
			exec common_get_study_database
				@id      = @id,
				@db_name = @db_name output
				
			if(@db_name <>'vrsdb')
				begin
					create table #tmpUID(study_uid nvarchar(100))
					set @strSQL ='insert into #tmpUID(study_uid)(select study_uid from ' + @db_name + '..study_hdr_archive where id=''' + convert(varchar(36),@id) + ''')'
					exec(@strSQL)
					select @study_uid = study_uid from #tmpUID
					drop table #tmpUID

					create table #tmpRptTxt(report_text ntext,report_text_html ntext,disclaimer_text ntext)
					insert into #tmpRptTxt(report_text,report_text_html,disclaimer_text)values(@report_text,@report_text_html,@disclaimer_text)
					set @strSQL = 'update ' + @db_name + '..study_hdr_final_reports_archive '
					set @strSQL = @strSQL + 'set report_text      = (select report_text from #tmpRptTxt),'
					set @strSQL = @strSQL + 'report_text_html     = (select report_text_html from #tmpRptTxt),'
					set @strSQL = @strSQL + 'disclaimer_reason_id = ' + convert(varchar,@disclaimer_reason_id) + ','
					set @strSQL = @strSQL + 'disclaimer_text      = (select disclaimer_text from #tmpRptTxt),'
					set @strSQL = @strSQL + 'updated_by           = ''' + convert(varchar(36),@updated_by) + ''','
					set @strSQL = @strSQL + 'date_updated         = getdate() '
					set @strSQL = @strSQL + 'where study_hdr_id=''' + convert(varchar(36),@id) + ''' '

					exec(@strSQL)
					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='035'
							drop table #tmpRptTxt
							return 0
						end
                     drop table #tmpRptTxt

					if(isnull(convert(nvarchar(max),@addendum_text),'')<> '')
						begin
							create table #tmpSrl(srl int)
						    set @strSQL ='insert into #tmpSrl(srl)(select max(addendum_srl) from ' + @db_name + '..study_report_addendums_archive where study_hdr_id=''' + convert(varchar(36),@id) + ''')'
						    exec(@strSQL)
						    select @addendum_srl = srl from #tmpSrl
						    drop table #tmpSrl

							set @addendum_srl = isnull(@addendum_srl,0) + 1

							create table #tmpRptID(report_id uniqueidentifier)
						    set @strSQL ='insert into #tmpRptID(report_id)(select report_id from ' + @db_name + '..study_hdr_final_reports_archive where study_hdr_id=''' + convert(varchar(36),@id) + ''')'
						    exec(@strSQL)
						    select @report_id = report_id from #tmpRptID
						    drop table #tmpRptID

							create table #tmpAddnTxt(addendum_text ntext,addendum_text_html ntext)
							insert into #tmpAddnTxt(addendum_text,addendum_text_html)values(@addendum_text,@addendum_text_html)
							set @strSQL ='insert into ' + @db_name + '..study_report_addendums_archive(report_id,addendum_srl,study_hdr_id,study_uid,'
							set @strSQL = @strSQL + 'addendum_text,addendum_text_html,pacs_wb,'
							set @strSQL = @strSQL + 'created_by,date_created,archived_by, date_archived)'
							set @strSQL = @strSQL + 'values(''' + convert(varchar(36),@report_id) + ''',' + convert(varchar,@addendum_srl) + ',''' + convert(varchar(36),@id) + ''',''' + @study_uid + ''','
							set @strSQL = @strSQL + '(select addendum_text from #tmpAddnTxt),(select addendum_text_html from #tmpAddnTxt),''Y'','
							set @strSQL = @strSQL + '''' + convert(varchar(36),@updated_by) + ''',getdate(),''' + convert(varchar(36),@updated_by) + ''',getdate())'

							exec(@strSQL)
							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_status=0,@error_code='035'
									drop table #tmpAddnTxt
									return 0
								end
							drop table #tmpAddnTxt
						end

					set @strSQL ='update ' + @db_name + '..study_hdr_archive '
					set @strSQL =@strSQL + 'set mark_to_teach = ''' + @mark_to_teach + ''' '
					set @strSQL =@strSQL + 'where id=''' + convert(varchar(36),@id) + ''' '

					exec(@strSQL)
					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='035'
							return 0
						end
				end
		end

	
	if(isnull(convert(nvarchar(max),@addendum_text),'')<> '')
		set @activity_text =  'Addendum #' + convert(varchar,@addendum_srl) + ' added'
	else
		set @activity_text =  'Report modified after finalisation' 

	exec common_study_user_activity_trail_save
		@study_hdr_id  = @id,
		@study_uid     = @study_uid,
		@menu_id       = @menu_id,
		@activity_text = @activity_text,
		@session_id    = @session_id,
		@activity_by   = @updated_by,
		@error_code    = @error_code output,
		@return_status = @return_status output

	if(@return_status=0)
		begin
			rollback transaction
			return 0
		end


	
	/*************************GENERATE NOTIFICATION*****************************/
	declare @recipient_address nvarchar(500),
			@recipient_name nvarchar(200),
			@recipient_mobile nvarchar(500),
			@recipient_fax_no nvarchar(30),
			@custom_report nchar(1),
			@fax_report nchar(1),
			@fax_file_name nvarchar(max),
			@patient_name nvarchar(200),
			@VRSPACSLINKURL nvarchar(200),
			@study_types varchar(max),
			@email_subject varchar(250),
			@email_text nvarchar(max),
			@sms_text nvarchar(max),
			@species_id int,
			@breed_id uniqueidentifier,
			@modality_id int,
			@RPTMAILUSRACCT nvarchar(100),
			@RPTMAILUSRPWD nvarchar(100),
			@SMSSENDERNO nvarchar(200),
			@mobile_no nvarchar(20),
			@FAXENABLE nchar(1),
			@release nchar(1),
			@counter int,
			@rowcount int

	select @VRSPACSLINKURL = data_type_string
	from general_settings
	where control_code ='VRSPACSLINKURL'

	select @RPTMAILUSRACCT = data_type_string
	from general_settings
	where control_code ='RPTMAILUSRACCT'

	select @RPTMAILUSRPWD = data_type_string
	from general_settings
	where control_code ='RPTMAILUSRPWD'

	select @FAXENABLE = data_type_string
	from general_settings
	where control_code ='FAXENABLE'

    set @VRSPACSLINKURL = @VRSPACSLINKURL + convert(varchar(36),@id) + '&vw=rpt&fmt=2'
	set @study_types=''

	if(select count(id) from study_hdr where id = @id)>0
		begin
			select  @patient_name               = patient_name,
					@species_id                 = species_id,
					@breed_id                   = breed_id,
					@modality_id                = modality_id,
					@institution_id             = institution_id,
					@physician_id               = physician_id,
					@final_rpt_release_datetime = isnull(final_rpt_release_datetime,'01jan1900')
			from study_hdr
			where id= @id
			and study_uid = @study_uid

			select @study_types = @study_types +  convert(varchar,st.name) + ','
			from study_hdr_study_types shst
			inner join  modality_study_types st on st.id= shst.study_type_id
			where shst.study_hdr_id=@id 

			if(@final_rpt_release_datetime<getdate())
				begin
					set @release='Y'
				end
			else
				begin
					set @release='N'
				end
		end
	else if(select count(id) from study_hdr_archive where id = @id)>0
		begin
			select @patient_name   = patient_name,
			       @species_id     = species_id,
				   @breed_id       = breed_id,
				   @modality_id    = modality_id,
				   @institution_id = institution_id,
				   @physician_id   = physician_id
			from study_hdr_archive
			where id= @id
			and study_uid = @study_uid

			select @study_types = @study_types +  convert(varchar,st.name) + ','
			from study_hdr_study_types_archive shst
			inner join  modality_study_types st on st.id= shst.study_type_id
			where shst.study_hdr_id=@id 

			set @release='Y'
		end
	else
		begin
			create table #tmp(patient_name nvarchar(200),institution_id uniqueidentifier,physician_id uniqueidentifier,species_id int,breed_id uniqueidentifier, modality_id int)
			set @strSQL ='insert into #tmp(patient_name,institution_id,physician_id,species_id,breed_id,modality_id)(select patient_name,institution_id,physician_id,species_id,breed_id,modality_id from ' + @db_name + '..study_hdr_archive where id=''' + convert(varchar(36),@id) + ''')'
			exec(@strSQL)

			select @patient_name   = patient_name,
			       @species_id     = species_id,
				   @breed_id       = breed_id,
				   @modality_id    = modality_id,
				   @institution_id = institution_id,
				   @physician_id   = physician_id
			from #tmp

			
			drop table #tmp
			
			create table #tmpST(name nvarchar(100))
			set @strSQL ='insert into #tmpST(name)'
			set @strSQL =@strSQL + '(select st.name from ' + @db_name + '..study_hdr_study_types_archive shst '
			set @strSQL =@strSQL + 'inner join  modality_study_types st on st.id= shst.study_type_id where shst.study_hdr_id=''' + convert(varchar(36),@id) + ''')'
			exec(@strSQL)
			

			select @study_types = @study_types +  convert(varchar,name) + ','
			from #tmpST 
			drop table #tmpST

			set @release='Y'

		end


	if(isnull(@study_types,'')<>'') select @study_types = substring(@study_types,1,len(@study_types)-1)

	select  @recipient_address = isnull(physician_email,''),
			@recipient_name    = isnull(physician_name,''),
			@recipient_mobile  = isnull(physician_mobile,'')
	from institution_physician_link
	where institution_id = @institution_id
	and physician_id = @physician_id

	if(isnull(@recipient_address,'') <> '')
		begin
			if(isnull(convert(nvarchar(max),@addendum_text),'')<> '')
				begin
					set @email_subject = + 'Addendum added to final report for ' + isnull(@patient_name,'')
					set @email_text    = 'Addendum added to final report for :- \n\n'
				end
			else
				begin
					set @email_subject = + 'Final report for ' + isnull(@patient_name,'') + ' modified'
					set @email_text    = 'Final report modified for :- \n\n'
				end
						
			
			set @email_text    = @email_text + ' Patient    : ' + @patient_name + '\n'
			set @email_text    = @email_text + ' Species    : ' + isnull((select name from species where id = @species_id),'') + '\n'
			set @email_text    = @email_text + ' Breed      : ' + isnull((select name from breed where id = @breed_id),'') + '\n'
			set @email_text    = @email_text + ' Modality   : ' + isnull((select name from modality where id = @modality_id),'') + '\n'
			set @email_text    = @email_text + ' Study Type : ' + isnull(@study_types,'') + '\n'
			set @email_text    = @email_text + '\n\n'
			set @email_text    = @email_text +'<a href=''' + @VRSPACSLINKURL + ''' target=''_blank''>Click here to view the report</a>\n\n'
			set @email_text    = @email_text +'This is an automated message from VETRIS.Please do not reply to the message.\n'


			insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,
								email_subject,email_text,study_hdr_id,study_uid,email_type,
								sender_email_address,sender_email_password,release_email,date_updated)
						values(newid(),getdate(),@recipient_address,@recipient_name,
								@email_subject,@email_text,@id,@study_uid,'RPT',
								@RPTMAILUSRACCT,@RPTMAILUSRPWD,@release,getdate())

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status=0,@error_code='371'
					return 0
				end

			if(isnull(convert(nvarchar(max),@addendum_text),'')<> '')
				begin
					set @activity_text =  'Email of Addendum #' + convert(varchar,@addendum_srl) + ' generated'
				end
			else
				begin
					set @activity_text =  'Email of modified report generated'
				end

			exec common_study_user_activity_trail_save
				@study_hdr_id  = @id,
				@study_uid     =@study_uid,
				@menu_id       = @menu_id,
				@activity_text = @activity_text,
				@session_id    = @session_id,
				@activity_by   = @updated_by,
				@error_code    = @error_code output,
				@return_status = @return_status output

			if(@return_status=0)
				begin
					rollback transaction
					return 0
				end

			update study_hdr set final_rpt_updated = 'Y' where id =@id
								
		end

	if(isnull(@recipient_mobile,'') <> '')
		begin
			create table #tmpMobile
			(
				rec_id int identity(1,1),
				mobile_no nvarchar(20)
			)

			select @SMSSENDERNO = data_type_string
			from general_settings
			where control_code ='SMSSENDERNO'

			set @VRSPACSLINKURL = @VRSPACSLINKURL + convert(varchar(36),@id) + '&vw=rpt&fmt=2' 
			if(isnull(convert(nvarchar(max),@addendum_text),'')<> '')
				begin
					set @sms_text    = 'Addendum added to final report for ' + isnull(@patient_name,'')
				end
			else
				begin
					set @sms_text    = 'Final report for ' + isnull(@patient_name,'') + ' modified'
				end


			if(charindex(';',@recipient_mobile) = 0 )
				begin
					insert into #tmpMobile(mobile_no) values(@recipient_mobile)
				end
			else
				begin
					while(charindex(';',@recipient_mobile) > 0)
						begin
							select @mobile_no = substring(@recipient_mobile,1,charindex(';',@recipient_mobile)-1)
							insert into #tmpMobile(mobile_no) values(rtrim(ltrim(@mobile_no)))
							select @recipient_mobile = substring(@recipient_mobile,charindex(';',@recipient_mobile) + 1,len(@recipient_mobile) - len(substring(@recipient_mobile,1,CHARINDEX(';',@recipient_mobile)-1)))
						end

					if(rtrim(ltrim(@recipient_mobile)) <> '')  insert into #tmpMobile(mobile_no) values(rtrim(ltrim(@recipient_mobile)))
				end

			select @counter=1,
				   @rowcount = count(rec_id) 
			from #tmpMobile

			set @mobile_no=''

			while(@counter <= @rowcount)
				begin
					select @mobile_no = mobile_no
					from #tmpMobile
					where rec_id = @counter

					insert into vrslogdb..sms_log(sms_log_id,sms_log_datetime,recipient_no,recipient_name,sender_no,sequence_no,sms_text,study_hdr_id,study_uid,sms_type,release_sms,date_updated)
							values(newid(),getdate(),@mobile_no,@recipient_name,@SMSSENDERNO,1,@sms_text,@id,@study_uid,'RPT',@release,getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='372'
							return 0
						end

					set @sms_text    = 'Click ' + @VRSPACSLINKURL + ' to view the report'

					insert into vrslogdb..sms_log(sms_log_id,sms_log_datetime,recipient_no,recipient_name,sender_no,sequence_no,sms_text,study_hdr_id,study_uid,date_updated)
								values(newid(),getdate(),@mobile_no,@recipient_name,@SMSSENDERNO,2,@sms_text,@id,@study_uid,getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='372'
							return 0
						end
					
					if(isnull(convert(nvarchar(max),@addendum_text),'')<> '')
						begin
							set @activity_text =  'SMS of Addendum #' + convert(varchar,@addendum_srl) + ' generated'
						end
					else
						begin
							set @activity_text =  'SMS of modified report generated'
						end

					exec common_study_user_activity_trail_save
						@study_hdr_id  = @id,
						@study_uid     = @study_uid,
						@menu_id       = @menu_id,
						@activity_text = @activity_text,
						@session_id    = @session_id,
						@activity_by   = @updated_by,
						@error_code    = @error_code output,
						@return_status = @return_status output

					if(@return_status=0)
						begin
							rollback transaction
							return 0
						end

					set @counter= @counter + 1
				end
		end

	if(@FAXENABLE='Y')
		begin
			select @fax_report = fax_rpt,
				   @recipient_fax_no = fax_no,
				   @custom_report= custom_report
			from institutions
			where id = @institution_id
							
			if(@fax_report='Y' and rtrim(ltrim(isnull(@recipient_fax_no,'')))<>'')
				begin
					set @patient_name = replace(@patient_name,' ','_')
					set @patient_name = replace(@patient_name,'''','')
					set @patient_name = replace(@patient_name,',','_')
					set @patient_name = replace(@patient_name,'__','_')

					set @fax_file_name = 'FINAL_REPORT_' + upper(@patient_name) +'.pdf'

					insert into vrslogdb..fax_log(id,log_datetime,recipient_no,study_hdr_id,study_uid,file_name,institution_id,report_type,custom_report,fax_type,release_fax,date_updated)
							     values(newid(),getdate(),@recipient_fax_no,@id,@study_uid,@fax_file_name,@institution_id,'F',@custom_report,'RPT',@release,getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='447'
							return 0
						end

					if(isnull(convert(nvarchar(max),@addendum_text),'')<> '')
						begin
							set @activity_text =  'Fax of Addendum #' + convert(varchar,@addendum_srl) + ' generated'
						end
					else
						begin
							set @activity_text =  'Fax of modified report generated'
						end

					exec common_study_user_activity_trail_save
						@study_hdr_id  = @id,
						@study_uid     =@study_uid,
						@menu_id       = @menu_id,
						@activity_text = @activity_text,
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



		end
			  
/*************************GENERATE NOTIFICATION*****************************/

	commit transaction
	set @return_status=1
	set @error_code='034'
	set nocount off
	return 1

end


GO
