namespace App.WebApi.Models
{
    public class Branch
    {
        public int Id { get; set; }

        public int LicenseId { get; set; }

        public string StatusName { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string LicTypes { get; set; }

        public string RegionCode { get; set; }
        
        public string ResidenceTypeName { get; set; }

        public string PostIndex { get; set; }

        public string Address { get; set; }

        public string RegistrationDate { get; set; }

        public string TerminateDate { get; set; }

       // public int ParentId { get; set; }
    }
}
