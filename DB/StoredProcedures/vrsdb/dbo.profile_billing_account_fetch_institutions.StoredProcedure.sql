USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[profile_billing_account_fetch_institutions]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[profile_billing_account_fetch_institutions]
GO
/****** Object:  StoredProcedure [dbo].[profile_billing_account_fetch_institutions]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : profile_billing_account_fetch_institutions : fetch 
                  billing account institutions
** Created By   : Pavel Guha
** Created On   : 17/02/2020
*******************************************************/
--exec profile_billing_account_fetch_institutions 'AB352297-C935-4E57-9CA9-D7C146B15914'
CREATE procedure [dbo].[profile_billing_account_fetch_institutions]
    @id uniqueidentifier
as
begin
	 set nocount on

	select bail.institution_id,i.code,i.name,
	case when i.consult_applicable='Y' then 'Yes' else 'No' end consult_applicable,
	case when i.storage_applicable='Y' then 'Yes' else 'No' end storage_applicable
	from billing_account_institution_link bail
	inner join institutions i on i.id=bail.institution_id
	where bail.billing_account_id=@id
	and i.is_active='Y'
	order by name
		
	set nocount off
end

GO
