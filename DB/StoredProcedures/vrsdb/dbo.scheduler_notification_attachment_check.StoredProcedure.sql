USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_notification_attachment_check]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_notification_attachment_check]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_notification_attachment_check]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_notification_attachment_check : 
                  check attachment exists or not
** Created By   : Pavel Guha
** Created On   : 27/05/2020
*******************************************************/
CREATE procedure [dbo].[scheduler_notification_attachment_check]
	@file_name nvarchar(4000),
	@file_count int = 0 output
as
begin
	select @file_count = count(email_log_id) 
	from vrslogdb..email_log 
	where isnull(file_name,'') like '%' +  @file_name + '%'
end

GO
