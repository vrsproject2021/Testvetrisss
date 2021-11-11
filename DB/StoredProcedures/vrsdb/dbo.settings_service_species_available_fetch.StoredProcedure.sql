USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_service_species_available_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_service_species_available_fetch]
GO
/****** Object:  StoredProcedure [dbo].[settings_service_species_available_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_service_species_avaiable_fetch : 
                  fetch normal hours species wise service availability
** Created By   : Pavel Guha 
** Created On   : 09/07/2021
*******************************************************/
--exec settings_service_species_avaiable_fetch 9,'11111111-1111-1111-1111-111111111111','',0
create Procedure [dbo].[settings_service_species_available_fetch]
    @menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
As
	Begin
		set nocount on
	    create table #tmpService
		(
		  rec_id int identity(1,1),
		  id int,
		  name nvarchar(50) null,
		)

		create table #tmpspecies
		(
		  rec_id int identity(1,1),
		  service_id int,
		  service_name nvarchar(50) null,
		  species_id int,
		  species_name nvarchar(30) null,
		  available nchar(1) null default 'N',
		  message_display nvarchar(500) null default ''
		)

		declare @rc int,
		        @ctr int,
				@service_id int,
				@service_name nvarchar(50)


		delete from sys_record_lock where user_id = @user_id
	    delete from sys_record_lock_ui where user_id = @user_id

		insert into #tmpService(id,name)
		(select id,name from  services where is_active='Y') order by name

		select @rc = @@rowcount,@ctr=1

		insert into #tmpspecies(service_id,service_name,species_id,species_name,available,message_display)
		(select spa.service_id,service_name = s.name,spa.species_id,species_name= sp.name,
		       spa.available,message_display=isnull(spa.message_display,'') 
		from settings_service_species_available spa
		inner join services s on s.id = spa.service_id
		inner join species sp on sp.id = spa.species_id)
		order by s.name,sp.name

		while(@ctr <= @rc)
			begin
				select @service_id = id,@service_name = name from #tmpService where rec_id=@ctr

				if(select count(service_id) from #tmpspecies where service_id=@service_id)=0
					begin
						insert into #tmpspecies(service_id,service_name,species_id,species_name)
						(select @service_id,@service_name,id,name
						from species
						where is_active='Y')
					end
				else
					begin
						insert into #tmpspecies(service_id,service_name,species_id,species_name)
						(select @service_id,@service_name,id,name
						from species
						where is_active='Y'
						and id not in (select species_id from #tmpspecies where service_id=@service_id))
					end

				set @ctr = @ctr + 1
			end

		if(select count(record_id) from sys_record_lock where record_id=@menu_id and menu_id=@menu_id)=0
			begin
				exec common_lock_record
					@menu_id       = @menu_id,
					@record_id     = @menu_id,
					@user_id       = @user_id,
					@error_code    = @error_code output,
					@return_status = @return_status output	
						
				if(@return_status=0)
					begin
						return 0
					end
			end

		select * from #tmpService
		select * from #tmpspecies order by service_name,species_name

		set nocount off
	End
GO
