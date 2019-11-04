using System;
using System.Collections.Generic;
using System.Linq;
using App.Core.Data.Attributes;
using System.Reflection;

namespace App.Core.Data.Helpers
{
    public class PredicateBuildHelperOld<T>
    {
        static Dictionary<PropertyInfo, PredicateCase> _properties;
        static string alias = "qry_x";

        class SearchCase
        {
            public string PropName { get; set; }
            public string InputName { get; set; }
            public string Value { get; set; }
            public PropertyInfo PropertyInfo { get; set; }
            public PredicateCase PredicateCase { get; set; }
        }

        public static string BuildPredicate(IDictionary<string, string> paramList)
        {
            string predicate = null;

            if (paramList == null)
                return null;

            if (paramList.Count == 0)
                return null;

            if (_properties == null)
            {
                _properties = typeof(T).GetProperties()
                    .Where(p => p.IsDefined(typeof(PredicateCase), true))
                    .ToDictionary(p => p,
                                  p => (PredicateCase)p.GetCustomAttributes(typeof(PredicateCase), true).First());
            }

            var cases = new List<SearchCase>();
            foreach (var item in paramList)
            {
                var sc = new SearchCase();

                if (string.IsNullOrEmpty(item.Key) || string.IsNullOrEmpty(item.Value) || item.Key == "X-Requested-With") continue;

                sc.PropName = item.Key.ToLower();
                sc.InputName = item.Key.ToLower();
                sc.Value = item.Value;

                if (sc.PropName.EndsWith("_from"))
                    sc.PropName = sc.PropName.Replace("_from", "");
                else if (sc.PropName.EndsWith("_to"))
                    sc.PropName = sc.PropName.Replace("_to", "");

                sc.PropertyInfo = _properties.Keys.FirstOrDefault(x => x.Name.ToLower() == sc.PropName.ToLower());
                if (sc.PropertyInfo != null && _properties.TryGetValue(sc.PropertyInfo, out var pcase))
                {
                    sc.PredicateCase = pcase;
                    sc.PropName = sc.PropertyInfo.Name;

                    if (sc.PropertyInfo.PropertyType == typeof(bool))
                        sc.Value = (sc.Value == "on").ToString();
                    
                    cases.Add(sc);
                }
            }

            foreach (var item in cases.Where(x => x.PredicateCase.Operation == PredicateOperation.Equals))
            {
                if (item.PropertyInfo.PropertyType == typeof(string) ||
                    item.PropertyInfo.PropertyType == typeof(Guid?) ||
                    item.PropertyInfo.PropertyType == typeof(Guid))
                {
                    predicate += $" AND ({alias}.\"{item.PropName}\" = '{item.Value}')";
                }
                else
                if (item.PropertyInfo.PropertyType == typeof(double) ||
                    item.PropertyInfo.PropertyType == typeof(decimal) ||
                    item.PropertyInfo.PropertyType == typeof(long) ||
                    item.PropertyInfo.PropertyType == typeof(bool) ||
                    item.PropertyInfo.PropertyType == typeof(int))
                {
                    predicate += $" AND ({alias}.\"{item.PropName}\" = {item.Value})";
                }
            }

            foreach (var item in cases.Where(x => x.PredicateCase.Operation == PredicateOperation.Contains))
            {
                predicate += $" AND (POSITION('{item.Value.ToLower()}' in LOWER({alias}.\"{item.PropName}\")) > 0)";
            }

            foreach (var item in cases.Where(x => x.PredicateCase.Operation == PredicateOperation.ValueRange))
            {
                string[] dates;

                if (item.Value.Contains('&'))
                {
                    dates = item.Value.Split('&');
                    dates[0] = DateTime.Parse(dates[0], null, System.Globalization.DateTimeStyles.RoundtripKind).ToString();
                    dates[1] = DateTime.Parse(dates[1], null, System.Globalization.DateTimeStyles.RoundtripKind).ToString();
                }
                else
                {
                    dates = item.Value.Split('-');
                }

                if (item.PropertyInfo.PropertyType == typeof(string) ||
                    item.PropertyInfo.PropertyType == typeof(Guid?) ||
                    item.PropertyInfo.PropertyType == typeof(Guid) ||
                    item.PropertyInfo.PropertyType == typeof(DateTime) ||
                    item.PropertyInfo.PropertyType == typeof(DateTime?))
                {
                    predicate += $" AND ({alias}.\"{item.PropName}\" >= '{dates[0]}')";
                    predicate += $" AND ({alias}.\"{item.PropName}\" <= '{dates[1]}')";
                }
                else
                if (item.PropertyInfo.PropertyType == typeof(double) ||
                    item.PropertyInfo.PropertyType == typeof(double?) ||
                    item.PropertyInfo.PropertyType == typeof(decimal) ||
                    item.PropertyInfo.PropertyType == typeof(decimal?) ||
                    item.PropertyInfo.PropertyType == typeof(long) ||
                    item.PropertyInfo.PropertyType == typeof(long?) ||
                    item.PropertyInfo.PropertyType == typeof(int) ||
                    item.PropertyInfo.PropertyType == typeof(int?))
                {
                    predicate += $" AND ({alias}.\"{item.PropName}\" >= {dates[0]})";
                    predicate += $" AND ({alias}.\"{item.PropName}\" <= {dates[1]})";
                };
            }

            foreach (var item in cases.Where(x => x.PredicateCase.Operation == PredicateOperation.InputRange))
            {
                string oper;

                if (item.InputName.EndsWith("_from"))
                    oper = ">=";
                else if (item.InputName.EndsWith("_to"))
                    oper = "<=";
                else
                    oper = "=";

                if (item.PropertyInfo.PropertyType == typeof(string) ||
                    item.PropertyInfo.PropertyType == typeof(Guid?) ||
                    item.PropertyInfo.PropertyType == typeof(Guid))
                {
                    predicate += $" AND ({alias}.\"{item.PropName}\" {oper} '{item.Value}')";
                }
                else
                if (item.PropertyInfo.PropertyType == typeof(double) ||
                    item.PropertyInfo.PropertyType == typeof(double?) ||
                    item.PropertyInfo.PropertyType == typeof(decimal) ||
                    item.PropertyInfo.PropertyType == typeof(decimal?) ||
                    item.PropertyInfo.PropertyType == typeof(long) ||
                    item.PropertyInfo.PropertyType == typeof(long?) ||
                    item.PropertyInfo.PropertyType == typeof(int) ||
                    item.PropertyInfo.PropertyType == typeof(int?))
                {
                    predicate += $" AND ({alias}.\"{item.PropName}\" {oper} {item.Value})";
                }
                else if (item.PropertyInfo.PropertyType == typeof(DateTime))
                {
                    string dateString = item.Value;
                    if (dateString.Contains('T'))
                    {
                        dateString = DateTime.Parse(dateString, null, System.Globalization.DateTimeStyles.RoundtripKind).ToString();
                    }
                    predicate += $" AND ({alias}.\"{item.PropName}\" {oper} '{dateString}')";
                }
            }

            foreach (var item in cases.Where(x => x.PredicateCase.Operation == PredicateOperation.Condition))
            {
                if (!string.IsNullOrEmpty(item.PredicateCase.Condition))
                {
                    predicate += $" AND ({item.PredicateCase.Condition.Replace("#item", alias).Replace("#value", item.Value)})";
                }
            }

            var op_overlaps = cases.Where(x => x.PredicateCase.Operation == PredicateOperation.Overlaps);
            if (op_overlaps.Count() > 0)
            {
                var groups = op_overlaps.GroupBy(x => x.PredicateCase.Group).ToList();
                foreach (var g in groups)
                {
                    if (g.Count() != 2)
                        continue;
                    var list = g.ToList();
                    predicate += $" AND (({alias}.\"{list[0].PropName}\",{alias}.\"{list[1].PropName}\") overlaps ('{list[0].Value}','{list[1].Value}'))";
                }
            }

            return predicate != null ? " where (true)" + predicate : null;
        }
    }
}
