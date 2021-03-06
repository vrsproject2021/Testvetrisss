USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_schedule_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[radiologist_schedule_update]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_schedule_update]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : radiologist_schedule_update : add/edit/delete
                  radiologist schedule
** Created By   : Pavel Guha
** Created On   : 18/09/2019
*******************************************************/
create procedure [dbo].[radiologist_schedule_update]
(
	@strQuery        nvarchar(4000),
	@updated_by      uniqueidentifier,
	@menu_id         int,
	@error_code		 nvarchar(10)  = '' output,
    @return_status   int		   = 0  output
)
as
begin
	set nocount on 
	begin transaction

	exec(@strQuery)

	if(@@rowcount = 0)
		begin
			rollback transaction
			select @error_code='193',@return_status=0
			return 0
		end

	commit transaction
	set @return_status=1
	set @error_code='194'
	set nocount off

	return 1
end



GO
