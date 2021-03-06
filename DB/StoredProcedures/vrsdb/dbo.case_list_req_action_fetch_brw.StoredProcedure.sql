USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_req_action_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_req_action_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[case_list_req_action_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_req_action_fetch_brw :  fetch case(s)
				  requiring action
** Created By   : Pavel Guha
** Created On   : 10/04/2019
*******************************************************/
-- exec case_list_req_action_fetch_brw '','','00000000-0000-0000-0000-000000000000','N','01Jan1900','01Jan1900','','11111111-1111-1111-1111-111111111111'
-- exec case_list_req_action_fetch_brw '','','00000000-0000-0000-0000-000000000000','N','01Jan1900','01Jan1900','dab99512-dbb7-4adf-9589-59eb764964fc'
CREATE PROCEDURE [dbo].[case_list_req_action_fetch_brw] 
    @patient_name nvarchar(100) ='',
    @modality nvarchar(50),
	@institution_id uniqueidentifier = '00000000-0000-0000-0000-000000000000',
	@consider_received_date nchar(1)='N',
	@received_date_from datetime='01Jan1900',
	@received_date_till datetime='01Jan1900',
	@study_uid nvarchar(100) ='', 
    @user_id uniqueidentifier,
	@menu_id int,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000'
as
begin
	set nocount on
	declare @strSQL varchar(max),
	        @user_role_id int,
	        @user_role_code nvarchar(10)
	declare @APIVER nvarchar(200),
			@PACIMGVWRURL nvarchar(200),
			@record_id uniqueidentifier,
			@suid nvarchar(100),
		    @menu_text nvarchar(100),
		    @activity_text nvarchar(max),
			@error_code nvarchar(10),
		    @return_status int


	select @menu_text= menu_desc from sys_menu where menu_id=@menu_id

	select @user_role_id = u.user_role_id,
	       @user_role_code = ur.code
	from users u
	inner join user_roles ur on ur.id =u.user_role_id
	where u.id = @user_id
	

	if(select count(record_id) from sys_record_lock_ui where menu_id =@menu_id and user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id)>0
		begin
			select @record_id =record_id from sys_record_lock_ui where menu_id =@menu_id and user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id
			select @suid = study_uid from study_hdr where id= @record_id

			 set @activity_text =  'Lock Released'
			 set @error_code=''
			 set @return_status=0

			 exec common_study_user_activity_trail_save
				@study_hdr_id  = @record_id,
				@study_uid     = @suid,
				@menu_id       = @menu_id,
				@activity_text = @activity_text,
				@session_id    = @session_id,
				@activity_by   = @user_id,
				@error_code    = @error_code output,
				@return_status = @return_status output

			set @activity_text =  @menu_text  + '==> Lock released => Study UID ' + @suid
			set @error_code=''
			set @return_status=0	
				
			exec common_user_activity_log
				@user_id       = @user_id,
				@activity_text = @activity_text,
				@menu_id       = @menu_id,
				@session_id    = @session_id,
				@error_code    = @error_code output,
				@return_status = @return_status output
		end

	delete from sys_record_lock where user_id = @user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id
	delete from sys_record_lock_ui where user_id = @user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id

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

	

	if(@user_role_code = 'SUPP' or @user_role_code = 'SYSADMIN' or @user_role_code='RDL')
		begin
		    insert into #tmpInst(id)
			(select id from institutions where is_active='Y'
			union
		    select id='00000000-0000-0000-0000-000000000000')
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

	set @strSQL= 'select h.id,h.study_uid,received_date=h.synched_on,'
	set @strSQL=  @strSQL + 'time_left  = isnull(replicate(''0'',2- len(convert(varchar,datediff(mi,getdate(),h.finishing_datetime)/60)))  + convert(varchar,datediff(mi,getdate(),h.finishing_datetime)/60) + '':''+ replicate(''0'',2- len(convert(varchar,datediff(mi,getdate(),h.finishing_datetime)%60)))  + convert(varchar,datediff(mi,getdate(),h.finishing_datetime)%60),''00:00''),' 
	set @strSQL=  @strSQL + 'institution_name_pacs = isnull(i.name,h.institution_name_pacs),'
	set @strSQL=  @strSQL + 'case when h.patient_name like ''%[^a-zA-Z0-9.( )_-]%'' then '''' else isnull(h.patient_fname,'''') + '' '' +  isnull(h.patient_lname,'''') end patient_name,'
	--set @strSQL=  @strSQL + 'patient_sex=isnull(h.patient_sex,''''),'
	set @strSQL=  @strSQL + 'modality_pacs=isnull(h.modality_pacs,''''),h.received_via_dicom_router,h.status_last_updated_on,promo='''',patient_id=isnull(h.patient_id_pacs,''''),accession_no=isnull(h.accession_no_pacs,''''),'
	set @strSQL= @strSQL + 'PACIMGVWRURL=''' + @PACIMGVWRURL + ''' '
    set @strSQL= @strSQL + 'from study_hdr h '
	set @strSQL= @strSQL + 'left outer join institutions i on i.id = h.institution_id '
	set @strSQL= @strSQL + 'where h.study_status = 1 ' 
	set @strSQL= @strSQL + 'and h.deleted = ''N'' ' 
	set @strSQL= @strSQL + 'and isnull(h.merge_status,''N'') = ''N'' ' 
	
 
    if(isnull(@study_uid,'') <>'')
		begin
			set @strSQL= @strSQL + ' and upper(h.study_uid) like ''%'+upper(@study_uid)+'%'' ' 
		end
	if(isnull(@patient_name,'') <>'')
		begin
			set @strSQL= @strSQL + ' and upper(isnull(h.patient_fname,'''') + '' '' +  upper(isnull(h.patient_lname,''''))) like ''%'+upper(@patient_name)+'%'' ' 
		end
	if (isnull(@modality,'') <>'')
		 begin
			set @strSQL=@strSQL+' and upper(h.modality_pacs) like ''%'+upper(@modality)+'%'' '
		 end
	if (isnull(@institution_id,'00000000-0000-0000-0000-000000000000') <>'00000000-0000-0000-0000-000000000000')
		begin
			set @strSQL=@strSQL+' and h.institution_id = '''+ convert(varchar(36),@institution_id) + ''' '
		end
	else
		begin
			set @strSQL=@strSQL+' and h.institution_id in (select id from #tmpInst) '
		end

	if (isnull(@consider_received_date,'N') ='Y')
		begin
			set @strSQL=@strSQL+'and h.received_date between '''+ convert(varchar(11),@received_date_from,106) + ''' and ''' + convert(varchar(11),@received_date_till,106) + ''' '
		 end

    set @strSQL= @strSQL + 'order by h.status_last_updated_on desc,h.study_uid,h.patient_name,h.modality_pacs,h.body_part_pacs'
	--print @strSQL
	exec(@strSQL)

	set @activity_text =  @menu_text  + '==> Study list loaded'
	set @error_code=''
	set @return_status=0

	exec common_user_activity_log
		@user_id       = @user_id,
		@activity_text = @activity_text,
		@menu_id       = @menu_id,
		@session_id    = @session_id,
		@error_code    = @error_code output,
		@return_status = @return_status output

	drop table #tmpInst
	set nocount off
end

GO
