using System;

namespace App.Core.Data.DTM
{
    public class DtmTemplatesDataDetailDTO
    {
        public string DocId  { get; set; }
        public long TemplateId { get; set; }
        public string TemplateName { get; set; }
        public long EntityId { get; set; }
        public string EntityName { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
