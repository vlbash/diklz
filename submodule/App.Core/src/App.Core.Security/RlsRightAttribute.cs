using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Core.Security
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RlsRightAttribute: Attribute
    {
        /// <summary>
        /// The name of type, that is a subject of Row level security rights
        /// </summary>
        public string TypeName { get; }
        /// <summary>
        /// The name of Guid property, that contains a value to check with Row level security system 
        /// </summary>
        public string PropertyName { get; }

        public RlsRightAttribute(string typeName, string propertyName) : base()
        {
            TypeName = typeName;
            PropertyName = propertyName;
        }
    }
}
