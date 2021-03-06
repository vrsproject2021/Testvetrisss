USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_downloaded_listener_file_check]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_downloaded_listener_file_check]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_downloaded_listener_file_check]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_downloaded_listener_file_check : 
                  save file downloaded from listener
** Created By   : Pavel Guha
** Created On   : 09/02/2021
*******************************************************/
create procedure [dbo].[scheduler_downloaded_listener_file_check]
	@file_name nvarchar(500)
as
begin
	
	set nocount on

	declare @id uniqueidentifier


	if(select count(id) from scheduler_file_downloads_dtls where file_name=@file_name)>0
		begin
			select @id = id
			from scheduler_file_downloads_dtls 
			where file_name=@file_name
		end
	else
		begin
			select @id= '00000000-0000-0000-0000-000000000000'
		end

	select  id,
	        study_uid,
			institution_id,
			institution_code,
			institution_name
	from scheduler_file_downloads
	where id= @id
	

	set nocount off


end


GO
