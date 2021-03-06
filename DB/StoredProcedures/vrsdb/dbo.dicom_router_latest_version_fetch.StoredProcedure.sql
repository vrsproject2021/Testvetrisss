USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[dicom_router_latest_version_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[dicom_router_latest_version_fetch]
GO
/****** Object:  StoredProcedure [dbo].[dicom_router_latest_version_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : dicom_router_latest_version_fetch : fetch
                  dicom router's latest 
** Created By   : Pavel Guha
** Created On   : 09/11/2019
*******************************************************/
create procedure [dbo].[dicom_router_latest_version_fetch]
	@version_no nvarchar(50)='' output
as
begin
	select @version_no=version_no
	from sys_dicom_router_version
	where date_released = (select MAX(date_released) from sys_dicom_router_version)
end
GO
