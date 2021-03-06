USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[billing_account_vault_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[billing_account_vault_fetch]
GO
/****** Object:  StoredProcedure [dbo].[billing_account_vault_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*******************************************************
** Version		: 1.0.0.0
** Procedure    : billing_account_vault_fetch : fetch vault info
** Created By   : KC
** Created On   : 08/04/2020
*******************************************************/
--exec billing_account_vault_fetch '570D5DFA-4173-4121-99A5-F4D17EF438B7'
CREATE procedure [dbo].[billing_account_vault_fetch]
    @billing_account_id uniqueidentifier
as
begin
	set nocount on
	
	declare @vaultId uniqueidentifier;

	
	IF OBJECT_ID('tempdb.dbo.#T1', 'U') IS NOT NULL
			DROP TABLE #T1;
	select 
		a.billing_account_id,
		a.vault_id,
		a.vault_type,
		a.vault_card_type,
		a.vault_card,
		a.vault_exp,
		a.vault_account,
		a.holder_name,
		datediff(mi, ISNULL(a.date_updated,a.date_created), getdate()) last_used 
		into #t1
	from billing_account_vault a
	inner join billing_account b on b.id=a.billing_account_id 
	where b.id = @billing_account_id

	

	select @vaultId=a.vault_id from (
		select top 1 vault_id, min(last_used) m from #t1
		group by vault_id
		order by min(last_used)
	) a

	update #t1 set last_used=case when vault_id=@vaultId then 1 else 0 end;
	select * from #t1

	set nocount off
end

GO
