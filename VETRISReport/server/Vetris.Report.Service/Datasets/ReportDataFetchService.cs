using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Vetris.Report.Core.Extensions;
using Vetris.Report.Core.Models.Metadatas;
using Vetris.Report.Core.Models.Queries;
using Vetris.Report.Core.Session;
using Vetris.Report.DataAccess;

namespace Vetris.Report.Service.Datasets
{
    public class ReportDataFetchService : ApplicationService, IReportDataFetchService
    {

        public ReportDataFetchService(IDatabaseContext database, ISessionInfo session):base(database, session)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Object name</param>
        /// <returns></returns>
        public dynamic FetchData(string objectname)
        {
            _db.OpenConnection();
            var ds = _db.SQL($"select * from [{objectname}]").ExecuteDataSet();

            _db.CloseConnection();
            return ds.Tables[0].TableToDictionary();
        }

        public dynamic FetchData(QueryDto input)
        {
            _db.OpenConnection();
            var db = _db.QueryFactory();
            var query = db.Query(input.ObjectName);
            //query = query.Select(input.Columns.Select(i => i.Column).ToArray());
            if (input.Columns != null)
            {
                foreach (var c in input.Columns)
                {
                    if (c.Type == "string")
                    {
                        if (c.Operator == "eq") query = query.Where(c.Column, c.Value1.StringValue);
                        if (c.Operator == "neq") query = query.WhereNot(c.Column, c.Value1.StringValue);
                        if (c.Operator == "like") query = query.WhereLike(c.Column, c.Value1.StringValue);
                        if (c.Operator == "notlike") query = query.WhereNotLike(c.Column, c.Value1.StringValue);
                    }
                    if (c.Type == "number" && c.Value1 != null && c.Value1.NumberValue.HasValue)
                    {
                        if (c.Operator == "eq") query = query.Where(c.Column, c.Value1.NumberValue);
                        if (c.Operator == "neq") query = query.WhereNot(c.Column, c.Value1.NumberValue);
                        if (c.Operator == "gt") query = query.Where(c.Column, ">", c.Value1.NumberValue);
                        if (c.Operator == "lt") query = query.Where(c.Column, "<", c.Value1.NumberValue);
                        if (c.Operator == "gte") query = query.Where(c.Column, ">=", c.Value1.NumberValue);
                        if (c.Operator == "lte") query = query.Where(c.Column, "<=", c.Value1.NumberValue);

                        if (c.Value2 != null && c.Value2.NumberValue.HasValue)
                        {
                            if (c.Operator == "between") query = query.WhereBetween(c.Column, c.Value1.NumberValue, c.Value2.NumberValue);
                        }

                    }
                    if (c.Type == "Date" && c.Value1 != null && c.Value1.DateValue.HasValue)
                    {
                        if (c.Operator == "eq") query = query.WhereDate(c.Column, c.Value1.DateValue);
                        if (c.Operator == "neq") query = query.WhereNotDate(c.Column, c.Value1.DateValue);
                        if (c.Operator == "gt") query = query.WhereDate(c.Column, ">", c.Value1.DateValue);
                        if (c.Operator == "lt") query = query.WhereDate(c.Column, "<", c.Value1.DateValue);
                        if (c.Operator == "gte") query = query.WhereDate(c.Column, ">=", c.Value1.DateValue);
                        if (c.Operator == "lte") query = query.WhereDate(c.Column, "<=", c.Value1.DateValue);
                        if (c.Value2 != null && c.Value2.DateValue.HasValue)
                        {
                            if (c.Operator == "between") query = query.WhereBetween(c.Column, c.Value1.DateValue, c.Value2.DateValue);
                        }
                    }


                    if (c.Type == "boolean" && c.Value1 != null && c.Value1.BooleanValue.HasValue)
                    {
                        if (c.Operator == "eq") query = query.Where(c.Column, c.Value1.BooleanValue);
                        if (c.Operator == "neq") query = query.WhereNot(c.Column, c.Value1.BooleanValue);
                    }

                }
                var ascending_cols = input.Columns.Where(i => i.SortDirection == "asc").Select(i => i.Column).ToArray();
                var descending_cols = input.Columns.Where(i => i.SortDirection == "desc").Select(i => i.Column).ToArray();

                if (ascending_cols.Length > 0) query = query.OrderBy(ascending_cols);
                if (descending_cols.Length > 0) query = query.OrderByDesc(descending_cols);
            }
            if (input.PageSize > 0 && input.PageNo > 0)
            {
                var pq = query.Paginate(input.PageNo.Value, input.PageSize.Value);

                return new { 
                    TotalRecords=pq.Count,
                    pq.TotalPages,
                    CurrentPage=pq.Page,
                    pq.HasNext,
                    pq.HasPrevious,
                    Items = pq.List
                };

            }
            else
            {
                var data = query.Get();

                return data;
            }
            
        }

        public DataSet GetReportData(string sql, Dictionary<string, object> parameters)
        {
            try
            {
                var ds = new DataSet();
                _db.OpenConnection();
                var q = _db.SQL(sql);
                if (parameters!=null && parameters.Count > 0)
                {
                    foreach (var p in parameters)
                    {
                        if(!p.Key.StartsWith("@") && sql.Contains($"@{p.Key}"))
                            q.AddParameter($"@{p.Key}", p.Value);
                        else if(p.Key.StartsWith("@") && sql.Contains(p.Key))
                            q.AddParameter(p.Key, p.Value);
                    }
                }
                ds = q.ExecuteDataSet();
                _db.CloseConnection();
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _db.CloseConnection();
            }
            
        } 
    }
}
