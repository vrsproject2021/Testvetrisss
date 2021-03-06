USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_control_params_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_control_params_fetch]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_control_params_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_control_params_fetch : fetch 
				  invoicing control parameters 
** Created By   : BK
** Created On   : 11/11/2019
*******************************************************/
create procedure [dbo].[invoicing_control_params_fetch]
(
	@user_role_id	bigint,
	@menu_id		bigint,
	@user_id		uniqueidentifier,
	@error_code		nvarchar(10)		= '' output,
    @return_status	int					= 0  output
)
as
	begin
		set nocount on

		exec common_lock_record
		@menu_id       = @menu_id,
		@record_id     = @menu_id,
		@user_id       = @user_id,
		@error_code    = @error_code output,
		@return_status = @return_status output	
						
		if(@return_status=0)
			begin
				return 0
			end

		select * from invoicing_control_params

		set nocount off
	end
GO
