using System;

namespace App.Data.DTO.LIMS
{
    public class LimsCheck
    {
        public int CheckId { get; set; }
        public DateTime? FactDate{ get; set; }
        public int DefectCount{ get; set; }
    }
}
