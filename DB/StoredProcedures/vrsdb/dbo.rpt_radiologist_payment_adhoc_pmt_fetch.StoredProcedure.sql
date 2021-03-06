USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_radiologist_payment_adhoc_pmt_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_radiologist_payment_adhoc_pmt_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_radiologist_payment_adhoc_pmt_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_radiologist_payment_adhoc_pmt_fetch : fetch 
                  radiologist payment details
** Created By   : Pavel Guha
** Created On   : 06/11/2020
*******************************************************/
--exec rpt_radiologist_payment_adhoc_pmt_fetch '397F7134-F742-45F3-924B-9A9F77EC20DC','ECF1FA18-7B5E-4295-A113-C32AB42171EC'
create procedure [dbo].[rpt_radiologist_payment_adhoc_pmt_fetch]
	@billing_cycle_id uniqueidentifier,
	@radiologist_id uniqueidentifier
as
begin
	 set nocount on

	 
	 select oap.head_id,aph.name,oap.adhoc_payment
	 from ap_radiologist_other_adhoc_payments oap
	 inner join ap_adhoc_payment_heads aph on aph.id = oap.head_id
	 where oap.radiologist_id=@radiologist_id
	 and oap.billing_cycle_id=@billing_cycle_id
	 

	set nocount off
end

GO
