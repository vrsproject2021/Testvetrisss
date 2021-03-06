USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_promotion_billing_account_fetch_institutions]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_promotion_billing_account_fetch_institutions]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_promotion_billing_account_fetch_institutions]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_promotion_billing_account_fetch_institutions : fetch 
                  billing account Institutions
** Created By   : BK
** Created On   : 26/11/2019
*******************************************************/
--exec invoicing_promotion_billing_account_fetch_institutions '21187A6F-1173-4F82-B460-25FD2421D01A'
--exec invoicing_promotion_billing_account_fetch_institutions '00000000-0000-0000-0000-000000000000'
CREATE procedure [dbo].[invoicing_promotion_billing_account_fetch_institutions]
    @id uniqueidentifier
as
begin
	 set nocount on
	declare @count1 int,
	        @count2 int
	
	 create table #tmpInst
	 (
		promo_inst_id uniqueidentifier,
		billing_account_id uniqueidentifier,
		institution_id uniqueidentifier,
		code nvarchar(5),
		name nvarchar(100),
		sel char(1) default 'Y'
	 )

	 insert into #tmpInst(promo_inst_id,billing_account_id,institution_id,code,name,sel)
	(select promo_inst_id=api.id,api.billing_account_id, api.institution_id,i.code,i.name,sel='Y'
	from ar_promotion_institution api
	inner join institutions i on i.id=api.institution_id
	where api.billing_account_id=@id
	and i.is_active='Y'
	union
	select promo_inst_id='00000000-0000-0000-0000-000000000000', inst.billing_account_id, institution_id =inst.id,inst.code,inst.name,sel='N'
	from institutions inst
	where is_active='Y'
	and billing_account_id=@id
	and id not in (select institution_id from ar_promotion_institution where billing_account_id=@id))
	order by sel desc,name

	select @count1 = count(promo_inst_id) from #tmpInst
	select @count2 = count(promo_inst_id) from #tmpInst where promo_inst_id='00000000-0000-0000-0000-000000000000'

	if(@count1 = @count2)
		begin
			update #tmpInst set sel='Y'
		end
	
	
	select * from #tmpInst
		
	

	drop table #tmpInst
	set nocount off
end

GO
