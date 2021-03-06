USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[billing_account_vault_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[billing_account_vault_save]
GO
/****** Object:  StoredProcedure [dbo].[billing_account_vault_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : sav : save
** Created By   : KC
** Created On   : 03/05/2020
*******************************************************/
CREATE procedure [dbo].[billing_account_vault_save]
(
	@id						uniqueidentifier	= '00000000-0000-0000-0000-000000000000' output,
	@billing_account_id		uniqueidentifier,
	@vault_id       uniqueidentifier,
	@vault_type     nvarchar(5) = 'card',
	@vault_card     nvarchar(19) = NULL,
	@vault_card_type nvarchar(10) = NULL,
	@vault_exp      nvarchar(5) = NULL,
	@vault_name     nvarchar(50) = NULL,
	@vault_account  nvarchar(30) = NULL,
	@vault_aba  nvarchar(30) = NULL,
	@holder_name nvarchar(100) ='',
	@user_id				uniqueidentifier,
    @menu_id                int,
	@error_code				nvarchar(10)		= '' output,
    @return_status			int					= 0  output
)
as
	begin
		
		begin transaction

			set @id = newid();
				 
			insert into [billing_account_vault]
			   ([id]
			   ,[billing_account_id]
			   ,[vault_id]
			   ,[vault_type]
			   ,[vault_card]
			   ,[vault_card_type]
			   ,[vault_exp]
			   ,[vault_name]
			   ,[vault_account]
			   ,[vault_aba]
			   ,[holder_name]
			   ,[created_by]
			   ,[date_created])
     
			select 
				@id,
				@billing_account_id,
				@vault_id,
				@vault_type,
				@vault_card,
				@vault_card_type,
				@vault_exp,
				@vault_name,
				@vault_account,
				@vault_aba,
				@holder_name,
				@user_id,
				GETDATE()

			if(@@rowcount=0)
			begin
				rollback transaction
				select	@return_status=0,@error_code='035'
				return 0
			end
		

		commit transaction

		set @return_status=1
		set @error_code='034'
		set nocount off

		return 1
	end

GO
