USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[common_check_record_lock_ui]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[common_check_record_lock_ui]
GO
/****** Object:  StoredProcedure [dbo].[common_check_record_lock_ui]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : common_check_record_lock_ui : check record lock
** Created By   : Pavel Guha 
** Created On   : 11/04/2019
*******************************************************/
--exec common_check_record_lock_ui 20,'ED4244CD-EBB5-4EEA-B013-DE63EEC525D1','11111111-1111-1111-1111-111111111111','EED6DCC6-7948-4AFA-BCA7-C9F1757B2552','','',0
CREATE procedure [dbo].[common_check_record_lock_ui]
    @menu_id int,
    @record_id uniqueidentifier,
    @user_id uniqueidentifier,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @user_name nvarchar(130) = '' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	  declare @lock_user_id uniqueidentifier
	  --print @record_id
	  --print @menu_id
	  --print @user_id
	  --print @session_id
	  if(select count(record_id) from sys_record_lock_ui where record_id=@record_id and menu_id=@menu_id and user_id<>@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')<>@session_id)>0
		begin
			select @lock_user_id=user_id  
			from sys_record_lock_ui 
			where record_id=@record_id 
			and menu_id=@menu_id 
			
			
			select @user_name = name 
			from users 
			where id=@lock_user_id
			
			if(@record_id<>'00000000-0000-0000-0000-000000000000')
				select @error_code='033',@return_status=0
			--else if(@record_id='00000000-0000-0000-0000-000000000000')
			--	select @error_code='110',@return_status=0
			print @user_name
			return 
		end
	 else
		begin
			select @user_name='',@error_code='',@return_status=1
			return 
		end 
	  

	
end

GO
