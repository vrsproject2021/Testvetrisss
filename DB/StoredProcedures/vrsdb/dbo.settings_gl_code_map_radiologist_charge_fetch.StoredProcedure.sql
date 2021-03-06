USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_gl_code_map_radiologist_charge_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_gl_code_map_radiologist_charge_fetch]
GO
/****** Object:  StoredProcedure [dbo].[settings_gl_code_map_radiologist_charge_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_gl_code_map_radiologist_charge_fetch : 
				  fetch g/l code mapping with non reveni=ue heads
** Created By   : Pavel Guha
** Created On   : 03/10/2020
*******************************************************/
--exec settings_gl_code_map_radiologist_charge_fetch
create procedure [dbo].[settings_gl_code_map_radiologist_charge_fetch]

as
begin
		set nocount on

		
		select ROW_NUMBER() over (order by id) as row_id,
		       id,name,gl_code
		from sys_radiologist_group



		set nocount off
end

GO
