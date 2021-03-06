USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_qb_posting_details_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_payments_qb_posting_details_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_qb_posting_details_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_payments_qb_posting_details_fetch : fetch quickbooks
				  posting details
** Created By   : Pavel Guha
** Created On   : 15/06/2020
*******************************************************/
--exec ar_payments_qb_posting_details_fetch '113DB806-7A9D-4328-BA73-BF8049929164','7FEEF947-C5DA-48DF-A1AA-A1EFA9823821'
CREATE procedure [dbo].[ar_payments_qb_posting_details_fetch]
	@billing_account_id uniqueidentifier,
	@payment_id uniqueidentifier,
	@error_msg nvarchar(300) ='' output,
	@return_type int = 0 output
as
begin
	 set nocount on

	 declare @day_end_date datetime,
			 @gl_desc nvarchar(100),
			 @srl_no int,
			 @gl_code nvarchar(5),
			 @amt_dr money,
			 @amt_cr money,
	         @rowcount int,
			 @counter int

	 create table #tmp
	 (
		sl_no int identity(1,1),
		billing_account_name nvarchar(100)  default '',
		payref_no nvarchar(50) default '',
		payref_date datetime default '01jan1900',
		narration_hdr nvarchar(500),
		gl_code nvarchar(5),
		amount_dr money default 0,
		amount_cr money default 0,
		narration nvarchar(250)
	 )

	 declare @billing_account_name nvarchar(100),
	         @payment_amount money,
			 @DEBTOR_gl_code nvarchar(5),
			 @RCVBL_gl_code nvarchar(5),
			 @payment_mode nvarchar(20),
			 @processing_ref_no nvarchar(100),
			 @payref_no nvarchar(50),
			 @payref_date datetime,
			 @narration_hdr nvarchar(500)

	 select @billing_account_name = qb_name from billing_account where id = @billing_account_id

	 select @payment_amount = isnull(payment_amount,0),
			@payment_mode = case when payment_mode  = 1 then 'Online' else 'Offline' end,
			@processing_ref_no = processing_ref_no,
			@payref_no = payref_no,
			@payref_date = payref_date
	 from ar_payments 
	 where id = @payment_id 
	 and billing_account_id = @billing_account_id

	 select @DEBTOR_gl_code = gl_code from ar_non_revenue_acct_control where control_code='DEBTOR'
	 select @RCVBL_gl_code = gl_code from ar_non_revenue_acct_control where control_code='RCVBL'
	 
	 --Debit details
	 if(isnull(@payment_amount,0)>0 and isnull(@RCVBL_gl_code,'')<>'') 
		begin
			insert into #tmp(billing_account_name,payref_no,payref_date,gl_code,amount_dr,narration)
			(select @billing_account_name,@payref_no,@payref_date,@RCVBL_gl_code,@payment_amount, @payment_mode +' payment debited - payment ref # ' + @payref_no + ' dated ' + convert(varchar(20),@payref_date,107) + '; Transaction ref # ' + @processing_ref_no)
		end
	
	--Credit details
    if(isnull(@payment_amount,0)>0 and isnull(@DEBTOR_gl_code,'')<>'') 
		begin
			insert into #tmp(billing_account_name,payref_no,payref_date,gl_code,amount_cr,narration)
			(select @billing_account_name,@payref_no,@payref_date,@DEBTOR_gl_code,@payment_amount, @payment_mode +' payment credited - payment ref # ' + @payref_no + ' dated ' + convert(varchar(20),@payref_date,107) + '; Transaction ref # ' + @processing_ref_no)
		end

	set @narration_hdr ='Posting of Paymet Ref # ' + @payref_no + ' dtd ' + convert(varchar,@payref_date,107) + ' by billing account ' + @billing_account_name 
	update #tmp set narration_hdr = @narration_hdr

	 select billing_account_name,payref_no,payref_date,narration_hdr from #tmp where sl_no=1

	 select * from #tmp where gl_code<>''

	  if(isnull((select sum(amount_dr) from #tmp where gl_code<>''),0) <> isnull((select sum(amount_cr) from #tmp where gl_code<>''),0))
		begin
			select @error_msg ='The invoice voucher of ' + @billing_account_name + ', payment ref # ' + @payref_no + ' is not balanced',
			       @return_type=0
		end
	  else
		begin
				begin transaction
				select @rowcount= max(sl_no),
			           @counter = 1
		        from #tmp

				while(@counter<=@rowcount)
					begin
						select @srl_no = sl_no,
							   @gl_code = gl_code,
							   @amt_dr  = amount_dr,
							   @amt_cr  = amount_cr
						from #tmp
						where sl_no = @counter

						select @gl_desc = gl_desc from sys_gl_codes where gl_code=@gl_code
						set @day_end_date = convert(datetime,convert(varchar(11),@payref_date,106))

						if(select count(ref_no) from day_end_vetris_account_posting where day_end_date=@day_end_date and ref_no=@payref_no and ref_type='PMTREC' and gl_code=@gl_code)=0
							begin
								insert into day_end_vetris_account_posting(day_end_date,ref_no,ref_date,ref_type,gl_code,gl_desc,dr_amount,cr_amount,date_updated)
																	values(@day_end_date,@payref_no,@payref_date,'PMTREC',@gl_code,@gl_desc,@amt_dr,@amt_cr,getdate())
							end
						else
							begin
								update day_end_vetris_account_posting
								set gl_desc      = @gl_desc,
									dr_amount    = @amt_dr,
									cr_amount    = @amt_cr,
									date_updated = getdate()
								where day_end_date=@day_end_date 
								and ref_no=@payref_no 
								and ref_type='PMTREC' 
								and gl_code=@gl_code
							end

						if(@@rowcount=0)
							begin
								rollback transaction
								select @error_msg='Failed to update the day end (payment) posting failure of billing account : '+ @billing_account_name + ', Payment Ref. ' + @payref_no,
										@return_type=0
								return 0
							end

						set @counter = @counter + 1

					end

					commit transaction
				
				
				select @error_msg ='',
			           @return_type=1
		end
	
	set nocount off
end

GO
