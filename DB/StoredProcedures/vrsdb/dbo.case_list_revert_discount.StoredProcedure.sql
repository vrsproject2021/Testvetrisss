USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_revert_discount]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_revert_discount]
GO
/****** Object:  StoredProcedure [dbo].[case_list_revert_discount]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_revert_discount : 
                  apply discount on a case
** Created By   : Pavel Guha 
** Created On   : 05 Jun 2020
*******************************************************/

CREATE procedure [dbo].[case_list_revert_discount]
	@study_id uniqueidentifier,
	@reason_id uniqueidentifier,
	@menu_id int,
    @user_id uniqueidentifier,
    @user_name nvarchar(500) = '' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
	begin
		set nocount on
		

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
				update study_hdr
				set discount_per      = 0,
					promo_reason_id   = @reason_id,
					promo_applied_by  = @user_id,
					promo_applied_on  =getdate()
				where id = @study_id
			end
		else if(select count(id) from study_hdr_archive where id=@study_id)>0
			begin
				update study_hdr_archive
				set discount_per      = 0,
					promo_reason_id   = @reason_id,
					promo_applied_by  = @user_id,
					promo_applied_on  =getdate()
				where id = @study_id
			end


		if(@@rowcount=0)
			begin
				rollback transaction
				select @error_code='323',@return_status=0,@user_name=''
				return 0
			end

		commit transaction
		select @error_code='324',@return_status=1,@user_name=''
		set nocount off
		return 1
	end
GO
