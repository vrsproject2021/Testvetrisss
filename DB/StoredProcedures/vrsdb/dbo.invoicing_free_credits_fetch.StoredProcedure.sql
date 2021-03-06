USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_free_credits_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_free_credits_fetch]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_free_credits_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_free_credits_fetch : fetch
                  free credits. 
** Created By   : BK
** Created On   : 13/11/2019
*******************************************************/
create procedure [dbo].[invoicing_free_credits_fetch]
(
	
	@menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
)
as

begin
	set nocount on
	create table #tmpCrdtHdr
	(
		rec_id int identity(1,1),
		billing_account_id uniqueidentifier,
		billing_acc_name nvarchar(250),
		total_free_credit int default 0,
		bal_free_credit int default 8
			
	)
	create table #tmpInst
	(
		rec_id int identity(1,1),
		billing_account_id uniqueidentifier,
		institution_id uniqueidentifier,
		institution_code nvarchar(5),
		institution_name nvarchar(100)
	)

	delete from sys_record_lock where user_id = @user_id
	delete from sys_record_lock_ui where user_id = @user_id

	insert into #tmpCrdtHdr(billing_account_id,billing_acc_name,total_free_credit,bal_free_credit)
	(select
				
			biiling_account_id =id,
			billing_account_name= name,
			total_free_credit=0,
			bal_free_credit=8
				
		from billing_account ba
		where is_active='Y'
	)
	insert into #tmpInst(billing_account_id,institution_id,institution_name,institution_code)
	(select ins.billing_account_id, 
			ins.id,dbo.InitCap(ins.name),ins.code	
		from institutions ins
		where ins.is_active='Y'
		and ins.billing_account_id in (select billing_account_id from #tmpCrdtHdr)
	)


	select * from #tmpCrdtHdr
	select * from #tmpInst
	drop table #tmpCrdtHdr
	drop table #tmpInst

	if(select count(record_id) from sys_record_lock where record_id=@menu_id and menu_id=@menu_id)=0
		begin
			exec common_lock_record
				@menu_id       = @menu_id,
				@record_id     = @menu_id,
				@user_id       = @user_id,
				@error_code    = @error_code output,
				@return_status = @return_status output	
						
			if(@return_status=0)
				begin
					return 0
				end
		end
	select @error_code='',@return_status=1

	set nocount off
end
GO
