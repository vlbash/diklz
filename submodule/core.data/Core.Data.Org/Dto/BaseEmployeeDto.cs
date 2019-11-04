using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;
using Core.Common.Attributes;
using Core.Common.Enums;

namespace Core.Data.Org.Dto
{
    //[RightsCheckList(nameof(Employee), nameof(Person),
    //    nameof(Organization), nameof(global::Core.Data.Org.Models.OrgUnitPosition), nameof(OrgUnitPositionEmployee), nameof(Position), nameof(OrgUnit))]
    //[RlsRight(nameof(Entities.ATU.Region), nameof(RegionId))]
    //[RlsRight(nameof(Organization), nameof(OrganizationId))]
    public abstract class BaseEmployeeDetailDto : BaseDto
    {
        public virtual Guid? PersonId { get; set; }

        [Display(Name = "П.І.Б персони")]
        public virtual string PersonFIO { get; set; }

        [Display(Name = "Прізвище")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual string PersonLastName { get; set; }

        [Display(Name = "Ім'я")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual string PersonName { get; set; }

        [Display(Name = "По-батькові")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual string PersonMiddleName { get; set; }

        [Display(Name = "РНОКПП")]
        public virtual string PersonIPN { get; set; }

        //[Display(Name = "Дата народження")]
        //[Required(ErrorMessage = "Заповніть поле")]
        //public DateTime PersonBithday { get; set; }
        
        [Display(Name = "Телефон")]
        public virtual string PersonPhone { get; set; }

        [Display(Name = "Ел.адреса")]
        public virtual string PersonEmail { get; set; }

        [Display(Name = "Організація")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual Guid? OrganizationId { get; set; }

        [Display(Name = "Організація")] 
        public virtual string OrganizationName { get; set; }

        [Display(Name = "Посада")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual Guid? OrgUnitPositionId { get; set; }

        [Display(Name = "Посада")] 
        public virtual string OrgUnitPosition { get; set; }

        [Display(Name = "Область")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual Guid? RegionId { get; set; }

        [Display(Name = "Область")]
        public virtual string Region { get; set; }

        [NotMapped]
        public virtual string _ReturnUrl { get; set; }
    }

    //[RightsCheckList(nameof(Employee), nameof(Person),
    //    nameof(Organization), nameof(OrgUnitPosition), nameof(OrgUnitPositionEmployee), nameof(Position), nameof(OrgUnit))]
    //[RlsRight(nameof(Entities.ATU.Region), nameof(RegionId))]
    //[RlsRight(nameof(Organization), nameof(OrganizationId))]
    public abstract class BaseEmployeeListDto: BaseDto, IPagingCounted
    {
        public virtual int TotalRecordCount { get; set; }

        [Display(Name = "П.І.Б")]
        [CaseFilter(CaseFilterOperation.Contains)]
        public virtual string PersonFIO { get; set; }

        [Display(Name = "Область")]
        [CaseFilter(CaseFilterOperation.Equals)]
        public virtual Guid? RegionId { get; set; }

        [Display(Name = "Область")]
        public virtual string Region { get; set; }

        [Display(Name = "Назва організації")]
        public virtual Guid? OrganizationId { get; set; }

        [Display(Name = "Назва організації")]
        public virtual string OrganizationName { get; set; }

        [Display(Name = "РНОКПП")]
        [CaseFilter(CaseFilterOperation.Contains)]
        public virtual string PersonIPN { get; set; }
        
        [Display(Name = "Телефон")]
        public virtual string PersonPhone { get; set; }
        
        [Display(Name = "Ел.адреса")]
        public virtual string PersonEmail { get; set; }
    }

    //[RightsCheckList(nameof(Employee), nameof(Person))]
    public abstract class BaseEmployeeMinDto: BaseDto
    {
        public virtual string Name { get; set; }
        public virtual Guid? OrganizationId { get; set; }
    }

    //[RightsCheckList(nameof(Employee), nameof(Person))]
    public abstract class BaseEmployeeForLoginDto: BaseDto
    {
        public virtual string Name { get; set; }
        public virtual string UserId { get; set; }
    }

}
