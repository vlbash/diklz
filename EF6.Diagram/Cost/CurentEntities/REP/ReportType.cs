using Astum.Core.Data.Entities.Common;
using System;
using System.Collections.Generic;

namespace App.Data.Entities.REP
{
    public class ReportType : BaseEntity
    {
        public Guid? ParentId { get; set; }

        public ReportType Parent { get; set; }

        public ICollection<ReportType> SubCategories { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        //тип ресурсу з "Класифікатор типів ресурсів" RecordStatus
        public string ReportStatusEnum { get; set; } 
    }
}
