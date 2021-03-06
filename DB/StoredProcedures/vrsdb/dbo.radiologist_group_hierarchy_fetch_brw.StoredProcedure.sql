USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_group_hierarchy_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[radiologist_group_hierarchy_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_group_hierarchy_fetch_brw]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : radiologist_group_hierarchy_fetch_brw :  fetch cpromotion
                  names
** Created By   : Pavel Guha
** Created On   : 13/01/2021
*******************************************************/
-- exec radiologist_group_hierarchy_fetch_brw '',0,'11111111-1111-1111-1111-111111111111',34,'',0
create PROCEDURE [dbo].[radiologist_group_hierarchy_fetch_brw] 
    @name nvarchar(50)='',
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
		id int,
		name nvarchar(50),
		display_order int,
		changed nchar(1) null default 'N'
	)
	
	set @strSQL='insert into #tmp(id,name,display_order)'
	set @strSQL= @strSQL + '(select id,name,display_order '
    set @strSQL= @strSQL + 'from sys_radiologist_group '
	set @strSQL= @strSQL + 'where 1 = 1' 
	
 
	
	if (isnull(@name,'') <>'')
		 begin
			set @strSQL=@strSQL+' and upper(name) like ''%'+upper(@name)+'%'' '
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
