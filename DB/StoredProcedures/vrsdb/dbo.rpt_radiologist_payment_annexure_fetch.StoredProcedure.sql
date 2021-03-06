USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_radiologist_payment_annexure_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_radiologist_payment_annexure_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_radiologist_payment_annexure_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_radiologist_payment_annexure_fetch : fetch 
                  radiologist payment annexure
** Created By   : Pavel Guha
** Created On   : 28/01/2020
*******************************************************/
--exec rpt_radiologist_payment_annexure_fetch '586953d4-2b3d-44dd-9349-b1c1b2701246','ECF1FA18-7B5E-4295-A113-C32AB42171EC'
CREATE procedure [dbo].[rpt_radiologist_payment_annexure_fetch]
	@billing_cycle_id uniqueidentifier,
	@radiologist_id uniqueidentifier
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
		is_prelim nvarchar(5),
		is_final nvarchar(5),
		institution_name nvarchar(100),
		prelim_amount money,
		final_amount money,
		adhoc_amount money,
		total_amount money
	 )

	 insert into #tmp(study_id,received_date,patient_name,modality_name,priority_desc,institution_name,
	                  is_prelim,is_final,prelim_amount,final_amount,adhoc_amount,total_amount)
	 (select rpd.study_id,sh.received_date,
	         patient_name = dbo.InitCap(sh.patient_name),
		     modality_name = dbo.InitCap(isnull(m.name,'')),
		     priority_desc = isnull(p.priority_desc,''),
		     institution_name = dbo.InitCap(isnull(i.name,'')),
		     case when rpd.is_reading_prelim ='Y' then 'Yes' else 'No' end is_prelim,
			 case when rpd.is_reading_final ='Y' then 'Yes' else 'No' end is_final,
			 rpd.prelim_amount, rpd.final_amount,rpd.adhoc_amount,rpd.total_amount
	 from ap_radiologist_payment_dtls rpd
	 inner join study_hdr sh on sh.id = rpd.study_id
	 left outer join modality m on m.id = sh.modality_id
	 inner join sys_priority p on p.priority_id = sh.priority_id
	 inner join institutions i on i.id = sh.institution_id
	 where rpd.billing_cycle_id=@billing_cycle_id
	 and rpd.radiologist_id=@radiologist_id
	 union
	 select rpd.study_id,sh.received_date,
	         patient_name = dbo.InitCap(sh.patient_name),
		     modality_name = dbo.InitCap(isnull(m.name,'')),
		     priority_desc = isnull(p.priority_desc,''),
		     institution_name = dbo.InitCap(isnull(i.name,'')),
		     case when rpd.is_reading_prelim ='Y' then 'Yes' else 'No' end is_prelim,
			 case when rpd.is_reading_final ='Y' then 'Yes' else 'No' end is_final,
			 rpd.prelim_amount, rpd.final_amount,rpd.adhoc_amount,rpd.total_amount
	 from ap_radiologist_payment_dtls rpd
	 inner join study_hdr_archive sh on sh.id = rpd.study_id
	 left outer join modality m on m.id = sh.modality_id
	 inner join sys_priority p on p.priority_id = sh.priority_id
	 inner join institutions i on i.id = sh.institution_id
	 where rpd.billing_cycle_id=@billing_cycle_id
	 and rpd.radiologist_id=@radiologist_id)
	 order by received_date

	

	 select * from #tmp order by received_date,patient_name

	 drop table #tmp

	set nocount off
end

GO
