USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_radiologist_payment_settings_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_radiologist_payment_settings_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_radiologist_payment_settings_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_radiologist_payment_settings_fetch : fetch 
                  invoice settings
** Created By   : Pavel Guha
** Created On   : 13/11/2019
*******************************************************/
--exec rpt_radiologist_payment_settings_fetch
create procedure [dbo].[rpt_radiologist_payment_settings_fetch]
as
begin
	 set nocount on

	 select company_address	    = (select data_value_char from invoicing_control_params where control_code='COMPADDR')
	        
	  
		
	set nocount off
end

GO
