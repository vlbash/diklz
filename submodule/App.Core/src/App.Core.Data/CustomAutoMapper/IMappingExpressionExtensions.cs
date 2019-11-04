using App.Core.Data.Helpers;
using App.Core.Security;
using AutoMapper;

namespace App.Core.Data.CustomAutoMapper
{
    public static class AutoMapperExtensions
    {
        public static IMappingExpression<TSource, TDestination> MapOnlyIfChanged<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map, string entityNameForRightsChecking = null)
        {
            map.ForAllMembers(source =>
            {
                source.Condition((sourceObject, destObject, sourceProperty, destProperty, context) =>
                {
                    if (entityNameForRightsChecking != null
                        && context.Options.Items.TryGetValue("rights", out var objRights)
                        && objRights is UserApplicationRights rights)
                    {
                        rights.AssertWriteRights(entityNameForRightsChecking, source.DestinationMember.Name);
                    }

                    if (sourceProperty == null)
                    {
                        return !(destProperty == null);
                    }
                    return !sourceProperty.Equals(destProperty);
                });
            });
            return map;
        }

        public static IMappingExpression MapOnlyIfChanged(this IMappingExpression map, string entityNameForRightsChecking = null)
        {
            map.ForAllMembers(source =>
            {
                source.Condition((sourceObject, destObject, sourceProperty, destProperty, context) =>
                {
                    if (entityNameForRightsChecking != null
                        && context.Options.Items.TryGetValue("rights", out var objRights)
                        && objRights is UserApplicationRights rights)
                    {
                        rights.AssertWriteRights(entityNameForRightsChecking, source.DestinationMember.Name);
                    }

                    if (sourceProperty == null)
                    {
                        return !(destProperty == null);
                    }
                    return !sourceProperty.Equals(destProperty);
                });
            });
            return map;
        }
    }
}
