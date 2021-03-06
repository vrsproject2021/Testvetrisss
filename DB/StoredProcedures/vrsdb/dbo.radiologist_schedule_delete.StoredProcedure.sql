USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_schedule_delete]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[radiologist_schedule_delete]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_schedule_delete]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : radiologist_schedule_delete : delete
                  radiologist schedule details for single instance.
** Created By   : Pavel Guha
** Created On   : 19/09/2019
*******************************************************/
CREATE procedure [dbo].[radiologist_schedule_delete]
(
	@id              uniqueidentifier,
	@updated_by      uniqueidentifier,
	@menu_id         int,
	@error_code		 nvarchar(10)  = '' output,
    @return_status   int		   = 0  output
)
as
begin
	set nocount on 

	begin transaction

	
	delete from radiologist_schedule where id = @id
	
	if(@@rowcount=0)
		begin
			rollback transaction
			select @error_code='198',@return_status=0
			return 0
		end

	
	commit transaction
	set @return_status=1
	set @error_code='198'
	set nocount off

	return 1
end



GO
