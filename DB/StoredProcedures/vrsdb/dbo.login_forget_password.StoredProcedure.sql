USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[login_forget_password]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[login_forget_password]
GO
/****** Object:  StoredProcedure [dbo].[login_forget_password]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : login_forget_password 
** Created By   : Rajdeep Paul
** Created On   : 03/06/2019
*******************************************************/
-- exec login_validate 'ADMIN','L3FU1aoWCoQ=','22222222-2222-2222-2222-222222222222','','',0
create procedure [dbo].[login_forget_password]
    @login_id nvarchar(50),
	@error_code nvarchar(10)='' output,
	@return_status int =0 output
as
begin
	declare @userPwd nvarchar(50)

	select @userPwd = isnull(password,'') from users where login_id=@login_id
	 if (@userPwd<>'')
		 begin
			select control_code,data_type_number,data_type_string from general_settings where control_code in ('MAILSVRNAME','MAILSVRPORT','MAILSVRUSRCODE','MAILSVRUSRPWD','MAILSSLENABLED','MAILSENDER')
			select  password,name from users where login_id=@login_id
			select @error_code='',@return_status=1
			return 1
		 end
	 else
		  begin
				select @error_code='000',@return_status=0
				return 0
		  end

	
end

GO
