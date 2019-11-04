using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using App.Business.Services.ImlServices;
using App.Business.Services.LimsService;
using App.Core.Business.Services;
using App.Core.Business.Services.ObjectMapper;
using App.Core.Data;
using App.Core.Data.Repositories;
using App.Data.Contexts;
using App.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Business.Infrastructure.Scheduler
{
    public class ChangesOfPendingSpodu: ScheduledProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        

        public ChangesOfPendingSpodu(IServiceScopeFactory serviceScopeFactory
                                     ) :
            base(serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            
        }

        protected override  string Schedule => "0 0 * * *"; // 00:00 every day
        protected override string TaskName => "GetPendingChangesSpodu";

        protected override Task ProcessInScope(IServiceProvider serviceProvider)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var limsDbContext = scope.ServiceProvider.GetService<LimsDbContext>();
                var coreDbContext = scope.ServiceProvider.GetService<CoreDbContext>();
                var migrationDbContext = scope.ServiceProvider.GetService<MigrationDbContext>();
                var mapper = scope.ServiceProvider.GetService<IObjectMapper>();
                var commonRep = new CommonRepository(coreDbContext, null, null);
                var dataService = new CommonDataService(commonRep, null);
                var limsRep = new LimsRepository(null, null, limsDbContext, null);
                var limsExchangeService = new LimsExchangeService(limsRep, dataService, null, mapper, migrationDbContext, null ,null,null);

                Task.WaitAll(limsExchangeService.ImportLimsDepartmentalSubordination());
            }
            return Task.CompletedTask;
        }
    }

}
