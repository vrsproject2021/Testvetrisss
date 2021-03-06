USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[study_rec_img_fetch_files]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[study_rec_img_fetch_files]
GO
/****** Object:  StoredProcedure [dbo].[study_rec_img_fetch_files]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : study_rec_img_fetch_files : fetch dcm file list
** Created By   : Pavel Guha
** Created On   : 21/08/2019
*******************************************************/
--exec study_rec_img_fetch_files '00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000'
CREATE procedure [dbo].[study_rec_img_fetch_files]
    @id uniqueidentifier ='00000000-0000-0000-0000-000000000000',
	@institution_id uniqueidentifier ='00000000-0000-0000-0000-000000000000',
	@user_id uniqueidentifier	
as
begin
	 set nocount on
	 declare @user_role_id int,
	         @user_role_code nvarchar(10),
			 @billing_account_id uniqueidentifier

	select @user_role_id = u.user_role_id,
		   @user_role_code = ur.code
	from users u
	inner join user_roles ur on ur.id =u.user_role_id
	where u.id = @user_id

	create table #tmp
	(
		id uniqueidentifier,
		file_name nvarchar(250),
		received_on datetime,
		institution_id uniqueidentifier,
		instititution_name nvarchar(100),
		patient_country_id int null default 0,
		patient_state_id int null default 0,
		patient_city nvarchar(100) null default '',
		sel nchar(1),
		del nchar(1) null default ''
	)
	create table #tmpFinal
	(
		srl_no int identity(1,1),
		id uniqueidentifier,
		file_name nvarchar(250),
		received_on datetime,
		institution_id uniqueidentifier,
		instititution_name nvarchar(100),
		patient_country_id int null default 0,
		patient_state_id int null default 0,
		patient_city nvarchar(100) null default '',
		sel nchar(1),
		del nchar(1) null default ''
	)	  
	create table #tmpInst
	(
		institution_id uniqueidentifier
	)

	if(@id ='00000000-0000-0000-0000-000000000000')
		begin
			if(@institution_id ='00000000-0000-0000-0000-000000000000')
				begin
					if(@user_role_code = 'SUPP' or @user_role_code = 'SYSADMIN' or @user_role_code = 'RDL')
						begin
							insert into #tmpInst(institution_id)
							(select id from institutions where is_active='Y')
						end
					else if(@user_role_code = 'IU')
						begin
							select @institution_id = institution_id from institution_user_link where user_id = @user_id 
							insert into #tmpInst(institution_id)
							(select id from institutions where is_active='Y' and id = @institution_id)
						end
					else if(@user_role_code = 'AU')
						begin
							select @billing_account_id = id from billing_account where login_user_id = @user_id 
							insert into #tmpInst(institution_id)
							(select id from institutions where is_active='Y' and billing_account_id = @billing_account_id)
						end
					else if(@user_role_code = 'SALES')
						begin
							insert into #tmpInst(institution_id)
							(select id from institutions where is_active='Y' and id in (select institution_id from institution_salesperson_link where salesperson_user_id = @user_id))
						end

					insert into #tmp(id,file_name,received_on,institution_id,instititution_name,patient_country_id,patient_state_id,patient_city,sel)
					(select f.id,f.file_name,f.date_downloaded,f.institution_id,i.name,i.country_id,i.state_id,i.city,'N'
					from scheduler_img_file_downloads_ungrouped f
					inner join institutions i on i.id = f.institution_id
					where f.grouped = 'N'
					and f.is_stored='N'
					and institution_id in (select institution_id from #tmpInst))
					order by f.file_name
				end
			else
				begin
					insert into #tmp(id,file_name,received_on,institution_id,instititution_name,patient_country_id,patient_state_id,patient_city,sel)
					(select f.id,f.file_name,f.date_downloaded,f.institution_id,i.name,i.country_id,i.state_id,i.city,'N'
					from scheduler_img_file_downloads_ungrouped f
					inner join institutions i on i.id = f.institution_id
					where f.grouped = 'N'
					and f.is_stored='N'
					and f.institution_id = @institution_id)
					order by f.file_name
				end
		end
	else
		begin
			insert into #tmp(id,file_name,received_on,institution_id,instititution_name,patient_country_id,patient_state_id,patient_city,sel)
			(select fd.ungrouped_id,fd.file_name,fdu.date_downloaded,fh.institution_id,i.name,i.country_id,i.state_id,i.city,'Y'
			from scheduler_img_file_downloads_grouped_dtls fd
			inner join scheduler_img_file_downloads_grouped fh on fh.id=fd.id
			inner join scheduler_img_file_downloads_ungrouped fdu on fdu.grouped_id= fd.id
			inner join institutions i on i.id = fh.institution_id
			where fd.id = @id
			union
			select f.id,f.file_name,f.date_downloaded,f.institution_id,i.name,i.country_id,i.state_id,i.city,'N'
			from scheduler_img_file_downloads_ungrouped f
			inner join institutions i on i.id = f.institution_id
			where f.grouped = 'N'
			and f.is_stored='N'
			and f.institution_id = @institution_id)
			order by file_name
		end

	insert into  #tmpFinal(id,file_name,received_on,institution_id,instititution_name,patient_country_id,patient_state_id,patient_city,sel)
	(select id,file_name,received_on,institution_id,instititution_name,patient_country_id,patient_state_id,patient_city,sel from #tmp)
	order by sel desc,received_on desc,file_name
	

	select * from #tmpFinal order by srl_no

	drop table #tmp
	drop table #tmpFinal
	drop table #tmpInst
		
	set nocount off
end

GO
