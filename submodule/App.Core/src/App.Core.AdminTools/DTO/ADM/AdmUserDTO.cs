using App.Core.Data.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.DTO.Common;

namespace App.Core.AdminTools.DTO.ADM
{
    public class AdmUserListDTO : BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [DisplayName("Логін")]
        [PredicateCase(PredicateOperation.Contains)]
        public string Name { get; set; }

        [DisplayName("Ім'я персони")]
        [PredicateCase(PredicateOperation.Contains)]
        public string PersonName { get; set; }

    }

    public class AdmUserDetailDTO : BaseDTO
    {
        [NotMapped]
        public string _ReturnUrl { get; set; }

        [Required]
        [DisplayName("Логін")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Ім'я персони")]
        public string PersonName { get; set; }

        public Guid PersonId { get; set; }

        [DisplayName("Ролі")]
        public string ProfilesInfo { get; set; }

        [Required]
        [DisplayName("Профілі")]
        [NotMapped]
        public List<Guid> ProfilesList { get; set; }

        public string ProfilesString
        {
            get
            {
                if (ProfilesList == null)
                    return null;
                return String.Join("|", ProfilesList.ToArray());
            }
            set
            {
                ProfilesList = value?.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(Guid.Parse) ?? new List<Guid>();
            }
        }
    }
}
