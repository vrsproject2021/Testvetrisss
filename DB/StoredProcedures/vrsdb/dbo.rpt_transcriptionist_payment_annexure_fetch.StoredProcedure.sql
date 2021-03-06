USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_transcriptionist_payment_annexure_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_transcriptionist_payment_annexure_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_transcriptionist_payment_annexure_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_transcriptionist_payment_annexure_fetch : fetch 
                  transcriptionist payment annexure
** Created By   : Pavel Guha
** Created On   : 27/10/2020
*******************************************************/
--exec rpt_transcriptionist_payment_annexure_fetch '3ACFB756-45AF-424C-80E5-5B66406E08A6','26EE5523-976A-4BE3-83E9-46D04FD79079'
create procedure [dbo].[rpt_transcriptionist_payment_annexure_fetch]
	@billing_cycle_id uniqueidentifier,
	@transcriptionist_id uniqueidentifier
as
begin
	 set nocount on

	 create table #tmp
	 (
		rec_id int identity(1,1),
	    study_id uniqueidentifier,
		received_date datetime,
		patient_name nvarchar(250),
		modality_name nvarchar(30),
		priority_desc nvarchar(30),
		institution_name nvarchar(100),
		amount money,
		addl_stat_rate money,
		adhoc_amount money,
		total_amount money
	 )

	 insert into #tmp(study_id,received_date,patient_name,modality_name,priority_desc,institution_name,
	                  amount,addl_stat_rate,adhoc_amount,total_amount)
	 (select tpd.study_id,sh.received_date,
	         patient_name = dbo.InitCap(sh.patient_name),
		     modality_name = dbo.InitCap(isnull(m.name,'')),
		     priority_desc = isnull(p.priority_desc,''),
		     institution_name = dbo.InitCap(isnull(i.name,'')),
			 tpd.amount, tpd.addl_stat_rate,tpd.adhoc_amount,tpd.total_amount
	 from ap_transcriptionist_payment_dtls tpd
	 inner join study_hdr sh on sh.id = tpd.study_id
	 left outer join modality m on m.id = sh.modality_id
	 inner join sys_priority p on p.priority_id = sh.priority_id
	 inner join institutions i on i.id = sh.institution_id
	 where tpd.billing_cycle_id=@billing_cycle_id
	 and tpd.transcriptionist_id=@transcriptionist_id
	 union
	 select tpd.study_id,sh.received_date,
	         patient_name = dbo.InitCap(sh.patient_name),
		     modality_name = dbo.InitCap(isnull(m.name,'')),
		     priority_desc = isnull(p.priority_desc,''),
		     institution_name = dbo.InitCap(isnull(i.name,'')),
			 tpd.amount, tpd.addl_stat_rate,tpd.adhoc_amount,tpd.total_amount
	 from ap_transcriptionist_payment_dtls tpd
	 inner join study_hdr_archive sh on sh.id = tpd.study_id
	 left outer join modality m on m.id = sh.modality_id
	 inner join sys_priority p on p.priority_id = sh.priority_id
	 inner join institutions i on i.id = sh.institution_id
	 where tpd.billing_cycle_id=@billing_cycle_id
	 and tpd.transcriptionist_id=@transcriptionist_id)
	 order by received_date

	

	 select * from #tmp order by received_date,patient_name

	 drop table #tmp

	set nocount off
end

GO
