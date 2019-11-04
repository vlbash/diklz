using System;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.CryptoSignMiddleware;
using App.Business.Services.ImlServices;
using App.Business.Services.PrlServices;
using App.Business.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace App.HostBack.Controllers
{
    [Authorize]
    public class EuSignController: Controller
    {
        private readonly IPrlApplicationService _prlAppService;
        private readonly ImlApplicationService _imlApplicationService;

        public EuSignController(IPrlApplicationService prlAppService, ImlApplicationService imlApplicationService)
        {
            _prlAppService = prlAppService;
            _imlApplicationService = imlApplicationService;
        }

        [BreadCrumb(Title = "Ідентифікація за електронним підписом", Order = 3)]
        [HttpGet]
        public async Task<IActionResult> Sign(string urlAction)
        {
            if (string.IsNullOrEmpty(urlAction))
                return await Task.Run(() => NotFound());
            return View(model: urlAction);
        }

        [MiddlewareFilter(typeof(ProxyHandlerMiddlewarePipeline))]
        public async Task<IActionResult> ProxyMiddleware()
        {
            return await Task.Run(() => NotFound());
        }

        [BreadCrumb(Title = "Перевірка файлів підписаних ЕЦП", Order = 3)]
        [HttpGet]
        public async Task<IActionResult> VerificationSignedFiles(Guid entityId, string entityName)
        {
            return View((entityId, entityName));
        }

        public async Task<IActionResult> GetFiles(Guid entityId, string entityName)
        {
            var success = true;
            var modelObj = new FilesSignViewModel();
            try
            {
                switch (entityName)
                {
                    case "PrlApplication":
                        modelObj = await _prlAppService.GetFilesForVerification(entityId);
                        break;
                    case "ImlApplication":
                        modelObj = await _imlApplicationService.GetFilesForVerification(entityId);
                        break;
                    default:
                        Log.Error("Entity name isn't received");
                        success = false;
                        break;
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                success = false;
            }

            return Json(new { success = success, model = modelObj });
        }
    }
}
