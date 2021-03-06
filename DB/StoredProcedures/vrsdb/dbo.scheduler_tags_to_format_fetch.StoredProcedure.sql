USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_tags_to_format_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_tags_to_format_fetch]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_tags_to_format_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_tags_to_format_fetch : 
                  fetch file download records to finalise
** Created By   : Pavel Guha
** Created On   : 02/08/2019
*******************************************************/
--exec [scheduler_tags_to_format_fetch] '9305A40D-706B-47B9-8E2F-A4422E462053'
create procedure [dbo].[scheduler_tags_to_format_fetch]
	@institution_id uniqueidentifier
as
begin
	
	set nocount on

	
	select group_id,element_id,default_value,junk_characters
	from institution_dispute_dicom_tags
	where institution_id = @institution_id
	

	set nocount off


end


GO
