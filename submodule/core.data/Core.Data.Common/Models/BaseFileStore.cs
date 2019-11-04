using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;
using Core.Common.Enums;

namespace Core.Data.Common.Models
{
    [Display(Name = "Збережені файли")]
    [Table("FileStore")]
    public abstract class BaseFileStore : BaseEntity
    {
        public virtual FileType FileType { get; set; } 
        public virtual string EntityName { get; set; }
        public virtual Guid EntityId { get; set; }

        public virtual string FilePath { get; set; }
        public virtual string FileName { get; set; }
        public virtual string ContentType { get; set; }
        public virtual string OrigFileName { get; set; }
        public virtual double FileSize { get; set; }

        public virtual bool Ock { get; set; }
        public virtual string DocumentType { get; set; }
        public virtual string Description { get; set; }
    }
}
