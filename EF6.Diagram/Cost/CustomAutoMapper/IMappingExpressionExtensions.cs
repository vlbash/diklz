
using AutoMapper;

namespace Astum.Core.Data.CustomAutoMapper
{
    public static class AutoMapperExtensions
    {
        public static IMappingExpression<TSource, TDestination> MapOnlyIfChanged<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map, string EntityNameForRightsChecking = null)
        {
            map.ForAllMembers(source =>
            {
                source.Condition((sourceObject, destObject, sourceProperty, destProperty, context) =>
                {
                    if (EntityNameForRightsChecking != null)
                    {
                    }

                    if (sourceProperty == null)
                        return !(destProperty == null);
                    return !sourceProperty.Equals(destProperty);
                });
            });
            return map;
        }
    }
}
