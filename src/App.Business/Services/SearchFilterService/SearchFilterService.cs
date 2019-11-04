//using System;
//using System.Collections.Generic;
//using App.Core.Common.Attributes;
//using Newtonsoft.Json;
//using Microsoft.Extensions.Caching.Memory;
//using App.Core.Data.Helpers;

//namespace App.Business.Services.SearchFilterService
//{
//    public class SearchFilterService : ISearchFilterService
//    {
//        private readonly IMemoryCache _cache;

//        public SearchFilterService(IMemoryCache cache)
//        {
//            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
//        }

//        public string GenerateInputConfig(Type dtoType)
//        {
//            var cacheName = dtoType.FullName + "Filter";
//            if (!_cache.TryGetValue(cacheName, out List<object> result))
//            {
//                result = new List<object>();
//                try
//                {
//                    var dtoMembers = dtoType.GetMembers();
//                    foreach (var member in dtoMembers)
//                    {


//                        var fieldAttributes = member.GetCustomAttributes(typeof(SearchFilterAttribute), true);
//                        foreach (var attribute in fieldAttributes)
//                        {
//                            var t = attribute.GetPropValue("LabelName");

//                            if (attribute.GetPropValue("FieldType").Equals("text"))
//                            {
//                                result.Add(new
//                                {
//                                    labelName = attribute.GetPropValue("LabelName"),
//                                    name = member.Name,
//                                    type = attribute.GetPropValue("FieldType")
//                                });
//                            }
//                            else if (attribute.GetPropValue("FieldType").Equals("select"))
//                            {
//                                var selectOptions = new Dictionary<string, string>();
//                                selectOptions.Add("entity", attribute.GetPropValue("Entity").ToString());
//                                selectOptions.Add("entityType", attribute.GetPropValue("EntityType").ToString());

//                                if (attribute.GetPropertyValueDictionary().ContainsKey("Expression") && attribute.GetPropValue("Expression") != null)
//                                {
//                                    selectOptions.Add("expression", attribute.GetPropValue("Expression").ToString());
//                                }
//                                result.Add(new
//                                {
//                                    labelName = attribute.GetPropValue("LabelName"),
//                                    name = member.Name,
//                                    type = attribute.GetPropValue("FieldType"),
//                                    selectOptions
//                                });
//                            }

//                        }
//                    }
//                    _cache.Set(cacheName, result);
//                }
//                catch (Exception e)
//                {
//                    Console.WriteLine("An exception occurred: {0}", e.Message);
//                }
//            }
            
//            return JsonConvert.SerializeObject(result);
//        }
//    }
//}
