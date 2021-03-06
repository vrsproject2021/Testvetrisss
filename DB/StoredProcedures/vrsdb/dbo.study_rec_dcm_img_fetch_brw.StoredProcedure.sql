USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[study_rec_dcm_img_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[study_rec_dcm_img_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[study_rec_dcm_img_fetch_brw]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : study_rec_dcm_img_fetch_brw :  fetch studies
				  downloaded with dcm image
** Created By   : Pavel Guha
** Created On   : 05/08/2019
*******************************************************/
-- exec study_rec_dcm_img_fetch_brw '','','00000000-0000-0000-0000-000000000000','N','01Jan1900','01Jan1900','','A','11111111-1111-1111-1111-111111111111'
create PROCEDURE [dbo].[study_rec_dcm_img_fetch_brw] 
	@patient_id nvarchar(200) ='',
    @patient_name nvarchar(200) ='',
	@institution_id uniqueidentifier = '00000000-0000-0000-0000-000000000000',
	@consider_study_date nchar(1)='N',
	@study_date_from datetime='01Jan1900',
	@study_date_till datetime='01Jan1900',
	@study_uid nvarchar(100) ='', 
	@approved nchar(1)='A',
    @user_id uniqueidentifier
as
begin
	set nocount on
	declare @strSQL varchar(max),
	        @user_role_id int,
	        @user_role_code nvarchar(10)

	delete from sys_record_lock where user_id = @user_id
	delete from sys_record_lock_ui where user_id = @user_id

	create table #tmpInst
	(
	   id uniqueidentifier
	)
	create table #tmp
	(
		id uniqueidentifier,
		date_downloaded datetime,
		study_uid nvarchar(100),
		study_date datetime,
		institution_id uniqueidentifier,
		institution_code nvarchar(5),
		institution_name nvarchar(100),
		patient_id nvarchar(20),
		patient_fname nvarchar(80),
		patient_lname nvarchar(80),
		file_count int,
		file_xfer_count int,
		approve_for_pacs nchar(1)
	)


	select @user_role_id = u.user_role_id,
	       @user_role_code = ur.code
	from users u
	inner join user_roles ur on ur.id =u.user_role_id
	where u.id = @user_id

	if(@user_role_code = 'SUPP' or @user_role_code = 'SYSADMIN')
		begin
		    insert into #tmpInst(id)
			(select id from institutions where is_active='Y')
			 order by name
		end
	else if(@user_role_code = 'IU')
		begin
			insert into #tmpInst(id)
			(select id
			from institutions 
			where is_active='Y'
			and id in (select institution_id
			           from institution_user_link
					   where user_id = @user_id))
			order by name
		end
	else if(@user_role_code = 'SALES')
		begin
			insert into #tmpInst(id)
			(select id
			from institutions 
			where is_active='Y'
			and id in (select institution_id
			           from institution_salesperson_link
					   where salesperson_user_id = @user_id))
			order by name
		end

	set @strSQL='insert into #tmp(id,date_downloaded,study_uid,study_date,'
	set @strSQL= @strSQL + 'institution_id,institution_code,institution_name,'
	set @strSQL= @strSQL + 'patient_id,patient_fname,patient_lname,'
	set @strSQL= @strSQL + 'file_count,file_xfer_count,approve_for_pacs)'
	set @strSQL= @strSQL + '(select id,date_downloaded,study_uid,study_date=isnull(study_date,''01jan1900''),'
	set @strSQL= @strSQL + 'institution_id,institution_code,institution_name,'
	set @strSQL= @strSQL + 'patient_id= isnull(patient_id,''''),'
	set @strSQL=  @strSQL + 'case when patient_fname like ''%[^a-zA-Z0-9.&( )_-]%'' then '''' else isnull(patient_fname,'''') end patient_fname,'
	set @strSQL=  @strSQL + 'case when patient_lname like ''%[^a-zA-Z0-9.&( )_-]%'' then '''' else isnull(patient_lname,'''') end patient_lname,'
	set @strSQL= @strSQL + 'file_count,file_xfer_count,approve_for_pacs '
    set @strSQL= @strSQL + 'from scheduler_file_downloads '
	set @strSQL= @strSQL + 'where file_count > file_xfer_count ' 
 
	
 
    if(isnull(@study_uid,'') <>'')
		begin
			set @strSQL= @strSQL + ' and upper(hdr.study_uid) like ''%'+upper(@study_uid)+'%'' ' 
		end
	if(isnull(@patient_id,'') <>'')
		begin
			set @strSQL= @strSQL + ' and upper(isnull(patient_id,'''') like ''%'+upper(@patient_id)+'%'' ' 
		end
	if(isnull(@patient_name,'') <>'')
		begin
			set @strSQL= @strSQL + ' and upper(isnull(patient_fname,'''')  + '' '' +  upper(isnull(patient_lname,''''))) like ''%'+upper(@patient_name)+'%'' ' 
		end
	if (isnull(@institution_id,'00000000-0000-0000-0000-000000000000') <>'00000000-0000-0000-0000-000000000000')
		begin
			set @strSQL=@strSQL+' and institution_id = '''+ convert(varchar(36),@institution_id) + ''' '
		end
	else
		begin
			set @strSQL=@strSQL+' and institution_id in (select id from #tmpInst) '
		end
	if (isnull(@consider_study_date,'N') ='Y')
		begin
			set @strSQL=@strSQL+'and study_date between '''+ convert(varchar(11),@study_date_from,106) + ''' and ''' + convert(varchar(11),@study_date_till,106) + ''' '
		 end
	if (isnull(@approved,'A') <>'A')
		begin
			set @strSQL=@strSQL+'and approve_for_pacs = '''+ @approved + ''' '
		 end

	set @strSQL= @strSQL + ') '
    set @strSQL= @strSQL + 'order by date_downloaded desc,study_uid,institution_name,study_date,patient_fname,patient_lname'
	--print @strSQL
	exec(@strSQL)

	select * from #tmp

	drop table #tmpInst
	drop table #tmp
	set nocount off
end

GO
