USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_study_types_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_study_types_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[master_study_types_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_study_types_fetch_brw :  fetch case(s)
				  requiring action
** Created By   : Pavel Guha
** Created On   : 10/05/2019
*******************************************************/
-- exec master_study_types_fetch_brw '',0,'X','11111111-1111-1111-1111-111111111111',14,'',0
CREATE PROCEDURE [dbo].[master_study_types_fetch_brw] 
    @name nvarchar(50),
	@modality_id int =0,
	@category_id int =0,
	@is_active nchar(1)='X',
    @user_id uniqueidentifier,
	@menu_id int,
	@error_code nvarchar(10)='' output,
	@return_status int =0 output 
as
begin
	set nocount on
	declare @strSQL varchar(max)

	delete from sys_record_lock where user_id = @user_id
	delete from sys_record_lock_ui where user_id = @user_id

	create table #tmp
	(
		rec_id int identity(1,1),
		id uniqueidentifier,
		name nvarchar(50),
		modality_id int,
		category_id int,
		validate_study_count nchar(1),
		is_active nchar(1),
		changed nchar(1) null default 'N',
		action nvarchar(1) null default '',
	)
	
	set @strSQL='insert into #tmp(id,name,modality_id,category_id,validate_study_count,is_active)'
	set @strSQL= @strSQL + '(select id,name,modality_id,category_id,validate_study_count,is_active '
    set @strSQL= @strSQL + 'from modality_study_types '
	set @strSQL= @strSQL + 'where 1 = 1 ' 
	
 
	if (isnull(@name,'') <>'')
		 begin
			set @strSQL=@strSQL+' and upper(name) like ''%'+upper(@name)+'%'' '
		 end
	if (isnull(@modality_id,0) >0)
		begin
			set @strSQL=@strSQL+' and modality_id = ' + convert(varchar, @modality_id)
		 end
	if (isnull(@category_id,0) >0)
		begin
			set @strSQL=@strSQL+' and category_id = ' + convert(varchar, @category_id)
		 end
	if (isnull(@is_active,'X') <>'X')
		begin
			set @strSQL=@strSQL+' and is_active = '''+ @is_active + ''' '
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
