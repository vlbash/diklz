using System;
using AutoMapper;

namespace App.Core.Business.Services.ObjectMapper
{
    public interface IObjectMapper
    {
        TDestination Map<TDestination>(object source, TDestination destination = null) where TDestination : class;
    }
}
