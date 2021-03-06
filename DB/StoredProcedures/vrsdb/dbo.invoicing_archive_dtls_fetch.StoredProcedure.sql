USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_archive_dtls_fetch]    Script Date: 27-08-2021 15:29:25 ******/
DROP PROCEDURE [dbo].[invoicing_archive_dtls_fetch]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_archive_dtls_fetch]    Script Date: 27-08-2021 15:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_archive_dtls_fetch : 
                  fetch invoice details from archive
** Created By   : Pavel Guha 
** Created On   : 12/08/2021
*******************************************************/
--exec invoicing_archive_dtls_fetch 'D6FC1504-F380-4966-B135-5AEBE40FB683','AB352297-C935-4E57-9CA9-D7C146B15914','vrsarchive20db'
--exec invoicing_archive_dtls_fetch 'D6FC1504-F380-4966-B135-5AEBE40FB683','00000000-0000-0000-0000-000000000000','vrsarchive20db'
CREATE procedure [dbo].[invoicing_archive_dtls_fetch]
	@billing_cycle_id uniqueidentifier,
	@billing_account_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @arch_db_name nvarchar(30)
as
	begin
		set nocount on

		declare @strSQL1 varchar(max),
				@strSQL2 varchar(max),
				@strSQL3 varchar(max)

		if(@billing_account_id ='00000000-0000-0000-0000-000000000000')
			begin
				set @strSQL1 = 'select ih.billing_account_id,'
				set @strSQL1 = @strSQL1 + 'ih.billing_cycle_id,'
				set @strSQL1 = @strSQL1 + 'billing_account_name = dbo.InitCap(replace(ba.name,char(39),'''')),'
				set @strSQL1 = @strSQL1 + 'ih.total_study_count,'
				set @strSQL1 = @strSQL1 + 'ih.total_study_count_std,'
				set @strSQL1 = @strSQL1 + 'ih.total_study_count_stat,'
				set @strSQL1 = @strSQL1 + 'ih.total_amount,'
				set @strSQL1 = @strSQL1 + 'ih.approved,'
				set @strSQL1 = @strSQL1 + 'ih.total_disc_amount,'
				set @strSQL1 = @strSQL1 + 'ih.total_free_credits,'
				set @strSQL1 = @strSQL1 + 'action='''' '
				set @strSQL1 = @strSQL1 + 'from ' + @arch_db_name + '..invoice_hdr ih '
				set @strSQL1 = @strSQL1 + 'inner join billing_account ba on ba.id = ih.billing_account_id '
				set @strSQL1 = @strSQL1 + 'where ih.billing_cycle_id = ''' + convert(varchar(36),@billing_cycle_id) + ''' '
				set @strSQL1 = @strSQL1 + 'order by ba.name '

				set @strSQL2 = 'select iih.billing_account_id,'
				set @strSQL2 = @strSQL2 + 'iih.billing_cycle_id,'
				set @strSQL2 = @strSQL2 + 'iih.institution_id,'
				set @strSQL2 = @strSQL2 + 'institution_code = i.code,'
				set @strSQL2 = @strSQL2 + 'institution_name = dbo.InitCap(replace(i.name,char(39),'''')),'
				set @strSQL2 = @strSQL2 + 'iih.total_study_count,'
				set @strSQL2 = @strSQL2 + 'iih.total_study_count_std,'
				set @strSQL2 = @strSQL2 + 'iih.total_study_count_stat,'
				set @strSQL2 = @strSQL2 + 'iih.total_disc_amount,'
				set @strSQL2 = @strSQL2 + 'iih.free_read_count,'
				set @strSQL2 = @strSQL2 + 'iih.total_amount,'
				set @strSQL2 = @strSQL2 + 'iih.approved,'
				set @strSQL2 = @strSQL2 + 'action='''' '
				set @strSQL2 = @strSQL2 + 'from ' + @arch_db_name + '..invoice_institution_hdr iih '
				set @strSQL2 = @strSQL2 + 'inner join institutions i on i.id = iih.institution_id '
				set @strSQL2 = @strSQL2 + 'where iih.billing_cycle_id = ''' + convert(varchar(36),@billing_cycle_id) + ''' '
				set @strSQL2 = @strSQL2 + 'order by i.name'

				
				set @strSQL3 = 'select iid.billing_account_id,'
				set @strSQL3 = @strSQL3 + 'iid.billing_cycle_id,'
				set @strSQL3 = @strSQL3 + 'iid.institution_id,'
				set @strSQL3 = @strSQL3 + 'iid.study_id,'
				set @strSQL3 = @strSQL3 + 'iid.study_uid,'
				set @strSQL3 = @strSQL3 + 'sh.received_date,'
				set @strSQL3 = @strSQL3 + 'iid.modality_id,'
				set @strSQL3 = @strSQL3 + 'modality_name = dbo.InitCap(isnull(m.name,''Unkown'')),'
				set @strSQL3 = @strSQL3 + 'patient_name = dbo.InitCap(isnull(sh.patient_name,'''')),'
				set @strSQL3 = @strSQL3 + 'sh.priority_id,'
				set @strSQL3 = @strSQL3 + 'priority_desc = isnull(p.priority_desc,''Unknown''),'
				set @strSQL3 = @strSQL3 + 'iid.image_count,'
				set @strSQL3 = @strSQL3 + 'object_count = isnull(sh.object_count,iid.image_count),'
				set @strSQL3 = @strSQL3 + 'iid.rate,'
				set @strSQL3 = @strSQL3 + 'iid.amount,'
				set @strSQL3 = @strSQL3 + 'service_amount = isnull(iid.service_amount,0),'
				set @strSQL3 = @strSQL3 + 'total_amount   = isnull(iid.total_amount,0),'
				set @strSQL3 = @strSQL3 + 'iid.billed,'
				set @strSQL3 = @strSQL3 + 'iid.approved,'
				set @strSQL3 = @strSQL3 + 'case '
				set @strSQL3 = @strSQL3 + 'when iid.is_free=''Y'' then ''Free credit applied'' '
				set @strSQL3 = @strSQL3 + 'when iid.disc_per_applied>0 then ''Discount of '' + '
				set @strSQL3 = @strSQL3 + 'convert(varchar(6),iid.disc_per_applied)' 
				set @strSQL3 = @strSQL3 + '+ ''% ($''+ '
				set @strSQL3 = @strSQL3 + 'convert(varchar(12),iid.disc_amount)' 
				set @strSQL3 = @strSQL3 + '+'') applied'' '
				set @strSQL3 = @strSQL3 + 'else '''' '
				set @strSQL3 = @strSQL3 + 'end promo_dtls '
				set @strSQL3 = @strSQL3 + 'from ' + @arch_db_name + '..invoice_institution_dtls iid '
				set @strSQL3 = @strSQL3 + 'left outer join modality m on m.id= iid.modality_id '
				set @strSQL3 = @strSQL3 + 'inner join ' + @arch_db_name + '..study_hdr_archive sh on sh.id = iid.study_id '
				set @strSQL3 = @strSQL3 + 'left outer join sys_priority p on p.priority_id = sh.priority_id '
				set @strSQL3 = @strSQL3 + 'where iid.billing_cycle_id= ''' + convert(varchar(36),@billing_cycle_id) + ''' '
				set @strSQL3 = @strSQL3 + 'order by sh.received_date '
				
			end
		else
			begin
				set @strSQL1 = 'select ih.billing_account_id,'
				set @strSQL1 = @strSQL1 + 'ih.billing_cycle_id,'
				set @strSQL1 = @strSQL1 + 'billing_account_name = dbo.InitCap(replace(ba.name,char(39),'''')),'
				set @strSQL1 = @strSQL1 + 'ih.total_study_count,'
				set @strSQL1 = @strSQL1 + 'ih.total_study_count_std,'
				set @strSQL1 = @strSQL1 + 'ih.total_study_count_stat,'
				set @strSQL1 = @strSQL1 + 'ih.total_amount,'
				set @strSQL1 = @strSQL1 + 'ih.approved,'
				set @strSQL1 = @strSQL1 + 'ih.total_disc_amount,'
				set @strSQL1 = @strSQL1 + 'ih.total_free_credits,'
				set @strSQL1 = @strSQL1 + 'action='''' '
				set @strSQL1 = @strSQL1 + 'from ' + @arch_db_name + '..invoice_hdr ih '
				set @strSQL1 = @strSQL1 + 'inner join billing_account ba on ba.id = ih.billing_account_id '
				set @strSQL1 = @strSQL1 + 'where billing_cycle_id = ''' + convert(varchar(36),@billing_cycle_id) + ''' '
				set @strSQL1 = @strSQL1 + 'and ih.billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
				set @strSQL1 = @strSQL1 + 'order by ba.name '

				set @strSQL2 = 'select iih.billing_account_id,'
				set @strSQL2 = @strSQL2 + 'iih.billing_cycle_id,'
				set @strSQL2 = @strSQL2 + 'iih.institution_id,'
				set @strSQL2 = @strSQL2 + 'institution_code = i.code,'
				set @strSQL2 = @strSQL2 + 'institution_name = dbo.InitCap(replace(i.name,char(39),'''')),'
				set @strSQL2 = @strSQL2 + 'iih.total_study_count,'
				set @strSQL2 = @strSQL2 + 'iih.total_study_count_std,'
				set @strSQL2 = @strSQL2 + 'iih.total_study_count_stat,'
				set @strSQL2 = @strSQL2 + 'iih.total_disc_amount,'
				set @strSQL2 = @strSQL2 + 'iih.free_read_count,'
				set @strSQL2 = @strSQL2 + 'iih.total_amount,'
				set @strSQL2 = @strSQL2 + 'iih.approved,'
				set @strSQL2 = @strSQL2 + 'action='''' '
				set @strSQL2 = @strSQL2 + 'from ' + @arch_db_name + '..invoice_institution_hdr iih '
				set @strSQL2 = @strSQL2 + 'inner join institutions i on i.id = iih.institution_id '
				set @strSQL2 = @strSQL2 + 'where iih.billing_cycle_id = ''' + convert(varchar(36),@billing_cycle_id) + ''' '
				set @strSQL2 = @strSQL2 + 'and iih.billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
				set @strSQL2 = @strSQL2 + 'order by i.name'

				set @strSQL3 = 'select iid.billing_account_id,'
				set @strSQL3 = @strSQL3 + 'iid.billing_cycle_id,'
				set @strSQL3 = @strSQL3 + 'iid.institution_id,'
				set @strSQL3 = @strSQL3 + 'iid.study_id,'
				set @strSQL3 = @strSQL3 + 'iid.study_uid,'
				set @strSQL3 = @strSQL3 + 'sh.received_date,'
				set @strSQL3 = @strSQL3 + 'iid.modality_id,'
				set @strSQL3 = @strSQL3 + 'modality_name = dbo.InitCap(isnull(m.name,''Unkown'')),'
				set @strSQL3 = @strSQL3 + 'patient_name = dbo.InitCap(isnull(sh.patient_name,'''')),'
				set @strSQL3 = @strSQL3 + 'sh.priority_id,'
				set @strSQL3 = @strSQL3 + 'priority_desc = isnull(p.priority_desc,''Unknown''),'
				set @strSQL3 = @strSQL3 + 'iid.image_count,'
				set @strSQL3 = @strSQL3 + 'object_count = isnull(sh.object_count,iid.image_count),'
				set @strSQL3 = @strSQL3 + 'iid.rate,'
				set @strSQL3 = @strSQL3 + 'iid.amount,'
				set @strSQL3 = @strSQL3 + 'service_amount = isnull(iid.service_amount,0),'
				set @strSQL3 = @strSQL3 + 'total_amount   = isnull(iid.total_amount,0),'
				set @strSQL3 = @strSQL3 + 'iid.billed,'
				set @strSQL3 = @strSQL3 + 'iid.approved,'
				set @strSQL3 = @strSQL3 + 'case '
				set @strSQL3 = @strSQL3 + 'when iid.is_free=''Y'' then ''Free credit applied'' '
				set @strSQL3 = @strSQL3 + 'when iid.disc_per_applied>0 then ''Discount of '' + '
				set @strSQL3 = @strSQL3 + 'convert(varchar(6),iid.disc_per_applied)' 
				set @strSQL3 = @strSQL3 + '+ ''% ($''+ '
				set @strSQL3 = @strSQL3 + 'convert(varchar(12),iid.disc_amount)' 
				set @strSQL3 = @strSQL3 + '+'') applied'' '
				set @strSQL3 = @strSQL3 + 'else '''' '
				set @strSQL3 = @strSQL3 + 'end promo_dtls '
				set @strSQL3 = @strSQL3 + 'from ' + @arch_db_name + '..invoice_institution_dtls iid '
				set @strSQL3 = @strSQL3 + 'left outer join modality m on m.id= iid.modality_id '
				set @strSQL3 = @strSQL3 + 'inner join ' + @arch_db_name + '.. study_hdr_archive sh on sh.id = iid.study_id '
				set @strSQL3 = @strSQL3 + 'left outer join sys_priority p on p.priority_id = sh.priority_id '
				set @strSQL3 = @strSQL3 + 'where iid.billing_cycle_id= ''' + convert(varchar(36),@billing_cycle_id) + ''' '
				set @strSQL3 = @strSQL3 + 'and iid.billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
				set @strSQL3 = @strSQL3 + 'order by sh.received_date '
			end

	
		--print @strSQL1
		--print @strSQL2
		--print @strSQL3

		exec (@strSQL1)
		exec (@strSQL2)
		exec (@strSQL3)
		
		set nocount off
		return 1
	end
GO
