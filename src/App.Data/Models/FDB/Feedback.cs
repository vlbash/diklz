using System;
using App.Core.Data.Entities.Common;

namespace App.Data.Models.FDB
{
    public class Feedback: BaseEntity
    {
        public Guid? AppId { get; set; }

        public string AppSort { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public Guid? OrgId { get; set; }

        public Guid? OrgEmployeeId { get; set; }

        public bool IsRated { get; set; }
    }
}
