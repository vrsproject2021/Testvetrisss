using Newtonsoft.Json;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Vetris.Report.Core.Configurations;
using Vetris.Report.Core.Extensions;
using Vetris.Report.Core.Models;
using Vetris.Report.Core.Models.Reports;
using Vetris.Report.Core.Session;
using Vetris.Report.DataAccess;
using Vetris.Reporting.Library.ReportClasses;

namespace Vetris.Report.Service.Reports
{
    public class ReportingService : ApplicationService, IReportingService
    {
        private readonly IAppFolders AppFolders;
        public ReportingService(IDatabaseContext database, ISessionInfo session, IAppFolders folders) : base(database, session)
        {
            AppFolders = folders;
        }

        public async Task<PreviewPreparedArg> PreparePreviewArguments(ReportPreviewGeneratingArg arg)
        {
            var report = JsonConvert.DeserializeObject<Reporting.Library.ReportClasses.Report>(arg.Report);
            DataSet ds = null;

            if (report.DataSets != null && report.DataSets.DataSet != null && report.DataSets.DataSet.Count > 0)
                ds = report.DataSets.DataSet.FirstOrDefault();

            string commandText = null;
            if (ds != null && ds.Query != null && ds.Query != null)
                commandText = ds.Query.CommandText;

            List<ReportParameter> parameters = new List<ReportParameter>();
            if (report.ReportParameters != null && report.ReportParameters.ReportParameter != null && report.ReportParameters.ReportParameter.Count > 0)
            {
                parameters = report.ReportParameters.ReportParameter;
            }

            var body = report.GetBody();
            Tablix table = null;

            if (body.ReportItems.Tablix != null && body.ReportItems.Tablix.Count > 0)
            {
                table = body.ReportItems.Tablix.FirstOrDefault();

            }
            List<Filter> filters = new List<Filter>();
            if (table != null && table.Filters != null && table.Filters.Filter != null && table.Filters.Filter.Count > 0)
            {
                filters = table.Filters.Filter;
                table.Filters = null;
            }

            string token = Guid.NewGuid().ToString("N");
            string filename = $"{token}.rptx";
            filename = Path.Combine(AppFolders.TempReportsFolder, filename);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(report.ToXML().ToString());
            using (XmlTextWriter writer = new XmlTextWriter(filename, System.Text.Encoding.UTF8))
            {
                writer.Formatting = System.Xml.Formatting.Indented;
                doc.Save(writer);
            }
            await Task.FromResult(0);


            var result = new PreviewPreparedArg() { DatasetName = ds?.Name, RDLC = filename, CommandText = commandText, Filters = new List<FilterInput>(), Parameters = new List<ParameterInput>() };
            // process parameters and filters 
            if (parameters.Count > 0)
            {
                foreach (var p in parameters)
                {
                    var par = new ParameterInput() { ParameterName = p.Name };
                    if (!p.InputValue.IsNullOrEmpty())
                    {
                        if (p.DataType == "DateTime")
                        {
                            if (p.InputValue.ToString().Contains("T"))
                                par.Value = p.InputValue.Substring(0, p.InputValue.IndexOf("T"));
                            else
                                par.Value = p.InputValue;

                            if (par.Value != null)
                            {
                                par.Value = par.Value.ToString().ToDateTime();
                                par.PassedValue = par.Value.ToString();
                            }
                        }
                        else if (p.DataType == "Boolean")
                        {
                            par.Value = p.InputValue.ToLower() == "true";
                            par.PassedValue = p.InputValue;
                        }
                        else if (p.DataType == "Float")
                        {
                            par.Value = Convert.ToDecimal(p.InputValue);
                            par.PassedValue = p.InputValue;
                        }
                        else if (p.DataType == "Integer")
                        {
                            par.Value = Convert.ToInt32(p.InputValue);
                            par.PassedValue = p.InputValue;
                        }
                        else {
                            par.Value = p.InputValue;
                            par.PassedValue = p.InputValue;
                        }
                    }
                    result.Parameters.Add(par);

                }
            }
            if (filters.Count > 0)
            {
                foreach (var f in filters)
                {
                    var filter = new FilterInput() { Operator = f.Operator };
                    if (!f.FilterExpression.IsNullOrEmpty())
                    {
                        filter.Column = f.FilterExpression.RDLCFieldName();
                    }
                    if (f.FilterValues != null && f.FilterValues.FilterValue != null && f.FilterValues.FilterValue.Count > 0)
                    {
                        if (!f.FilterValues.FilterValue[0].IsNullOrEmpty())
                            filter.ParameterName1 = f.FilterValues.FilterValue[0].RDLCFieldName();

                        if (f.FilterValues.FilterValue.Count > 1 && !f.FilterValues.FilterValue[1].IsNullOrEmpty())
                            filter.ParameterName2 = f.FilterValues.FilterValue[1].RDLCFieldName();
                    }
                    result.Filters.Add(filter);

                }
            }
            ModifyQuery(result);
            // validate 

            return result;
        }

