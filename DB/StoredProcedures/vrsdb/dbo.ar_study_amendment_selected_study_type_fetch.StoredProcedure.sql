USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_study_amendment_selected_study_type_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_study_amendment_selected_study_type_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_study_amendment_selected_study_type_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_study_amendment_selected_study_type_fetch : 
                  fetch selected study types
** Created By   : Pavel Guha 
** Created On   : 16-Mar-2021
*******************************************************/

--exec ar_study_amendment_study_type_fetch 'cf84156f-8f53-4906-9a0d-0fcb65393225',3
create procedure [dbo].[ar_study_amendment_selected_study_type_fetch]
	@id uniqueidentifier,
	@modality_id int
	
as
	begin
		set nocount on

		declare @category_id int

		create table #tmp
		(
			srl_no int identity(1,1),
			study_type_id uniqueidentifier,
			study_type_name nvarchar(50)
		)

		if(Select count(id) from study_hdr where id=@id)>0
			begin
				select @category_id = category_id from study_hdr where id=@id
				insert into #tmp(study_type_id,study_type_name)
     			(select shst.study_type_id,
					   st.name
				from study_hdr_study_types shst
				inner join modality_study_types st on st.id = shst.study_type_id
				where shst.study_hdr_id=@id
				and st.modality_id = @modality_id
				and st.category_id=@category_id)
				order by name
			end
		else
			begin
				select @category_id = category_id from study_hdr_archive where id=@id

				insert into #tmp(study_type_id,study_type_name)
     			(select shst.study_type_id,
					   st.name
				from study_hdr_study_types_archive shst
				inner join modality_study_types st on st.id = shst.study_type_id
				where shst.study_hdr_id=@id
				and st.modality_id = @modality_id
				and st.category_id=@category_id)
				order by name
			end
		
		select * from #tmp
		drop table #tmp
		set nocount off
	end

	
GO
