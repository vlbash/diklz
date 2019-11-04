using System;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Enums;

namespace App.Core.Data.Entities.Common
{
    [Display(Name = "Збережені файли")]
    public class FileStore : BaseEntity
    {
        public FileType FileType { get; set; } 
        public string EntityName { get; set; }
        public Guid EntityId { get; set; }

        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string OrigFileName { get; set; }
        public double FileSize { get; set; }

        public bool Ock { get; set; }
        public string DocumentType { get; set; }
        public string Description { get; set; }
    }
}
