USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_process_status_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_process_status_fetch]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_process_status_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_process_status_fetch : 
                  fetch invoicing processing details 
				  if locked by other user
** Created By   : Pavel Guha 
** Created On   : 12/11/2019
*******************************************************/
--exec invoicing_process_status_fetch 'CD6E2FE4-9AC6-4010-9A9B-08EB687B09DD','1A1D9DEE-8D88-4ACB-9861-B0A254C30E34',0,0
--exec invoicing_process_status_fetch 'CD6E2FE4-9AC6-4010-9A9B-08EB687B09DD','00000000-0000-0000-0000-000000000000'
--exec invoicing_process_status_fetch 'CD6E2FE4-9AC6-4010-9A9B-08EB687B09DD','98A30426-BA37-4564-98D0-A2CF6A4B9929',0,0
create procedure [dbo].[invoicing_process_status_fetch]
	@billing_cycle_id uniqueidentifier,
	@billing_account_id uniqueidentifier='00000000-0000-0000-0000-000000000000'
	
as
	begin
		set nocount on
		declare @approve_count int,
				@process_count int

		if(@billing_account_id ='00000000-0000-0000-0000-000000000000')
			begin

				select @process_count = count(id)
				from invoice_institution_dtls
				where billing_cycle_id=@billing_cycle_id

				select @approve_count = count(id)
				from invoice_institution_dtls
				where billing_cycle_id=@billing_cycle_id
				and approved='Y'

			end
		else
			begin
				select @process_count = count(id)
				from invoice_institution_dtls
				where billing_cycle_id=@billing_cycle_id
				and billing_account_id = @billing_account_id

				select @approve_count = count(id)
				from invoice_institution_dtls
				where billing_cycle_id=@billing_cycle_id
				and approved='Y'
				and billing_account_id = @billing_account_id
			end

		select approve_count = @approve_count,
		       process_count = @process_count

		set nocount off
		--print @process_count
		--print @approve_count
	end
GO
