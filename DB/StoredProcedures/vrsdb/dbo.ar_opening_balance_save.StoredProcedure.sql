USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_opening_balance_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_opening_balance_save]
GO
/****** Object:  StoredProcedure [dbo].[ar_opening_balance_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_opening_balance : save
** Created By   : KC
** Created On   : 30/04/2020
*******************************************************/
CREATE procedure [dbo].[ar_opening_balance_save]
(
	@id						uniqueidentifier	= '00000000-0000-0000-0000-000000000000' output,
	@billing_account_id		uniqueidentifier,
	@opbal_date  date,
	@invoice_no nvarchar(30),
	@opbal_amount  money,
	@isadjusted bit,
	@user_id				uniqueidentifier,
    @menu_id                int,
    @user_name              nvarchar(700)		= '' output,
	@error_code				nvarchar(10)		= '' output,
    @return_status			int					= 0  output
)
as
	begin
		
		begin transaction

		if(@id = '00000000-0000-0000-0000-000000000000')
			begin
				 set @id = newid();
				
				 insert into ar_opening_balance(
					id, 
					billing_account_id, 
					opbal_date, 
					invoice_no, 
					opbal_amount, 
					isadjusted, 
					created_by,
					date_created, 
					updated_by, 
					date_updated)
				select 
					@id,
					@billing_account_id,
					@opbal_date,
					@invoice_no,
					@opbal_amount, 
					@isadjusted, 
					@user_id, 
					GETDATE(),
					NULL,
					NULL
				

				if(@@rowcount=0)
				begin
					rollback transaction
					select	@return_status=0,@error_code='035'
					return 0
				end
			end
		else
			begin
				
				exec common_check_record_lock_ui
				@menu_id       = @menu_id,
				@record_id     = @id,
				@user_id       = @user_id,
				@user_name     = @user_name output,
				@error_code    = @error_code output,
				@return_status = @return_status output
		
				if(@return_status=0)
					begin
						rollback transaction
						return 0
					end

				UPDATE dbo.ar_opening_balance
					   SET 
						   opbal_date       = @opbal_date,
						   invoice_no		= @invoice_no,
						   opbal_amount     = @opbal_amount,
						   isadjusted		= @isadjusted,
						   updated_by       = @user_id,
						   date_updated     = GETDATE()
					 WHERE id = @id
				if(@@rowcount=0)
				begin
					rollback transaction
					select	@return_status=0,@error_code='035'
					return 0
				end

			end

		commit transaction

		set @return_status=1
		set @error_code='034'
		set nocount off

		return 1
	end

GO
