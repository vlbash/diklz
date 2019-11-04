using App.Core.Base;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using System;
using System.ComponentModel;
using App.Core.Data.Entities.ORG;
using App.Core.Security;

namespace App.Core.Data.DTO.Org
{
    public interface IOrgUnitDetailDTO
    {
        Guid Id { get; set; }
        string Name { get; set; }
        Guid? ParentId { get; set; }
        string Parent { get; set; }
        string Description { get; set; }
        //string PropertiesJson { get; set; }
        //string AtuAddressGuids { get; set; }
    }

    public interface IOrgUnitListDTO
    {
        Guid Id { get; set; }
        string Name { get; set; }
    }

    [RightsCheckList(nameof(OrgUnit))]
    public class OrgUnitDetailDTO : BaseDTO, IOrgUnitDetailDTO
    {
        [DisplayName("Назва")]
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public string Parent { get; set; }
        public string Description { get; set; }
        //public string PropertiesJson { get; set; }
        public string AtuAddressGuids { get; set; }
    }

    [RightsCheckList(nameof(OrgUnit))]
    public class OrgUnitListDTO : BaseDTO, IOrgUnitListDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [DisplayName("Назва")]
        [PredicateCase(PredicateOperation.Contains)]
        public string Name { get; set; }


        //public string PropertiesJson { get; set; }
    }

    [RightsCheckList(nameof(OrgUnit))]
    public class OrgUnitMinDTO : BaseDTO
    {
        public string Name { get; set; }
    }
}
