using System;
using System.Collections.Generic;
using System.ComponentModel;
using App.Core.Common.Attributes;

namespace App.Data.DTO.LOG
{
    public class UserIdentLog
    {
        public string UserGuid { get; set; }

        public Dictionary<string, string> LoginData { get; set; }

        public Guid PersonId { get; set; }

        [DisplayName("Профіль")]
        [RequiredNonDefault(ErrorMessage = "Профіль не вибрано")]
        public Guid ProfileId { get; set; }

        [DisplayName("Співробітник")]
        [RequiredNonDefault(ErrorMessage = "Робітника не вибрано")]
        public Guid EmployeeId { get; set; }
    }
}
