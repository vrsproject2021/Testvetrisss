USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_invoicing_statement_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_invoicing_statement_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_invoicing_statement_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_invoicing_statement_fetch : 
                  fetch invoice statement
** Created By   : Pavel Guha 
** Created On   : 07/01/2020
*******************************************************/
--exec rpt_invoicing_statement_fetch 'CD6E2FE4-9AC6-4010-9A9B-08EB687B09DD','00000000-0000-0000-0000-000000000000'
CREATE procedure [dbo].[rpt_invoicing_statement_fetch]
	@billing_cycle_id uniqueidentifier,
	@billing_account_id uniqueidentifier='00000000-0000-0000-0000-000000000000'
as
	begin
		set nocount on

		 declare @arch_db_name nvarchar(30),
			     @strSQL varchar(max)

		select @arch_db_name = arch_db_name from billing_cycle where id=@billing_cycle_id
		

		if(@billing_account_id ='00000000-0000-0000-0000-000000000000')
			begin
				if(isnull(@arch_db_name,'')='')
					begin
						select iid.billing_account_id,
								billing_account = ba.name,
								ba_total_study_count = ih.total_study_count,
								ba_total_study_count_std = ih.total_study_count_std,
								ba_total_study_count_stat = ih.total_study_count_stat,
								ba_total_amount = isnull(ih.total_amount,0),
								case 
									when ih.approved='Y' then 'Approved (' + ih.invoice_no + ')'
									when ih.approved='N' then ' Not Approved'
								end ba_approved,
								ba_total_disc_amount = isnull(ih.total_disc_amount,0),
								ba_total_free_credits = isnull(ih.total_free_credits,0),
								iid.institution_id,
								institution = i.name,
								inst_total_study_count = iih.total_study_count,
								inst_total_study_count_std = iih.total_study_count_std,
								inst_total_study_count_stat = iih.total_study_count_stat,
								inst_total_disc_amount = iih.total_disc_amount,
								inst_free_read_count = iih.free_read_count,
								inst_total_amount = iih.total_amount,
								case 
									when iih.approved='Y' then 'Approved (' + iih.invoice_no + ')'
									when iih.approved='N' then ' Not Approved'
								end inst_approved,
								iid.study_id,
								iid.study_uid,
								sh.received_date,
								modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
								patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
								case
									when isnull(p.is_stat,'N')='N' then '-' else isnull(p.short_desc,'')
								end std_study,
								case
									when isnull(p.is_stat,'N')='Y' then isnull(p.short_desc,'') else '-'
								end stat_study,
								iid.image_count,
								object_count = isnull(sh.object_count,iid.image_count),
								iid.amount,
								service_amount = isnull(iid.service_amount,0),
								iid.disc_amount,
								total_amount   = isnull(iid.total_amount,0),
								case
									when iid.is_free='Y' then 'Free credit applied'
									when iid.disc_per_applied>0 then 'Discount of ' + convert(varchar(6),disc_per_applied) + '% ($' + convert(varchar(12),disc_amount)	 + ') applied'
									when iid.disc_amt_applied>0 then 'Discount of $' + convert(varchar(12),iid.disc_amount)	 + ' applied'
									else ''
								end promo_dtls
						from invoice_institution_dtls iid
						inner join invoice_hdr ih on ih.billing_account_id = iid.billing_account_id and ih.billing_cycle_id = iid.billing_cycle_id
						inner join invoice_institution_hdr iih on iih.institution_id = iid.institution_id and iih.billing_cycle_id = iid.billing_cycle_id
						left outer join modality m on m.id= iid.modality_id
						inner join billing_account ba on ba.id = iid.billing_account_id
						inner join institutions i on i.id = iid.institution_id
						inner join study_hdr sh on sh.id = iid.study_id
						left outer join sys_priority p on p.priority_id = sh.priority_id
						where iid.billing_cycle_id = @billing_cycle_id
						union
						select iid.billing_account_id,
								billing_account = ba.name,
								ba_total_study_count = ih.total_study_count,
								ba_total_study_count_std = ih.total_study_count_std,
								ba_total_study_count_stat = ih.total_study_count_stat,
								ba_total_amount = isnull(ih.total_amount,0),
								case 
									when ih.approved='Y' then 'Approved (' + ih.invoice_no + ')'
									when ih.approved='N' then ' Not Approved'
								end ba_approved,
								ba_total_disc_amount = isnull(ih.total_disc_amount,0),
								ba_total_free_credits = isnull(ih.total_free_credits,0),
								iid.institution_id,
								institution = i.name,
								inst_total_study_count = iih.total_study_count,
								inst_total_study_count_std = iih.total_study_count_std,
								inst_total_study_count_stat = iih.total_study_count_stat,
								inst_total_disc_amount = iih.total_disc_amount,
								inst_free_read_count = iih.free_read_count,
								inst_total_amount = iih.total_amount,
								case 
									when iih.approved='Y' then 'Approved (' + iih.invoice_no + ')'
									when iih.approved='N' then ' Not Approved'
								end inst_approved,
								iid.study_id,
								iid.study_uid,
								sh.received_date,
								modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
								patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
								case
									when isnull(p.is_stat,'N')='N' then '-' else isnull(p.short_desc,'')
								end std_study,
								case
									when isnull(p.is_stat,'N')='Y' then isnull(p.short_desc,'') else '-'
								end stat_study,
								iid.image_count,
								object_count = isnull(sh.object_count,iid.image_count),
								iid.amount,
								service_amount = isnull(iid.service_amount,0),
								iid.disc_amount,
								total_amount   = isnull(iid.total_amount,0),
								case
									when iid.is_free='Y' then 'Free credit applied'
									when iid.disc_per_applied>0 then 'Discount of ' + convert(varchar(6),disc_per_applied) + '% ($' + convert(varchar(12),disc_amount)	 + ') applied'
									when iid.disc_amt_applied>0 then 'Discount of $' + convert(varchar(12),iid.disc_amount)	 + ' applied'
									else ''
								end promo_dtls
						from invoice_institution_dtls iid
						inner join invoice_hdr ih on ih.billing_account_id = iid.billing_account_id and ih.billing_cycle_id = iid.billing_cycle_id
						inner join invoice_institution_hdr iih on iih.institution_id = iid.institution_id and iih.billing_cycle_id = iid.billing_cycle_id
						left outer join modality m on m.id= iid.modality_id
						inner join billing_account ba on ba.id = iid.billing_account_id
						inner join institutions i on i.id = iid.institution_id
						inner join study_hdr_archive sh on sh.id = iid.study_id
						left outer join sys_priority p on p.priority_id = sh.priority_id
						where iid.billing_cycle_id = @billing_cycle_id
						order by billing_account,institution,sh.received_date
					end
				else
					begin
						set @strSQL ='select iid.billing_account_id,'
						set @strSQL = @strSQL + 'billing_account = ba.name,'
						set @strSQL = @strSQL + 'ba_total_study_count = ih.total_study_count,'
						set @strSQL = @strSQL + 'ba_total_study_count_std = ih.total_study_count_std,'
						set @strSQL = @strSQL + 'ba_total_study_count_stat = ih.total_study_count_stat,'
						set @strSQL = @strSQL + 'ba_total_amount = isnull(ih.total_amount,0),'
						set @strSQL = @strSQL + 'case '
						set @strSQL = @strSQL + 'when ih.approved=''Y'' then ''Approved ('' + ' 
						set @strSQL = @strSQL + 'ih.invoice_no' 
						set @strSQL = @strSQL + '+ '')'' '
						set @strSQL = @strSQL + 'when ih.approved=''N'' then ''Not Approved'' '
						set @strSQL = @strSQL + 'end ba_approved,'
						set @strSQL = @strSQL + 'ba_total_disc_amount = isnull(ih.total_disc_amount,0),'
						set @strSQL = @strSQL + 'ba_total_free_credits = isnull(ih.total_free_credits,0),'
						set @strSQL = @strSQL + 'iid.institution_id,'
						set @strSQL = @strSQL + 'institution = i.name,'
						set @strSQL = @strSQL + 'inst_total_study_count = iih.total_study_count,'
						set @strSQL = @strSQL + 'inst_total_study_count_std = iih.total_study_count_std,'
						set @strSQL = @strSQL + 'inst_total_study_count_stat = iih.total_study_count_stat,'
						set @strSQL = @strSQL + 'inst_total_disc_amount = iih.total_disc_amount,'
						set @strSQL = @strSQL + 'inst_free_read_count = iih.free_read_count,'
						set @strSQL = @strSQL + 'inst_total_amount = iih.total_amount,'
						set @strSQL = @strSQL + 'case '
						set @strSQL = @strSQL + 'when iih.approved=''Y'' then ''Approved ('' + ' 
						set @strSQL = @strSQL + 'iih.invoice_no' 
						set @strSQL = @strSQL + '+ '')'' '
						set @strSQL = @strSQL + 'when iih.approved=''N'' then ''Not Approved'' '
						set @strSQL = @strSQL + 'end inst_approved,'
						set @strSQL = @strSQL + 'iid.study_id,'
						set @strSQL = @strSQL + 'iid.study_uid,'
						set @strSQL = @strSQL + 'sh.received_date,'
						set @strSQL = @strSQL + 'modality_name = dbo.InitCap(isnull(m.name,''Unkown'')),'
						set @strSQL = @strSQL + 'patient_name = dbo.InitCap(isnull(sh.patient_name,'''')),'
						set @strSQL = @strSQL + 'case '
						set @strSQL = @strSQL + 'when isnull(p.is_stat,''N'')=''N'' then ''-'' else isnull(p.short_desc,'''') '
						set @strSQL = @strSQL + 'end std_study,'
						set @strSQL = @strSQL + 'case '
						set @strSQL = @strSQL + 'when isnull(p.is_stat,''N'')=''Y'' then isnull(p.short_desc,'''') else ''-'' '
						set @strSQL = @strSQL + 'end stat_study,'
						set @strSQL = @strSQL + 'iid.image_count,'
						set @strSQL = @strSQL + 'object_count = isnull(sh.object_count,iid.image_count),'
						set @strSQL = @strSQL + 'iid.amount, '
						set @strSQL = @strSQL + 'service_amount = isnull(iid.service_amount,0),'
						set @strSQL = @strSQL + 'iid.disc_amount,'
						set @strSQL = @strSQL + 'total_amount   = isnull(iid.total_amount,0),'
						set @strSQL = @strSQL + 'case '
						set @strSQL = @strSQL + 'when iid.is_free=''Y'' then ''Free credit applied'' '
						set @strSQL = @strSQL + 'when iid.disc_per_applied>0 then ''Discount of '' + '
						set @strSQL = @strSQL + 'convert(varchar(6),iid.disc_per_applied)' 
						set @strSQL = @strSQL + '+ ''% ($''+ '
						set @strSQL = @strSQL + 'convert(varchar(12),iid.disc_amount)' 
						set @strSQL = @strSQL + '+'') applied'' '
						set @strSQL = @strSQL + 'when iid.disc_amt_applied>0 then ''Discount of '' + '
						set @strSQL = @strSQL + '+ ''$''+ '
						set @strSQL = @strSQL + 'convert(varchar(12),iid.disc_amount)' 
						set @strSQL = @strSQL + '+'' applied'' '
						set @strSQL = @strSQL + 'else '''' '
						set @strSQL = @strSQL + 'end promo_dtls '
						set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls iid '
						set @strSQL = @strSQL + 'inner join ' + @arch_db_name + '..invoice_hdr ih on ih.billing_account_id = iid.billing_account_id and ih.billing_cycle_id = iid.billing_cycle_id '
						set @strSQL = @strSQL + 'inner join ' + @arch_db_name + '..invoice_institution_hdr iih on iih.institution_id = iid.institution_id and iih.billing_cycle_id = iid.billing_cycle_id '
						set @strSQL = @strSQL + 'left outer join modality m on m.id= iid.modality_id '
						set @strSQL = @strSQL + 'inner join billing_account ba on ba.id = iid.billing_account_id '
						set @strSQL = @strSQL + 'inner join institutions i on i.id = iid.institution_id '
						set @strSQL = @strSQL + 'inner join ' + @arch_db_name + '..study_hdr_archive sh on sh.id = iid.study_id '
						set @strSQL = @strSQL + 'left outer join sys_priority p on p.priority_id = sh.priority_id '
						set @strSQL = @strSQL + 'where iid.billing_cycle_id= ''' + convert(varchar(36),@billing_cycle_id) + ''' '
						set @strSQL = @strSQL + 'order by billing_account,institution,sh.received_date'

						exec(@strSQL)
					end
			end
		else
			begin
				if(isnull(@arch_db_name,'')='')
					begin
						select iid.billing_account_id,
							   billing_account = ba.name,
							   ba_total_study_count = ih.total_study_count,
							   ba_total_study_count_std = ih.total_study_count_std,
							   ba_total_study_count_stat = ih.total_study_count_stat,
							   ba_total_amount = isnull(ih.total_amount,0),
								case 
									when ih.approved='Y' then 'Approved (' + ih.invoice_no + ')'
									when ih.approved='N' then ' Not Approved'
							   end ba_approved,
							   total_disc_amount = isnull(ih.total_disc_amount,0),
							   ba_total_free_credits = isnull(ih.total_free_credits,0),
							   iid.institution_id,
							   institution = i.name,
							   iih.total_study_count,
							   iih.total_study_count_std,
							   iih.total_study_count_stat,
							   iih.total_disc_amount,
							   iih.free_read_count,
							   iih.total_amount,
							   case 
									when iih.approved='Y' then 'Approved (' + iih.invoice_no + ')'
									when iih.approved='N' then ' Not Approved'
							   end inst_approved,
							   iid.study_id,
							   iid.study_uid,
							   sh.received_date,
							   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
							   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
							   case
									when isnull(p.is_stat,'N')='N' then '-' else isnull(p.short_desc,'')
							   end std_study,
							   case
									when isnull(p.is_stat,'N')='Y' then isnull(p.short_desc,'') else '-'
							   end stat_study,
							   iid.image_count,
							   object_count = isnull(sh.object_count,iid.image_count),
							   iid.amount,
							   service_amount = isnull(iid.service_amount,0),
							   iid.disc_amount,
							   total_amount   = isnull(iid.total_amount,0),
							   case
									when iid.is_free='Y' then 'Free credit applied'
									when iid.disc_per_applied>0 then 'Discount of ' + convert(varchar(6),disc_per_applied) + '% ($' + convert(varchar(12),disc_amount)	 + ') applied'
									when iid.disc_amt_applied>0 then 'Discount of $' + convert(varchar(12),iid.disc_amount)	 + ' applied'
									else ''
							   end promo_dtls
						from invoice_institution_dtls iid
						inner join invoice_hdr ih on ih.billing_account_id = iid.billing_account_id and ih.billing_cycle_id = iid.billing_cycle_id
						inner join invoice_institution_hdr iih on iih.institution_id = iid.institution_id and iih.billing_cycle_id = iid.billing_cycle_id
						left outer join modality m on m.id= iid.modality_id
						inner join billing_account ba on ba.id = iid.billing_account_id
						inner join institutions i on i.id = iid.institution_id
						inner join study_hdr sh on sh.id = iid.study_id
						left outer join sys_priority p on p.priority_id = sh.priority_id
						where iid.billing_cycle_id = @billing_cycle_id
						and iid.billing_account_id = @billing_account_id
						union
						select iid.billing_account_id,
							   billing_account = ba.name,
							   ba_total_study_count = ih.total_study_count,
							   ba_total_study_count_std = ih.total_study_count_std,
							   ba_total_study_count_stat = ih.total_study_count_stat,
							   ba_total_amount = isnull(ih.total_amount,0),
							   case 
									when ih.approved='Y' then 'Approved'
									when ih.approved='N' then ' Not Approved'
							   end ba_approved,
							   total_disc_amount = isnull(ih.total_disc_amount,0),
							   ba_total_free_credits = isnull(ih.total_free_credits,0),
							   iid.institution_id,
							   institution = i.name,
							   iih.total_study_count,
							   iih.total_study_count_std,
							   iih.total_study_count_stat,
							   iih.total_disc_amount,
							   iih.free_read_count,
							   iih.total_amount,
							   case 
									when iih.approved='Y' then 'Approved'
									when iih.approved='N' then ' Not Approved'
							   end inst_approved,
							   iid.study_id,
							   iid.study_uid,
							   sh.received_date,
							   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
							   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
								case
									when isnull(p.is_stat,'N')='N' then isnull(p.short_desc,'') else '-'
							   end std_study,
							   case
									when isnull(p.is_stat,'N')='Y' then isnull(p.short_desc,'') else '-'
							   end stat_study,
							   iid.image_count,
							   object_count = isnull(sh.object_count,iid.image_count),
							   iid.amount,
							   service_amount = isnull(iid.service_amount,0),
							   iid.disc_amount,
							   total_amount   = isnull(iid.total_amount,0),

							   case
									when iid.is_free='Y' then 'Free credit applied'
									when iid.disc_per_applied>0 then 'Discount of ' + convert(varchar(6),disc_per_applied) + '% ($' + convert(varchar(12),disc_amount)	 + ') applied'
									when iid.disc_amt_applied>0 then 'Discount of $' + convert(varchar(12),iid.disc_amount)	 + ' applied'
									else ''
							   end promo_dtls
						from invoice_institution_dtls iid
						inner join invoice_hdr ih on ih.billing_account_id = iid.billing_account_id and ih.billing_cycle_id = iid.billing_cycle_id
						inner join invoice_institution_hdr iih on iih.institution_id = iid.institution_id and iih.billing_cycle_id = iid.billing_cycle_id
						left outer join modality m on m.id= iid.modality_id
						inner join billing_account ba on ba.id = iid.billing_account_id
						inner join institutions i on i.id = iid.institution_id
						inner join study_hdr_archive sh on sh.id = iid.study_id
						left outer join sys_priority p on p.priority_id = sh.priority_id
						where iid.billing_cycle_id = @billing_cycle_id
						and iid.billing_account_id = @billing_account_id
						order by billing_account,institution,sh.received_date
					end
				else
					begin
						set @strSQL ='select iid.billing_account_id,'
						set @strSQL = @strSQL + 'billing_account = ba.name,'
						set @strSQL = @strSQL + 'ba_total_study_count = ih.total_study_count,'
						set @strSQL = @strSQL + 'ba_total_study_count_std = ih.total_study_count_std,'
						set @strSQL = @strSQL + 'ba_total_study_count_stat = ih.total_study_count_stat,'
						set @strSQL = @strSQL + 'ba_total_amount = isnull(ih.total_amount,0),'
						set @strSQL = @strSQL + 'case '
						set @strSQL = @strSQL + 'when ih.approved=''Y'' then ''Approved ('' + ' 
						set @strSQL = @strSQL + 'ih.invoice_no' 
						set @strSQL = @strSQL + '+ '')'' '
						set @strSQL = @strSQL + 'when ih.approved=''N'' then ''Not Approved'' '
						set @strSQL = @strSQL + 'end ba_approved,'
						set @strSQL = @strSQL + 'ba_total_disc_amount = isnull(ih.total_disc_amount,0),'
						set @strSQL = @strSQL + 'ba_total_free_credits = isnull(ih.total_free_credits,0),'
						set @strSQL = @strSQL + 'iid.institution_id,'
						set @strSQL = @strSQL + 'institution = i.name,'
						set @strSQL = @strSQL + 'inst_total_study_count = iih.total_study_count,'
						set @strSQL = @strSQL + 'inst_total_study_count_std = iih.total_study_count_std,'
						set @strSQL = @strSQL + 'inst_total_study_count_stat = iih.total_study_count_stat,'
						set @strSQL = @strSQL + 'inst_total_disc_amount = iih.total_disc_amount,'
						set @strSQL = @strSQL + 'inst_free_read_count = iih.free_read_count,'
						set @strSQL = @strSQL + 'inst_total_amount = iih.total_amount,'
						set @strSQL = @strSQL + 'case '
						set @strSQL = @strSQL + 'when iih.approved=''Y'' then ''Approved ('' + ' 
						set @strSQL = @strSQL + 'iih.invoice_no' 
						set @strSQL = @strSQL + '+ '')'' '
						set @strSQL = @strSQL + 'when iih.approved=''N'' then ''Not Approved'' '
						set @strSQL = @strSQL + 'end inst_approved,'
						set @strSQL = @strSQL + 'iid.study_id,'
						set @strSQL = @strSQL + 'iid.study_uid,'
						set @strSQL = @strSQL + 'sh.received_date,'
						set @strSQL = @strSQL + 'modality_name = dbo.InitCap(isnull(m.name,''Unkown'')),'
						set @strSQL = @strSQL + 'patient_name = dbo.InitCap(isnull(sh.patient_name,'''')),'
						set @strSQL = @strSQL + 'case '
						set @strSQL = @strSQL + 'when isnull(p.is_stat,''N'')=''N'' then ''-'' else isnull(p.short_desc,'''') '
						set @strSQL = @strSQL + 'end std_study,'
						set @strSQL = @strSQL + 'case '
						set @strSQL = @strSQL + 'when isnull(p.is_stat,''N'')=''Y'' then isnull(p.short_desc,'''') else ''-'' '
						set @strSQL = @strSQL + 'end stat_study,'
						set @strSQL = @strSQL + 'iid.image_count,'
						set @strSQL = @strSQL + 'object_count = isnull(sh.object_count,iid.image_count),'
						set @strSQL = @strSQL + 'iid.amount, '
						set @strSQL = @strSQL + 'service_amount = isnull(iid.service_amount,0),'
						set @strSQL = @strSQL + 'iid.disc_amount,'
						set @strSQL = @strSQL + 'total_amount   = isnull(iid.total_amount,0),'
						set @strSQL = @strSQL + 'case '
						set @strSQL = @strSQL + 'when iid.is_free=''Y'' then ''Free credit applied'' '
						set @strSQL = @strSQL + 'when iid.disc_per_applied>0 then ''Discount of '' + '
						set @strSQL = @strSQL + 'convert(varchar(6),iid.disc_per_applied)' 
						set @strSQL = @strSQL + '+ ''% ($''+ '
						set @strSQL = @strSQL + 'convert(varchar(12),iid.disc_amount)' 
						set @strSQL = @strSQL + '+'') applied'' '
						set @strSQL = @strSQL + 'when iid.disc_amt_applied>0 then ''Discount of '' + '
						set @strSQL = @strSQL + '+ ''$''+ '
						set @strSQL = @strSQL + 'convert(varchar(12),iid.disc_amount)' 
						set @strSQL = @strSQL + '+'' applied'' '
						set @strSQL = @strSQL + 'else '''' '
						set @strSQL = @strSQL + 'end promo_dtls '
						set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls iid '
						set @strSQL = @strSQL + 'inner join ' + @arch_db_name + '..invoice_hdr ih on ih.billing_account_id = iid.billing_account_id and ih.billing_cycle_id = iid.billing_cycle_id '
						set @strSQL = @strSQL + 'inner join ' + @arch_db_name + '..invoice_institution_hdr iih on iih.institution_id = iid.institution_id and iih.billing_cycle_id = iid.billing_cycle_id '
						set @strSQL = @strSQL + 'left outer join modality m on m.id= iid.modality_id '
						set @strSQL = @strSQL + 'inner join billing_account ba on ba.id = iid.billing_account_id '
						set @strSQL = @strSQL + 'inner join institutions i on i.id = iid.institution_id '
						set @strSQL = @strSQL + 'inner join ' + @arch_db_name + '..study_hdr_archive sh on sh.id = iid.study_id '
						set @strSQL = @strSQL + 'left outer join sys_priority p on p.priority_id = sh.priority_id '
						set @strSQL = @strSQL + 'where iid.billing_cycle_id= ''' + convert(varchar(36),@billing_cycle_id) + ''' '
						set @strSQL = @strSQL + 'order by billing_account,institution,sh.received_date'

						exec(@strSQL)
					end
			end

		set nocount off
		return 1
	end
GO
