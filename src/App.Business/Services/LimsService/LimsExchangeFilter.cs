using System;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Services.ImlServices;
using App.Business.Services.PrlServices;
using App.Business.Services.TrlServices;
using App.Data.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Business.Services.LimsService
{
    public class LimsExchangeFilter: Attribute, IAsyncActionFilter
    {
        private LimsRepository.ChangesTrackedEnum _changesTracked { get; }
        private PrlApplicationProcessService _prlApplicationProcessService { get; }
        private LimsExchangeService _limsExchangeService { get; }
        private ImlApplicationProcessService _imlApplicationProcessService { get; }
        private TrlApplicationProcessService _trlApplicationProcessService { get; }

        public LimsExchangeFilter(PrlApplicationProcessService prlApplicationProcessService,
            LimsRepository.ChangesTrackedEnum changesTracked, LimsExchangeService limsExchangeService, ImlApplicationProcessService imlApplicationProcessService
            , TrlApplicationProcessService trlApplicationProcessService)
        {
            _changesTracked = changesTracked;
            _limsExchangeService = limsExchangeService;
            _imlApplicationProcessService = imlApplicationProcessService;
            _prlApplicationProcessService = prlApplicationProcessService;
            _trlApplicationProcessService = trlApplicationProcessService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            switch (_changesTracked)
            {
                case LimsRepository.ChangesTrackedEnum.AppProtocol:
                {
                    await UpdateDecision();
                    break;
                }

                case LimsRepository.ChangesTrackedEnum.AppNotice:
                {
                    await _prlApplicationProcessService.UpdateLicenseMessage();
                    break;
                }

                case LimsRepository.ChangesTrackedEnum.AppCheck:
                {
                    await _prlApplicationProcessService.UpdatePreLicenseCheck();
                    break;
                }

                case LimsRepository.ChangesTrackedEnum.EndLicCheck:
                {
                    await _prlApplicationProcessService.UpdateEndLicCheck();
                    break;
                }
                case LimsRepository.ChangesTrackedEnum.Application:
                {
                    await _prlApplicationProcessService.UpdatePreLicenseCheck();
                    break;
                }

            }

            await next();
        }

        private async Task UpdateDecision()
        {
            var protocols = await _limsExchangeService.UpdateLimsProtocols();
            var closedProtocols = protocols.Where(p => p.StatusId == 2).ToList();
            foreach (var closedProtocol in closedProtocols)
            {
                switch (closedProtocol.Type)
                {
                    case "PRL":
                        await _prlApplicationProcessService.UpdateDecision(closedProtocol);
                        break;
                    case "IML":
                        await _imlApplicationProcessService.UpdateDecision(closedProtocol);
                        break;
                    case "TRL":
                        await _trlApplicationProcessService.UpdateDecision(closedProtocol);
                        break;
                    default: break;
                }
            }
        }
    }
}
