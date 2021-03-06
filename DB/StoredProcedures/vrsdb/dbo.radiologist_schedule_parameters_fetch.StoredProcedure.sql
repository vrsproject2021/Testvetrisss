USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_schedule_parameters_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[radiologist_schedule_parameters_fetch]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_schedule_parameters_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : radiologist_schedule_parameters_fetch : 
                  getch radiologist schedule parameters
** Created By   : Pavel Guha
** Created On   : 16/09/2019
*******************************************************/
--exec radiologist_schedule_radiologists_fetch
CREATE procedure [dbo].[radiologist_schedule_parameters_fetch]
	@user_id uniqueidentifier
as
begin
	
	set nocount on

	declare @user_role_id int,
	        @user_role_code nvarchar(5),
			@radiologist_id uniqueidentifier,
			@schedule_view nchar(1)

	create table #tmp
	(
		srl_no int identity(1,1),
		id uniqueidentifier,
		name nvarchar(250),
		sel nvarchar(1) null default 'N'
	)

	insert into #tmp(id,name)
	(select id,name 
	from radiologists 
	where is_active='Y')
	order by name


	select @user_role_id = user_role_id
	from users
	where id =@user_id

	select @user_role_code = code
	from user_roles
	where id=@user_role_id

	if(@user_role_code='RDL')
		begin
			select @radiologist_id = id,
			       @schedule_view  = schedule_view
			from radiologists
			where login_user_id = @user_id
		end
	else
		begin
			select @radiologist_id = '00000000-0000-0000-0000-000000000000',
			       @schedule_view  = 'A'
		end


	

	select * from #tmp 
	select radiologist_id = @radiologist_id,
	       schedule_view  = @schedule_view,
		   user_role_code = @user_role_code,
		   RADCALSTARTTIME = (select data_type_string from general_settings where control_code ='RADCALSTARTTIME')


	drop table #tmp

	set nocount off


end


GO
