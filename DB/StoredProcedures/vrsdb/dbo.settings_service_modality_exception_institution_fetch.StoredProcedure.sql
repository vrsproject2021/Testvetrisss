USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_service_modality_exception_institution_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_service_modality_exception_institution_fetch]
GO
/****** Object:  StoredProcedure [dbo].[settings_service_modality_exception_institution_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_service_modality_exception_institution_fetch : 
                  fetch exception institution for modality wise service availability
** Created By   : Pavel Guha 
** Created On   : 02/04/2021
*******************************************************/
--exec settings_service_modality_exception_institution_fetch 1,1,'N',9,'11111111-1111-1111-1111-111111111111','',0
CREATE Procedure [dbo].[settings_service_modality_exception_institution_fetch]
    @service_id int,
	@record_id int,
	@after_hours nchar(1),
    @menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
As
	Begin
		set nocount on
	  
		select smei.institution_id,i.code,i.name,sel='Y'
		from settings_service_modality_available_exception_institution smei
		inner join institutions i on i.id = smei.institution_id
		where i.is_active='Y'
		and smei.modality_id=@record_id
		and smei.service_id=@service_id
		and smei.after_hours=@after_hours
		union
		select institution_id = id,code,name,sel='N'
		from institutions
		where is_active='Y'
		and id not in (select institution_id
		               from settings_service_modality_available_exception_institution
					   where modality_id=@record_id
					   and service_id=@service_id
					   and after_hours=@after_hours)
		order by sel desc,name

		set nocount off
	End
GO
