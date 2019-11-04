using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Astum.Core.Data.Enums;
using Astum.Core.Data.Interfaces;

namespace Astum.Core.Data.Entities.Common
{
    public class BaseEntity : IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public RecordState RecordState { get; set; } = RecordState.N;
        [MaxLength(128)]
        public string Caption { get; set; }
        [MaxLength(128)]
        public string ModifiedBy { get; set; }
        [MaxLength(64)]
        public string ModifiedBy_Id { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [MaxLength(128)]
        public string CreatedBy { get; set; }
        [MaxLength(64)]
        public string CreatedBy_Id { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}