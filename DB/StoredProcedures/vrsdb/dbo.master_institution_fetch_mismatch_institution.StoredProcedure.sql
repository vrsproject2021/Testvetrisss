USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_mismatch_institution]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_institution_fetch_mismatch_institution]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_mismatch_institution]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_institution_fetch_mismatch_institution : 
                  fetch mismatch institutions
** Created By   : Pavel Guha
** Created On   : 15/09/2020
*******************************************************/
--exec master_institution_fetch_mismatch_institution '669AB247-0FC8-4518-A802-03828A1384F6'
CREATE procedure [dbo].[master_institution_fetch_mismatch_institution]
	@id uniqueidentifier
as
begin
	 set nocount on

    create table #tmp
	(
		rec_id int identity(1,1),
		institution_id uniqueidentifier,
		instituion_name nvarchar(100),
		sel nchar(1) default 'N'
	)


	insert into #tmp(institution_id,instituion_name,sel)
	( select id   = institution_id,
	         name = alternate_name,
			 sel  ='Y'
	  from institution_alt_name_link
	  where institution_id = @id
	  union
	  select id,name,sel='N'
	  from institutions
	  where is_active='Y'
	  and isnull(code,'')='')
	  order by name

	select row_number() over(order by sel desc,instituion_name) as rec_id,
	       institution_id,
		   instituion_name,
		   sel
	from #tmp 

	drop table #tmp
		
	set nocount off
end

GO
