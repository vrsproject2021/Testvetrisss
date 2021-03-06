USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rate_brw_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rate_brw_fetch_params]
GO
/****** Object:  StoredProcedure [dbo].[rate_brw_fetch_params]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rate_brw_fetch_params : fetch rate
                  browser parameters 
** Created By   : Pavel Guha
** Created On   : 26/06/2019
*******************************************************/

CREATE PROCEDURE [dbo].[rate_brw_fetch_params] 
as
begin
	set nocount on
	
	select id,name from modality where is_active='Y' order by name
	select id,name from services where is_active='Y' order by name

	set nocount off
end


GO
