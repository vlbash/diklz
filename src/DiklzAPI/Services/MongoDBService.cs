using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Common.Extensions;
using App.WebApi.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;

namespace App.WebApi.Services
{
    internal class MongoDbService
    {
        private IMongoRepository _mongoRepository;

        public MongoDbService(IMongoRepository mongoRepository)
        {
            _mongoRepository = mongoRepository;
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

        public void DeleteLicenses()
        {
            try
            {
                _mongoRepository
                    .GetCollection<License>().Database.DropCollection("License");
            }
            catch (Exception e)
            {
                Log.Error(e, "Error deleting license to MongoDb");
                throw;
            }
        }


        public void SaveLicenses(IEnumerable<License> licenses)
        {
            try
            {
                foreach (var license in licenses)
                {
                    _mongoRepository
                        .GetCollection<License>()
                        .ReplaceOne(
                            x => x.Id == license.Id,
                            replacement: license,
                            options: new UpdateOptions {IsUpsert = true}
                        );
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Error saving license to MongoDb");
                throw;
            }
        }

        public async Task InsertLogs(IList<string> loggingList)
        {
            try
            {
                await _mongoRepository
                    .GetCollection<MongoLogModel>("Logs")
                    .InsertOneAsync( new MongoLogModel{Id = Guid.NewGuid(), LoggingList = loggingList, LogDate = DateTime.Now});
            }
            catch (Exception e)
            {
                Log.Error(e, "Error saving logs to MongoDb");
            }
        }
    }

    internal interface IMongoRepository
    {
        IMongoCollection<T> GetCollection<T>(string nameOfCollection = "");
        T GetEntityById<T>(int id);
        B GetNestedElementById<T,B>(int id) where B : class;
        IMongoDatabase GetDatabase();
    }

    internal class MongoRepository : IMongoRepository
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        private IConfiguration Configuration;

        public MongoRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            _client = new MongoClient(Configuration.GetConnectionString("MongoConnection"));
            _database = _client.GetDatabase(Configuration.GetSection("MongoDatabaseName").Value);
        }

        public IMongoCollection<T> GetCollection<T>(string nameOfCollection = "")
        {
            return _database.GetCollection<T>(string.IsNullOrEmpty(nameOfCollection) ? typeof(T).Name : nameOfCollection);
        }

        public IMongoDatabase GetDatabase()
        {
            return _database;
        }

        public T GetEntityById<T>(int id)
        {
            return GetCollection<T>().Find(new BsonDocument("_id",id)).FirstOrDefault();
        }

        public B GetNestedElementById<T, B>(int id)
            where B: class 
        {
            var filter = Builders<T>.Filter.ElemMatch(typeof(B).Name,
                Builders<B>.Filter.Eq("_id", id));
            var coll = GetCollection<T>().Find(filter).ToList();
            return coll.Select(x => x.GetPropValue(typeof(B).Name)).FirstOrDefault() as B;
        }
    }
}
