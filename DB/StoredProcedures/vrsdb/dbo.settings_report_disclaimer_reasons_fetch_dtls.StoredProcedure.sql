USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_report_disclaimer_reasons_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_report_disclaimer_reasons_fetch_dtls]
GO
/****** Object:  StoredProcedure [dbo].[settings_report_disclaimer_reasons_fetch_dtls]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_report_disclaimer_reasons_fetch_dtls : fetch user details
** Created By   : Pavel Guha
** Created On   : 18/11/2020
*******************************************************/
--exec settings_report_disclaimer_reasons_fetch_dtls '3BE93540-F0C9-4947-9E46-7A7B37F8D9FD',1,'11111111-1111-1111-1111-111111111111','',0
create procedure [dbo].[settings_report_disclaimer_reasons_fetch_dtls]
    @id int,
    @menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	 set nocount on


	select type,
	       description = ISNULL(description,''),
	       is_active 
	from report_disclaimer_reasons  
	where id=@id

	
	if(@id<>0)
		begin
			
				if(select count(record_id) from sys_record_lock where record_id=@id and menu_id=@menu_id)=0
					begin
						exec common_lock_record
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
			if(select count(record_id) from sys_record_lock where user_id=@user_id)>0
			    begin
				  delete from sys_record_lock where user_id=@user_id
				  delete from sys_record_lock_ui where user_id=@user_id
			    end
		end

	

		
	set nocount off
end

GO
