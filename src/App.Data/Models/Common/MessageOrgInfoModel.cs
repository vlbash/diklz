using System;
using System.Collections.Generic;
using System.Text;

namespace App.Data.Models.Common
{
    public class MessageOrgInfoModel
    {
        public string OrgDirector { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public string EDRPOU { get; set; }
        public Guid AddressId { get; set; }
    }
}
