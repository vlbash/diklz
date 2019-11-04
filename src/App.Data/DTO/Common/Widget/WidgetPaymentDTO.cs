using System;
using System.Collections.Generic;
using System.Text;
using App.Core.Base;
using App.Core.Security;
using App.Data.Models.ORG;

namespace App.Data.DTO.Common.Widget
{

    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class WidgetPaymentDTO : CoreDTO
    {
        public string AppType { get; set; }
        public Guid OrgUnitId { get; set; }
        public string AppSort { get; set; }
        public string RegNumber { get; set; }
        public DateTime RegDate { get; set; }
        public string EdocumentStatus { get; set; }
        public DateTime DecisionDate { get; set; }
    }
}
