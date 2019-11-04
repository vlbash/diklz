using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using App.Core.Data.CustomAutoMapper;
using App.Core.Data.Interfaces;
using AutoMapper;

namespace App.Core.Business.Services.ObjectMapper
{
    public class ObjectMapper: IObjectMapper
    {
        private static readonly Dictionary<Type, Dictionary<Type, IMapper>> _mappers = new Dictionary<Type, Dictionary<Type, IMapper>>();

        public TDestination Map<TDestination>(object source, TDestination destination = null)
            where TDestination : class
        {
            var mapper = GetMapperInternal(source.GetType(), typeof(TDestination));
            if (destination != null)
            {

                return mapper.Map(source, destination);
            }

            return mapper.Map<TDestination>(source);

        }

        private IMapper GetMapperInternal(Type sourceType, Type destinationType)
        {
            IMapper mapper = null;
            if (_mappers.TryGetValue(sourceType, out var typeMappers))
            {
                if (typeMappers.TryGetValue(destinationType, out mapper))
                {
                    return mapper;
                }
            }
            else
            {
                typeMappers = new Dictionary<Type, IMapper>();
                _mappers[sourceType] = typeMappers;
            }

            if (mapper == null)
            {
                if (typeof(IMapped).IsAssignableFrom(sourceType))
                {
                    mapper = (IMapper)sourceType.GetProperty("GetMapper", BindingFlags.Public | BindingFlags.Static).GetValue(null, null);
                }
                else
                {
                    mapper = new MapperConfiguration(cfg =>
                        cfg.CreateMap(sourceType, destinationType)
                            .MapOnlyIfChanged(destinationType.Name))
                            .CreateMapper();
                }
            }

            typeMappers[destinationType] = mapper;
            return mapper;
        }

    }
}
