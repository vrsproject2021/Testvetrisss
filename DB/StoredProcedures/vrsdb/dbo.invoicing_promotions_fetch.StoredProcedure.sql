USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_promotions_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_promotions_fetch]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_promotions_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_promotions_fetch : fetch 
                  billing account Institutions
** Created By   : BK
** Created On   : 26/11/2019
*******************************************************/
--exec invoicing_promotions_fetch '21187A6F-1173-4F82-B460-25FD2421D01A'
--exec invoicing_promotions_fetch '00000000-0000-0000-0000-000000000000'
CREATE procedure [dbo].[invoicing_promotions_fetch]
    @id uniqueidentifier,
	@billing_account_id uniqueidentifier
as
begin
	 set nocount on
	declare @count1 int,
	        @count2 int
	
	 create table #tmp
	 (
		id uniqueidentifier,
		line_no int,
		institution_id uniqueidentifier,
		modality_id int,
		discount_percent decimal(5,2) null default 0,
		free_credits int null default 0,
		del nvarchar(1) null default ''
	 )

	 insert into #tmp(id,line_no,institution_id,modality_id,discount_percent,free_credits)
	 (select id,line_no,institution_id,modality_id,discount_percent,free_credits
	  from ar_promotion_institution
	  where hdr_id=@id)

	
	select * from #tmp

	select bail.institution_id,institution_name=i.name
	from billing_account_institution_link bail
	inner join institutions i on i.id = bail.institution_id
	where bail.billing_account_id=@billing_account_id
	and i.is_active='Y'
	order by i.name

	select id,name from modality where is_active='Y' order by name
		
	

	drop table #tmp
	set nocount off
end

GO
