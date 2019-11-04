using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using App.Data.DTO.LIMS;
using Newtonsoft.Json;

namespace App.WebApi.Models
{
    public class LicenseLIMS
    {
        public int Id { get; set; }
        
        public int LicenseStatusId { get; set; }
        
        public string RegNumber { get; set; }

        public string OrderNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public string RegistrationDate { get; set; }

        public string TerminateDate { get; set; }

        public string LicenseTypesName { get; set; }

        public string LicenseTypesIds { get; set; }

        public string EDRPOU { get; set; }

        public string OrganizationName { get; set; }
        
        public string OrganizationAddress { get; set; }

        public string PostIndex { get; set; }

        public string Phone { get; set; }

        public string OrgDirector { get; set; }

        public string OwnershipType { get; set; }

        public string LegalFormType { get; set; }

        public string OrganizationFormName { get; set; }

        public string ListOfBranchesString { get; set; }


        [Description("List of branches")]
        public List<BranchLicense> Branches {
            get
            {
                var serializer = new XmlSerializer(typeof(List<BranchLicense>), new XmlRootAttribute("Branches"));
                return string.IsNullOrEmpty(ListOfBranchesString)
                    ? new List<BranchLicense>()
                    : serializer.Deserialize(new StringReader($"<Branches> {ListOfBranchesString} </Branches>")) as
                        List<BranchLicense>;
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
