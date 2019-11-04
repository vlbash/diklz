using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Business.Infrastructure.BackgroundService;
using Microsoft.Extensions.DependencyInjection;
using NCrontab;
using Serilog;

namespace App.Business.Infrastructure.Scheduler
{
    public abstract class ScheduledProcessor: ScopedProcessor
    {
        private CrontabSchedule _schedule;
        private DateTime _nextRun;
        /// <summary>
        /// https://en.wikipedia.org/wiki/Cron
        /// </summary>
        protected abstract string Schedule { get; }
        protected abstract string TaskName { get; }

        protected ScheduledProcessor(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
            _schedule = CrontabSchedule.Parse(Schedule);
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);

            Log.Information($"[Background-service] - {TaskName} | {_nextRun:dd-MM-yyyy HH:mm:ss}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var now = DateTime.Now;
                if (now > _nextRun)
                {
                    Log.Information("[Background-service] - " + TaskName + " started");
                    await Process();
                    _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                }
                await Task.Delay(5000, stoppingToken); //5 seconds delay
            }
            while (!stoppingToken.IsCancellationRequested);
        }
    }
}
