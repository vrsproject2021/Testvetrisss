USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_qb_reverse_posting_details_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_qb_reverse_posting_details_fetch]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_qb_reverse_posting_details_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_qb_reverse_posting_details_fetch : fetch quickbooks
				  posting details
** Created By   : Pavel Guha
** Created On   : 15/06/2020
*******************************************************/
--exec invoicing_qb_reverse_posting_details_fetch '3225FD47-3FF5-4863-8E89-22E960263EB3','9552A13B-CD82-48DB-9AEB-B10B3537DBA3'
--exec invoicing_qb_reverse_posting_details_fetch 'D6FC1504-F380-4966-B135-5AEBE40FB683','AE349D10-55DC-46CE-91FF-024419D9D12F'
CREATE procedure [dbo].[invoicing_qb_reverse_posting_details_fetch]
    @billing_cycle_id uniqueidentifier,
	@billing_account_id uniqueidentifier,
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
		billing_account_name nvarchar(100) default '',
		billing_cycle_name nvarchar(50) default '',
		invoice_srl_no int default 0,
		invoice_no nvarchar(30) default '',
		invoice_date datetime default '01jan1900',
		narration_hdr nvarchar(500) default '',
		gl_code nvarchar(5),
		amount_dr money default 0,
		amount_cr money default 0,
		narration nvarchar(250)
	 )

	 declare @billing_account_name nvarchar(100),
			 @billing_cycle_name nvarchar(30),
	         @account_total money,
			 @discount_total money,
			 @promo_total money,
			 @revenue_total money,
			 @DEBTOR_gl_code nvarchar(5),
			 @DISC_gl_code nvarchar(5),
			 @PROMO_gl_code nvarchar(5),
			 @invoice_srl_no int,
			 @invoice_no nvarchar(30),
			 @invoice_date datetime,
			 @narration_hdr nvarchar(500)

	 select @billing_account_name = name from billing_account where id = @billing_account_id
	 select @billing_cycle_name = name from billing_cycle where id=@billing_cycle_name

	 select @discount_total = isnull(sum(disc_amount),0) 
	 from invoice_institution_dtls 
	 where billing_cycle_id = @billing_cycle_id 
	 and billing_account_id = @billing_account_id
	 and promotion_id ='00000000-0000-0000-0000-000000000000'

	 select @promo_total = isnull(sum(disc_amount),0) 
	 from invoice_institution_dtls iid 
	 --inner join ar_promotions arp on arp.id = iid.promotion_id and arp.billing_account_id = iid.billing_account_id
	 where iid.billing_cycle_id = @billing_cycle_id 
	 and iid.billing_account_id = @billing_account_id
	 --and arp.promotion_type='F'
	 and promotion_id <>'00000000-0000-0000-0000-000000000000'
	 and is_free='N'

	 select @promo_total = isnull(@promo_total,0) +  isnull(sum(study_price + service_price),0) 
	 from invoice_institution_dtls iid 
	 --inner join ar_promotions arp on arp.id = iid.promotion_id and arp.billing_account_id = iid.billing_account_id
	 where iid.billing_cycle_id = @billing_cycle_id 
	 and iid.billing_account_id = @billing_account_id
	 --and arp.promotion_type='F'
	 and is_free='Y'

	 select @revenue_total =  isnull(sum(total_amount),0) 
	 from invoice_institution_dtls iid 
	 where iid.billing_cycle_id = @billing_cycle_id 
	 and iid.billing_account_id = @billing_account_id

	 select @account_total = isnull(@revenue_total,0) - (isnull(@discount_total,0) +  isnull(@promo_total,0)),
			@invoice_srl_no = invoice_srl_no,
			@invoice_no = invoice_no,
			@invoice_date = invoice_date,
			@day_end_date = date_disapproved
	 from invoice_hdr 
	 where billing_cycle_id = @billing_cycle_id 
	 and billing_account_id = @billing_account_id
	 

	 select @DEBTOR_gl_code = gl_code from ar_non_revenue_acct_control where control_code='DEBTOR'
	 select @DISC_gl_code = gl_code from ar_non_revenue_acct_control where control_code='DISC'
	 select @PROMO_gl_code = gl_code from ar_non_revenue_acct_control where control_code='PROMO'

	 --Debit details
	 if((@discount_total + @promo_total) > 0 and ((@revenue_total>@account_total) and @account_total>=0))
		begin
			--print '1'
			insert into #tmp(gl_code,amount_dr,narration)
			(select isnull(mgl.gl_code,''),sum(isnull(iid.study_price,0)),isnull(m.name,'') + ' of ' + isnull(sc.name,'') + ' total of '+ isnull(@invoice_no,'') +' dated ' + convert(varchar(20),@invoice_date,107) + ' debited'
			 from invoice_institution_dtls iid
			 inner join study_hdr sh on sh.id = iid.study_id
			 inner join modality_gl_code_link mgl on mgl.modality_id = sh.modality_id and mgl.category_id = sh.category_id
			 inner join modality m on m.id = mgl.modality_id
			 inner join sys_study_category sc on sc.id = mgl.category_id
			 where iid.billing_cycle_id = @billing_cycle_id 
			 and iid.billing_account_id = @billing_account_id
			 group by mgl.modality_id,mgl.category_id,mgl.gl_code,m.name,sc.name)

			 insert into #tmp(gl_code,amount_dr,narration)
			(select isnull(mgl.gl_code,''),sum(isnull(iid.study_price,0)),isnull(m.name,'') + ' of ' + isnull(sc.name,'') + ' total of '+ isnull(@invoice_no,'') +' dated ' + convert(varchar(20),@invoice_date,107) + ' debited'
			 from invoice_institution_dtls iid
			 inner join study_hdr_archive sh on sh.id = iid.study_id
			 inner join modality_gl_code_link mgl on mgl.modality_id = sh.modality_id and mgl.category_id = sh.category_id
			 inner join modality m on m.id = mgl.modality_id
			 inner join sys_study_category sc on sc.id = mgl.category_id
			 where iid.billing_cycle_id = @billing_cycle_id 
			 and iid.billing_account_id = @billing_account_id
			 group by mgl.modality_id,mgl.category_id,mgl.gl_code,m.name,sc.name)
		end
	else
		begin
			--print '2'
			insert into #tmp(gl_code,amount_dr,narration)
			(select isnull(mgl.gl_code,''),sum(isnull(iid.study_price,0)),isnull(m.name,'') + ' of ' + isnull(sc.name,'') + ' total of '+ isnull(@invoice_no,'') +' dated ' + convert(varchar(20),@invoice_date,107) + ' credited'
			 from invoice_institution_dtls iid
			 inner join study_hdr sh on sh.id = iid.study_id
			 inner join modality_gl_code_link mgl on mgl.modality_id = sh.modality_id and mgl.category_id = sh.category_id
			 inner join modality m on m.id = mgl.modality_id
			 inner join sys_study_category sc on sc.id = mgl.category_id
			 where iid.billing_cycle_id = @billing_cycle_id 
			 and iid.billing_account_id = @billing_account_id
			 group by mgl.modality_id,mgl.category_id,mgl.gl_code,m.name,sc.name)

			 insert into #tmp(gl_code,amount_dr,narration)
			(select isnull(mgl.gl_code,''),sum(isnull(iid.study_price,0)),isnull(m.name,'') + ' of ' + isnull(sc.name,'') + ' total of '+ isnull(@invoice_no,'') +' dated ' + convert(varchar(20),@invoice_date,107) + ' credited'
			 from invoice_institution_dtls iid
			 inner join study_hdr_archive sh on sh.id = iid.study_id
			 inner join modality_gl_code_link mgl on mgl.modality_id = sh.modality_id and mgl.category_id = sh.category_id
			 inner join modality m on m.id = mgl.modality_id
			 inner join sys_study_category sc on sc.id = mgl.category_id
			 where iid.billing_cycle_id = @billing_cycle_id 
			 and iid.billing_account_id = @billing_account_id
			 group by mgl.modality_id,mgl.category_id,mgl.gl_code,m.name,sc.name)
		end

	 --service details
	 insert into #tmp(gl_code,amount_dr,narration)
	 (select isnull(sgl.gl_code_default,''),sum(isnull(isd.service_price,0)),
	 case 
		when isnull(m.name,'') <> '' then  isnull(s.name,'') +  ' for ' + isnull(m.name,'') + ' total of '+ isnull(@invoice_no,'') +' dated ' + convert(varchar(20),@invoice_date,107) + ' credited'
		                             else  isnull(s.name,'') +  ' total of '+ isnull(@invoice_no,'') +' dated ' + convert(varchar(20),@invoice_date,107) + ' credited'
	 end narration
	 from invoice_service_dtls isd
	 inner join service_gl_code_link sgl on sgl.service_id = isd.service_id and sgl.modality_id = isd.modality_id
	 inner join services s on s.id = sgl.service_id 
	 left outer join modality m on m.id = sgl.modality_id
	 where isd.billing_cycle_id = @billing_cycle_id 
	 and isd.billing_account_id = @billing_account_id
	 and isd.is_after_hrs='N'
	 group by isd.service_id,isd.modality_id,sgl.gl_code_default,s.name,m.name)

	 insert into #tmp(gl_code,amount_dr,narration)
	 (select isnull(sgl.gl_code_after_hrs,''),sum(isnull(isd.service_price_after_hrs,0)),
	 case 
		when isnull(m.name,'') <> '' then  isnull(s.name,'') +  ' for ' + isnull(m.name,'') + ' total (After Hrs.) of '+ isnull(@invoice_no,'') +' dated ' + convert(varchar(20),@invoice_date,107) + ' credited'
		                             else  isnull(s.name,'') +  ' total of (After Hrs.)'+ isnull(@invoice_no,'') +' dated ' + convert(varchar(20),@invoice_date,107) + ' credited'
	 end narration
	 from invoice_service_dtls isd
	 inner join service_gl_code_link sgl on sgl.service_id = isd.service_id and sgl.modality_id = isd.modality_id
	 inner join services s on s.id = sgl.service_id 
	 left outer join modality m on m.id = sgl.modality_id
	 where isd.billing_cycle_id = @billing_cycle_id 
	 and isd.billing_account_id = @billing_account_id
	 and isd.is_after_hrs='Y'
	 group by isd.service_id,isd.modality_id,sgl.gl_code_after_hrs,s.name,m.name)
	 --insert into #tmp(gl_code,amount_dr,narration)
	 --(select isnull(s.gl_code,''),sum(isnull(isd.service_price,0)),isnull(s.name,'') + ' total of '+ @invoice_no +' dated ' + convert(varchar(20),@invoice_date,107) + ' debited'
	 --from invoice_service_dtls isd
	 --inner join services s on s.id = isd.service_id
	 --where isd.billing_cycle_id = @billing_cycle_id 
	 --and isd.billing_account_id = @billing_account_id
	 --and s.code not in ('CONSULT','STORAGE')
	 --group by isd.service_id,s.gl_code,s.name)

	-- insert into #tmp(gl_code,amount_dr,narration)
	-- (select isnull(s.gl_code,''),sum(isnull(isd.service_price,0)),isnull(s.name,'') + ' total of '+ @invoice_no +' dated ' + convert(varchar(20),@invoice_date,107) + ' debited'
	--  from invoice_service_dtls isd
	--  inner join services s on s.id = isd.service_id
	--  where isd.billing_cycle_id = @billing_cycle_id 
	--  and isd.billing_account_id = @billing_account_id
	--  and s.code = 'CONSULT'
	--  group by isd.service_id,s.gl_code,s.name)

	--insert into #tmp(gl_code,amount_dr,narration)
	--(select isnull(s.gl_code,''),sum(isnull(isd.service_price,0)),isnull(s.name,'') + ' total debited'
	-- from invoice_service_dtls isd
	-- inner join services s on s.id = isd.service_id
	-- where isd.billing_cycle_id = @billing_cycle_id 
	-- and isd.billing_account_id = @billing_account_id
	-- and s.code = 'STORAGE'
	-- group by isd.service_id,s.gl_code,s.name)


	--credit details
	if(isnull(@revenue_total,0)>0 and isnull(@revenue_total,0)<(isnull(@discount_total,0) +  isnull(@promo_total,0)) and isnull(@DEBTOR_gl_code,'')<>'') 
		begin
			insert into #tmp(billing_account_name,billing_cycle_name,invoice_srl_no,invoice_no,invoice_date,gl_code,amount_cr,narration)
			(select @billing_account_name,@billing_cycle_name,@invoice_srl_no,@invoice_no,@invoice_date,@DEBTOR_gl_code,@revenue_total, 'Invoice total of '+ @invoice_no +' dated ' + convert(varchar(20),@invoice_date,107) + ' credited')
		end
	else if(isnull(@revenue_total,0)>0 and isnull(@DEBTOR_gl_code,'')<>'') 
		begin
			insert into #tmp(billing_account_name,billing_cycle_name,invoice_srl_no,invoice_no,invoice_date,gl_code,amount_cr,narration)
			(select @billing_account_name,@billing_cycle_name,@invoice_srl_no,@invoice_no,@invoice_date,@DEBTOR_gl_code,@revenue_total, 'Invoice total of '+ isnull(@invoice_no,'') +' dated ' + convert(varchar(20),@invoice_date,107) + ' credited')
		end
	
	if(isnull(@discount_total,0)>0 and isnull(@DISC_gl_code,'')<>'') 
		begin
			insert into #tmp(billing_account_name,billing_cycle_name,invoice_srl_no,invoice_no,invoice_date,gl_code,amount_cr,narration)
			(select @billing_account_name,@billing_cycle_name,@invoice_srl_no,@invoice_no,@invoice_date,@DISC_gl_code,@discount_total, 'Discount total of '+ isnull(@invoice_no,'') +' dated ' + convert(varchar(20),@invoice_date,107) + ' credited')
		end

    if(isnull(@promo_total,0)>0 and isnull(@PROMO_gl_code,'')<>'') 
		begin
			insert into #tmp(billing_account_name,billing_cycle_name,invoice_srl_no,invoice_no,invoice_date,gl_code,amount_cr,narration)
			(select @billing_account_name,@billing_cycle_name,@invoice_srl_no,@invoice_no,@invoice_date,@PROMO_gl_code,@promo_total, 'Free Credit total of '+ isnull(@invoice_no,'') +' dated ' + convert(varchar(20),@invoice_date,107) + ' credited')
		end
	
	 set @narration_hdr ='Reverse posting of Invoice # ' + @invoice_no + ' dtd ' + convert(varchar,@invoice_date,107) + ' billed to billing account ' + @billing_account_name + ' for the billing cycle ' + @billing_cycle_name
   	 update #tmp 
	 set billing_account_name=@billing_account_name,
	     billing_cycle_name=@billing_cycle_name,
		 invoice_srl_no = @invoice_srl_no,
		 invoice_no = @invoice_no,
		 invoice_date = @invoice_date,
		 narration_hdr = @narration_hdr

	 select billing_account_name,billing_cycle_name,invoice_srl_no,invoice_no,invoice_date,narration_hdr from #tmp where sl_no=1

	 select * from #tmp where gl_code<>''

	 if(isnull((select sum(amount_dr) from #tmp where gl_code<>''),0) <> isnull((select sum(amount_cr) from #tmp where gl_code<>''),0))
		begin
			select @error_msg ='The invoice reversal voucher of ' + @billing_account_name + ', billing cycle ' + @billing_cycle_name + ' is not balanced',
			       @return_type=0

			update invoice_hdr set update_qb='F' where billing_cycle_id=@billing_cycle_id and billing_account_id=@billing_account_id
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
						set @day_end_date = convert(datetime,convert(varchar(11),@day_end_date,106))

						if(select count(ref_no) from day_end_vetris_account_posting where day_end_date=@day_end_date and ref_no=@invoice_no and ref_type='INVREV' and gl_code=@gl_code)=0
							begin
								insert into day_end_vetris_account_posting(day_end_date,ref_no,ref_date,ref_type,gl_code,gl_desc,dr_amount,cr_amount,date_updated)
																	values(@day_end_date,@invoice_no,@invoice_date,'INVREV',@gl_code,@gl_desc,@amt_dr,@amt_cr,getdate())
							end
						else
							begin
								update day_end_vetris_account_posting
								set gl_desc      = @gl_desc,
									dr_amount    = @amt_dr,
									cr_amount    = @amt_cr,
									date_updated = getdate()
								where day_end_date=@day_end_date 
								and ref_no=@invoice_no 
								and ref_type='INVREV' 
								and gl_code=@gl_code
							end

						if(@@rowcount=0)
							begin
								rollback transaction
								select @error_msg='Failed to update the day end posting (invoice reversal) of billing account : '+ @billing_account_name + ', billing cycle ' + @billing_cycle_name + ', Invoice # ' + @invoice_no,
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
