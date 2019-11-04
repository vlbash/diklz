using System;
using System.Collections.Generic;
using System.Text;
using App.Core.Business.Services;
using App.Core.Data.Binders;
using App.Core.Data.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace App.Core.Data.Providers
{
    /// <summary>  
    /// <see cref="DateTimeBinder"/> provider.  
    /// </summary>  
    /// <seealso cref="IModelBinderProvider" />  
    public class DateTimeBinderProvider: IModelBinderProvider
    {
        /// <summary>  
        /// The user culture  
        /// </summary>  
        protected readonly Func<IUserInfoService> UserInfoService;
        /// <summary>  
        /// Initializes a new instance of the <see cref="DateTimeBinderProvider"/> class.  
        /// </summary>  
        /// <param name="userCulture">The user culture.</param>  
        public DateTimeBinderProvider(Func<IUserInfoService> userInfoService)
        {
            UserInfoService = userInfoService;
        }
        /// <summary>  
        /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder" /> based on <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBinderProviderContext" />.  
        /// </summary>  
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBinderProviderContext" />.</param>  
        /// <returns>  
        /// An <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder" />.  
        /// </returns>  
        /// <exception cref="System.ArgumentNullException">context</exception>  
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }
            if (context.Metadata.UnderlyingOrModelType == typeof(DateTime)) {
                return new DateTimeBinder(UserInfoService());
            }
            return null; // TODO: Find alternate.  
        }
    }
}
