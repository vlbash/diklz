using Astum.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Astum.Core.Data.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Astum.Core.Data.Interfaces;
using Astum.Core.Data.Entities.ORG;

namespace App.Data.Entities.PRJ
{
    public class PrjPaymentOrder : BaseEntity, IDocument
    {
        public string RegNumber { get; set; }
        public DateTime? RegDate { get; set; }
        public string DocStateCode { get; set; }
        public string Description { get; set; }
        public PrjContract Contract { get; set; }
        public Guid ContractId { get; set; }
        public decimal Amount { get; set; }
        public Guid PayerId { get; set; }
        public OrgOrganization Payer { get; set; }
        public string ReceiverName { get; set; }
        public string ActsOfCompletedWork { get; set; }
    }
}
