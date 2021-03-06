USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_apply_discount]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_apply_discount]
GO
/****** Object:  StoredProcedure [dbo].[case_list_apply_discount]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_apply_discount : 
                  apply discount on a case
** Created By   : Pavel Guha 
** Created On   : 02/01/2020
*******************************************************/

CREATE procedure [dbo].[case_list_apply_discount]
	@study_id uniqueidentifier,
	@discount_type nchar(1),
	@discount_percent decimal(5,2) = 0,
	@discount_amount money = 0,
	@reason_id uniqueidentifier,
	@menu_id int,
    @user_id uniqueidentifier,
	@session_id uniqueidentifier ='00000000-0000-0000-0000-000000000000',
    @user_name nvarchar(500) = '' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
	begin
		set nocount on
		
		declare @activity_text nvarchar(max),
		        @menu_text nvarchar(100),
		        @promo_reason nvarchar(250),
				@study_uid nvarchar(100)

		 exec common_check_record_lock_ui
				@menu_id       = @menu_id,
				@record_id     = @study_id,
				@user_id       = @user_id,
				@user_name     = @user_name output,
				@error_code    = @error_code output,
				@return_status = @return_status output
		
		if(@return_status=0)
			begin
				return 0
			end

		if(select count(id) from study_hdr where id=@study_id)>0
			begin
				if(select isnull(invoiced,'N') from study_hdr where id=@study_id)='Y'
					begin
						select @error_code='267',@return_status=0,@user_name=''
						return 0
					end
			end
		else if(select count(id) from study_hdr_archive where id=@study_id)>0
			begin
				if(select isnull(invoiced,'N') from study_hdr where id=@study_id)='Y'
					begin
						select @error_code='267',@return_status=0,@user_name=''
						return 0
					end
			end
		else
			begin
				select @error_code='268',@return_status=0,@user_name=''
				return 0	
			end

		begin transaction

		if(select count(id) from study_hdr where id=@study_id)>0
			begin
				select @study_uid = study_uid from study_hdr where id=@study_id

				if(@discount_type ='P')
					begin
						update study_hdr
						set discount_per      = @discount_percent,
						    discount_type     = @discount_type,
							promo_reason_id   = @reason_id,
							promo_applied_by  = @user_id,
							promo_applied_on  =getdate()
						where id = @study_id
					end
				else if(@discount_type ='A')
					begin
						update study_hdr
						set discount_amount   = @discount_amount,
						    discount_type     = @discount_type,
							promo_reason_id   = @reason_id,
							promo_applied_by  = @user_id,
							promo_applied_on  =getdate()
						where id = @study_id
					end
			end
		else if(select count(id) from study_hdr_archive where id=@study_id)>0
			begin
				select @study_uid = study_uid from study_hdr_archive where id=@study_id

				if(@discount_type ='P')
					begin
						update study_hdr_archive
						set discount_per      = @discount_percent,
							discount_type     = @discount_type,
							promo_reason_id   = @reason_id,
							promo_applied_by  = @user_id,
							promo_applied_on  =getdate()
						where id = @study_id
					end
				else if(@discount_type ='A')
					begin
						update study_hdr_archive
						set discount_amount   = @discount_amount,
						    discount_type     = @discount_type,
							promo_reason_id   = @reason_id,
							promo_applied_by  = @user_id,
							promo_applied_on  =getdate()
						where id = @study_id
					end
			end


		if(@@rowcount=0)
			begin
				rollback transaction
				select @error_code='266',@return_status=0,@user_name=''
				return 0
			end

		select @promo_reason = reason from promo_reasons where id = @reason_id
		if(@discount_type ='P')
			begin
				set @activity_text = 'Applied Discount : ' + convert(nvarchar(15), @discount_percent) + '%, Reason :' + @promo_reason 
			end
		else if(@discount_type ='A')
			begin
				set @activity_text = 'Applied Discount : ' + convert(nvarchar(15), @discount_amount) + ', Reason :' + @promo_reason 
			end

		exec common_study_user_activity_trail_save
				@study_hdr_id  = @study_id,
				@study_uid     ='',
				@menu_id       = @menu_id,
				@activity_text = @activity_text,
				@session_id    = @session_id,
				@activity_by   = @user_id,
				@error_code    = @error_code output,
				@return_status = @return_status output

		if(@return_status=0)
			begin
				rollback transaction
				return 0
			end

        select @menu_text= menu_desc from sys_menu where menu_id=@menu_id
		set @activity_text = isnull(@menu_text,'') + '==> Study UID' + @study_uid + '=>' + @activity_text 
		exec common_user_activity_log
			 @user_id       = @user_id,
			 @activity_text = @activity_text,
			 @session_id    = @session_id,
			 @menu_id       = @menu_id,
			 @error_code    = @error_code output,
			 @return_status = @return_status output

	  if(@return_status=0)
			begin
			    rollback transaction
				return 0
			end

		commit transaction
		select @error_code='269',@return_status=1,@user_name=''
		set nocount off
		return 1
	end
GO
