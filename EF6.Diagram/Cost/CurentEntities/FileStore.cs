using System;
using Astum.Core.Data.Enums;

namespace Astum.Core.Data.Entities.Common
{
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
    }
}
