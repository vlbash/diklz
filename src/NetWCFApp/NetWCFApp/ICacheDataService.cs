using System.Collections.Generic;
using App.WebApi.SOAP;

namespace NetWCFApp
{
    public interface ICacheDataService
    {
        List<License> CachedLicenses();
    }
}
