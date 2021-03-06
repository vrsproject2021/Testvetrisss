USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_billing_account_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_billing_account_update]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_billing_account_update]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_billing_account_update : 
				  update billing account's quick books table
** Created By   : Pavel Guha
** Created On   : 23/06/2020
*******************************************************/
CREATE procedure [dbo].[scheduler_billing_account_update]
	@billing_account_id uniqueidentifier= '00000000-0000-0000-0000-000000000000',
	@debtor_id nvarchar(20)= '',
	@error_msg nvarchar(500)='' output,
	@return_type int=0 output
as
begin
	set nocount on

	declare @billing_account_name nvarchar(100),
	        @old_debtor_id nvarchar(20)
	
	    begin transaction

		select @billing_account_name = name,
		       @old_debtor_id = debtor_id
		from billing_account 
		where id=@billing_account_id

		if(@old_debtor_id<>@debtor_id)
			begin
				update billing_account
				set debtor_id   = @debtor_id,
					qb_name     = @billing_account_name,
					update_qb   = 'N', 
					update_qb_on=getdate()
				where id=@billing_account_id
			end
		else
			begin
				update billing_account
				set debtor_id   = @debtor_id,
					update_qb   = 'N', 
					update_qb_on=getdate()
				where id=@billing_account_id
			end

		if(@@rowcount=0)
			begin
				rollback transaction
				select @error_msg='Failed to update the billing account : '+ @billing_account_name,
				       @return_type=0
				return 0
			end

		commit transaction

	set nocount off
	select @error_msg='',@return_type=1
	return 1

end

GO
