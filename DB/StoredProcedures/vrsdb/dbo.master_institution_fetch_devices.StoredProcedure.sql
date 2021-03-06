USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_devices]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_institution_fetch_devices]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_devices]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_institution_fetch_devices : fetch institution devices
** Created By   : Pavel Guha
** Created On   : 24/04/2019
*******************************************************/
--exec master_institution_fetch_devices 'faded955-300e-4517-8f6c-e6513b1889f6'
CREATE procedure [dbo].[master_institution_fetch_devices]
    @id uniqueidentifier
as
begin
	 set nocount on
	
	create table #tmp
	(
		rec_id int identity(1,1),
		device_id uniqueidentifier,
		manufacturer nvarchar(200),
		modality nvarchar(50),
		modality_ae_title nvarchar(50),
		weight_uom nvarchar(10), -- Added on 2nd SEP 2019 @BK
		del nvarchar(1) default ''
	)

	insert into #tmp(device_id,manufacturer,modality,modality_ae_title,weight_uom)
	(select device_id,manufacturer,modality,modality_ae_title,weight_uom
	from institution_device_link
	where institution_id=@id)
	order by manufacturer,modality,modality_ae_title

	select * from #tmp

	drop table #tmp
		
	set nocount off
end

GO
