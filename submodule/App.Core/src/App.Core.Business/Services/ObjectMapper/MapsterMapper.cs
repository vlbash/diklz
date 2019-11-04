using System;
using Mapster;

namespace App.Core.Business.Services.ObjectMapper
{
    public class MapsterMapper: IObjectMapper
    {
        public TDestination Map<TDestination>(object source, TDestination destination = null)
            where TDestination : class
        {
            if (destination != null)
            {
                return source.Adapt(destination);
            }
            return source.Adapt<TDestination>();
        }
    }
}
