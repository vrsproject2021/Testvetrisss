USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[get_version]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[get_version]
GO
/****** Object:  StoredProcedure [dbo].[get_version]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec get_version ''
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : get_version : get the version
** Created By   : Pavel Guha
** Created On   : 16/07/2021
*******************************************************/
create procedure [dbo].[get_version]
	@version_no nvarchar(50)='' output
as
begin
	select @version_no=version_no
	from sys_version
	where last_updated = (select MAX(last_updated) from sys_version)
	
	--print @version_no
end

GO
