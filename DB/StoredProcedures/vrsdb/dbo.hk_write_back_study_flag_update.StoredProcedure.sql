USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[hk_write_back_study_flag_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[hk_write_back_study_flag_update]
GO
/****** Object:  StoredProcedure [dbo].[hk_write_back_study_flag_update]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : hk_write_back_study_flag_update : 
                  update write back flag
** Created By   : Pavel Guha
** Created On   : 16/10/2019
*******************************************************/
CREATE procedure [dbo].[hk_write_back_study_flag_update]
    @id uniqueidentifier,
	@pacs_wb nchar(1),
	@error_code nvarchar(10) = '' output,
	@return_status int = 0 output
as
begin
	
	set nocount on
	declare @wb nchar(1)

	if(@pacs_wb='Y') set @wb='N'
	else if(@pacs_wb='N') set @wb='Y'
	
	begin transaction

	if(select count(id) from study_hdr where id=@id)>0
		begin
			update study_hdr
			set pacs_wb=@wb
			where id = @id
		end
	else
		begin
			update study_hdr_archive
			set pacs_wb=@wb
			where id = @id
		end

	if(@@rowcount = 0)
		begin
			rollback transaction
			select @return_status=0,@error_code='213' 
			return 0
		end

	commit transaction 
	select @return_status=1,@error_code='212'
	set nocount off
	return 1

end


GO
