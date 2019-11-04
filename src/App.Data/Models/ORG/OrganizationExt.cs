using System.ComponentModel.DataAnnotations;
using App.Core.Data.Entities.ORG;

namespace App.Data.Models.ORG
{
    public class OrganizationExt: Organization
    {
        #region Organization protected data
        //this fields may be only changed in specific documents 
        //or if organization has not obtained license yet
        //do not allow to change this, only via re-creating organization and inserting parent Id

        [MaxLength(30)]
        public string EDRPOU { get; set; }
        
        [MaxLength(30)]
        public string INN { get; set; }
      
        #endregion

        [MaxLength(100)]
        public string EMail { get; set; }
    }
}
