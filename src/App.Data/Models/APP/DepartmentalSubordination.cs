using System;
using System.Collections.Generic;
using System.Text;
using App.Core.Data.Entities.Common;

namespace App.Data.Models.APP
{
    public class DepartmentalSubordination : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
