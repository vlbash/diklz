using System;

namespace App.Data.DTO.LIMS
{
    public class LimsPendingChanges
    {
        public long Id { get; set; }
        public string EntityName{ get; set; }
        public long EntityId{ get; set; }
        public string Action{ get; set; }
        public bool Processed{ get; set; }
        public DateTime Created{ get; set; }
    }
}
