USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[hk_missing_study_check]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[hk_missing_study_check]
GO
/****** Object:  StoredProcedure [dbo].[hk_missing_study_check]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : hk_missing_study_check : check missing studies
** Created By   : Pavel Guha
** Created On   : 14/08/2019
*******************************************************/
CREATE procedure [dbo].[hk_missing_study_check]
    @xml_data ntext
as
begin
	
	set nocount on
	declare @hDoc int,
		    @counter bigint,
	        @rowcount bigint,
			@count1 int,
			@count2 int,
	        @study_uid nvarchar(100),
			@received_date datetime,
			@institution_name nvarchar(100),
			@patient_name nvarchar(100),
			@status_id int,
			@status_desc nvarchar(30)

	create table #tmp
	(
		id bigint identity(1,1),
		study_uid nvarchar(100),
		received_date datetime,
		institution_name nvarchar(100),
		patient_name nvarchar(100),
		status_id int,
		status_desc nvarchar(30) null default '',
		synch nvarchar(1) null default ''
	)


	
	exec sp_xml_preparedocument @hDoc output,@xml_data

	set @counter = 1
	select  @rowcount=count(row_id)  
	from openxml(@hDoc,'data/row', 2)  
	with( row_id bigint )

	while(@counter <= @rowcount)
		begin
			select  @study_uid        = study_uid,
					@received_date    = received_date ,
					@institution_name = institution_name,
					@patient_name     = patient_name,
					@status_id        = status_id
			from openxml(@hDoc,'data/row',2)
			with
			( 
				study_uid nvarchar(100),
				received_date datetime,
				institution_name nvarchar(100),
				patient_name nvarchar(100),
				status_id int,
				row_id bigint
			) xmlTemp where xmlTemp.row_id = @counter  
			
			
			
						
			if(select count(study_uid) from study_synch_dump where study_uid=@study_uid)=0
				begin
					if(@status_id > 0)
						begin
							insert into #tmp(study_uid,received_date,institution_name,patient_name,status_id)
							          values(@study_uid,@received_date,@institution_name,@patient_name,@status_id)
						end
					
				end
			else 
				begin
					select @count1 = count(study_uid) from study_hdr where study_uid=@study_uid
					select @count2 = count(study_uid) from study_hdr_archive where study_uid=@study_uid

					if(@count1=0 and @count2=0)
						begin
							delete from study_synch_dump where study_uid=@study_uid
							insert into #tmp(study_uid,received_date,institution_name,patient_name,status_id)
							          values(@study_uid,@received_date,@institution_name,@patient_name,@status_id)
						end
				end
			
			

			set @counter = @counter + 1
		end


	exec sp_xml_removedocument @hDoc

	update #tmp set status_desc = (select status_desc from sys_study_status_pacs where status_id = #tmp.status_id)


	select * from #tmp order by received_date 

	drop table #tmp
	set nocount off


end


GO
