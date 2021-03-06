USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[operation_time_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[operation_time_fetch]
GO
/****** Object:  StoredProcedure [dbo].[operation_time_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : operation_time_fetch : fetch operation time
** Created By   : Pavel Guha 
** Created On   : 19/11/2020
*******************************************************/
CREATE Procedure [dbo].[operation_time_fetch]
    @menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
As
	Begin
		delete from sys_record_lock where user_id = @user_id
	    delete from sys_record_lock_ui where user_id = @user_id

		select day_no,day_name,from_time,till_time,time_zone_id,message_display=isnull(message_display,'') from settings_operation_time

		if(select count(record_id) from sys_record_lock where record_id=@menu_id and menu_id=@menu_id)=0
			begin
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
			end
	End
GO
