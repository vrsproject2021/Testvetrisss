USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[dashboard_new_case_hourly_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[dashboard_new_case_hourly_fetch]
GO
/****** Object:  StoredProcedure [dbo].[dashboard_new_case_hourly_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec dashboard_new_case_hourly_fetch '2021-07-30','2021-07-30',0
CREATE Proc [dbo].[dashboard_new_case_hourly_fetch]
    @fromDate datetime,
    @toDate datetime,          
    @modality_id int = null
as
Begin
    select modality,
           ISNULL([0], 0) AS [00-01],
           ISNULL([1], 0) AS [01-02],
           ISNULL([2], 0) AS [02-03],
           ISNULL([3], 0) AS [03-04],
           ISNULL([4], 0) AS [04-05],
           ISNULL([5], 0) AS [05-06],
           ISNULL([6], 0) AS [06-07],
           ISNULL([7], 0) AS [07-08],
           ISNULL([8], 0) AS [08-09],
           ISNULL([9], 0) AS [09-10],
           ISNULL([10], 0) AS [10-11],
           ISNULL([11], 0) AS [11-12],
           ISNULL([12], 0) AS [12-13],
           ISNULL([13], 0) AS [13-14],
           ISNULL([14], 0) AS [14-15],
           ISNULL([15], 0) AS [15-16],
           ISNULL([16], 0) AS [16-17],
           ISNULL([17], 0) AS [17-18],
           ISNULL([18], 0) AS [18-19],
           ISNULL([19], 0) AS [19-20],
           ISNULL([20], 0) AS [20-21],
           ISNULL([21], 0) AS [21-22],
           ISNULL([22], 0) AS [22-23],
           ISNULL([23], 0) AS [23-00]
    from
    (
        select m.code modality,
               DATEPART(HOUR, synched_on) [hour],
               count(1) [count]
        from (
		select id, priority_id, synched_on, modality_id from study_hdr h1 with (nolock)
		union
		select id, priority_id, synched_on, modality_id from vw_studies h2 with (nolock)) h

            inner join modality m with (nolock)
                on m.id = h.modality_id
        where CONVERT(date, synched_on)
              between CONVERT(date, @fromDate) and CONVERT(date, @toDate)
              and modality_id = case
                                    when ISNULL(@modality_id, 0) = 0 then
                                        modality_id
                                    else
                                        @modality_id
                                end
        group by m.code,
                 DATEPART(HOUR, synched_on)
    ) a
    pivot
    (
        sum([count])
        for [hour] in ([0], [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [14], [15], [16],
                       [17], [18], [19], [20], [21], [22], [23]
                      )
    ) as b
End
GO
