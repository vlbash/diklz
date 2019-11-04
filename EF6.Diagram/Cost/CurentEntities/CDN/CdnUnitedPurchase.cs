using Astum.Core.Data.Entities.Common;
using System;
using System.Collections.Generic;

namespace App.Data.Entities.CDN
{
    public class CdnUnitedPurchase: BaseDirectory
    {
        //Підпорядкування
        public Guid? ParentId { get; set; }

        public CdnUnitedPurchase Parent { get; set; }

        public ICollection<CdnUnitedPurchase> Children { get; set; }

        //Дата початку дїї
        public DateTime StartRecordDate { get; set; }

        //Дата закінчення дії
        public DateTime EndRecordDate { get; set; }

        //Назва англ
        public string NameEn { get; set; }
    }
}
