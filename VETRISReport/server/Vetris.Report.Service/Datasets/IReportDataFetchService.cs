using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Vetris.Report.Core.Dependency;
using Vetris.Report.Core.Models.Metadatas;
using Vetris.Report.Core.Models.Queries;

namespace Vetris.Report.Service.Datasets
{
    public interface IReportDataFetchService : ITransientDependency
    {
        dynamic FetchData(string objectname);
        dynamic FetchData(QueryDto input);
        DataSet GetReportData(string sql, Dictionary<string, object> parameters);
    }
}
