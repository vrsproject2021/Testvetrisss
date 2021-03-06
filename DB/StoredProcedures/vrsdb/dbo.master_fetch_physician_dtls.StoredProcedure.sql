USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_fetch_physician_dtls]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_fetch_physician_dtls]
GO
/****** Object:  StoredProcedure [dbo].[master_fetch_physician_dtls]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_fetch_physician_dtls : fetch 
                  physician details
** Created By   : Pavel Guha
** Created On   : 26/04/2019
*******************************************************/
CREATE procedure [dbo].[master_fetch_physician_dtls]
    @email_id nvarchar(50)='',
	@mobile_no nvarchar(20)='',
	@name nvarchar(200)=''
as
begin
	 set nocount on

	 if(rtrim(ltrim(isnull(@email_id,'')))<> '')
		begin
			select id,name,email_id,mobile_no
			from physicians
			where upper(rtrim(ltrim(email_id))) = upper(rtrim(ltrim(@email_id)))
		end
	 else if(rtrim(ltrim(isnull(@mobile_no,'')))<> '')
		begin
			select id,name,email_id,mobile_no
			from physicians
			where upper(rtrim(ltrim(mobile_no))) = upper(rtrim(ltrim(@mobile_no)))
		end
	 else if(rtrim(ltrim(isnull(@name,'')))<> '')
		begin
			select id,name,email_id,mobile_no
			from physicians
			where upper(rtrim(ltrim(name))) = upper(rtrim(ltrim(@name)))
		end
	else 
		begin
			select id,name,email_id,mobile_no
			from physicians
			where id ='00000000-0000-0000-0000-000000000000'
			
		end
	
		
	set nocount off
end

GO
