USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_billing_account_invoice_hdr_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_billing_account_invoice_hdr_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_billing_account_invoice_hdr_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_billing_account_invoice_hdr_fetch : fetch 
                  institution invoice header
** Created By   : Pavel Guha
** Created On   : 13/11/2019
*******************************************************/
--exec rpt_billing_account_invoice_hdr_fetch 'D6FC1504-F380-4966-B135-5AEBE40FB683','3E9961FE-32AC-48A8-B0BE-7AEEC60E14D7'
CREATE procedure [dbo].[rpt_billing_account_invoice_hdr_fetch]
	@billing_cycle_id uniqueidentifier,
	@billing_account_id uniqueidentifier
as
begin
	 set nocount on

	 declare @DUEDTDAYS int,
	         @arch_db_name nvarchar(30),
			 @strSQL varchar(max)

	 select @DUEDTDAYS = data_value_int from invoicing_control_params where control_code='DUEDTDAYS'
	 select @arch_db_name = arch_db_name from billing_cycle where id=@billing_cycle_id
	 
	 if(isnull(@arch_db_name,'') ='')
		begin
			 select account_name    = dbo.InitCap(ba.name),
					address_1       = dbo.InitCap(ba.address_1),
					address_2       = dbo.InitCap(ba.address_2),
					city            = dbo.InitCap(ba.city),
					state_name      = dbo.InitCap(isnull(s.name,'')),
					country_name    = dbo.InitCap(isnull(c.name,'')),
					zip             = isnull(ba.zip,''),
					invoice_no      = isnull(ih.invoice_no,'TBG'),
					invoice_date    = isnull(ih.invoice_date,getdate()),
					billing_cycle   = convert(varchar(10),bc.date_from,101) + ' - ' + convert(varchar(10),bc.date_till,101),
					due_date        = dateadd(d,@DUEDTDAYS,isnull(ih.invoice_date,getdate())),
					approved        = ih.approved
			 from invoice_hdr ih
			 inner join billing_account ba on ba.id = ih.billing_account_id
			 inner join billing_cycle bc on bc.id = ih.billing_cycle_id
			 left outer join sys_states s on s.id = ba.state_id
			 left outer join sys_country c on c.id = ba.country_id
			 where ih.billing_account_id = @billing_account_id
			 and ih.billing_cycle_id = @billing_cycle_id
		end
	else
		begin
			set @strSQL = 'select account_name       = dbo.InitCap(ba.name),'
			set @strSQL = @strSQL + 'address_1       = dbo.InitCap(ba.address_1),'
			set @strSQL = @strSQL + 'address_2       = dbo.InitCap(ba.address_2),'
			set @strSQL = @strSQL + 'city            = dbo.InitCap(ba.city),'
			set @strSQL = @strSQL + 'state_name      = dbo.InitCap(isnull(s.name,'''')),'
			set @strSQL = @strSQL + 'country_name    = dbo.InitCap(isnull(c.name,'''')),'
			set @strSQL = @strSQL +  'zip             = isnull(ba.zip,''''),'
			set @strSQL = @strSQL + 'invoice_no      = isnull(ih.invoice_no,''TBG''),'
			set @strSQL = @strSQL + 'invoice_date    = isnull(ih.invoice_date,getdate()),'
			set @strSQL = @strSQL + 'billing_cycle   = convert(varchar(10),bc.date_from,101) + '' - '' + convert(varchar(10),bc.date_till,101),'
			set @strSQL = @strSQL + 'due_date        = ih.invoice_due_date,'
			set @strSQL = @strSQL + 'approved        = ih.approved '
			set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_hdr ih '
			set @strSQL = @strSQL + 'inner join billing_account ba on ba.id = ih.billing_account_id '
			set @strSQL = @strSQL + 'inner join billing_cycle bc on bc.id = ih.billing_cycle_id '
			set @strSQL = @strSQL + 'left outer join sys_states s on s.id = ba.state_id '
			set @strSQL = @strSQL + 'left outer join sys_country c on c.id = ba.country_id '
			set @strSQL = @strSQL + 'where ih.billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
			set @strSQL = @strSQL + 'and ih.billing_cycle_id = ''' + convert(varchar(36),@billing_cycle_id) + ''' '

			exec(@strSQL)
		end
		
	set nocount off
end

GO
