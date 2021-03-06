USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ap_radiologist_payment_view_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ap_radiologist_payment_view_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ap_radiologist_payment_view_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ap_radiologist_payment_view_fetch : 
                  fetch radiologist payment details 
				  if locked by other user
** Created By   : Pavel Guha 
** Created On   : 28/01/2020
*******************************************************/
--exec ap_radiologist_payment_view_fetch 'CD6E2FE4-9AC6-4010-9A9B-08EB687B09DD','BE7CFCBE-BF98-407B-9FB4-660F7534FA4B'
--exec ap_radiologist_payment_view_fetch 'CD6E2FE4-9AC6-4010-9A9B-08EB687B09DD','00000000-0000-0000-0000-000000000000'
CREATE procedure [dbo].[ap_radiologist_payment_view_fetch]
	@billing_cycle_id uniqueidentifier,
	@radiologist_id uniqueidentifier='00000000-0000-0000-0000-000000000000'
as
	begin
		set nocount on

		if(@radiologist_id ='00000000-0000-0000-0000-000000000000')
			begin
				select rph.radiologist_id,
					   rph.billing_cycle_id,
					   radiologist_name = dbo.InitCap(r.name),
					   rph.total_study_count_prelim,
					   rph.total_study_count_final,
					   rph.total_amount,
					   rph.approved
				from ap_radiologist_payment_hdr rph
				inner join radiologists r on r.id = rph.radiologist_id
				where billing_cycle_id = @billing_cycle_id
				order by r.name

				---*********************
				select row_number() over(order by radiologist_id,received_date) as row_id,
					   radiologist_id,
					   billing_cycle_id,
					   study_id,
					   study_uid,
					   received_date,
					   modality_name,
					   priority_desc,
					   institution_name,
					   patient_name,
					   is_reading_prelim,
					   is_reading_final,
					   prelim_amount,
					   final_amount,
					   adhoc_amount,
					   total_amount,
					   custom_report
				from
				(select rpd.radiologist_id,
					   rpd.billing_cycle_id,
					   rpd.study_id,
					   rpd.study_uid,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   priority_desc = dbo.InitCap(isnull(p.priority_desc,'Unkown')),
					   institution_name=i.name,
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   rpd.is_reading_prelim,
					   rpd.is_reading_final,
					   rpd.prelim_amount,
					   rpd.final_amount,
					   rpd.adhoc_amount,
					   rpd.total_amount,
					   custom_report = isnull(i.custom_report,'N')
				from ap_radiologist_payment_dtls rpd
				left outer join modality m on m.id= rpd.modality_id
				left outer join sys_priority p on p.priority_id = rpd.priority_id
				inner join study_hdr sh on sh.id = rpd.study_id
				inner join institutions i on i.id = sh.institution_id
				where rpd.billing_cycle_id = @billing_cycle_id
				union
				select rpd.radiologist_id,
					   rpd.billing_cycle_id,
					   rpd.study_id,
					   rpd.study_uid,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   priority_desc = dbo.InitCap(isnull(p.priority_desc,'Unkown')),
					   institution_name=i.name,
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   rpd.is_reading_prelim,
					   rpd.is_reading_final,
					   rpd.prelim_amount,
					   rpd.final_amount,
					   rpd.adhoc_amount,
					   rpd.total_amount,
					   custom_report = isnull(i.custom_report,'N')
				from ap_radiologist_payment_dtls rpd
				left outer join modality m on m.id= rpd.modality_id
				left outer join sys_priority p on p.priority_id = rpd.priority_id
				inner join study_hdr_archive sh on sh.id = rpd.study_id
				inner join institutions i on sh.institution_id=i.id
			    where rpd.billing_cycle_id = @billing_cycle_id) t
				---**************
				
			end
		else
			begin
				select rph.radiologist_id,
					   rph.billing_cycle_id,
					   radiologist_name = dbo.InitCap(r.name),
					   rph.total_study_count_prelim,
					   rph.total_study_count_final,
					   rph.total_amount,
					   rph.approved
				from ap_radiologist_payment_hdr rph
				inner join radiologists r on r.id = rph.radiologist_id
				where billing_cycle_id		= @billing_cycle_id
				  and rph.radiologist_id	= @radiologist_id
				order by r.name

				select row_number() over(order by radiologist_id,received_date) as row_id,
					   radiologist_id,
					   billing_cycle_id,
					   study_id,
					   study_uid,
					   received_date,
					   modality_name,
					   priority_desc,
					   institution_name,
					   patient_name,
					   is_reading_prelim,
					   is_reading_final,
					   prelim_amount,
					   final_amount,
					   adhoc_amount,
					   total_amount,
					   custom_report
				from
				(select rpd.radiologist_id,
					   rpd.billing_cycle_id,
					   rpd.study_id,
					   rpd.study_uid,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   priority_desc = dbo.InitCap(isnull(p.priority_desc,'Unkown')),
					   institution_name=i.name,
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   rpd.is_reading_prelim,
					   rpd.is_reading_final,
					   rpd.prelim_amount,
					   rpd.final_amount,
					   rpd.adhoc_amount,
					   rpd.total_amount,
					   custom_report = isnull(i.custom_report,'N')
				from ap_radiologist_payment_dtls rpd
				left outer join modality m on m.id= rpd.modality_id
				left outer join sys_priority p on p.priority_id = rpd.priority_id
				inner join study_hdr sh on sh.id = rpd.study_id
				inner join institutions i on i.id =  sh.institution_id
				where rpd.billing_cycle_id	= @billing_cycle_id
				and rpd.radiologist_id	= @radiologist_id
				union
				select rpd.radiologist_id,
					   rpd.billing_cycle_id,
					   rpd.study_id,
					   rpd.study_uid,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   priority_desc = dbo.InitCap(isnull(p.priority_desc,'Unkown')),
					   institution_name=i.name,
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   rpd.is_reading_prelim,
					   rpd.is_reading_final,
					   rpd.prelim_amount,
					   rpd.final_amount,
					   rpd.adhoc_amount,
					   rpd.total_amount,
					   custom_report = isnull(i.custom_report,'N')
				from ap_radiologist_payment_dtls rpd
				left outer join modality m on m.id= rpd.modality_id
				left outer join sys_priority p on p.priority_id = rpd.priority_id
				inner join study_hdr_archive sh on sh.id = rpd.study_id
				inner join institutions i on i.id = sh.institution_id
				where rpd.billing_cycle_id	= @billing_cycle_id
				and rpd.radiologist_id	= @radiologist_id) t
				

			end

		set nocount off

	end
GO
