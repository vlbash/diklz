using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using App.Business.Extensions;
using App.Data.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace App.Business.Attributes
{
    /// <summary>
    /// BreadCrumb Action Filter. It can be applied to action methods or controllers.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class BreadCrumbAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Use this property to remove all of the previous items of the current stack
        /// </summary>
        public bool ClearStack { get; set; }

        /// <summary>
        /// Title of the current item
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A constant URL of the current item. If UseDefaultRouteUrl is set to true, its value will be ignored
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// If UseDefaultRouteUrl is set to true, this property indicated all of the route items should be removed from the final URL
        /// </summary>
        public bool RemoveAllDefaultRouteValues { get; set; }

        /// <summary>
        /// If UseDefaultRouteUrl is set to true, this property indicated which route items should be removed from the final URL
        /// </summary>
        public string[] RemoveRouteValues { get; set; }

        /// <summary>
        /// This property is useful for controller level bread crumbs. If it's true, the Url value will be calculated automatically from the DefaultRoute
        /// </summary>
        public bool UseDefaultRouteUrl { get; set; }

        /// <summary>
        /// This property is useful when you need a back functionality. If it's true, the Url value will be previous Url using UrlReferrer
        /// </summary>
        public bool UsePreviousUrl { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (ClearStack)
            {
                context.HttpContext.ClearBreadCrumbs();
            }

            var url = string.IsNullOrWhiteSpace(Url) ? context.HttpContext.Request.GetEncodedUrl() : Url;

            if (UseDefaultRouteUrl)
            {
                url = GetDefaultControllerActionUrl(context);
            }

            if (UsePreviousUrl)
            {
                url = context.HttpContext.Request.Headers["Referrer"];
            }

            SetEmptyTitleFromAttributes(context);

            context.HttpContext.AddBreadCrumb(new BreadCrumb
            {
                Link = url,
                Name = Title,
                Position = Order,
            });

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.SaveCookies();

            base.OnActionExecuted(context);
        }

        private string GetDefaultControllerActionUrl(ActionExecutingContext filterContext)
        {
            var defaultAction = GetDefaultAction(filterContext);
            var urlHelper = GetUrlHelper(filterContext);

            if (RemoveAllDefaultRouteValues)
            {
                return urlHelper.ActionWithoutRouteValues(defaultAction);
            }

            if (RemoveRouteValues == null || !RemoveRouteValues.Any())
            {
                return urlHelper.Action(defaultAction);
            }

            return urlHelper.ActionWithoutRouteValues(defaultAction, RemoveRouteValues);
        }

        private static string GetDefaultAction(ActionExecutingContext filterContext)
        {
            object defaultActionData;
            var defaultRoute = filterContext.RouteData.Routers.OfType<Route>().FirstOrDefault();
            if (defaultRoute != null)
            {
                if (defaultRoute.Defaults.TryGetValue("action", out defaultActionData))
                {
                    return defaultActionData as string;
                }
                throw new InvalidOperationException("The default action of this controller not found.");
            }

            if (filterContext.RouteData.Values.TryGetValue("action", out defaultActionData))
            {
                return defaultActionData as string;
            }
            throw new InvalidOperationException("The default action of this controller not found.");
        }

        private static IUrlHelper GetUrlHelper(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as Controller;
            if (controller == null)
            {
                throw new NullReferenceException("Failed to find the current Controller.");
            }

            var urlHelper = controller.Url;
            if (urlHelper == null)
            {
                throw new NullReferenceException("Failed to find the IUrlHelper of the filterContext.Controller.");
            }

            return urlHelper;
        }

        private void SetEmptyTitleFromAttributes(ActionExecutingContext filterContext)
        {
            if (!string.IsNullOrWhiteSpace(Title))
            {
                return;
            }

            var descriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor == null)
            {
                return;
            }

            var currentFilter = filterContext.ActionDescriptor
                .FilterDescriptors
                .Select(filterDescriptor => filterDescriptor)
                .FirstOrDefault(filterDescriptor => ReferenceEquals(filterDescriptor.Filter, this));
            if (currentFilter == null)
            {
                return;
            }

            MemberInfo typeInfo = null;

            if (currentFilter.Scope == FilterScope.Action)
            {
                typeInfo = descriptor.MethodInfo;
            }
            if (currentFilter.Scope == FilterScope.Controller)
            {
                typeInfo = descriptor.ControllerTypeInfo;
            }

            if (typeInfo == null)
            {
                return;
            }

            Title = typeInfo.GetCustomAttribute<DisplayNameAttribute>(inherit: true)?.DisplayName;
            if (string.IsNullOrWhiteSpace(Title))
            {
                Title = typeInfo.GetCustomAttribute<DescriptionAttribute>(inherit: true)?.Description;
            }
        }
    }
}
