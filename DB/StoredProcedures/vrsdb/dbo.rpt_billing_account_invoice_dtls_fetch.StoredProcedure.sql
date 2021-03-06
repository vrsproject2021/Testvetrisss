USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_billing_account_invoice_dtls_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_billing_account_invoice_dtls_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_billing_account_invoice_dtls_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_billing_account_invoice_dtls_fetch : fetch 
                  billing_account invoice details
** Created By   : Pavel Guha
** Created On   : 13/11/2019
*******************************************************/
--exec rpt_billing_account_invoice_dtls_fetch 'D6FC1504-F380-4966-B135-5AEBE40FB683','AB352297-C935-4E57-9CA9-D7C146B15914'
--exec rpt_billing_account_invoice_dtls_fetch '3225FD47-3FF5-4863-8E89-22E960263EB3','3E9961FE-32AC-48A8-B0BE-7AEEC60E14D7'
--exec rpt_billing_account_invoice_dtls_fetch '3225FD47-3FF5-4863-8E89-22E960263EB3','AB352297-C935-4E57-9CA9-D7C146B15914'
--exec rpt_billing_account_invoice_dtls_fetch '3225FD47-3FF5-4863-8E89-22E960263EB3','5B7B3B0A-5807-42AA-A80F-FD3AABE7D816'
--exec rpt_billing_account_invoice_dtls_fetch '3225FD47-3FF5-4863-8E89-22E960263EB3','DAB2C254-B5E5-4E86-ACEA-2418133988B1'
--exec rpt_billing_account_invoice_dtls_fetch '3225FD47-3FF5-4863-8E89-22E960263EB3','4F723EF9-641E-4CFC-B13E-A1B43ADEF66F'
--exec rpt_billing_account_invoice_dtls_fetch 'D6FC1504-F380-4966-B135-5AEBE40FB683','AB352297-C935-4E57-9CA9-D7C146B15914'
CREATE procedure [dbo].[rpt_billing_account_invoice_dtls_fetch]
	@billing_cycle_id uniqueidentifier,
	@billing_account_id uniqueidentifier
