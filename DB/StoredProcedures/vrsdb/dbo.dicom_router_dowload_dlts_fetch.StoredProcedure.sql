USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[dicom_router_dowload_dlts_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[dicom_router_dowload_dlts_fetch]
GO
/****** Object:  StoredProcedure [dbo].[dicom_router_dowload_dlts_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : dicom_router_dowload_dlts_fetch : fetch
                  dicom router's latest 
** Created By   : Pavel Guha
** Created On   : 18/01/2020
*******************************************************/
--exec dicom_router_dowload_dlts_fetch 'a54692e6-a88a-4a78-b9dd-fbf10d4aec4f'
CREATE procedure [dbo].[dicom_router_dowload_dlts_fetch]
	@user_id uniqueidentifier
as
begin
	declare @user_role_id int,
	        @user_role_code nvarchar(10),
			@institution_id uniqueidentifier,
			@billing_account_id uniqueidentifier

	select version_no
	from sys_dicom_router_version
	where date_released = (select MAX(date_released) from sys_dicom_router_version)

	select @user_role_id = u.user_role_id,
	       @user_role_code = ur.code
	from users u
	inner join user_roles ur on ur.id =u.user_role_id
	where u.id = @user_id

	
		print @user_role_code
	if(@user_role_code = 'IU')
		begin
			select @institution_id = institution_id from institution_user_link where user_id=@user_id
			select code,name from institutions where id = @institution_id
		end
	else if(@user_role_code = 'AU')
		begin
			select @billing_account_id = id from billing_account where login_user_id=@user_id

			select code,name 
			from institutions
			where billing_account_id = @billing_account_id
			and is_active='Y'
			order by name
		end
	else
		begin
			select code,name from institutions where is_active='Y' order by name
		end	

end
GO
