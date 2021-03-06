USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[day_end_accounts_posting_data_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[day_end_accounts_posting_data_save]
GO
/****** Object:  StoredProcedure [dbo].[day_end_accounts_posting_data_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : day_end_accounts_posting_data_save : 
				  save accounts posting data at day end
** Created By   : Pavel Guha
** Created On   : 24/12/2020
*******************************************************/
create procedure [dbo].[day_end_accounts_posting_data_save]
	@day_end_date datetime,
    @TVP_data as day_end_acct_posts readonly,
	@error_msg nvarchar(500)='' output,
	@return_type int=0 output
as
begin
	set nocount on

	declare @srl_no int,
	        @ref_no nvarchar(20),
			@date_created datetime,
			@date_modified datetime,
			@date_txn datetime,
			@txn_id nvarchar(30),
			@txn_type nvarchar(10),
			@txn_ref_no nvarchar(30),
			@dr_cr_name nvarchar(100),
			@dr_cr_id uniqueidentifier,
			@gl_code nvarchar(5),
			@gl_desc nvarchar(100),
			@dr_amt money,
			@cr_amt money,
			@rowcount int,
			@counter int
	
	

	if(select count(day_end_date) from day_end_accounts_posting_processed where day_end_date=@day_end_date)=0
		begin
			begin transaction
			insert into day_end_accounts_posting_processed(day_end_date,record_count,process_completed)
					                                values(@day_end_date,0,'P')
			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_msg = 'Failed to update the day end accounts posting process status for the date ' + convert(varchar(10),@day_end_date,101),
							@return_type=0
					return 0
				end
			commit transaction

			if(select count(srl_no) from @TVP_data)>0
				begin
					select @rowcount = count(srl_no),
						   @counter = 1
					from @TVP_data

					begin transaction

					while(@counter<=@rowcount)
						begin
							select  @ref_no         = ref_no,
									@date_created   = date_created,
									@date_modified  = date_modified,
									@date_txn       = date_txn,
									@txn_id         = txn_id,
									@dr_cr_name     = dr_cr_name,
									@gl_code        = gl_code,
									@dr_amt         = dr_amount,
									@cr_amt         = cr_amount
							from @TVP_data
							where srl_no = @counter

							set @txn_type=''
							set @txn_ref_no=''
							set @dr_cr_id='00000000-0000-0000-0000-000000000000'
							set @gl_desc=''

							if(select count(txn_id) from day_end_accounts_postings where txn_id=@txn_id and gl_code=@gl_code)=0
								begin 
									if(select count(id) from invoice_hdr where qb_posting_id = @txn_id)>0
										begin
											select @txn_type ='INV',
												   @txn_ref_no = invoice_no,
												   @dr_cr_id   = billing_account_id
											 from invoice_hdr
											 where qb_posting_id = @txn_id

											 select @dr_cr_name = name from billing_account where id=@dr_cr_id
										end
									else if(select count(id) from invoice_hdr where qb_rev_posting_id = @txn_id)>0
										begin
											select @txn_type ='INVREV',
												   @txn_ref_no = invoice_no,
												   @dr_cr_id   = billing_account_id
											 from invoice_hdr
											 where qb_rev_posting_id = @txn_id

											 select @dr_cr_name = name from billing_account where id=@dr_cr_id
										end
									else if(select count(id) from ar_payments where qb_posting_id = @txn_id)>0
										begin
											select @txn_type ='PMTREC',
												   @txn_ref_no = payref_no,
												   @dr_cr_id   = billing_account_id
											 from ar_payments
											 where qb_posting_id = @txn_id

											 select @dr_cr_name = name from billing_account where id=@dr_cr_id
										end
									else if(select count(id) from ar_refunds where qb_posting_id = @txn_id)>0
										begin
											select @txn_type ='PMTREF',
												   @txn_ref_no = refundref_no,
												   @dr_cr_id   = billing_account_id
											 from ar_refunds
											 where qb_posting_id = @txn_id

											 select @dr_cr_name = name from billing_account where id=@dr_cr_id
										end
									else if(select count(id) from ap_radiologist_payment_hdr where qb_posting_id = @txn_id)>0
										begin
											select @txn_type ='PMTRAD',
												   @txn_ref_no = payment_no,
												   @dr_cr_id   = radiologist_id
											 from ap_radiologist_payment_hdr
											 where qb_posting_id = @txn_id

											 select @dr_cr_name = name from radiologists where id=@dr_cr_id
										end
									else if(select count(id) from ap_radiologist_payment_hdr where qb_rev_posting_id = @txn_id)>0
										begin
											select @txn_type ='PMTRADREV',
												   @txn_ref_no = payment_no,
												   @dr_cr_id   = radiologist_id
											 from ap_radiologist_payment_hdr
											 where qb_rev_posting_id = @txn_id

											  select @dr_cr_name = name from radiologists where id=@dr_cr_id
										end
									else if(select count(id) from ap_transcriptionist_payment_hdr where qb_posting_id = @txn_id)>0
										begin
											select @txn_type ='PMTTRS',
												   @txn_ref_no = payment_no,
												   @dr_cr_id   = transcriptionist_id
											 from ap_transcriptionist_payment_hdr
											 where qb_posting_id = @txn_id

											 select @dr_cr_name = name from transciptionists where id=@dr_cr_id
										end
									else if(select count(id) from ap_transcriptionist_payment_hdr where qb_rev_posting_id = @txn_id)>0
										begin
											select @txn_type ='PMTTRSREV',
												   @txn_ref_no = payment_no,
												   @dr_cr_id   = transcriptionist_id
											 from ap_transcriptionist_payment_hdr
											 where qb_rev_posting_id = @txn_id

											  select @dr_cr_name = name from transciptionists where id=@dr_cr_id
										end

								    select @gl_desc = gl_desc from sys_gl_codes where gl_code = @gl_code

									insert into day_end_accounts_postings(day_end_date,ref_no,date_created,date_modified,date_txn,
									                                      txn_id,txn_type,txn_ref_no,dr_cr_name,dr_cr_id,
																		  gl_code,gl_desc,dr_amount,cr_amount,date_synced)
																   values(@day_end_date,isnull(@ref_no,''),@date_created,@date_modified,@date_txn,
									                                      @txn_id,@txn_type,@txn_ref_no,@dr_cr_name,@dr_cr_id,
																		  @gl_code,isnull(@gl_desc,''),@dr_amt,@cr_amt,getdate())

									if(@@rowcount=0)
										begin
											rollback transaction
											select @error_msg = 'Failed to sync the day end accounts posting for the date ' + convert(varchar(10),@day_end_date,101),
												   @return_type=0
											return 0
										end
							    end
						   

							set @counter = @counter + 1
						end


					
					update day_end_accounts_posting_processed
					set process_completed = 'Y',
					    record_count      = @rowcount
					where day_end_date= @day_end_date

					if(@@rowcount=0)
						begin
							rollback transaction
							select @error_msg = 'Failed to update the day end accounts posting process status for the date ' + convert(varchar(10),@day_end_date,101),
									@return_type=0
							return 0
						end

					commit transaction
				end
			else
				begin
					begin transaction
					update day_end_accounts_posting_processed
					set process_completed = 'Y'
					where day_end_date= @day_end_date

					if(@@rowcount=0)
						begin
							rollback transaction
							select @error_msg = 'Failed to update the day end accounts posting process status for the date ' + convert(varchar(10),@day_end_date,101),
									@return_type=0
							return 0
						end
					commit transaction
				end
		end
	else
		begin
			select @error_msg = 'Day end accounts data already saved for the date ' + convert(varchar(10),@day_end_date,101),
			       @return_type=0
			return 0
		end
	

	set nocount off
	select @error_msg='',@return_type=1
	return 1

end

GO
