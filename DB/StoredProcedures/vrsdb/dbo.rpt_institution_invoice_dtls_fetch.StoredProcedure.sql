USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_institution_invoice_dtls_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_institution_invoice_dtls_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_institution_invoice_dtls_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_institution_invoice_dtls_fetch : fetch 
                  institution invoice details
** Created By   : Pavel Guha
** Created On   : 13/11/2019
*******************************************************/
--exec rpt_institution_invoice_dtls_fetch 'D6FC1504-F380-4966-B135-5AEBE40FB683','CB7442AA-F0AE-4BB1-B994-B12F22F8FAA0'
CREATE procedure [dbo].[rpt_institution_invoice_dtls_fetch]
	@billing_cycle_id uniqueidentifier,
	@institution_id uniqueidentifier
as
begin
	 set nocount on

	 declare @arch_db_name nvarchar(30),
			 @strSQL varchar(max)

	 create table #tmp
	 (
		modality_id int,
		modality_name nvarchar(30),
		qty int null default 0,
		free_credits int null default 0,
		total_qty int null default 0,
		free_credit_amount money null default 0,
		discount_amount money null default 0,
		total_discount money null default 0,
		amount money null default 0,
		service_amount money null default 0,
		total_amount money null default 0
	 )

	 select @arch_db_name = arch_db_name from billing_cycle where id=@billing_cycle_id
	 if(isnull(@arch_db_name,''))=''
		begin
			set @arch_db_name='vrsdb'
		end

	 insert into #tmp(modality_id,modality_name) 
	 (select id,dbo.InitCap(name) from modality where is_active='Y') order by name

	 set @strSQL ='update #tmp '
	 set @strSQL =@strSQL + 'set qty = isnull((select count(id) '
	 set @strSQL =@strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls '
	 set @strSQL =@strSQL + 'where modality_id = #tmp.modality_id '
	 set @strSQL =@strSQL + 'and institution_id = ''' + convert(varchar(36),@institution_id) + ''' '
	 set @strSQL =@strSQL + 'and billing_cycle_id= ''' + convert(varchar(36),@billing_cycle_id) + '''),0)'
	 exec(@strSQL)

	set @strSQL ='update #tmp '
	set @strSQL =@strSQL + 'set free_credits = isnull((select count(is_free) '
	set @strSQL =@strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls '
	set @strSQL =@strSQL + 'where modality_id = #tmp.modality_id '
	set @strSQL =@strSQL + 'and institution_id = ''' + convert(varchar(36),@institution_id) + ''' '
	set @strSQL =@strSQL + 'and billing_cycle_id= ''' + convert(varchar(36),@billing_cycle_id) + ''' '
	set @strSQL =@strSQL + 'and is_free=''Y''),0)'
	exec(@strSQL)

	update #tmp 
	set total_qty = qty - free_credits

    set @strSQL ='update #tmp '
	set @strSQL =@strSQL + 'set free_credits = free_credits + isnull((select count(id) '
	set @strSQL =@strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls '
	set @strSQL =@strSQL + 'where modality_id = #tmp.modality_id '
	set @strSQL =@strSQL + 'and institution_id = ''' + convert(varchar(36),@institution_id) + ''' '
	set @strSQL =@strSQL + 'and billing_cycle_id= ''' + convert(varchar(36),@billing_cycle_id) + ''' '
	set @strSQL =@strSQL + 'and is_free=''N'' '
	set @strSQL =@strSQL + 'and disc_per_applied>0),0)'
	exec(@strSQL)

	set @strSQL ='update #tmp '
	set @strSQL =@strSQL + 'set free_credit_amount = isnull((select sum(study_price) '
	set @strSQL =@strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls '
	set @strSQL =@strSQL + 'where modality_id = #tmp.modality_id '
	set @strSQL =@strSQL + 'and institution_id = ''' + convert(varchar(36),@institution_id) + ''' '
	set @strSQL =@strSQL + 'and billing_cycle_id= ''' + convert(varchar(36),@billing_cycle_id) + ''' '
	set @strSQL =@strSQL + 'and is_free=''Y''),0)'
	
	exec(@strSQL)

	set @strSQL ='update #tmp '
	set @strSQL =@strSQL + 'set free_credit_amount = free_credit_amount + isnull((select sum(service_price) '
	set @strSQL =@strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls '
	set @strSQL =@strSQL + 'where modality_id = #tmp.modality_id '
	set @strSQL =@strSQL + 'and institution_id = ''' + convert(varchar(36),@institution_id) + ''' '
	set @strSQL =@strSQL + 'and billing_cycle_id= ''' + convert(varchar(36),@billing_cycle_id) + ''' '
	set @strSQL =@strSQL + 'and is_free=''Y''),0)'
	
	exec(@strSQL)

	set @strSQL ='update #tmp '
	set @strSQL =@strSQL + 'set discount_amount = isnull((select sum(disc_amount) '
	set @strSQL =@strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls '
	set @strSQL =@strSQL + 'where modality_id = #tmp.modality_id '
	set @strSQL =@strSQL + 'and institution_id = ''' + convert(varchar(36),@institution_id) + ''' '
	set @strSQL =@strSQL + 'and billing_cycle_id= ''' + convert(varchar(36),@billing_cycle_id) + '''),0)'
	
	exec(@strSQL)

	update #tmp
	set total_discount = free_credit_amount + discount_amount

	

	set @strSQL ='update #tmp '
	set @strSQL =@strSQL + 'set amount = isnull((select sum(amount) '
	set @strSQL =@strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls '
	set @strSQL =@strSQL + 'where modality_id = #tmp.modality_id '
	set @strSQL =@strSQL + 'and institution_id = ''' + convert(varchar(36),@institution_id) + ''' '
	set @strSQL =@strSQL + 'and billing_cycle_id= ''' + convert(varchar(36),@billing_cycle_id) + '''),0)'
	exec(@strSQL)


	set @strSQL ='update #tmp '
	set @strSQL =@strSQL + 'set service_amount = isnull((select sum(service_amount) '
	set @strSQL =@strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls '
	set @strSQL =@strSQL + 'where modality_id = #tmp.modality_id '
	set @strSQL =@strSQL + 'and institution_id = ''' + convert(varchar(36),@institution_id) + ''' '
	set @strSQL =@strSQL + 'and billing_cycle_id= ''' + convert(varchar(36),@billing_cycle_id) + '''),0)'
	exec(@strSQL)

	

   update #tmp set total_amount = amount + service_amount


	select * from #tmp

	drop table #tmp

	set nocount off
end

GO
