USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[modality_wise_study_type_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[modality_wise_study_type_fetch]
GO
/****** Object:  StoredProcedure [dbo].[modality_wise_study_type_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    :  modality_wise_study_type_fetch:fetch modality wise study_types
** Created By   : Pavel Guha
** Created On   : 19/04/2019
*******************************************************
*******************************************************/
-- exec modality_wise_study_type_fetch 2
create procedure [dbo].[modality_wise_study_type_fetch]
	@modality_id int
as
begin
	set nocount on
	
	select id,name 
	from modality_study_types 
	where modality_id=@modality_id 
	and is_active='Y'
	order by name

	set nocount off
	
end

GO
