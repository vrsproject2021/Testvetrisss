USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_details_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_details_fetch_params]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_details_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_details_fetch_params : fetch invoicing
				  details parameters 
** Created By   : Pavel Guha
** Created On   : 19/04/2019
*******************************************************/
CREATE procedure [dbo].[invoicing_details_fetch_params] 
	@user_id uniqueidentifier
as
begin
	set nocount on

	delete from sys_record_lock where user_id=@user_id
	delete from sys_record_lock_ui where user_id= @user_id

	select id,name,date_from from billing_cycle order by date_from desc
	select id,name from billing_account where is_active='Y' order by name

	set nocount off
end


GO
