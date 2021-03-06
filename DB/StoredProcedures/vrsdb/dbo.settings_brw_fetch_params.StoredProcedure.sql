USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_brw_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_brw_fetch_params]
GO
/****** Object:  StoredProcedure [dbo].[settings_brw_fetch_params]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_brw_fetch_params : fetch
                  settings browser parameters 
** Created By   : Pavel Guha
** Created On   : 23/04/2019
*******************************************************/

CREATE PROCEDURE [dbo].[settings_brw_fetch_params] 
as
begin
	set nocount on
	
	select id,name from user_roles where is_active='Y' and is_visible='Y'  order by name
	select status_id,status_desc from sys_study_status_pacs order by status_id
	select priority_id,priority_desc from sys_priority where is_active = 'Y' order by priority_id

	set nocount off
end


GO
