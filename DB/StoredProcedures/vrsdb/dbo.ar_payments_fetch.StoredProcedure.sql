USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_payments_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_payments_fetch : fetch ar_payments
** Created By   : KC
** Created On   : 08/04/2020
*******************************************************/
CREATE procedure [dbo].[ar_payments_fetch]
    @id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@billing_account_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	 set nocount on

	 declare 
	         @user_role_id int,
	         @user_role_code nvarchar(10),
			 @fetch_for_new bit = 0;

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
			select @billing_account_id=billing_account_id from ar_payments where id=@id;
			select 
				ap.id, 
				ap.billing_account_id, 
				ap.payment_mode, 
				case when ap.payment_mode='0' then 'Offline' else 'Online' end payment_mode_name, 
				ap.payref_no, 
				ap.payref_date, 
				ap.processing_ref_no, 
				ap.processing_ref_date, 
				ap.processing_pg_name, 
				ap.processing_status, 
				case when ap.payment_mode='0' then 'Pass' else (case when ap.processing_status='1' then 'Pass' else 'Failed' end) end processing_status_name, 
				ap.payment_amount,
				ap.remarks,
				ap.auth_code,
				ap.cvv_response,
				ap.avs_response,
				ap.created_by, 
				ap.created_by user_id, 
				u.name user_name, 
				ap.date_created, 
				ap.updated_by, 
				ap.date_updated,
				ba.name billing_account_name, 
				ba.code billing_account_code
			from ar_payments ap
			inner join billing_account ba on ap.billing_account_id=ba.id
			inner join users u on ap.created_by=u.id
			where ap.id=@id
		end
	else
		begin
			select 
				ap.id, 
				ap.billing_account_id,
				ap.payment_mode, 
				case when ap.payment_mode='0' then 'OFFLINE' else 'ONLINE' end payment_mode_name, 
				ap.payref_no, 
				ap.payref_date, 
				ap.processing_ref_no, 
				ap.processing_ref_date, 
				ap.processing_pg_name, 
				ap.processing_status, 
				case when ap.payment_mode='0' then 'Pass' else (case when ap.processing_status='1' then 'Pass' else 'Failed' end) end processing_status_name, 
				ap.payment_amount, 
				ap.remarks,
				ap.auth_code,
				ap.cvv_response,
				ap.avs_response,
				ap.created_by, 
				ap.created_by user_id, 
				u.name user_name, 
				ap.date_created, 
				ap.updated_by, 
				ap.date_updated,
				ba.name billing_account_name, 
				ba.code billing_account_code
			from ar_payments ap
			inner join billing_account ba on ap.billing_account_id=ba.id
			inner join users u on ap.created_by=u.id
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
	--invoice outstanding
	select a.id, a.invoice_no, a.invoice_date, a.total_amount, 
			sum(a.adjusted) already_adjusted,
			sum(a.refunded) refunded,
			a.total_amount-sum(a.adjusted+a.refunded) balance,
			sum(a.curr_adjusted) adjusted,
			a.total_amount-sum(a.adjusted+a.curr_adjusted+a.refunded) current_balance,
			cast(0 as bit) selected from (
				select hdr.id,hdr.invoice_no,hdr.opbal_date invoice_date,hdr.opbal_amount total_amount, case when aj.ar_refunds_id is null then isnull(aj.adj_amount,0) else 0.00 end adjusted, case when aj.ar_refunds_id is not null then isnull(aj.adj_amount,0) else 0.00 end refunded, 0.00 curr_adjusted 
				from ar_opening_balance hdr with(nolock) 
				left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id --and aj.adj_source='O'
				where hdr.billing_account_id = @billing_account_id 
					  and ISNULL(aj.ar_payments_id,'00000000-0000-0000-0000-000000000000')<>@id
					  and hdr.invoice_no is not null
					  and hdr.opbal_amount>0
				union all
				select hdr.id,hdr.invoice_no,hdr.invoice_date,hdr.total_amount, case when aj.ar_refunds_id is null then isnull(aj.adj_amount,0) else 0.00 end adjusted, case when aj.ar_refunds_id is not null then isnull(aj.adj_amount,0) else 0.00 end refunded, 0.00 curr_adjusted 
				from invoice_hdr hdr with(nolock) 
				left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id
				left join billing_account ba with(nolock) on ba.id=hdr.billing_account_id
				where hdr.billing_account_id = @billing_account_id 
					  and ISNULL(aj.ar_payments_id,'00000000-0000-0000-0000-000000000000')<>@id
					  and hdr.invoice_no is not null
					  and hdr.total_amount>0
					  and hdr.approved='Y'
				union all
				select hdr.id,hdr.invoice_no,hdr.opbal_date invoice_date,hdr.opbal_amount total_amount,0.00 as adjusted, case when aj.ar_refunds_id is not null then isnull(aj.adj_amount,0) else 0.00 end refunded,  case when aj.ar_refunds_id is null then isnull(aj.adj_amount,0) else 0.00 end curr_adjusted 
				from ar_opening_balance hdr with(nolock) 
				left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id --and aj.adj_source='O'
				where hdr.billing_account_id = @billing_account_id 
					  and ISNULL(aj.ar_payments_id,'00000000-0000-0000-0000-000000000000')=@id
					  and hdr.invoice_no is not null
					  and hdr.opbal_amount>0
				union all
				select hdr.id,hdr.invoice_no,hdr.invoice_date,hdr.total_amount,0.00 as adjusted, case when aj.ar_refunds_id is not null then isnull(aj.adj_amount,0) else 0.00 end refunded,  case when aj.ar_refunds_id is null then isnull(aj.adj_amount,0) else 0.00 end curr_adjusted 
				from invoice_hdr hdr with(nolock) 
				left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id
				where hdr.billing_account_id = @billing_account_id 
					  and ISNULL(aj.ar_payments_id,'00000000-0000-0000-0000-000000000000')=@id
					  and hdr.invoice_no is not null
					  and hdr.total_amount>0
					  and hdr.approved='Y'
	) a
	group by a.id, a.invoice_no, a.invoice_date, a.total_amount
	having case @fetch_for_new when 1 then a.total_amount-sum(a.adjusted+a.refunded) else sum(a.curr_adjusted) end >0
	order by convert(datetime,a.invoice_date) asc;

	-- all payments
	select 
			ap.id, 
			ap.billing_account_id,
			ap.payment_mode, 
			case when ap.payment_mode='0' then 'OFFLINE' else 'ONLINE' end payment_mode_name, 
			ap.payref_no, 
			ap.payref_date, 
			ap.processing_ref_no, 
			ap.processing_ref_date, 
			ap.processing_pg_name, 
			ap.processing_status, 
			case when ap.payment_mode='0' then 'Pass' else (case when ap.processing_status='1' then 'Pass' else 'Failed' end) end processing_status_name, 
			ap.payment_amount, 
			ap.created_by, 
			ap.date_created, 
			ap.updated_by, 
			ap.date_updated,
			ba.name billing_account_name, 
			ba.code billing_account_code
		from ar_payments ap
		inner join billing_account ba on ap.billing_account_id=ba.id
		where ba.id=@billing_account_id
		order by ap.date_created desc;

	

	set nocount off
end

GO
