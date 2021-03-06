USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[day_end_account_post_mail_sending_params_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[day_end_account_post_mail_sending_params_fetch]
GO
/****** Object:  StoredProcedure [dbo].[day_end_account_post_mail_sending_params_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : day_end_account_post_mail_sending_params_fetch : 
                  fetch  mail parameters 
** Created By   : Pavel Guha 
** Created On   : 27/12/2020
*******************************************************/

create procedure [dbo].[day_end_account_post_mail_sending_params_fetch]
	
as
	begin
		set nocount on

		select rec_email_id  = (select data_type_string from general_settings where control_code = 'DEACCTPOSTRCPT'),
		       mail_server      = (select data_type_string from general_settings where control_code='MAILSVRNAME'),
			   mail_server_port = (select data_type_number from general_settings where control_code='MAILSVRPORT'),
		       mail_user_code   = (select data_type_string from general_settings where control_code ='MAILSVRUSRCODE'),
			   mail_user_pwd    = (select data_type_string from general_settings where control_code ='MAILSVRUSRPWD'),
			   mail_ssl_enabled = (select data_type_string from general_settings where control_code='MAILSSLENABLED')

		set nocount off
		
	end
GO
