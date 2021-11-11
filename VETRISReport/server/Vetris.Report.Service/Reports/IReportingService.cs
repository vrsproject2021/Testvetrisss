using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vetris.Report.Core.Dependency;
using Vetris.Report.Core.Models.Reports;

namespace Vetris.Report.Service.Reports
{
    public interface IReportingService : ITransientDependency
    {
        Task<PreviewPreparedArg> PreparePreviewArguments(ReportPreviewGeneratingArg arg);
        Task<PreviewPreparedArg> PrepareReportArguments(Guid id, List<ParameterInput> reportparams);
        Task<dynamic> GetForExecute(Guid Id);
        Task Create(SysReports input);
        Task Save(SysReportSaveModelDto input);
        Task<List<SysReportListModelDto>> GetAll(ReportListingQueryArg input);
        Task<SysReportGetEditModelDto> GetForEdit(Guid Id);
        Task<dynamic> GetCategories();
    }
}
