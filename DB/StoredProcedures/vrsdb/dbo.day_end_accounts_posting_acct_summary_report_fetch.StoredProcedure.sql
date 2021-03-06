USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[day_end_accounts_posting_acct_summary_report_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[day_end_accounts_posting_acct_summary_report_fetch]
GO
/****** Object:  StoredProcedure [dbo].[day_end_accounts_posting_acct_summary_report_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : day_end_accounts_posting_acct_summary_report_fetch : 
				  fetch account wise summary at day end
** Created By   : Pavel Guha
** Created On   : 24/12/2020
*******************************************************/
create procedure [dbo].[day_end_accounts_posting_acct_summary_report_fetch]
	@day_end_date datetime
as
begin
	set nocount on

	select gl_code,gl_desc,
	       dr_amt = sum(dr_amount),
		   cr_amt = sum(cr_amount)
	from day_end_accounts_postings
	where day_end_date=@day_end_date
	group by gl_code,gl_desc
	order by gl_code

	set nocount off
	
end

GO
