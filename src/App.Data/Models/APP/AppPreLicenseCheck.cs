using System;
using App.Core.Data.Entities.Common;

namespace App.Data.Models.APP
{
    public class AppPreLicenseCheck : BaseEntity
    {
        public Guid AppId { get; set; }
        public DateTime ScheduledStartDate { get; set; }
        public DateTime ScheduledEndDate { get; set; }
        public Guid CheckCreatedId { get; set; }
        public DateTime CreationDateOfCheck { get; set; }

        public DateTime? EndDateOfCheck { get; set; }
        public int? ResultOfCheck { get; set; }
        public long? OldLimsId { get; set; }
    }
}
