using System.ComponentModel.DataAnnotations;

namespace App.Core.Common.Attributes
{
    public class MoneyAttribute: DataTypeAttribute
    {
        public MoneyAttribute(): base(DataType.Currency)
        {
            DisplayFormat.DataFormatString = "{0:f2}";
            DisplayFormat.ApplyFormatInEditMode = true;
        }

        public override string GetDataTypeName()
        {
            return "Money";
        }
    }
}
