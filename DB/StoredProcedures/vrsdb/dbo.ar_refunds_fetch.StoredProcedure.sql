USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_refunds_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_refunds_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_refunds_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_refunds_fetch : fetch ar_refunds
** Created By   : KC
** Created On   : 23/07/2020
*******************************************************/
create procedure [dbo].[ar_refunds_fetch]
    @id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@billing_account_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	 set nocount on

	 declare @REFUNDDAYS int,
	         @user_role_id int,
	         @user_role_code nvarchar(10),
			 @fetch_for_new bit = 0

	select @REFUNDDAYS = data_value_int from invoicing_control_params where control_code='REFUNDDAYS'
	select @user_role_id = u.user_role_id,
	       @user_role_code = ur.code
	from users u
	inner join user_roles ur on ur.id =u.user_role_id
	where u.id = @user_id

	if(ISNULL(@id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000'
	   and ISNULL(@billing_account_id,'00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000') 
		set @fetch_for_new=1;

	if(ISNULL(@id,'00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000') 
		begin
			select @billing_account_id=billing_account_id from ar_refunds where id=@id;
			select
				ar.id,
				ar.billing_account_id, 
				ar.ar_payments_id, 
				ar.refund_mode, 
				ar.refundref_no, 
				ar.refundref_date, 
				ar.processing_ref_no, 
				ar.processing_ref_date, 
				ar.processing_pg_name, 
				ar.processing_status, 
				ar.refund_amount,
				ar.remarks,
				ar.created_by,
				ar.date_created,
				ar.updated_by,
				ar.date_updated
			from ar_refunds ar
			inner join ar_payments ap on ap.id=ar.ar_payments_id
			inner join billing_account ba on ar.billing_account_id=ba.id
			inner join users u on ar.created_by=u.id
			where ar.id=@id;
		end
	else
		begin
			select
				ar.id,
				ar.billing_account_id, 
				ar.ar_payments_id,
				ar.refund_mode, 
				ar.refundref_no, 
				ar.refundref_date, 
				ar.processing_ref_no, 
				ar.processing_ref_date, 
				ar.processing_pg_name, 
				ar.processing_status, 
				ar.refund_amount,
				ar.remarks,
				ar.created_by,
				ar.date_created,
				ar.updated_by,
				ar.date_updated
			from ar_refunds ar
			inner join ar_payments ap on ap.id=ar.ar_payments_id
			inner join billing_account ba on ar.billing_account_id=ba.id
			inner join users u on ar.created_by=u.id
			where 1=0
		end
	if(@id<>'00000000-0000-0000-0000-000000000000')
		begin
			
				if(select count(record_id) from sys_record_lock_ui where record_id=@id and menu_id=@menu_id)=0
					begin
						exec common_lock_record_ui
							@menu_id       = @menu_id,
							@record_id     = @id,
							@user_id       = @user_id,
							@error_code    = @error_code output,
							@return_status = @return_status output	
						
						if(@return_status=0)
							begin
								return 0
							end
					end
				
		end
    else
		begin
			if(select count(record_id) from sys_record_lock_ui where user_id=@user_id)>0
			    begin
				  delete from sys_record_lock_ui where user_id=@user_id
				  delete from sys_record_lock where user_id=@user_id
			    end
		end


	-- billing account
	select id,name from billing_account with(nolock)
		where is_active='Y' 
			and id = case ISNULL(@billing_account_id,'00000000-0000-0000-0000-000000000000') 
					 when '00000000-0000-0000-0000-000000000000' then id
					 else @billing_account_id 
					 end
		order by name
	

	--- refundable payments with adjusted one 
	select a.id, a.payref_no, a.payref_date, a.payment_amount, a.processing_ref_no,
				a.payment_amount,
				a.payment_amount-sum(a.adjusted) refundable,
				sum(a.curr_adjusted) current_refund,
				a.payment_amount-sum(a.adjusted+a.curr_adjusted) current_balance,
				case when sum(a.curr_adjusted)>0 then cast(1 as bit) else cast(0 as bit) end selected from (
					select hdr.id,hdr.payref_no,hdr.payref_date,hdr.payment_amount,hdr.processing_ref_no, isnull(aj.refund_amount,0) adjusted, 0.00 curr_adjusted 
					from ar_payments hdr with(nolock) 
					left join ar_refunds aj with(nolock) on hdr.id=aj.ar_payments_id
					left join billing_account ba with(nolock) on ba.id=hdr.billing_account_id
					where hdr.billing_account_id = @billing_account_id
							and ISNULL(aj.id,'00000000-0000-0000-0000-000000000000')<>@id
							and hdr.payref_no is not null
							and hdr.payment_amount>0
							and hdr.payment_mode='1' and hdr.processing_status='1'
							AND hdr.payref_date > DATEADD(day,-1 * @REFUNDDAYS, getdate()) 
					union all
					select hdr.id,hdr.payref_no,hdr.payref_date,hdr.payment_amount,hdr.processing_ref_no, 0.00 adjusted, isnull(aj.refund_amount,0) curr_adjusted 
					from ar_payments hdr with(nolock) 
					left join ar_refunds aj with(nolock) on hdr.id=aj.ar_payments_id
					left join billing_account ba with(nolock) on ba.id=hdr.billing_account_id
					where hdr.billing_account_id = @billing_account_id
							and ISNULL(aj.id,'00000000-0000-0000-0000-000000000000')=@id
							and hdr.payref_no is not null
							and hdr.payment_mode='1' and hdr.processing_status='1'
							and hdr.payment_amount>0
				) a
		group by a.id, a.payref_no, a.payref_date, a.payment_amount, a.processing_ref_no, a.payment_amount
		having case @fetch_for_new when 1 then a.payment_amount-sum(a.adjusted) else sum(a.curr_adjusted) end >0
		order by payref_date asc;
	

	set nocount off
end

GO
