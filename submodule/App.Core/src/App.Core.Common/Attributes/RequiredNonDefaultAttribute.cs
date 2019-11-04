using System;
using System.ComponentModel.DataAnnotations;

namespace App.Core.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredNonDefaultAttribute: ValidationAttribute
    {
        public RequiredNonDefaultAttribute()
            : base("{0}: поле не заповнене") {
        }

        public override bool IsValid(object value) {
            return value != null && !Equals(value, Activator.CreateInstance(value.GetType()));
        }
    }
}
