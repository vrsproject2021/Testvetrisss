using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Vetris.Report.Core.Configurations;
using Vetris.Report.Core.Dependency;
using Vetris.Report.Core.Extensions;
using Vetris.Report.Core.Helper;
using Vetris.Report.Core.Models;
using Vetris.Report.Core.Session;
using Vetris.Report.DataAccess;

namespace Vetris.Report.Service.Excel
{
    public class EpPlusExcelExporterBase:ApplicationService, ITransientDependency
    {
        public IAppFolders AppFolders { get; set; }
        public EpPlusExcelExporterBase(IDatabaseContext database, IAppFolders folders, ISessionInfo session) : base(database,session)
        {
            AppFolders = folders;
        }

        protected FileDto CreateExcelPackage(string fileName, Action<ExcelPackage> creator)
        {
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            using (var excelPackage = new ExcelPackage())
            {
                creator(excelPackage);
                Save(excelPackage, file);
            }

            return file;
        }
        protected void Save(ExcelPackage excelPackage, FileDto file)
        {
            var filePath = Path.Combine(AppFolders.TempFileDownloadFolder, file.FileToken);
            excelPackage.SaveAs(new FileInfo(filePath));
        }

        protected void AddHeader(ExcelWorksheet sheet, params string[] headerTexts)
        {
            if (headerTexts.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeader(sheet, i + 1, headerTexts[i]);
            }
        }

        protected void AddHeader(ExcelWorksheet sheet, int columnIndex, string headerText)
        {
            sheet.Cells[1, columnIndex].Value = headerText;
            sheet.Cells[1, columnIndex].Style.Font.Bold = true;
        }

        protected void AddObjects<T>(ExcelWorksheet sheet, int startRowIndex, IList<T> items, params Func<T, object>[] propertySelectors)
        {
            if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 0; i < items.Count; i++)
            {
                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    sheet.Cells[i + startRowIndex, j + 1].Value = propertySelectors[j](items[i]);
                }
            }
        }
    }
}
