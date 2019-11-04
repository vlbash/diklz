
namespace App.WebApi.SOAP
{
    public class WebApiService: IWebApiService
    {
        private  IMongoService mongoService = new MongoDbService();

       

        public License GetLicenseByEdrpou(string edrpou) => mongoService.GetLicenseByEdrpou(edrpou);

        public Branch GetBranchById(int id)
        {
            var br = mongoService.GetBranchById(id);
            return br;
        }

        public License GetLicenseById(int id)
        {
            var lic = mongoService.GetLicenseById(id);
            return lic;
        }
    }
}
