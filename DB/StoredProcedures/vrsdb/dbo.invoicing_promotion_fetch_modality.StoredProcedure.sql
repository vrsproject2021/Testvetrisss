USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_promotion_fetch_modality]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_promotion_fetch_modality]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_promotion_fetch_modality]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_promotion_free_credit_fetch_modality : fetch 
                 invoicing promotion free credit modality
** Created By   : BK
** Created On   : 26/11/2019
*******************************************************/
create procedure [dbo].[invoicing_promotion_fetch_modality]
    @id uniqueidentifier,
	@promo_type char(1)
as
begin
	 set nocount on
	 if(@promo_type = 'C')
		 begin
			select promo_mod_id = pm.id, pm.billing_account_id,pm.institution_id,pm.modality_id,m.code,m.name,
						free_credits=isnull(pm.free_credits,0)
			from ar_promotion_modality pm
			inner join modality m on m.id=pm.modality_id
			inner join ar_promotions ap on ap.billing_account_id=pm.billing_account_id
			where pm.billing_account_id=@id and ap.promo_type='C'
			union
			select promo_mod_id='00000000-0000-0000-0000-000000000000', billing_account_id='00000000-0000-0000-0000-000000000000',institution_id='00000000-0000-0000-0000-000000000000',m.id,m.code,m.name,free_credits=0
			from modality m
			where is_active='Y'
			and id not in (select distinct modality_id from ar_promotion_modality where billing_account_id=@id)
			order by name desc
		 end
		else if(@promo_type = 'D')
			 begin
			select promo_mod_id=pm.id, pm.billing_account_id,pm.institution_id,pm.modality_id,m.code,m.name,sel='Y'
			from ar_promotion_modality pm
			inner join modality m on m.id=pm.modality_id
			inner join ar_promotions ap on ap.billing_account_id=pm.billing_account_id
			where pm.billing_account_id=@id and ap.promo_type='D'
			union
			select promo_mod_id = '00000000-0000-0000-0000-000000000000', billing_account_id='00000000-0000-0000-0000-000000000000',institution_id='00000000-0000-0000-0000-000000000000',m.id,m.code,m.name,sel='N'
			from modality m
			where is_active='Y'
			and id not in (select distinct modality_id from ar_promotion_modality where billing_account_id=@id)
			order by name desc
		 end
	
		
	set nocount off
end

GO
