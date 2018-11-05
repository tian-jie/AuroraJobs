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
            assemblyName = GetAbsolutePath(assemblyName);
            Assembly assembly = null;
            assembly = Assembly.LoadFrom(assemblyName);
            var type = assembly.GetType(className, true, true);

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
        /// <param name="task"></param>
        public void ScheduleJob(IScheduler scheduler, ScheduledTask task)
        {
            _logger.Debug($"entering  QuartzManager.ScheduleJob - {task.Name}, {task.NextRunTime}");
            if (ValidExpression(task.CronExpression))
            {
                Type type = GetClassInfo(task.AssemblyName, task.ClassName);
                if (type != null)
                {
                    IJobDetail job = new JobDetailImpl(task.Id.ToString(), task.Id.ToString() + "Group", type);
                    job.JobDataMap.Add("Parameters", task.JobArgs);
                    job.JobDataMap.Add("JobName", task.Name);

                    CronTriggerImpl trigger = new CronTriggerImpl();
                    trigger.CronExpressionString = task.CronExpression;
                    trigger.Name = task.Id.ToString();
                    trigger.Description = task.Description;
                    trigger.StartTimeUtc = DateTime.UtcNow;
                    trigger.Group = task.Id + "TriggerGroup";
                    scheduler.ScheduleJob(job, trigger);
                }
                else
                {
                    new ScheduledTaskService().AddScheduledTaskHistory(task.Id, task.Name, DateTime.Now, task.AssemblyName + task.ClassName + "无效，无法启动该任务");
                }
            }
            else
            {
                new ScheduledTaskService().AddScheduledTaskHistory(task.Id, task.Name, DateTime.Now, task.CronExpression + "不是正确的Cron表达式,无法启动该任务");
            }

        }


        /// <summary>
        /// Job状态管控
        /// </summary>
        /// <param name="scheduler"></param>
        public async Task JobScheduler(IScheduler scheduler)
        {
            _logger.Debug($"entering  QuartzManager.JobScheduler");

            var jobList = new ScheduledTaskService().GetAllowScheduleJobInfoList();
            _logger.Debug($"({jobList.Count}) jobs found");

            if (jobList == null || jobList.Count == 0)
            {
                return;
            }

            foreach (var job in jobList)
            {
                try
                {
                    var jobKey = new JobKey(job.Id.ToString(), job.Id.ToString() + "Group");
                    _logger.Debug($"prcessing job - {job.Name}, jobInfo.NextRunTime:{job.NextRunTime}, jobKey={jobKey}");

                    var existsJobKey = await scheduler.CheckExists(jobKey);
                    if (!(existsJobKey))
                    {
                        _logger.Debug($"no jobKey, state={job.Status}");

                        if (job.Status == Business.enums.JobStatus.Idle || job.Status == Business.enums.JobStatus.Running || job.Status == Business.enums.JobStatus.Starting)
                        {
                            ScheduleJob(scheduler, job);
                            if (!existsJobKey)
                            {
                                _logger.Debug("if (!existsJobKey), set to JobStatus.Idle");
                                new ScheduledTaskService().UpdateScheduledTaskState(job.Id, Business.enums.JobStatus.Idle);
                            }
                            else
                            {
                                _logger.Debug("if (existsJobKey), set to JobStatus.Running");
                                new ScheduledTaskService().UpdateScheduledTaskState(job.Id, Business.enums.JobStatus.Running);
                            }
                        }
                        else if (job.Status == Business.enums.JobStatus.Stopping)
                        {
                            _logger.Debug("if (job.Status == JobStatus.Stopping), set to JobStatus.Idle");
                            new ScheduledTaskService().UpdateScheduledTaskState(job.Id, Business.enums.JobStatus.Idle);
                        }
                    }
                    else
                    {
                        _logger.Debug($"has jobKey");
                        if (job.Status == Business.enums.JobStatus.Stopping)
                        {
                            await scheduler.DeleteJob(jobKey);
                            _logger.Debug("has jobKey, if (job.Status == JobStatus.Stopping), set to JobStatus.Idle");
                            new ScheduledTaskService().UpdateScheduledTaskState(job.Id, Business.enums.JobStatus.Idle);
                        }
                        else if (job.Status == Business.enums.JobStatus.Starting)
                        {
                            _logger.Debug("has jobKey, if (job.Status == JobStatus.Starting), set to JobStatus.Running");
                            new ScheduledTaskService().UpdateScheduledTaskState(job.Id, Business.enums.JobStatus.Running);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("任务加载失败:" + job.Name, ex);
                    new ScheduledTaskService().UpdateScheduledTaskState(job.Id, Business.enums.JobStatus.Error);
                }
            }
        }

    }
}
