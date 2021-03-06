USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_radiologist_worklist_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_radiologist_worklist_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[case_list_radiologist_worklist_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_radiologist_worklist_fetch_brw :  fetch case(s)
				  requiring action
** Created By   : Pavel Guha
** Created On   : 22/04/2019
*******************************************************/
-- exec case_list_radiologist_worklist_fetch_brw '',0,'00000000-0000-0000-0000-000000000000','N','01Jan1900','01Jan1900',-1,0,'11111111-1111-1111-1111-111111111111'
-- exec case_list_radiologist_worklist_fetch_brw '',0,0,'f1c7e42a-5640-48dc-9ad4-9a04f951fc35','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','','N','01Jan1900','01Jan1900',-1,'11111111-1111-1111-1111-111111111111'
create PROCEDURE [dbo].[case_list_radiologist_worklist_fetch_brw] 
    @patient_name nvarchar(100) ='',
    @modality_id int=0,
	@institution_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@consider_received_date nchar(1)='N',
	@received_date_from datetime='01Jan1900',
	@received_date_till datetime='01Jan1900',
	@status_id int = -1,
	@category_id int =0,
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

	select @user_role_id = u.user_role_id,
	       @user_role_code = ur.code
	from users u
	inner join user_roles ur on ur.id =u.user_role_id
	where u.id = @user_id

	if(@user_role_code = 'SUPP' or @user_role_code = 'SYSADMIN' or @user_role_code = 'RDL')
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
	else if(@user_role_code = 'AU')
		begin
			insert into #tmpInst(id)
			(select bail.institution_id
			 from billing_account_institution_link bail
			 inner join institutions i on i.id = bail.institution_id
			 inner join billing_account ba on ba.id = bail.billing_account_id
			 where i.is_active='Y'
			 and ba.login_user_id = @user_id)
			 order by i.name
		end

	set @strSQL= 'select hdr.id,hdr.study_uid,received_date=hdr.synched_on,hdr.status_last_updated_on,'
	set @strSQL=  @strSQL + 'time_left  = isnull(replicate(''0'',2- len(convert(varchar,datediff(mi,getdate(),hdr.finishing_datetime)/60)))  + convert(varchar,datediff(mi,getdate(),hdr.finishing_datetime)/60) + '':''+ replicate(''0'',2- len(convert(varchar,datediff(mi,getdate(),hdr.finishing_datetime)%60)))  + convert(varchar,datediff(mi,getdate(),hdr.finishing_datetime)%60),''00:00''),' 
	set @strSQL=  @strSQL + 'case when hdr.patient_name like ''%[^a-zA-Z0-9.&( )_-]%'' then '''' else dbo.InitCap(isnull(hdr.patient_fname,'''') + '' '' +  isnull(hdr.patient_lname,'''')) end patient_name,'
	set @strSQL= @strSQL + 'modality_name=dbo.initcap(isnull(m.name,'''')),category_name=dbo.initcap(isnull(c.name,'''')),hdr.priority_id,priority_desc= isnull(p.priority_desc,''''),'
	set @strSQL= @strSQL + 'institution_name= dbo.initcap(ins.name),physician_name = dbo.initcap(isnull(ph.name,'''')), '
	set @strSQL= @strSQL + 'stat.status_desc,'
	set @strSQL= @strSQL + 'hdr.status_last_updated_on, '
	set @strSQL= @strSQL + 'PACLOGINURL=replace((select data_type_string from general_settings where control_code=''PACLOGINURL''),''#V1'',hdr.study_uid), '
	set @strSQL= @strSQL + 'PACMAILRPTURL=(select data_type_string from general_settings where control_code=''PACMAILRPTURL'') + hdr.study_uid, '
	set @strSQL= @strSQL + 'PACIMGVWRURL=replace((select data_type_string from general_settings where control_code=''PACIMGVWRURL''),''#V1'',hdr.study_uid), '
	set @strSQL = @strSQL + 'edit_report='''' '
    set @strSQL= @strSQL + 'from study_hdr hdr '
	set @strSQL= @strSQL + 'left outer join modality m on m.id= hdr.modality_id '
	set @strSQL= @strSQL + 'left outer join sys_study_category c on c.id= hdr.category_id '
	set @strSQL= @strSQL + 'inner join institutions ins on ins.id= hdr.institution_id '
	set @strSQL= @strSQL + 'left outer join sys_priority p on p.priority_id= hdr.priority_id '
	set @strSQL= @strSQL + 'left outer join physicians ph on ph.id= hdr.physician_id '
	set @strSQL= @strSQL + 'inner join sys_study_status_pacs stat on stat.status_id= hdr.study_status_pacs '
	set @strSQL= @strSQL + 'where hdr.study_status_pacs in (50,60,80,100) ' 
	set @strSQL= @strSQL + 'and hdr.deleted = ''N''' 
	
 
	if(isnull(@patient_name,'') <>'')
		begin
			set @strSQL= @strSQL + ' and upper(isnull(hdr.patient_fname,'''') + + '' '' +  upper(isnull(hdr.patient_lname,''''))) like ''%'+upper(@patient_name)+'%'' ' 
		end
	if (isnull(@modality_id,0) <>0)
		 begin
			set @strSQL=@strSQL+' and hdr.modality_id = '+ convert(varchar,@modality_id)
		 end
    if (isnull(@category_id,0) <>0)
		 begin
			set @strSQL=@strSQL+' and hdr.category_id = '+ convert(varchar,@category_id)
		 end
	
	if (isnull(@institution_id,'00000000-0000-0000-0000-000000000000') <>'00000000-0000-0000-0000-000000000000')
		begin
			set @strSQL=@strSQL+' and hdr.institution_id = '''+ convert(varchar(36),@institution_id) + ''' '
		end
	else
		begin
			set @strSQL=@strSQL+' and hdr.institution_id in (select id from #tmpInst) '
		end
	if (isnull(@consider_received_date,'N') ='Y')
		begin
			set @strSQL=@strSQL+'and received_date between '''+ convert(varchar(11),@received_date_from,106) + ''' and ''' + convert(varchar(11),@received_date_till,106) + ''' '
		 end
	if (isnull(@status_id,-1) <>-1)
		 begin
			set @strSQL=@strSQL+' and hdr.study_status_pacs = '+ convert(varchar,@status_id)
		 end

    set @strSQL= @strSQL + ' order by hdr.status_last_updated_on desc,hdr.study_uid,hdr.patient_name,modality_name'
	--print @strSQL
	exec(@strSQL)
	drop table #tmpInst
	set nocount off
end


GO
