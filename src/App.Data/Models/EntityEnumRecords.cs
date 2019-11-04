using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using App.Core.Data.Entities.Common;

namespace App.Data.Models
{
    public class EntityEnumRecords: BaseEntity
    {
        public Guid EntityId { get; set; }
        public string EntityType { get; set; }
        public string EnumRecordType { get; set; }
        public string EnumRecordCode { get; set; }
    }
}
