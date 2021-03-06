USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_billing_cycle_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_billing_cycle_fetch_dtls]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_billing_cycle_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_billing_cycle_fetch_dtls : fetch bill cycle details
** Created By   : BK
** Created On   : 25/07/2019
*******************************************************/
CREATE procedure [dbo].[invoicing_billing_cycle_fetch_dtls]
(
	@id uniqueidentifier,
    @menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
)

as

	begin
		set nocount on

		select 
	       name			= isnull(bc.name,''),
		   date_from,
		   date_till,
		   locked
		   
		from billing_cycle bc 
		where id=@id

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
		
		set nocount off
	end
GO
