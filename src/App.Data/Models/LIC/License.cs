using System;
using App.Core.Data.Entities.Common;
using App.Data.Models.IML;
using App.Data.Models.ORG;
using App.Data.Models.PRL;
using App.Data.Models.TRL;

namespace App.Data.Models.LIC
{
    public class License: BaseEntity
    {
        public Guid OrganizationId { get; set; }
        public OrganizationExt Organization { get; set; }

        public Guid TrlLicenseId { get; set; }
        public TrlLicense TrlLicense { get; set; }

        public Guid ImlLicenseId { get; set; }
        public ImlLicense ImlLicense { get; set; }

        public Guid PrlLicenseId { get; set; }
        public PrlLicense PrlLicense { get; set; }
    }
}
