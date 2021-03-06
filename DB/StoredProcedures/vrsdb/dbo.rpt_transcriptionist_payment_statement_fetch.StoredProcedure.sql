USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_transcriptionist_payment_statement_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_transcriptionist_payment_statement_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_transcriptionist_payment_statement_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_transcriptionist_payment_statement_fetch : 
                  fetch transcriptionist payment statement
** Created By   : Pavel Guha 
** Created On   : 27/10/2020
*******************************************************/
--exec rpt_transcriptionist_payment_statement_fetch '3ACFB756-45AF-424C-80E5-5B66406E08A6','00000000-0000-0000-0000-000000000000'
create procedure [dbo].[rpt_transcriptionist_payment_statement_fetch]
	@billing_cycle_id uniqueidentifier,
	@transcriptionist_id uniqueidentifier='00000000-0000-0000-0000-000000000000'
as
	begin
		set nocount on
		

		if(@transcriptionist_id ='00000000-0000-0000-0000-000000000000')
			begin
				select tpd.transcriptionist_id,
					   transcriptionist = t.name,
					   case 
							when rph.approved='Y' then 'Approved (' + rph.payment_no + ')'
							when rph.approved='N' then 'Not Approved'
					   end approved,
					   institution = i.name,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   p.priority_desc, 
					   
					   tpd.amount,
					   tpd.addl_stat_rate,
					   tpd.adhoc_amount,
					   tpd.total_amount
				from ap_transcriptionist_payment_dtls tpd
				inner join ap_transcriptionist_payment_hdr rph on rph.transcriptionist_id = tpd.transcriptionist_id and rph.billing_cycle_id = tpd.billing_cycle_id
				left outer join modality m on m.id= tpd.modality_id
				inner join transciptionists t on t.id = tpd.transcriptionist_id
				inner join study_hdr sh on sh.id = tpd.study_id
				inner join institutions i on i.id = sh.institution_id
				inner join sys_priority p on p.priority_id = sh.priority_id
				where tpd.billing_cycle_id = @billing_cycle_id
				union
				select tpd.transcriptionist_id,
					   transcriptionist = t.name,
					   case 
							when rph.approved='Y' then 'Approved (' + rph.payment_no + ')'
							when rph.approved='N' then 'Not Approved'
					   end approved,
					   institution = i.name,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   p.priority_desc, 
					   tpd.amount,
					   tpd.addl_stat_rate,
					   tpd.adhoc_amount,
					   tpd.total_amount
				from ap_transcriptionist_payment_dtls tpd
				inner join ap_transcriptionist_payment_hdr rph on rph.transcriptionist_id = tpd.transcriptionist_id and rph.billing_cycle_id = tpd.billing_cycle_id
				left outer join modality m on m.id= tpd.modality_id
				inner join transciptionists t on t.id = tpd.transcriptionist_id
				inner join study_hdr_archive sh on sh.id = tpd.study_id
				inner join institutions i on i.id = sh.institution_id
				inner join sys_priority p on p.priority_id = sh.priority_id
				where tpd.billing_cycle_id = @billing_cycle_id
				order by transcriptionist,institution,sh.received_date
			end
		else
			begin
				select tpd.transcriptionist_id,
					   transcriptionist = t.name,
					   case 
							when rph.approved='Y' then 'Approved (' + rph.payment_no + ')'
							when rph.approved='N' then 'Not Approved'
					   end approved,
					   institution = i.name,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   p.priority_desc, 
					   tpd.amount,
					   tpd.addl_stat_rate,
					   tpd.adhoc_amount,
					   tpd.total_amount
				from ap_transcriptionist_payment_dtls tpd
				inner join ap_transcriptionist_payment_hdr rph on rph.transcriptionist_id = tpd.transcriptionist_id and rph.billing_cycle_id = tpd.billing_cycle_id
				left outer join modality m on m.id= tpd.modality_id
				inner join transciptionists t on t.id = tpd.transcriptionist_id
				inner join study_hdr sh on sh.id = tpd.study_id
				inner join institutions i on i.id = sh.institution_id
				inner join sys_priority p on p.priority_id = sh.priority_id
				where tpd.billing_cycle_id = @billing_cycle_id
				and tpd.transcriptionist_id = @transcriptionist_id
				union
				select tpd.transcriptionist_id,
					   transcriptionist = t.name,
					   case 
							when rph.approved='Y' then 'Approved (' + rph.payment_no + ')'
							when rph.approved='N' then 'Not Approved'
					   end approved,
					   institution = i.name,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   p.priority_desc, 
					   tpd.amount,
					   tpd.addl_stat_rate,
					   tpd.adhoc_amount,
					   tpd.total_amount
				from ap_transcriptionist_payment_dtls tpd
				inner join ap_transcriptionist_payment_hdr rph on rph.transcriptionist_id = tpd.transcriptionist_id and rph.billing_cycle_id = tpd.billing_cycle_id
				left outer join modality m on m.id= tpd.modality_id
				inner join transciptionists t on t.id = tpd.transcriptionist_id
				inner join study_hdr_archive sh on sh.id = tpd.study_id
				inner join institutions i on i.id = sh.institution_id
				inner join sys_priority p on p.priority_id = sh.priority_id
				where tpd.billing_cycle_id = @billing_cycle_id
				and tpd.transcriptionist_id = @transcriptionist_id
				order by transcriptionist,institution,sh.received_date
			end

	

		
		set nocount off
		return 1
	end
GO
