USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_salesperson_fetch_institutions]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_salesperson_fetch_institutions]
GO
/****** Object:  StoredProcedure [dbo].[master_salesperson_fetch_institutions]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_salesperson_fetch_institutions : fetch institutions 
				  linked to salespersons
** Created By   : Pavel Guha
** Created On   : 21/05/2019
*******************************************************/
--exec master_salesperson_fetch_institutions 'faded955-300e-4517-8f6c-e6513b1889f6'
CREATE procedure [dbo].[master_salesperson_fetch_institutions]
    @id uniqueidentifier
as
begin
	 set nocount on

	 create table #tmp
	(
		rec_id int identity(1,1),
		institution_id uniqueidentifier,
		salesperson_user_id uniqueidentifier,
		commission_1st_yr decimal(5,2),-- Added on 4th SEP 2019 @BK
		commission_2nd_yr decimal(5,2),-- Added on 4th SEP 2019 @BK
		del nvarchar(1) default ''
	)
	
	insert into #tmp(institution_id,salesperson_user_id,commission_1st_yr,commission_2nd_yr)
	(select spl.institution_id,spl.salesperson_user_id,spl.commission_1st_yr,spl.commission_2nd_yr
	from institution_salesperson_link spl
	inner join institutions i on i.id = spl.institution_id
	where spl.salesperson_id=@id
	and i.is_active='Y')
	order by i.name 

	select * from #tmp

	drop table #tmp
		
	set nocount off
end

GO
