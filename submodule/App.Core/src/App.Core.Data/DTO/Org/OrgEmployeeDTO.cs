using App.Core.Data.DTO.Common;
using System;
using System.ComponentModel;
using App.Core.Data.Attributes;
using App.Core.Data.Interfaces;
using AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Entities.CDN;
using App.Core.Data.Entities.Common;
using App.Core.Data.Entities.ORG;
using App.Core.Security;

namespace App.Core.Data.DTO.Org
{
    [RightsCheckList(nameof(Employee), nameof(Person),
        nameof(Organization), nameof(Entities.ORG.OrgUnitPosition), nameof(OrgUnitPositionEmployee), nameof(Position), nameof(OrgUnit))]
    [RlsRight(nameof(Entities.ATU.Region), nameof(RegionId))]
    [RlsRight(nameof(Organization), nameof(OrganizationId))]
    public class OrgEmployeeDetailDTO : BaseDTO
    {
        public static IMapper PersonMapper { get; } = new MapperConfiguration(cfg => {
            cfg.RecognizePrefixes("Person");
            cfg.CreateMap<OrgEmployeeDetailDTO, PersonDetailDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.PersonId));
        }).CreateMapper();

        public Guid? PersonId { get; set; }

        [DisplayName("П.І.Б персони")]
        public string PersonFIO { get; set; }

        [DisplayName("Прізвище")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string PersonLastName { get; set; }

        [DisplayName("Ім'я")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string PersonName { get; set; }

        [DisplayName("По-батькові")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string PersonMiddleName { get; set; }

        [DisplayName("РНОКПП")]
        public string PersonIPN { get; set; }

        //[DisplayName("Дата народження")]
        //[Required(ErrorMessage = "Заповніть поле")]
        //public DateTime PersonBithday { get; set; }
        
        [DisplayName("Телефон")]
        public string PersonPhone { get; set; }

        [DisplayName("Ел.адреса")]
        public string PersonEmail { get; set; }

        [DisplayName("Організація")]
        [Required(ErrorMessage = "Заповніть поле")]
        public Guid? OrganizationId { get; set; }

        [DisplayName("Організація")] 
        public string OrganizationName { get; set; }

        [DisplayName("Посада")]
        [Required(ErrorMessage = "Заповніть поле")]
        public Guid? OrgUnitPositionId { get; set; }

        [DisplayName("Посада")] 
        public string OrgUnitPosition { get; set; }

        [DisplayName("Область")]
        [Required(ErrorMessage = "Заповніть поле")]
        public Guid? RegionId { get; set; }

        [DisplayName("Область")]
        public string Region { get; set; }

        [NotMapped]
        public string _ReturnUrl { get; set; }
    }

    [RightsCheckList(nameof(Employee), nameof(Person),
        nameof(Organization), nameof(Entities.ORG.OrgUnitPosition), nameof(OrgUnitPositionEmployee), nameof(Position), nameof(OrgUnit))]
    [RlsRight(nameof(Entities.ATU.Region), nameof(RegionId))]
    [RlsRight(nameof(Organization), nameof(OrganizationId))]
    public class OrgEmployeeListDTO : BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [DisplayName("П.І.Б")]
        [PredicateCase(PredicateOperation.Contains)]
        public string PersonFIO { get; set; }

        [DisplayName("Область")]
        [PredicateCase(PredicateOperation.Equals)]
        public Guid? RegionId { get; set; }

        [DisplayName("Область")]
        public string Region { get; set; }

        [DisplayName("Назва організації")]
        public Guid? OrganizationId { get; set; }

        [DisplayName("Назва організації")]
        public string OrganizationName { get; set; }

        [DisplayName("РНОКПП")]
        [PredicateCase(PredicateOperation.Contains)]
        public string PersonIPN { get; set; }
        
        [DisplayName("Телефон")]
        public string PersonPhone { get; set; }
        
        [DisplayName("Ел.адреса")]
        public string PersonEmail { get; set; }
    }

    [RightsCheckList(nameof(Employee), nameof(Person))]
    public class OrgEmployeeMinDTO : BaseDTO
    {
        public string Name { get; set; }
        public Guid? OrganizationId { get; set; }
    }

    [RightsCheckList(nameof(Employee), nameof(Person))]
    public class OrgEmployeeForLoginDTO: BaseDTO
    {
        public string Name { get; set; }
        public string UserId { get; set; }
    }

}
