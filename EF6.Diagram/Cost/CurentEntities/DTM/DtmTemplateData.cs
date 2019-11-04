using System;
using System.ComponentModel.DataAnnotations.Schema;
using Astum.Core.Data.Entities.Common;
using Astum.Core.Data.Enums;
using Z.EntityFramework.Plus;

namespace Astum.Core.Data.Entities.DTM
{
    [AuditInclude]
    [AuditDisplay("DtmTemplateData")]
    public class DtmTemplateData : BaseEntity
    {
        public string DocId { get; set; }
        public Guid EntityId { get; set; }
        public string EntityName { get; set; }
        public Guid TemplateElementId { get; set; }
        public DtmTemplateElement TemplateElement { get; set; }

        [Column(TypeName = "jsonb")]
        public string Value { get; set; }


        [NotMapped]
        public string ElementName { get; set; }
        [NotMapped]
        public Guid? TemplateElementParentId { get; set; }
        [NotMapped]
        public string TemplateElementCode { get; set; }
    }
}
