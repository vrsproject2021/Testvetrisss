USE [vrsdb]
GO
/****** Object:  View [dbo].[studies_view]    Script Date: 20-08-2021 20:59:58 ******/
DROP VIEW [dbo].[studies_view]
GO
/****** Object:  View [dbo].[studies_view]    Script Date: 20-08-2021 20:59:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[studies_view] AS SELECT 
       a.study_uid, a.received_date, a.patient_name, b.name AS modality, n.name category, 
       d.name AS institution, 
       c.name AS physician, 
       h.name AS radiologist, m.status_desc AS study_status, usr.name approved_by, 
       a.study_date, 
       i.name AS prelim_radiologist, 
       k.name AS final_radiologist, a.rpt_approve_date, a.finishing_datetime finish_date, 
       a.manually_assigned, j.name AS dict_radiologist, a.transcription_finishing_datetime,
       a.accession_no,a.patient_id_pacs
FROM  dbo.study_hdr AS a INNER JOIN
       dbo.modality AS b ON a.modality_id = b.id LEFT OUTER JOIN
	   dbo.study_hdr_final_reports fr ON a.id = fr.study_hdr_id LEFT OUTER JOIN
	   dbo.users usr ON fr.created_by = usr.id LEFT OUTER JOIN
       dbo.physicians AS c ON a.physician_id = c.id LEFT OUTER JOIN
       dbo.institutions AS d ON a.institution_id = d.id LEFT OUTER JOIN
       dbo.species AS e ON a.species_id = e.id LEFT OUTER JOIN
       dbo.body_part AS f ON a.body_part_id = f.id LEFT OUTER JOIN
       dbo.body_part AS g ON a.body_part_id = g.id LEFT OUTER JOIN
       dbo.radiologists AS h ON a.radiologist_id = h.id LEFT OUTER JOIN
       dbo.radiologists AS i ON a.prelim_radiologist_id = i.id LEFT OUTER JOIN
       dbo.radiologists AS j ON a.dict_radiologist_id = j.id LEFT OUTER JOIN
       dbo.radiologists AS k ON a.final_radiologist_id = k.id LEFT OUTER JOIN
       dbo.breed AS l ON a.breed_id = l.id LEFT OUTER JOIN
       dbo.sys_study_status AS m ON a.study_status = m.status_id LEFT OUTER JOIN
       dbo.sys_study_category AS n ON a.category_id = n.id;
GO
