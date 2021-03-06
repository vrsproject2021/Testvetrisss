USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_image_count_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_image_count_fetch]
GO
/****** Object:  StoredProcedure [dbo].[case_list_image_count_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_image_count_fetch : fetch image count
** Created By   : Pavel Guha
** Created On   : 27/11/2019
*******************************************************/
CREATE procedure [dbo].[case_list_image_count_fetch]
    @id uniqueidentifier
as
begin
	 set nocount on

	select img_count =  isnull(img_count,0),
	       object_count = isnull(object_count,0)
	from study_hdr 
	where id=@id


	set nocount off
end


GO
