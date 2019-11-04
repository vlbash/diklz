using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using App.Core.Base;
using App.Core.Data.CustomAutoMapper;
using App.Core.Data.Entities.Common;
using App.Core.Data.Entities.ORG;
using App.Core.Data.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Models.ORG
{
    //OrgDepartmentExt
    public class Branch: BaseEntity, IOrgUnit, IDerivedEntity
    {
        #region IDerivedEntity
        static Type _baseType = typeof(OrgUnit);
        static Type _type = typeof(Branch);
        static IMapper _mapper = new MapperConfiguration(cfg => cfg.CreateMap<Branch, OrgUnit>()
            .ForMember(x => x.DerivedClass, opt => opt.MapFrom(o => _type.Name)).MapOnlyIfChanged()).CreateMapper();
        public override IMapper _Mapper { get; } = _mapper;
        public override Type _BaseType { get; } = _baseType;
        public override Type _Type { get; } = _type;
        public string BaseClass { get; set; } = _baseType.Name;
        public override IEntity _BaseQuery(DbContext context) => context.Set<OrgUnit>().SingleOrDefault(x => x.Id == Id);
        #endregion

        #region IOrgUnit
        [DisplayName("Назва")]
        public string Name { get; set; }
        [MaxLength(20)]
        public string Code { get; set; }

        //ссылка на бренч из дочерней сущности, для историчности
        public Guid? ParentId { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public string Category { get; set; }
        #endregion endregion

        //не понятно для чего добавили :)
        //public Guid? ParentBranchId { get; set; }
        //public Branch ParentBranch { get; set; }

        //OrgOrganization
        //ссылка на организацию
        public Guid OrganizationId { get; set; }
        public OrganizationExt Organization { get; set; }

        [MaxLength(20)]
        public string BranchState { get; set; }

        [MaxLength(20)]
        public string BranchActivity { get; set; } = "Active";

        [MaxLength(255)]
        public string PhoneNumber { get; set; }

        [MaxLength(20)]
        public string FaxNumber { get; set; }

        [MaxLength(100)]
        public string EMail { get; set; }

        //id бренча во внутренней системе лимс для API
        public int LimsLicenseBranchId { get; set; }

        public Guid AddressId { get; set; }

        public string AdressEng { get; set; }

        public bool? LicenseDeleteCheck { get; set; }

        #region Iml

        public bool ImlIsAvailiableStorageZone { get; set; }

        public bool ImlIsAvailiablePermitIssueZone { get; set; }

        public bool ImlIsAvailiableQuality { get; set; }

        #endregion

        #region Trl

        public Guid BranchId { get; set; }

        public bool TrlIsManufacture { get; set; }

        public bool TrlIsWholesale { get; set; }

        public bool TrlIsRetail { get; set; }

        #endregion

        #region Prl

        public bool PrlIsAvailiableProdSites { get; set; }

        public bool PrlIsAvailiableQualityZone { get; set; }

        public bool PrlIsAvailiableStorageZone { get; set; }

        public bool PrlIsAvailiablePickupZone { get; set; }

        public string OperationListForm { get; set; }// to Json

        public string OperationListFormChanging { get; set; } //don't map when applying to license 

        #endregion

        public bool? IsFromLicense { get; set; }

        // этот айдишник - ложь!
        // этот id является айдишником в таблице LIC_APP_BRANCH и не может быть использован для выборок и или процедур!
        public long OldLimsId { get; set; }

        public string AsepticConditions { get; set; }

        public string SpecialConditions { get; set; }

        public string BranchType { get; set; }

        public bool CreateTds { get; set; } //доручення ТДС

        public bool CreateDls { get; set; } //доручення ДЛС

        public string AreaOfCommonPremises { get; set; } //площа виробничих приміщень

        public string TotalArea { get; set; } // загальна площа

        public string Lpz { get; set; } //лікувально-профілактичний заклад
    }
}
