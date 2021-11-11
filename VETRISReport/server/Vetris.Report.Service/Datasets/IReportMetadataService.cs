using System;
using System.Collections.Generic;
using System.Text;
using Vetris.Report.Core.Dependency;
using Vetris.Report.Core.Models;
using Vetris.Report.Core.Models.Metadatas;
using Vetris.Report.Core.Models.Queries;
using Vetris.Report.Core.Models.Reports;

namespace Vetris.Report.Service.Datasets
{
    public interface IReportMetadataService : ITransientDependency
    {
        List<MetaDataJson> Columns(Guid id);
        bool CreateOrModifyView(CreateViewInputDto input);
        dynamic GetDatasets();
        dynamic GetDataset(Guid id);
        dynamic ValidateDatasetQuery(string query);
        dynamic GetFields(string commandText, List<ParamArg> parametrs = null);
    }
}
