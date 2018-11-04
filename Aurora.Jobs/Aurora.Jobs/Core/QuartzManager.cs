using Aurora.Jobs.Core.Business.Info;
using Aurora.Jobs.Core.Services;
using log4net;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace Aurora.Jobs.Core
{
    public class QuartzManager
    {
        private readonly ILog _logger;


        public QuartzManager()
        {
            _logger = LogManager.GetLogger(GetType());
        }
        /// <summary>
        /// 从程序集中加载指定类
        /// </summary>
        /// <param name="assemblyName">含后缀的程序集名</param>
        /// <param name="className">含命名空间完整类名</param>
        /// <returns></returns>
        private Type GetClassInfo(string assemblyName, string className)
        {
            Type type = null;
            try
            {
                assemblyName = GetAbsolutePath(assemblyName);
                Assembly assembly = null;
                assembly = Assembly.LoadFrom(assemblyName);
                type = assembly.GetType(className, true, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return type;
        }

        /// <summary>
        /// 校验字符串是否为正确的Cron表达式
        /// </summary>
        /// <param name="cronExpression">带校验表达式</param>
        /// <returns></returns>
        public bool ValidExpression(string cronExpression)
        {
            return CronExpression.IsValidExpression(cronExpression);
        }

        /// <summary>
        ///  获取文件的绝对路径
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <returns></returns>
        public string GetAbsolutePath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                throw new ArgumentNullException("参数relativePath空异常！");
            }
            relativePath = relativePath.Replace("/", "\\");
            if (relativePath[0] == '\\')
            {
                relativePath = relativePath.Remove(0, 1);
            }
            if (HttpContext.Current != null)
            {
                return Path.Combine(HttpRuntime.AppDomainAppPath, relativePath);
            }
            else
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            }
        }


        /// <summary>
        /// Job调度
        /// </summary>
        /// <param name="scheduler"></param>
        /// <param name="jobInfo"></param>
        public void ScheduleJob(IScheduler scheduler, ScheduledTask jobInfo)
        {
            _logger.Debug($"entering  QuartzManager.ScheduleJob - {jobInfo.Name}, {jobInfo.NextRunTime}");
            if (ValidExpression(jobInfo.CronExpression))
            {
                Type type = GetClassInfo(jobInfo.AssemblyName, jobInfo.ClassName);
                if (type != null)
                {
                    IJobDetail job = new JobDetailImpl(jobInfo.ScheduledTaskId.ToString(), jobInfo.ScheduledTaskId.ToString() + "Group", type);
                    job.JobDataMap.Add("Parameters", jobInfo.JobArgs);
                    job.JobDataMap.Add("JobName", jobInfo.Name);

                    CronTriggerImpl trigger = new CronTriggerImpl();
                    trigger.CronExpressionString = jobInfo.CronExpression;
                    trigger.Name = jobInfo.ScheduledTaskId.ToString();
                    trigger.Description = jobInfo.Description;
                    trigger.StartTimeUtc = DateTime.UtcNow;
                    trigger.Group = jobInfo.ScheduledTaskId + "TriggerGroup";
                    scheduler.ScheduleJob(job, trigger);
                }
                else
                {
                    new ScheduledTaskService().WriteBackgroundJoLog(jobInfo.ScheduledTaskId, jobInfo.Name, DateTime.Now, jobInfo.AssemblyName + jobInfo.ClassName + "无效，无法启动该任务");
                }
            }
            else
            {
                new ScheduledTaskService().WriteBackgroundJoLog(jobInfo.ScheduledTaskId, jobInfo.Name, DateTime.Now, jobInfo.CronExpression + "不是正确的Cron表达式,无法启动该任务");
            }
        }

        
        /// <summary>
        /// Job状态管控
        /// </summary>
        /// <param name="scheduler"></param>
        public async Task JobScheduler(IScheduler scheduler)
        {
            _logger.Debug($"entering  QuartzManager.JobScheduler");

            var list = new ScheduledTaskService().GetAllowScheduleJobInfoList();
            _logger.Debug($"({list.Count}) jobs found");

            if (list == null || list.Count == 0)
            {
                return;
            }

            foreach (var jobInfo in list)
            {
                var jobKey = new JobKey(jobInfo.ScheduledTaskId.ToString(), jobInfo.ScheduledTaskId.ToString() + "Group");
                _logger.Debug($"prcessing job - {jobInfo.Name}, jobInfo.NextRunTime:{jobInfo.NextRunTime}, jobKey={jobKey}");

                var existsJobKey = await scheduler.CheckExists(jobKey);
                if (!(existsJobKey))
                {
                    _logger.Debug($"no jobKey, state={jobInfo.Status}");

                    if (jobInfo.Status == Business.enums.JobStatus.Idle || jobInfo.Status == Business.enums.JobStatus.Running || jobInfo.Status == Business.enums.JobStatus.Starting)
                    {
                        ScheduleJob(scheduler, jobInfo);
                        if (!existsJobKey)
                        {
                            new ScheduledTaskService().UpdateScheduledTaskState(jobInfo.ScheduledTaskId, Business.enums.JobStatus.Idle);
                        }
                        else
                        {
                            new ScheduledTaskService().UpdateScheduledTaskState(jobInfo.ScheduledTaskId, Business.enums.JobStatus.Running);
                        }
                    }
                    else if (jobInfo.Status == Business.enums.JobStatus.Stopping)
                    {
                        new ScheduledTaskService().UpdateScheduledTaskState(jobInfo.ScheduledTaskId, Business.enums.JobStatus.Idle);
                    }
                }
                else
                {
                    _logger.Debug($"has jobKey");
                    if (jobInfo.Status == Business.enums.JobStatus.Stopping)
                    {
                        await scheduler.DeleteJob(jobKey);
                        new ScheduledTaskService().UpdateScheduledTaskState(jobInfo.ScheduledTaskId, Business.enums.JobStatus.Idle);
                    }
                    else if (jobInfo.Status == Business.enums.JobStatus.Starting)
                    {
                        new ScheduledTaskService().UpdateScheduledTaskState(jobInfo.ScheduledTaskId, Business.enums.JobStatus.Running);
                    }
                }
            }
        }

    }
}
