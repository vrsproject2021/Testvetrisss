USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_multiple_case_assign_radiologist_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[radiologist_multiple_case_assign_radiologist_fetch]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_multiple_case_assign_radiologist_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : radiologist_multiple_case_assign_radiologist_fetch :  fetch 
				  radiologist list for assignment of multiple cases
** Created By   : Pavel Guha
** Created On   : 03/10/2020
*******************************************************/

CREATE PROCEDURE [dbo].[radiologist_multiple_case_assign_radiologist_fetch] 
	@xml_study ntext,
	@type nvarchar(1)='',
	@filter nchar(1) ='A',
	@menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	set nocount on

	declare @hDoc int
	declare @assigned_prelim_radiologist_id uniqueidentifier,
	        @assigned_final_radiologist_id uniqueidentifier,
			@institution_id uniqueidentifier,
			@category_id int,
			@modality_id int,
			@species_id int,
			@status_id int,
			@rowcount int,
			@counter int

	exec sp_xml_preparedocument @hDoc output,@xml_study 

	create table #tmpRad
	(
		id uniqueidentifier,
		name nvarchar(250)
	)
	create table #tmpST
	(
		study_type_id uniqueidentifier
	)
	create table #tmp
	(
		rec_id int identity(1,1),
		institution_id uniqueidentifier,
		category_id int,
		modality_id int,
		species_id int,
		study_status_pacs int
	)

	insert into #tmp(institution_id,category_id,modality_id,species_id,study_status_pacs)
			  (select distinct institution_id,category_id,modality_id,species_id,study_status_pacs
			   from study_hdr
			   where id in (select id
							from openxml(@hDoc,'study/row', 2)  
							with(  id uniqueidentifier,
								   row_id int )))

	insert into #tmpST(study_type_id) 
	(select distinct study_type_id
	from study_hdr_study_types
	where study_hdr_id in (select id
						   from openxml(@hDoc,'study/row', 2)  
						   with(  id uniqueidentifier,
								   row_id int )))

	insert into #tmpRad(id,name) (select id,name from radiologists where is_active='Y') order by name

	
	
	set @counter = 1
	select  @rowcount=count(row_id)  
	from openxml(@hDoc,'study/row', 2)  
	with( row_id int )

	while(@counter <= @rowcount)
		begin
			select @institution_id = institution_id,
				   @category_id    = category_id,
				   @modality_id    = modality_id,
				   @species_id     = species_id,
				   @status_id      = study_status_pacs
			from #tmp
			where rec_id = @counter

			delete 
			from #tmpRad
			where id in (select radiologist_id from radiologist_functional_rights_exception_institution where institution_id=@institution_id)

			delete 
			from #tmpRad
			where id not in (select radiologist_id from radiologist_functional_rights_modality where modality_id=@modality_id)

			delete 
			from #tmpRad
			where id not in (select radiologist_id from radiologist_functional_rights_species where species_id=@species_id)

			if(@category_id=3)
				begin
					delete 
					from #tmpRad
					where id not in (select radiologist_id from radiologist_functional_rights_assigned where right_code='WRKCNSLTCASE')
				end

			delete 
			from #tmpRad
			where id in (select radiologist_id from radiologist_functional_rights_exception_study_type where study_type_id in (select study_type_id from #tmpST))

			if(@type='P')
				begin
					delete 
					from #tmpRad
					where id not in (select radiologist_id from radiologist_functional_rights_assigned where right_code in ('DICTRPT','UPDPRELIMRPT'))
			 
				end
			else if(@type='F')
				begin
					delete 
					from #tmpRad
					where id not in (select radiologist_id from radiologist_functional_rights_assigned where right_code ='UPDFINALRPT')
				end

			set @counter = @counter + 1
		end

    if(@filter = 'A')
		begin
			select radiologist_id   =  id,
					radiologist_name = name,
					assign ='N'
			from #tmpRad
			order by name
		end
	else if(@filter = 'S')
		begin
				select rs.radiologist_id,
					radiologist_name = r.name,
					assign ='N'
				from radiologist_schedule rs
				inner join #tmpRad r on r.id = rs.radiologist_id
				where getdate() between rs.start_datetime and rs.end_datetime
				order by r.name
		end

	drop table #tmpRad
	drop table #tmpST
	drop table #tmp
	exec sp_xml_removedocument @hDoc
	
	set nocount off
end


GO
