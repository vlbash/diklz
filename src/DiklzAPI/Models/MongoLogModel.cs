using System;
using System.Collections.Generic;

namespace App.WebApi.Models
{
    internal class MongoLogModel
    {
        public Guid Id { get; set; }

        public IList<string> LoggingList { get; set; }

        public DateTime LogDate { get; set; }
    }
}
