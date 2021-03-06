USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[mypayment_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[mypayment_fetch_params]
GO
/****** Object:  StoredProcedure [dbo].[mypayment_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : mypayment_fetch_params 
				  details parameters 
** Created By   : KC
** Created On   : 08/04/2020
*******************************************************/
CREATE procedure [dbo].[mypayment_fetch_params] 
(
	@user_id uniqueidentifier = null
)
as
	
begin
	set nocount on

	select 
		id,name 
	from billing_account 
	where is_active='Y'
		and login_user_id=case when @user_id is null then login_user_id else @user_id end 
	order by name; 

	select 
		id,name 
	from users 
	where is_active='Y'
		and id= @user_id 
	order by name;
	set nocount off
end


GO
