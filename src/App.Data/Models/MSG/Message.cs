using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using App.Core.Base;
using App.Core.Data.CustomAutoMapper;
using App.Core.Data.Entities.Common;
using App.Core.Data.Entities.ORG;
using App.Core.Data.Interfaces;
using App.Data.Interfaces;
using App.Data.Models.ORG;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Models.MSG
{
    public class Message : BaseEntity, ILimsDoc, IDerivedEntity
    {
        #region ILimsDoc

        public Guid? ParentId { get; set; }
        public LimsDoc Parent { get; set; }

        public Guid? PerformerId { get; set; }
        public Employee Performer { get; set; }

        public string RegNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RegDate { get; set; }

        public string Description { get; set; }
        public Guid OrgUnitId { get; set; }
        public OrganizationExt OrgUnit { get; set; }
        public Guid OrganizationInfoId { get; set; }
        public long OldLimsId { get; set; }

        #endregion

        #region IDerivedEntity

        static Type _baseType = typeof(LimsDoc);
        static Type _type = typeof(Message);

        static IMapper _mapper = new MapperConfiguration(cfg => cfg.CreateMap<Message, LimsDoc>()
            .ForMember(x => x.DerivedClass, opt => opt.MapFrom(o => _type.Name)).MapOnlyIfChanged()).CreateMapper();

        public override IMapper _Mapper { get; } = _mapper;
        public override Type _BaseType { get; } = _baseType;
        public override Type _Type { get; } = _type;
        public string BaseClass { get; set; } = _baseType.Name;

        public override IEntity _BaseQuery(DbContext context) =>
            context.Set<LimsDoc>().SingleOrDefault(x => x.Id == Id);

        #endregion
        
        public Guid MessageParentId { get; set; }

        public bool IsCreatedOnPortal { get; set; }

        [DisplayName("Ліцензія виробництва")]
        public bool IsPrlLicense { get; set; }

        [DisplayName("Ліцензія опту")]
        public bool IsTrlLicense { get; set; }

        [DisplayName("Ліцензія імпорту")]
        public bool IsImlLicense { get; set; }

        [DisplayName("Тип повідомлення")]
        public string MessageType { get; set; }
        
        public string MessageHierarchyType { get; set; }

        [DisplayName("Номер вихідного листа ліцензіата")]
        public string MessageNumber { get; set; }

        [DisplayName("Дата вихідного листа ліцензіата")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime MessageDate { get; set; }

        [DisplayName("Текст повідомлення")]
        public string MessageText { get; set; }

        [DisplayName("Стан повідомлення")]
        public string MessageState { get; set; }

        public Guid MpdSelectedId { get; set; }

        #region SgdChiefNameChange

        [DisplayName("ПІБ нового керівника СГД")]
        public string SgdShiefFullName { get; set; }

        [DisplayName("ПІБ керівника СГД")]
        public string SgdShiefOldFullName { get; set; }
        #endregion

        #region SgdNameChange

        [DisplayName("Нова назва юридичної особи / ПІБ фізичної особи-підприємця")]
        public string SgdNewFullName { get; set; }

        [DisplayName("Назва юридичної особи / ПІБ фізичної особи-підприємця")]
        public string SgdOldFullName { get; set; }

        #endregion

        #region OrgFopLocationChange
        [DisplayName("Нове місцезнаходження юридичної особи/ Місце проживання фізичної особи-підприємця")]
        public Guid NewLocationId { get; set; }

        [DisplayName("Місцезнаходження юридичної особи/ Місце проживання фізичної особи-підприємця")]
        public Guid OldLocationId { get; set; }

        #endregion

        #region MPDActivityRestoration

        [DataType(DataType.Date)]
        [DisplayName("Дата відновлення дяльності")]
        public DateTime? RestorationDate { get; set; }

        [DisplayName("Причина відновлення дяльності")]
        public string RestorationReason { get; set; }

        #endregion

        #region MPDActivitySuspension

        [DataType(DataType.Date)]
        [DisplayName("Дата початку призупинення діяльності")]
        public DateTime? SuspensionStartDate { get; set; }

        [DisplayName("Причина призупинення роботи")]
        public string SuspensionReason { get; set; }

        #endregion

        #region MPDClosingForSomeActivity

        [DataType(DataType.Date)]
        [DisplayName("Дата закриття МПД")]
        public DateTime? ClosingDate { get; set; }

        [DisplayName("Причина закриття МПД")]
        public string ClosingReason { get; set; }

        #endregion

        #region MPDLocationRatification

        [DisplayName("Нова адреса місця провадження діяльності")]
        public Guid AddressBusinessActivityId { get; set; }

        #endregion

        #region PharmacyHeadReplacement

        [MaxLength(100)]
        [DisplayName("Ім'я")]
        public string PharmacyHeadName { get; set; }

        [MaxLength(200)]
        [DisplayName("По батькові")]
        public string PharmacyHeadMiddleName { get; set; }

        [MaxLength(200)]
        [DisplayName("Прізвище")]
        public string PharmacyHeadLastName { get; set; }

        #endregion

        #region PharmacyAreaChange

        [DisplayName("Нова площа аптечного закладу")]
        public long? NewPharmacyArea { get; set; }

        #endregion

        #region PharmacyNameChange

        [DisplayName("Нова назва аптечного закладу")]
        public string NewPharmacyName { get; set; }

        #endregion

        #region LeaseAgreementChange

        [DisplayName("Назва юридичної особи з яким укладено новий договір")]
        public string NewLegalEntity { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Дата укладання договору")]
        public DateTime? LeaseAgreementStartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Діє до")]
        public DateTime? LeaseAgreementEndDate { get; set; }

        #endregion
    }
}
