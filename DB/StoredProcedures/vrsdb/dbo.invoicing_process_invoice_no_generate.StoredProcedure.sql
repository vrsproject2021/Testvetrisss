USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_process_invoice_no_generate]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_process_invoice_no_generate]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_process_invoice_no_generate]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_process_invoice_no_generate : 
                  generate invoice no
** Created By   : Pavel Guha 
** Created On   : 20/11/2019
*******************************************************/
CREATE procedure [dbo].[invoicing_process_invoice_no_generate]
	@invoice_srl_no   int         = 0 output,
	@invoice_no       nvarchar(50)='' output
as
	begin
		set nocount on

		declare @STARTINVSRL int,
		        @INVPRFX nvarchar(200),
		        @last_srl_no int,
				@invoice_year int 
				

		select @STARTINVSRL= data_value_int
		from invoicing_control_params
		where control_code='STARTINVSRL'

		select @INVPRFX = data_value_char
		from invoicing_control_params
		where control_code='INVPRFX'

		select @invoice_year= year(getdate())

		if(select count(invoice_srl_no) from invoice_hdr where invoice_srl_no=@STARTINVSRL)=0
			begin
				set @invoice_srl_no = @STARTINVSRL
			end
		else
			begin
				select @last_srl_no = max(invoice_srl_no)
				from invoice_hdr 
				where invoice_srl_year = @invoice_year

				set @last_srl_no = isnull(@last_srl_no,0)
				set @invoice_srl_no = @last_srl_no + 1
			end
		if(rtrim(ltrim(isnull(@INVPRFX,'')))<>'') set @INVPRFX=@INVPRFX + '/' 
		set @invoice_no = @INVPRFX  + convert(varchar(4),@invoice_year) + '/' + convert(varchar,@invoice_srl_no)
		
		set nocount off
	end
GO