        public async Task<PreviewPreparedArg> PrepareReportArguments(Guid id, List<ParameterInput> reportparams)
        {
            _db.OpenConnection();
            var reportString = (await _db.QueryFactory()
                        .Query("SysReports")
                        .Where("Id", id)
                        .Select("Report")
                        .GetAsync<string>())
                        .FirstOrDefault();
            _db.CloseConnection();
            if (reportString.IsNullOrEmpty()) throw new Exception("Invalid report definition");
            var report = JsonConvert.DeserializeObject<Reporting.Library.ReportClasses.Report>(reportString);
            DataSet ds = null;

            if (report.DataSets != null && report.DataSets.DataSet != null && report.DataSets.DataSet.Count > 0)
                ds = report.DataSets.DataSet.FirstOrDefault();

            string commandText = null;
            if (ds != null && ds.Query != null && ds.Query != null)
                commandText = ds.Query.CommandText;

            List<ReportParameter> parameters = new List<ReportParameter>();
            if (report.ReportParameters != null && report.ReportParameters.ReportParameter != null && report.ReportParameters.ReportParameter.Count > 0)
            {
                parameters = report.ReportParameters.ReportParameter;
            }

            var body = report.GetBody();
            Tablix table = null;

            if (body.ReportItems.Tablix != null && body.ReportItems.Tablix.Count > 0)
            {
                table = body.ReportItems.Tablix.FirstOrDefault();

            }
            List<Filter> filters = new List<Filter>();
            if (table != null && table.Filters != null && table.Filters.Filter != null && table.Filters.Filter.Count > 0)
            {
                filters = table.Filters.Filter;
                table.Filters = null;
            }

            // create temporary report file (.rptx)
            string token = Guid.NewGuid().ToString("N");
            string filename = $"{token}.rptx";
            filename = Path.Combine(AppFolders.TempReportsFolder, filename);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(report.ToXML().ToString());
            using (XmlTextWriter writer = new XmlTextWriter(filename, System.Text.Encoding.UTF8))
            {
                writer.Formatting = System.Xml.Formatting.Indented;
                doc.Save(writer);
            }
            await Task.FromResult(0);


            var result = new PreviewPreparedArg() { DatasetName = ds?.Name, RDLC = filename, CommandText = commandText, Filters = new List<FilterInput>(), Parameters = new List<ParameterInput>() };
            // process parameters and filters 
            if (parameters.Count > 0)
            {
                if (reportparams == null) throw new Exception($"Parameters was not supplied.");
                foreach (var p in parameters)
                {
                    var par = new ParameterInput() { ParameterName = p.Name };
                    var supplied = reportparams.FirstOrDefault(i => i.ParameterName == p.Name);
                    if (supplied == null) throw new Exception($"Parameter @{p.Name} was not supplied."); 
                    if (!p.InputValue.IsNullOrEmpty())
                    {
                        if (p.DataType == "DateTime")
                        {
                            par.Value = supplied.Value;
                            if (par.Value != null)
                            {
                                if(par.Value.ToString().Contains("T"))
                                    par.Value = par.Value.ToString().Substring(0,par.Value.ToString().IndexOf("T"));
                                par.Value = par.Value.ToString().ToDateTime();
                                par.PassedValue = par.Value.ToString();
                            }
                        }
                        else if (p.DataType == "Boolean" || p.DataType == "Float" || p.DataType == "Integer")
                        {
                            par.Value = supplied.Value;
                            par.PassedValue = supplied.Value?.ToString();
                        }
                        else
                        {
                            par.Value = supplied.Value?.ToString();
                            par.PassedValue = supplied.Value?.ToString();
                        }
                    }
                    result.Parameters.Add(par);

                }
            }
            if (filters.Count > 0)
            {
                foreach (var f in filters)
                {
                    var filter = new FilterInput() { Operator = f.Operator };
                    if (!f.FilterExpression.IsNullOrEmpty())
                    {
                        filter.Column = f.FilterExpression.RDLCFieldName();
                    }
                    if (f.FilterValues != null && f.FilterValues.FilterValue != null && f.FilterValues.FilterValue.Count > 0)
                    {
                        if (!f.FilterValues.FilterValue[0].IsNullOrEmpty())
                            filter.ParameterName1 = f.FilterValues.FilterValue[0].RDLCFieldName();

                        if (f.FilterValues.FilterValue.Count > 1 && !f.FilterValues.FilterValue[1].IsNullOrEmpty())
                            filter.ParameterName2 = f.FilterValues.FilterValue[1].RDLCFieldName();
                    }
                    result.Filters.Add(filter);

                }
            }
            ModifyQuery(result);
           

            return result;
        }

