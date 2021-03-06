USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_fetch_physicians]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_billing_account_fetch_physicians]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_fetch_physicians]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_billing_account_fetch_physicians : fetch 
                  billing account institutions
** Created By   : Pavel Guha
** Created On   : 22/10/2019
*******************************************************/
--exec master_billing_account_fetch_physicians '0ADC6D71-F7A1-4C7E-A49F-00BAA3365A52'
create procedure [dbo].[master_billing_account_fetch_physicians]
    @id uniqueidentifier
as
begin
	 set nocount on

	select bail.institution_id,i.code,i.name
	from billing_account_institution_link bail
	inner join institutions i on i.id=bail.institution_id
	where bail.billing_account_id=@id
	and i.is_active='Y'
	order by name


	select bap.institution_id,bap.physician_id,ipl.physician_fname,ipl.physician_lname,ipl.physician_credentials,
	       ipl.physician_email,ipl.physician_mobile
	from billing_account_physicians bap
	inner join institutions i on i.id=bap.institution_id
	inner join institution_physician_link ipl on ipl.institution_id = bap.institution_id and ipl.physician_id=bap.physician_id
	where bap.billing_account_id=@id
	and i.is_active='Y'
	order by ipl.physician_fname
		
	set nocount off
end

GO
