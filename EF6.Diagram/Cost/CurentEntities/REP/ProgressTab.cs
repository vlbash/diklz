using System;
using System.Collections.Generic;
using System.Text;

namespace App.Data.Entities.REP
{
    public class ProgressTab : ProgressBase
    {
        public double ResultYesterday { get; }

        public double ResultToday { get; }

        public double DiffYesterday { get; }

        public double DiffToday { get; }

        public double ResultTotal { get; }

        public double DiffTotal { get; }

        public double TargetValueTotal { get; }

        public ProgressTab()
        {

        }

        public ProgressTab(string workTypeName)
        {
            WorkTypeName = workTypeName;
        }

        public ProgressTab(ProgressBase b)
        {
            WorkTypeName = b.WorkTypeName;
            TargetValueYesterday = b.TargetValueYesterday;
            TargetValueToday = b.TargetValueToday;
            WttNumberofUnitsYesterday = b.WttNumberofUnitsYesterday;
            WttNumberofUnitsToday = b.WttNumberofUnitsToday;
            GwsNumberofUnitsYesterday = b.GwsNumberofUnitsYesterday;
            GwsNumberofUnitsToday = b.GwsNumberofUnitsToday;

            if (b.WttNumberofUnitsYesterday != 0)
            {
                ResultYesterday = b.GwsNumberofUnitsYesterday / b.WttNumberofUnitsYesterday;
            }

            if (b.WttNumberofUnitsToday != 0)
            {
                ResultToday = b.GwsNumberofUnitsToday / b.WttNumberofUnitsToday;
            }

            DiffYesterday = b.TargetValueYesterday - ResultYesterday;

            DiffToday = b.TargetValueToday - ResultToday;

            ResultTotal = ResultYesterday + ResultToday;

            DiffTotal = DiffYesterday + DiffToday;

            TargetValueTotal = b.TargetValueYesterday + b.TargetValueToday;
        }
    }
}
