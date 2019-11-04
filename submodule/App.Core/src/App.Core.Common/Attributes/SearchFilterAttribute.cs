using System;

namespace App.Core.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class SearchFilterAttribute: Attribute
    {
        public string LabelName { get; private set; }
        public string FieldType { get; private set; }
        public bool DataGroup { get; private set; }
        public int Order { get; private set; }

        public SearchFilterAttribute(string LabelName, string FieldType, int Order = 1, bool DataGroup = false)
        {
            this.LabelName = LabelName;
            this.FieldType = FieldType;
            this.DataGroup = DataGroup;
            this.Order = Order;
        }
    }
}
// attribute for all types exclusive of dataGroup:
//          [SearchFilter(LabelName: "some name", FieldType: "some type")]
// attribute for dataGroup:
//          [SearchFilter(LabelName: "some name", FieldType: "some type", DataGroup: true/false)]

