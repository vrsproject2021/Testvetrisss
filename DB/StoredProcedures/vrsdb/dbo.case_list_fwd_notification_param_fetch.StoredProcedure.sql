USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fwd_notification_param_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_fwd_notification_param_fetch]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fwd_notification_param_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_notification_param_fetch : 
                  fetch invoicing mail parameters 
				  if locked by other user
** Created By   : Pavel Guha 
** Created On   : 03/02/2020
*******************************************************/
--exec case_list_fwd_notification_param_fetch '92EF3CE9-7B76-48EE-9BF7-94AE85944329','11111111-1111-1111-1111-111111111111'
CREATE procedure [dbo].[case_list_fwd_notification_param_fetch]
	@study_id uniqueidentifier,
	@user_id uniqueidentifier
as
	begin
		set nocount on

		declare @patient_name nvarchar(250),
		        @study_types varchar(max),
				@rpt_type nvarchar(20),
				@email_subject varchar(250),
			    @email_text nvarchar(max),
				@VRSPACSLINKURL nvarchar(200),
				@sms_text nvarchar(max),
				@fax_rpt nchar(1),
				@fax_to_number nvarchar(20),
				@study_status int,
				@custom_rpt nchar(1),
				@species_id int,
				@species_name nvarchar(30),
				@breed_id uniqueidentifier,
				@breed_name nvarchar(50),
				@modality_id int,
				@modality_name nvarchar(30)

		--sender name
		select name from users where id = @user_id
		
		--notification params
		select mail_server      = (select data_type_string from general_settings where control_code='MAILSVRNAME'),
			   mail_server_port = (select data_type_number from general_settings where control_code='MAILSVRPORT'),
		       mail_user_code   = (select data_type_string from general_settings where control_code='RPTMAILUSRACCT'),
			   mail_user_pwd    = (select data_type_string from general_settings where control_code='RPTMAILUSRPWD'),
			   mail_ssl_enabled = (select data_type_string from general_settings where control_code='MAILSSLENABLED'),
			   smd_acct_sid     = (select data_type_string from general_settings where control_code='SMSACCTSID'),
			   smd_auth_token   = (select data_type_string from general_settings where control_code='SMSAUTHTKNNO'),
			   sms_sender_no    = (select data_type_string from general_settings where control_code='SMSSENDERNO'),
			   fax_enabled      = (select data_type_string from general_settings where control_code='FAXENABLE'),
			   fax_api_url      = (select data_type_string from general_settings where control_code='FAXAPIURL'),
		       fax_auth_uid     = (select data_type_string from general_settings where control_code='FAXAUTHUSERID'),
			   fax_auth_pwd     = (select data_type_string from general_settings where control_code='FAXAUTHPWD'),
			   fax_csid         = (select data_type_string from general_settings where control_code='FAXCSID'),
			   fax_ref_text     = (select data_type_string from general_settings where control_code='FAXREFTEXT'),
			   fax_reply_at     = (select data_type_string from general_settings where control_code='FAXREPADDR'),
			   fax_contact_at   = (select data_type_string from general_settings where control_code='FAXCONTACT'),
			   fax_retry        = (select data_type_number from general_settings where control_code='FAXRETRY')

		--texts
		if(select count(id) from study_hdr where id = @study_id)>0
			begin
				select @patient_name = sh.patient_name,
				       @modality_id  = sh.modality_id,
					   @species_id   = sh.species_id,
					   @breed_id     = sh.breed_id,
				       @study_status = sh.study_status_pacs,
					   @rpt_type     = s.status_desc,
					   @fax_rpt      = isnull(i.fax_rpt,'N'),
					   @fax_to_number= replace(isnull(i.fax_no,''),'-',''),
					   @custom_rpt   = isnull(i.custom_report,'N')
				from study_hdr sh
				inner join sys_study_status_pacs s on s.status_id = sh.study_status_pacs
				inner join institutions i on i.id = sh.institution_id
				where sh.id= @study_id

				set @study_types=''
				select @study_types = @study_types +  convert(varchar,st.name) + ','
				from study_hdr_study_types shst
				inner join  modality_study_types st on st.id= shst.study_type_id
				where shst.study_hdr_id=@study_id 
			end
		else
			begin
				select @patient_name = sh.patient_name,
				       @modality_id  = sh.modality_id,
					   @species_id   = sh.species_id,
					   @breed_id     = sh.breed_id,
					   @study_status = sh.study_status_pacs,
					   @rpt_type     = s.status_desc,
					   @fax_rpt      = isnull(i.fax_rpt,'N'),
					   @fax_to_number= replace(isnull(i.fax_no,''),'-',''),
					   @custom_rpt   = isnull(i.custom_report,'N')
				from study_hdr_archive sh
				inner join sys_study_status_pacs s on s.status_id = sh.study_status_pacs
				inner join institutions i on i.id = sh.institution_id
				where sh.id= @study_id

				set @study_types=''
				select @study_types = @study_types +  convert(varchar,st.name) + ','
				from study_hdr_study_types_archive shst
				inner join  modality_study_types st on st.id= shst.study_type_id
				where shst.study_hdr_id=@study_id 
			end

		--print @patient_name
		--print @rpt_type

		

		select @modality_name = name from modality where id = @modality_id
		select @species_name = name from species where id=@species_id
		select @breed_name = name from breed where id=@breed_id

		if(isnull(@study_types,'')<>'') select @study_types = substring(@study_types,1,len(@study_types)-1)
		set @email_subject = @rpt_type + ' Report Available For ' + isnull(@patient_name,'')
						
		set @email_text    = @rpt_type + ' report is available for :- <br/>'
		set @email_text    = @email_text + ' Patient    : ' + @patient_name + '<br/>'
		set @email_text    = @email_text + ' Species    : ' + isnull(@species_name,'') + '<br/>'
		set @email_text    = @email_text + ' Breed      : ' + isnull(@breed_name,'') + '<br/>'
		set @email_text    = @email_text + ' Modality   : ' + isnull(@modality_name,'') + '<br/>'
		set @email_text    = @email_text + ' Study Type : ' + isnull(@study_types,'') + '<br/><br/>'
		set @email_text    = @email_text + '(Please check the PDF version of the report attached with this email.)<br/>'

		
		set @sms_text    = @rpt_type + ' report available for ' + isnull(@patient_name,'')

		select @VRSPACSLINKURL = data_type_string
		from general_settings
		where control_code ='VRSPACSLINKURL'

		set @VRSPACSLINKURL = @VRSPACSLINKURL + convert(varchar(36),@study_id)


		select	email_subject  = @email_subject,
				email_text     = @email_text,
				sms_text       = @sms_text,
				fax_rpt        = @fax_rpt,
				fax_no         = @fax_to_number,
				study_status   = @study_status,
				custom_rpt     = @custom_rpt,
				patient_name   = @patient_name,
				VRSPACSLINKURL = @VRSPACSLINKURL



		set nocount off
		
	end
GO
