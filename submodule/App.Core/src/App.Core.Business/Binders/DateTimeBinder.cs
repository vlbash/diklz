using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Core.Data.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace App.Core.Data.Binders
{
    /// <summary>  
    /// Date time model binder.  
    /// </summary>  
    /// <seealso cref="IModelBinder" />  
    public class DateTimeBinder
        : IModelBinder
    {
        /// <summary>  
        /// The user culture  
        /// </summary>  
        protected readonly UserCultureInfo UserCultureInfo;
        /// <summary>  
        /// Initializes a new instance of the <see cref="DateTimeBinder"/> class.  
        /// </summary>  
        /// <param name="userCulture">The user culture.</param>  
        public DateTimeBinder(IUserInfoService userInfoService)
        {
            UserCultureInfo = userInfoService.GetCurrentUserInfo().UserCultureInfo;
        }
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) {
                throw new ArgumentNullException(nameof(bindingContext));
            }
            var valueProviderResult = bindingContext.ValueProvider
              .GetValue(bindingContext.ModelName);
            if (string.IsNullOrEmpty(valueProviderResult.FirstValue)) {
                ModelBindingResult.Failed();
            }
            else if (DateTime.TryParse(valueProviderResult.FirstValue, null, DateTimeStyles.AdjustToUniversal, out var datetime)) {
                // TODO: now UserCultureInfo is null. find out why
                var resultDate = UserCultureInfo?.GetUtcTime(datetime) ?? datetime;
                bindingContext.Result =
                    ModelBindingResult.Success(resultDate);
            }
            else {
                // TODO: [Enhancement] Could be implemented in better way.  
                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName,
                    bindingContext.ModelMetadata
                    .ModelBindingMessageProvider.AttemptedValueIsInvalidAccessor(
                      valueProviderResult.ToString(), nameof(DateTime)));
            }
            return Task.CompletedTask;
        }
    }
}
