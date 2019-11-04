using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.CryptoSignMiddleware;
using Microsoft.AspNetCore.Mvc;

namespace App.Host.Controllers
{
    public class EuSignController : Controller
    {
        [BreadCrumb(Title = "Ідентифікація за електронним підписом", Order = 3)]
        [HttpGet]
        public async Task<IActionResult> Sign(string urlAction, string urlActionSave)
        {
            if (string.IsNullOrEmpty(urlAction) || string.IsNullOrEmpty(urlActionSave))
                return await Task.Run(() => NotFound());
            return View(model:(urlAction, urlActionSave));
        }

        [MiddlewareFilter(typeof(ProxyHandlerMiddlewarePipeline))]
        public async Task<IActionResult> ProxyMiddleware()
        {
            return await Task.Run(() => NotFound());
        }
    }
}
