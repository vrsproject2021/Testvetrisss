USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_day_end_fetch_image_records_to_delete]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_day_end_fetch_image_records_to_delete]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_day_end_fetch_image_records_to_delete]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 
** Procedure    : scheduler_day_end_fetch_image_records_to_delete : 
				  fetch image records records to delete
** Created By   : Pavel Guha
** Created On   : 02/05/2020
*******************************************************/
-- exec scheduler_day_end_fetch_image_records_to_delete 
create procedure [dbo].[scheduler_day_end_fetch_image_records_to_delete]

as
begin
	select  id,file_name,import_session_id,institution_id
	from scheduler_image_files_to_delete 
	order by id

end

GO
