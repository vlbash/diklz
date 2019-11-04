using System;
using System.Collections.Generic;
using System.Text;

namespace App.Core.Business.Extensions
{
    using App.Core.Business.Services;
    using App.Core.Data.Converters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    /// <summary>  
    /// <see cref="DateTimeConverter"/> initializer.  
    /// </summary>  
    public static class DateTimeConverterExtension
    {
        /// <summary>  
        /// Registers the date time converter.  
        /// </summary>  
        /// <param name="option">The option.</param>  
        /// <param name="serviceCollection">The service collection.</param>  
        /// <returns></returns>  
        public static MvcJsonOptions RegisterJsonDateTimeConverter(this MvcJsonOptions option,
          IServiceCollection serviceCollection)
        {
            // TODO: BuildServiceProvider could be optimized  
            option.SerializerSettings.Converters.Add(
                new JsonDateTimeConverter(() => serviceCollection.BuildServiceProvider().GetService<IUserInfoService>())
                );
            return option;
        }
    }
}
