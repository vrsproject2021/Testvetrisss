USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_schedule_details_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[radiologist_schedule_details_fetch]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_schedule_details_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_approved_grouped_img_files_to_transfer_fetch : 
                  fetch file download records to finalise
** Created By   : Pavel Guha
** Created On   : 16/09/2019
*******************************************************/
--exec radiologist_schedule_details_fetch '18Aug2020 00:00','18Aug2020 23:59','00000000-0000-0000-0000-000000000000'
CREATE procedure [dbo].[radiologist_schedule_details_fetch]
	@from_date datetime,
	@till_date datetime,
	@radiologist_id uniqueidentifier='00000000-0000-0000-0000-000000000000'
as
begin
	
	set nocount on
	

	if(isnull(@radiologist_id,'00000000-0000-0000-0000-000000000000')='00000000-0000-0000-0000-000000000000')
		begin
			select rs.id,name = r.name,r.identity_color,
					notes = right('0'+ltrim(right(convert(varchar,rs.start_datetime,100),8)),7)  + '-' +  right('0'+ltrim(right(convert(varchar,rs.end_datetime,100),8)),7) + ' ' +rs.notes,
					rs.start_datetime,rs.end_datetime,rs.duration_in_ms,rs.radiologist_id
			from radiologist_schedule rs
			inner join radiologists r on r.id = rs.radiologist_id
			where rs.start_datetime between convert(datetime,convert(varchar(11),@from_date,106) + ' 00:00:00') and convert(datetime,convert(varchar(11),@till_date,106) + ' 23:59:59')
			order by rs.start_datetime
		end
	else
		begin
			select rs.id,name = r.name,r.identity_color,
					notes = right('0'+ltrim(right(convert(varchar,rs.start_datetime,100),8)),7)  + '-' +  right('0'+ltrim(right(convert(varchar,rs.end_datetime,100),8)),7) + ' ' +rs.notes,
					rs.start_datetime,rs.end_datetime,rs.duration_in_ms,rs.radiologist_id
			from radiologist_schedule rs
			inner join radiologists r on r.id = rs.radiologist_id
			where rs.start_datetime between convert(datetime,convert(varchar(11),@from_date,106) + ' 00:00:00') and convert(datetime,convert(varchar(11),@till_date,106) + ' 23:59:59')
			and rs.radiologist_id = @radiologist_id
			order by rs.start_datetime
		end


	set nocount off


end


GO
