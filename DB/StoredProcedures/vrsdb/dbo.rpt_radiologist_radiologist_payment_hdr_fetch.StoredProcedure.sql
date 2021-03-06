USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_radiologist_radiologist_payment_hdr_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_radiologist_radiologist_payment_hdr_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_radiologist_radiologist_payment_hdr_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_radiologist_radiologist_payment_hdr_fetch : fetch 
                  radiologist payment header
** Created By   : Pavel Guha
** Created On   : 28/01/2020
*******************************************************/
--exec rpt_radiologist_radiologist_payment_hdr_fetch 'CD6E2FE4-9AC6-4010-9A9B-08EB687B09DD','C2AA3933-E9D0-48A8-9FA6-ECBC4DDA3ADB'
CREATE procedure [dbo].[rpt_radiologist_radiologist_payment_hdr_fetch]
	@billing_cycle_id uniqueidentifier,
	@radiologist_id uniqueidentifier
as
begin	
	 set nocount on

	

	 select radiologist_name= r.name,
	        payment_no      = isnull(rph.payment_no,'TBG'),
			payment_date    = isnull(rph.payment_date,getdate()),
			billing_cycle   = convert(varchar(10),bc.date_from,101) + ' - ' + convert(varchar(10),bc.date_till,101)
	 from ap_radiologist_payment_hdr rph
	 inner join radiologists r on r.id = rph.radiologist_id
	 inner join billing_cycle bc on bc.id = rph.billing_cycle_id
	 where rph.radiologist_id = @radiologist_id
	 and rph.billing_cycle_id = @billing_cycle_id
		
	set nocount off
end

GO
