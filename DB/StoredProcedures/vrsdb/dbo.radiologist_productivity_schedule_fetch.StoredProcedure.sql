USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_productivity_schedule_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[radiologist_productivity_schedule_fetch]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_productivity_schedule_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : New Schedule fetch
** Created By   : Kamalaksha Chandra
** Created On   : 02/01/2021
*******************************************************/
--exec radiologist_productivity_schedule_fetch '18Aug2020 00:00','18Aug2020 23:59','00000000-0000-0000-0000-000000000000'
create procedure [dbo].[radiologist_productivity_schedule_fetch]
	@from_date datetime,
	@till_date datetime,
	@radiologist_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@group_id int =0
as
begin
	
	set nocount on
	

	if(isnull(@radiologist_id,'00000000-0000-0000-0000-000000000000')='00000000-0000-0000-0000-000000000000')
		begin
			select rs.id,
					rs.notes,
					rs.start_datetime startTime,rs.end_datetime endTime,rs.radiologist_id readerId
			from radiologist_schedule rs
			inner join radiologists r on r.id = rs.radiologist_id
			where rs.start_datetime between convert(datetime,convert(varchar(11),@from_date,106) + ' 00:00:00') and convert(datetime,convert(varchar(11),@till_date,106) + ' 23:59:59')
				  and  r.acct_group_id = case when @group_id<=0 then r.acct_group_id else @group_id end
			order by rs.start_datetime
		end
	else
		begin
			select rs.id,
					rs.notes,
					rs.start_datetime startTime,rs.end_datetime endTime,rs.radiologist_id readerId
			from radiologist_schedule rs
			inner join radiologists r on r.id = rs.radiologist_id
			where rs.start_datetime between convert(datetime,convert(varchar(11),@from_date,106) + ' 00:00:00') and convert(datetime,convert(varchar(11),@till_date,106) + ' 23:59:59')
			and rs.radiologist_id = @radiologist_id
			order by rs.start_datetime
		end


	set nocount off


end


GO
