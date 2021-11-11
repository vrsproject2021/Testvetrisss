using Vetris.Report.Core.Models;
using Vetris.Report.Core.Models.Queries;

namespace Vetris.Report.Service.Excel
{
    public interface IDataToExcelExporter
    {
        FileDto ExportToFile(QueryDto input);
    }
}