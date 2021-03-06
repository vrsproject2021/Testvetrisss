USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ap_transcriptionist_payment_view_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ap_transcriptionist_payment_view_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ap_transcriptionist_payment_view_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ap_transcriptionist_payment_view_fetch : 
                  fetch transcriptionist payment details 
				  if locked by other user
** Created By   : Pavel Guha 
** Created On   : 28/01/2020
*******************************************************/
create procedure [dbo].[ap_transcriptionist_payment_view_fetch]
	@billing_cycle_id uniqueidentifier,
	@transcriptionist_id uniqueidentifier='00000000-0000-0000-0000-000000000000'
as
	begin
		set nocount on

		if(@transcriptionist_id ='00000000-0000-0000-0000-000000000000')
			begin
				select tph.transcriptionist_id,
					   tph.billing_cycle_id,
					   transcriptionist_name = dbo.InitCap(r.name),
					   tph.total_study_count,
					   tph.total_study_count_std,
					   tph.total_study_count_stat,
					   tph.total_amount,
					   tph.approved
				from ap_transcriptionist_payment_hdr tph
				inner join transciptionists r on r.id = tph.transcriptionist_id
				where billing_cycle_id = @billing_cycle_id
				order by r.name

				---*********************
				select row_number() over(order by transcriptionist_id,received_date) as row_id,
					   transcriptionist_id,
					   billing_cycle_id,
					   study_id,
					   study_uid,
					   received_date,
					   modality_name,
					   priority_desc,
					   institution_name,
					   patient_name,
					   amount,
					   adhoc_amount,
					   total_amount,
					   custom_report
				from
				(select tpd.transcriptionist_id,
					   tpd.billing_cycle_id,
					   tpd.study_id,
					   tpd.study_uid,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   priority_desc = dbo.InitCap(isnull(p.priority_desc,'Unkown')),
					   institution_name=i.name,
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   tpd.amount,
					   tpd.adhoc_amount,
					   tpd.total_amount,
					   custom_report = isnull(i.custom_report,'N')
				from ap_transcriptionist_payment_dtls tpd
				left outer join modality m on m.id= tpd.modality_id
				left outer join sys_priority p on p.priority_id = tpd.priority_id
				inner join study_hdr sh on sh.id = tpd.study_id
				inner join institutions i on i.id = sh.institution_id
				where tpd.billing_cycle_id = @billing_cycle_id
				union
				select tpd.transcriptionist_id,
					   tpd.billing_cycle_id,
					   tpd.study_id,
					   tpd.study_uid,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   priority_desc = dbo.InitCap(isnull(p.priority_desc,'Unkown')),
					   institution_name=i.name,
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   tpd.amount,
					   tpd.adhoc_amount,
					   tpd.total_amount,
					   custom_report = isnull(i.custom_report,'N')
				from ap_transcriptionist_payment_dtls tpd
				left outer join modality m on m.id= tpd.modality_id
				left outer join sys_priority p on p.priority_id = tpd.priority_id
				inner join study_hdr_archive sh on sh.id = tpd.study_id
				inner join institutions i on sh.institution_id=i.id
			    where tpd.billing_cycle_id = @billing_cycle_id) t
				---**************
				
			end
		else
			begin
				select tph.transcriptionist_id,
					   tph.billing_cycle_id,
					   transcriptionist_name = dbo.InitCap(r.name),
					   tph.total_study_count,
					   tph.total_study_count_std,
					   tph.total_study_count_stat,
					   tph.total_amount,
					   tph.approved
				from ap_transcriptionist_payment_hdr tph
				inner join transciptionists r on r.id = tph.transcriptionist_id
				where billing_cycle_id = @billing_cycle_id
				and tph.transcriptionist_id	= @transcriptionist_id
				order by r.name

				select row_number() over(order by transcriptionist_id,received_date) as row_id,
					   transcriptionist_id,
					   billing_cycle_id,
					   study_id,
					   study_uid,
					   received_date,
					   modality_name,
					   priority_desc,
					   institution_name,
					   patient_name,
					   amount,
					   adhoc_amount,
					   total_amount,
					   custom_report
				from
				(select tpd.transcriptionist_id,
					   tpd.billing_cycle_id,
					   tpd.study_id,
					   tpd.study_uid,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   priority_desc = dbo.InitCap(isnull(p.priority_desc,'Unkown')),
					   institution_name=i.name,
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   tpd.amount,
					   tpd.adhoc_amount,
					   tpd.total_amount,
					   custom_report = isnull(i.custom_report,'N')
				from ap_transcriptionist_payment_dtls tpd
				left outer join modality m on m.id= tpd.modality_id
				left outer join sys_priority p on p.priority_id = tpd.priority_id
				inner join study_hdr sh on sh.id = tpd.study_id
				inner join institutions i on i.id = sh.institution_id
				where tpd.billing_cycle_id = @billing_cycle_id
				and tpd.transcriptionist_id = @transcriptionist_id
				union
				select tpd.transcriptionist_id,
					   tpd.billing_cycle_id,
					   tpd.study_id,
					   tpd.study_uid,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   priority_desc = dbo.InitCap(isnull(p.priority_desc,'Unkown')),
					   institution_name=i.name,
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   tpd.amount,
					   tpd.adhoc_amount,
					   tpd.total_amount,
					   custom_report = isnull(i.custom_report,'N')
				from ap_transcriptionist_payment_dtls tpd
				left outer join modality m on m.id= tpd.modality_id
				left outer join sys_priority p on p.priority_id = tpd.priority_id
				inner join study_hdr_archive sh on sh.id = tpd.study_id
				inner join institutions i on sh.institution_id=i.id
			    where tpd.billing_cycle_id = @billing_cycle_id
				and tpd.transcriptionist_id = @transcriptionist_id) t
				

			end

		set nocount off

	end
GO
