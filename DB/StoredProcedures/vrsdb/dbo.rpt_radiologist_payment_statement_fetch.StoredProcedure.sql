USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_radiologist_payment_statement_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_radiologist_payment_statement_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_radiologist_payment_statement_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_radiologist_payment_statement_fetch : 
                  fetch radiologist payment statement
** Created By   : Pavel Guha 
** Created On   : 28/01/2020
*******************************************************/
--exec rpt_radiologist_payment_statement_fetch '397F7134-F742-45F3-924B-9A9F77EC20DC','ECF1FA18-7B5E-4295-A113-C32AB42171EC'
--exec rpt_radiologist_payment_statement_fetch '397F7134-F742-45F3-924B-9A9F77EC20DC','00000000-0000-0000-0000-000000000000'
CREATE procedure [dbo].[rpt_radiologist_payment_statement_fetch]
	@billing_cycle_id uniqueidentifier,
	@radiologist_id uniqueidentifier='00000000-0000-0000-0000-000000000000'
as
	begin
		set nocount on
		

		if(@radiologist_id ='00000000-0000-0000-0000-000000000000')
			begin
				select rpd.radiologist_id,
					   radiologist = r.name,
					   other_adhoc_amount = isnull((select sum(adhoc_payment) from ap_radiologist_other_adhoc_payments where radiologist_id=rph.radiologist_id and billing_cycle_id=@billing_cycle_id),0),
					   case 
							when rph.approved='Y' then 'Approved (' + rph.payment_no + ')'
							when rph.approved='N' then 'Not Approved'
					   end approved,
					   institution = i.name,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   p.priority_desc, 
					   case when rpd.is_reading_prelim ='Y' then 'Yes' else 'No' end is_reading_prelim,
					   case when rpd.is_reading_final ='Y' then 'Yes' else 'No' end is_reading_final,
					   case
							when rpd.is_reading_prelim='Y' and rpd.is_reading_final='Y' then 2 else 1
					   end study_count,
					   rpd.prelim_amount,
					   rpd.final_amount,
					   rpd.adhoc_amount,
					   rpd.total_amount,
					   grand_adhoc_amount = isnull((select sum(adhoc_payment) from ap_radiologist_other_adhoc_payments where  billing_cycle_id=@billing_cycle_id),0) 
											+ isnull((select sum(adhoc_amount) from ap_radiologist_payment_dtls where billing_cycle_id=@billing_cycle_id),0) 
				from ap_radiologist_payment_dtls rpd
				inner join ap_radiologist_payment_hdr rph on rph.radiologist_id = rpd.radiologist_id and rph.billing_cycle_id = rpd.billing_cycle_id
				left outer join modality m on m.id= rpd.modality_id
				inner join radiologists r on r.id = rpd.radiologist_id
				inner join study_hdr sh on sh.id = rpd.study_id
				inner join institutions i on i.id = sh.institution_id
				inner join sys_priority p on p.priority_id = sh.priority_id
				where rpd.billing_cycle_id = @billing_cycle_id
				union
				select rpd.radiologist_id,
					   radiologist = r.name,
					   other_adhoc_amount = isnull((select sum(adhoc_payment) from ap_radiologist_other_adhoc_payments where radiologist_id=rph.radiologist_id and billing_cycle_id=@billing_cycle_id),0),
					   case 
							when rph.approved='Y' then 'Approved (' + rph.payment_no + ')'
							when rph.approved='N' then 'Not Approved'
					   end approved,
					   institution = i.name,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   p.priority_desc, 
					   case when rpd.is_reading_prelim ='Y' then 'Yes' else 'No' end is_reading_prelim,
					   case when rpd.is_reading_final ='Y' then 'Yes' else 'No' end is_reading_final,
					    case
							when rpd.is_reading_prelim='Y' and rpd.is_reading_final='Y' then 2 else 1
					   end study_count,
					   rpd.prelim_amount,
					   rpd.final_amount,
					   rpd.adhoc_amount,
					   rpd.total_amount,
					   grand_adhoc_amount = isnull((select sum(adhoc_payment) from ap_radiologist_other_adhoc_payments where  billing_cycle_id=@billing_cycle_id),0) 
											+ isnull((select sum(adhoc_amount) from ap_radiologist_payment_dtls where billing_cycle_id=@billing_cycle_id),0) 
				from ap_radiologist_payment_dtls rpd
				inner join ap_radiologist_payment_hdr rph on rph.radiologist_id = rpd.radiologist_id and rph.billing_cycle_id = rpd.billing_cycle_id
				left outer join modality m on m.id= rpd.modality_id
				inner join radiologists r on r.id = rpd.radiologist_id
				inner join study_hdr_archive sh on sh.id = rpd.study_id
				inner join institutions i on i.id = sh.institution_id
				inner join sys_priority p on p.priority_id = sh.priority_id
				where rpd.billing_cycle_id = @billing_cycle_id
				order by radiologist,institution,sh.received_date
			end
		else
			begin
				select rpd.radiologist_id,
					   radiologist = r.name,
					   other_adhoc_amount = isnull((select sum(adhoc_payment) from ap_radiologist_other_adhoc_payments where radiologist_id=@radiologist_id and billing_cycle_id=@billing_cycle_id),0),
					   case 
							when rph.approved='Y' then 'Approved (' + rph.payment_no + ')'
							when rph.approved='N' then 'Not Approved'
					   end approved,
					   institution = i.name,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   p.priority_desc, 
					   case when rpd.is_reading_prelim ='Y' then 'Yes' else 'No' end is_reading_prelim,
					   case when rpd.is_reading_final ='Y' then 'Yes' else 'No' end is_reading_final,
					    case
							when rpd.is_reading_prelim='Y' and rpd.is_reading_final='Y' then 2 else 1
					   end study_count,
					   rpd.prelim_amount,
					   rpd.final_amount,
					   rpd.adhoc_amount,
					   rpd.total_amount,
					   grand_adhoc_amount = isnull((select sum(adhoc_payment) from ap_radiologist_other_adhoc_payments where billing_cycle_id=@billing_cycle_id),0) 
											+ isnull((select sum(adhoc_amount) from ap_radiologist_payment_dtls where billing_cycle_id=@billing_cycle_id),0) 
				from ap_radiologist_payment_dtls rpd
				inner join ap_radiologist_payment_hdr rph on rph.radiologist_id = rpd.radiologist_id and rph.billing_cycle_id = rpd.billing_cycle_id
				left outer join modality m on m.id= rpd.modality_id
				inner join radiologists r on r.id = rpd.radiologist_id
				inner join study_hdr sh on sh.id = rpd.study_id
				inner join institutions i on i.id = sh.institution_id
				inner join sys_priority p on p.priority_id = sh.priority_id
				where rpd.billing_cycle_id = @billing_cycle_id
				and rpd.radiologist_id = @radiologist_id
				union
				select rpd.radiologist_id,
					   radiologist = r.name,
					   other_adhoc_amount = isnull((select sum(adhoc_payment) from ap_radiologist_other_adhoc_payments where radiologist_id=@radiologist_id and billing_cycle_id=@billing_cycle_id),0),
					   case 
							when rph.approved='Y' then 'Approved (' + rph.payment_no + ')'
							when rph.approved='N' then 'Not Approved'
					   end approved,
					   institution = i.name,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   p.priority_desc, 
					   case when rpd.is_reading_prelim ='Y' then 'Yes' else 'No' end is_reading_prelim,
					   case when rpd.is_reading_final ='Y' then 'Yes' else 'No' end is_reading_final,
					    case
							when rpd.is_reading_prelim='Y' and rpd.is_reading_final='Y' then 2 else 1
					   end study_count,
					   rpd.prelim_amount,
					   rpd.final_amount,
					   rpd.adhoc_amount,
					   rpd.total_amount,
					   grand_adhoc_amount = isnull((select sum(adhoc_payment) from ap_radiologist_other_adhoc_payments where billing_cycle_id=@billing_cycle_id),0) 
											+ isnull((select sum(adhoc_amount) from ap_radiologist_payment_dtls where billing_cycle_id=@billing_cycle_id),0) 
				from ap_radiologist_payment_dtls rpd
				inner join ap_radiologist_payment_hdr rph on rph.radiologist_id = rpd.radiologist_id and rph.billing_cycle_id = rpd.billing_cycle_id
				left outer join modality m on m.id= rpd.modality_id
				inner join radiologists r on r.id = rpd.radiologist_id
				inner join study_hdr_archive sh on sh.id = rpd.study_id
				inner join institutions i on i.id = sh.institution_id
				inner join sys_priority p on p.priority_id = sh.priority_id
				where rpd.billing_cycle_id = @billing_cycle_id
				and rpd.radiologist_id = @radiologist_id
				order by radiologist,institution,sh.received_date
			end

	

		
		set nocount off
		return 1
	end
GO
