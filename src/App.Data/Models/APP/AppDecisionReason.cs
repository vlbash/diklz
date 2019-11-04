using System;
using App.Core.Data.Entities.Common;

namespace App.Data.Models.APP
{
    public class AppDecisionReason: BaseEntity
    {
        public Guid AppDecisionId { get; set; }
        public AppDecision AppDecision { get; set; }
        public string ReasonType { get; set; }
    }
}
