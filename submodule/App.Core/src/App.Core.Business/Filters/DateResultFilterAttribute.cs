using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace App.Core.Business.Filters
{
    // as for now this filter uses reflection to search for all DateTime properties
    //TODO: to cache metadata of DTOs somewhere (Redis?) and use that cache instead of reflection
    public class DateResultFilterAttribute: IAsyncResultFilter
    {
        private readonly IMemoryCache _cache;

        public DateResultFilterAttribute(IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context,
                                                    ResultExecutionDelegate next)
        {
            object model = null;
            if (context.Result is Microsoft.AspNetCore.Mvc.ViewResult viewResult)
            {
                model = viewResult.Model;
            }
            else if (context.Result is Microsoft.AspNetCore.Mvc.ObjectResult objResult)
            {
                model = objResult.Value;
            }
            else if (context.Result is Microsoft.AspNetCore.Mvc.PartialViewResult partialViewResult)
            {
                model = partialViewResult.Model;
            }

            //if (model != null)
            //{
            //    if (model is IList modelList)
            //    {
            //        foreach (var item in modelList)
            //        {
            //            ChangeDatesToLocal(item);
            //        }
            //    }
            //    else
            //    {
            //        ChangeDatesToLocal(model);
            //    }
            //}  todo закоментовано для відповідності данних між базою і view

            await next();
        }

        private void ChangeDatesToLocal(object model)
        {
            var modelType = model.GetType();

            if (!_cache.TryGetValue(modelType, out IEnumerable<PropertyInfo> dateProps))
            {
                PropertyInfo[] propInfos = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                dateProps = propInfos.Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(30));

                _cache.Set(modelType, dateProps);
            }

            foreach (var prop in dateProps)
            {
                var propValue = prop.GetValue(model);
                if (propValue == null)
                {
                    continue;
                }
                var propType = prop.GetType();

                DateTime date;
                if (propType == typeof(DateTime?))
                {
                    if (propValue is null)
                    {
                        continue;
                    }
                    date = ((DateTime?)propValue).GetValueOrDefault(DateTime.Now);
                }
                else
                {
                    date = (DateTime)propValue;
                }

                prop.SetValue(model, date.ToLocalTime());
            }
        }
    }
}
