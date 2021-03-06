USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_transcriptionist_transcriptionist_payment_hdr_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_transcriptionist_transcriptionist_payment_hdr_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_transcriptionist_transcriptionist_payment_hdr_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_transcriptionist_transcriptionist_payment_hdr_fetch : fetch 
                  transcriptionist payment header
** Created By   : Pavel Guha
** Created On   : 27/10/2020
*******************************************************/
--exec rpt_transcriptionist_transcriptionist_payment_hdr_fetch '3ACFB756-45AF-424C-80E5-5B66406E08A6','26EE5523-976A-4BE3-83E9-46D04FD79079'
create procedure [dbo].[rpt_transcriptionist_transcriptionist_payment_hdr_fetch]
	@billing_cycle_id uniqueidentifier,
	@transcriptionist_id uniqueidentifier
as
begin	
	 set nocount on

	

	 select transcriptionist_name= t.name,
	        payment_no      = isnull(rph.payment_no,'TBG'),
			payment_date    = isnull(rph.payment_date,getdate()),
			billing_cycle   = convert(varchar(10),bc.date_from,101) + ' - ' + convert(varchar(10),bc.date_till,101)
	 from ap_transcriptionist_payment_hdr rph
	 inner join transciptionists t on t.id = rph.transcriptionist_id
	 inner join billing_cycle bc on bc.id = rph.billing_cycle_id
	 where rph.transcriptionist_id = @transcriptionist_id
	 and rph.billing_cycle_id = @billing_cycle_id
		
	set nocount off
end

GO
