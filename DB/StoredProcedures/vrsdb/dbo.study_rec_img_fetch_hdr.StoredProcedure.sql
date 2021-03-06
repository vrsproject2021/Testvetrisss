USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[study_rec_img_fetch_hdr]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[study_rec_img_fetch_hdr]
GO
/****** Object:  StoredProcedure [dbo].[study_rec_img_fetch_hdr]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : study_rec_img_fetch_hdr : fetch case list header
** Created By   : Pavel Guha
** Created On   : 20/08/2019
*******************************************************/
--exec study_rec_img_fetch_hdr '00000000-0000-0000-0000-000000000000',37,'A54692E6-A88A-4A78-B9DD-FBF10D4AEC4F','',0
--exec study_rec_img_fetch_hdr '00000000-0000-0000-0000-000000000000',37,'11111111-1111-1111-1111-111111111111','f2a05587-8e14-4a5e-a581-03793647ab66','',0
CREATE procedure [dbo].[study_rec_img_fetch_hdr]
    @id uniqueidentifier,
    @menu_id int,
    @user_id uniqueidentifier,
	@session_id uniqueidentifier = '00000000-0000-0000-0000-000000000000',
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	 set nocount on
	 set datefirst 1

	  declare @user_role_id int,
	          @user_role_code nvarchar(10),
			  @institution_id uniqueidentifier,
			  @institution_consult_applicable nchar(1),
			  @billing_account_id uniqueidentifier,
			  @day_no int,
			  @start_from datetime,
			  @end_at datetime,
			  @curr_date_time datetime,
			  @beyond_operation_time nchar(1)

	  select @user_role_id = u.user_role_id,
			 @user_role_code = ur.code
	  from users u
	  inner join user_roles ur on ur.id =u.user_role_id
	  where u.id = @user_id

	  select @day_no = datepart(dw,getdate())
	  select  @start_from   = convert(varchar(11),getdate(),106) + ' ' + from_time,
			  @end_at       = convert(varchar(11),getdate(),106) + ' ' + till_time
	  from settings_operation_time
	  where day_no = @day_no

	select @curr_date_time = getdate()

	if((@curr_date_time not between @start_from and @end_at))
		set @beyond_operation_time='Y'
	else
		set @beyond_operation_time='N'

	select hdr.id,hdr.study_uid,hdr.study_date,
	       patient_id    = isnull(hdr.patient_id,''),
		   patient_fname = isnull(hdr.patient_fname,''),
		   patient_lname = isnull(hdr.patient_lname,''),
		   hdr.modality_id,
		   hdr.category_id,
		   institution_id = isnull(hdr.institution_id,'00000000-0000-0000-0000-000000000000'),
		   hdr.institution_code,
		   institution_name= isnull(ins.name,''),
		   hdr.file_count,
		   hdr.file_xfer_count,
		   series_instance_uid = isnull(hdr.series_instance_uid,''),
		   series_no= isnull(hdr.series_no,''),
		   hdr.approve_for_pacs
	from scheduler_img_file_downloads_grouped hdr
	left outer join institutions ins on ins.id= hdr.institution_id
	where hdr.id=@id

	
	if(@id<>'00000000-0000-0000-0000-000000000000')
		begin
			
				if(select count(record_id) from sys_record_lock_ui where record_id=@id and menu_id=@menu_id)=0
					begin
						exec common_lock_record_ui
							@menu_id       = @menu_id,
							@record_id     = @id,
							@user_id       = @user_id,
							@error_code    = @error_code output,
							@return_status = @return_status output	
						
						if(@return_status=0)
							begin
								return 0
							end
					end

					select @institution_id = institution_id
					from study_hdr 
					where id=@id

					select @institution_consult_applicable = consult_applicable 
					from institutions 
					where id = @institution_id
				
		end
    else
		begin
			if(select count(record_id) from sys_record_lock_ui where user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id)>0
			    begin
				  delete from sys_record_lock_ui where user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id
				  delete from sys_record_lock where user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id
			    end

			set @institution_id= '00000000-0000-0000-0000-000000000000'
			set @institution_consult_applicable='N'
		end

    --institutions
	if(@user_role_code = 'SUPP' or @user_role_code = 'SYSADMIN' or @user_role_code = 'RDL')
		begin
			select id,name from institutions where is_active='Y' order by name
		end
	else if(@user_role_code = 'IU')
		begin
			select @institution_id = institution_id from institution_user_link where user_id = @user_id 
			--print @institution_id
			select id,name from institutions where is_active='Y' and id = @institution_id order by name
		end
	else if(@user_role_code = 'AU')
		begin
			select @billing_account_id = id from billing_account where login_user_id = @user_id 
			--print @institution_id
			select id,name from institutions where is_active='Y' and billing_account_id = @billing_account_id order by name
		end
	else if(@user_role_code = 'SALES')
		begin
			select id,name from institutions where is_active='Y' and id in (select institution_id from institution_salesperson_link where salesperson_user_id = @user_id) order by name
		end
	--modalitity
	select id,name from modality where is_active='Y' order by name
	--ftp dowload temp folder path
	select FTPDLFLDRTMP = data_type_string from general_settings where control_code='FTPDLFLDRTMP'
	--species
	select id,name from species where is_active='Y' order by name
	--patient_id
	select id,code,name,patient_id_srl from institutions where id=@institution_id
	--physicians
	select id,name= rtrim(ltrim(isnull(lname,'') + ' ' + isnull(fname,'') + ' ' + isnull(credentials,''))) 
	from physicians where is_active='Y' 
	and id in (select physician_id from institution_physician_link where institution_id=@institution_id) 
	order by lname
	--priority
	select priority_id,priority_desc,is_stat from sys_priority where is_active='Y' order by priority_desc
	--consult applicable
	select institution_consult_applicable = isnull(@institution_consult_applicable,'N')
	--category
	select id,name,is_default from sys_study_category order by is_default desc,name

	--country
	select id,name from sys_country  order by name

	--state
	select id,name from sys_states where country_id=0

	--beyond_operation_time
	select  beyond_operation_time=@beyond_operation_time

	--Modality Service Availability
	select sma.service_id,sma.modality_id,s.priority_id,sma.available 
	from settings_service_modality_available sma
	inner join services s on s.id = sma.service_id
	inner join sys_priority p on p.priority_id = s.priority_id
	and p.is_stat='Y'

   --Modality Service Availability (After Hours)
	select sma.service_id,sma.modality_id,s.priority_id,sma.available 
	from settings_service_modality_available_after_hours sma
	inner join services s on s.id = sma.service_id
	inner join sys_priority p on p.priority_id = s.priority_id
	and p.is_stat='Y'

	--Modality Service Exception Institution (Normal Hours)
	select smei.service_id,smei.modality_id,s.priority_id,smei.institution_id 
	from settings_service_modality_available_exception_institution smei
	inner join services s on s.id = smei.service_id
	where smei.after_hours='N'

	--Modality Service Exception Institution (After Hours)
	select smei.service_id,smei.modality_id,s.priority_id,smei.institution_id 
	from settings_service_modality_available_exception_institution smei
	inner join services s on s.id = smei.service_id
	where smei.after_hours='Y'

	--Species Service Availability (Normal Hours)
	select ssa.service_id,ssa.species_id,s.priority_id,ssa.available 
	from settings_service_species_available ssa
	inner join services s on s.id = ssa.service_id
	inner join sys_priority p on p.priority_id = s.priority_id
	and p.is_stat='Y'

	--Species Service Availability (After Hours)
	select ssa.service_id,ssa.species_id,s.priority_id,ssa.available 
	from settings_service_species_available_after_hours ssa
	inner join services s on s.id = ssa.service_id
	inner join sys_priority p on p.priority_id = s.priority_id
	and p.is_stat='Y'

	--Species Service Exception Institution (Normal Hours)
	select ssei.service_id,ssei.species_id,s.priority_id,ssei.institution_id 
	from settings_service_species_available_exception_institution ssei
	inner join services s on s.id = ssei.service_id
	where ssei.after_hours='N'

	--Species Service Exception Institution (After Hours)
	select ssei.service_id,ssei.species_id,s.priority_id,ssei.institution_id 
	from settings_service_species_available_exception_institution ssei
	inner join services s on s.id = ssei.service_id
	where ssei.after_hours='Y'
		
	set nocount off
end

GO
