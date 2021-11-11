using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Vetris.Reporting.Library.ReportClasses;
using Vetris.Report.Core.Extensions;
using Vetris.Report.Core.Models;
using Vetris.Report.Core.Models.Metadatas;
using Vetris.Report.Core.Models.Queries;
using Vetris.Report.Core.Session;
using Vetris.Report.DataAccess;
using Vetris.Report.Core.Models.Reports;

namespace Vetris.Report.Service.Datasets
{
    public class ReportMetadataService : ApplicationService, IReportMetadataService
    {
        public ReportMetadataService(IDatabaseContext database, ISessionInfo session) : base(database,session)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Object name</param>
        /// <returns></returns>
        public List<MetaDataJson> Columns(Guid id)
        {
            _db.OpenConnection();
            var db = _db.QueryFactory();
            var query = db.Query("data_sets")
                          .Where("id", id);
            var data = query.Get<DataSetsModel>().FirstOrDefault();
            if (data != null)
            {
                return data.ToOutputDto().Metadata;
            }
            return null;
        }

        public bool CreateOrModifyView(CreateViewInputDto input)
        {
            _db.OpenConnection();

            if (ValidateViewBody(input))
            {
                if(input.Id!=null)
                 _db.SQL(
                    $"DROP VIEW [{input.OldObjectName}];")
                    .ExecuteNonQuery();

                var result = _db.SQL(
                            $"CREATE VIEW [{input.ObjectName}] AS " +
                            $"{input.BodyText};")
                            .ExecuteNonQuery();
                var id = Guid.NewGuid();
                var factory = _db.QueryFactory();
                if (input.Id == null)
                {
                    factory.Query("data_sets")
                        .Insert(new
                        {
                            id = id,
                            name = input.Name,
                            object_name = input.ObjectName,
                            object_type = "View",
                            object_text = input.BodyText,
                            created_on = DateTime.Now,
                            created_by = SessionInfo.UserId,
                            last_modified_on = DateTime.Now,
                            last_modified_by = SessionInfo.UserId,
                            tabular_actions = input.Actions,
                            tabular_metadata = Newtonsoft.Json.JsonConvert.SerializeObject(input.Metadata)
                        });
                }
                else
                {
                    factory.Query("data_sets").Where("id", input.Id)
                            .Update(new
                            {
                                name = input.Name,
                                object_name = input.ObjectName,
                                object_type = "View",
                                object_text = input.BodyText,
                                last_modified_on = DateTime.Now,
                                last_modified_by = SessionInfo.UserId,
                                tabular_actions = input.Actions,
                                tabular_metadata = Newtonsoft.Json.JsonConvert.SerializeObject(input.Metadata)
                            });
                }
                
                _db.CloseConnection();
                return true;
            }

            return false;
        }

        
        private bool ValidateViewBody(CreateViewInputDto input)
        {
            // check object name already exists
            var oldobjname = "";
            if (input.Id != null)
            {
                var data = _db.QueryFactory().Query("data_sets").Where("id", input.Id).Get<DataSetsModel>().FirstOrDefault();
                if (data != null)
                {
                    oldobjname = data.object_name;
                    input.OldObjectName = data.object_name;
                    var ds = _db.SQL($"SELECT object_id FROM sys.views WHERE [name] = '{oldobjname}'")
                        .ExecuteDataSet();
                    if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
                    {
                        var objectId = Convert.ToString(ds.Tables[0].Rows[0][0]);
                        return true;
                    }
                }
                throw new Exception($"Old object {input.Id} not found!");
            }
            else
            {
                var ds = _db.SQL($"SELECT object_id FROM sys.views WHERE [name] = '{input.ObjectName}'")
                .ExecuteDataSet();
                if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
                {
                    var objectId = Convert.ToString(ds.Tables[0].Rows[0][0]);
                    return false;
                }
            }
            
            return true;
        }

        public dynamic GetDatasets()
        {
            _db.OpenConnection();
            var db = _db.QueryFactory();
            var query = db.Query("data_sets as a")
                          .LeftJoin("users as b", "b.id", "a.created_by")
                          .LeftJoin("users as c", "c.id", "a.last_modified_by")
                          .Select("a.id", "a.name", "a.object_name as objectName", "a.object_type as objectType", "a.tabular_actions as actions",
                                  "a.created_on as createdOn", "b.name as createdBy", "a.last_modified_on as lastModifiedOn", "c.name as LastModifiedBy")
                          .OrderBy("a.name");

            return query.Get();
        }
        public dynamic GetDataset(Guid id)
        {
            _db.OpenConnection();
            var db = _db.QueryFactory();
            var query = db.Query("data_sets")
                          .Where("id", id);
            var data = query.Get<DataSetsModel>().FirstOrDefault();
            if (data != null) return data.ToOutputDto();
            return null;
        }

        public dynamic ValidateDatasetQuery(string query)
        {
            int? error_line=null;
            string error_message = null;

            Regex regex = new Regex(@"\b(if|drop|delete|truncate|exec|dbcc|alter|while|loop|create)\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            if (regex.IsMatch(query))
            {
                return new
                {
                    success = false,
                    error_line=-1,
                    error_message="Only SELECT query supported with JOIN."
                };
            }
            _db.OpenConnection();
            var ds = _db.StoredProcedure("verify_dataset")
                    .AddParameter("@body", query)
                    .AddParameter("@error_line", error_line, System.Data.SqlDbType.Int, System.Data.ParameterDirection.Output)
                    .AddParameter("@error_message", error_message, System.Data.SqlDbType.VarChar, 200, System.Data.ParameterDirection.Output)
                    .ExecuteDataSet(c =>
                    {
                        if(c.Parameters["@error_line"].Value!=DBNull.Value)
                            error_line = Convert.ToInt32(c.Parameters["@error_line"].Value);
                        if (c.Parameters["@error_message"].Value != DBNull.Value)
                            error_message = Convert.ToString(c.Parameters["@error_message"].Value);
                    });
            if (ds.Tables.Count == 0)
            {
                return new
                {
                    success = false,
                    error_line,
                    error_message
                };
            }
            return new
            {
                success = true,
                MetaData = ds.Tables[0].TableToDictionary()
            };
        }

        public dynamic GetFields(string commandText, List<ParamArg> parametrs=null)
        {
            return MetadataGenerator.GetFields(commandText, GetParametersAndValues(parametrs));
        }

        private Dictionary<string,object> GetParametersAndValues(List<ParamArg> parametrs)
        {
            if (parametrs == null || parametrs.Count == 0) return null;
            var paramdata = new Dictionary<string, object>();
            if (parametrs.Count > 0)
            {
                foreach (var p in parametrs)
                {
                    var par = new ParameterInput() { ParameterName = p.Name };
                    if (!p.Value.IsNullOrEmpty())
                    {
                        if (p.DataType == "DateTime")
                        {
                            par.Value = p.Value.ToDateTime();
                        }
                        else if (p.DataType == "Boolean")
                        {
                            par.Value = p.Value.ToLower() == "true";
                        }
                        else if (p.DataType == "Float")
                        {
                            par.Value = Convert.ToDecimal(p.Value);
                        }
                        else if (p.DataType == "Integer")
                        {
                            par.Value = Convert.ToInt32(p.Value);
                        }
                        else
                            par.Value = p.Value;
                       
                    }
                    paramdata.Add(par.ParameterName, par.Value);

                }
            }
            return paramdata;
        }
    }
}
