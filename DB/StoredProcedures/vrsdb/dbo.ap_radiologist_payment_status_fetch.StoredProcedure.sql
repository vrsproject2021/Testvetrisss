USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ap_radiologist_payment_status_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ap_radiologist_payment_status_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ap_radiologist_payment_status_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ap_radiologist_payment_status_fetch : 
                  fetch invoicing processing details 
				  if locked by other user
** Created By   : Pavel Guha 
** Created On   : 12/11/2019
*******************************************************/
--exec ap_radiologist_payment_status_fetch 'CD6E2FE4-9AC6-4010-9A9B-08EB687B09DD','BE7CFCBE-BF98-407B-9FB4-660F7534FA4B'
--exec ap_radiologist_payment_status_fetch 'CD6E2FE4-9AC6-4010-9A9B-08EB687B09DD','00000000-0000-0000-0000-000000000000'
create procedure [dbo].[ap_radiologist_payment_status_fetch]
	@billing_cycle_id uniqueidentifier,
	@radiologist_id uniqueidentifier='00000000-0000-0000-0000-000000000000'
	
as
	begin
		set nocount on
		declare @approve_count int,
				@process_count int

		if(@radiologist_id ='00000000-0000-0000-0000-000000000000')
			begin

				select @process_count = count(id)
				from ap_radiologist_payment_hdr
				where billing_cycle_id=@billing_cycle_id

				select @approve_count = count(id)
				from ap_radiologist_payment_hdr
				where billing_cycle_id=@billing_cycle_id
				and approved='Y'

			end
		else
			begin
				select @process_count = count(id)
				from ap_radiologist_payment_hdr
				where billing_cycle_id=@billing_cycle_id
				and radiologist_id = @radiologist_id

				select @approve_count = count(id)
				from ap_radiologist_payment_hdr
				where billing_cycle_id=@billing_cycle_id
				and approved='Y'
				and radiologist_id = @radiologist_id
			end

		select approve_count = @approve_count,
		       process_count = @process_count

		set nocount off
		--print @process_count
		--print @approve_count
	end
GO