        private void ModifyQuery(PreviewPreparedArg arg)
        {
            if (arg.Filters.Count == 0 || arg.CommandText.IsNullOrEmpty()) return;
            var commandText = $"SELECT * FROM ({arg.CommandText}) QQ WHERE ";
            var conditions = new List<string>();
            foreach (var filter in arg.Filters)
            {
                if (filter.Operator == "Between")
                {
                    conditions.Add($"({filter.Column} BETWEEN @{filter.ParameterName1} AND @{filter.ParameterName2})");
                }
                if (filter.Operator == "In")
                {
                    conditions.Add($"({filter.Column} IN (@{filter.ParameterName1}))");
                }
                if (filter.Operator == "Like")
                {
                    conditions.Add($"({filter.Column} LIKE @{filter.ParameterName1})");
                }
                if (filter.Operator == "Equal")
                {
                    conditions.Add($"({filter.Column} = @{filter.ParameterName1})");
                }
                if (filter.Operator == "NotEqual")
                {
                    conditions.Add($"({filter.Column} <> @{filter.ParameterName1})");
                }

                if (filter.Operator == "LessThan")
                {
                    conditions.Add($"({filter.Column} < @{filter.ParameterName1})");
                }
                if (filter.Operator == "LessThanOrEqual")
                {
                    conditions.Add($"({filter.Column} <= @{filter.ParameterName1})");
                }
                if (filter.Operator == "GreaterThan")
                {
                    conditions.Add($"({filter.Column} > @{filter.ParameterName1})");
                }
                if (filter.Operator == "GreaterThanOrEqual")
                {
                    conditions.Add($"({filter.Column} >= @{filter.ParameterName1})");
                }
            }

            arg.CommandText = commandText + String.Join(" AND ", conditions);

        }


        public async Task Create(SysReports input)
        {
            try
            {
                _db.OpenConnection();
                ValidateCreation(input);
                input.Draft = true;
                input.CreatedBy = SessionInfo.UserId;
                input.CreatedOn = DateTime.Now;
                _db.BeginTransaction();

                var factory = _db.QueryFactory();
                var ret = await factory.Query("SysReports")
                    .InsertAsync(new { 
                        Id=input.Id,
                        Name=input.Name,
                        Category=input.Category,
                        Draft=input.Draft,
                        Report=input.Report,
                        CreatedBy=input.CreatedBy,
                        CreatedOn=input.CreatedOn
                    }, _db.Transaction);
                _db.Commit();
            }
            catch (Exception e)
            {
                _db.Rollback();

                throw e;
            }
            finally
            {
                _db.Dispose();
            }
        }
        public async Task Save(SysReportSaveModelDto input)
        {
            try
            {
                _db.OpenConnection();
                
                var existing=ValidateSave(input);
                existing.Category = input.Category;
                existing.Draft = input.Draft;
                existing.Report = input.Report;
                existing.LastModifiedBy = SessionInfo.UserId;
                existing.LastModifiedOn = DateTime.Now;

                _db.BeginTransaction();
                var factory = _db.QueryFactory();
                var ret = await factory.Query("SysReports")
                    .Where("Id", existing.Id)
                    .UpdateAsync(new
                    {
                        Category= existing.Category,
                        Draft = existing.Draft,
                        Report = existing.Report,
                        LastModifiedBy= existing.LastModifiedBy,
                        LastModifiedOn=existing.LastModifiedOn
                    }, _db.Transaction);
                _db.Commit();
            }
            catch (Exception e)
            {
                _db.Rollback();

                throw e;
            }
            finally
            {
                _db.Dispose();
            }
        }

