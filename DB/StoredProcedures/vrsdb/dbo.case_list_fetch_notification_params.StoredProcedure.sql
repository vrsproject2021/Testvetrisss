USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fetch_notification_params]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_fetch_notification_params]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fetch_notification_params]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_fetch_notification_params : fetch case 
                  email parameters
** Created By   : Pavel Guha
** Created On   : 21/04/2019
*******************************************************/
CREATE procedure [dbo].[case_list_fetch_notification_params]
    @id uniqueidentifier
as
begin
	 set nocount on

	select institution_name= isnull(ins.name,''),institution_email_id =isnull(ins.email_id,''),institution_mobile=isnull(ins.mobile_no,''),
		   physician_name = isnull(ph.name,''),physician_email_id = isnull(ph.email_id,''),physician_mobile=isnull(ph.mobile_no,'')
	from study_hdr hdr
	inner join institutions ins on ins.id= hdr.institution_id
	inner join physicians ph on ph.id= hdr.physician_id
	where hdr.id=@id


	set nocount off
end

GO
