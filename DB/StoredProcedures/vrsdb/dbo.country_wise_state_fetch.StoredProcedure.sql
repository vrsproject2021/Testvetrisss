USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[country_wise_state_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[country_wise_state_fetch]
GO
/****** Object:  StoredProcedure [dbo].[country_wise_state_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[country_wise_state_fetch]
	@country_id int 
as
begin
	set nocount on
	
	select id,name from sys_states where country_id=@country_id  order by name
	

	set nocount off
end

GO
