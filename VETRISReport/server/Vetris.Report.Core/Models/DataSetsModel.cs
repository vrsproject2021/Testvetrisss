using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vetris.Report.Core.Models
{
    public class DataSetsModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string object_name { get; set; }
        public string object_type { get; set; }
        public string object_text { get; set; }
        public string tabular_actions { get; set; }
        public string tabular_metadata { get; set; }
        public Guid? created_by { get; set; }
        public DateTime? created_On { get; set; }
        public Guid? last_modified_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public DataSetOutputDto ToOutputDto()
        {
            return new DataSetOutputDto
            {
                Id = id,
                Name = name,
                ObjectName = object_name,
                ObjectType = "View",
                BodyText = object_text,
                Actions = tabular_actions,
                Metadata=JsonConvert.DeserializeObject<List<MetaDataJson>>(tabular_metadata??"")
            };
        }
    }
}
