USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_prelimreport_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_prelimreport_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_prelimreport_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_prelimreport_fetch : fetch 
                  preliminary report details
** Created By   : Pavel Guha
** Created On   : 13-05-2020
*******************************************************/
--exec rpt_prelimreport_fetch 'DE1401A2-AC74-4FA7-A4FC-E3E868D24C93','11111111-1111-1111-1111-111111111111'

CREATE procedure [dbo].[rpt_prelimreport_fetch]
    @id uniqueidentifier,
	@user_id uniqueidentifier
    
as
begin
	 set nocount on

	 declare @study_id nvarchar(36),
	         @userid nvarchar(36)

	 select company_name    = (select data_value_char from invoicing_control_params where control_code='COMPNAME'),
			company_address = (select data_value_char from invoicing_control_params where control_code='COMPADDR')

	 select @study_id = convert(varchar(36),@id),
	        @userid   = convert(varchar(36),@user_id)

	-- report details
	 exec rpt_studyreport_prelim_fetch
		   @id      = @study_id,
		   @user_id = @userid

	
	set nocount off
end

GO
