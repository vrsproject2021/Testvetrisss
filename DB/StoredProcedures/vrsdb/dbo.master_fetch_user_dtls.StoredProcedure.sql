USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_fetch_user_dtls]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_fetch_user_dtls]
GO
/****** Object:  StoredProcedure [dbo].[master_fetch_user_dtls]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_fetch_user_dtls : fetch 
                  physician details
** Created By   : Pavel Guha
** Created On   : 26/04/2019
*******************************************************/
CREATE procedure [dbo].[master_fetch_user_dtls]
    @login_id nvarchar(50)
as
begin
	 set nocount on

	 select id,login_id,password,pacs_user_id,pacs_password,email_id,
	        contact_no= isnull(contact_no,''),is_active
	 from users 
	 where login_id=@login_id
	 and is_active='Y'
		
	set nocount off
end

GO
