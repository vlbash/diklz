using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Services.AppServices;
using App.Business.Services.PrlServices;
using App.Core.Business.Services;
using App.Data.DTO.Common.Widget;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Host.Controllers
{
    [Authorize(Policy = "Registered")]
    public class WidgetController: Controller
    {
        private ICommonDataService _commonDataService { get; }

        public WidgetController(ICommonDataService commonDataService)
        {
            _commonDataService = commonDataService;
        }

        public async Task<IActionResult> GetPaymentInfo()
        {
            var listPay = (await _commonDataService.GetDtoAsync<WidgetPaymentDTO>())
                .Where(p => string.IsNullOrEmpty(p.EdocumentStatus) ||  new List<string>{ "RequiresPayment", "PaymentNotVerified", "WaitingForConfirmation" }.Contains(p.EdocumentStatus));
            return PartialView("WidgetPayment", listPay.ToList());
        }
    }
}
