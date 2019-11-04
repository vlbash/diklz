using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Business.Extensions;
using App.Core.Business.Services;
using App.Core.Data.DTO.Common;
using App.Data.DTO.APP;
using App.Data.DTO.LOG;
using App.Data.DTO.PRL;
using App.Data.Models.MSG;
using Microsoft.EntityFrameworkCore.Internal;
using FileStoreDTO = App.Data.DTO.Common.FileStoreDTO;
using PrlLicenseDetailDTO = App.Data.DTO.PRL.PrlLicenseDetailDTO;

namespace App.Business.Services.Common
{
    public class AuditService
    {
        private static List<string> FollowTables => new List<string>
        {
            "PrlApplication",
            "Message",
            "PrlContractor",
            "AppDecision",
            "PrlLicense",
            "AppPreLicenseCheck",
            "AppAssignee",
            "AppLicenseMessage",
            "FileStore",
            "Person"
        };

        public string StrFollowTable => string.Join(", ", FollowTables.Select(p => "'" + p + "'"));

        public ICommonDataService DataService { get; }

        public AuditService(ICommonDataService dataService)
        {
            DataService = dataService;
        }

        public IEnumerable<LogAuditEntryListDTO> GeEntityHistoryById(int auditId)
        {
            IEnumerable<LogAuditEntryListDTO> audEntityHistory = new List<LogAuditEntryListDTO>();

            var modelId = DataService.GetDto<LogAuditListOfChangesDTO>(p =>
                p.AuditEntryId == auditId && p.PropertyName == "Id").Select(p => p.OldValueFormatted).SingleOrDefault();

            if (modelId == null)
            {
                return audEntityHistory;
            }

            var listChanges = DataService.GetDto<LogAuditListOfChangesDTO>(p => p.OldValueFormatted == modelId).Select(p => p.AuditEntryId).ToList();
            audEntityHistory = DataService.GetDto<LogAuditEntryListDTO>(p => listChanges.Contains(p.AuditEntryId), extraParameters:StrFollowTable);

            return audEntityHistory;
        }

        public void SetDisplayNameEntityProp(IEnumerable<LogAuditListOfChangesDTO> model, bool isEmptyPropName = false)
        {
            IEnumerable<PropertyInfo> propertyInfos = new List<PropertyInfo>();
            var auditListOfChangesDto = model.ToList();

            switch (auditListOfChangesDto.FirstOrDefault()?.EntityName)
            {
                case "PrlApplication":
                    propertyInfos = typeof(PrlAppDetailDTO).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead);
                    break;
                case "Message":
                    propertyInfos = typeof(Message).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead);
                    break;
                case "FileStore":
                    propertyInfos = typeof(FileStoreDTO).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead);
                    break;
                case "PrlContractor":
                    propertyInfos = typeof(PrlContractorDetailDTO).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead);
                    break;
                case "AppDecision":
                    propertyInfos = typeof(AppDecisionDTO).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead);
                    break;
                case "PrlLicense":
                    propertyInfos = typeof(PrlLicenseDetailDTO).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead);
                    break;
                case "AppPreLicenseCheck":
                    propertyInfos = typeof(AppPreLicenseCheckDTO).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead);
                    break;
                case "AppAssignee":
                    propertyInfos = typeof(AppAssigneeDetailDTO).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead);
                    break;
                case "AppLicenseMessage":
                    propertyInfos = typeof(AppLicenseMessageDTO).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead);
                    break;
                case "Person":
                    propertyInfos = typeof(PersonDetailDTO).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead);
                    break;
            }

            var propFriendlyName = propertyInfos.Select(p => p.GetDisplayName(isEmptyPropName));

            foreach (var logAuditListOfChangesDto in auditListOfChangesDto)
            {
                var prop = propertyInfos.FirstOrDefault(p => p.Name == logAuditListOfChangesDto.PropertyName);
                if(prop == null)
                {
                    continue;
                }

                var idxProp = propertyInfos.IndexOf(prop);
                logAuditListOfChangesDto.PropertyDisplayName = propFriendlyName.ElementAt(idxProp);
            }
        }
    }
}
