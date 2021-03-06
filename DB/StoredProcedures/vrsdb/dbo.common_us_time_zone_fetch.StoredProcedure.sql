USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[common_us_time_zone_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[common_us_time_zone_fetch]
GO
/****** Object:  StoredProcedure [dbo].[common_us_time_zone_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : common_us_time_zone_fetch : fetch US timezones
** Created By   : Pavel Guha 
** Created On   : 17/04/2019
*******************************************************/
create Procedure [dbo].[common_us_time_zone_fetch]

As
	Begin
		
		select id,name,gmt_diff,is_default from sys_us_time_zones order by gmt_diff desc

		
	End
GO
