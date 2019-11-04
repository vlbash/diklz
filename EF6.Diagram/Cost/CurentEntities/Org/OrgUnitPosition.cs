using Astum.Core.Data.Enums;
using Astum.Core.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Astum.Core.Data.Entities.Common;

namespace Astum.Core.Data.Entities.ORG
{
    public class OrgUnitPosition : BaseEntity
    {
        public Guid OrgUnitId { get; set; }
		//public OrgUnit OrgUnit { get; set; }
		public Guid OrgPositionId { get; set; }
		//public OrgPosition OrgPosition { get; set; }
        [DisplayName("Тип посади")]
        public string OrgPositionType { get; set; }
        public bool IsResource { get; set;}

        [NotMapped]
        public string OrgPositionTypeName { get; set; }

        public ICollection<OrgEmployee> Employees { get; set; }

        //[NotMapped]
        //[DisplayName("Назва")]
        //public string Name
        //{
        //    get => string.Format("{0}-{1}", OrgUnit.Name, OrgPosition.Name);
        //    set { }
        //}
    }
}