as
begin
	 set nocount on

	 declare @rc int,
	         @arch_db_name nvarchar(30),
			 @strSQL varchar(max)

	 create table #tmp
	 (
	    institution_id uniqueidentifier,
		institution_name nvarchar(100),
		item_type nchar(1),
		item_id int,
		item_name nvarchar(30),
		qty int null default 0,
		free_credits int null default 0,
		free_credit_amount money null default 0,
		discount_amount money null default 0,
		total_discount money null default 0,
		total_qty int null default 0,
		amount money null default 0,
		service_amount money null default 0,
		total_amount money null default 0,
		institution_count int default 0,
		item_count int default 0
	 )

	 select @arch_db_name = arch_db_name from billing_cycle where id=@billing_cycle_id
	 if(isnull(@arch_db_name,''))=''
		begin
			set @arch_db_name='vrsdb'
		end

	 set @strSQL = 'insert into #tmp(institution_id,institution_name,item_type,item_id,item_name)'
	 set @strSQL = @strSQL + '(select distinct iid.institution_id,institution_name = dbo.InitCap(i.name),''M'',iid.modality_id, modality_name= dbo.InitCap(m.name) '
	 set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls iid '
	 set @strSQL = @strSQL + 'inner join institutions i on i.id= iid.institution_id '
	 set @strSQL = @strSQL + 'inner join modality m on m.id = iid.modality_id '
	 set @strSQL = @strSQL + 'where iid.billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
	 set @strSQL = @strSQL + 'and iid.billing_cycle_id     = ''' + convert(varchar(36),@billing_cycle_id) + ''') '
	 set @strSQL = @strSQL + 'order by institution_name,modality_name'

	 exec(@strSQL)
	 set @rc=@@rowcount

	 set @strSQL = 'insert into #tmp(institution_id,institution_name,item_type,item_id,item_name)'
	 set @strSQL = @strSQL + '(select distinct isd.institution_id,institution_name = dbo.InitCap(i.name),''S'',isd.service_id, service_name= dbo.InitCap(s.name) '
	 set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_service_dtls isd '
	 set @strSQL = @strSQL + 'inner join institutions i on i.id= isd.institution_id '
	 set @strSQL = @strSQL + 'inner join services s on s.id = isd.service_id '
	 set @strSQL = @strSQL + 'where isd.billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
	 set @strSQL = @strSQL + 'and isd.billing_cycle_id     = ''' + convert(varchar(36),@billing_cycle_id) + ''') '
	 set @strSQL = @strSQL + 'order by institution_name,service_name'

	 exec(@strSQL)
	 set @rc=@rc + @@rowcount

	 if(@@rowcount=0)
		begin
			set @strSQL = 'insert into #tmp(institution_id,institution_name, item_id,item_name)'
			set @strSQL = @strSQL + '(select distinct ih.institution_id,institution_name = dbo.InitCap(i.name),0, modality_name= ''N/A'' '
			set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_institution_hdr ih '
			set @strSQL = @strSQL + 'inner join institutions i on i.id= ih.institution_id '
			set @strSQL = @strSQL + 'where ih.billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
			set @strSQL = @strSQL + 'and ih.billing_cycle_id     = ''' + convert(varchar(36),@billing_cycle_id) + ''') '
			set @strSQL = @strSQL + 'order by institution_name,modality_name'

			exec(@strSQL)
		end

	-- qty
	 set @strSQL = 'update #tmp '
	 set @strSQL = @strSQL + 'set qty = isnull((select count(id) '
	 set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls '
	 set @strSQL = @strSQL + 'where modality_id = #tmp.item_id '
	 set @strSQL = @strSQL + 'and billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
	 set @strSQL = @strSQL + 'and institution_id = #tmp.institution_id '
	 set @strSQL = @strSQL + 'and billing_cycle_id=''' + convert(varchar(36),@billing_cycle_id) + '''),0) '
	 set @strSQL = @strSQL + 'where item_type = ''M'' '


	 exec(@strSQL)

	set @strSQL = 'update #tmp '
	set @strSQL = @strSQL + 'set qty = isnull((select count(id) '
	set @strSQL = @strSQL + 'from invoice_service_dtls '
	set @strSQL = @strSQL + 'where service_id = #tmp.item_id '
	set @strSQL = @strSQL + 'and billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
	set @strSQL = @strSQL + 'and institution_id = #tmp.institution_id '
	set @strSQL = @strSQL + 'and billing_cycle_id=''' + convert(varchar(36),@billing_cycle_id) + '''),0) '
	set @strSQL = @strSQL + 'where item_type = ''S'' '

	exec(@strSQL)

	--free credits (modality)
	set @strSQL =  'update #tmp '
	set @strSQL = @strSQL + 'set free_credits = isnull((select count(is_free) '
	set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls '
	set @strSQL = @strSQL + 'where modality_id = #tmp.item_id '
	set @strSQL = @strSQL + 'and billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
	set @strSQL = @strSQL + 'and institution_id = #tmp.institution_id '
	set @strSQL = @strSQL + 'and billing_cycle_id=''' + convert(varchar(36),@billing_cycle_id) + ''' '
	set @strSQL = @strSQL + 'and is_free=''Y'' '
	set @strSQL = @strSQL + 'and disc_per_applied=0),0) '
	set @strSQL = @strSQL + 'where item_type = ''M'' '

	exec(@strSQL)

	update #tmp
	set total_qty = qty - free_credits
	where item_type = 'M'

	set @strSQL =  'update #tmp '
	set @strSQL = @strSQL + 'set free_credits = isnull((select count(is_free) '
	set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls '
	set @strSQL = @strSQL + 'where modality_id = #tmp.item_id '
	set @strSQL = @strSQL + 'and billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
	set @strSQL = @strSQL + 'and institution_id = #tmp.institution_id '
	set @strSQL = @strSQL + 'and billing_cycle_id=''' + convert(varchar(36),@billing_cycle_id) + ''' '
	set @strSQL = @strSQL + 'and is_free=''N'' '
	set @strSQL = @strSQL + 'and disc_per_applied>0),0) '
	set @strSQL = @strSQL + 'where item_type = ''M'' '

	exec(@strSQL)

	--free credit amount (Modality)
	set @strSQL =  'update #tmp '
	set @strSQL = @strSQL + 'set free_credit_amount = isnull((select sum(study_price) '
	set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls '
	set @strSQL = @strSQL + 'where modality_id = #tmp.item_id '
	set @strSQL = @strSQL + 'and billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
	set @strSQL = @strSQL + 'and institution_id = #tmp.institution_id '
	set @strSQL = @strSQL + 'and billing_cycle_id=''' + convert(varchar(36),@billing_cycle_id) + ''' '
	set @strSQL = @strSQL + 'and is_free=''Y''),0) '
	set @strSQL = @strSQL + 'where item_type = ''M'' '

	exec(@strSQL)

	--update #tmp
	--set free_credit_amount = free_credit_amount + isnull((select sum(service_price)
	--							   from invoice_institution_dtls
	--							   where modality_id = #tmp.item_id
	--							   and billing_account_id = @billing_account_id
	--							   and institution_id = #tmp.institution_id
	--							   and billing_cycle_id=@billing_cycle_id
	--							   and is_free='Y'),0)
	--where item_type = 'M'

	--dicount amount  (Modality)

	--set @strSQL =  'update #tmp '
	--set @strSQL = @strSQL + 'set discount_amount = isnull((select sum((disc_per_applied * study_price)/100) '
	--set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls '
	--set @strSQL = @strSQL + 'where modality_id = #tmp.item_id '
	--set @strSQL = @strSQL + 'and billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
	--set @strSQL = @strSQL + 'and institution_id = #tmp.institution_id '
	--set @strSQL = @strSQL + 'and billing_cycle_id=''' + convert(varchar(36),@billing_cycle_id) + '''),0) '
	--set @strSQL = @strSQL + 'where item_type = ''M'' '

	set @strSQL =  'update #tmp '
	set @strSQL = @strSQL + 'set discount_amount = isnull((select sum(disc_amount) '
	set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls '
	set @strSQL = @strSQL + 'where modality_id = #tmp.item_id '
	set @strSQL = @strSQL + 'and billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
	set @strSQL = @strSQL + 'and institution_id = #tmp.institution_id '
	set @strSQL = @strSQL + 'and billing_cycle_id=''' + convert(varchar(36),@billing_cycle_id) + '''),0) '
	set @strSQL = @strSQL + 'where item_type = ''M'' '

	exec(@strSQL)

	update #tmp
	set total_discount = free_credit_amount + discount_amount
	where item_type = 'M'

	--amount  (Modality)
	set @strSQL =  'update #tmp '
	set @strSQL = @strSQL + 'set amount = isnull((select sum(amount) '
	set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls '
	set @strSQL = @strSQL + 'where modality_id = #tmp.item_id '
	set @strSQL = @strSQL + 'and billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
	set @strSQL = @strSQL + 'and institution_id = #tmp.institution_id '
	set @strSQL = @strSQL + 'and billing_cycle_id=''' + convert(varchar(36),@billing_cycle_id) + '''),0) '
	set @strSQL = @strSQL + 'where item_type = ''M'' '

	exec(@strSQL)

	--free credits (services)
	set @strSQL =  'update #tmp '
	set @strSQL = @strSQL + 'set free_credits = isnull((select count(is_free) '
	set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_service_dtls '
	set @strSQL = @strSQL + 'where service_id = #tmp.item_id '
	set @strSQL = @strSQL + 'and billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
	set @strSQL = @strSQL + 'and institution_id = #tmp.institution_id '
	set @strSQL = @strSQL + 'and billing_cycle_id=''' + convert(varchar(36),@billing_cycle_id) + ''' '
	set @strSQL = @strSQL + 'and is_free=''Y'' '
	set @strSQL = @strSQL + 'and disc_per_applied=0),0) '
	set @strSQL = @strSQL + 'where item_type = ''S'' '

	exec(@strSQL)

	set @strSQL =  'update #tmp '
	set @strSQL = @strSQL + 'set free_credits = free_credits + isnull((select count(id) '
	set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_service_dtls '
	set @strSQL = @strSQL + 'where service_id = #tmp.item_id '
	set @strSQL = @strSQL + 'and billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
	set @strSQL = @strSQL + 'and institution_id = #tmp.institution_id '
	set @strSQL = @strSQL + 'and billing_cycle_id=''' + convert(varchar(36),@billing_cycle_id) + ''' '
	set @strSQL = @strSQL + 'and is_free=''N'' '
	set @strSQL = @strSQL + 'and disc_per_applied>0),0) '
	set @strSQL = @strSQL + 'where item_type = ''S'' '

	exec(@strSQL)

	--free credit amount (services)
	set @strSQL =  'update #tmp '
	set @strSQL = @strSQL + 'set free_credit_amount = isnull((select sum(service_price) '
	set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_service_dtls '
	set @strSQL = @strSQL + 'where service_id = #tmp.item_id '
	set @strSQL = @strSQL + 'and billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
	set @strSQL = @strSQL + 'and institution_id = #tmp.institution_id '
	set @strSQL = @strSQL + 'and billing_cycle_id=''' + convert(varchar(36),@billing_cycle_id) + ''' '
	set @strSQL = @strSQL + 'and is_free=''Y''),0)'
	set @strSQL = @strSQL + 'where item_type = ''S'' '

	exec(@strSQL)

	--dicount amount (services)
	set @strSQL =  'update #tmp '
	set @strSQL = @strSQL + 'set discount_amount = isnull((select sum((disc_per_applied * service_price)/100) '
	set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_service_dtls '
	set @strSQL = @strSQL + 'where service_id = #tmp.item_id '
	set @strSQL = @strSQL + 'and billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
	set @strSQL = @strSQL + 'and institution_id = #tmp.institution_id '
	set @strSQL = @strSQL + 'and billing_cycle_id=''' + convert(varchar(36),@billing_cycle_id) + '''),0) '
	set @strSQL = @strSQL + 'where item_type = ''S'' '

	exec(@strSQL)

	--amount  (Services)
	set @strSQL =  'update #tmp '
	set @strSQL = @strSQL + 'set amount = isnull((select sum(amount) '
	set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_service_dtls '
	set @strSQL = @strSQL + 'where service_id = #tmp.item_id '
	set @strSQL = @strSQL + 'and billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
	set @strSQL = @strSQL + 'and institution_id = #tmp.institution_id '
	set @strSQL = @strSQL + 'and billing_cycle_id=''' + convert(varchar(36),@billing_cycle_id) + '''),0) '
	set @strSQL = @strSQL + 'where item_type = ''S'' '

	exec(@strSQL)

	update #tmp
	set total_discount = free_credit_amount + discount_amount
	where item_type = 'S'

	--Service Amount
	set @strSQL =  'update #tmp '
	set @strSQL = @strSQL + 'set service_amount = isnull((select sum(service_amount) '
	set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls '
	set @strSQL = @strSQL + 'where modality_id = #tmp.item_id '
	set @strSQL = @strSQL + 'and billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
	set @strSQL = @strSQL + 'and institution_id = #tmp.institution_id '
	set @strSQL = @strSQL + 'and billing_cycle_id=''' + convert(varchar(36),@billing_cycle_id) + '''),0) '
	set @strSQL = @strSQL + 'where item_type = ''M'' '
								 
	exec(@strSQL)
	
	
	update #tmp set total_amount = amount + service_amount where item_type = 'M'

	update #tmp set institution_count = (select count(distinct institution_id) from #tmp)

	update #tmp
	set item_count = isnull((select count(item_id)
								   from #tmp t1
								   where t1.institution_id = #tmp.institution_id),0)
	where item_type = 'M'


	select * from #tmp order by institution_name

	drop table #tmp

	set nocount off
end

GO
