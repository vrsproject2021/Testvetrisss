USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_service_details_fetch_ws8]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_service_details_fetch_ws8]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_service_details_fetch_ws8]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_service_details_fetch_ws8 : fetch service details
** Created By   : Pavel Guha
** Created On   : 11/03/2020
*******************************************************/
--exec scheduler_service_details_fetch_ws8 9
CREATE procedure [dbo].[scheduler_service_details_fetch_ws8]
	@service_id int
as
begin
	set nocount on

    select service_name,frequency
	from scheduler_data_services 
	where service_id=@service_id

	if(@service_id =1)
		begin
			select control_code,data_type_string from general_settings where control_code in ('WS8SRVIP','WS8CLTIP','APIVER','WS8SRVUID','WS8SRVPWD','LFPATH','DCMDMPEXEPATH','FTPSRCFOLDER')
			select field_code from sys_pacs_query_fields where service_id = @service_id order by display_index
		end
	else if(@service_id =2)
		begin
			select control_code,data_type_string from general_settings where control_code in ('WS8SRVIP','WS8CLTIP','APIVER','WS8SRVUID','WS8SRVPWD','WRITEBACKURL','XFEREXEPATH','PACSARCHIVEFLDR','XFEREXEPARMS','IMG2DCMEXEPATH','PDF2IMGEXEPATH','DOCDCMPATH','USRUPDURL','XFEREXEPARMSJPGLL','XFEREXEPARMJ2KLL','XFEREXEPARMJ2KLS','XFEREXEPATHALT','TEMPDCMATCHPATH')
			select field_code from sys_pacs_query_fields where service_id = @service_id order by display_index
		end
	else if(@service_id =3)
		begin
			select control_code,data_type_string from general_settings where control_code in ('WS8SRVIP','WS8CLTIP','APIVER','WS8SRVUID','WS8SRVPWD','NEWDATAURL','RPTFETCHURL','PACIMGVWRURL','PACLOGINURL','PACSRPTVWRURL','PACSUSERID','PACSUSERPWD')
			select field_code from sys_pacs_query_fields where service_id = @service_id order by display_index
		end
	else if(@service_id =4)
		begin
			select control_code,data_type_string,data_type_number 
			from general_settings 
			where control_code in ('MAILSENDER','MAILSSLENABLED','MAILSVRNAME','MAILSVRPORT','MAILSVRUSRCODE','MAILSVRUSRPWD','SMSSENDERNO','SMSACCTSID','SMSAUTHTKNNO','INVMAILFOLDER','RPTSRVURL','RPTSRVFLDR','FTPDLFLDRTMP','FAXAPIURL','FAXAUTHUSERID','FAXAUTHPWD','FAXCSID','FAXREFTEXT','FAXREPADDR','FAXCONTACT','FAXRETRY','FAXFILEFLDR','SCHCASVCENBL')
		end
	else if(@service_id =5)
		begin
			select control_code,data_type_string,data_type_number 
			from general_settings 
			where control_code in ('DOCDCMPATH','FTPDLFLDRTMP','INVMAILFOLDER','WS8SRVIP','WS8CLTIP','WS8SRVUID','WS8SRVPWD','TEMPDCMATCHPATH','PACSARCHIVEFLDR','PACSARCHALTFLDR','DCMRCVRFLDR')
		end
	else if(@service_id =6)
		begin
			select control_code,data_type_string from general_settings where control_code  in ('WS8SRVIP','WS8CLTIP','APIVER','WS8SRVUID','WS8SRVPWD')
			select field_code from sys_pacs_query_fields where service_id = @service_id order by display_index
		end
	else if(@service_id =7)
		begin
			select control_code,data_type_string,data_type_number 
			from general_settings 
			where control_code in ('FTPHOST','FTPPORT','FTPUSER','FTPPWD','FTPFLDR','FTPDLFLDRTMP','FTPLOGFLDR','FTPLOGDLFLDRTMP','FTPDLMODE','FTPSRCFOLDER','PACSXFERDLFLDR','XFEREXEPATH','XFEREXEPATHALT','XFEREXEPARMS','XFEREXEPARMSJPGLL','XFEREXEPARMJ2KLL','XFEREXEPARMJ2KLS','XFEREXEPARMSSENDDCM','PACSARCHIVEFLDR','WRITEBACKURL','IMG2DCMEXEPATH','DCMMODIFYEXEPATH','DCMRCVRFLDR','FILESHOLDPATH','SENDLFTOPACS','DCMDMPEXEPATH','LFPATH')
			select field_code from sys_pacs_query_fields where service_id = @service_id order by display_index
		end
	else if(@service_id =9)
		begin
			select control_code,data_type_string,data_type_number 
			from general_settings 
			where control_code in ('SCHCASVCENBL')
		end
	else if(@service_id =10)
		begin
			select control_code,data_type_string,data_type_number 
			from general_settings 
			where control_code in ('DCMMODIFYEXEPATH','DCMDMPEXEPATH','DCMRCVEXEPATH','DCMLSNFLDR1','DCMLSNFLDR2','DCMLSNFLDR3','DCMLSNFLDR4','FILESHOLDPATH','RCVSYNTAX1','RCVSYNTAX2','RCVSYNTAX3','RCVSYNTAX4','PACSARCHIVEFLDR','ENBLLDS','DSNOOFPORTS','UMDCMFILES')
		end
    else if(@service_id =11)
		begin
			select control_code,data_type_string,data_type_number 
			from general_settings 
			where control_code in ('DCMRCVRFLDR','DCMDMPEXEPATH','XFEREXEPATH','XFEREXEPATHALT','XFEREXEPARMS','XFEREXEPARMSJPGLL','XFEREXEPARMJ2KLL','XFEREXEPARMJ2KLS','XFEREXEPARMSSENDDCM','PACSXFERDLFLDR','ENBLLSPACSXFER','UMDCMFILES')
		end

	set nocount off
end


GO
