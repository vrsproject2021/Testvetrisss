USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_fetch_salesperson_dtls]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_fetch_salesperson_dtls]
GO
/****** Object:  StoredProcedure [dbo].[master_fetch_salesperson_dtls]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_fetch_salesperson_dtls : fetch 
                  sales person details
** Created By   : Pavel Guha
** Created On   : 21/05/2019
*******************************************************/
create procedure [dbo].[master_fetch_salesperson_dtls]
    @email_id nvarchar(50)=''
as
begin
	 set nocount on

	 if(rtrim(ltrim(isnull(@email_id,'')))<> '')
		begin
			select distinct sp.id,sp.fname,sp.lname,ispl.salesperson_login_email,ispl.salesperson_email,ispl.salesperson_mobile,ispl.salesperson_pacs_user_id,ispl.salesperson_pacs_password
			from salespersons sp
			inner join institution_salesperson_link ispl on ispl.salesperson_id = sp.id
			where upper(rtrim(ltrim(ispl.salesperson_login_email))) = upper(rtrim(ltrim(@email_id)))
		end
	else 
		begin
			select id = '00000000-0000-0000-0000-000000000000',
			       fname='',
				   lname='',
				   salesperson_login_email   ='',
				   salesperson_email         = '',
				   salesperson_mobile        = '',
				   salesperson_pacs_user_id  = '',
				   salesperson_pacs_password = ''
			
		end
	
		
	set nocount off
end

GO
