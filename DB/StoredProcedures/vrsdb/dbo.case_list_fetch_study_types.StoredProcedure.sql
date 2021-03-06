USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fetch_study_types]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_fetch_study_types]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fetch_study_types]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_fetch_study_types : fetch single task header docs.
** Created By   : Pavel Guha
** Created On   : 10/04/2019
*******************************************************/
--exec case_list_fetch_study_types 'faded955-300e-4517-8f6c-e6513b1889f6'
CREATE procedure [dbo].[case_list_fetch_study_types]
    @id uniqueidentifier,
	@modality_id int,
	@institution_id uniqueidentifier='00000000-0000-0000-0000-000000000000'
as
begin
	 set nocount on
	create table #tmp
	(
		srl_no int identity(1,1),
		study_type_id uniqueidentifier,
		study_type_name nvarchar(50),
		sel nchar(1),
		validate_study_count nchar(1),
		category_id int
	)

	insert into #tmp(study_type_id,study_type_name,sel,validate_study_count,category_id)
	(select shst.study_type_id,st.name,'Y',st.validate_study_count,st.category_id
	from study_hdr_study_types shst
	inner join modality_study_types st on st.id = shst.study_type_id
	where shst.study_hdr_id=@id
	and st.is_active='Y')
	order by st.name

	insert into #tmp(study_type_id,study_type_name,sel,validate_study_count,category_id)
	(select id,name,'N',validate_study_count,category_id
	from modality_study_types 
	where modality_id=@modality_id
	and is_active ='Y'
	and category_id in (select category_id from institution_category_link where institution_id = @institution_id )
	and id not in (select study_type_id from study_hdr_study_types where study_hdr_id=@id))
	order by name

	select * from #tmp order by srl_no

	select track_by,invoice_by from modality where id=@modality_id
		
	set nocount off
end

GO
