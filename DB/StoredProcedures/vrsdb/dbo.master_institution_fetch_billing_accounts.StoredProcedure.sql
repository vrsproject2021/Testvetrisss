USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_billing_accounts]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_institution_fetch_billing_accounts]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_billing_accounts]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_institution_fetch_billing_accounts : fetch institution 
                  billing accounts
** Created By   : Pavel Guha
** Created On   : 04/11/2019
*******************************************************/
--exec master_institution_fetch_billing_accounts 'faded955-300e-4517-8f6c-e6513b1889f6'
create procedure [dbo].[master_institution_fetch_billing_accounts]
    @id uniqueidentifier
as
begin
	 set nocount on
	
	select id,name from billing_account where is_active = 'Y' order by name
	select billing_account_id from billing_account_institution_link where institution_id=@id
		
	set nocount off
end

GO
