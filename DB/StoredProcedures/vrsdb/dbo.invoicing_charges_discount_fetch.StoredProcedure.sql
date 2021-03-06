USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_charges_discount_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_charges_discount_fetch]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_charges_discount_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_charges_discount_fetch : fetch
                  charges discounts. 
** Created By   : BK
** Created On   : 14/11/2019
*******************************************************/
create procedure [dbo].[invoicing_charges_discount_fetch]
(
	@menu_id int,
    @user_id uniqueidentifier
)
as
	begin
		declare @counter bigint,
				@rowcount bigint,
				@billing_acc_id uniqueidentifier

		create table #tmpDiscPerc
		(
			rec_id int identity(1,1),
			billing_account_id uniqueidentifier,
			billing_acc_name nvarchar(250)
		)

		delete from sys_record_lock where user_id = @user_id
	    delete from sys_record_lock_ui where user_id = @user_id

		insert into #tmpDiscPerc(billing_account_id,billing_acc_name)
		(select
				
				biiling_account_id =id,
				billing_account_name= name
			from billing_account ba
			where is_active='Y'
		)

		select  @rowcount=(select count(billing_account_id) from #tmpDiscPerc)

		if(@rowcount > 0)
			begin
				set @counter = 1
				while(@counter <= @rowcount)
					begin
						select @billing_acc_id=billing_account_id from #tmpDiscPerc where rec_id=@counter
						if((select count(billing_account_id) from invoicing_charges_discount where  billing_account_id=@billing_acc_id) = 0)
							begin
								insert into invoicing_charges_discount(billing_account_id,discount_perc,updated_by,date_updated)
												 values(@billing_acc_id,0.00,@user_id,GETDATE())
							end

						set @counter = @counter + 1
					end
			end

		select icd.billing_account_id as billing_account_id,ba.code as code,ba.name as name,icd.discount_perc as discount_perc

		from invoicing_charges_discount icd
			inner join billing_account ba
				on ba.id=icd.billing_account_id

		drop table #tmpDiscPerc
	end
GO
