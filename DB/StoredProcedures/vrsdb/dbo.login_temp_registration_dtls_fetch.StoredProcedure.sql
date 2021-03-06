USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[login_temp_registration_dtls_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[login_temp_registration_dtls_fetch]
GO
/****** Object:  StoredProcedure [dbo].[login_temp_registration_dtls_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : get_registration_login : registration login
** Created By   : AM
** Created On   : 07/28/2020
*******************************************************/
-- exec login_validate 'I4PADM','Xrayadm','00000000-0000-0000-0000-000000000000','','','',0
CREATE procedure [dbo].[login_temp_registration_dtls_fetch]
    @registration_id uniqueidentifier= '00000000-0000-0000-0000-000000000000',
	@code varchar(5) ='' output,
	@name nvarchar(100) ='' output,
	@error_code nvarchar(10)='' output,
	@return_status int =0 output
as
begin
	
	declare @email_verified nchar(1)

	if (select count(id) from institutions_reg where id=@registration_id)=0
		begin
			select @error_code='369',@return_status=0 /*Illegal access not allowed*/
			return 0
		end

    select @code = code,
		   @name = login_id,
		   @email_verified=isnull(is_email_verified,'N')
	from institutions_reg 
	where id=@registration_id
    
	begin transaction
	
	if(@email_verified='N')
		begin
			--Update is_email_verified field
			update institutions_reg set is_email_verified = 'Y' where id=@registration_id
		end

	select @error_code='',@return_status=1
	commit transaction
	return 1

end

GO
