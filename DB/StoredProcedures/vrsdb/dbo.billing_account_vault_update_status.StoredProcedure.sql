USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[billing_account_vault_update_status]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[billing_account_vault_update_status]
GO
/****** Object:  StoredProcedure [dbo].[billing_account_vault_update_status]    Script Date: 28-09-2021 19:36:34 ******/
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
-- exec [dbo].[billing_account_vault_update_status] '47324d45-0e9f-46ec-9663-11d110fc5eb2'
CREATE procedure [dbo].[billing_account_vault_update_status]
(
	@vault_id       uniqueidentifier
)
as
	begin
		
		begin transaction
				 
			update [billing_account_vault]
			   set date_updated=GETDATE()
			   where vault_id = @vault_id;

		commit transaction


		return 1
	end

GO
