using App.Core.Data.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Interfaces;
using App.Core.Base;
using App.Core.Data.DTO.Common;

namespace App.Core.AdminTools.DTO.ADM
{
    public class AdmProfileListDTO : BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [DisplayName("Назва")]
        [PredicateCase(PredicateOperation.Contains)]
        public string Name { get; set; }

        [DisplayName("Активність")]
        public bool IsActive { get; set; }

    }

    public class AdmProfileDetailDTO : BaseDTO
    {
        [NotMapped]
        public string _ReturnUrl { get; set; }

        [Required]
        [DisplayName("Назва")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Активність")]
        public bool IsActive { get; set; }

        [DisplayName("Ролі")]
        public string RolesInfo { get; set; }

        [Required]
        [DisplayName("Ролі")]
        [NotMapped]
        public List<Guid> RolesList { get; set; }

        public string RolesString
        {
            get
            {
                if (RolesList == null)
                    return null;
                return String.Join("|", RolesList.ToArray());
            }
            set
            {
                RolesList = value?.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(Guid.Parse) ?? new List<Guid>();
            }
        }

        [DisplayName("Області")]
        public string RegionsInfo { get; set; }

        [Required]
        [DisplayName("Області")]
        [NotMapped]
        public List<Guid> RegionsList { get; set; }

        public string RegionsString
        {
            get
            {
                if (RegionsList == null)
                    return null;
                return String.Join("|", RegionsList.ToArray());
            }
            set
            {
                RegionsList = value?.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(Guid.Parse) ?? new List<Guid>();
            }
        }

    }
}
