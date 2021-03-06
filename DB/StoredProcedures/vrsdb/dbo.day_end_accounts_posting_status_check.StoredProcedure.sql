USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[day_end_accounts_posting_status_check]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[day_end_accounts_posting_status_check]
GO
/****** Object:  StoredProcedure [dbo].[day_end_accounts_posting_status_check]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : day_end_accounts_posting_status_check : 
				  check the day end accounts posting status
** Created By   : Pavel Guha
** Created On   : 24/12/2020
*******************************************************/
create procedure [dbo].[day_end_accounts_posting_status_check]
	@day_end_date datetime,
	@status nchar(1)='N' output
as
begin
	set nocount on

	if(select count(day_end_date) from day_end_accounts_posting_processed where day_end_date=@day_end_date)>0
		begin
			select @status=process_completed from day_end_accounts_posting_processed where day_end_date=@day_end_date
		end
	else
		begin
			set @status='N'
		end


	set nocount off
	return 1

end

GO
