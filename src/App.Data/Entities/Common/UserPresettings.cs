using System;
using App.Core.Data.Entities.Common;

namespace App.Data.Entities.Common
{
    public class UserPresettings: BaseEntity
    {
        public Guid UserId { get; set; }

        public string JournalName { get; set; }

        public string PresettingsJson { get; set; }
    }
}
