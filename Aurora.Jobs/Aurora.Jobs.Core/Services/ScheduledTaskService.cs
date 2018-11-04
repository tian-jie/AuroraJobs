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
        /// <param name="ScheduledTaskInfo">ScheduledTaskInfo实体</param>
        /// <returns></returns>
        public bool InsertScheduledTask(ScheduledTask ScheduledTaskInfo)
        {
            return new ScheduledTaskManager().InsertScheduledTask(ScheduledTaskInfo);
        }

        /// <summary>
        /// Job修改
        /// </summary>
        /// <param name="ScheduledTaskInfo">ScheduledTaskInfo实体</param>
        /// <returns></returns>
        public bool UpdateScheduledTask(ScheduledTask ScheduledTaskInfo)
        {
            return new ScheduledTaskManager().UpdateScheduledTask(ScheduledTaskInfo);
        }

        /// <summary>
        /// Job删除
        /// </summary>
        /// <param name="ScheduledTaskId">Job ID</param>
        /// <returns></returns>
        public bool DeleteScheduledTask(System.Guid ScheduledTaskId)
        {
            return new ScheduledTaskManager().DeleteScheduledTask(ScheduledTaskId);
        }

        /// <summary>
        /// Job删除
        /// </summary>
        /// <param name="idList">ID集合</param>
        /// <returns></returns>
        public bool DeleteScheduledTask(List<System.Guid> idList, out string rtMsg)
        {
            bool result = false;
            rtMsg = string.Empty;
            int i = 0;
            if (idList != null && idList.Count > 0)
            {
                foreach (System.Guid ScheduledTaskId in idList)
                {
                    ScheduledTask ScheduledTaskInfo = GetScheduledTaskInfo(ScheduledTaskId);
                    if (ScheduledTaskInfo.Status != JobStatus.Idle)
                    {
                        rtMsg = string.Format("{0}状态不为 停止状态,无法进行删除！", ScheduledTaskInfo.Name);
                        return false;
                    }
                }

                foreach (System.Guid ScheduledTaskId in idList)
                {
                    DeleteScheduledTask(ScheduledTaskId);
                    i++;
                }
            }
            result = i > 0;
            return result;
        }

        /// <summary>
        /// Job详情
        /// </summary>
        /// <param name="ScheduledTaskId">Job ID</param>
        /// <returns></returns>
        public ScheduledTask GetScheduledTaskInfo(System.Guid ScheduledTaskId)
        {
            return new ScheduledTaskManager().GetScheduledTaskInfo(ScheduledTaskId);
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
        /// <param name="ScheduledTaskId">Job ID</param>
        /// <param name="State">状态</param>
        /// <returns></returns>
        public bool UpdateScheduledTaskState(System.Guid ScheduledTaskId, JobStatus State)
        {
            return new ScheduledTaskManager().UpdateScheduledTaskState(ScheduledTaskId, State);
        }

        /// <summary>
        /// 更新Job运行信息 
        /// </summary>
        /// <param name="ScheduledTaskId">Job ID</param>
        /// <param name="LastRunTime">最后运行时间</param>
        /// <param name="NextRunTime">下次运行时间</param>
        public void UpdateScheduledTaskStatus(System.Guid ScheduledTaskId, DateTime LastRunTime, DateTime NextRunTime)
        {
            new ScheduledTaskManager().UpdateScheduledTaskStatus(ScheduledTaskId, LastRunTime, NextRunTime);
        }

        /// <summary>
        /// 更新Job运行信息 
        /// </summary>
        /// <param name="ScheduledTaskId">Job ID</param>
        /// <param name="JobName">Job名称</param>
        /// <param name="LastRunTime">最后运行时间</param>
        /// <param name="NextRunTime">下次运行时间</param>
        /// <param name="ExecutionDuration">运行时长</param>
        /// <param name="RunLog">日志</param>
        public void UpdateScheduledTaskStatus(System.Guid ScheduledTaskId, string JobName, DateTime LastRunTime, DateTime NextRunTime, double ExecutionDuration, string RunLog)
        {
            UpdateScheduledTaskStatus(ScheduledTaskId, LastRunTime, NextRunTime);
            AddScheduledTaskHistory(ScheduledTaskId, JobName, LastRunTime, ExecutionDuration, RunLog);
        }


        /// <summary>
        /// Job日志详情
        /// </summary>
        /// <param name="ScheduledTaskHistoryId">日志ID</param>
        /// <returns></returns>
        public ScheduledTaskHistory GetScheduledTaskHistoryInfo(System.Guid ScheduledTaskHistoryId)
        {
            return new ScheduledTaskManager().GetScheduledTaskHistoryInfo(ScheduledTaskHistoryId);
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
        public bool DeleteScheduledTaskHistory(System.Guid ScheduledTaskHistoryId)
        {
            return new ScheduledTaskManager().DeleteScheduledTaskHistory(ScheduledTaskHistoryId);
        }

        /// <summary>
        /// Job 日志删除
        /// </summary>
        /// <param name="ScheduledTaskHistoryIdList">日志ID集合</param>
        /// <returns></returns>
        public bool DeleteScheduledTaskHistory(List<System.Guid> ScheduledTaskHistoryIdList)
        {
            bool result = false;
            if (ScheduledTaskHistoryIdList != null && ScheduledTaskHistoryIdList.Count > 0)
            {
                int i = 0;
                foreach (System.Guid ScheduledTaskHistoryId in ScheduledTaskHistoryIdList)
                {
                    if (DeleteScheduledTaskHistory(ScheduledTaskHistoryId))
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
        public void WriteBackgroundJoLog(Guid jobId, string jobName, DateTime executedTime, string executedResult)
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
        public void AddScheduledTaskHistory(Guid jobId, string jobName, DateTime executedTime, double duration, string executedResult)
        {
            ScheduledTaskHistory ScheduledTaskHistoryInfo = new ScheduledTaskHistory();
            ScheduledTaskHistoryInfo.ScheduledTaskHistoryId = System.Guid.NewGuid();
            ScheduledTaskHistoryInfo.ScheduledTaskId = jobId;
            ScheduledTaskHistoryInfo.JobName = jobName;
            ScheduledTaskHistoryInfo.ExecutionTime = executedTime;
            ScheduledTaskHistoryInfo.ExecutionDuration = duration;
            ScheduledTaskHistoryInfo.CreatedDateTime = DateTime.Now;
            ScheduledTaskHistoryInfo.RunLog = executedResult;
            new ScheduledTaskManager().AddScheduledTaskHistory(ScheduledTaskHistoryInfo);
        }

    }
}
