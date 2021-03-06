USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[hk_study_types_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[hk_study_types_fetch]
GO
/****** Object:  StoredProcedure [dbo].[hk_study_types_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : hk_study_types_fetch : fetch study types
** Created By   : Pavel Guha
** Created On   : 29/05/2019
*******************************************************/
--exec hk_study_types_fetch 'faded955-300e-4517-8f6c-e6513b1889f6'
CREATE procedure [dbo].[hk_study_types_fetch]
    @id uniqueidentifier,
	@modality_id int
as
begin
	 set nocount on
	create table #tmp
	(
		srl_no int identity(1,1),
		study_type_id uniqueidentifier,
		study_type_name nvarchar(50)
	)

	insert into #tmp(study_type_id,study_type_name)
	(select study_type_id,st.name
	from study_hdr_study_types shst
	inner join modality_study_types st on st.id = shst.study_type_id
	where shst.study_hdr_id=@id
	and modality_id=@modality_id)
	order by st.name

	

	select * from #tmp order by srl_no

	select track_by from modality where id=@modality_id
		
	set nocount off
end

GO
