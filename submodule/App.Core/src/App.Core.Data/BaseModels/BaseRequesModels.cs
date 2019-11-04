using System;

namespace App.Core.Data.BaseModels
{
    public class BaseRequestListParams
    {       
        public string Name { get; set; }
        public Int16 RowStart { get; set; }
        public Int16 PageSize { get; set; }
        public Int16 LanguageId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Mode { get; set; } 

    }
}
