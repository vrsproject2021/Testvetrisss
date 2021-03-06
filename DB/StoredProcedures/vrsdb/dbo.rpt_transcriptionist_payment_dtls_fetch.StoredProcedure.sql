USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_transcriptionist_payment_dtls_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_transcriptionist_payment_dtls_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_transcriptionist_payment_dtls_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_transcriptionist_payment_dtls_fetch : fetch 
                  transcriptionist payment details
** Created By   : Pavel Guha
** Created On   : 27/10/2020
*******************************************************/
--exec rpt_transcriptionist_payment_dtls_fetch '3ACFB756-45AF-424C-80E5-5B66406E08A6','26EE5523-976A-4BE3-83E9-46D04FD79079'
--exec rpt_transcriptionist_payment_dtls_fetch '3ACFB756-45AF-424C-80E5-5B66406E08A6','945EC5FA-FDD4-4BBE-813D-AD4EFBE9E1EF'
create procedure [dbo].[rpt_transcriptionist_payment_dtls_fetch]
	@billing_cycle_id uniqueidentifier,
	@transcriptionist_id uniqueidentifier
as
begin
	 set nocount on

	 create table #tmp
	 (
		modality_id int,
		modality_name nvarchar(30),
		total_study_count int default 0,
		total_study_count_std int default 0,
	    total_study_count_stat int default 0,
     	total_std_amount money null default 0,
		total_stat_amount money null default 0,
		total_adhoc_amount money null default 0,
		total_amount money null default 0
	 )

	 insert into #tmp(modality_id,modality_name) 
	 (select id,name = dbo.InitCap(name) 
	  from modality) 
	  order by name


	update #tmp
	set total_study_count_std = isnull((select count(id)
											   from ap_transcriptionist_payment_dtls
											   where modality_id = #tmp.modality_id
											   and priority_id = 20
											   and transcriptionist_id = @transcriptionist_id
											   and billing_cycle_id=@billing_cycle_id),0)

	
	update #tmp
	set total_study_count_stat = isnull((select count(id)
											   from ap_transcriptionist_payment_dtls
											   where modality_id = #tmp.modality_id
											   and priority_id = 10
											   and transcriptionist_id = @transcriptionist_id
											   and billing_cycle_id=@billing_cycle_id),0)

	 update #tmp
	 set total_study_count = total_study_count_std + total_study_count_stat



	 
	update #tmp 
    set total_std_amount =  isnull((select sum(amount)
							   from ap_transcriptionist_payment_dtls
							   where modality_id = #tmp.modality_id
							   and priority_id = 20
							   and transcriptionist_id = @transcriptionist_id
							   and billing_cycle_id=@billing_cycle_id),0)

	
	update #tmp 
    set total_stat_amount =  isnull((select sum(amount + addl_stat_rate)
							   from ap_transcriptionist_payment_dtls
							   where modality_id = #tmp.modality_id
							   and priority_id = 10
							   and transcriptionist_id = @transcriptionist_id
							   and billing_cycle_id=@billing_cycle_id),0)

	update #tmp 
    set total_adhoc_amount =  isnull((select sum(adhoc_amount)
							   from ap_transcriptionist_payment_dtls
							   where modality_id = #tmp.modality_id
							   and transcriptionist_id = @transcriptionist_id
							   and billing_cycle_id=@billing_cycle_id),0)

   update #tmp 
   set total_amount =  isnull((select sum(total_amount)
							   from ap_transcriptionist_payment_dtls
							   where modality_id = #tmp.modality_id
							   and transcriptionist_id = @transcriptionist_id
							   and billing_cycle_id=@billing_cycle_id),0)


	select * from #tmp order by modality_name

	drop table #tmp

	set nocount off
end

GO
