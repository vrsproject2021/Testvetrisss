USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_report_disclaimer_reasons_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_report_disclaimer_reasons_save]
GO
/****** Object:  StoredProcedure [dbo].[settings_report_disclaimer_reasons_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_report_disclaimer_reasons_save : save 
                  report disclaimer reasons
** Created By   : Pavel Guha
** Created On   : 16/11/2020
*******************************************************/
CREATE procedure [dbo].[settings_report_disclaimer_reasons_save]
    @id			  int=0 output,
	@type         nvarchar(30),
	@description  ntext,
	@is_active    nchar(1) ='Y',
    @updated_by   uniqueidentifier,
    @menu_id      int,
    @user_name    nvarchar(500) = '' output,
    @error_code   nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	set nocount on 
	
	if(select count(id) from report_disclaimer_reasons where upper(type) = upper(@type) and id<>@id)>0
		begin
				select @error_code='421',@return_status=0,@user_name=''
				return 0
		end

    
	
	
	
	begin transaction
	
	if(@id=0)
		begin
			insert into report_disclaimer_reasons(type,description,is_active,created_by,date_created)
					                       values(@type,@description,@is_active,@updated_by,getdate())

			if(@@rowcount=0)
				begin
					rollback transaction
					select	@return_status=0,
							@error_code='035'
					return 0
				end
		end
	else
		begin
			exec common_check_record_lock
				@menu_id       = @menu_id,
				@record_id     = @id,
				@user_id       = @updated_by,
				@user_name     = @user_name output,
				@error_code    = @error_code output,
				@return_status = @return_status output
		
			if(@return_status=0)
				begin
					return 0
				end
				
			update report_disclaimer_reasons
			set    type           = @type,
			        description   = @description,
					is_active     = @is_active,
					updated_by    = @updated_by,
					date_updated  = getdate()
			where id=@id 

			if(@@rowcount=0)
				begin
					rollback transaction
					select	@return_status=0,
							@error_code='035'
					return 0
				end
		end
	
	exec common_lock_record
		@menu_id       = @menu_id,
		@record_id     = @id,
		@user_id       = @updated_by,
		@error_code    = @error_code output,
		@return_status = @return_status output

	if(@return_status=0)
		begin
			rollback transaction
			return 0
		end
	

		
	commit transaction
	select @error_code='034',@return_status=1
	set nocount off
	return 1
	
end

GO
