using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
 using System.Linq;

namespace ConfigHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadJson(args[0]);
            //LoadJson("Production");
            //Console.WriteLine(args[0]);
        }

        static public void LoadJson(string envName)
        {

            var parList = new ParamList();
            using (var r = new StreamReader(@"D:\Projects\ConfigHelper\input\paramList.json"))
            {
                string json = r.ReadToEnd();
                //List<Param> items = JsonConvert.DeserializeObject<List<Param>>(json);
                 parList = JsonConvert.DeserializeObject<ParamList>(json);
            }

            var par = parList.env.Where(x=> x.environment == envName).First();

            //envName

            var settings  =  new Appsetting();
            using (var r = new StreamReader(@"D:\Projects\ConfigHelper\input\appsettings.json"))
            {
                string json = r.ReadToEnd();
                //var items1 = JsonConvert.DeserializeObject<List<Item>>(json);
                settings = JsonConvert.DeserializeObject<Appsetting>(json);
            }

            settings.ConnectionStrings.DefaultConnection = par.connectionString;

            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter($@"D:\Projects\ConfigHelper\input\appsettings.{envName}.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, settings);
                // {"ExpiryDate":new Date(1230375600000),"Price":0}
            }

            // 1. read appsetting file
            // 2. create model appsetting
            // 3. seve file appsetting.Test.json 
            
        }

        public class Param
        {

            public string  environment;
            public string connectionString;
            public string Authority;
            public string FileStorePath;
            public string TemplatePath;
            public string RowCount;
        }

        public class ParamList
        {
            public List<Param>  env;
        }

        public class Appsetting
        {
            public ConnectionStrings  ConnectionStrings;
            public string AppName;
            public bool? SeedDB;
            public Serilog Serilog;
            public Identity Identity;
            public Appsettings Appsettings;
            public Encryption Encryption;
            public Paging Paging;
        }
        public class ConnectionStrings
        {
            public string  DefaultConnection;
        }
        public class Serilog
        {
           public string[] Using = {};
           public string MinimumLevel;
           public  writeTo[] writeTo = {};
           public string[] Enrich = {};
        }
       public class writeTo
        {
            public string Name;
            public Args Args;
        } 
        public class Args
        {
            public string pathFormat;
            public bool? shared;
        }
        public class Identity
        {
            public bool? UseIdentity;
            public string Authority;
            public string ResponseType;
            public Client Client;
        }
        public class Client
        {
            public string Id;
            public string Secret;
            public string[] Scopes;
        }
        public class Appsettings
        {
            public string FileStorePath;
            public string TemplatePath;
        }
        public class Encryption
        {
            public string Key;
            public string IV;
        }     
        public class Paging
        {
            public string RowCount;
        }
    }
}
