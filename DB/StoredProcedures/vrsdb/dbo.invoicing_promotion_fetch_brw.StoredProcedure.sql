USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_promotion_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_promotion_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_promotion_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_promotion_fetch_brw : fetch promotions browser
** Created By   : BK
** Created On   : 25/11/2019
*******************************************************/
CREATE procedure [dbo].[invoicing_promotion_fetch_brw]
(
	@billing_account_id	uniqueidentifier,
	@promotion_type     nchar(1)                ='A',
	@is_active			nchar(1)				= 'A',
	@created_by			uniqueidentifier        ='00000000-0000-0000-0000-000000000000',
	@reason_id          uniqueidentifier        ='00000000-0000-0000-0000-000000000000',
	@menu_id			int,
    @user_id			uniqueidentifier,
    @error_code			nvarchar(10)		= '' output,
    @return_status		int					= 0 output
)
as
	begin
		set nocount on
		declare @strSQL varchar(max)
		delete from sys_record_lock where user_id = @user_id
	    delete from sys_record_lock_ui where user_id = @user_id

		--update ar_promotions set is_active='N' where valid_till < convert(datetime,convert(varchar(11),getdate(),106))

		set @strSQL='select '
		set @strSQL= @strSQL +' ap.id,'
		set @strSQL= @strSQL +'	ap.created_by as user_id,'
		set @strSQL= @strSQL +'	u.name as user_name,'
		set @strSQL= @strSQL +'	ap.billing_account_id,'
		set @strSQL= @strSQL +'	ba.code as billing_account_code,'
		set @strSQL= @strSQL +'	ba.name as billing_account_name,'
		set @strSQL= @strSQL +'	ap.date_created	 as created_on,'
		set @strSQL= @strSQL +'	pr.reason,'
		set @strSQL= @strSQL +'	case when ap.promotion_type=''D'' then ''Discount'' when promotion_type=''F'' then ''Free Credits'' end  promotion_type,'
		set @strSQL= @strSQL +'	ap.valid_till,'
		set @strSQL= @strSQL +'	case when ap.is_active=''Y'' then ''Active'' else ''Inactive'' end  is_active'
		set @strSQL= @strSQL +' from ar_promotions ap '
		set @strSQL= @strSQL +' inner join billing_account ba on ba.id = ap.billing_account_id '
		set @strSQL= @strSQL +' inner join promo_reasons pr on pr.id = ap.reason_id '
		set @strSQL= @strSQL +' inner join users u on u.id = ap.created_by'
		set @strSQL= @strSQL +' where 1 = 1 '

		
		if(isnull(@billing_account_id,'00000000-0000-0000-0000-000000000000') <>'00000000-0000-0000-0000-000000000000')
			begin
				set @strSQL= @strSQL +' and ap.billing_account_id= '''+ convert(varchar(36),@billing_account_id) + ''' '
			end
		if(isnull(@promotion_type,'A') <>'A')
			begin
				set @strSQL= @strSQL +' and ap.promotion_type= '''+ @promotion_type + ''' '
			end
		if(isnull(@is_active,'') <> 'A')
			begin
				set @strSQL= @strSQL +' and ap.is_active='''+@is_active+ ''' '
			end
		if(isnull(@created_by,'00000000-0000-0000-0000-000000000000') <>'00000000-0000-0000-0000-000000000000')
			begin
				set @strSQL=@strSQL+' and ap.created_by= '''+ convert(varchar(36),@created_by) + ''' '
			end
		if(isnull(@reason_id,'00000000-0000-0000-0000-000000000000') <>'00000000-0000-0000-0000-000000000000')
			begin
				set @strSQL= @strSQL +' and ap.reason_id= '''+ convert(varchar(36),@reason_id) + ''' '
			end

		set @strSQL=@strSQL+' order by ap.date_created desc,ba.name'

		--print @strSQL
		exec(@strSQL)
		set nocount off
	end
GO
