using System.ComponentModel;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;

namespace App.Data.DTO.SEC
{
    public class ProfileListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [DisplayName("Активовано")]
        public bool IsActive { get; set; }
    }

    public class ProfileDetailDTO: BaseDTO
    {
        [DisplayName("Активовано")]
        public bool IsActive { get; set; }
    }
}
