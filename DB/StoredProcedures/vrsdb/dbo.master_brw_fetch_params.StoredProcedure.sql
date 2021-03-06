USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_brw_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_brw_fetch_params]
GO
/****** Object:  StoredProcedure [dbo].[master_brw_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_brw_fetch_params : fetch case list
                  browser parameters 
** Created By   : Pavel Guha
** Created On   : 23/04/2019
*******************************************************/

CREATE PROCEDURE [dbo].[master_brw_fetch_params] 
as
begin
	set nocount on
	
	select id,name from sys_country order by name
	select id,name from modality  where is_active='Y' order by name
	select id,name from species where is_active='Y' order by name
	select priority_id,priority_desc from sys_priority where is_active='Y'  order by priority_desc
	select id,name from sys_study_category order by name
	select id,
			case
				  when gmt_diff<0 then name + ' (GMT' + convert(varchar(10) ,(gmt_diff_mins/60) ) + ':' + replicate('0',2-len(convert(varchar(10) ,(abs(gmt_diff_mins%60))))) + convert(varchar(10) ,(abs(gmt_diff_mins%60))) + ')'
				      else name + ' (GMT+' + convert(varchar(10) ,(gmt_diff_mins/60) ) + ':' + replicate('0',2-len(convert(varchar(10) ,(abs(gmt_diff_mins%60))))) + convert(varchar(10) ,(abs(gmt_diff_mins%60))) + ')'
			end name,
		   is_default 
	from sys_us_time_zones 
	order by gmt_diff,name
	select id,name from sys_radiologist_group order by display_order

	set nocount off
end


GO
