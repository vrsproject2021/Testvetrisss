USE [vrslogdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_day_end_log_db_process]    Script Date: 20-08-2021 20:44:27 ******/
DROP PROCEDURE [dbo].[scheduler_day_end_log_db_process]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_day_end_log_db_process]    Script Date: 20-08-2021 20:44:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_day_end_log_db_process : Process Day End Data
** Created By   : Pavel Guha
** Created On   : 05/06/2021
*******************************************************/
--exec scheduler_day_end_log_db_process
CREATE procedure [dbo].[scheduler_day_end_log_db_process]
as
begin
	declare @file_id int

	select @file_id = file_id
	from sys.database_files
	where name ='vrslogdb_log'
	
	DBCC SHRINKFILE (@file_id, TRUNCATEONLY); 
end
GO
