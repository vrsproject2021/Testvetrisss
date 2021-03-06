USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_promotions]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_institution_fetch_promotions]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_promotions]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_institution_fetch_promotions : fetch promotions browser
** Created By   : Pavel Guha
** Created On   : 01/01/2020
*******************************************************/
create procedure [dbo].[master_institution_fetch_promotions]
	@id	uniqueidentifier
as
	begin
		set nocount on
		

		select  ap.id,
				created_by = u.name ,
				ap.date_created,
				ap.promotion_type,
				case when ap.promotion_type='D' then 'Discount' when promotion_type='F' then 'Free Credit' end  promotion_type_desc,
				api.discount_percent,
				api.free_credits,
				ap.valid_from,
				ap.valid_till,
				pr.reason,
				case when ap.is_active='Y' then 'Yes' else 'No' end  is_active
		from ar_promotion_institution api
		inner join ar_promotions ap on ap.id= api.hdr_id
		inner join promo_reasons pr on pr.id = ap.reason_id
		inner join users u on u.id = ap.created_by
		where api.institution_id=@id
		order by ap.valid_till desc
		
		set nocount off
	end
GO
