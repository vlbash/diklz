using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.Serialization.Json;
using System.Text;
using App.WebApi.SOAP;

namespace NetWCFApp
{
    public class CacheDataService : ICacheDataService
    {
        private const string dataCachKey = "LicensesData";
        private static readonly ObjectCache cache = MemoryCache.Default;

        public List<License> CachedLicenses()
        {
            List<License> licenses = (List<License>)cache.Get(dataCachKey);
            if (licenses == null)
            {
                licenses = GetLicenses();
                cache.Add(dataCachKey, licenses, DateTime.Now.AddHours(1.1));
            }
            return licenses;
        }

        private List<License> GetLicenses()
        {
            var licenses = new List<License>();

            var folderName = AppDomain.CurrentDomain.BaseDirectory + "/JsonData";

            var fileName = (from f in new DirectoryInfo(folderName).GetFiles()
                orderby f.LastWriteTime descending
                select f.Name).FirstOrDefault();

            using (StreamReader sr = new StreamReader(folderName + "/" + fileName))
            {
                licenses.AddRange(deserializeJson<List<License>>(sr.ReadToEnd()));
            }

            return licenses;
        }

        private static T deserializeJson<T>(string result)
        {
            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(result)))
            {
                ms.Position = 0;
                return (T)jsonSer.ReadObject(ms);
            }
        }
    }
}
