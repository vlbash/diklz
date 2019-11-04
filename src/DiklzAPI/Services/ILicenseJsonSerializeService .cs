using System.Collections.Generic;
using App.WebApi.Models;

namespace App.WebApi.Services
{
    internal interface ILicenseJsonSerializeService
    {
        List<License> GetLicenses(IList<string> loggingList);
    }
}
