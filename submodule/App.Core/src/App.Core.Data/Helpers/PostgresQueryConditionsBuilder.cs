using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using App.Core.Common.Extensions;
using App.Core.Data.Attributes;

namespace App.Core.Data.Helpers
{
    public class PostgresQueryConditionsBuilder: IQueryConditionsHelper
    {
        public string GetQueryConditionsString(Type type, IDictionary<string, string> parameters, string queryAlias)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return string.Empty;
            }

            foreach (var key in parameters.Keys.ToList())
            {
                parameters[key] = parameters[key].Replace("'", "''");
            }

            if (!_typeProperties.TryGetValue(type, out var properties))
            {
                properties = type.GetProperties()
                   .Where(p => p.IsDefined(typeof(PredicateCase), true))
                   .ToDictionary(p => p,
                                 p => (PredicateCase)p.GetCustomAttributes(typeof(PredicateCase), true).First());
                _typeProperties.Add(type, properties);
            }

            var cases = FillSearchCaseList(properties, parameters);
            var conditions = GetStringConditions(cases, queryAlias);

            //return string.IsNullOrEmpty(conditions) ? null : " where (true)" + conditions;
            return conditions;
        }

        private readonly Dictionary<Type, Dictionary<PropertyInfo, PredicateCase>> _typeProperties
                    = new Dictionary<Type, Dictionary<PropertyInfo, PredicateCase>>();

        private class SearchCase
        {
            public string PropName { get; set; }
            public string InputName { get; set; }
            public string Value { get; set; }
            public PropertyInfo PropertyInfo { get; set; }
            public PredicateCase PredicateCase { get; set; }
        }

        private List<SearchCase> FillSearchCaseList(Dictionary<PropertyInfo, PredicateCase> properties,
                IDictionary<string, string> parameters)
        {
            var cases = new List<SearchCase>();
            foreach (var item in parameters) {

                if (string.IsNullOrEmpty(item.Key) || string.IsNullOrEmpty(item.Value) || item.Key == "X-Requested-With") {
                    continue;
                }

                var sc = new SearchCase
                {
                    PropName = item.Key.ToLower(),
                    InputName = item.Key.ToLower(),
                    Value = item.Value
                };

                if (sc.PropName.EndsWith("_from")) {
                    //sc.PropName = sc.PropName.Replace("_from", "");
                    sc.PropName = sc.PropName.Remove(sc.PropName.LastIndexOf("_from"));
                } else if (sc.PropName.EndsWith("_to")) {
                    //sc.PropName = sc.PropName.Replace("_to", "");
                    sc.PropName = sc.PropName.Remove(sc.PropName.LastIndexOf("_to"));
                }

                sc.PropertyInfo = properties.Keys.FirstOrDefault(x => x.Name.ToLower() == sc.PropName.ToLower());
                if (sc.PropertyInfo != null && properties.TryGetValue(sc.PropertyInfo, out var pcase)) {
                    sc.PredicateCase = pcase;
                    sc.PropName = sc.PropertyInfo.Name;

                    if (sc.PropertyInfo.PropertyType == typeof(bool)) {
                        sc.Value = (sc.Value == "on").ToString();
                    }

                    cases.Add(sc);
                }
            }

            return cases;
        }

        private string GetStringConditions(List<SearchCase> cases, string queryAlias)
        {
            var withEqualsConditions = CreateEqualsStringConditions(cases, queryAlias);
            var withContainsConditions = CreateContainsStringConditions(cases, queryAlias, withEqualsConditions);
            var withValueRangeConditions = CreateValueRangeStringConditions(cases, queryAlias, withContainsConditions);
            var withInputRangeConditions = CreateInputRangeStringConditions(cases, queryAlias, withValueRangeConditions);
            var withConditionConditions = CreateConditionStringConditions(cases, queryAlias, withInputRangeConditions);
            var conditions = CreateOverlapsStringConditions(cases, queryAlias, withConditionConditions);

            return conditions;
        }

        private string CreateEqualsStringConditions(List<SearchCase> cases, string queryAlias, string addToConditions = "")
        {
            var newConditions = addToConditions;
            foreach (var item in cases.Where(x => x.PredicateCase.Operation == PredicateOperation.Equals)) {
                var condition = "";
                if (item.PropertyInfo.PropertyType == typeof(string) ||
                    item.PropertyInfo.PropertyType == typeof(Guid?) ||
                    item.PropertyInfo.PropertyType == typeof(Guid)) {
                    condition = $"({queryAlias}.{item.PropName.ToSnakeCase()} = '{item.Value}')";
                }
                else {
                    condition = $"({queryAlias}.{item.PropName.ToSnakeCase()} = {item.Value})";
                }
                newConditions = ConcatStringsWithAnd(newConditions, condition);
            }

            return newConditions;
        }

        private string CreateContainsStringConditions(List<SearchCase> cases, string queryAlias, string addToConditions = "")
        {
            var newConditions = addToConditions;
            foreach (var item in cases.Where(x => x.PredicateCase.Operation == PredicateOperation.Contains)) {
                var condition = $"(POSITION('{item.Value.ToLower()}' in LOWER({queryAlias}.{item.PropName.ToSnakeCase()})) > 0)";
                newConditions = ConcatStringsWithAnd(newConditions, condition);
            }

            return newConditions;
        }

        private string CreateValueRangeStringConditions(List<SearchCase> cases, string queryAlias, string addToConditions = "")
        {
            var newConditions = addToConditions;
            foreach (var item in cases.Where(x => x.PredicateCase.Operation == PredicateOperation.ValueRange)) {
                string[] dates;

                if (item.Value.Contains('&')) {
                    dates = item.Value.Split('&');
                    dates[0] = DateTime.Parse(dates[0], null, System.Globalization.DateTimeStyles.RoundtripKind).ToString();
                    dates[1] = DateTime.Parse(dates[1], null, System.Globalization.DateTimeStyles.RoundtripKind).ToString();
                }
                else {
                    dates = item.Value.Split('-');
                }

                var condition = "";
                if (item.PropertyInfo.PropertyType == typeof(string) ||
                    item.PropertyInfo.PropertyType == typeof(Guid?) ||
                    item.PropertyInfo.PropertyType == typeof(Guid) ||
                    item.PropertyInfo.PropertyType == typeof(DateTime) ||
                    item.PropertyInfo.PropertyType == typeof(DateTime?)) {
                    condition = $"({queryAlias}.{item.PropName.ToSnakeCase()} >= '{dates[0]}')";
                    condition += $" and ({queryAlias}.{item.PropName.ToSnakeCase()} <= '{dates[1]}')";
                }
                else {
                    condition = $" ({queryAlias}.{item.PropName.ToSnakeCase()} >= {dates[0]})";
                    condition += $" and ({queryAlias}.{item.PropName.ToSnakeCase()} <= {dates[1]})";
                };
                newConditions = ConcatStringsWithAnd(newConditions, condition);
            }

            return newConditions;
        }

        private string CreateInputRangeStringConditions(List<SearchCase> cases, string queryAlias, string addToConditions = "")
        {
            var newConditions = addToConditions;
            foreach (var item in cases.Where(x => x.PredicateCase.Operation == PredicateOperation.InputRange)) {
                string oper;

                if (item.InputName.EndsWith("_from")) {
                    oper = ">=";
                } else if (item.InputName.EndsWith("_to")) {
                    oper = "<=";
                } else {
                    oper = "=";
                }

                if (oper != "=" && item.Value == null)
                {
                    continue;
                }

                var condition = "";
                if (item.PropertyInfo.PropertyType == typeof(string) ||
                    item.PropertyInfo.PropertyType == typeof(Guid?) || item.PropertyInfo.PropertyType == typeof(Guid))
                {
                    condition = $"({queryAlias}.{item.PropName.ToSnakeCase()} {oper} '{item.Value}')";
                }
                else if (item.PropertyInfo.PropertyType == typeof(DateTime) ||
                         item.PropertyInfo.PropertyType == typeof(DateTime?))
                {
                    var dateString = item.Value;
                    if (dateString.Contains('T'))
                    {
                        var date = DateTime.Parse(dateString, null, DateTimeStyles.AssumeUniversal);
                        if (oper == "<=")
                        {
                            date = date.AddDays(1);
                        }

                        dateString = date.ToString("dd.MM.yyyy");
                    }

                    condition = $"({queryAlias}.{item.PropName.ToSnakeCase()} {oper} to_date('{dateString}','dd.mm.yyyy'))";
                }
                else
                {
                    condition = $"({queryAlias}.{item.PropName.ToSnakeCase()} {oper} {item.Value})";
                }

                newConditions = ConcatStringsWithAnd(newConditions, condition);
            }

            return newConditions;
        }

        private string CreateConditionStringConditions(List<SearchCase> cases, string queryAlias, string addToConditions = "")
        {
            var newConditions = addToConditions;
            foreach (var item in cases.Where(x => x.PredicateCase.Operation == PredicateOperation.Condition)) {
                if (!string.IsNullOrEmpty(item.PredicateCase.Condition)) {
                    var condition = $"({item.PredicateCase.Condition.Replace("#item", queryAlias).Replace("#value", item.Value)})";
                    newConditions = ConcatStringsWithAnd(newConditions, condition);
                }
            }

            return newConditions;
        }

        private string CreateOverlapsStringConditions(List<SearchCase> cases, string queryAlias, string addToConditions = "")
        {
            var newConditions = addToConditions;
            var op_overlaps = cases.Where(x => x.PredicateCase.Operation == PredicateOperation.Overlaps).ToArray();
            if (op_overlaps.Length > 0) {
                var groups = op_overlaps.GroupBy(x => x.PredicateCase.Group)
                    .Where(g => g.Count() == 2);
                foreach (var g in groups) {
                    var group = g.ToList();
                    var condition = $"(({queryAlias}.{group[0].PropName.ToSnakeCase()},{queryAlias}.{group[1].PropName.ToSnakeCase()}) overlaps ('{group[0].Value}','{group[1].Value}'))";
                    newConditions = ConcatStringsWithAnd(newConditions, condition);
                }
            }

            return newConditions;
        }

        private string ConcatStringsWithAnd(string firstString, string secondString)
        {
            var result = firstString;
            if (string.IsNullOrEmpty(firstString))
            {
                result = secondString;
            } else {
                result += " and " + secondString;
            }

            return result;
        }
    }
}
