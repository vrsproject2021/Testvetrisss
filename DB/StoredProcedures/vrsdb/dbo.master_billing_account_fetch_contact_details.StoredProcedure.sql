USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_fetch_contact_details]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_billing_account_fetch_contact_details]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_fetch_contact_details]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_billing_account_fetch_contact_details : fetch 
                  contact details
** Created By   : Pavel Guha
** Created On   : 04/11/2019
*******************************************************/
--exec master_billing_account_fetch_contact_details '0ADC6D71-F7A1-4C7E-A49F-00BAA3365A52'
create procedure [dbo].[master_billing_account_fetch_contact_details]
	@institution_id uniqueidentifier
as
begin
	 set nocount on

	
	select institution_id=id,institution_name=name,phone_no,fax_no = mobile_no,contact_person_name,contact_person_mobile,contact_person_email_id = email_id
	from institutions
	where id = @institution_id


	
	set nocount off
end

GO
