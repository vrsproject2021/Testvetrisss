USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_qb_posting_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_payments_qb_posting_update]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_qb_posting_update]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_payments_qb_posting_update : 
				  update payment posting
** Created By   : Pavel Guha
** Created On   : 25/06/2020
*******************************************************/
CREATE procedure [dbo].[ar_payments_qb_posting_update]
	@payment_id uniqueidentifier= '00000000-0000-0000-0000-000000000000',
	@billing_account_id uniqueidentifier= '00000000-0000-0000-0000-000000000000',
	@posting_id nvarchar(20)= '',
	@is_success nchar(1)='Y',
	--@TVP_data as day_end_acct_posts_vrs readonly,
	@error_msg nvarchar(500)='' output,
	@return_type int=0 output
as
begin
	set nocount on

	declare @billing_account_name nvarchar(100),
	        @payment_ref_no nvarchar(50),
			@ref_no nvarchar(30),
			@ref_date datetime
			--@ref_type nvarchar(10),
			--@day_end_date datetime,
			--@gl_desc nvarchar(100),
			--@srl_no int,
			--@gl_code nvarchar(5),
			--@amt_dr money,
			--@amt_cr money,
			--@rowcount int,
			--@counter int
	
	    begin transaction

		select @billing_account_name = name from billing_account where id=@billing_account_id
		select @payment_ref_no = payref_no + 'dtd ' + convert(varchar,payref_date,107),
		       @ref_no = payref_no,
		       @ref_date = payref_date
		from ar_payments 
		where id=@payment_id

		if(@is_success='Y')
			begin
				update ar_payments
				set qb_posting_id   = @posting_id,
					post_to_qb       = 'N', 
					qb_posted_on    = getdate()
				where billing_account_id=@billing_account_id
				and id = @payment_id

				if(@@rowcount=0)
					begin
						rollback transaction
						select @error_msg='Failed to update the payment posting of billing account : '+ @billing_account_name + ', Payment Ref. ' + @payment_ref_no,
								@return_type=0
						return 0
					end
			end
		else if(@is_success ='N')
			begin
				update ar_payments
				set post_to_qb       = 'F', 
					qb_posted_on    = getdate()
				where billing_account_id=@billing_account_id
				and id = @payment_id

				if(@@rowcount=0)
					begin
						rollback transaction
						select @error_msg='Failed to update the payment posting failure of billing account : '+ @billing_account_name + ', Payment Ref. ' + @payment_ref_no,
								@return_type=0
						return 0
					end
			end
		
		--select @day_end_date = convert(datetime,convert(varchar(11),date_created,106)),
		--	   @ref_type     = 'PMTREC'
  --      from ar_payments 
	 --   where billing_account_id = @billing_account_id 

		--select @rowcount= max(srl_no),
		--	   @counter = 1
		--from @TVP_Data

		--while(@counter<=@rowcount)
		--	begin
		--		select @srl_no = srl_no,
		--				@gl_code = gl_code,
		--				@amt_dr  = dr_amount,
		--				@amt_cr  = cr_amount
		--		from @TVP_Data
		--		where srl_no = @counter

		--		select @gl_desc = gl_desc from sys_gl_codes where gl_code=@gl_code

		--		if(select count(ref_no) from day_end_vetris_account_posting where day_end_date=@day_end_date and ref_no=@ref_no and ref_type=@ref_type and gl_code=@gl_code)=0
		--			begin
		--				insert into day_end_vetris_account_posting(day_end_date,ref_no,ref_date,ref_type,gl_code,gl_desc,dr_amount,cr_amount,date_updated)
		--						                            values(@day_end_date,@ref_no,@ref_date,@ref_type,@gl_code,@gl_desc,@amt_dr,@amt_cr,getdate())
		--			end
		--		else
		--			begin
		--				update day_end_vetris_account_posting
		--				set gl_desc      = @gl_desc,
		--					dr_amount    = @amt_dr,
		--					cr_amount    = @amt_cr,
		--					date_updated = getdate()
		--				where day_end_date=@day_end_date 
		--				and ref_no=@ref_no 
		--				and ref_type=@ref_type 
		--				and gl_code=@gl_code
		--			end

		--		if(@@rowcount=0)
		--			begin
		--				rollback transaction
		--				select @error_msg='Failed to update the day end (payment) posting failure of billing account : '+ @billing_account_name + ', Payment Ref. ' + @payment_ref_no,
		--						@return_type=0
		--				return 0
		--			end

		--		set @counter = @counter + 1
		--	end

		commit transaction

	set nocount off
	select @error_msg='',@return_type=1
	return 1

end

GO
