USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[hk_missing_study_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[hk_missing_study_fetch_params]
GO
/****** Object:  StoredProcedure [dbo].[hk_missing_study_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : hk_missing_study_fetch_params : fetch missing case parameters 
** Created By   : Pavel Guha
** Created On   : 18/07/2019
*******************************************************/
--exec hk_missing_study_fetch_params '97f1dc7a-34ef-45a9-87fa-1d7ddf8882f5'
CREATE PROCEDURE [dbo].[hk_missing_study_fetch_params] 
	@user_id uniqueidentifier
as
begin
	set nocount on

	declare @PACIMGCNTURL nvarchar(max),
	        @NEWDATAURL nvarchar(200),
	        @pacs_login_id nvarchar(20),
			@pacs_pwd nvarchar(200),
			@url_fields nvarchar(1000),
			@WS8SRVIP nvarchar(200),
			@WS8URLMSK nvarchar(200),
			@WS8CLTIP nvarchar(200),
			@WS8SRVUID nvarchar(200),
			@WS8SRVPWD nvarchar(200),
			@APIVER nvarchar(200)


	select @pacs_login_id = pacs_user_id,
	       @pacs_pwd      = pacs_password
	from users
	where id = @user_id

	set @url_fields=''
	select @url_fields = @url_fields + field_code +   '%0A' 
	from sys_pacs_query_fields
    where service_id= 6 
	order by display_index

	set @url_fields = substring(@url_fields,0,len(@url_fields)-2)


	select @PACIMGCNTURL = replace((select data_type_string from general_settings where control_code='PACIMGCNTURL'),'NIMG%0ANOBJ',@url_fields)
	select @PACIMGCNTURL = replace(@PACIMGCNTURL,'#V2',isnull(@pacs_login_id,''))
	select @NEWDATAURL   = data_type_string from general_settings where control_code='NEWDATAURL' 
	select @WS8SRVIP     = data_type_string from general_settings where control_code='WS8SRVIP'
	select @WS8URLMSK    = data_type_string from general_settings where control_code='WS8URLMSK'
	select @WS8CLTIP     = data_type_string from general_settings where control_code='WS8CLTIP'
	select @WS8SRVUID    = data_type_string from general_settings where control_code='WS8SRVUID'
    select @WS8SRVPWD    = data_type_string from general_settings where control_code='WS8SRVPWD'
	select @APIVER       = data_type_string from general_settings where control_code='APIVER'

	select PACIMGCNTURL  = @PACIMGCNTURL,
	       pacs_login_id = @pacs_login_id,
		   pacs_password = @pacs_pwd,
		   NEWDATAURL    = @NEWDATAURL,
		   WS8SRVIP      = @WS8SRVIP,
		   WS8URLMSK     = @WS8URLMSK,
		   WS8CLTIP      = @WS8CLTIP,
		   WS8SRVUID     = @WS8SRVUID,
		   WS8SRVPWD     = @WS8SRVPWD,
		   APIVER        = @APIVER


	set nocount off
end


GO
