USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_StudyReport_fetch_dtls]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_StudyReport_fetch_dtls]
GO
/****** Object:  StoredProcedure [dbo].[rpt_StudyReport_fetch_dtls]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_StudyReport_fetch_dtls : fetch case report details
** Created By   : Pavel Guha
** Created On   : 20/04/2019
*******************************************************/
--exec rpt_StudyReport_fetch_dtls'28292a63-f658-48a2-94e1-9284339522dc'
CREATE procedure [dbo].[rpt_StudyReport_fetch_dtls]
    @id nvarchar(36)
    
as
begin
	 set nocount on

	select report_id,report_text
	from study_hdr_prelim_reports
	where convert(varchar(36),study_hdr_id)=@id


	set nocount off
end

GO
