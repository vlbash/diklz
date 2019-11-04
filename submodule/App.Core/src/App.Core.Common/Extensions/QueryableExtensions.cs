using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace App.Core.Common.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string sortExpression) where T : class
        {
            var index = 0;
            var sortingNames = sortExpression.Split(',');
            IQueryable<T> result = null;
            foreach (var item in sortingNames)
            {
                var sortingFunction = index++ > 0 ? "ThenBy" : "OrderBy";
                if (item.StartsWith("-"))
                {
                    sortingFunction += "Descending";
                    sortExpression = item.Substring(1);
                }
                else
                {
                    sortExpression = item;
                }

                var intermidateSource = result ?? source;
                var methodCall = GenerateMethodCall<T>(intermidateSource, sortingFunction, sortExpression.TrimStart());
                result = intermidateSource.Provider.CreateQuery<T>(methodCall);
            }
            return result ?? source;
        }

        private static MethodCallExpression GenerateMethodCall<T>(IQueryable<T> source, string methodName, string fieldName) where T : class
        {
            var type = typeof(T);
            var selector = GenerateSelector<T>(fieldName, out var selectorResultType);
            var resultExp = Expression.Call(typeof(Queryable), methodName,
                                            new Type[] { type, selectorResultType },
                                            source.Expression, Expression.Quote(selector));
            return resultExp;
        }

        private static LambdaExpression GenerateSelector<T>(string propertyName, out Type resultType) where T : class
        {
            // Create a parameter to pass into the Lambda expression (x => x.OrderByField).
            var parameter = Expression.Parameter(typeof(T), "x");
            //  create the selector part, but support child properties
            PropertyInfo property;
            Expression propertyAccess;
            if (propertyName.Contains('.'))
            {
                // support to be sorted on child fields.
                var childProperties = propertyName.Split('.');
                property = typeof(T).GetProperty(childProperties[0]);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
                for (var i = 1; i < childProperties.Length; i++)
                {
                    property = property.PropertyType.GetProperty(childProperties[i]);
                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
            else
            {
                property = typeof(T).GetProperty(propertyName);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
            }
            resultType = property.PropertyType;
            // Create the order by expression.
            return Expression.Lambda(propertyAccess, parameter);
        }
    }
}
