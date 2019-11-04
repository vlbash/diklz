using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace App.Core.Mvc.Extensions
{
    public static class ControllerBaseExtensions
    {
        [NonAction]
        public static IActionResult RedirectBack(this ControllerBase controller, string defaultAction = "Index", object routeValues = null)
        {
            if (controller.Request?.Form?.TryGetValue("_ReturnUrl", out var strval) == true && !string.IsNullOrEmpty(strval.ToString()))
            {
                var returl = strval.ToString();
                if (!Uri.TryCreate(returl, UriKind.Absolute, out _))
                {
                    if (!returl.StartsWith("/"))
                        returl = '/' + returl;
                    returl = controller.HttpContext.Request.Scheme + "://" + controller.HttpContext.Request.Host + returl;
                }

                var ubuilder = new UriBuilder(returl);
                var query = HttpUtility.ParseQueryString(ubuilder.Query);
                var rvd = new RouteValueDictionary(routeValues);
                foreach (var item in rvd)
                {
                    query["_ret" + item.Key] = item.Value.ToString();
                }

                ubuilder.Query = query.ToString();
                returl = ubuilder.ToString();

                return controller.Redirect(returl);
            }
            else
                return controller.RedirectToAction(defaultAction, routeValues);
        }
    }
}
