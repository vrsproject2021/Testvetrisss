USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_modality_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_modality_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[master_modality_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_modality_fetch_brw :  fetch case(s)
				  requiring action
** Created By   : Pavel Guha
** Created On   : 10/04/2019
*******************************************************/
-- exec master_modality_fetch_brw '','','Y','11111111-1111-1111-1111-111111111111',10,'b6f78363-bd97-463d-b84f-a0fa8c986b23','',0
CREATE PROCEDURE [dbo].[master_modality_fetch_brw] 
    @code nvarchar(10) ='',
    @name nvarchar(50),
	@is_active nchar(1)='X',
    @user_id uniqueidentifier,
	@menu_id int,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@error_code nvarchar(10)='' output,
	@return_status int =0 output 
as
begin
	set nocount on
	declare @strSQL varchar(max),
	        @activity_text nvarchar(max),
			@menu_text nvarchar(100)

	delete from sys_record_lock where user_id = @user_id
	delete from sys_record_lock_ui where user_id = @user_id

	create table #tmp
	(
		rec_id int identity(1,1),
		id int,
		code nvarchar(10),
		name nvarchar(50),
		dicom_tag nvarchar(50),
		track_by nchar(1),
		invoice_by nchar(1),
		file_receive_path nvarchar(250),
		is_active nchar(1),
		changed nchar(1) null default 'N',
		action nvarchar(1) null default '',
	)
	
	set @strSQL='insert into #tmp(id,code,name,dicom_tag,track_by,invoice_by,file_receive_path,is_active)'
	set @strSQL= @strSQL + '(select id,code,name,dicom_tag,track_by,invoice_by,file_receive_path,is_active '
    set @strSQL= @strSQL + 'from modality '
	set @strSQL= @strSQL + 'where 1 = 1 ' 
	
 
	if(isnull(@code,'') <>'')
		begin
			set @strSQL= @strSQL + ' and upper(code) like ''%'+upper(@code)+'%'' ' 
		end
	if (isnull(@name,'') <>'')
		 begin
			set @strSQL=@strSQL+' and upper(name) like ''%'+upper(@name)+'%'' '
		 end
	if (isnull(@is_active,'X') <>'X')
		begin
			set @strSQL=@strSQL+'and is_active = '''+ @is_active + ''' '
		 end

    set @strSQL= @strSQL + ') order by name'
	--print @strSQL
	exec(@strSQL)
	select * from #tmp

	drop table #tmp

	if(select count(record_id) from sys_record_lock where record_id=@menu_id and menu_id=@menu_id)=0
		begin
			exec common_lock_record
				@menu_id       = @menu_id,
				@record_id     = @menu_id,
				@user_id       = @user_id,
				@session_id    = @session_id,
				@error_code    = @error_code output,
				@return_status = @return_status output	
						
			if(@return_status=0)
				begin
					return 0
				end

			select @menu_text= menu_desc from sys_menu where menu_id=@menu_id
			set  @activity_text =  isnull(@menu_text,'')  + '==> Opened & Locked ' 
			exec common_user_activity_log
					@user_id       = @user_id,
					@activity_text = @activity_text,
					@session_id    = @session_id,
					@menu_id       = @menu_id,
					@error_code    = @error_code output,
					@return_status = @return_status output

			if(@return_status=0)
				begin
					return 0
				end
		end
	select @error_code='',@return_status=1
	set nocount off
end




GO
