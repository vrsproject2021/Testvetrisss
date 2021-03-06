USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_invoice_process_final_fetch]    Script Date: 20-08-2021 20:43:17 ******/
DROP PROCEDURE [dbo].[ar_invoice_process_final_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_invoice_process_final_fetch]    Script Date: 20-08-2021 20:43:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_invoice_process_final_fetch : 
                  fetch only the final & cancelled invoice
** Created By   : Pavel Guha 
** Created On   :05/03/2021
*******************************************************/
CREATE procedure [dbo].[ar_invoice_process_final_fetch]
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
				and ih.approved in ('Y','X')
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
				and iih.approved in ('Y','X')
				order by i.name

				select iid.billing_account_id,
					   iid.billing_cycle_id,
					   iid.institution_id,
					   iid.study_id,
					   iid.study_uid,
					   sh.received_date,
					   iid.category_id,
					   category_name =  dbo.InitCap(isnull(c.name,'Unkown')),
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
				left outer join sys_study_category c on c.id= iid.category_id
				left outer join modality m on m.id= iid.modality_id
				inner join study_hdr sh on sh.id = iid.study_id
				inner join sys_priority p on p.priority_id = sh.priority_id
				where iid.billing_cycle_id = @billing_cycle_id
				and iid.approved in ('Y','X')
				union
				select iid.billing_account_id,
					   iid.billing_cycle_id,
					   iid.institution_id,
					   iid.study_id,
					   iid.study_uid,
					   sh.received_date,
					   iid.category_id,
					   category_name =  dbo.InitCap(isnull(c.name,'Unkown')),
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
				left outer join sys_study_category c on c.id= iid.category_id
				left outer join modality m on m.id= iid.modality_id
				inner join study_hdr_archive sh on sh.id = iid.study_id
				inner join sys_priority p on p.priority_id = sh.priority_id
				where iid.billing_cycle_id = @billing_cycle_id
				and iid.approved in ('Y','X')
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
				and ih.approved in ('Y','X')
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
				and iih.approved in ('Y','X')
				order by i.name

				select iid.billing_account_id,
					   iid.billing_cycle_id,
					   iid.institution_id,
					   iid.study_id,
					   iid.study_uid,
					   sh.received_date,
					   iid.category_id,
					   category_name =  dbo.InitCap(isnull(c.name,'Unkown')),
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
				left outer join sys_study_category c on c.id= iid.category_id
				left outer join modality m on m.id= iid.modality_id
				inner join study_hdr sh on sh.id = iid.study_id
				inner join sys_priority p on p.priority_id = sh.priority_id
				where iid.billing_cycle_id = @billing_cycle_id
				and iid.billing_account_id=@billing_account_id
				and iid.approved in ('Y','X')
				union
				select iid.billing_account_id,
					   iid.billing_cycle_id,
					   iid.institution_id,
					   iid.study_id,
					   iid.study_uid,
					   sh.received_date,
					   iid.category_id,
					   category_name =  dbo.InitCap(isnull(c.name,'Unkown')),
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
				left outer join sys_study_category c on c.id= iid.category_id
				left outer join modality m on m.id= iid.modality_id
				inner join study_hdr_archive sh on sh.id = iid.study_id
				inner join sys_priority p on p.priority_id = sh.priority_id
				where iid.billing_cycle_id = @billing_cycle_id
				and iid.billing_account_id=@billing_account_id
				and iid.approved in ('Y','X')
				order by sh.received_date
			end

		
		set nocount off

	end
GO
