USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_promotion_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_promotion_fetch_dtls]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_promotion_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_promotion_fetch_dtls : fetch promotions details
** Created By   : BK
** Created On   : 27/11/2019
*******************************************************/
CREATE procedure [dbo].[invoicing_promotion_fetch_dtls]
(
	@id					uniqueidentifier,
    @menu_id			int,
    @user_id			uniqueidentifier,
    @error_code			nvarchar(10)		= '' output,
    @return_status		int					= 0 output
)
as
	begin
		set nocount on
		select ap.promotion_type,
			   ap.billing_account_id,
			   valid_from = isnull(ap.valid_from,'01jan1900'),
			   ap.valid_till,
			   ap.is_active,
			   ap.date_created,
			   reason_id = isnull(ap.reason_id,'00000000-0000-0000-0000-000000000000')
		from ar_promotions ap
		where ap.id=@id

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

		exec invoicing_promotion_fetch_params
		     @menu_id = @menu_id

		set nocount off
	end
GO
