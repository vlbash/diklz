using System.ComponentModel;
using Astum.Core.Data.Interfaces;
using Z.EntityFramework.Plus;

namespace Astum.Core.Data.Entities.Common
{
    [AuditInclude]
    [AuditDisplay("Перерахування")]
    public class EnumRecord : BaseEntity, IEnumRecord
    {
        [DisplayName("Тип")]
        public string EnumType { get; set; }

        [DisplayName("Код")]
        public string Code { get; set; }

        [DisplayName("Значення")]
        public string Name { get; set; }

        public string ExParam1 { get; set; }
        public string ExParam2 { get; set; }
    }
}
