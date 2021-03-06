USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_process_view_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_process_view_fetch]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_process_view_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_process_view_fetch : 
                  fetch invoicing processing details 
				  if locked by other user
** Created By   : Pavel Guha 
** Created On   : 12/11/2019
*******************************************************/
--exec invoicing_process_view_fetch 'CD6E2FE4-9AC6-4010-9A9B-08EB687B09DD','1A1D9DEE-8D88-4ACB-9861-B0A254C30E34',45,'11111111-1111-1111-1111-111111111111','','',0
--exec invoicing_process_view_fetch 'D2B3965C-73F1-4BB5-AD9A-31003C1A869E','00000000-0000-0000-0000-000000000000'
--exec invoicing_process_view_fetch 'CD6E2FE4-9AC6-4010-9A9B-08EB687B09DD','98A30426-BA37-4564-98D0-A2CF6A4B9929',45,'11111111-1111-1111-1111-111111111111','','',0
CREATE procedure [dbo].[invoicing_process_view_fetch]
	@billing_cycle_id uniqueidentifier,
	@billing_account_id uniqueidentifier='00000000-0000-0000-0000-000000000000'
as
	begin
		set nocount on


		if(@billing_account_id ='00000000-0000-0000-0000-000000000000')
			begin
				select ih.billing_account_id,
					   ih.billing_cycle_id,
					   billing_account_name = dbo.InitCap(replace(ba.name,char(39),'')),
					   ih.total_study_count,
					   ih.total_study_count_std,
					   ih.total_study_count_stat,
					   ih.total_amount,
					   ih.approved,
					   ih.total_disc_amount,
					   ih.total_free_credits,
					   action=''
				from invoice_hdr ih
				inner join billing_account ba on ba.id = ih.billing_account_id
				where billing_cycle_id = @billing_cycle_id
				order by ba.name

				select iih.billing_account_id,
					   iih.billing_cycle_id,
					   iih.institution_id,
					   institution_code = i.code,
					   institution_name = dbo.InitCap(replace(i.name,char(39),'')),
					   iih.total_study_count,
					   iih.total_study_count_std,
					   iih.total_study_count_stat,
					   iih.total_disc_amount,
					   iih.free_read_count,
					   iih.total_amount,
					   iih.approved,
					   action='' 
				from invoice_institution_hdr iih
				inner join institutions i on i.id = iih.institution_id
				where billing_cycle_id = @billing_cycle_id
				order by i.name

				select iid.billing_account_id,
					   iid.billing_cycle_id,
					   iid.institution_id,
					   iid.study_id,
					   iid.study_uid,
					   sh.received_date,
					   iid.modality_id,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   sh.priority_id,
					   p.priority_desc,
					   iid.image_count,
					   object_count = isnull(sh.object_count,iid.image_count),
					   iid.rate,
					   iid.amount,
					   service_amount = isnull(iid.service_amount,0),
					   total_amount   = isnull(iid.total_amount,0),
					   iid.billed,
					   iid.approved,
					   case
							when iid.is_free='Y' then 'Free credit applied'
							when iid.disc_per_applied>0 then 'Discount of ' + convert(varchar(6),disc_per_applied) + '% ($' + convert(varchar(12),disc_amount)	 + ') applied'
							else ''
					   end promo_dtls
				from invoice_institution_dtls iid
				left outer join modality m on m.id= iid.modality_id
				inner join study_hdr sh on sh.id = iid.study_id
				inner join sys_priority p on p.priority_id = sh.priority_id
				where iid.billing_cycle_id = @billing_cycle_id
				union
				select iid.billing_account_id,
					   iid.billing_cycle_id,
					   iid.institution_id,
					   iid.study_id,
					   iid.study_uid,
					   sh.received_date,
					   iid.modality_id,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   sh.priority_id,
					   p.priority_desc,
					   iid.image_count,
					   object_count = isnull(sh.object_count,iid.image_count),
					   iid.rate,
					   iid.amount,
					   service_amount = isnull(iid.service_amount,0),
					   total_amount   = isnull(iid.total_amount,0),
					   iid.billed,
					   iid.approved,
					   case
							when iid.is_free='Y' then 'Free credit applied'
							when iid.disc_per_applied>0 then 'Discount of ' + convert(varchar(6),disc_per_applied) + '% ($' + convert(varchar(12),disc_amount)	 + ') applied'
							else ''
					   end promo_dtls
				from invoice_institution_dtls iid
				left outer join modality m on m.id= iid.modality_id
				inner join study_hdr_archive sh on sh.id = iid.study_id
				inner join sys_priority p on p.priority_id = sh.priority_id
				where iid.billing_cycle_id = @billing_cycle_id
				order by sh.received_date
				
			end
		else
			begin
				select ih.billing_account_id,
					   ih.billing_cycle_id,
					   billing_account_name = dbo.InitCap(replace(ba.name,char(39),'')),
					   ih.total_study_count,
					   ih.total_study_count_std,
					   ih.total_study_count_stat,
					   ih.total_amount,
					   ih.approved,
					   ih.total_disc_amount,
					   ih.total_free_credits,
					   action=''
				from invoice_hdr ih
				inner join billing_account ba on ba.id = ih.billing_account_id
				where ih.billing_cycle_id = @billing_cycle_id
				and ih.billing_account_id=@billing_account_id
				order by ba.name

				select iih.billing_account_id,
					   iih.billing_cycle_id,
					   iih.institution_id,
					   institution_code = i.code,
					   institution_name = dbo.InitCap(replace(i.name,char(39),'')),
					   iih.total_study_count,
					   iih.total_study_count_std,
					   iih.total_study_count_stat,
					   iih.total_disc_amount,
					   iih.free_read_count,
					   iih.total_amount,
					   iih.approved,
					   action='' 
				from invoice_institution_hdr iih
				inner join institutions i on i.id = iih.institution_id
				where iih.billing_cycle_id = @billing_cycle_id
				and iih.billing_account_id=@billing_account_id
				order by i.name

				select iid.billing_account_id,
					   iid.billing_cycle_id,
					   iid.institution_id,
					   iid.study_id,
					   iid.study_uid,
					   sh.received_date,
					   iid.modality_id,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   sh.priority_id,
					   p.priority_desc,
					   iid.image_count,
					   object_count = isnull(sh.object_count,iid.image_count),
					   iid.rate,
					   iid.amount,
					   service_amount = isnull(iid.service_amount,0),
					   total_amount   = isnull(iid.total_amount,0),
					   iid.billed,
					   iid.approved,
					    case
							when iid.is_free='Y' then 'Free credit applied'
							when iid.disc_per_applied>0 then 'Discount of ' + convert(varchar(6),disc_per_applied) + '% ($' + convert(varchar(12),disc_amount)	 + ') applied'
							else ''
					   end promo_dtls
				from invoice_institution_dtls iid
				left outer join modality m on m.id= iid.modality_id
				inner join study_hdr sh on sh.id = iid.study_id
				inner join sys_priority p on p.priority_id = sh.priority_id
				where iid.billing_cycle_id = @billing_cycle_id
				and iid.billing_account_id=@billing_account_id
				union
				select iid.billing_account_id,
					   iid.billing_cycle_id,
					   iid.institution_id,
					   iid.study_id,
					   iid.study_uid,
					   sh.received_date,
					   iid.modality_id,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   sh.priority_id,
					   p.priority_desc,
					   iid.image_count,
					   object_count = isnull(sh.object_count,iid.image_count),
					   iid.rate,
					   iid.amount,
					   service_amount = isnull(iid.service_amount,0),
					   total_amount   = isnull(iid.total_amount,0),
					   iid.billed,
					   iid.approved,
					    case
							when iid.is_free='Y' then 'Free credit applied'
							when iid.disc_per_applied>0 then 'Discount of ' + convert(varchar(6),disc_per_applied) + '% ($' + convert(varchar(12),disc_amount)	 + ') applied'
							else ''
					   end promo_dtls
				from invoice_institution_dtls iid
				left outer join modality m on m.id= iid.modality_id
				inner join study_hdr_archive sh on sh.id = iid.study_id
				inner join sys_priority p on p.priority_id = sh.priority_id
				where iid.billing_cycle_id = @billing_cycle_id
				and iid.billing_account_id=@billing_account_id
				order by sh.received_date
			end

	
		

		
		
		set nocount off

	end
GO
