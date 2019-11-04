using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using App.Core.Base;
using App.Core.Data.CustomAutoMapper;
using App.Core.Data.Entities.Common;
using App.Core.Data.Entities.ORG;
using App.Core.Data.Interfaces;
using App.Data.Interfaces;
using App.Data.Models.CRV;
using App.Data.Models.IML;
using App.Data.Models.ORG;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Models.P902
{
    public class ResultInputControl: BaseEntity, ILimsDoc, IDerivedEntity
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

        public string State { get; set; }

        public string TeritorialService { get; set; }

        public Guid LicenseId { get; set; }
        
        public Guid LimsRPId { get; set; }

        public LimsRP LimsRP { get; set; }

        public string RegisterNumber { get; set; }

        public DateTime? EndDate { get; set; }

        public string DrugName { get; set; }

        public string DrugForm { get; set; }

        public string ProducerName { get; set; }

        public string ProducerCountry { get; set; }

        public string MedicineSeries { get; set; }

        public DateTime? MedicineExpirationDate { get; set; }

        public string SizeOfSeries { get; set; }

        public string UnitOfMeasurement { get; set; }

        public string AmountOfImportedMedicine { get; set; }

        public string WinNumber { get; set; }

        public DateTime? DateWin { get; set; }

        public string InputControlResult { get; set; }

        public string NameOfMismatch { get; set; }

        public string Comment { get; set; }

        public bool SendCheck { get; set; }
    }
}
