USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_institution_invoice_hdr_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_institution_invoice_hdr_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_institution_invoice_hdr_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_institution_invoice_hdr_fetch : fetch 
                  institution invoice header
** Created By   : Pavel Guha
** Created On   : 13/11/2019
*******************************************************/
--exec rpt_institution_invoice_hdr_fetch 'D6FC1504-F380-4966-B135-5AEBE40FB683','CB7442AA-F0AE-4BB1-B994-B12F22F8FAA0'
CREATE procedure [dbo].[rpt_institution_invoice_hdr_fetch]
	@billing_cycle_id uniqueidentifier,
	@institution_id uniqueidentifier
as
begin
	 set nocount on

	 declare @DUEDTDAYS int,
	         @arch_db_name nvarchar(30),
			 @strSQL varchar(max)

	 select @DUEDTDAYS = data_value_int from invoicing_control_params where control_code='DUEDTDAYS'
	 select @arch_db_name = arch_db_name from billing_cycle where id=@billing_cycle_id	

	 if(isnull(@arch_db_name,''))=''
		begin
			 select institution_name= dbo.InitCap(i.name),
					invoice_no      = isnull(iih.invoice_no,'TBG'),
					invoice_date    = isnull(iih.invoice_date,getdate()),
					billing_cycle   = convert(varchar(10),bc.date_from,101) + ' - ' + convert(varchar(10),bc.date_till,101),
					due_date        = dateadd(d,@DUEDTDAYS,isnull(iih.invoice_date,getdate()))
			 from invoice_institution_hdr iih
			 inner join institutions i on i.id = iih.institution_id
			 inner join billing_cycle bc on bc.id = iih.billing_cycle_id
			 where iih.institution_id = @institution_id
			 and iih.billing_cycle_id = @billing_cycle_id
		end
	else
		begin
			set @strSQL ='select institution_name= dbo.InitCap(i.name),'
			set @strSQL = @strSQL + 'invoice_no      = isnull(iih.invoice_no,''TBG''),'
			set @strSQL = @strSQL + 'invoice_date    = isnull(iih.invoice_date,getdate()),'
			set @strSQL = @strSQL + 'billing_cycle   = convert(varchar(10),bc.date_from,101) + '' - '' + convert(varchar(10),bc.date_till,101),'
			set @strSQL = @strSQL + 'due_date        = iih.invoice_date '
			set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_institution_hdr iih '
			set @strSQL = @strSQL + 'inner join institutions i on i.id = iih.institution_id '
			set @strSQL = @strSQL + 'inner join billing_cycle bc on bc.id = iih.billing_cycle_id '
			set @strSQL = @strSQL + 'where iih.institution_id = ''' + convert(varchar(36),@institution_id) + ''' '
			set @strSQL = @strSQL + 'and iih.billing_cycle_id = ''' + convert(varchar(36),@billing_cycle_id) + ''' '

			exec(@strSQL)
		end

		
	set nocount off
end

GO
