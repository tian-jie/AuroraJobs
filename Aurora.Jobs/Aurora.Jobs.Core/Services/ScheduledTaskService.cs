using Aurora.Jobs.Core.Business.enums;
using Aurora.Jobs.Core.Business.Info;
using Aurora.Jobs.Core.Business.Manager;
using System;
using System.Collections.Generic;

namespace Aurora.Jobs.Core.Services
{
    public class ScheduledTaskService
    {
        public ScheduledTaskService() { }

        /// <summary>
        /// Job新增
        /// </summary>
        /// <param name="task">ScheduledTaskInfo实体</param>
        /// <returns></returns>
        public bool InsertScheduledTask(ScheduledTask task)
        {
            return new ScheduledTaskManager().InsertScheduledTask(task);
        }

        /// <summary>
        /// Job修改
        /// </summary>
        /// <param name="task">ScheduledTaskInfo实体</param>
        /// <returns></returns>
        public bool UpdateScheduledTask(ScheduledTask task)
        {
            return new ScheduledTaskManager().UpdateScheduledTask(task);
        }

        /// <summary>
        /// Job删除
        /// </summary>
        /// <param name="taskId">Job ID</param>
        /// <returns></returns>
        public bool DeleteScheduledTask(int taskId)
        {
            return new ScheduledTaskManager().DeleteScheduledTask(taskId);
        }

        /// <summary>
        /// Job删除
        /// </summary>
        /// <param name="idList">ID集合</param>
        /// <returns></returns>
        public bool DeleteScheduledTask(List<int> idList, out string rtMsg)
        {
            if (idList == null)
            {
                throw new ArgumentNullException(nameof(idList));
            }

            bool result = false;
            rtMsg = string.Empty;
            int i = 0;
            if (idList != null && idList.Count > 0)
            {
                foreach (var scheduledTaskId in idList)
                {
                    ScheduledTask ScheduledTaskInfo = GetScheduledTaskInfo(scheduledTaskId);
                    if (ScheduledTaskInfo.Status != JobStatus.Idle)
                    {
                        rtMsg = string.Format("{0}状态不为 停止状态,无法进行删除！", ScheduledTaskInfo.Name);
                        return false;
                    }
                }

                foreach (var scheduledTaskId in idList)
                {
                    DeleteScheduledTask(scheduledTaskId);
                    i++;
                }
            }
            result = i > 0;
            return result;
        }

        /// <summary>
        /// Job详情
        /// </summary>
        /// <param name="taskId">Job ID</param>
        /// <returns></returns>
        public ScheduledTask GetScheduledTaskInfo(int taskId)
        {
            return new ScheduledTaskManager().GetScheduledTaskInfo(taskId);
        }

        /// <summary>
        /// Job集合(分页 )
        /// </summary>
        /// <param name="parameter">参数集</param>
        /// <returns></returns>
        public PagerModel<ScheduledTask> GetScheduledTaskInfoPagerList(PageParameter parameter)
        {
            return new ScheduledTaskManager().GeScheduledTaskInfoPagerList(parameter);
        }

        /// <summary>
        /// 获取允许调度的Job集合
        /// </summary>
        /// <returns></returns>
        public List<ScheduledTask> GetAllowScheduleJobInfoList()
        {
            return new ScheduledTaskManager().GeAllowScheduleJobInfoList();
        }

        /// <summary>
        /// 更新Job状态
        /// </summary>
        /// <param name="taskId">Job ID</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public bool UpdateScheduledTaskState(int taskId, JobStatus status)
        {
            return new ScheduledTaskManager().UpdateScheduledTaskState(taskId, status);
        }

        /// <summary>
        /// 更新Job运行信息 
        /// </summary>
        /// <param name="taskId">Job ID</param>
        /// <param name="lastRunTime">最后运行时间</param>
        /// <param name="nextRunTime">下次运行时间</param>
        public void UpdateScheduledTaskStatus(int taskId, DateTime lastRunTime, DateTime nextRunTime)
        {
            new ScheduledTaskManager().UpdateScheduledTaskStatus(taskId, lastRunTime, nextRunTime);
        }

