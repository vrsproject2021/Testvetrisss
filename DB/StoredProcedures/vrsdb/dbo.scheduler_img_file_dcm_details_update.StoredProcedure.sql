USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_img_file_dcm_details_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_img_file_dcm_details_update]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_img_file_dcm_details_update]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_img_file_dcm_details_update : 
                  update downloaded image file's dcm details
** Created By   : Pavel Guha
** Created On   : 20/08/2019
*******************************************************/
create procedure [dbo].[scheduler_img_file_dcm_details_update]
	@id uniqueidentifier,
	@file_name nvarchar(250),
	@dcm_file_name  nvarchar(250),
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	set nocount on

	begin transaction 

	update scheduler_img_file_downloads_grouped_dtls
	set dicomised     ='Y',
	    dcm_file_name = @dcm_file_name
	where id = @id
	and file_name = @file_name

	if(@@rowcount=0)
		begin
			rollback transaction
			select @return_type=0,@error_msg='Failed to update dcm file details of file ' + @file_name
			return 0
		end

	commit transaction
	set @return_type=1
	set @error_msg=''
	set nocount off
	return 1

end


GO
