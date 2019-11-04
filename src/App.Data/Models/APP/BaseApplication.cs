using System;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Entities.Common;

namespace App.Data.Models.APP
{
    public class BaseApplication: BaseEntity
    {
        //application type: production, import, commerce
        [MaxLength(30)]
        public string AppType { get; set; }

        //application sort: new license, change branch, change contractors etc
        [MaxLength(40)]
        public string AppSort { get; set; }

        //applications state: project, sent, etc
        [MaxLength(30)]
        public string AppState { get; set; }

        [MaxLength(30)]
        public string BackOfficeAppState { get; set; }

        public Guid? AppDecisionId { get; set; }
        public AppDecision AppDecision { get; set; }

        public Guid? AppPreLicenseCheckId { get; set; }
        public AppPreLicenseCheck AppPreLicenseCheck { get; set; }

        public Guid? AppLicenseMessageId { get; set; }
        public AppLicenseMessage AppLicenseMessage { get; set; }

        //application decision notes from lims employee
        public string AppDecisionNotes { get; set; }

        public bool IsCheckMpd { get; set; }

        public bool IsPaperLicense { get; set; }

        public bool IsCourierDelivery { get; set; }

        public bool IsPostDelivery { get; set; }

        public bool IsAgreeLicenseTerms { get; set; }

        public bool IsAgreeProcessingData { get; set; }

        public bool IsProtectionFromAggressors { get; set; }

        public bool IsCourierResults { get; set; }

        public bool IsPostResults { get; set; }

        public bool IsElectricFormResults { get; set; }

        public string Duns { get; set; }

        public string Comment { get; set; }

        public bool IsCreatedOnPortal { get; set; }

        public string ExpertiseResult { get; set; }

        public DateTime? ExpertiseDate { get; set; }
        public string ExpertiseComment { get; set; }

        public Guid? PerformerOfExpertise { get; set; }

        public string ErrorProcessingLicense { get; set; }

        public bool PrlInPharmacies { get; set; }

        public bool WholesaleOfMedicines { get; set; }

        public bool RetailOfMedicines { get; set; }
    }
}
