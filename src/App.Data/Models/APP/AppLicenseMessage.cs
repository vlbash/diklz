using System;
using System.ComponentModel;
using App.Core.Data.Entities.Common;

namespace App.Data.Models.APP
{
    public class AppLicenseMessage : BaseEntity
    {
        public Guid AppId { get; set; }
        [DefaultValue("")]
        public string MessageNumber { get; set; }
        public DateTime DateOfMessage { get; set; }
        public string SignedJobPosition { get; set; }
        public string SignedFullName { get; set; }
        public Guid Performer { get; set; }
        public string State { get; set; }
        public string AttachedFile { get; set; }
        public long OldLimsId { get; set; }
    }
}
