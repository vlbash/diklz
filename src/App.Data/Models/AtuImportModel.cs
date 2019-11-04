using System.Collections.Generic;

namespace App.Data.Models
{
    public class AtuImport
    {
        public class ATU
        {
            public List<leve1Info> level1 { get; set; } = new List<leve1Info>();
        }

        public class leve1Info
        {
            public string code { get; set; }
            public string name { get; set; }
            public List<leve2Info> level2 { get; set; } = new List<leve2Info>();
        }

        public class leve2Info
        {
            public string code { get; set; }
            public string name { get; set; }
            public List<leve3Info> level3 { get; set; } = new List<leve3Info>();
        }

        public class leve3Info
        {
            public string code { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public List<leve4Info> level4 { get; set; } = new List<leve4Info>();
        }

        public class leve4Info
        {
            public string code { get; set; }
            public string name { get; set; }
            public string type { get; set; }
        }
    }
}
