using System;
using App.Core.Data.DTO.Common;

namespace App.Data.DTO.FDB
{
    public class FeedBackDetailsDTO: BaseDTO
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
