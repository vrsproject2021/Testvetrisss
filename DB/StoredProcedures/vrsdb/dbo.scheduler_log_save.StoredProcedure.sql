USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_log_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_log_save]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_log_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_log_save : save scheduler log
** Created By   : Pavel Guha
** Created On   : 12/04/2019
*******************************************************/
--exec scheduler_log_save 1,0,'Test','',0
CREATE procedure [dbo].[scheduler_log_save]
    @is_error bit=0,
    @service_id int=0,
	@log_message varchar(8000)='',
	@error_msg nvarchar(100) = '' output,
	@return_type int = 0 output
as
begin
	
	insert into vrslogdb..sys_scheduler_log(is_error,service_id,log_date,log_message)
	                       values(@is_error,@service_id,getdate(),@log_message)

	if(@@rowcount>0)
		begin
			select  @error_msg='',@return_type=1	
			return 1				
		end
	else
		begin
			select  @error_msg='Failed to save the scheduler log',@return_type=0
			return 0
		end

	--print @error_msg
	--print @return_type
	
end


GO
