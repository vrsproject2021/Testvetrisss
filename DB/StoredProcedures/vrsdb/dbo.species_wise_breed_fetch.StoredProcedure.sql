USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[species_wise_breed_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[species_wise_breed_fetch]
GO
/****** Object:  StoredProcedure [dbo].[species_wise_breed_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    :  species_wise_breed_fetch:fetch species wise breeds
** Created By   : Pavel Guha
** Created On   : 11/04/2019
*******************************************************
*******************************************************/
-- exec species_wise_breed_fetch 2
CREATE procedure [dbo].[species_wise_breed_fetch]
	@species_id int
as
begin
	set nocount on
	
	select id,name 
	from breed 
	where species_id=@species_id 
	and is_active='Y'
	order by name

	set nocount off
	
end

GO
