USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_radiologist_payment_dtls_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_radiologist_payment_dtls_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_radiologist_payment_dtls_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_radiologist_payment_dtls_fetch : fetch 
                  radiologist payment details
** Created By   : Pavel Guha
** Created On   : 28/01/2020
*******************************************************/
--exec rpt_radiologist_payment_dtls_fetch '586953d4-2b3d-44dd-9349-b1c1b2701246','ECF1FA18-7B5E-4295-A113-C32AB42171EC'
CREATE procedure [dbo].[rpt_radiologist_payment_dtls_fetch]
	@billing_cycle_id uniqueidentifier,
	@radiologist_id uniqueidentifier
as
begin
	 set nocount on

	 create table #tmp
	 (
		modality_id int,
		modality_name nvarchar(30),
		total_study_count_prelim_std int default 0,
	    total_study_count_prelim_stat int default 0,
		total_study_count_final_std int default 0,
	    total_study_count_final_stat int default 0,
		total_study_count int null default 0,
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
	 set total_study_count_prelim_std = isnull((select count(id)
											   from ap_radiologist_payment_dtls
											   where modality_id = #tmp.modality_id
											   and priority_id = 20
											   and is_reading_prelim='Y'
											   and radiologist_id = @radiologist_id
											   and billing_cycle_id=@billing_cycle_id),0)

	
	update #tmp
	set total_study_count_prelim_stat = isnull((select count(id)
											   from ap_radiologist_payment_dtls
											   where modality_id = #tmp.modality_id
											   and priority_id = 10
											   and is_reading_prelim='Y'
											   and radiologist_id = @radiologist_id
											   and billing_cycle_id=@billing_cycle_id),0)

	update #tmp
	set total_study_count_final_std = isnull((select count(id)
											   from ap_radiologist_payment_dtls
											   where modality_id = #tmp.modality_id
											   and priority_id = 20
											   and is_reading_final='Y'
											   and radiologist_id = @radiologist_id
											   and billing_cycle_id=@billing_cycle_id),0)

	
	update #tmp
	set total_study_count_final_stat = isnull((select count(id)
											   from ap_radiologist_payment_dtls
											   where modality_id = #tmp.modality_id
											   and priority_id = 10
											   and is_reading_final='Y'
											   and radiologist_id = @radiologist_id
											   and billing_cycle_id=@billing_cycle_id),0)

	 update #tmp
	 set total_study_count = total_study_count_prelim_std + total_study_count_prelim_stat + total_study_count_final_std + total_study_count_final_stat




	 
	update #tmp 
    set total_std_amount =  isnull((select sum(prelim_amount + final_amount)
							   from ap_radiologist_payment_dtls
							   where modality_id = #tmp.modality_id
							   and priority_id = 20
							   and radiologist_id = @radiologist_id
							   and billing_cycle_id=@billing_cycle_id),0)

	
	update #tmp 
    set total_stat_amount =  isnull((select sum(prelim_amount + final_amount)
							   from ap_radiologist_payment_dtls
							   where modality_id = #tmp.modality_id
							   and priority_id = 10
							   and radiologist_id = @radiologist_id
							   and billing_cycle_id=@billing_cycle_id),0)

	update #tmp 
    set total_adhoc_amount =  isnull((select sum(adhoc_amount)
							   from ap_radiologist_payment_dtls
							   where modality_id = #tmp.modality_id
							   and radiologist_id = @radiologist_id
							   and billing_cycle_id=@billing_cycle_id),0)

   update #tmp 
   set total_amount =  isnull((select sum(total_amount)
							   from ap_radiologist_payment_dtls
							   where modality_id = #tmp.modality_id
							   and radiologist_id = @radiologist_id
							   and billing_cycle_id=@billing_cycle_id),0)


	select * from #tmp order by modality_name

	drop table #tmp

	set nocount off
end

GO
