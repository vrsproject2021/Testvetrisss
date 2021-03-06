USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_refund_qb_posting_details_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_refund_qb_posting_details_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_refund_qb_posting_details_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_refund_qb_posting_details_fetch : fetch quickbooks
				  posting details
** Created By   : Pavel Guha
** Created On   : 27/07/2020
*******************************************************/
--exec ar_refund_qb_posting_details_fetch '41C35E74-2375-41C4-B508-1AE09DD57100','CA360EB9-B8B0-4B83-ACD7-1CF10D39037D'
CREATE procedure [dbo].[ar_refund_qb_posting_details_fetch]
	@billing_account_id uniqueidentifier,
	@refund_id uniqueidentifier,
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
		refundref_no nvarchar(50) default '',
		refundref_date datetime default '01jan1900',
		narration_hdr nvarchar(500),
		gl_code nvarchar(5),
		amount_dr money default 0,
		amount_cr money default 0,
		narration nvarchar(250)
	 )

	 declare @billing_account_name nvarchar(100),
	         @refund_amount money,
			 @DEBTOR_gl_code nvarchar(5),
			 @RCVBL_gl_code nvarchar(5),
			 @refund_mode nvarchar(20),
			 @processing_ref_no nvarchar(100),
			 @refundref_no nvarchar(50),
			 @refundref_date datetime,
			 @narration_hdr nvarchar(500)

	 select @billing_account_name = qb_name from billing_account where id = @billing_account_id

	 select @refund_amount     = isnull(refund_amount,0),
			@refund_mode       = case when refund_mode  = 1 then 'Online' else 'Offline' end,
			@processing_ref_no = isnull(processing_ref_no,''),
			@refundref_no      = refundref_no,
			@refundref_date    = refundref_date
	 from ar_refunds 
	 where id = @refund_id 
	 and billing_account_id = @billing_account_id

	

	 select @DEBTOR_gl_code = gl_code from ar_non_revenue_acct_control where control_code='DEBTOR'
	 select @RCVBL_gl_code = gl_code from ar_non_revenue_acct_control where control_code='RCVBL'

	 --Debit details
    if(isnull(@refund_amount,0)>0 and isnull(@DEBTOR_gl_code,'')<>'') 
		begin
			insert into #tmp(billing_account_name,refundref_no,refundref_date,gl_code,amount_dr,narration)
			(select @billing_account_name,@refundref_no,@refundref_date,@DEBTOR_gl_code,@refund_amount, @refund_mode +' refund debited - refund ref # ' + @refundref_no + ' dated ' + convert(varchar(20),@refundref_date,107) + '; Transaction ref # ' + @processing_ref_no)
		end
	 
	 --Credit details
	 if(isnull(@refund_amount,0)>0 and isnull(@RCVBL_gl_code,'')<>'') 
		begin
			insert into #tmp(billing_account_name,refundref_no,refundref_date,gl_code,amount_cr,narration)
			(select @billing_account_name,@refundref_no,@refundref_date,@RCVBL_gl_code,@refund_amount, @refund_mode +' refund credited - refund ref # ' + @refundref_no + ' dated ' + convert(varchar(20),@refundref_date,107) + '; Transaction ref # ' + @processing_ref_no)
		end
	
	

	set @narration_hdr ='Posting of Redund Ref # ' + @refundref_no + ' dtd ' + convert(varchar,@refundref_date,107) + ' by billing account ' + @billing_account_name 
	update #tmp set narration_hdr = @narration_hdr

	 select billing_account_name,refundref_no,refundref_date,narration_hdr from #tmp where sl_no=1

	 select * from #tmp where gl_code<>''

	  if(isnull((select sum(amount_dr) from #tmp where gl_code<>''),0) <> isnull((select sum(amount_cr) from #tmp where gl_code<>''),0))
		begin
			select @error_msg ='The invoice voucher of ' + @billing_account_name + ', refund ref # ' + @refundref_no + ' is not balanced',
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
						set @day_end_date = convert(datetime,convert(varchar(11),@refundref_date,106))

						if(select count(ref_no) from day_end_vetris_account_posting where day_end_date=@day_end_date and ref_no=@refundref_no and ref_type='PMTREF' and gl_code=@gl_code)=0
							begin
								insert into day_end_vetris_account_posting(day_end_date,ref_no,ref_date,ref_type,gl_code,gl_desc,dr_amount,cr_amount,date_updated)
																	values(@day_end_date,@refundref_no,@refundref_date,'PMTREF',@gl_code,@gl_desc,@amt_dr,@amt_cr,getdate())
							end
						else
							begin
								update day_end_vetris_account_posting
								set gl_desc      = @gl_desc,
									dr_amount    = @amt_dr,
									cr_amount    = @amt_cr,
									date_updated = getdate()
								where day_end_date=@day_end_date 
								and ref_no=@refundref_no 
								and ref_type='PMTREF' 
								and gl_code=@gl_code
							end

						if(@@rowcount=0)
							begin
								rollback transaction
								select @error_msg='Failed to update the day end (refund) posting failure of billing account : '+ @billing_account_name + ', Refund Ref. ' + @refundref_no,
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
