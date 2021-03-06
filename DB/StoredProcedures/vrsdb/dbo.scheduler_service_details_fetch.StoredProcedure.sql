USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_service_details_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_service_details_fetch]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_service_details_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_scheduler_service_details_fetch : fetch service details
** Created By   : Pavel Guha
** Created On   : 12/04/2019
*******************************************************/
--exec scheduler_service_details_fetch 2
CREATE procedure [dbo].[scheduler_service_details_fetch]
	@service_id int
as
begin
	
    select service_name,frequency
	from scheduler_data_services 
	where service_id=@service_id

	if(@service_id =1)
		begin
			select control_code,data_type_string from general_settings where control_code='NEWDATAURL'
			select field_code from sys_pacs_query_fields where service_id = @service_id order by display_index
		end
	else if(@service_id =2)
		begin
			select control_code,data_type_string from general_settings where control_code in ('WRITEBACKURL','XFEREXEPATH','XFEREXEPARMS','IMG2DCMEXEPATH','PDF2IMGEXEPATH','DOCDCMPATH','USRUPDURL','XFEREXEPARMSJPGLL','XFEREXEPARMJ2KLL','XFEREXEPARMJ2KLS')
			select field_code from sys_pacs_query_fields where service_id = @service_id order by display_index
		end
	else if(@service_id =3)
		begin
			select control_code,data_type_string from general_settings where control_code in ('NEWDATAURL','RPTFETCHURL','PACIMGVWRURL','PACLOGINURL','PACSRPTVWRURL','PACSUSERID','PACSUSERPWD')
			select field_code from sys_pacs_query_fields where service_id = @service_id order by display_index
		end
	else if(@service_id =4)
		begin
			select control_code,data_type_string,data_type_number 
			from general_settings 
			where control_code in ('MAILSENDER','MAILSSLENABLED','MAILSVRNAME','MAILSVRPORT','MAILSVRUSRCODE','MAILSVRUSRPWD','SMSSENDERNO','SMSACCTSID','SMSAUTHTKNNO')
		end
	else if(@service_id =5)
		begin
			select control_code,data_type_string,data_type_number 
			from general_settings 
			where control_code in ('DOCDCMPATH','FTPDLFLDRTMP')
		end
	else if(@service_id =6)
		begin
			select control_code,data_type_string from general_settings where control_code='NEWDATAURL'
			select field_code from sys_pacs_query_fields where service_id = @service_id order by display_index
		end
	else if(@service_id =7)
		begin
			select control_code,data_type_string,data_type_number 
			from general_settings 
			where control_code in ('FTPHOST','FTPPORT','FTPUSER','FTPPWD','FTPFLDR','FTPDLFLDRTMP','FTPLOGFLDR','FTPLOGDLFLDRTMP','FTPDLMODE','FTPSRCFOLDER','PACSXFERDLFLDR','XFEREXEPATH','XFEREXEPARMS','XFEREXEPARMSJPGLL','XFEREXEPARMJ2KLL','XFEREXEPARMJ2KLS','PACSARCHIVEFLDR','WRITEBACKURL','IMG2DCMEXEPATH','DCMMODIFYEXEPATH')
			select field_code from sys_pacs_query_fields where service_id = @service_id order by display_index
		end
end


GO
