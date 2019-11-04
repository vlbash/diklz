using System;
using System.Collections.Generic;
using System.Text;

namespace App.Data.Entities.REP
{
    public class ProgressBase
    {
        public string WorkTypeName { get; set; }

        public double TargetValueYesterday { get; set; }

        public double WttNumberofUnitsYesterday { get; set; }

        public double GwsNumberofUnitsYesterday { get; set; }

        public double TargetValueToday { get; set; }

        public double WttNumberofUnitsToday { get; set; }

        public double GwsNumberofUnitsToday { get; set; }

        public ProgressBase() { }

        public ProgressBase(string workTypeName)
        {
            WorkTypeName = workTypeName;
        }
    }
}
