using System;
using System.Configuration;
using System.Linq;
using MongoDB.Driver;

namespace App.WebApi.SOAP
{
    public interface IMongoService
    {
        License GetLicenseByEdrpou(string edrpou);
        License GetLicenseById(int id);
        Branch GetBranchById(int id);
    }

    public class MongoDbService: IMongoService
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<License> _licenseCollection;
        private readonly IMongoCollection<RequestLog> _requestLogsCollection;

        public MongoDbService()
        {
            string connectionString = ConfigurationManager.AppSettings["MongoConnection"];
            _client = new MongoClient(connectionString: connectionString);
            _database = _client.GetDatabase(ConfigurationManager.AppSettings["MongoDbName"]);
            _licenseCollection = GetCollection<License>();
            _requestLogsCollection = GetCollection<RequestLog>("RequestLogs");
        }

        public IMongoCollection<T> GetCollection<T>(string nameOfCollection = "")
        {
            return _database.GetCollection<T>(string.IsNullOrEmpty(nameOfCollection) ? typeof(T).Name : nameOfCollection);
        }

        public IMongoCollection<License> GetLicenseCollection()
        {
            return _licenseCollection;
        }

        public License GetLicenseByEdrpou(string edrpou)
        {
            var licensesByEdrpou =
                GetLicenseCollection().Find(filter: x => x.EDRPOU == edrpou).ToList();
            var license = licensesByEdrpou.FirstOrDefault(x => x.LicenseStatusId == 1)
                          ?? licensesByEdrpou.OrderByDescending(x => x.TerminateDate).FirstOrDefault();
            _requestLogsCollection.InsertOne(new RequestLog
            {
                Id = Guid.NewGuid(), RequestQuery = $"Method: GetLicenseByEdrpou, EDRPOU: {edrpou}", LogDateTime = DateTime.Now, ResultObject = license
            });
            return license;
        }

        public License GetLicenseById(int id)
        {
            var license = GetLicenseCollection().Find(filter: x => x.Id == id).FirstOrDefault();
            if (license != null && license.LicenseStatusId != 1)
            {
                var edrpouLicenses = GetLicenseCollection().Find(filter: x => x.EDRPOU == license.EDRPOU).ToList();
                var activeLicense = edrpouLicenses.FirstOrDefault(x => x.LicenseStatusId == 1);

                license = activeLicense ?? edrpouLicenses.OrderByDescending(x => x.TerminateDate).FirstOrDefault();
            }
            _requestLogsCollection.InsertOne(new RequestLog
            {
                Id = Guid.NewGuid(), RequestQuery = $"Method: GetLicenseById, Id: {id}", LogDateTime = DateTime.Now, ResultObject = license
            });
            return license;
        }

        public Branch GetBranchById(int id)
        {
            var filter = Builders<License>.Filter.ElemMatch(field: "Branches", filter: Builders<Branch>.Filter.Eq(field: "_id", value: id));
            var branch = GetLicenseCollection().Find(filter: filter).ToList().SelectMany(selector: x => x.Branches).FirstOrDefault();
            branch = branch != null && branch.StatusName == "Ліквідована" && branch.TerminateDate != null && branch.ParentId != 0
                ? GetBranchById(id: branch.ParentId)
                : branch;
            _requestLogsCollection.InsertOne(new RequestLog
            {
                Id = Guid.NewGuid(), RequestQuery = $"Method: GetBranchById, Id: {id}", LogDateTime = DateTime.Now, ResultObject = branch
            });
            return branch;
        }

        //public void DoStuff()
        //{
        //    BsonClassMap.RegisterClassMap<License>(cm => cm.AutoMap());
        //    BsonClassMap.RegisterClassMap<Branch>(cm => cm.AutoMap());

        //    ////how to insert
        //    //_mongoRepository.GetCollection<License>().InsertOne(new License {Id = 22, Branches = new List<Branch> {new Branch {Id = 324, Address = "test adress"}}});

        //    var col = _mongoRepository.GetCollection<License>();
        //    var builder1 = Builders<License>.Filter.Where(x => x.Id == 1722347);
        //    //how to get by id
        //    Stopwatch stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    var find = col.Find(builder1);
        //    stopwatch.Stop();
        //    var stopwatchElapsed = stopwatch.Elapsed;
        //    stopwatch.Start();
        //    var lic = find.SingleOrDefault(CancellationToken.None);
        //    stopwatch.Stop();
        //    var elapsed1 = stopwatch.Elapsed;

        //    //how to find branch
        //    var filter = Builders<License>.Filter.ElemMatch("Branches",
        //        Builders<Branch>.Filter.Eq("_id", 1891104));
        //    stopwatch.Restart();
        //    var lic2 = _mongoRepository.GetCollection<License>().Find(filter).ToList().SelectMany(x => x.Branches).FirstOrDefault();
        //    stopwatch.Stop();
        //    var timeSpan = stopwatch.Elapsed;
        //}
    }
}
