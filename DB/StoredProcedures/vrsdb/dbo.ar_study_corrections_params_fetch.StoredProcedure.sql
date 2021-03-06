USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_study_corrections_params_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_study_corrections_params_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_study_corrections_params_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_study_corrections_params_fetch : fetch study amendment
                  parameters 
** Created By   : Pavel Guha
** Created On   : 19/04/2019
*******************************************************/
--exec ar_study_corrections_params_fetch '11111111-1111-1111-1111-111111111111'
CREATE PROCEDURE [dbo].[ar_study_corrections_params_fetch] 
	@user_id uniqueidentifier,
	@billing_account_id uniqueidentifier = '00000000-0000-0000-0000-000000000000'
as
begin
	set nocount on

	declare @user_role_id int,
	        @user_role_code nvarchar(10)

	select @user_role_id = u.user_role_id,
	       @user_role_code = ur.code
	from users u
	inner join user_roles ur on ur.id =u.user_role_id
	where u.id = @user_id

	select id,name from billing_cycle order by date_from desc

	if(@user_role_code = 'SUPP' or @user_role_code = 'SYSADMIN')
		begin
			if(isnull(@billing_account_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000')
				begin
					select id,name= dbo.InitCap(name) from institutions where is_active='Y' order by name
			    end
			else
				begin
					select id,name= dbo.InitCap(name) from institutions where is_active='Y' and billing_account_id=@billing_account_id order by name
				end
		end
	else if(@user_role_code = 'IU')
		begin
			if(isnull(@billing_account_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000')
				begin
					select id,name = dbo.InitCap(name)
					from institutions 
					where is_active='Y'
					and id in (select institution_id
							   from institution_user_link
							   where user_id = @user_id   ) 
					order by name
				end
			else
				begin
					select id,name = dbo.InitCap(name)
					from institutions 
					where is_active='Y'
					and billing_account_id=@billing_account_id
					and id in (select institution_id
							   from institution_user_link
							   where user_id = @user_id   ) 
					order by name
				end
		end
	else if(@user_role_code = 'AU')
		begin
			if(isnull(@billing_account_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000')
				begin
					select id = bail.institution_id,name= dbo.InitCap(i.name)
					from billing_account_institution_link bail
					inner join institutions i on i.id = bail.institution_id
					inner join billing_account ba on ba.id = bail.billing_account_id
					where i.is_active='Y'
					and ba.login_user_id = @user_id
					order by i.name
			   end
			else
				begin
					select id = bail.institution_id,name= dbo.InitCap(i.name)
					from billing_account_institution_link bail
					inner join institutions i on i.id = bail.institution_id
					inner join billing_account ba on ba.id = bail.billing_account_id
					where i.is_active='Y'
					and bail.billing_account_id=@billing_account_id
					and ba.login_user_id = @user_id
					order by i.name
				end
		end
	else if(@user_role_code = 'SALES')
		begin
			if(isnull(@billing_account_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000')
				begin
					select id,name = dbo.InitCap(name)
					from institutions 
					where is_active='Y'
					and id in (select institution_id
							   from institution_salesperson_link
							   where salesperson_user_id = @user_id   ) 
					order by name
			   end
			else
				begin
					select id,name = dbo.InitCap(name)
					from institutions 
					where is_active='Y'
					and billing_account_id=@billing_account_id
					and id in (select institution_id
							   from institution_salesperson_link
							   where salesperson_user_id = @user_id   ) 
					order by name
				end
		end

	select id,name= dbo.InitCap(name) from modality where is_active='Y' order by name
	select priority_id,priority_desc from sys_priority where is_active='Y' order by priority_desc
	
	select control_code,
	       data_type_string
	from general_settings
	where control_code in ('APIVER','WS8SRVIP','WS8CLTIP','WS8SRVUID','WS8SRVPWD','PACSTUDYDELURL')

	select id,name= dbo.InitCap(name) from sys_study_category order by name


	set nocount off
end


GO
