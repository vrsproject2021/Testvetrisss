USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_case_study_report_save_ws8]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_case_study_report_save_ws8]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_case_study_report_save_ws8]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_case_study_report_save_ws8 : update
                  case study reports
** Created By   : Pavel Guha
** Created On   : 12/03/2020
*******************************************************/
CREATE procedure [dbo].[scheduler_case_study_report_save_ws8]
    @study_id uniqueidentifier,
	@study_uid nvarchar(100),
	@status_id int,
	@report_text nvarchar(max) =null,
	@report_text_html ntext =null,
	@TVP_addendums as case_study_report_addendum readonly,
	@PACIMGVWRURL nvarchar(200),
	@PACLOGINURL nvarchar(200),
	@PACSRPTVWRURL nvarchar(200),
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	set nocount on

	declare @gen_mail nchar(1),
			@gen_sms nchar(1),
			@gen_fax nchar(1),
	        @rpt_type nvarchar(20),
			@recipient_address nvarchar(500),
			@recipient_name nvarchar(200),
			@email_subject varchar(250),
			@email_text nvarchar(max),
			@patient_name nvarchar(200),
			@owner_name nvarchar(200),
			@PACMAILRPTURL nvarchar(200),
			@VRSPACSLINKURL nvarchar(200),
			@SMSSENDERNO nvarchar(200),
			@study_types varchar(max),
			@recipient_mobile nvarchar(500),
			@mobile_no nvarchar(20),
			@sms_text nvarchar(max),
			@fax_no nvarchar(30),
			@fax_rpt nchar(1),
			@fax_file_name nvarchar(max),
			@custom_rpt nchar(1),
			@institution_id uniqueidentifier,
			@physician_id uniqueidentifier,
			@rowcount int,
			@counter int,
			@old_report_text nvarchar(max),
			@addendum_text nvarchar(max),
			@old_addendum_text nvarchar(max),
			@addendum_text_html nvarchar(max),
			@old_addendum_text_html nvarchar(max),
			@srl_no int,
			@report_id uniqueidentifier,
			@synch_from_pacs nchar(1),
			@created_by uniqueidentifier,
			@final_rpt_release_datetime datetime,
			@release nchar(1)

	declare @RPTMAILUSRACCT nvarchar(100),
            @RPTMAILUSRPWD nvarchar(100),
			@FAXENABLE nchar(1)

	create table #tmpMobile
	(
		rec_id int identity(1,1),
		mobile_no nvarchar(20)
	)

	begin transaction

	set @gen_mail='N'
	set @gen_sms='N'
	set @gen_fax='N'
	set @rpt_type=''

	select @RPTMAILUSRACCT = data_type_string
    from general_settings
    where control_code ='RPTMAILUSRACCT'

    select @RPTMAILUSRPWD = data_type_string
    from general_settings
    where control_code ='RPTMAILUSRPWD'

	select @institution_id             = institution_id,
	       @physician_id               = physician_id
	from study_hdr
	where id = @study_id


	if(@status_id =60)
		begin
			if(select count(report_id) from study_hdr_dictated_reports where study_hdr_id = @study_id and study_uid =@study_uid)=0
				begin
					select @created_by = (select login_user_id from radiologists where id = isnull((select prelim_radiologist_id from study_hdr where id=@study_id and study_uid=@study_uid),'00000000-0000-0000-0000-000000000000'))
					insert into study_hdr_dictated_reports(report_id,study_hdr_id,study_uid,report_text,report_text_html,created_by,date_created)
												    values(newid(),@study_id,@study_uid,@report_text,@report_text_html,isnull(@created_by,'00000000-0000-0000-0000-000000000000'),getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to update dictated report for Study UID ' + @study_uid
							return 0
						end
				end
			else
				begin
					select @synch_from_pacs = synch_from_pacs from study_hdr_dictated_reports where study_hdr_id = @study_id and study_uid =@study_uid 

					if(@synch_from_pacs='Y')
						begin
							update study_hdr_dictated_reports
							set report_text      = @report_text,
							    report_text_html = @report_text_html,
								synch_from_pacs  ='N'
							where study_hdr_id = @study_id 
							and study_uid =@study_uid

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to update dictated report for Study UID ' + @study_uid
									return 0
								end
						 end
				end
		end
	else if(@status_id =80)
		begin
			-- for notifying report creation by email/Fax
			if(select count(report_id) from study_hdr_prelim_reports where study_hdr_id = @study_id and study_uid =@study_uid)=0
				begin
					--if(datalength(@report_text) = 0) set @gen_mail='N'
					--else set @gen_mail='Y'
					
					set @gen_mail='Y'
					set @gen_sms='Y'
					set @gen_fax='Y'
					set @rpt_type='Preliminary'

					select @created_by = (select login_user_id from radiologists where id = isnull((select prelim_radiologist_id from study_hdr where id=@study_id and study_uid=@study_uid),'00000000-0000-0000-0000-000000000000'))
					insert into study_hdr_prelim_reports(report_id,study_hdr_id,study_uid,report_text,report_text_html,created_by,date_created)
												  values(newid(),@study_id,@study_uid,@report_text,@report_text_html,isnull(@created_by,'00000000-0000-0000-0000-000000000000'),getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to update preliminary report for Study UID ' + @study_uid
							return 0
						end
				end
			else
				begin
					select @synch_from_pacs = synch_from_pacs from study_hdr_prelim_reports where study_hdr_id = @study_id and study_uid =@study_uid

					if(@synch_from_pacs = 'Y')
						begin
							update study_hdr_prelim_reports
							set report_text      = @report_text,
								report_text_html = @report_text_html,
								synch_from_pacs  = 'N'
							where study_hdr_id = @study_id 
							and study_uid =@study_uid

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to update report for Study UID ' + @study_uid
									return 0
								end

							set @gen_mail='Y'
							set @gen_sms='Y'
							set @gen_fax ='Y'
							set @rpt_type='Preliminary'

						end
				end

			-- for notifying report creation by SMS
			if(select count(report_id) from study_hdr_prelim_reports where study_hdr_id = @study_id and study_uid =@study_uid)=0
				begin
					set @gen_sms='Y'
					set @rpt_type='Preliminary'
					select @created_by = (select login_user_id from radiologists where id = isnull((select prelim_radiologist_id from study_hdr where id=@study_id and study_uid=@study_uid),'00000000-0000-0000-0000-000000000000'))

					insert into study_hdr_prelim_reports(report_id,study_hdr_id,study_uid,report_text,report_text_html,sms_updated,created_by,date_created)
												  values(newid(),@study_id,@study_uid,@report_text,@report_text_html,'Y',isnull(@created_by,'00000000-0000-0000-0000-000000000000'),getdate())
					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to update preliminary report for Study UID ' + @study_uid
							return 0
						end
				end
			else
				begin
					select @synch_from_pacs = synch_from_pacs from study_hdr_prelim_reports where study_hdr_id = @study_id and study_uid =@study_uid

					if(@synch_from_pacs = 'Y')
						begin
							update study_hdr_prelim_reports
							set report_text   = @report_text,
								report_text_html = @report_text_html,
								sms_updated ='Y',
								synch_from_pacs ='N'
							where study_hdr_id = @study_id 
							and study_uid =@study_uid

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to update report for Study UID ' + @study_uid
									return 0
								end

							set @gen_sms='Y'
							set @rpt_type='Preliminary'

					    end
				end

			update study_hdr set prelim_sms_updated = 'Y' where id =@study_id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to update status for report synch for Study UID ' + @study_uid
					return 0
				end


		end
	else if(@status_id =100)
		begin
			-- for notifying report creation by email
			if(select count(report_id) from study_hdr_final_reports where study_hdr_id = @study_id and study_uid =@study_uid)=0
				begin
					set @gen_mail='Y'
					set @gen_sms='Y'
					set @gen_fax='Y'
					set @rpt_type='Final'
					select @created_by = (select login_user_id from radiologists where id = isnull((select final_radiologist_id from study_hdr where id=@study_id and study_uid=@study_uid),'00000000-0000-0000-0000-000000000000'))
					insert into study_hdr_final_reports(report_id,study_hdr_id,study_uid,report_text,report_text_html,created_by,date_created)
												  values(newid(),@study_id,@study_uid,@report_text,@report_text_html,isnull(@created_by,'00000000-0000-0000-0000-000000000000'),getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to update final report for Study UID ' + @study_uid
							return 0
						end

				end
			else
				begin
					
					select @synch_from_pacs = synch_from_pacs from study_hdr_prelim_reports where study_hdr_id = @study_id and study_uid =@study_uid

					if(@synch_from_pacs='Y')
						begin
							update study_hdr_final_reports
							set report_text      = @report_text,
								report_text_html = @report_text_html,
								synch_from_pacs ='N'
							where study_hdr_id = @study_id 
							and study_uid =@study_uid

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to update final report for Study UID ' + @study_uid
									return 0
								end

							set @gen_mail='Y'
							set @gen_sms='Y'
							set @gen_fax='Y'
							set @rpt_type='Final'
						end
				end

			-- for notifying report creation by SMS
			if(select count(report_id) from study_hdr_final_reports where study_hdr_id = @study_id and study_uid =@study_uid)=0
				begin
					set @gen_sms='Y'
					set @rpt_type='Final'
					set @report_id = newid()
					select @created_by = (select login_user_id from radiologists where id = isnull((select final_radiologist_id from study_hdr where id=@study_id and study_uid=@study_uid),'00000000-0000-0000-0000-000000000000'))
					insert into study_hdr_final_reports(report_id,study_hdr_id,study_uid,report_text,report_text_html,created_by,date_created)
												  values(@report_id,@study_id,@study_uid,@report_text,@report_text_html,isnull(@created_by,'00000000-0000-0000-0000-000000000000'),getdate())
					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to update final report for Study UID ' + @study_uid
							return 0
						end
				end
			else
				begin
					select @synch_from_pacs = synch_from_pacs from study_hdr_prelim_reports where study_hdr_id = @study_id and study_uid =@study_uid
					if(@synch_from_pacs = 'Y')
						begin
							update study_hdr_final_reports
							set report_text = @report_text,
								report_text_html = @report_text_html,
								synch_from_pacs = 'N'
							where study_hdr_id = @study_id 
							and study_uid =@study_uid

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to update report for Study UID ' + @study_uid
									return 0
								end

							set @gen_sms='Y'
							set @rpt_type='Final'
						end
				end


			--ADDENDUMS
			select @rowcount = count(srl_no),
				   @counter = 1
		    from @TVP_addendums

			while(@counter <= @rowcount)
				begin
					select @srl_no             = srl_no,
					       @addendum_text      = addendum_text,
						   @addendum_text_html = addendum_text_html
					from @TVP_addendums
					where srl_no = @counter

					---- for notifying report creation by email
					if(select count(addendum_srl) from study_report_addendums where study_hdr_id = @study_id and study_uid =@study_uid and addendum_srl=@srl_no)=0
						begin
							select @report_id = report_id from study_hdr_final_reports where study_hdr_id = @study_id and study_uid =@study_uid

							insert into study_report_addendums(report_id,addendum_srl,study_hdr_id,study_uid,addendum_text,addendum_text_html,
							                                   created_by,date_created)
														values(@report_id,@srl_no,@study_id,@study_uid,@addendum_text,@addendum_text_html,
							                                   '00000000-0000-0000-0000-000000000000',getdate())

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to update report addendum for Study UID ' + @study_uid
									return 0
								end

							set @gen_mail='Y'
							set @gen_sms='Y'
							set @gen_fax='Y'
							set @rpt_type='Final'
						end
					--else
					--	begin
					--		select @old_addendum_text = addendum_text from study_report_addendums where study_hdr_id = @study_id and study_uid =@study_uid and addendum_srl=@srl_no

					--		if((@old_addendum_text <> @addendum_text))
					--			begin
					--				update study_report_addendums
					--				set addendum_text      = @addendum_text,
					--				    addendum_text_html = @addendum_text_html
					--				where addendum_srl=@srl_no
					--				and study_hdr_id = @study_id 
					--				and study_uid =@study_uid

					--				if(@@rowcount=0)
					--					begin
					--						rollback transaction
					--						select @return_type=0,@error_msg='Failed to update report addendum for Study UID ' + @study_uid
					--						return 0
					--					end

					--				set @gen_mail='Y'
					--				set @gen_sms='Y'
					--				set @rpt_type='Final'
					--			end
					--	end

					-- for notifying report creation by SMS
					if(select count(addendum_srl) from study_report_addendums where study_hdr_id = @study_id and study_uid =@study_uid and addendum_srl=@srl_no)=0
						begin
							set @gen_sms='Y'
							set @rpt_type='Final'
							select @report_id = report_id from study_hdr_final_reports where study_hdr_id = @study_id and study_uid =@study_uid

							insert into study_report_addendums(report_id,addendum_srl,study_hdr_id,study_uid,addendum_text,addendum_text_html,
												created_by,date_created)
										values(@report_id,@srl_no,@study_id,@study_uid,@addendum_text,@addendum_text_html,
												'00000000-0000-0000-0000-000000000000',getdate())
							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to update report addendum for Study UID ' + @study_uid
									return 0
								end
						end
					--else
					--	begin
					--		select @old_addendum_text = addendum_text from study_report_addendums where study_hdr_id = @study_id and study_uid =@study_uid and addendum_srl=@srl_no
					--		if((@old_addendum_text <> @addendum_text))
					--			begin
					--				update study_report_addendums
					--				set addendum_text      = @addendum_text,
					--				    addendum_text_html = @addendum_text_html
					--				where addendum_srl=@srl_no
					--				and study_hdr_id = @study_id 
					--				and study_uid =@study_uid

					--				if(@@rowcount=0)
					--					begin
					--						rollback transaction
					--						select @return_type=0,@error_msg='Failed to update report addendum for Study UID ' + @study_uid
					--						return 0
					--					end

					--				set @gen_sms='Y'
					--				set @rpt_type='Final'
					--			end
					--	end
					
					set @counter = @counter + 1
				end

			
		end


	if(@gen_mail='Y')
		begin
			
			select @recipient_address = isnull(physician_email,''),
			       @recipient_name    = isnull(physician_name,'')
			from institution_physician_link
			where institution_id = @institution_id
			and physician_id = @physician_id

			if(isnull(@recipient_address,'') <> '')
				begin
						select @patient_name               = patient_name,
							   @final_rpt_release_datetime = final_rpt_release_datetime
						from study_hdr
						where id= @study_id
						and study_uid = @study_uid

						select @owner_name = rtrim(ltrim(owner_first_name + ' ' + owner_last_name))
						from study_hdr
						where id= @study_id
						and study_uid = @study_uid

						--set @PACIMGVWRURL   = replace(@PACIMGVWRURL,'#V1',@study_uid)
						--set @PACLOGINURL    = replace(@PACLOGINURL,'#V1',@study_uid)
						--set @PACSRPTVWRURL  = replace(@PACSRPTVWRURL,'#V1',(select accession_no from study_hdr where id=@study_id and study_uid=@study_uid))

						--select @PACMAILRPTURL = data_type_string
						--from general_settings
						--where control_code ='PACMAILRPTURL'

						select @VRSPACSLINKURL = data_type_string
						from general_settings
						where control_code ='VRSPACSLINKURL'

						--set @PACMAILRPTURL = @PACMAILRPTURL + @study_uid

						set @VRSPACSLINKURL = @VRSPACSLINKURL + convert(varchar(36),@study_id) + '&vw=rpt&fmt=1'

						if(@rpt_type='Preliminary')
							begin
								set @release ='Y'
							end
						else if(@rpt_type='Final')
							begin
								if(@final_rpt_release_datetime<getdate())
									begin
										set @release='Y'
									end
								else
									begin
										set @release='N'
									end
							end

						set @study_types=''
						select @study_types = @study_types +  convert(varchar,st.name) + ','
						from study_hdr_study_types shst
						inner join  modality_study_types st on st.id= shst.study_type_id
						where shst.study_hdr_id=@study_id 

						if(isnull(@study_types,'')<>'') select @study_types = substring(@study_types,1,len(@study_types)-1)
						

						set @email_subject = @rpt_type + ' Report Available For ' + isnull(@patient_name,'')
						
						set @email_text    = @rpt_type + ' report is available for :- \n\n'
						set @email_text    = @email_text + ' Patient    : ' + @patient_name + '\n'
						--set @email_text    = @email_text + ' Owner      : ' + @owner_name + '\n'
						set @email_text    = @email_text + ' Species    : ' + isnull((select name from species where id = (select species_id from study_hdr where id=@study_id and study_uid = @study_uid)),'') + '\n'
						set @email_text    = @email_text + ' Breed      : ' + isnull((select name from breed where id = (select breed_id from study_hdr where id=@study_id and study_uid = @study_uid)),'') + '\n'
						set @email_text    = @email_text + ' Modality   : ' + isnull((select name from modality where id = (select modality_id from study_hdr where id=@study_id and study_uid = @study_uid)),'') + '\n'
						set @email_text    = @email_text + ' Study Type : ' + isnull(@study_types,'') + '\n'
						set @email_text    = @email_text + '\n\n'
						set @email_text    = @email_text +'<a href=''' + @VRSPACSLINKURL + ''' target=''_blank''>Click here to view the report</a>\n\n'
						set @email_text    = @email_text +'This is an automated message from VETRIS.Please do not reply to the message.\n'


						insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,
											            email_subject,email_text,study_hdr_id,study_uid,sender_email_address,sender_email_password,
											            release_email,date_updated)
										      values(newid(),getdate(),@recipient_address,@recipient_name,
											         @email_subject,@email_text,@study_id,@study_uid,@RPTMAILUSRACCT,@RPTMAILUSRPWD,
											         @release,getdate())

						if(@@rowcount=0)
							begin
								rollback transaction
								select @return_type=0,@error_msg='Failed to generate email record for Study UID ' + @study_uid
								return 0
							end

						if(@rpt_type = 'Preliminary')
							begin
								update study_hdr_prelim_reports
								set report_text   = @report_text,
									email_updated ='Y'
								where study_hdr_id = @study_id 
								and study_uid =@study_uid

								if(@@rowcount=0)
									begin
										rollback transaction
										select @return_type=0,@error_msg='Failed to update report for Study UID ' + @study_uid
										return 0
									end

								update study_hdr set prelim_rpt_updated = 'Y' where id =@study_id

								if(@@rowcount=0)
									begin
										rollback transaction
										select @return_type=0,@error_msg='Failed to update status for report synch for Study UID ' + @study_uid
										return 0
									end
							end
						else if(@rpt_type = 'Final')
							begin
								update study_hdr_final_reports
								set report_text   = @report_text,
									email_updated ='Y'
								where study_hdr_id = @study_id 
								and study_uid =@study_uid

								if(@@rowcount=0)
									begin
										rollback transaction
										select @return_type=0,@error_msg='Failed to update report for Study UID ' + @study_uid
										return 0
									end

								 update study_hdr set final_rpt_updated = 'Y' where id =@study_id

								 if(@@rowcount=0)
									begin
										rollback transaction
										select @return_type=0,@error_msg='Failed to update status for report synch for Study UID ' + @study_uid
										return 0
									end
							end
				end
			

		end

	if(@gen_sms='Y')
		begin
			
			select @recipient_mobile = isnull(physician_mobile,''),
			       @recipient_name    = isnull(physician_name,'')
			from institution_physician_link
			where institution_id = @institution_id
			and physician_id = @physician_id

			if(isnull(@recipient_mobile,'') <> '')
				begin
						select @patient_name               = patient_name,
							   @final_rpt_release_datetime = final_rpt_release_datetime
						from study_hdr
						where id= @study_id
						and study_uid = @study_uid

						select @owner_name = rtrim(ltrim(isnull(owner_first_name,'') + ' ' + isnull(owner_last_name,'')))
						from study_hdr
						where id= @study_id
						and study_uid = @study_uid

						--select @PACMAILRPTURL = data_type_string
						--from general_settings
						--where control_code ='PACMAILRPTURL'

						select @VRSPACSLINKURL = data_type_string
						from general_settings
						where control_code ='VRSPACSLINKURL'

						select @SMSSENDERNO = data_type_string
						from general_settings
						where control_code ='SMSSENDERNO'


						--set @PACMAILRPTURL = @PACMAILRPTURL + @study_uid

						set @VRSPACSLINKURL = @VRSPACSLINKURL + convert(varchar(36),@study_id) + '&vw=rpt&fmt=1' 

						set @sms_text    = @rpt_type + ' report available for ' + isnull(@patient_name,'')
						--+ '/' + isnull(@owner_name,'')

						if(@rpt_type='Preliminary')
							begin
								set @release ='Y'
							end
						else if(@rpt_type='Final')
							begin
								if(@final_rpt_release_datetime<getdate())
									begin
										set @release='Y'
									end
								else
									begin
										set @release='N'
									end
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
										               values(newid(),getdate(),@mobile_no,@recipient_name,@SMSSENDERNO,1,@sms_text,@study_id,@study_uid,'RPT',@release,getdate())

								if(@@rowcount=0)
									begin
										rollback transaction
										select @return_type=0,@error_msg='Failed to generate sms record for Study UID ' + @study_uid
										return 0
									end

								set @sms_text    = 'Click ' + @VRSPACSLINKURL + ' to view the report'

								insert into vrslogdb..sms_log(sms_log_id,sms_log_datetime,recipient_no,recipient_name,sender_no,sequence_no,sms_text,study_hdr_id,study_uid,date_updated)
												       values(newid(),getdate(),@mobile_no,@recipient_name,@SMSSENDERNO,2,@sms_text,@study_id,@study_uid,getdate())

								if(@@rowcount=0)
									begin
										rollback transaction
										select @return_type=0,@error_msg='Failed to generate sms record for Study UID ' + @study_uid
										return 0
									end

								set @counter= @counter + 1
							end


						if(@rpt_type = 'Preliminary')
							begin
								update study_hdr_prelim_reports
								set report_text   = @report_text,
									sms_updated ='Y'
								where study_hdr_id = @study_id 
								and study_uid =@study_uid

								if(@@rowcount=0)
									begin
										rollback transaction
										select @return_type=0,@error_msg='Failed to update report for Study UID ' + @study_uid
										return 0
									end

								update study_hdr set prelim_sms_updated = 'Y' where id =@study_id

								if(@@rowcount=0)
									begin
										rollback transaction
										select @return_type=0,@error_msg='Failed to update status for report synch for Study UID ' + @study_uid
										return 0
									end
							end
						else if(@rpt_type = 'Final')
							begin
								update study_hdr_final_reports
								set report_text   = @report_text,
									sms_updated ='Y'
								where study_hdr_id = @study_id 
								and study_uid =@study_uid

								if(@@rowcount=0)
									begin
										rollback transaction
										select @return_type=0,@error_msg='Failed to update report for Study UID ' + @study_uid
										return 0
									end

								 update study_hdr set final_sms_updated = 'Y' where id =@study_id

								 if(@@rowcount=0)
									begin
										rollback transaction
										select @return_type=0,@error_msg='Failed to update status for report synch for Study UID ' + @study_uid
										return 0
									end
							end


				end
			

		end

	if(@gen_fax='Y')
		begin
			if(@FAXENABLE='Y')
				begin
					select @fax_no  = isnull(fax_no,''),
						   @fax_rpt = isnull(fax_rpt,''),
						   @custom_rpt = custom_report
					from institutions
					where id = @institution_id

					if(isnull(@fax_no,'') <> '' and isnull(@fax_rpt,'N')='Y')
						begin
							select @patient_name               = patient_name,
							       @final_rpt_release_datetime = final_rpt_release_datetime
						    from study_hdr
						    where id= @study_id
						    and study_uid = @study_uid

							if(@rpt_type='Preliminary')
									begin
										set @release ='Y'
									end
								else if(@rpt_type='Final')
									begin
										if(@final_rpt_release_datetime<getdate())
											begin
												set @release='Y'
											end
										else
											begin
												set @release='N'
											end
									end

							set @patient_name = replace(@patient_name,' ','_')
							set @patient_name = replace(@patient_name,'''','')
							set @patient_name = replace(@patient_name,',','_')
							set @patient_name = replace(@patient_name,'__','_')

							set @fax_file_name = upper(@rpt_type) + '_REPORT_' + upper(@patient_name) +'.pdf'

							insert into vrslogdb..fax_log(id,log_datetime,recipient_no,study_hdr_id,study_uid,file_name,institution_id,report_type,custom_report,fax_type,release_fax,date_updated)
											       values(newid(),getdate(),@fax_no,@study_id,@study_uid,@fax_file_name,@institution_id,substring(upper(@rpt_type),1,1),@custom_rpt,'RPT',@release,getdate())

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to generate fax record for Study UID ' + @study_uid
									return 0
								end

						end
				end
		end

	commit transaction
	set @return_type=1
	set @error_msg='Report of for Study UID ' + @study_uid + ' updated successfully'
	set nocount off
	return 1

end


GO
