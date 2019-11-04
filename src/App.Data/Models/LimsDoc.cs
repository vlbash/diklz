using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Entities.Common;
using App.Core.Data.Entities.ORG;
using App.Core.Data.Interfaces;
using App.Data.Interfaces;
using App.Data.Models.ORG;

namespace App.Data.Models
{
    public class LimsDoc: BaseEntity, ILimsDoc, IDerivableEntity
    {
        #region IDerivableEntity

        public string DerivedClass { get; set; }

        #endregion

        #region ILimsDoc

        public Guid? ParentId { get; set; }
        public LimsDoc Parent { get; set; }

        public Guid? ApplicantId { get; set; }
        public OrganizationExt Applicant { get; set; }

        public Guid? PerformerId { get; set; }
        public Employee Performer { get; set; }

        [MaxLength(20)]
        public string FaxNumber { get; set; }

        [MaxLength(100)]
        public string EMail { get; set; }

        [MaxLength(24)]
        public string Phone { get; set; }

        public string RegNumber { get; set; }

        public DateTime? RegDate { get; set; }

        [MaxLength(2000)]
        [DisplayName("Опис")]
        public string Description { get; set; }

        
        public Guid OrgUnitId { get; set; }
        public OrganizationExt OrgUnit { get; set; }

        public Guid OrganizationInfoId { get; set; }
        public long OldLimsId { get; set; }

        #endregion
    }
}
