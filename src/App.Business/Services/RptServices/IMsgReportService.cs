using System;
using System.Threading.Tasks;

namespace App.Business.Services.RptServices
{
    public interface IMgsReportService
    {
        Task<string> MsgToPDFSgdChiefNameChange(Guid id);
        Task<string> MsgToPDFOrgFopLocationChange(Guid id);
        Task<string> MsgToPDFSgdNameChange(Guid id);
        Task<string> MsgToPDFAnotherEvent(Guid id);
        Task<string> MsgToPDFMPDActivitySuspension(Guid id);
        // тестировать
        Task<string> MsgToPDFMPDActivityRestoration(Guid id);
        Task<string> MsgToPDFMPDClosingForSomeActivity(Guid id);
        // тестировать
        Task<string> MsgToPDFMPDRestorationAfterSomeActivity(Guid id);
        Task<string> MsgToPDFMPDLocationRatification(Guid id);
        Task<string> MsgToPDFProductionDossierChange(Guid id);

        // тестировать
        Task<string> MsgToPDFPharmacyHeadReplacement(Guid id);

        // пустая заглушка
        Task<string> MsgToPDFPharmacyAreaChange(Guid id);

        // пустая заглушка
        Task<string> MsgToPDFPharmacyNameChange(Guid id);

        // пустая заглушка
        Task<string> MsgToPDFLeaseAgreementChange(Guid id);
        
        // пустая заглушка
        Task<string> MsgToPDFSupplierChange(Guid id);
    }
}
