USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[control_params_fetch_by_codes]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[control_params_fetch_by_codes]
GO
/****** Object:  StoredProcedure [dbo].[control_params_fetch_by_codes]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---- =============================================
---- Author:		KC
---- Create date: 26/04/2020
---- Description:	control_params_fetch_by_codes : fetch by comma seperated codes
---- exec [dbo].[control_params_fetch_by_codes] 'TNTKNKEY,TNAPIKEY,OLPMTURL'
---- =============================================

CREATE PROCEDURE [dbo].[control_params_fetch_by_codes] 
(
	@codes nvarchar(max) -- comma seperated
)
AS
BEGIN
	
	SET NOCOUNT ON;

	select 
		control_code, data_value_char, data_value_int, data_value_dec, value_type, ui_prefix
	from invoicing_control_params a
	inner join dbo.Split(@codes,',') b on b.[Data]=a.control_code;

	set nocount off

END

GO
