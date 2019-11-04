using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using App.Business.Services.ImlServices;
using App.Business.Services.LimsService;
using App.Core.Business.Services;
using App.Core.Data;
using App.Core.Data.Repositories;
using App.Data.Contexts;
using App.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Business.Infrastructure.Scheduler
{
    public class ChangesOfPendingRP: ScheduledProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public ChangesOfPendingRP(IServiceScopeFactory serviceScopeFactory):
            base(serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override  string Schedule => "0 0 * * *"; // 00:00 every day
        protected override string TaskName => "GetPendingChangesRp";

        protected override Task ProcessInScope(IServiceProvider serviceProvider)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var limsDbContext = scope.ServiceProvider.GetService<LimsDbContext>();
                var coreDbContext = scope.ServiceProvider.GetService<CoreDbContext>();
                var commonRep = new CommonRepository(coreDbContext, null, null);
                var dataService = new CommonDataService(commonRep, null);
                var limsRep = new LimsRepository(null, null, limsDbContext, null);
                var limsExchangeService = new LimsExchangeService(limsRep, dataService, null,null,null,null ,null,null);

                Task.WaitAll(limsExchangeService.UpdateLimsRp());
            }
            return Task.CompletedTask;
        }
    }

}
