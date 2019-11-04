using System;
using System.Collections.Generic;
using App.Core.Data.Helpers;
using App.Core.Security;

namespace App.Core.Business.Services
{
    public class UserInfo
    {
        public string Id { get; set; }
        public Dictionary<string, string> LoginData { get; set; }
        public Guid ProfileId { get; set; }
        public Guid UserId { get; set; }
        public Guid PersonId { get; set; }
        public UserApplicationRights Rights { get; set; }
        public UserCultureInfo UserCultureInfo { get; set; }
    }
}
