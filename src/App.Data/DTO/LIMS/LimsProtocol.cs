using System;

namespace App.Data.DTO.LIMS
{
    public class LimsProtocol
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string ProtocolNumber { get; set; }
        public DateTime? ProtocolDate { get; set; }
        public string OrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public int OldLimsId { get; set; }
        public string Type { get; set; }
    }
}