        /// <summary>
        /// 更新Job运行信息 
        /// </summary>
        /// <param name="taskId">Job ID</param>
        /// <param name="jobName">Job名称</param>
        /// <param name="lastRunTime">最后运行时间</param>
        /// <param name="nextRunTime">下次运行时间</param>
        /// <param name="duration">运行时长</param>
        /// <param name="executedResult">日志</param>
        public void UpdateScheduledTaskStatus(int taskId, string jobName, DateTime lastRunTime, DateTime nextRunTime, double duration, string executedResult)
        {
            UpdateScheduledTaskStatus(taskId, lastRunTime, nextRunTime);
            AddScheduledTaskHistory(taskId, jobName, lastRunTime, duration, executedResult);
        }


        /// <summary>
        /// Job日志详情
        /// </summary>
        /// <param name="taskHistoryId">日志ID</param>
        /// <returns></returns>
        public ScheduledTaskHistory GetScheduledTaskHistoryInfo(int taskHistoryId)
        {
            return new ScheduledTaskManager().GetScheduledTaskHistoryInfo(taskHistoryId);
        }

        /// <summary>
        /// Job日志集合（分页）
        /// </summary>
        /// <param name="parameter">参数集</param>
        /// <returns></returns>
        public PagerModel<ScheduledTaskHistory> GetScheduledTaskHistoryInfoPagerList(PageParameter parameter)
        {
            return new ScheduledTaskManager().GetScheduledTaskHistoryInfoPagerList(parameter);
        }

        /// <summary>
        /// Job日志删除
        /// </summary>
        /// <param name="ScheduledTaskHistoryId">日志ID</param>
        /// <returns></returns>
        public bool DeleteScheduledTaskHistory(int taskHistoryId)
        {
            return new ScheduledTaskManager().DeleteScheduledTaskHistory(taskHistoryId);
        }

        /// <summary>
        /// Job 日志删除
        /// </summary>
        /// <param name="ScheduledTaskHistoryIdList">日志ID集合</param>
        /// <returns></returns>
        public bool DeleteScheduledTaskHistory(List<int> taskHistoryIdList)
        {
            bool result = false;
            if (taskHistoryIdList != null && taskHistoryIdList.Count > 0)
            {
                int i = 0;
                foreach (var ScheduledTaskHistoryId in taskHistoryIdList)
                {
                    if (DeleteScheduledTaskHistory(taskHistoryIdList))
                        i++;
                }
                result = i > 0;
            }
            return result;
        }


        /// <summary>
        /// Job运行日志记录
        /// </summary>
        /// <param name="jobId">Job ID</param>
        /// <param name="jobName">Job名称</param>
        /// <param name="executedTime">开始执行时间</param>
        /// <param name="executedResult">日志内容</param>
        public void AddScheduledTaskHistory(int jobId, string jobName, DateTime executedTime, string executedResult)
        {
            AddScheduledTaskHistory(jobId, jobName, executedTime, 0, executedResult);
        }

        /// <summary>
        /// Job运行日志记录
        /// </summary>
        /// <param name="jobId">Job ID</param>
        /// <param name="jobName">Job名称</param>
        /// <param name="executedTime">开始执行时间</param>
        /// <param name="duration">执行时长</param>
        /// <param name="executedResult">日志内容</param>
        public void AddScheduledTaskHistory(int jobId, string jobName, DateTime executedTime, double duration, string executedResult)
        {
            ScheduledTaskHistory ScheduledTaskHistoryInfo = new ScheduledTaskHistory();
            ScheduledTaskHistoryInfo.ScheduledTaskId = jobId;
            ScheduledTaskHistoryInfo.JobName = jobName;
            ScheduledTaskHistoryInfo.ExecutedTime = executedTime;
            ScheduledTaskHistoryInfo.ExecutionDuration = duration;
            ScheduledTaskHistoryInfo.CreatedDateTime = DateTime.Now;
            ScheduledTaskHistoryInfo.ExecutedResult = executedResult;
            new ScheduledTaskManager().AddScheduledTaskHistory(ScheduledTaskHistoryInfo);
        }

    }
}
