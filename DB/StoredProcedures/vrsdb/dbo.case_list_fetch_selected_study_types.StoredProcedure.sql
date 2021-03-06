USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fetch_selected_study_types]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_fetch_selected_study_types]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fetch_selected_study_types]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_fetch_selected_study_types : fetch 
				  selected study types
** Created By   : Pavel Guha
** Created On   : 19/07/2019
*******************************************************/
--exec case_list_fetch_selected_study_types '70e5f2c4-343c-4aa5-b6d2-812af6f802e7',1
CREATE procedure [dbo].[case_list_fetch_selected_study_types]
    @id uniqueidentifier,
	@modality_id int,
	@institution_id uniqueidentifier='00000000-0000-0000-0000-000000000000'
as
begin
	 set nocount on

	 declare @strSQL varchar(max),
	         @db_name nvarchar(50)

	create table #tmp
	(
		srl_no int identity(1,1),
		study_type_id uniqueidentifier,
		study_type_name nvarchar(50)
	)

	if(isnull(@institution_id,'00000000-0000-0000-0000-000000000000')='00000000-0000-0000-0000-000000000000')
		begin
			select @institution_id= institution_id from study_hdr where id=@id
		end

	if(select count(study_hdr_id) from study_hdr_study_types where study_hdr_id=@id)>0
		begin
			insert into #tmp(study_type_id,study_type_name)
			(select study_type_id,st.name
			from study_hdr_study_types shst
			inner join modality_study_types st on st.id = shst.study_type_id
			where shst.study_hdr_id=@id
			and st.category_id in (select category_id from institution_category_link where institution_id=isnull(@institution_id,'00000000-0000-0000-0000-000000000000')))
			order by st.name
		end
	else if(select count(study_hdr_id) from study_hdr_study_types_archive where study_hdr_id=@id)>0
		begin
			insert into #tmp(study_type_id,study_type_name)
			(select study_type_id,st.name
			from study_hdr_study_types_archive shst
			inner join modality_study_types st on st.id = shst.study_type_id
			where shst.study_hdr_id=@id
			and st.category_id in (select category_id from institution_category_link where institution_id=isnull(@institution_id,'00000000-0000-0000-0000-000000000000')))
			order by st.name
		end
	else
		begin
			set @db_name=''
			exec common_get_study_database
				@id      = @id,
				@db_name = @db_name output

			set @strSQL = 'insert into #tmp(study_type_id,study_type_name)'
			set @strSQL = @strSQL + '(select study_type_id,st.name '
			set @strSQL = @strSQL + 'from ' + @db_name + '..study_hdr_study_types_archive shst '
			set @strSQL = @strSQL + 'inner join modality_study_types st on st.id = shst.study_type_id '
			set @strSQL = @strSQL + 'where shst.study_hdr_id= ''' + convert(varchar(36),@id) + ''' '
			set @strSQL = @strSQL + 'and st.category_id in (select category_id from institution_category_link where institution_id=isnull(''' +convert(varchar(36), @institution_id) + ''',''00000000-0000-0000-0000-000000000000''))) '
			set @strSQL = @strSQL + 'order by st.name'

			--print @strSQL
			exec(@strSQL)
		end



	select * from #tmp order by srl_no
	select track_by from modality where id=@modality_id
		
	set nocount off
end

GO
