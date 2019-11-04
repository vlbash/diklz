using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace App.Core.Business.Services
{
    public interface ISearchFilterSettingsService
    {
        string GenerateInputConfig(Type dtoType);
        string GenerateInputConfig(Type dtoType, Dictionary<string, SelectList> dictionaryOfSelectLists);
        string GetUserPresettings(string journalName);
        Task SetUserPresettings(string journalName, string presettingsJson);
    }
}
