USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ap_transcriptionist_payment_no_generate]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ap_transcriptionist_payment_no_generate]
GO
/****** Object:  StoredProcedure [dbo].[ap_transcriptionist_payment_no_generate]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ap_transcriptionist_payment_no_generate : 
                  generate payment no
** Created By   : Pavel Guha
** Created On   : 26/10/2020
*******************************************************/
create procedure [dbo].[ap_transcriptionist_payment_no_generate]
	@transcriptionist_code nvarchar(10),
	@payment_srl_no   int         = 0 output,
	@payment_no       nvarchar(50)= '' output
as
	begin
		set nocount on

		declare @RADPMTSRL int,
		        @last_srl_no int,
				@payment_year int 
				

		select @RADPMTSRL= data_value_int
		from invoicing_control_params
		where control_code='RADPMTSRL'

		
		select @payment_year= year(getdate())

		if(select count(payment_srl_no) from ap_transcriptionist_payment_hdr where payment_srl_no=@RADPMTSRL)=0
			begin
				if(@RADPMTSRL>0)
					begin
						set @payment_srl_no = @RADPMTSRL
					end
				else
					begin
						select @last_srl_no = max(payment_srl_no)
						from ap_transcriptionist_payment_hdr 
						where payment_srl_year = @payment_year

						set @last_srl_no = isnull(@last_srl_no,0)
						set @payment_srl_no = @last_srl_no + 1
					end
			end
		else
			begin
				select @last_srl_no = max(payment_srl_no)
				from ap_transcriptionist_payment_hdr 
				where payment_srl_year = @payment_year

				set @last_srl_no = isnull(@last_srl_no,0)
				set @payment_srl_no = @last_srl_no + 1

			end

		set @payment_no =  convert(varchar(4),@payment_year) + '/' + @transcriptionist_code + '/' + convert(varchar,@payment_srl_no)
		
		set nocount off
	end
GO
