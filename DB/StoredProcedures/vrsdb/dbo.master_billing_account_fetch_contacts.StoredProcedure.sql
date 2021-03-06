USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_fetch_contacts]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_billing_account_fetch_contacts]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_fetch_contacts]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_billing_account_fetch_contacts : fetch 
                  contactc
** Created By   : Pavel Guha
** Created On   : 04/11/2019
*******************************************************/
--exec master_billing_account_fetch_contacts '0ADC6D71-F7A1-4C7E-A49F-00BAA3365A52'
--exec master_billing_account_fetch_contacts '00000000-0000-0000-0000-000000000000'
CREATE procedure [dbo].[master_billing_account_fetch_contacts]
    @id uniqueidentifier
as
begin
	 set nocount on

	  create table #tmp
	(
		rec_id int identity(1,1),
		institution_id uniqueidentifier,
		institution_name nvarchar(100),
		phone_no nvarchar(30),
		fax_no nvarchar(20),
		contact_person_name nvarchar(100),
		contact_person_mobile nvarchar(20),
		contact_person_email_id nvarchar(50)
		
	)


	insert into #tmp(institution_id,institution_name,phone_no,fax_no,contact_person_name,contact_person_mobile,contact_person_email_id)
	(select bac.institution_id,i.name,bac.phone_no,bac.fax_no,bac.contact_person_name,bac.contact_person_mobile,bac.contact_person_email_id
	from billing_account_contacts bac
	inner join institutions i on i.id = bac.institution_id
	where bac.billing_account_id=@id
	--union
	--select institution_id=id,name,phone_no=isnull(phone_no,''),fax_no =isnull(mobile_no,''),contact_person_name=isnull(contact_person_name,''),contact_person_mobile=isnull(contact_person_mobile,''),contact_person_email_id=isnull(email_id,'')
	--from institutions
	--where is_active='Y'
	--and billing_account_id=@id
	--and id not in (select institution_id from billing_account_institution_link)
	)
	
	order by name

	
	select * from #tmp

	drop table #tmp
		
	set nocount off
end

GO
