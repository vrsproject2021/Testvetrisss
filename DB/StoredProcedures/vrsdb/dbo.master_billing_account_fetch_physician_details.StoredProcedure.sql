USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_fetch_physician_details]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_billing_account_fetch_physician_details]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_fetch_physician_details]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_billing_account_fetch_physician_details : fetch 
                  physician details
** Created By   : Pavel Guha
** Created On   : 05/11/2019
*******************************************************/
--exec master_billing_account_fetch_physician_details 'CDA537C5-C73D-41D6-B01E-2FC0248DAF35'
create procedure [dbo].[master_billing_account_fetch_physician_details]
	@institution_id uniqueidentifier
as
begin
	 set nocount on

	
	select institution_id=id,code,name
	from institutions
	where id = @institution_id

	select institution_id,physician_id,physician_fname,physician_lname,physician_credentials,
	       physician_email,physician_mobile
	from institution_physician_link
	where institution_id=@institution_id


	
	set nocount off
end

GO
