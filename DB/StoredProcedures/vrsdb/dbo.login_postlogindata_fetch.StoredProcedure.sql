USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[login_postlogindata_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[login_postlogindata_fetch]
GO
/****** Object:  StoredProcedure [dbo].[login_postlogindata_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************  
*******************************************************  
** Version  : 1.0.0.0  
** Procedure    :  fetch post login data  
** Created By   : Pavel Guha  
** Created On   : 09/04/2019  
*******************************************************  
*******************************************************/  
-- exec login_postlogindata_fetch '3d517e0b-077c-49ca-9928-ede4e12eac48'  
CREATE procedure [dbo].[login_postlogindata_fetch]  
 @user_id uniqueidentifier  
as  
begin  
set nocount on  
   declare @user_role_id int,  
		   @user_role_code nvarchar(10),  
		   @institution_id uniqueidentifier,  
		   @billing_account_id uniqueidentifier,  
		   @APIVER nvarchar(200),  
		   @WS8SRVIP nvarchar(200),  
		   @WS8CLTIP nvarchar(200),  
		   @WS8SRVUID nvarchar(200),  
		   @WS8SRVPWD nvarchar(200),  
		   @WS8SessionID nvarchar(30),  
		   @RPTEGNURL nvarchar(200),  
		   @login_user_id uniqueidentifier,  
		   @radiologist_id uniqueidentifier,  
		   @session_creation_date datetime,  
		   @count int,  
		   @SCHCASVCENBL nchar(1),
		   @radiologistTimeZone nvarchar(100)=''
  
  
 --Unlock/Lock User  
 --if(select count(user_id) from sys_user_lock where user_id=@user_id )>0  
 -- begin  
     
 --  delete from  sys_user_lock  
 --  where user_id=@user_id   
 -- end  
    
 --delete from sys_record_lock where user_id=@user_id  
 --delete from sys_record_lock_ui where user_id=@user_id  
  
 --if(select count(user_id) from sys_user_lock where user_id=@user_id )=0  
 -- begin  
 --  insert into sys_user_lock([user_id],last_login)  
 --  values (@user_id,getdate())  
  
 -- end  
  
 select @user_role_id = u.user_role_id,  
        @user_role_code = ur.code  
 from users u  
 inner join user_roles ur on ur.id =u.user_role_id  
 where u.id = @user_id  
  
 if(@user_role_code = 'IU')  
  begin  
   select @institution_id = institution_id from institution_user_link where user_id=@user_id  
  end  
 else if(@user_role_code = 'AU')  
  begin  
   select @billing_account_id= id  
   from billing_account  
   where login_user_id = @user_id  
  end  
 else  
  begin  
   set @institution_id ='00000000-0000-0000-0000-000000000000'  
  end  
   
 if(@user_role_code = 'RDL')  
  begin  
   select @radiologist_id = id from radiologists where login_user_id = @user_id  
   --Radiologist time zone
	Select @radiologistTimeZone=tz.standard_name 
	from	radiologists rdgl with (nolock)
			inner join sys_us_time_zones tz with (nolock) on tz.id=rdgl.timezone_id
	where	rdgl.id=@radiologist_id
  end  
 else  
  begin  
   select @radiologist_id = '00000000-0000-0000-0000-000000000000'  
  end  
  
 select @APIVER       = data_type_string from general_settings where control_code ='APIVER'  
 select @WS8SRVIP     = data_type_string from general_settings where control_code ='WS8SRVIP'  
 select @WS8CLTIP     = data_type_string from general_settings where control_code ='WS8CLTIP'  
 select @WS8SRVUID    = data_type_string from general_settings where control_code ='WS8SRVUID'  
 select @WS8SRVPWD    = data_type_string from general_settings where control_code ='WS8SRVPWD'  
 select @RPTEGNURL    = data_type_string from general_settings where control_code ='RPTEGNURL'   
 select @SCHCASVCENBL = data_type_string from general_settings where control_code ='SCHCASVCENBL'  
  
 set @WS8SessionID=''  
  
 if(@APIVER = 8)  
  begin  
   --select @WS8SessionID = session_id  
   --from sys_ws8_session  
   --where convert(datetime,convert(datetime,date_created,106)) = convert(datetime,convert(datetime,getdate(),106))  
  
   select @WS8SessionID = session_id  
   from sys_ws8_session  
   where datediff(MI,date_created,getdate())<50  
  
   set @WS8SessionID = isnull(@WS8SessionID,'')  
  end  
    
    select user_role_id,  
           user_role_code=@user_role_code,  
           code,  
		   name,  
		   contact_no             = isnull(contact_no,''),  
		   allow_manual_submission,  
		   allow_dashboard_view,  
		   theme_pref             = isnull(theme_pref,'DEFAULT'),
		   institution_code       = isnull((select code from institutions where id =@institution_id),''),  
		   institution_name       = isnull((select name from institutions where id =@institution_id),'NA'),  
		   chat_url               = isnull((select data_type_string from general_settings where control_code='CHATURL'),''),  
		   enable_chat            = isnull((select data_type_string from general_settings where control_code='ENBLCHAT'),''),  
		   billing_account_id     = isnull(@billing_account_id,'00000000-0000-0000-0000-000000000000'),  
		   billing_account_name   = isnull((select name from billing_account where id=@billing_account_id),''),  
		   radiologist_id         = @radiologist_id,
		   session_id             = isnull(@WS8SessionID,''),  
		   default_time_zone_id   = isnull((select id from sys_us_time_zones where is_default='Y'),0),  
		   default_time_zone_name = isnull((select standard_name from sys_us_time_zones where is_default='Y'),''),  
		   APIVER                 = isnull(@APIVER,''),  
		   WS8SRVIP               = isnull(@WS8SRVIP,''),  
		   WS8CLTIP               = isnull(@WS8CLTIP,''),  
	       WS8SRVUID              = isnull(@WS8SRVUID,''),  
		   WS8SRVPWD              = isnull(@WS8SRVPWD,''),  
		   RPTEGNURL              = isnull(@RPTEGNURL,''),
		   radiologistTimeZone    = @radiologistTimeZone
 from users      
 where id=@user_id  
  
 create table #tmp  
 (  
  menu_id int,  
  menu_desc nvarchar(50),  
  parent_id int,  
  menu_level int,  
  nav_method nvarchar(5),  
  nav_url nvarchar(100),  
  is_browser nchar(1),  
  menu_icon nvarchar(50),  
  display_index int,  
  is_dropdown nchar(1),  
  show_rec_count nchar(1),  
  record_count int null default 0  
    
 )  
 create table #tmpInst  
 (  
    id uniqueidentifier  
 )  
 create table #tmpModality  
 (  
    id int  
 )  
 create table #tmpSpecies  
 (  
    id int  
 )  
 create table #tmpID(id uniqueidentifier)  
  
 insert into #tmp(menu_id,menu_desc,parent_id,menu_level,nav_method,nav_url,is_browser,menu_icon,display_index,is_dropdown,show_rec_count)  
 (select umr.menu_id,m.menu_desc,m.parent_id,m.menu_level,m.nav_method,m.nav_url,m.is_browser,m.menu_icon,m.display_index,m.is_dropdown,m.show_rec_count  
 from user_menu_rights umr  
 inner join sys_menu m on m.menu_id=umr.menu_id  
 where m.is_enabled='Y'  
 and m.is_dropdown='Y'  
 and umr.user_id = @user_id)  
 order by m.parent_id,m.display_index  
  
 insert into #tmpModality(id)  
 (select id from modality where is_active='Y')  
 order by name  
  
 insert into #tmpSpecies(id)  
 (select id from species where is_active='Y')  
  order by name  
  
 if(@user_role_code = 'SUPP' or @user_role_code = 'SYSADMIN' or @user_role_code='TRS')  
  begin  
      insert into #tmpInst(id)  
   (select id from institutions where is_active='Y'  
    union  
    select id='00000000-0000-0000-0000-000000000000')  
  end  
 else if(@user_role_code = 'IU')  
  begin  
   insert into #tmpInst(id)  
   (select id  
   from institutions   
   where is_active='Y'  
   and id in (select institution_id  
              from institution_user_link  
        where user_id = @user_id))  
   order by name  
  end  
 else if(@user_role_code = 'AU')  
  begin  
   insert into #tmpInst(id)  
   (select bail.institution_id  
   from billing_account_institution_link bail  
   inner join institutions i on i.id = bail.institution_id  
   inner join billing_account ba on ba.id = bail.billing_account_id  
   where i.is_active='Y'  
   and ba.login_user_id = @user_id)  
   order by i.name  
  end  
 else if(@user_role_code = 'RDL')  
  begin  
   if(select count(institution_id) from radiologist_functional_rights_exception_institution where radiologist_id = @radiologist_id)=0  
    begin  
      insert into #tmpInst(id)  
      (select id from institutions where is_active='Y')  
      order by name  
    end  
   else  
    begin  
      insert into #tmpInst(id)  
      (select id from institutions   
      where is_active='Y'   
      and id not in (select institution_id from radiologist_functional_rights_exception_institution  where radiologist_id = @radiologist_id))  
      order by name  
    end  
  
   delete from #tmpModality where id not in (select modality_id from radiologist_functional_rights_modality  where radiologist_id = @radiologist_id)  
   delete from #tmpSpecies where id not in (select species_id from radiologist_functional_rights_species  where radiologist_id = @radiologist_id)  
  end  
 else if(@user_role_code = 'SALES')  
  begin  
   insert into #tmpInst(id)  
   (select id  
   from institutions   
   where is_active='Y'  
   and id in (select institution_id  
              from institution_salesperson_link  
        where salesperson_user_id = @user_id))  
   order by name  
  end  
  
 if(select count(menu_id) from #tmp where menu_id=20)>0  
  begin  
   update #tmp  
   set record_count=isnull((select count(id)   
                            from study_hdr   
          where study_status=1   
          and study_status_pacs=0  
          and deleted='N'  
          and isnull(merge_status,'N') = 'N'  
          and institution_id in (select id from #tmpInst)),0)  
   where menu_id = 20  
  end  
   
 if(select count(menu_id) from #tmp where menu_id=21)>0  
  begin  
   if(@user_role_code = 'RDL')  
    begin  
     insert into #tmpID(id)  
     (select id  
      from study_hdr   
      where study_status=2   
      and study_status_pacs in (10,20,50,60)  
      and deleted='N'  
      and institution_id in (select id from #tmpInst)  
      and modality_id in (select id from #tmpModality)  
      and species_id in (select id from #tmpSpecies))  
  
     if(@SCHCASVCENBL='Y')  
      begin  
       if(select count(right_code) from radiologist_functional_rights_assigned where right_code='UPDFINALRPT' and radiologist_id=@radiologist_id)> 0  
        begin  
         if(select count(right_code) from radiologist_functional_rights_assigned where right_code='WRKCNSLTCASE' and radiologist_id=@radiologist_id)=0  
          begin  
           delete   
           from #tmpID  
           where id in (select id  
              from study_hdr  
              where study_status=2   
              and study_status_pacs in (10,20,50,60)  
              and deleted='N'  
              and institution_id in (select id from #tmpInst)  
              and modality_id in (select id from #tmpModality)  
              and species_id in (select id from #tmpSpecies)  
              and charindex('CONSULT',upper(isnull(service_codes,''))) > 0)  
          end  
  
         if(select count(other_radiologist_id) from radiologist_functional_rights_other_radiologist where radiologist_id = @radiologist_id)>0  
          begin  
           delete   
           from #tmpID  
           where id in (select id  
                from study_hdr   
                where study_status=2   
                and study_status_pacs in (10,20,50,60)  
                and deleted='N'  
                and institution_id in (select id from #tmpInst)  
                and modality_id in (select id from #tmpModality)  
                and species_id in (select id from #tmpSpecies)  
                and (isnull(radiologist_id,'00000000-0000-0000-0000-000000000000') not in (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = @radiologist_id union select other_radiologist_id='00000000-
0000-0000-0000-000000000000' union select other_radiologist_id=@radiologist_id)))  
                --and (isnull(final_radiologist_id,'00000000-0000-0000-0000-000000000000') in (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = @radiologist_id))))  
          end  
         else  
          begin  
           delete   
           from #tmpID  
           where id in (select id  
              from study_hdr   
              where study_status=2   
              and study_status_pacs in (10,20,50,60)  
              and deleted='N'  
              and institution_id in (select id from #tmpInst)  
              and modality_id in (select id from #tmpModality)  
              and species_id in (select id from #tmpSpecies)  
              and (isnull(radiologist_id,'00000000-0000-0000-0000-000000000000') not in ('00000000-0000-0000-0000-000000000000',@radiologist_id)))  
              --and (isnull(final_radiologist_id,'00000000-0000-0000-0000-000000000000') in (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = @radiologist_id))))  
          end  
  
         --if(select count(right_code) from radiologist_functional_rights_assigned where right_code='ACCLOCKSTUDY' and radiologist_id = @radiologist_id)=0  
         -- begin  
         --  delete   
         --  from #tmpID  
         --  where id in (select id  
         --      from study_hdr   
         --      where study_status=2   
         --      and study_status_pacs in (10,20,50,60)  
         --      and deleted='N'  
         --      and institution_id in (select id from #tmpInst)  
         --      and modality_id in (select id from #tmpModality)  
         --      and isnull(prelim_radiologist_id,'00000000-0000-0000-0000-000000000000') not in ('00000000-0000-0000-0000-000000000000',@radiologist_id)  
         --      and isnull(final_radiologist_id,'00000000-0000-0000-0000-000000000000') not in ('00000000-0000-0000-0000-000000000000',@radiologist_id))  
  
         -- end  
  
         if(select count(study_type_id) from radiologist_functional_rights_exception_study_type where radiologist_id = @radiologist_id)>0  
          begin  
           delete   
           from #tmpID  
           where id in (select id  
              from study_hdr  
              where study_status=2   
              and study_status_pacs in (10,20,50,60)  
              and deleted='N'  
              and institution_id in (select id from #tmpInst)  
              and modality_id in (select id from #tmpModality)  
              and species_id in (select id from #tmpSpecies)  
              and id in (select distinct hst.study_hdr_id from study_hdr_study_types hst   
                 inner join study_hdr sh on sh.id = hst.study_hdr_id  
                 where sh.study_status=2   
                 and hst.study_type_id in (select study_type_id   
                       from radiologist_functional_rights_exception_study_type   
                       where radiologist_id = @radiologist_id)))  
  
          end  
  
         if(select count(right_code) from radiologist_functional_rights_assigned where right_code='WRKCNSLTCASE' and radiologist_id=@radiologist_id)>0  
          begin  
           delete   
           from #tmpID  
           where id in (select id  
              from study_hdr  
              where study_status=2   
              and study_status_pacs in (10,20,50,60)  
              and deleted='N'  
              and institution_id in (select id from #tmpInst)  
              and modality_id in (select id from #tmpModality)  
              and species_id in (select id from #tmpSpecies)  
              and charindex('CONSULT',upper(isnull(service_codes,''))) <= 0)  
          end  
  
         
        end  
       else  
        begin  
         delete   
         from #tmpID  
         where id not in (select id  
             from study_hdr  
             where study_status=2   
             and study_status_pacs in (10,20,50,60)  
             and deleted='N'  
             and institution_id in (select id from #tmpInst)  
             and modality_id in (select id from #tmpModality)  
             and species_id in (select id from #tmpSpecies)  
             and radiologist_id = @radiologist_id)  
        end  
      end  
     else  
      begin  
       if(select count(right_code) from radiologist_functional_rights_assigned where right_code='WRKCNSLTCASE' and radiologist_id=@radiologist_id)=0  
        begin  
         delete   
         from #tmpID  
         where id in (select id  
            from study_hdr  
            where study_status=2   
            and study_status_pacs in (10,20,50,60)  
            and deleted='N'  
            and institution_id in (select id from #tmpInst)  
            and modality_id in (select id from #tmpModality)  
            and species_id in (select id from #tmpSpecies)  
            and charindex('CONSULT',upper(isnull(service_codes,''))) > 0)  
        end  
  
       if(select count(other_radiologist_id) from radiologist_functional_rights_other_radiologist where radiologist_id = @radiologist_id)>0  
        begin  
         delete   
         from #tmpID  
         where id in (select id                from study_hdr   
              where study_status=2   
              and study_status_pacs in (10,20,50,60)  
              and deleted='N'  
              and institution_id in (select id from #tmpInst)  
              and modality_id in (select id from #tmpModality)  
              and species_id in (select id from #tmpSpecies)  
              and (isnull(radiologist_id,'00000000-0000-0000-0000-000000000000') not in 
			  (select other_radiologist_id from radiologist_functional_rights_other_radiologist 
			  where radiologist_id = @radiologist_id union select other_radiologist_id='00000000-0000-0000-0000-000000000000' union select other_radiologist_id=@radiologist_id)))  
              --and (isnull(final_radiologist_id,'00000000-0000-0000-0000-000000000000') in (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = @radiologist_id))))  
        end  
       else  
        begin  
         delete   
         from #tmpID  
         where id in (select id  
            from study_hdr   
            where study_status=2   
            and study_status_pacs in (10,20,50,60)  
            and deleted='N'  
            and institution_id in (select id from #tmpInst)  
            and modality_id in (select id from #tmpModality)  
            and species_id in (select id from #tmpSpecies)  
            and (isnull(radiologist_id,'00000000-0000-0000-0000-000000000000') not in ('00000000-0000-0000-0000-000000000000',@radiologist_id)))  
            --and (isnull(final_radiologist_id,'00000000-0000-0000-0000-000000000000') in (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = @radiologist_id))))  
        end  
  
       if(select count(study_type_id) from radiologist_functional_rights_exception_study_type where radiologist_id = @radiologist_id)>0  
        begin  
         delete   
         from #tmpID  
         where id in (select id  
            from study_hdr  
            where study_status=2   
            and study_status_pacs in (10,20,50,60)  
            and deleted='N'  
            and institution_id in (select id from #tmpInst)  
            and modality_id in (select id from #tmpModality)  
            and species_id in (select id from #tmpSpecies)  
            and id in (select distinct hst.study_hdr_id from study_hdr_study_types hst   
               inner join study_hdr sh on sh.id = hst.study_hdr_id  
               where sh.study_status=2   
               and hst.study_type_id in (select study_type_id   
                     from radiologist_functional_rights_exception_study_type   
                     where radiologist_id = @radiologist_id)))  
  
        end  
  
       if(select count(right_code) from radiologist_functional_rights_assigned where right_code='WRKCNSLTCASE' and radiologist_id=@radiologist_id)>0  
        begin  
         delete   
         from #tmpID  
         where id in (select id  
            from study_hdr  
            where study_status=2   
            and study_status_pacs in (10,20,50,60)  
            and deleted='N'  
            and institution_id in (select id from #tmpInst)  
            and modality_id in (select id from #tmpModality)  
            and species_id in (select id from #tmpSpecies)  
            and charindex('CONSULT',upper(isnull(service_codes,''))) <= 0)  
        end  
      end  
  
     if(select count(right_code) from radiologist_functional_rights_assigned where right_code='ACCLOCKSTUDY' and radiologist_id=@radiologist_id)=0  
      begin  
       delete   
       from #tmpID  
       where id in (select l.record_id   
           from sys_record_lock_ui l  
           inner join study_hdr h  on h.id=l.record_id  
           where l.menu_id=21   
           and h.study_status=2   
           and h.study_status_pacs in (10,20,50,60)  
           and h.deleted='N'  
           and h.institution_id in (select id from #tmpInst)  
           and h.modality_id in (select id from #tmpModality)  
           and h.species_id in (select id from #tmpSpecies)  
           and l.user_id<>@user_id)  
      end  
       
     select @count = count(id) from #tmpID  
     update #tmp set record_count = @count where menu_id=21  
     truncate table  #tmpID  
    end  
   else if(@user_role_code = 'TRS')  
    begin  
     insert into #tmpID(id)  
     (select id  
      from study_hdr   
      where study_status=2   
      and study_status_pacs = 60  
      and deleted='N'  
      and institution_id in (select id from #tmpInst)  
      and modality_id in (select id from #tmpModality)  
      and species_id in (select id from #tmpSpecies)  
      and dict_tanscriptionist_id = '00000000-0000-0000-0000-000000000000'  
      and id in ((select id   
                  from study_hdr   
         where isnull(radiologist_id,'00000000-0000-0000-0000-000000000000') in (select id   
                                                                                 from radiologists   
                           where is_active='Y'   
                           and transcription_required='Y'))  
            union  
         (select id   
                  from study_hdr   
         where isnull(prelim_radiologist_id,'00000000-0000-0000-0000-000000000000') in (select id   
                                                                                        from radiologists   
                                  where is_active='Y'   
                                  and transcription_required='Y'))))  
  
      
     select @count = count(id) from #tmpID  
     update #tmp set record_count = @count where menu_id=21  
     truncate table  #tmpID  
    end  
   else  if(@user_role_code = 'AU' or @user_role_code = 'IU')  
    begin  
     update #tmp  
     set record_count=isnull((select count(id)   
            from study_hdr   
            where study_status_pacs in (10,20,50,60,100)  
            and deleted='N'  
            and final_rpt_released='N'  
            and institution_id in (select id from #tmpInst)),0)  
     where menu_id = 21  
    end  
   else  
    begin  
     update #tmp  
     set record_count=isnull((select count(id)   
            from study_hdr where study_status=2   
            and study_status_pacs in (10,20,50,60)  
            and deleted='N'  
            and institution_id in (select id from #tmpInst)),0)  
     where menu_id = 21  
    end  
  end  
  
 if(select count(menu_id) from #tmp where menu_id=22)>0  
  begin  
   if(@user_role_code = 'RDL')  
    begin  
     if(@SCHCASVCENBL='Y')  
      begin  
       if(select count(right_code) from radiologist_functional_rights_assigned where right_code='UPDFINALRPT' and radiologist_id=@radiologist_id)> 0  
        begin  
         insert into #tmpID(id)  
         (select id  
          from study_hdr   
          where study_status=3   
          and study_status_pacs = 80  
          and deleted='N'  
          and institution_id in (select id from #tmpInst)  
          and modality_id in (select id from #tmpModality)  
          and species_id in (select id from #tmpSpecies)  
          and (isnull(prelim_radiologist_id,'00000000-0000-0000-0000-000000000000') = @radiologist_id  
            or isnull(final_radiologist_id,'00000000-0000-0000-0000-000000000000')= @radiologist_id))  
  
         if(select count(right_code) from radiologist_functional_rights_assigned where right_code='WRKCNSLTCASE' and radiologist_id=@radiologist_id)=0  
          begin  
           delete   
           from #tmpID  
           where id in (select id  
              from study_hdr  
              where study_status=3   
              and study_status_pacs =80  
              and deleted='N'  
              and institution_id in (select id from #tmpInst)  
              and modality_id in (select id from #tmpModality)  
              and charindex('CONSULT',upper(isnull(service_codes,''))) > 0)  
          end  
  
         if(select count(other_radiologist_id) from radiologist_functional_rights_other_radiologist where radiologist_id = @radiologist_id)>0  
          begin  
           delete   
           from #tmpID  
           where id in (select id  
               from study_hdr   
               where study_status=3   
               and study_status_pacs = 80  
               and deleted='N'  
               and institution_id in (select id from #tmpInst)  
               and modality_id in (select id from #tmpModality)  
               and (isnull(prelim_radiologist_id,'00000000-0000-0000-0000-000000000000') not in (select other_radiologist_id   
                                   from radiologist_functional_rights_other_radiologist   
                                   where radiologist_id = @radiologist_id   
                                   union   
                                   select other_radiologist_id='00000000-0000-0000-0000-000000000000'   
                                   union   
                                   select other_radiologist_id=@radiologist_id)  
                or (isnull(final_radiologist_id,'00000000-0000-0000-0000-000000000000') not in (select other_radiologist_id   
                                    from radiologist_functional_rights_other_radiologist   
                                    where radiologist_id = @radiologist_id   
                                    union   
                                    select other_radiologist_id='00000000-0000-0000-0000-000000000000'   
                                    union   
                                    select other_radiologist_id=@radiologist_id))))  
          end  
  
         --if(select count(right_code) from radiologist_functional_rights_assigned where right_code='ACCLOCKSTUDY' and radiologist_id = @radiologist_id)=0  
          -- begin  
          --  delete   
          --  from #tmpID  
          --  where id in (select id  
          --      from study_hdr   
          --      where study_status=3   
          --      and study_status_pacs = 80  
          --      and deleted='N'  
          --      and institution_id in (select id from #tmpInst)  
          --      and modality_id in (select id from #tmpModality)  
          --      and isnull(prelim_radiologist_id,'00000000-0000-0000-0000-000000000000') not in ('00000000-0000-0000-0000-000000000000',@radiologist_id)  
          --      and isnull(final_radiologist_id,'00000000-0000-0000-0000-000000000000') not in ('00000000-0000-0000-0000-000000000000',@radiologist_id))  
          -- end  
  
         if(select count(study_type_id) from radiologist_functional_rights_exception_study_type where radiologist_id = @radiologist_id)>0  
          begin  
           delete   
           from #tmpID  
           where id in (select id  
              from study_hdr  
              where study_status=3   
              and study_status_pacs = 80  
              and deleted='N'  
              and institution_id in (select id from #tmpInst)  
              and modality_id in (select id from #tmpModality)  
              and id in (select distinct hst.study_hdr_id from study_hdr_study_types hst   
                 inner join study_hdr sh on sh.id = hst.study_hdr_id  
                 where sh.study_status=3   
                 and hst.study_type_id in (select study_type_id   
                       from radiologist_functional_rights_exception_study_type   
                       where radiologist_id = @radiologist_id)))  
  
          end  
        end   
       else  
        begin  
         insert into #tmpID(id)  
         (select id  
          from study_hdr   
          where study_status=3   
          and study_status_pacs = 80  
          and deleted='N'  
          and institution_id in (select id from #tmpInst)  
          and modality_id in (select id from #tmpModality)  
          and species_id in (select id from #tmpSpecies)  
          and (isnull(radiologist_id,'00000000-0000-0000-0000-000000000000') = @radiologist_id  
            or isnull(prelim_radiologist_id,'00000000-0000-0000-0000-000000000000')= @radiologist_id))  
        end   
      end  
     else  
      begin  
       insert into #tmpID(id)  
       (select id  
        from study_hdr   
        where study_status=3   
        and study_status_pacs = 80  
        and deleted='N'  
        and institution_id in (select id from #tmpInst)  
        and modality_id in (select id from #tmpModality)  
        and species_id in (select id from #tmpSpecies)  
        and (isnull(prelim_radiologist_id,'00000000-0000-0000-0000-000000000000') = @radiologist_id  
         or isnull(final_radiologist_id,'00000000-0000-0000-0000-000000000000')= @radiologist_id))  
  
       if(select count(right_code) from radiologist_functional_rights_assigned where right_code='WRKCNSLTCASE' and radiologist_id=@radiologist_id)=0  
        begin  
         delete   
         from #tmpID  
         where id in (select id  
            from study_hdr  
            where study_status=3   
            and study_status_pacs =80  
            and deleted='N'  
            and institution_id in (select id from #tmpInst)  
            and modality_id in (select id from #tmpModality)  
            and species_id in (select id from #tmpSpecies)  
            and charindex('CONSULT',upper(isnull(service_codes,''))) > 0)  
        end  
  
       if(select count(other_radiologist_id) from radiologist_functional_rights_other_radiologist where radiologist_id = @radiologist_id)>0  
        begin  
         delete   
         from #tmpID  
         where id in (select id  
             from study_hdr   
             where study_status=3   
             and study_status_pacs = 80  
             and deleted='N'  
             and institution_id in (select id from #tmpInst)  
             and modality_id in (select id from #tmpModality)  
             and species_id in (select id from #tmpSpecies)  
             and (isnull(prelim_radiologist_id,'00000000-0000-0000-0000-000000000000') not in (select other_radiologist_id   
                                 from radiologist_functional_rights_other_radiologist   
                                 where radiologist_id = @radiologist_id   
                                 union   
                                 select other_radiologist_id='00000000-0000-0000-0000-000000000000'   
                                 union   
                                 select other_radiologist_id=@radiologist_id)  
              or (isnull(final_radiologist_id,'00000000-0000-0000-0000-000000000000') not in (select other_radiologist_id   
                                  from radiologist_functional_rights_other_radiologist   
                                  where radiologist_id = @radiologist_id   
                                  union   
                                  select other_radiologist_id='00000000-0000-0000-0000-000000000000'   
                                  union   
                                  select other_radiologist_id=@radiologist_id))))  
        end  
   
       if(select count(study_type_id) from radiologist_functional_rights_exception_study_type where radiologist_id = @radiologist_id)>0  
        begin  
         delete   
         from #tmpID  
         where id in (select id  
            from study_hdr  
            where study_status=3   
            and study_status_pacs = 80  
            and deleted='N'  
            and institution_id in (select id from #tmpInst)  
            and modality_id in (select id from #tmpModality)  
            and species_id in (select id from #tmpSpecies)  
            and id in (select distinct hst.study_hdr_id from study_hdr_study_types hst   
               inner join study_hdr sh on sh.id = hst.study_hdr_id  
               where sh.study_status=3   
               and hst.study_type_id in (select study_type_id   
                     from radiologist_functional_rights_exception_study_type   
                     where radiologist_id = @radiologist_id)))  
  
        end  
      end  
       
     select @count = count(id) from #tmpID  
     update #tmp set record_count = @count where menu_id=22  
     truncate table  #tmpID  
    end  
   else  if(@user_role_code = 'AU' or @user_role_code = 'IU')  
    begin  
     update #tmp  
     set record_count=isnull((select count(id)   
            from study_hdr   
            where study_status_pacs in (80,100)  
            and deleted='N'  
            and final_rpt_released='N'  
            and institution_id in (select id from #tmpInst)),0)  
     where menu_id = 22  
    end  
   else  
    begin  
     update #tmp  
     set record_count=isnull((select count(id)   
            from study_hdr   
            where study_status=3   
            and study_status_pacs =80  
            and deleted='N'  
            and institution_id in (select id from #tmpInst)),0)  
     where menu_id = 22  
    end  
  end  
  
if(select count(menu_id) from #tmp where menu_id=23)>0  
  begin  
   if(@user_role_code = 'RDL')  
    begin  
  
     insert into #tmpID(id)  
     (select id  
      from study_hdr   
      where study_status=4   
      and study_status_pacs = 100  
      and deleted='N'  
      and institution_id in (select id from #tmpInst)  
      and modality_id in (select id from #tmpModality)  
      and species_id in (select id from #tmpSpecies))  
      --and (isnull(prelim_radiologist_id,'00000000-0000-0000-0000-000000000000') = @radiologist_id  
      --                    or isnull(final_radiologist_id,'00000000-0000-0000-0000-000000000000')= @radiologist_id))  
  
     if(select count(right_code) from radiologist_functional_rights_assigned where right_code='WRKCNSLTCASE' and radiologist_id=@radiologist_id)=0  
      begin  
       delete   
       from #tmpID  
       where id in (select id  
          from study_hdr  
          where study_status=4   
          and study_status_pacs =100  
          and deleted='N'  
          and institution_id in (select id from #tmpInst)  
          and modality_id in (select id from #tmpModality)  
          and species_id in (select id from #tmpSpecies)  
          and charindex('CONSULT',upper(isnull(service_codes,''))) > 0)  
      end  
  
     if(select count(other_radiologist_id) from radiologist_functional_rights_other_radiologist where radiologist_id = @radiologist_id)>0  
      begin  
       --   delete   
       --from #tmpID  
       --where id in (select id  
       --    from study_hdr   
       --    where study_status=4   
       --    and study_status_pacs = 100  
       --    and deleted='N'  
       --    and institution_id in (select id from #tmpInst)  
       --    and modality_id in (select id from #tmpModality)  
       --    and (isnull(radiologist_id,'00000000-0000-0000-0000-000000000000') not in (select other_radiologist_id   
       --                                                                                      from radiologist_functional_rights_other_radiologist   
       --                          where radiologist_id = @radiologist_id   
       --                          union   
       --                          select other_radiologist_id=@radiologist_id)  
       --         and (isnull(final_radiologist_id,'00000000-0000-0000-0000-000000000000') not in (select other_radiologist_id   
       --                          from radiologist_functional_rights_other_radiologist   
       --                          where radiologist_id = @radiologist_id   
       --                          union   
       --                          select other_radiologist_id=@radiologist_id))))  
  
       delete   
       from #tmpID  
       where id in (select id  
           from study_hdr   
           where study_status=4   
           and study_status_pacs = 100  
           and deleted='N'  
           and institution_id in (select id from #tmpInst)  
           and modality_id in (select id from #tmpModality)  
           and species_id in (select id from #tmpSpecies)  
           and (isnull(radiologist_id,'00000000-0000-0000-0000-000000000000') not in (select other_radiologist_id   
                                                                                             from radiologist_functional_rights_other_radiologist   
                                 where radiologist_id = @radiologist_id   
                                 union   
                                 select other_radiologist_id=@radiologist_id)))  
      end  
     else  
      begin  
  
       delete   
       from #tmpID  
       where id in (select id  
           from study_hdr   
           where study_status=4   
           and study_status_pacs = 100  
           and deleted='N'  
           and institution_id in (select id from #tmpInst)  
           and modality_id in (select id from #tmpModality)  
           and species_id in (select id from #tmpSpecies)  
           and isnull(radiologist_id,'00000000-0000-0000-0000-000000000000')<> @radiologist_id)  
                  
      end  
  
     --if(select count(right_code) from radiologist_functional_rights_assigned where right_code='ACCLOCKSTUDY' and radiologist_id = @radiologist_id)=0  
     -- begin  
     --  delete   
     --  from #tmpID  
     --  where id in (select id  
     --      from study_hdr   
     --      where study_status=3   
     --      and study_status_pacs = 80  
     --      and deleted='N'  
     --      and institution_id in (select id from #tmpInst)  
     --      and modality_id in (select id from #tmpModality)  
     --      and isnull(final_radiologist_id,'00000000-0000-0000-0000-000000000000') not in ('00000000-0000-0000-0000-000000000000',@radiologist_id))  
     -- end  
  
     if(select count(study_type_id) from radiologist_functional_rights_exception_study_type where radiologist_id = @radiologist_id)>0  
      begin  
       delete   
       from #tmpID  
       where id in (select id  
          from study_hdr  
          where study_status=4   
          and study_status_pacs = 100  
          and deleted='N'  
          and institution_id in (select id from #tmpInst)  
          and modality_id in (select id from #tmpModality)  
          and species_id in (select id from #tmpSpecies)  
          and id in (select distinct hst.study_hdr_id from study_hdr_study_types hst   
             inner join study_hdr sh on sh.id = hst.study_hdr_id  
             where sh.study_status=4   
             and hst.study_type_id in (select study_type_id   
                   from radiologist_functional_rights_exception_study_type   
                   where radiologist_id = @radiologist_id)))  
  
      end  
  
     if(select count(right_code) from radiologist_functional_rights_assigned where right_code='WRKCNSLTCASE' and radiologist_id=@radiologist_id)>0  
      begin  
       delete   
       from #tmpID  
       where id in (select id  
          from study_hdr  
          where study_status=4   
          and study_status_pacs =100  
          and deleted='N'  
          and institution_id in (select id from #tmpInst)  
          and modality_id in (select id from #tmpModality)  
          and species_id in (select id from #tmpSpecies)  
          and charindex('CONSULT',upper(isnull(service_codes,''))) <= 0)  
      end  
  
     select @count = count(id) from #tmpID  
     update #tmp set record_count = @count where menu_id=23  
     truncate table  #tmpID  
    end  
   else if(@user_role_code = 'AU' or @user_role_code='IU')  
    begin  
     update #tmp  
     set record_count=isnull((select count(id)   
            from study_hdr   
            where study_status=4   
            and study_status_pacs =100  
            and final_rpt_released='Y'  
            and deleted='N'  
            and institution_id in (select id from #tmpInst)),0)  
     where menu_id = 23  
    end  
   else  
    begin  
     update #tmp  
     set record_count=isnull((select count(id)   
            from study_hdr   
            where study_status=4   
            and study_status_pacs =100  
            and deleted='N'  
            and institution_id in (select id from #tmpInst)),0)  
     where menu_id = 23  
    end  
  end  
  
  --if(select count(menu_id) from #tmp where menu_id=34)>0  
  --begin  
  -- update #tmp  
  -- set record_count=isnull((select count(study_uid)   
  --                          from scheduler_file_downloads   
  --        where file_count > file_xfer_count  
  --        and institution_id in (select id from #tmpInst)),0)  
  -- where menu_id = 34  
  --end  
  
 if(select count(menu_id) from #tmp where menu_id=37)>0  
  begin  
   update #tmp  
   set record_count=isnull((select count(id)   
                            from scheduler_img_file_downloads_ungrouped   
          where grouped ='N'  
          and is_stored='N'  
          and institution_id in (select id from #tmpInst)),0)  
   where menu_id = 37  
  end  
  
 select * from #tmp  
 select pacs_user_id,pacs_password from users where id=@user_id  
  
 select * from sys_dashboard_settings where parent_id=0 and is_enabled='Y'    
  
  
 drop table #tmp  
 drop table #tmpInst  
 drop table #tmpModality  
 drop table #tmpSpecies  
 drop table #tmpID  
  
 set nocount off  
   
end  
GO
