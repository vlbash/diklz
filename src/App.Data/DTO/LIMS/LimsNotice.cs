using System;

namespace App.Data.DTO.LIMS
{
    public class LimsNotice
    {
        public int NoticeId { get; set; }
        public int? AppId { get; set; }
        public int OwnerId { get; set; }
        public string RegNum { get; set; }
        public DateTime? RegDate { get; set; }
        public string AppTypeName { get; set; }
        public string AppRegNum { get; set; }
        public DateTime? AppRegDate { get; set; }
        public string SideName { get; set; }
        public int? StatusId { get; set; }
        public string StatusName { get; set; }
        public string SignerPib { get; set; }
        public string SignerPos { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatorName { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; }
    }
}
