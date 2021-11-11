using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vetris.Report.Core.Extensions;

namespace Vetris.Report.Core.Models.Reports
{
    public class SysReports
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [Required]
        [StringLength(150)]
        public string Category { get; set; }
        public bool Draft { get; set; }
        public string Report { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
    public class SysReportListModelDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public bool Draft { get; set; }
        public string CreatedByUser { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string LastModifiedByUser { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }

    public class SysReportGetEditModelDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public bool Draft { get; set; }
        public JObject Report { get; set; }
    }
    public class SysReportGetTempEditModelDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public bool Draft { get; set; }
        public string JsonReport { get; set; }

        public SysReports ToCreateModel()
            => new SysReports
            {
                Id = Id,
                Name = Name,
                Category = Category,
                Draft = Draft,
                Report = JsonReport
            }; 

        public SysReportGetEditModelDto ToGetEditModel()
                => new SysReportGetEditModelDto
                {
                    Id = Id,
                    Name=Name,
                    Category=Category,
                    Draft=Draft,
                    Report = JsonReport.IsNullOrEmpty()?null:JObject.Parse(JsonReport)
                };

    }
    public class SysReportSaveModelDto
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [Required]
        [StringLength(150)]
        public string Category { get; set; }
        public bool Draft { get; set; }
        [Required]
        public string Report { get; set; }
    }
}
