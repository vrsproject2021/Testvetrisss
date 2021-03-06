USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[dicom_router_import_session_check]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[dicom_router_import_session_check]
GO
/****** Object:  StoredProcedure [dbo].[dicom_router_import_session_check]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : dicom_router_import_session_check : fetch
                  institution details
** Created By   : Pavel Guha
** Created On   : 10/11/2019
*******************************************************/
--exec dicom_router_import_session_check '00037','S1D1211190710125Pv',0,'',0
CREATE procedure [dbo].[dicom_router_import_session_check]
	@institution_code nvarchar(5),
	@import_session_id nvarchar(30),
	@files_imported int = 0 output,
	@output_msg nvarchar(100)='' output,
	@return_status int = 0 output
as
begin
	set nocount on
	declare @id uniqueidentifier

	select @id= id from institutions where code=@institution_code
	set @files_imported=0

	--if(select count(id) from scheduler_file_downloads where institution_code=@institution_code and import_session_id=@import_session_id)>0
	--	begin	
	--		select @files_imported = count(file_name)
	--		from scheduler_file_downloads_dtls
	--		where id = (select id from scheduler_file_downloads where institution_code=@institution_code and import_session_id=@import_session_id)
			
	--    end 

	if(select count(study_uid) from dicom_router_files_received where institution_code=@institution_code and import_session_id=@import_session_id)>0
		begin	
			select @files_imported = count(file_name)
			from dicom_router_files_received
			where institution_code=@institution_code 
			and import_session_id=@import_session_id
	    end

	if(select count(id) from scheduler_img_file_downloads_ungrouped where institution_code=@institution_code and import_session_id=@import_session_id)>0
		begin
			select @files_imported =@files_imported + (select count(file_name) 
			                                           from scheduler_img_file_downloads_ungrouped 
													   where institution_code=@institution_code 
													   and import_session_id=@import_session_id)
				  
		end
	
	--print @files_imported
	  select @output_msg='SUCCESS', @return_status=1

	set nocount off
	return 1
	
end
GO
