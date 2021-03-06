USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_notification_invoice_list_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_notification_invoice_list_fetch]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_notification_invoice_list_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_notification_invoice_list_fetch : 
                  fetch list of invoice(s) for mailing
** Created By   : Pavel Guha
** Created On   : 27/05/2020
*******************************************************/
-- exec scheduler_notification_invoice_list_fetch 
CREATE procedure [dbo].[scheduler_notification_invoice_list_fetch]

as
begin
	select ih.id,
		   ih.billing_account_id,
		   billing_account_code = ba.code,
		   billing_account_name = ba.name,
		   ih.billing_cycle_id,
		   billing_cycle_name= bc.name,
		   ih.invoice_no,
		   ih.invoice_date
	from invoice_hdr ih
	inner join billing_account ba on ba.id = ih.billing_account_id
	inner join billing_cycle bc on bc.id = ih.billing_cycle_id
	where isnull(ih.pick_for_mail,'N')='Y'
end

GO
