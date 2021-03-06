USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[dashboard_open_case]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[dashboard_open_case]
GO
/****** Object:  StoredProcedure [dbo].[dashboard_open_case]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************              
*******************************************************              
** Version  : 1.0.0.0              
** Procedure    : dashboard_report_modality_wise_open_case_normal            
** Created By   : AM              
** Created On   : 09/06/2021              
*******************************************************/             
--exec dashboard_open_case 2          
CREATE procedure [dbo].[dashboard_open_case]            
 @mnu_id int           
as       
           
 begin           
 --modality_wise_open_case_normal          
	select m.name,m.code,COUNT(*) as modality_count     
	from study_hdr h with(nolock)            
		inner join modality m with(nolock) on m.id=h.modality_id            
		inner join sys_priority pr on pr.priority_id=h.priority_id            
	where h.study_status_pacs<100 and             
		m.is_active='Y' and             
		h.deleted='N' and             
		pr.is_stat='N'       
	group by m.name,m.code          
       
 --status_wise_open_case_normal          
	
	select             
	case	WHEN h.study_status_pacs=50 then 'Read'             
			WHEN h.study_status_pacs=60 THEN 'Dictated'            
			WHEN h.study_status_pacs=80 THEN 'Preliminary' 
	end as	study_status_pacs,            
	count(h.study_status_pacs) as count_status     
	from	study_hdr h with(nolock)            
		inner join modality m with(nolock) on m.id=h.modality_id            
		inner join sys_priority pr on pr.priority_id=h.priority_id            
	where	(h.study_status_pacs<100 and h.study_status_pacs>0)     
		and m.is_active='Y'      
		and h.deleted='N'     
		and pr.is_stat='N'       
	group by h.study_status_pacs
	union
	select   case	WHEN h.study_status_pacs=0 THEN 'Unviewed' end as	study_status_pacs,            
			count(h.study_status_pacs) as count_status     
	from	study_hdr h with(nolock)            
			inner join modality m with(nolock) on m.id=h.modality_id            
	where	(h.study_status_pacs<100 and h.study_status_pacs<=0)     
			and m.is_active='Y'      
			and h.deleted='N'       
	group by h.study_status_pacs
          
  --elapsed_time_open_case_normal          
	declare @slot_count int, @slot_1 int,@slot_2 int,@slot_3 int=0       
            
	select @slot_count=slot_count, @slot_1=slot_1,@slot_2=slot_2,@slot_3=slot_3             
	from sys_dashboard_settings_aging     
	where dashboard_menu_id=@mnu_id     
		and lower([key])='normal';              
            
	 declare @qry nvarchar(max)            
	 if (@slot_count=2)              
		 begin              
			 set @qry='            
			 select diff_in_minute,slotTotal,COUNT(diff_in_minute) as time_count from(            
				select Case When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_1)+' then ''<= '+CONVERT(varchar(10), @slot_1)+' ''            
						When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_1)+' and             
						DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_2)+'  then ''> '+CONVERT(varchar(10), @slot_1)+' <= '+CONVERT(varchar(10), @slot_2)+'''            
					Else ''0'' End diff_in_minute,  
					Case When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_1)+' then '+CONVERT(varchar(10), @slot_1)+'            
						When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_1)+' and             
						DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_2)+'  then '+CONVERT(varchar(10),@slot_1+@slot_2)+'          
					Else ''0'' End slotTotal  
			 from study_hdr h with(nolock)            
			 inner join modality m with(nolock) on m.id=h.modality_id            
			 inner join sys_priority pr on pr.priority_id=h.priority_id            
			 where h.study_status_pacs<100 and m.is_active=''Y'' and h.deleted=''N'' and pr.is_stat=''N'')            
			 As elapsed_time            
			 where diff_in_minute<>''0''            
			 group by diff_in_minute,slotTotal order by slotTotal'               
		 end              
	 if (@slot_count=3)              
		 begin              
			 set @qry='            
				 select diff_in_minute,slotTotal,COUNT(diff_in_minute) as time_count from(            
					 select Case When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_1)+' then ''<= '+CONVERT(varchar(10), @slot_1)+' ''      
							When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_1)+' and             
								DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_2)+'  then ''> '+CONVERT(varchar(10), @slot_1)+' <= '+CONVERT(varchar(10), @slot_2)+'''            
							When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_2)+' and             
								DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_3)+'  then ''> '+CONVERT(varchar(10), @slot_2)+' <= '+CONVERT(varchar(10), @slot_3)+'''            
							Else ''0'' End diff_in_minute,  
							Case	When	DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_1)+' then '+CONVERT(varchar(10), @slot_1)+'            
									When	DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_1)+' and             
											DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_2)+'  then '+CONVERT(varchar(10),@slot_1+@slot_2)+'          
									When	DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_2)+' and             
											DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_3)+'  then '+CONVERT(varchar(10),@slot_2+@slot_3)+'  
							Else ''0'' End slotTotal  
					from	study_hdr h with(nolock)            
							inner join modality m with(nolock) on m.id=h.modality_id            
							inner join sys_priority pr on pr.priority_id=h.priority_id            
					where h.study_status_pacs<100 and m.is_active=''Y'' and h.deleted=''N'' and pr.is_stat=''N'') As elapsed_time            
				where diff_in_minute<>''0''            
				group by diff_in_minute,slotTotal order by slotTotal'               
		 end              
	 if (@slot_count=4)              
		 begin              
			 set @qry='            
				 select diff_in_minute,slotTotal,COUNT(diff_in_minute) as time_count from(            
					select	Case	When	DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_1)+' then ''<= '+CONVERT(varchar(10), @slot_1)+' ''            
									When	DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_1)+' and             
											DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_2)+'  then ''> '+CONVERT(varchar(10), @slot_1)+' <= '+CONVERT(varchar(10), @slot_2)+'''            
									When	DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_2)+' and             
											DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_3)+'  then ''> '+CONVERT(varchar(10), @slot_2)+' <= '+CONVERT(varchar(10), @slot_3)+'''            
									When	DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_3)+' then ''> '+CONVERT(varchar(10), @slot_3)+'''            
							Else ''0'' End  diff_in_minute ,  
							Case	When	DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_1)+' then '+CONVERT(varchar(10), @slot_1)+'            
									When	DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_1)+' and             
											DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_2)+'  then '+CONVERT(varchar(10),@slot_1+@slot_2)+'          
									When	DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_2)+' and             
											DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_3)+'  then '+CONVERT(varchar(10),@slot_2+@slot_3)+'  
									When	DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_3)+' then '+CONVERT(varchar(10),@slot_3+@slot_3)+'  
							Else ''0'' End slotTotal from study_hdr h with(nolock)            
							inner join modality m with(nolock) on m.id=h.modality_id            
							inner join sys_priority pr on pr.priority_id=h.priority_id       
					where h.study_status_pacs<100 and m.is_active=''Y'' and h.deleted=''N'' and pr.is_stat=''N'') As elapsed_time            
				where	diff_in_minute<>''0''            
				group	by diff_in_minute,slotTotal  
				order	by slotTotal'      
		 end            
 exec sp_executesql @qry           
          
 --modality_wise_open_case_stat          
	select	m.name,m.code,COUNT(*) as modality_count from study_hdr h with(nolock)            
			inner join modality m with(nolock) on m.id=h.modality_id            
			inner join sys_priority pr on pr.priority_id=h.priority_id            
	where	h.study_status_pacs<100 and             
			m.is_active='Y' and             
			h.deleted='N' and             
			pr.is_stat='Y'    
	group by m.name,m.code            
          
