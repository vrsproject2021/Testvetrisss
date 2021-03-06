USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ap_transcriptionist_payment_details_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ap_transcriptionist_payment_details_fetch_params]
GO
/****** Object:  StoredProcedure [dbo].[ap_transcriptionist_payment_details_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ap_transcriptionist_payment_details_fetch_params : fetch transcriptionist payment
				  details parameters 
** Created By   : Pavel Guha
** Created On   : 26/10/2020
*******************************************************/
create procedure [dbo].[ap_transcriptionist_payment_details_fetch_params] 
as
begin
	set nocount on
	
	select id,name,date_from from billing_cycle order by date_from desc
	select id,name from transciptionists where is_active='Y' order by name

	set nocount off
end


GO
