using System;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;

namespace App.Data.DTO.SEC
{
    public class ProfileRowLevelRightListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }
        public Guid ProfileId { get; set; }
        public string EntityName { get; set; }
        public RowLevelAccessType AccessType { get; set; }
        public Guid? EntityId { get; set; }
        public Guid? SecurId { get; set; }
    }

    public class ProfileRowLevelRightDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }
        public Guid ProfileId { get; set; }
        public string EntityName { get; set; }
        public RowLevelAccessType AccessType { get; set; }
    }
}
