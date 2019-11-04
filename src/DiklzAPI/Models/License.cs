using System.Collections.Generic;
using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace App.WebApi.Models
{
    internal class License
    {
        [BsonId]
        public int Id { get; set; }
        
        public int LicenseStatusId { get; set; }
        
        public string RegNumber { get; set; }
        
        public string EDRPOU { get; set; }

        public string OrganizationName { get; set; }
        
        public string OrganizationAddress { get; set; }

        public string RegistrationDate { get; set; }

        public string TerminateDate { get; set; }

        public string LicenseTypesName { get; set; }

        public string OrganizationFormName { get; set; }

        [BsonIgnore]
        public string ListOfBranchesString { get; set; }

        [Description("List of branches")]
        public IEnumerable<Branch> Branches { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