        public async Task<List<SysReportListModelDto>> GetAll(ReportListingQueryArg input)
        {
            _db.OpenConnection();
            var q = _db.QueryFactory()
                    .Query("SysReports as a")
                    .LeftJoin("users as b", "b.id", "a.CreatedBy")
                    .LeftJoin("users as c", "c.id", "a.LastModifiedBy");
            if (!input.Category.IsNullOrEmpty())
            {
                q = q.Where("a.Category", input.Category);
            }
            if (!input.Search.IsNullOrEmpty())
            {
                q = q.WhereLike("a.Name", $"%{input.Search}%");
            }
            if (input.SortDirection == "desc")
                q = q.OrderByDesc("a.Name");
            else
                q = q.OrderBy("a.Name");
            var data = (await q.Select(
                    "a.{Id,Name,Category,CreatedBy,CreatedOn,LastModifiedBy,LastModifiedOn,Draft}",
                    "b.{name as CreatedByUser}",
                    "b.{name as LastModifiedByUser}"
                )
                .GetAsync<SysReportListModelDto>()).ToList();
                
            return data.ToList();
        }
        public async Task<dynamic> GetCategories()
        {
            _db.OpenConnection();
            var data = (await _db.QueryFactory()
                    .Query("SysReports")
                    .Select("Category as category")
                    .SelectRaw("COUNT(1) as count")
                    .GroupBy("Category")
                    .OrderBy("Category")
                    .GetAsync())
                    .ToList();
            return data;
        } 

        public async Task<dynamic> GetForExecute(Guid Id)
        {
            _db.OpenConnection();
            var data = (await _db.QueryFactory()
                   .Query("SysReports")
                   .Where("Id", Id)
                   .Select("Id", "Name", "Category", "Draft", "Report as JsonReport")
                   .GetAsync<SysReportGetTempEditModelDto>())
                   .FirstOrDefault();
            var report = JsonConvert.DeserializeObject<Reporting.Library.ReportClasses.Report>(data.JsonReport);
            List<ReportParameter> parameters = null;
            if (report.ReportParameters != null && report.ReportParameters.ReportParameter != null && report.ReportParameters.ReportParameter.Count > 0)
            {
                parameters = report.ReportParameters.ReportParameter;
            }
            if (parameters != null)
            {
                var paramlist = parameters.Select(i => new { i.Prompt, ParameterName = i.Name, i.DataType, i.Nullable }).ToList();
                return new
                {
                    Id,
                    ReportName = data.Name,
                    Parameters = paramlist
                };
            }

            return new
            {
                Id,
                ReportName = data.Name
            };
        }
        
        public async Task<SysReportGetEditModelDto> GetForEdit(Guid Id)
        {
            _db.OpenConnection();
            var data = (await _db.QueryFactory()
                    .Query("SysReports")
                    .Where("Id",Id)
                    .Select("Id", "Name", "Category", "Draft", "Report as JsonReport")
                    .GetAsync<SysReportGetTempEditModelDto>())
                    .First();
            return data.ToGetEditModel();
        }
        private void ValidateCreation(SysReports input)
        {
            var data = _db.QueryFactory().Query("SysReports")
                .Where("Name", input.Name).Get<SysReports>().FirstOrDefault();
            if (data != null)
            {
                throw new Exception("Report name already taken");
            }
        }
        private SysReports ValidateSave(SysReportSaveModelDto input)
        {
            var data = _db.QueryFactory().Query("SysReports")
                .Where("Id", input.Id).Get<SysReports>().FirstOrDefault();
            if (data == null)
            {
                throw new Exception("Report not found");
            }
            return data;
        }
    }
}
