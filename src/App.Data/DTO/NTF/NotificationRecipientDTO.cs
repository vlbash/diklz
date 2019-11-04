using System;
using App.Core.Data.DTO.Common;
using App.Core.Security;
using App.Data.Models.ORG;
using App.Data.Models.NTF;

namespace App.Data.DTO.NTF
{
    [RightsCheckList(nameof(Notification))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class NotificationRecipientDTO: BaseDTO
    {
        public Guid OrgUnitId { get; set; }
        public string RecipientName { get; set; }
        public string RecipientEmail { get; set; }

        public bool ReceiveOnChangeAllApplication { get; set; }
        public bool ReceiveOnChangeAllMessage { get; set; }
        public bool ReceiveOnChangeOrgInfo { get; set; }
        public bool ReceiveOnOverduePayment { get; set; }
    }
}
