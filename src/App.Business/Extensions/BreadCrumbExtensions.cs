using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App.Business.Extensions
{
    public static class BreadCrumbExtensions
    {
        /// <summary>
        /// The key value of the current item in the cookie
        /// </summary>
        public const string CurrentBreadCrumbKey = "Current_Lims_BreadCrumb_Key";

        private static List<BreadCrumb> breadCrumbs;

        private static List<BreadCrumb> GetCurrentCookie(HttpContext ctx)
        {
            var cookieObj = ctx.Request.Cookies[CurrentBreadCrumbKey];
            var breadCrumbs = new List<BreadCrumb>();
            if (cookieObj != null)
                breadCrumbs = JsonConvert.DeserializeObject<List<BreadCrumb>>(cookieObj);

            return breadCrumbs;
        }

        private static void SetCurrentCookie(HttpContext ctx, List<BreadCrumb> breadCrumbs)
        {
            var json = JsonConvert.SerializeObject(breadCrumbs);
            ctx.Response.Cookies.Append(CurrentBreadCrumbKey, json, new CookieOptions { Secure = false, IsEssential = true });
        }

        public static void SaveCookies(this HttpContext ctx)
        {
            SetCurrentCookie(ctx, breadCrumbs);
        }

        /// <summary>
        /// Clears the stack of the current cookie
        /// </summary>
        /// <param name="ctx"></param>
        public static bool ClearBreadCrumbs(this HttpContext ctx)
        {
            if (ctx == null)
            {
                return false;
            }

            if (breadCrumbs == null)
            {
                breadCrumbs = GetCurrentCookie(ctx);
            }

            breadCrumbs.RemoveAll(p => p.Position != 0);

            return true;
        }

        /// <summary>
        /// Clears the stack of the current cookie
        /// </summary>
        /// <param name="ctx"></param>
        public static bool ClearBreadCrumbs(this Controller ctx)
        {
            return ctx.HttpContext.ClearBreadCrumbs();
        }

        /// <summary>
        /// Delete the address to the current position.
        /// </summary>
        /// <param name="currentPosition"></param>
        /// <returns></returns>
        public static bool ClearBeforeBreadCrumbs(this HttpContext ctx, int position)
        {
            if (breadCrumbs == null)
            {
                breadCrumbs = GetCurrentCookie(ctx);
            }

            breadCrumbs.RemoveAll(p => p.Position > position);
            return true;
        }

        /// <summary>
        /// Adds a custom bread crumb to the list
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="breadCrumb"></param>
        public static bool AddBreadCrumb(this HttpContext ctx, BreadCrumb breadCrumb)
        {
            if (ctx == null)
            {
                return false;
            }

            if (breadCrumbs == null)
            {
                breadCrumbs = GetCurrentCookie(ctx);
            }

            breadCrumbs?.RemoveAll(p => p.Position > breadCrumb.Position);

            if (breadCrumbs.Any(crumb => crumb.Link.Equals(breadCrumb.Link, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            if (breadCrumbs.FirstOrDefault(p => p.Position == breadCrumb.Position) != null)
            {
                breadCrumbs.RemoveAll(p => p.Position == breadCrumb.Position);
            }


            breadCrumbs.Add(breadCrumb);

            return true;
        }

        /// <summary>
        ///     Modifies the current bread crumb
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="breadCrumbAction"></param>
        public static bool ModifyCurrentBreadCrumb(this HttpContext ctx, Action<BreadCrumb> breadCrumbAction)
        {
            if (ctx == null)
            {
                return false;
            }

            if (breadCrumbs == null)
            {
                breadCrumbs = GetCurrentCookie(ctx);
            }

            var url = ctx.Request.GetEncodedUrl();
            var breadCrumb = breadCrumbs.FirstOrDefault(crumb =>
                crumb.Link.Equals(url, StringComparison.OrdinalIgnoreCase));
            if (breadCrumb == null)
            {
                return false;
            }

            breadCrumbAction(breadCrumb);

            return true;
        }

        /// <summary>
        ///     Modifies the current bread crumb
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="breadCrumbAction"></param>
        public static bool ModifyCurrentBreadCrumb(this Controller ctx, Action<BreadCrumb> breadCrumbAction)
        {
            return ctx.HttpContext.ModifyCurrentBreadCrumb(breadCrumbAction);
        }

        /// <summary>
        /// Returns all the breadcrumbs
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="breadCrumbFilter"></param>
        public static IEnumerable<BreadCrumb> GetBreadCrumbs(this HttpContext ctx, Predicate<BreadCrumb> breadCrumbFilter = null)
        {
            if (ctx == null)
            {
                return Enumerable.Empty<BreadCrumb>();
            }

            if (breadCrumbs == null)
            {
                breadCrumbs = GetCurrentCookie(ctx);
            }

            return breadCrumbs.Where(breadCrumb => breadCrumbFilter == null || breadCrumbFilter(breadCrumb));
        }

        /// <summary>
        /// Returns all the breadcrumbs
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="breadCrumbFilter"></param>
        public static IEnumerable<BreadCrumb> GetBreadCrumbs(this Controller ctx, Predicate<BreadCrumb> breadCrumbFilter = null)
        {
            return ctx.HttpContext.GetBreadCrumbs(breadCrumbFilter);
        }
    }
}
