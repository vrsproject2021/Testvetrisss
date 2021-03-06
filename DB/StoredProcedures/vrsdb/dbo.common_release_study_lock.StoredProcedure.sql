USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[common_release_study_lock]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[common_release_study_lock]
GO
/****** Object:  StoredProcedure [dbo].[common_release_study_lock]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : common_release_study_lock : release study lock
** Created By   : Pavel Guha
** Created On   : 14/04/2021
*******************************************************/
--exec case_list_fetch_hdr '97e25bfc-72c7-444c-a5c5-e6b7693fcd64',21,'11111111-1111-1111-1111-111111111111','',0
CREATE procedure [dbo].[common_release_study_lock]
    @id uniqueidentifier,	
    @menu_id int,
    @user_id uniqueidentifier,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000'
as
begin
	 set nocount on

	 declare @activity_text nvarchar(max),
	         @error_code nvarchar(10),
			 @return_status int

	 set @activity_text =  'Lock Released'
	 set @error_code=''
	 set @return_status=0

	 exec common_study_user_activity_trail_save
		@study_hdr_id  = @id,
		@study_uid     = '',
		@menu_id       = @menu_id,
		@activity_text = @activity_text,
		@session_id    = @session_id,
		@activity_by   = @user_id,
		@error_code    = @error_code output,
		@return_status = @return_status output

	

	 exec login_menu_record_count_fetch
	     @user_id = @user_id

	 set nocount off
end
GO
