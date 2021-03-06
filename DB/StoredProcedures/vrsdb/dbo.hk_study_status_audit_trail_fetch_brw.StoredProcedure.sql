USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[hk_study_status_audit_trail_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[hk_study_status_audit_trail_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[hk_study_status_audit_trail_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : hk_study_status_audit_trail_fetch_brw :  fetch case(s)
** Created By   : Pavel Guha
** Created On   : 28/05/2019
*******************************************************/
-- exec hk_study_status_audit_trail_fetch_brw '','',0,'00000000-0000-0000-0000-000000000000','N','01Jan1900','01Jan1900',-1,'11111111-1111-1111-1111-111111111111'
-- exec hk_study_status_audit_trail_fetch_brw '','',0,'00000000-0000-0000-0000-000000000000','Y','02Jul2020','02Jul2020',-1,'11111111-1111-1111-1111-111111111111'
CREATE PROCEDURE [dbo].[hk_study_status_audit_trail_fetch_brw]
	@study_uid nvarchar(100) ='', 
    @patient_name nvarchar(100) ='',
    @modality_id int =0,
	@institution_id uniqueidentifier = '00000000-0000-0000-0000-000000000000',
	@consider_received_date nchar(1)='N',
	@received_date_from datetime='01Jan1900',
	@received_date_till datetime='01Jan1900',
	@status_id int =-1,
    @user_id uniqueidentifier
as
begin
	set nocount on
	declare @strSQL varchar(max),
			@strSQL1 varchar(max),
			@strSQL2 varchar(max),
	        @user_role_id int,
	        @user_role_code nvarchar(10),
			@APIVER nvarchar(200),
			@PACIMGVWRURL nvarchar(200)

	delete from sys_record_lock where user_id = @user_id
	delete from sys_record_lock_ui where user_id = @user_id

	create table #tmpInst
	(
	   id uniqueidentifier
	)

	select @APIVER = data_type_string
	from general_settings
	where control_code ='APIVER'

	if(@APIVER ='7.2')
		begin
			select @PACIMGVWRURL = data_type_string
			from general_settings
			where control_code ='PACIMGVWRURL'
		end
	else if(@APIVER ='8')
		begin
			select @PACIMGVWRURL = data_type_string
			from general_settings
			where control_code ='WS8IMGVWRURL'
		end

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
			(select id
			from institutions 
			where is_active='Y'
			and id in (select bail.institution_id
			           from billing_account_institution_link  bail
					   inner join billing_account ba on ba.id=bail.institution_id
					   where ba.login_user_id = @user_id))
			order by name
		end

	set @strSQL1= 'select hdr.id,hdr.study_uid,received_date=hdr.synched_on,'
	set @strSQL1= @strSQL1 + 'case when hdr.patient_name like ''%[^a-zA-Z0-9.&( )_-]%'' then '''' else dbo.InitCap(isnull(hdr.patient_fname,'''') + '' '' +  isnull(hdr.patient_lname,'''')) end patient_name,'
	set @strSQL1= @strSQL1 + 'modality_name=dbo.initcap(isnull(m.name,'''')),institution_name= dbo.initcap(isnull(ins.name,'''')), '
    set @strSQL1= @strSQL1 + 'hdr.study_status_pacs,ss.status_desc,hdr.date_updated,hdr.status_last_updated_on,'
	set @strSQL1= @strSQL1 + 'PACLOGINURL=replace((select data_type_string from general_settings where control_code=''PACLOGINURL''),''#V1'',hdr.study_uid),'
	set @strSQL1= @strSQL1 + 'PACSRPTVWRURL=replace((select data_type_string from general_settings where control_code=''PACSRPTVWRURL''),''#V1'',hdr.accession_no), '
	set @strSQL1= @strSQL1 + 'PACIMGVWRURL=''' + @PACIMGVWRURL + ''', '
	set @strSQL1= @strSQL1 + 'custom_report=isnull(ins.custom_report,''N''), '
	set @strSQL1= @strSQL1 + 'accession_no=isnull(hdr.accession_no,''''), '
	set @strSQL1= @strSQL1 + 'patient_id=isnull(hdr.patient_id,''''), '
	set @strSQL1= @strSQL1 + 'rpt_fmt=1 '
	set @strSQL1= @strSQL1 + 'from study_hdr hdr '
	set @strSQL1= @strSQL1 + 'inner join sys_study_status_pacs ss on ss.status_id =  hdr.study_status_pacs '
	set @strSQL1= @strSQL1 + 'left outer join modality m on m.id= hdr.modality_id '
	set @strSQL1= @strSQL1 + 'left outer join institutions ins on ins.id= hdr.institution_id '
	set @strSQL1= @strSQL1 + 'where  1 = 1' 
	
	if(isnull(@study_uid,'') <>'')
		begin
			set @strSQL1= @strSQL1 + ' and upper(hdr.study_uid) = '''+upper(@study_uid)+''' ' 
		end
	if(isnull(@patient_name,'') <>'')
		begin
			set @strSQL1= @strSQL1 + ' and upper(isnull(hdr.patient_fname,'''') + + '' '' +  upper(isnull(hdr.patient_lname,''''))) like ''%'+upper(@patient_name)+'%'' '
		end
	if (isnull(@modality_id,0) <>0)
		 begin
			set @strSQL1=@strSQL1+' and hdr.modality_id = '+ convert(varchar,@modality_id)
		 end
	if (isnull(@institution_id,'00000000-0000-0000-0000-000000000000') <>'00000000-0000-0000-0000-000000000000')
		begin
			set @strSQL1=@strSQL1+' and hdr.institution_id = '''+ convert(varchar(36),@institution_id) + ''' '
		end
	else
		begin
			set @strSQL1=@strSQL1+' and institution_id in (select id from #tmpInst) '
		end
	if (isnull(@consider_received_date,'N') ='Y')
		begin
			set @strSQL1=@strSQL1+'and hdr.synched_on between '''+ convert(varchar(11),@received_date_from,106) + ''' and ''' + convert(varchar(11),@received_date_till,106) + ''' '
		 end
	if (isnull(@status_id,-1) <>-1)
		 begin
			set @strSQL1=@strSQL1+' and hdr.study_status_pacs = '+ convert(varchar,@status_id)
		 end

    
	--print @strSQL1

	set @strSQL2= 'select hdr.id,hdr.study_uid,received_date=hdr.synched_on,'
	set @strSQL2= @strSQL2 + 'case when hdr.patient_name like ''%[^a-zA-Z0-9.&( )_-]%'' then '''' else dbo.InitCap(isnull(hdr.patient_fname,'''') + '' '' +  isnull(hdr.patient_lname,'''')) end patient_name,'
	set @strSQL2= @strSQL2 + 'modality_name=dbo.initcap(isnull(m.name,'''')),institution_name= dbo.initcap(isnull(ins.name,'''')), '
    set @strSQL2= @strSQL2 + 'hdr.study_status_pacs,ss.status_desc,hdr.date_updated,hdr.status_last_updated_on,'
	set @strSQL2= @strSQL2 + 'PACLOGINURL=replace((select data_type_string from general_settings where control_code=''PACLOGINURL''),''#V1'',hdr.study_uid),'
	set @strSQL2= @strSQL2 + 'PACSRPTVWRURL=replace((select data_type_string from general_settings where control_code=''PACSRPTVWRURL''),''#V1'',hdr.accession_no), '
	set @strSQL2= @strSQL2 + 'PACIMGVWRURL=''' + @PACIMGVWRURL + ''', '
	set @strSQL2= @strSQL2 + 'custom_report=isnull(ins.custom_report,''N''), '
	set @strSQL2= @strSQL2 + 'accession_no=isnull(hdr.accession_no,''''), '
	set @strSQL2= @strSQL2 + 'patient_id=isnull(hdr.patient_id,''''), '
	set @strSQL2= @strSQL2 + 'rpt_fmt=1 '
	set @strSQL2= @strSQL2 + 'from study_hdr_archive hdr '
	set @strSQL2= @strSQL2 + 'inner join sys_study_status_pacs ss on ss.status_id =  hdr.study_status_pacs '
	set @strSQL2= @strSQL2 + 'left outer join modality m on m.id= hdr.modality_id '
	set @strSQL2= @strSQL2 + 'left outer join institutions ins on ins.id= hdr.institution_id '
	set @strSQL2= @strSQL2 + 'where  1 = 1' 
	
	if(isnull(@study_uid,'') <>'')
		begin
			set @strSQL2= @strSQL2 + ' and upper(hdr.study_uid) = '''+upper(@study_uid)+''' ' 
		end
	if(isnull(@patient_name,'') <>'')
		begin
			set @strSQL2= @strSQL2 + ' and upper(isnull(hdr.patient_fname,'''') + + '' '' +  upper(isnull(hdr.patient_lname,''''))) like ''%'+upper(@patient_name)+'%'' '
		end
	if (isnull(@modality_id,0) <>0)
		 begin
			set @strSQL2=@strSQL2+' and hdr.modality_id = '+ convert(varchar,@modality_id)
		 end
	if (isnull(@institution_id,'00000000-0000-0000-0000-000000000000') <>'00000000-0000-0000-0000-000000000000')
		begin
			set @strSQL2=@strSQL2+' and hdr.institution_id = '''+ convert(varchar(36),@institution_id) + ''' '
		end
	else
		begin
			set @strSQL2=@strSQL2+' and institution_id in (select id from #tmpInst) '
		end
	if (isnull(@consider_received_date,'N') ='Y')
		begin
			set @strSQL2=@strSQL2+'and hdr.synched_on between '''+ convert(varchar(11),@received_date_from,106) + ''' and ''' + convert(varchar(11),@received_date_till,106) + ''' '
		 end
	if (isnull(@status_id,-1) <>-1)
		 begin
			set @strSQL2=@strSQL2+' and hdr.study_status_pacs = '+ convert(varchar,@status_id)
		 end

	set @strSQL = '(' + @strSQL1 + ' union ' + @strSQL2 + ')  order by hdr.status_last_updated_on desc,received_date desc,hdr.study_uid,patient_name'
	--print @strSQL1
	--print @strSQL1
    print @strSQL
	exec(@strSQL)

	drop table #tmpInst
	set nocount off
end

GO
