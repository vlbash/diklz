using System;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace App.Host.Extensions
{
    public static class ControllerBaseExtensions
    {
        [NonAction]
        public static IActionResult RedirectBack(this ControllerBase controller, string defaultAction = "Index", object routeValues = null)
        {
            if (controller.Request?.Form?.TryGetValue("_ReturnUrl", out var strval) == true && !string.IsNullOrEmpty(strval.ToString()))
            {
                var returl = strval.ToString();

                if (!Uri.TryCreate(returl, UriKind.Absolute, out var uri))
                {
                    if (!returl.StartsWith('/'))
                        returl = '/' + returl;
                    returl = controller.HttpContext.Request.Scheme + "://" + controller.HttpContext.Request.Host + returl;
                }

                var ubuilder = new UriBuilder(returl);
                var query = HttpUtility.ParseQueryString(ubuilder.Query);
                var rvd = new RouteValueDictionary(routeValues);
                foreach (var item in rvd)
                {
                    query["_ret"+item.Key] = item.Value.ToString();
                }

                ubuilder.Query = query.ToString();
                returl = ubuilder.ToString();

                return controller.Redirect(returl);
            }
            //else if (controller.Request.Headers["x-requested-with"] == "XMLHttpRequest")
            //    return controller.Redirect(controller.Request.Headers["Referer"].ToString());
            else
                return controller.RedirectToAction(defaultAction, routeValues);
        }
    }
}
