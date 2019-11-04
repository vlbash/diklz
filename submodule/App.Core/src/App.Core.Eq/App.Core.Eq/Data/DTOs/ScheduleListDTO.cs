using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;

namespace App.Core.Eq.Data.DTOs
{
    public class ScheduleListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }
    }
}
