USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_fetch_institutions]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_billing_account_fetch_institutions]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_fetch_institutions]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_billing_account_fetch_institutions : fetch 
                  billing account billing_accounts
** Created By   : Pavel Guha
** Created On   : 22/10/2019
*******************************************************/
--exec master_billing_account_fetch_institutions 'AB352297-C935-4E57-9CA9-D7C146B15914'
CREATE procedure [dbo].[master_billing_account_fetch_institutions]
    @id uniqueidentifier
as
begin
	 set nocount on

	select bail.institution_id,i.code,i.name,i.consult_applicable,i.storage_applicable,sel='Y'
	from billing_account_institution_link bail
	inner join institutions i on i.id=bail.institution_id
	where bail.billing_account_id=@id
	and i.code is not null
	and i.is_active='Y'
	union
	select institution_id =id,code,name,consult_applicable,storage_applicable,sel='N'
	from institutions
	where is_active='Y'
	and code is not null
	and id not in (select institution_id from billing_account_institution_link)
	order by sel desc,name
		
	set nocount off
end

GO
