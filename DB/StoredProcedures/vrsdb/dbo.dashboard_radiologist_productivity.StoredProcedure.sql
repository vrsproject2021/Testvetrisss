USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[dashboard_radiologist_productivity]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[dashboard_radiologist_productivity]
GO
/****** Object:  StoredProcedure [dbo].[dashboard_radiologist_productivity]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec dashboard_radiologist_productivity 'A56E8711-F0C9-42DC-9995-53421AE1B240','EE7A48A7-2CB5-4F6D-97EE-7B6B1021C46A'
CREATE PROCEDURE [dbo].[dashboard_radiologist_productivity]    
    @radiologist_id uniqueidentifier,    
    @user_id uniqueidentifier    
AS    
BEGIN    
    SELECT modality,    
           sum(assigned_count) assigned_count,    
           sum(work_progress_count) work_progress_count,    
           sum(today_count) today_count,    
           sum(this_month_count) this_month_count,    
           sum(last_month_count) last_month_count,    
           sum(this_year_count) this_year_count    
    FROM    
    (    
        SELECT mm.[name] modality,    ---assigned
               count(1) assigned_count,    
               0 work_progress_count,    
               0 today_count,    
               0 this_month_count,    
               0 last_month_count,    
               0 this_year_count    
        FROM study_hdr h with (nolock)    
        INNER JOIN modality mm with (nolock)  ON mm.id = h.modality_id   
        WHERE h.radiologist_id = @radiologist_id  
		 and h.id in (select study_hdr_id from study_hdr_dictated_reports where created_by=@user_id and convert(date,date_created)= convert(date,getdate())
		               union
					   select study_hdr_id from study_hdr_prelim_reports where created_by=@user_id and convert(date,date_created)= convert(date,getdate())
					   union
					   select study_hdr_id from study_hdr_final_reports where created_by=@user_id and convert(date,date_created)= convert(date,getdate())
					   union
					   select study_hdr_id=id from study_hdr where radiologist_id=@radiologist_id and study_status_pacs = 50)

        GROUP BY mm.[name]    
        UNION    
        SELECT m.name modality,   ---working on
               0 assigned_count,    
               count(1) work_progress_count,    
               0 today_count,    
               0 this_month_count,    
               0 last_month_count,    
               0 this_year_count    
        FROM study_hdr h with (nolock)    
            INNER JOIN modality m with (nolock)  ON m.id = h.modality_id    
            INNER JOIN sys_record_lock_ui lui with (nolock)    ON lui.record_id = h.id    
        WHERE lui.[user_id] = @user_id    
        GROUP BY m.name   
        UNION    
        SELECT m.[name] modality,  --today  
               0 assigned_count,    
               0 work_progress_count,    
               count(1) today,    
               0 this_month_count,    
               0 last_month_count,    
               0 this_year_count    
        FROM [dbo].study_hdr stu with (nolock)    
            INNER JOIN modality m with (nolock)  ON m.id = stu.modality_id    
        --WHERE convert(date, synched_on) = convert(date, GETDATE())    
              Where (    
                      stu.radiologist_id = @radiologist_id    
                      or stu.prelim_radiologist_id = @radiologist_id    
                      or stu.final_radiologist_id = @radiologist_id    
                  )    
			and stu.id in (select study_hdr_id from study_hdr_dictated_reports where created_by=@user_id and convert(date,date_created)= convert(date,getdate())
		               union
					   select study_hdr_id from study_hdr_prelim_reports where created_by=@user_id and convert(date,date_created)= convert(date,getdate())
					   union
					   select study_hdr_id from study_hdr_final_reports where created_by=@user_id and convert(date,date_created)= convert(date,getdate()))
        GROUP BY m.[name]    
        UNION   
		SELECT t.modality, -- this month   
               0 assigned_count,    
               0 work_progress_count,    
               0 today,    
               count(t.id) this_month_count,    
               0 last_month_count,    
               0 this_year_count    
		from 
		(select stu.id,modality = m.name
		from [dbo].[vw_studies] stu with (nolock) 
		INNER JOIN modality m with (nolock) ON m.id = stu.modality_id     
		WHERE convert(date, report_final_on) BETWEEN convert(date, DATEADD(DAY,(DATEPART(DAY,getdate())-1)*(-1),getdate())) AND convert(date,getdate()-1)  
		AND (    
                stu.dict_radiologist_id = @radiologist_id    
                or stu.prelim_radiologist_id = @radiologist_id    
                or stu.final_radiologist_id = @radiologist_id    
            ) 
	    union
		select stu.id,modality=m.name
		from [dbo].[study_hdr] stu with (nolock) 
		INNER JOIN modality m with (nolock) ON m.id = stu.modality_id     
		where (    
                stu.dict_radiologist_id = @radiologist_id    
                or stu.prelim_radiologist_id = @radiologist_id    
                or stu.final_radiologist_id = @radiologist_id    
              ) 
		and stu.id in (select study_hdr_id from study_hdr_dictated_reports where created_by=@user_id and convert(date,date_created)= convert(date,getdate())
		               union
					   select study_hdr_id from study_hdr_prelim_reports where created_by=@user_id and convert(date,date_created)= convert(date,getdate())
					   union
					   select study_hdr_id from study_hdr_final_reports where created_by=@user_id and convert(date,date_created)= convert(date,getdate()))) t
		group by t.modality 
        UNION    
        SELECT m.[name] modality,--last month    
               0 assigned_count,    
               0 work_progress_count,    
               0 today,    
               0 this_month_count,    
               count(1) last_month_count,    
               0 this_year_count    
        FROM [dbo].[vw_studies] stu with (nolock)    
            INNER JOIN modality m with (nolock)    
                ON m.id = stu.modality_id    
      --  WHERE convert(date, report_final_on)    
      --        BETWEEN	CONVERT(date, dateadd(d, - (day(dateadd(m, -1, getdate() - 2))), dateadd(m, -1, getdate() - 1))) 
						--AND CONVERT(date,dateadd( d, - (day(getdate())),getdate()))    
      --        AND (    
      --                stu.dict_radiologist_id = @radiologist_id    
      --                or stu.prelim_radiologist_id = @radiologist_id    
      --                or stu.final_radiologist_id = @radiologist_id    
      --            )   
	    where stu.id in  (select id from vw_studies where dict_radiologist_id = @radiologist_id and  convert(date, report_dictated_on)  BETWEEN CONVERT(date, dateadd(d, - (day(dateadd(m, -1, getdate() - 2))), dateadd(m, -1, getdate() - 1))) AND CONVERT(date,dateadd( d, - (day(getdate())),getdate()))
		                  union
						  select id from vw_studies where prelim_radiologist_id = @radiologist_id and convert(date, report_prelim_on)  BETWEEN CONVERT(date, dateadd(d, - (day(dateadd(m, -1, getdate() - 2))), dateadd(m, -1, getdate() - 1))) AND CONVERT(date,dateadd( d, - (day(getdate())),getdate()))
						  union
						  select id from vw_studies where final_radiologist_id = @radiologist_id and convert(date, report_final_on)  BETWEEN CONVERT(date, dateadd(d, - (day(dateadd(m, -1, getdate() - 2))), dateadd(m, -1, getdate() - 1))) AND CONVERT(date,dateadd( d, - (day(getdate())),getdate())))
        GROUP BY m.[name]    
        UNION  
		SELECT t.modality, -- this year   
               0 assigned_count,    
               0 work_progress_count,    
               0 today,    
               0 this_month_count,    
               0 last_month_count,    
               count(t.id) this_year_count   
		from 
		(select stu.id,modality = m.name
		from [dbo].[vw_studies] stu with (nolock) 
		INNER JOIN modality m with (nolock) ON m.id = stu.modality_id     
		--WHERE convert(date, report_final_on) BETWEEN convert(date, DATEADD(DAY,(DATEPART(DAY,getdate())-1)*(-1),getdate())) AND convert(date,getdate()-1)  
		where stu.id in  (select id from vw_studies where dict_radiologist_id = @radiologist_id and year(report_dictated_on) = year(GETDATE()) and datediff(day,isnull(report_dictated_on,'01jan1900'),getdate())>0 
		                  union
						  select id from vw_studies where prelim_radiologist_id = @radiologist_id and year(report_prelim_on) = year(GETDATE()) and datediff(day,isnull(report_prelim_on,'01jan1900'),getdate())>0  
						  union
						  select id from vw_studies where final_radiologist_id = @radiologist_id and year(report_final_on) = year(GETDATE()) and datediff(day,isnull(report_final_on,'01jan1900'),getdate())>0)
		
	    union
		select stu.id,modality=m.name
		from [dbo].[study_hdr] stu with (nolock) 
		INNER JOIN modality m with (nolock) ON m.id = stu.modality_id     
		where (    
                stu.dict_radiologist_id = @radiologist_id    
                or stu.prelim_radiologist_id = @radiologist_id    
                or stu.final_radiologist_id = @radiologist_id    
              ) 
		and stu.id in (select study_hdr_id from study_hdr_dictated_reports where created_by=@user_id and convert(date,date_created)= convert(date,getdate())
		               union
					   select study_hdr_id from study_hdr_prelim_reports where created_by=@user_id and convert(date,date_created)= convert(date,getdate())
					   union
					   select study_hdr_id from study_hdr_final_reports where created_by=@user_id and convert(date,date_created)= convert(date,getdate()))) t
		group by t.modality   


        --SELECT m.[name] modality,    
        --       0 assigned_count,    
        --       0 work_progress_count,    
        --       0 today,    
        --       0 this_month_count,    
        --       0 last_month_count,    
        --       count(1) this_year_count    
        --FROM [dbo].[vw_studies] stu with (nolock)    
        --    INNER JOIN modality m with (nolock)    
        --        ON m.id = stu.modality_id    
        --WHERE year(report_final_on) = year(GETDATE())    
        --      AND (    
        --              stu.dict_radiologist_id = @radiologist_id    
        --              or stu.prelim_radiologist_id = @radiologist_id    
        --              or stu.final_radiologist_id = @radiologist_id    
        --          )    
        --GROUP BY m.[name]    
    ) g    
    GROUP BY modality    
END 
GO