--status_wise_open_case_stat 
	select	case	WHEN h.study_status_pacs=50 then 'Read'             
					WHEN h.study_status_pacs=60 THEN 'Dictated'            
					WHEN h.study_status_pacs=80 THEN 'Preliminary' 
			end as	study_status_pacs,            
			count(h.study_status_pacs) as count_status     
	from	study_hdr h with(nolock)            
			inner join modality m with(nolock) on m.id=h.modality_id            
			inner join sys_priority pr on pr.priority_id=h.priority_id            
	where	(h.study_status_pacs<100 and h.study_status_pacs>0)     
			and m.is_active='Y'      
			and h.deleted='N'     
			and pr.is_stat='Y'       
	group by h.study_status_pacs
 --elapsed_time_open_case_stat          
	select @slot_count=slot_count, @slot_1=slot_1,@slot_2=slot_2,@slot_3=slot_3           
	from sys_dashboard_settings_aging where dashboard_menu_id=@mnu_id and lower([key])='stat';              
           
 if (@slot_count=2)              
 begin              
 set @qry='            
 select diff_in_minute,slotTotal,COUNT(diff_in_minute) as time_count from(            
 select Case When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_1)+' then ''<= '+CONVERT(varchar(10), @slot_1)+' ''            
     When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_1)+' and             
       DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_2)+'  then ''> '+CONVERT(varchar(10), @slot_1)+' <= '+CONVERT(varchar(10), @slot_2)+'''            
   Else ''0'' End  diff_in_minute,  
 Case When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_1)+' then '+CONVERT(varchar(10), @slot_1)+'            
   When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_1)+' and             
   DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_2)+'  then '+CONVERT(varchar(10),@slot_1+@slot_2)+'          
 Else ''0'' End slotTotal  
   from study_hdr h with(nolock)            
 inner join modality m with(nolock) on m.id=h.modality_id            
 inner join sys_priority pr on pr.priority_id=h.priority_id            
 where h.study_status_pacs<100 and m.is_active=''Y'' and h.deleted=''N'' and pr.is_stat=''Y'')            
 As elapsed_time          
 where diff_in_minute<>''0''            
 group by diff_in_minute,slotTotal order by slotTotal'               
 end              
 if (@slot_count=3)              
 begin              
 set @qry='            
 select diff_in_minute,slotTotal,COUNT(diff_in_minute) as time_count from(            
 select Case When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_1)+' then ''<= '+CONVERT(varchar(10), @slot_1)+' ''            
     When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_1)+' and             
       DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_2)+'  then ''> '+CONVERT(varchar(10), @slot_1)+' <= '+CONVERT(varchar(10), @slot_2)+'''            
     When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_2)+' and             
     DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_3)+'  then ''> '+CONVERT(varchar(10), @slot_2)+' <= '+CONVERT(varchar(10), @slot_3)+'''            
   Else ''0'' End diff_in_minute ,  
  Case When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_1)+' then '+CONVERT(varchar(10), @slot_1)+'            
     When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_1)+' and             
       DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_2)+'  then '+CONVERT(varchar(10),@slot_1+@slot_2)+'          
     When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_2)+' and             
     DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_3)+'  then '+CONVERT(varchar(10),@slot_2+@slot_3)+'  
   Else ''0'' End slotTotal   
   from study_hdr h with(nolock)            
 inner join modality m with(nolock) on m.id=h.modality_id            
 inner join sys_priority pr on pr.priority_id=h.priority_id            
 where h.study_status_pacs<100 and m.is_active=''Y'' and h.deleted=''N'' and pr.is_stat=''Y'')            
 As elapsed_time            
 where diff_in_minute<>''0''            
 group by diff_in_minute,slotTotal order by slotTotal '               
 end              
 if (@slot_count=4)              
 begin              
 set @qry='            
 select diff_in_minute,slotTotal,COUNT(diff_in_minute) as time_count from(            
 select Case When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_1)+' then ''<= '+CONVERT(varchar(10), @slot_1)+' ''            
     When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_1)+' and             
       DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_2)+'  then ''> '+CONVERT(varchar(10), @slot_1)+' <= '+CONVERT(varchar(10), @slot_2)+'''            
     When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_2)+' and             
       DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_3)+'  then ''> '+CONVERT(varchar(10), @slot_2)+' <= '+CONVERT(varchar(10), @slot_3)+'''            
     When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_3)+' then ''> '+CONVERT(varchar(10), @slot_3)+'''            
   Else ''0'' End diff_in_minute ,  
  Case When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_1)+' then '+CONVERT(varchar(10), @slot_1)+'            
     When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_1)+' and             
       DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_2)+'  then '+CONVERT(varchar(10),@slot_1+@slot_2)+'          
     When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_2)+' and             
     DATEDIFF(minute, h.synched_on, h.status_last_updated_on)<='+CONVERT(varchar(10), @slot_3)+'  then '+CONVERT(varchar(10),@slot_2+@slot_3)+'  
  When DATEDIFF(minute, h.synched_on, h.status_last_updated_on)>'+CONVERT(varchar(10), @slot_3)+' then '+CONVERT(varchar(10),@slot_3+@slot_3)+'  
   Else ''0'' End slotTotal   
   from study_hdr h with(nolock)            
 inner join modality m with(nolock) on m.id=h.modality_id            
 inner join sys_priority pr on pr.priority_id=h.priority_id            
 where h.study_status_pacs<100 and m.is_active=''Y'' and h.deleted=''N'' and pr.is_stat=''Y'')            
 As elapsed_time            
             
 group by diff_in_minute,slotTotal order by slotTotal'               
 end              
 exec sp_executesql @qry          
 end
GO
