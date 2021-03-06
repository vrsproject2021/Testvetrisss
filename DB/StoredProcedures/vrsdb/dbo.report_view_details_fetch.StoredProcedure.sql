USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[report_view_details_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[report_view_details_fetch]
GO
/****** Object:  StoredProcedure [dbo].[report_view_details_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : report_view_details_fetch : fetch physician details
** Created By   : Pavel Guha
** Created On   : 04/06/2019
*******************************************************/
--exec report_view_details_fetch '29b845d7-b957-41e5-b88d-e85c28721d28'
CREATE procedure [dbo].[report_view_details_fetch]
    @id uniqueidentifier
as
begin
	 set nocount on

	 declare @institution_id uniqueidentifier,
			 @study_uid nvarchar(100),
	         @pacs_user_id nvarchar(20),
			 @pacs_password nvarchar(200),
			 @PACSRPTVWRURL nvarchar(200),
			 @PACIMGVWRURL nvarchar(200),
			 @accession_no nvarchar(20),
			 @study_status_id int,
			 @user_id uniqueIdentifier,
			 @patient_name nvarchar(250),
			 @patient_id nvarchar(50),
			 @custom_report nchar(1),
			 @billing_account_id uniqueidentifier,
			 @APIVER nvarchar(200),
			 @WS8CLTIP nvarchar(200),
			 @WS8SRVIP nvarchar(200),
			 @WS8SRVUID nvarchar(200),
			 @WS8SRVPWD nvarchar(200),
			 @rpt_fmt nchar(1)

	select @APIVER = data_type_string
	from general_settings
	where control_code ='APIVER'

    if(select count(id) from study_hdr where id=@id)>0
		begin
			select @institution_id  = institution_id,
				   @accession_no    = accession_no,
				   @study_uid       = study_uid,
				   @study_status_id = study_status_pacs,
				   @patient_name    = isnull(patient_name,''),
				   @patient_id      = isnull(patient_id,'')
			from study_hdr 
			where id = @id
		end
    else if(select count(id) from study_hdr_archive where id=@id)>0
		begin
			select @institution_id  = institution_id,
				   @accession_no    = accession_no,
				   @study_uid       = study_uid,
				   @study_status_id = study_status_pacs,
				   @patient_name    = isnull(patient_name,''),
				   @patient_id      = isnull(patient_id,'')
			from study_hdr_archive 
			where id = @id
		end
	
	--print @accession_no
	select @rpt_fmt = rpt_format from institutions where id=@institution_id

	select top 1 @pacs_user_id = iul.user_pacs_user_id,
	             @pacs_password = iul.user_pacs_password
	from institution_user_link iul
	inner join users u on u.login_id = iul.user_pacs_user_id
	where iul.institution_id = @institution_id
	and u.is_active='Y'

	select @custom_report = isnull(custom_report,'N') from institutions where id=@institution_id

	if(isnull(@pacs_user_id,'')='')
		begin
			select @billing_account_id = billing_account_id from institutions where id = @institution_id

			select @pacs_user_id = login_id,
	               @pacs_password = login_pwd
			from billing_account 
			where id = @billing_account_id
		end

	select @user_id= id from users where login_id = @pacs_user_id

	if(@APIVER='7.2')
		begin
			select @PACSRPTVWRURL = replace((select data_type_string from general_settings where control_code='PACSRPTVWRURL'),'#V1',@accession_no)
			select @PACSRPTVWRURL = replace(@PACSRPTVWRURL,'#V2',isnull(@pacs_user_id,''))
			select @PACIMGVWRURL = replace((select data_type_string from general_settings where control_code='PACIMGVWRURL'),'#V1',@study_uid)
			select @PACIMGVWRURL = replace(@PACIMGVWRURL,'#V2',isnull(@pacs_user_id,''))
		end
	else
		begin
			 select @WS8CLTIP  = data_type_string from general_settings where control_code='WS8CLTIP'
			 select @WS8SRVIP  = data_type_string from general_settings where control_code='WS8SRVIP'
			 select @WS8SRVUID = data_type_string from general_settings where control_code='WS8SRVUID'
			 select @WS8SRVPWD = data_type_string from general_settings where control_code='WS8SRVPWD'

			 select @PACSRPTVWRURL = replace((select data_type_string from general_settings where control_code='PACSRPTVWRURL'),'#V1',@accession_no)
			 select @PACSRPTVWRURL = replace(@PACSRPTVWRURL,'#V2',isnull(@pacs_user_id,''))
			 select @PACIMGVWRURL = replace((select data_type_string from general_settings where control_code='WS8IMGVWRURL'),'#V1',@accession_no)
			 select @PACIMGVWRURL = replace(@PACIMGVWRURL,'#V2',isnull(@patient_id,''))
			 select @PACIMGVWRURL = replace(@PACIMGVWRURL,'#V4',isnull(@WS8SRVUID,''))

			 set @pacs_user_id = @WS8SRVUID
			 set @pacs_password = @WS8SRVPWD
		end

	select APIVER        = @APIVER,
		   PACSRPTVWRURL = isnull(@PACSRPTVWRURL,''),
		   PACIMGVWRURL = isnull(@PACIMGVWRURL,''),
	       pacs_password = isnull(@pacs_password,''),
		   pacs_user_id  = isnull(@pacs_user_id,''),
		   patient_name  = @patient_name,
		   custom_report = @custom_report,
		   rpt_fmt       = @rpt_fmt,
		   status_id     = @study_status_id,
		   user_id       = isnull(@user_id,'00000000-0000-0000-0000-000000000000'),
		   WS8CLTIP      = isnull(@WS8CLTIP,''),
		   WS8SRVIP      = isnull(@WS8SRVIP,''),
		   WS8SRVUID     = isnull(@WS8SRVUID,''),
		   WS8SRVPWD     = isnull(@WS8SRVPWD,'')
	         
		
	 --select study_uid     = (select study_uid from study_hdr where id=@id),
	 --       PACSLOGINURL  = (select data_type_string from general_settings where control_code='PACSLOGINURL'),
		--	PACMAILRPTURL = (select data_type_string from general_settings where control_code='PACMAILRPTURL')

	set nocount off
end

GO
