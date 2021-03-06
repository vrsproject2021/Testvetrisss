USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[profile_physician_email_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[profile_physician_email_fetch]
GO
/****** Object:  StoredProcedure [dbo].[profile_physician_email_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : profile_physician_email_fetch : fetch emai ids
                  of physician
** Created By   : Pavel Guha
** Created On   : 14/02/2020
*******************************************************/
--exec profile_physician_email_fetch '61BE4379-1750-4081-B049-14B81F82D36E','37C228C8-A040-4BA0-A61F-0FDEF5BD7C9F'
create procedure [dbo].[profile_physician_email_fetch]
    @physician_id uniqueidentifier,
	@institution_id uniqueidentifier
as
begin
	 set nocount on

	 declare @email_id nvarchar(500)

	 create table #tmp
	(
		rec_id int identity(1,1),
		physician_email nvarchar(500),
		del nvarchar(1) default ''
	)
	
	select @email_id = physician_email
	from institution_physician_link 
	where institution_id=@institution_id
	and physician_id=@physician_id

	set @email_id = isnull(@email_id,'')
	set @email_id = rtrim(ltrim(@email_id))

	if(@email_id<>'')
		begin
			if(charindex(';',@email_id)=0)
				begin
					insert into #tmp(physician_email) values (@email_id)
				end
			else
				begin
					insert into #tmp(physician_email) (select data from dbo.Split(@email_id,';'))
				end
		end

	select * from #tmp

	drop table #tmp
		
	set nocount off
end

GO
