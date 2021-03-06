USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ap_radiologist_payment_details_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ap_radiologist_payment_details_fetch_params]
GO
/****** Object:  StoredProcedure [dbo].[ap_radiologist_payment_details_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ap_radiologist_payment_details_fetch_params : fetch radiologist payment
				  details parameters 
** Created By   : BK
** Created On   : 26/12/2019
*******************************************************/
create procedure [dbo].[ap_radiologist_payment_details_fetch_params] 
as
begin
	set nocount on
	
	select id,name,date_from from billing_cycle order by date_from desc
	select id,name from radiologists where is_active='Y' order by name

	set nocount off
end


GO
