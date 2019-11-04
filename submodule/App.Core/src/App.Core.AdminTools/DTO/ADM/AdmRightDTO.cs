using App.Core.Data.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using App.Core.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.DTO.Common;

namespace App.Core.AdminTools.DTO.ADM
{
    public class AdmRightListDTO : BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [DisplayName("Назва")]
        [PredicateCase(PredicateOperation.Contains)]
        public string Name { get; set; }

        [DisplayName("Сутність")]
        [PredicateCase(PredicateOperation.Contains)]
        public string Entity { get; set; }

        [DisplayName("Активність")]
        public bool IsActive { get; set; }

        [DisplayName("Код")]
        public string Code { get; set; }
    }

    public class AdmRightDetailDTO : BaseDTO
    {
        [NotMapped]
        public string _ReturnUrl { get; set; }

        [Required]
        [DisplayName("Назва")]
        [PredicateCase(PredicateOperation.Contains)]
        public string Name { get; set; }

        [DisplayName("Сутність")]
        [PredicateCase(PredicateOperation.Equals)]
        public string Entity { get; set; }

        [Required]
        [DisplayName("Активність")]
        public bool IsActive { get; set; }
        
        [DisplayName("Код")]
        public string Code { get; set; }

        [NotMapped]
        public Dictionary<string, string> Properties { get; set; }

        public string PropertiesAsJson
        {
            get
            {
                return JsonConvert.SerializeObject(Properties,
                       settings: new JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.EscapeHtml });
            }
            set
            {
                Properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
            }
        }

        [DisplayName("Дозволи до полей обраної сутності")]
        [NotMapped]
        public string PropertiesAsJsonGrid
        {
            get
            {
                var propertiesJson = new List<SerJson>();
                if (Properties != null)
                {
                    foreach (var prop in Properties)
                    {
                        propertiesJson.Add(new SerJson { EntityFieldType = prop.Key, RightType = prop.Value });
                    }
                }
                return JsonConvert.SerializeObject(propertiesJson);
            }
            set
            {
                Properties = new Dictionary<string, string>();
                var values = JsonConvert.DeserializeObject<List<SerJson>>(value);
                values.ForEach(x => Properties.Add(x.EntityFieldType, x.RightType));
            }
        }

        internal class SerJson
        {
            public string EntityFieldType { get; set; }
            public string RightType { get; set; }
        }
    }
}
