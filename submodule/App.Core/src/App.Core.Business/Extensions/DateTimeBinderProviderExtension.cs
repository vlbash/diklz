using System;
using System.Collections.Generic;
using System.Text;

namespace App.Core.Business.Extensions
{
    using App.Core.Business.Services;
    using App.Core.Data.Helpers;
    using App.Core.Data.Interfaces;
    using App.Core.Data.Providers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    /// <summary>  
    /// <see cref="DateTimeBinderProvider"/> initializer  
    /// </summary>  
    public static class DateTimeBinderProviderExtension
    {
        /// <summary>  
        /// Registers the date time provider.  
        /// </summary>  
        /// <param name="option">The option.</param>  
        /// <param name="serviceCollection">The service collection.</param>  
        /// <returns></returns>  
        /// <exception cref="System.ArgumentNullException">option</exception>  
        public static MvcOptions RegisterDateTimeProvider(this MvcOptions option,
          IServiceCollection serviceCollection)
        {
            if (option == null) {
                throw new ArgumentNullException(nameof(option));
            }
            // TODO: BuildServiceProvider could be optimized
            option.ModelBinderProviders.Insert(0, new DateTimeBinderProvider(
              () => serviceCollection.BuildServiceProvider().GetService<IUserInfoService>()
              ));
            return option;
        }
    }
}
