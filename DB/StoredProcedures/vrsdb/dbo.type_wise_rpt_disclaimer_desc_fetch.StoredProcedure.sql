USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[type_wise_rpt_disclaimer_desc_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[type_wise_rpt_disclaimer_desc_fetch]
GO
/****** Object:  StoredProcedure [dbo].[type_wise_rpt_disclaimer_desc_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    :  type_wise_rpt_disclaimer_desc_fetch:
                  fetch type wise report disclaimer reason
				  description
** Created By   : Pavel Guha
** Created On   : 26/11/2020
*******************************************************
*******************************************************/
-- exec type_wise_rpt_disclaimer_desc_fetch 2
create procedure [dbo].[type_wise_rpt_disclaimer_desc_fetch]
	@id int
as
begin
	set nocount on
	
	select description from report_disclaimer_reasons where id=@id

	set nocount off
	
end

GO
