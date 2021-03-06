USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_billing_cycle_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_billing_cycle_save]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_billing_cycle_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_billing_cycle_save : save
                  bill cycle details.
** Created By   : BK
** Created On   : 07/11/2019
*******************************************************/
CREATE procedure [dbo].[invoicing_billing_cycle_save]
(
	@id						uniqueidentifier	= '00000000-0000-0000-0000-000000000000' output,
	@name					nvarchar(150)		= '',
	@date_from				datetime,
	@date_till				datetime,
	@locked					char(1)				= 'N',
	@user_id				uniqueidentifier,
    @menu_id                int,
    @user_name              nvarchar(700)		= '' output,
	@error_code				nvarchar(10)		= '' output,
    @return_status			int					= 0  output
)
as
	begin
		
		begin transaction
		if(@id = '00000000-0000-0000-0000-000000000000')
			begin
				 set @id = newid()
				 insert into billing_cycle
				 (
					id,
					name,
					date_from,
					date_till,
					locked,
					date_created,
					created_by
				 )
				 values
				 (
					@id,
					@name,
					@date_from,
					@date_till,
					@locked,
					GETDATE(),
					@user_id

				 )

				 if(@@rowcount=0)
				begin
					rollback transaction
					select	@return_status=0,@error_code='035'
					return 0
				end
			end
		else
			begin
				exec common_check_record_lock_ui
				@menu_id       = @menu_id,
				@record_id     = @id,
				@user_id       = @user_id,
				@user_name     = @user_name output,
				@error_code    = @error_code output,
				@return_status = @return_status output
		
				if(@return_status=0)
					begin
						rollback transaction
						return 0
					end


				update billing_cycle
					set name=@name,
						date_from=@date_from,
						date_till=@date_till,
						locked=@locked,
						date_updated=GETDATE(),
						updated_by=@user_id
				where id = @id

				if(@@rowcount=0)
				begin
					rollback transaction
					select	@return_status=0,@error_code='035'
					return 0
				end

			end



		exec common_lock_record_ui
		@menu_id       = @menu_id,
		@record_id     = @id,
		@user_id       = @user_id,
		@error_code    = @error_code output,
		@return_status = @return_status output

		if(@return_status=0)
			begin
				rollback transaction
				return 0
			end

		commit transaction

		set @return_status=1
		set @error_code='034'
		set nocount off

		return 1
	end
GO
