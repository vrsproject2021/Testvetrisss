USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_opening_balance_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_opening_balance_fetch_params]
GO
/****** Object:  StoredProcedure [dbo].[ar_opening_balance_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_opening_balance_fetch_params 
** Created By   : KC
** Created On   : 30/04/2020
*******************************************************/
CREATE procedure [dbo].[ar_opening_balance_fetch_params]
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	 set nocount on

	
	
	select id, [name] from billing_account where is_active='Y' order by [name];

	select  [ar_payments_year] [year_value] from sys_ar_payments_params order by [ar_payments_year];

	set nocount off
end

GO
