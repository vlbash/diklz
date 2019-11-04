using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace App.WebApi.SOAP
{
    [ServiceContract]
    public interface IWebApiService
    {
        [OperationContract]
        License GetLicenseByEdrpou(string edrpou);

        [OperationContract]
        Branch GetBranchById(int id);
        
        [OperationContract]
        License GetLicenseById(int id);
    }

    [DataContract]
    public class License
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int LicenseStatusId { get; set; }

        [DataMember]
        public string RegNumber { get; set; }

        [DataMember]
        public string EDRPOU { get; set; }

        [DataMember]
        public string OrganizationName { get; set; }

        [DataMember]
        public string OrganizationAddress { get; set; }

        [DataMember]
        public string RegistrationDate { get; set; }

        [DataMember]
        public string TerminateDate { get; set; }

        [DataMember]
        public string LicenseTypesName { get; set; }

        [DataMember]
        public string OrganizationFormName { get; set; }
        
        [DataMember]
        public IEnumerable<Branch> Branches { get; set; }
    }

    [DataContract]
    public class Branch
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int LicenseId { get; set; }

        [DataMember]
        public string StatusName { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string LicTypes { get; set; }

        [DataMember]
        public string RegionCode { get; set; }

        [DataMember]
        public string ResidenceTypeName { get; set; }

        [DataMember]
        public string PostIndex { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string RegistrationDate { get; set; }

        [DataMember]
        public string TerminateDate { get; set; }

        [DataMember]
        public int ParentId { get; set; }
    }

    public class RequestLog
    {
        public Guid Id { get; set; }

        public string RequestQuery { get; set; }

        public DateTime LogDateTime { get; set; }

        public object ResultObject { get; set; }
    }
}
