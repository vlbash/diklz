using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.Core.Common.Attributes
{
    public class DocumentDateAttribute: DataTypeAttribute
    {
        public DocumentDateAttribute() : base(DataType.Date)
        {
            DisplayFormat.DataFormatString = "{0:s}";
            DisplayFormat.ApplyFormatInEditMode = true;
        }

        public override string GetDataTypeName()
        {
            return "Document date";
        }
    }
}
