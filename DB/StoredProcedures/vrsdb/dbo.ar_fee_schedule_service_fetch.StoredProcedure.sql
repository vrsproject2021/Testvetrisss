USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_fee_schedule_service_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_fee_schedule_service_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_fee_schedule_service_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_fee_schedule_service_fetch : fetch fee schedule template
** Created By   : Pavel Guha
** Created On   : 24/02/2021
*******************************************************/

CREATE procedure [dbo].[ar_fee_schedule_service_fetch]
as
begin
	 set nocount on

	 

	 create table #tmp
	 (
		row_id int identity(1,1),
		id uniqueidentifier,
		service_id int,
		modality_id int,
		invoice_by nchar(1),
		invoice_by_desc nvarchar(30),
		default_count_from int ,
		default_count_to int ,
		fee_amount money,
		fee_amount_after_hrs money,
		gl_code nvarchar(5)
	 )

	insert into #tmp(id,service_id,modality_id,invoice_by,invoice_by_desc,default_count_from,default_count_to,
	                 fee_amount,fee_amount_after_hrs,gl_code)
	(select fst.id,fst.service_id,fst.modality_id,
			fst.invoice_by,
			case
				when fst.invoice_by='B' then 'Body Part'
				when fst.invoice_by='I' then 'Image'
				when fst.invoice_by='M' then 'Minute'
		    end invoice_by_desc,
			default_count_from     = isnull(fst.default_count_from,0),
		    default_count_to       = isnull(fst.default_count_to,0),
		    fee_amount              = isnull(fst.fee_amount,0),
		    fee_amount_after_hrs    = isnull(fst.fee_amount_after_hrs,0),
		    gl_code                 = isnull(fst.gl_code,'')
	from ar_service_fee_schedule_template fst
	inner join services s on s.id = fst.service_id
	left outer join modality m on m.id = fst.modality_id
	where fst.deleted='N')
	 order by s.name,isnull(m.name,'')
	
	
	select * from #tmp

	

	drop table #tmp
		
	set nocount off
end

GO
