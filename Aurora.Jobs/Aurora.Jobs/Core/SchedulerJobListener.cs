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
            try
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
                    _logger.Debug($"JobWasExecuted, jobName=\"{jobName}\"");
                    executedResult = context.MergedJobDataMap.FirstOrDefault(a => a.Key == "executedResult").Value as string;

                    _logger.Debug($"SchedulerJobListener.JobWasExecuted: {executedResult ?? "null"}");

                }
                if (jobException != null)
                {
                    _logger.Debug($"SchedulerJobListener.JobWasExecuted.jobException: {jobException.ToString()}");
                    executedResult = executedResult + " EX:" + jobException.ToString();
                }

                if (!string.IsNullOrEmpty(executedResult))
                {
                    executedResult = executedResult.Length > 1000 ? executedResult.Substring(0, 1000) : executedResult;
                }
                else
                {
                    executedResult = "job执行失败。。。";
                }
                new ScheduledTaskService().UpdateScheduledTaskStatus(taskId, jobName, fireTimeUtc, nextFireTimeUtc, duration, executedResult);
                _logger.Debug($"SchedulerJobListener.JobWasExecuted: all done");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return Task.FromResult(true);
        }

        public string Name
        {
            get { return "SchedulerJobListener"; }
        }
    }
}
