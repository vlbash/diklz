using System;
using System.Collections.Generic;
using App.Core.Data.Entities.Common;

namespace App.Data.Models.APP
{
    public class AppDecision: BaseEntity
    {
        public Guid AppId { get; set; }
        //application decision: accepted, denied, WithoutConsideration
        public string DecisionType { get; set; }
        public ICollection<AppDecisionReason> AppDecisionReasons { get; set; }
        public DateTime DateOfStart { get; set; }
        public Guid? ProtocolId { get; set; }
        public AppProtocol Protocol { get; set; }
        public string DecisionDescription { get; set; }
        public decimal PaidMoney { get; set; }
        public string Notes { get; set; }

        public bool IsClosed { get; set; }

        public AppDecision()
        {
            AppDecisionReasons = new List<AppDecisionReason>();
        }
    }
}
