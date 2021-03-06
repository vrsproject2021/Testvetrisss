USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_gl_code_map_non_revenue_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_gl_code_map_non_revenue_fetch]
GO
/****** Object:  StoredProcedure [dbo].[settings_gl_code_map_non_revenue_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_gl_code_map_non_revenue_fetch : 
				  fetch g/l code mapping with non reveni=ue heads
** Created By   : Pavel Guha
** Created On   : 17/06/2020
*******************************************************/
--exec settings_gl_code_map_non_revenue_fetch
create procedure [dbo].[settings_gl_code_map_non_revenue_fetch]

as
begin
		set nocount on

		create table #tmp
		(
			row_id int identity(1,1),
			control_code nvarchar(10),
			control_desc nvarchar(50),
			gl_code nvarchar(5)
		)
		
		insert into #tmp(control_code,control_desc,gl_code)
		(select control_code,control_desc,gl_code
		from ar_non_revenue_acct_control)
		order by control_desc

		select * from #tmp

		drop table #tmp

		set nocount off
end

GO
