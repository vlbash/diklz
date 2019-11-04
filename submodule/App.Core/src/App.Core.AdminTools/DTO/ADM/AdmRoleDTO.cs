using App.Core.Data.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Interfaces;
using App.Core.Data.DTO.Common;

namespace App.Core.AdminTools.DTO.ADM
{
    public class AdmRoleListDTO : BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [DisplayName("Назва")]
        [PredicateCase(PredicateOperation.Contains)]
        public string Name { get; set; }

        [DisplayName("Код")]
        public string Code { get; set; }

        [DisplayName("Активність")]
        public bool IsActive { get; set; }
    }

    public class AdmRoleDetailDTO : BaseDTO
    {
        [NotMapped]
        public string _ReturnUrl { get; set; }

        [Required]
        [DisplayName("Назва")]
        public string Name { get; set; }

        [DisplayName("Код")]
        public string Code { get; set; }

        [Required]
        [DisplayName("Активність")]
        public bool IsActive { get; set; }

        [DisplayName("Права")]
        public string RightsInfo { get; set; }        

        [Required]
        [DisplayName("Права")]
        [NotMapped]
        public List<Guid> RightsList { get; set; }

        public string RightsString
        {
            get
            {
                if (RightsList == null)
                    return null;
                return String.Join("|", RightsList.ToArray());
            }
            set
            {
                RightsList = value?.Split(new []{'|'}, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(Guid.Parse) ?? new List<Guid>();
            }
        }
    }
}
