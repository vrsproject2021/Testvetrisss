USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[institution_wise_physician_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[institution_wise_physician_fetch]
GO
/****** Object:  StoredProcedure [dbo].[institution_wise_physician_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    :  institution_wise_physician_fetch:fetch '
                   institution wise physicians
** Created By   : Pavel Guha
** Created On   : 18/04/2019
*******************************************************
*******************************************************/
-- exec institution_wise_physician_fetch 2
CREATE procedure [dbo].[institution_wise_physician_fetch]
	@institution_id uniqueidentifier
as
begin
	set nocount on
	
	select id,name=rtrim(ltrim(isnull(fname,'') + ' ' + isnull(lname,'') + ' ' +  isnull(credentials,''))) 
	from physicians 
	where is_active='Y' 
	and id in (select physician_id 
	           from institution_physician_link 
			   where institution_id=@institution_id) 
	order by lname

	select code,consult_applicable,patient_id_srl from institutions where id=@institution_id
	

	set nocount off
	
end

GO
