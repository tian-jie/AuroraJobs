using Aurora.Jobs.Core.Services;
using log4net;
using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Aurora.Jobs.Core
{
    public class SchedulerJobListener : IJobListener
    {
        private ILog _logger = LogManager.GetLogger("SchedulerJobListener");
        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.Debug($"JobExecutionVetoed: {context.JobDetail.Key.Name}");
            return Task.FromResult(true);
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.Debug($"JobToBeExecuted: {context.JobDetail.Key.Name}");
            return Task.FromResult(true);
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.Debug($"SchedulerJobListener.JobWasExecuted: {context.JobDetail.Key.Name}");
            var taskId = int.Parse(context.JobDetail.Key.Name);
            DateTime nextFireTimeUtc = TimeZoneInfo.ConvertTimeFromUtc(context.NextFireTimeUtc.Value.DateTime, TimeZoneInfo.Local);
            DateTime fireTimeUtc = TimeZoneInfo.ConvertTimeFromUtc(context.FireTimeUtc.UtcDateTime, TimeZoneInfo.Local);

            double duration = context.JobRunTime.TotalSeconds;
            string jobName = string.Empty;
            string executedResult = string.Empty;
            if (context.MergedJobDataMap != null)
            {
                _logger.Debug($"SchedulerJobListener.JobWasExecuted context.MergedJobDataMap: {context.MergedJobDataMap.Count}");

                jobName = context.MergedJobDataMap.GetString("JobName");
                executedResult = context.MergedJobDataMap.FirstOrDefault(a => a.Key == "executedResult").Value as string;
                //foreach (var item in context.MergedJobDataMap)
                //{
                //    string key = item.Key;
                //    if (key=="executedResult")
                //    {
                //        if (i > 0)
                //        {
                //            log.Append(",");
                //        }
                //        log.Append(item.Value);
                //        i++;
                //    }
                //}
                //if (i > 0)
                //{
                //    executedResult = string.Concat(log.ToString());
                //}

                _logger.Debug($"SchedulerJobListener.JobWasExecuted: {executedResult}");

            }
            if (jobException != null)
            {
                executedResult = executedResult + " EX:" + jobException.ToString();
            }

            executedResult = executedResult.Length > 1000 ? executedResult.Substring(0, 1000) : executedResult;
            new ScheduledTaskService().UpdateScheduledTaskStatus(taskId, jobName, fireTimeUtc, nextFireTimeUtc, duration, executedResult);

            return Task.FromResult(true);
        }

        public string Name
        {
            get { return "SchedulerJobListener"; }
        }
    }
}
