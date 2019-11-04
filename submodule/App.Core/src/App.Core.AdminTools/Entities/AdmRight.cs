using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.AdminTools.Entities.JoinTables;
using App.Core.Data.Entities.Common;
using App.Core.Data.Interfaces;
using Newtonsoft.Json;

namespace App.Core.AdminTools.Entities
{
    public class AdmRight : BaseEntity, IUserRight
    {
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(64)]
        public string Entity { get; set; }

        public string Code { get; set; }

        [NotMapped]
        public Dictionary<string, string> Properties { get; set; }

        public string PropertiesAsJson
        {
            get
            {
                return JsonConvert.SerializeObject(Properties,
                    new JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.EscapeHtml });
            }
            set
            {
                Properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
            }
        }

        public bool IsActive { get; set; }

        public List<AdmRoleAdmRight> Roles { get; set; }
    }
}
