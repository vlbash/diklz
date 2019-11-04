using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using App.Core.Base;
using App.Core.Data.CustomAutoMapper;
using App.Core.Data.Entities.Common;
using App.Core.Data.Entities.ORG;
using App.Core.Data.Interfaces;
using App.Data.Interfaces;
using App.Data.Models.APP;
using App.Data.Models.ORG;
using App.Data.Models.P902;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Models.P902
{
    public class AppConclusion: BaseEntity, ILimsDoc, IDerivedEntity
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
        static Type _type = typeof(ResultInputControl);

        static IMapper _mapper = new MapperConfiguration(cfg => cfg.CreateMap<ResultInputControl, LimsDoc>()
            .ForMember(x => x.DerivedClass, opt => opt.MapFrom(o => _type.Name)).MapOnlyIfChanged()).CreateMapper();

        public override IMapper _Mapper { get; } = _mapper;
        public override Type _BaseType { get; } = _baseType;
        public override Type _Type { get; } = _type;
        public string BaseClass { get; set; } = _baseType.Name;

        public override IEntity _BaseQuery(DbContext context) =>
            context.Set<LimsDoc>().SingleOrDefault(x => x.Id == Id);

        #endregion

        public Branch Branch { get; set; }

        public Guid BranchId { get; set; }

        public string AppConclusionStatus { get; set; } 

        public string AppState { get; set; }

        public string AppSort { get; set; }

        [DisplayName("№ заяви")]
        public string DocNum { get; set; }

        [DisplayName("Дата заяви")]
        public DateTime? AppRegDate { get; set; }

        [DisplayName("Уповноважена особа")]
        public string Assigne { get; set; }

        [DisplayName("Підрозділ ДС")]
        public string TeritorialService { get; set; } //?? Check this

    }
}
