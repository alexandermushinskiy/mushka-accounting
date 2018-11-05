using System;
using System.ComponentModel.DataAnnotations;

namespace Mushka.WebApi.Filters
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class RequireNonDefaultAttribute : ValidationAttribute
    {
        public RequireNonDefaultAttribute()
            : base("The {0} field is required.")
        {
        }

        public override bool IsValid(object value)
        {
            return value != null && !Equals(value, Activator.CreateInstance(value.GetType()));
        }
    }
}