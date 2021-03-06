USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_promotion_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_promotion_fetch_params]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_promotion_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_promotion_fetch_params : fetch promotion
				  details parameters 
** Created By   : BK
** Created On   : 25/11/2019
*******************************************************/
CREATE procedure [dbo].[invoicing_promotion_fetch_params] 
(
	@menu_id int
)
as
	
begin
	set nocount on
	
	select u.id,u.name 
	from users u
		inner join user_menu_rights umr
		on umr.user_id=u.id  
		where umr.menu_id= 	@menu_id
	 order by name desc

	select id,name from billing_account where is_active='Y' order by name
	select id,reason from promo_reasons where is_active='Y' order by reason 

	set nocount off
end


GO
