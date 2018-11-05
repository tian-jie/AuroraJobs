using Aurora.Jobs.Core.Business.enums;
using Aurora.Jobs.Core.Business.Info;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Aurora.Jobs.Core.Business.Manager
{
    public class ScheduledTaskManager : BaseManager
    {
        #region ScheduledTaskInfo

        /// <summary>
        /// Job新增
        /// </summary>
        /// <param name="task">ScheduledTaskInfo实体</param>
        /// <returns></returns>
        public bool InsertScheduledTask(ScheduledTask task)
        {
            task.CreatedDateTime = DateTime.Now;
            task.LastUpdatedDateTime = DateTime.Now;
            return db.Insertable(task).ExecuteCommand() > 0;
        }

        /// <summary>
        /// Job修改
        /// </summary>
        /// <param name="task">ScheduledTaskInfo实体</param>
        /// <returns></returns>
        public bool UpdateScheduledTask(ScheduledTask task)
        {
            var ScheduledTaskId = task.Id;

            task.LastUpdatedDateTime = DateTime.Now;
            db.Updateable(task).IgnoreColumns(it => new { it.LastRunTime, it.NextRunTime, it.RunCount, it.CreatedByUserId, it.CreatedByUserName, it.CreatedDateTime, it.IsDelete }).Where(it => it.Id == ScheduledTaskId).ExecuteCommand();
            return true;
        }

        /// <summary>
        /// Job删除
        /// </summary>
        /// <param name="taskId">Job ID</param>
        /// <returns></returns>
        public bool DeleteScheduledTask(int taskId)
        {
            ScheduledTask ScheduledTaskInfo = new ScheduledTask();

            ScheduledTaskInfo.Id = taskId;
            ScheduledTaskInfo.IsDelete = 1;
            ScheduledTaskInfo.LastUpdatedDateTime = DateTime.Now;
            db.Updateable(ScheduledTaskInfo).UpdateColumns(it => new { it.IsDelete, it.LastUpdatedDateTime }).Where(it => it.Id == taskId).ExecuteCommand();
            return true;
        }

        /// <summary>
        /// Job详情
        /// </summary>
        /// <param name="taskId">Job ID</param>
        /// <returns></returns>
        public ScheduledTask GetScheduledTaskInfo(int taskId)
        {
            return db.Queryable<ScheduledTask>().Where(it => it.Id == taskId).First();
        }

        /// <summary>
        /// Job集合(分页 )
        /// </summary>
        /// <param name="parameter">参数集</param>
        /// <returns></returns>
        public PagerModel<ScheduledTask> GeScheduledTaskInfoPagerList(PageParameter parameter)
        {
            int TotalRecord = 0;
            List<ScheduledTask> dataList = null;
            string Name = parameter.GetParameter("Name");
            if (!string.IsNullOrWhiteSpace(Name))
            {
                dataList = db.Queryable<ScheduledTask>()
                    .Where(it => it.Name.Contains(Name) && it.IsDelete == 0)
                     .OrderBy(it => it.CreatedDateTime, OrderByType.Desc)
                     .ToPageList(parameter.currentPageIndex, parameter.rows, ref TotalRecord);
            }
            else
            {
                dataList = db.Queryable<ScheduledTask>()
                    .Where(it => it.IsDelete == 0)
                      .OrderBy(it => it.CreatedDateTime, OrderByType.Desc)
                      .ToPageList(parameter.currentPageIndex, parameter.rows, ref TotalRecord);
            }

            PagerModel<ScheduledTask> pagerModel = new PagerModel<ScheduledTask>();
            pagerModel.dataList = dataList;
            pagerModel.TotalRecord = TotalRecord;
            pagerModel.CurrentPage = parameter.currentPageIndex;
            pagerModel.CalculateTotalPage(parameter.rows, TotalRecord);
            return pagerModel;
        }

        /// <summary>
        /// 获取允许调度的Job集合
        /// </summary>
        /// <returns></returns>
        public List<ScheduledTask> GeAllowScheduleJobInfoList()
        {
            List<ScheduledTask> list = null;
            list = db.Queryable<ScheduledTask>().Where(it => it.IsDelete == 0 && it.IsEnabled == true).OrderBy(it => it.CreatedDateTime, OrderByType.Desc).ToList();
            return list;
        }

        /// <summary>
        /// 更新Job状态
        /// </summary>
        /// <param name="taskId">Job ID</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public bool UpdateScheduledTaskState(int taskId, JobStatus status)
        {
            ScheduledTask ScheduledTaskInfo = new ScheduledTask();
            ScheduledTaskInfo.Id = taskId;
            ScheduledTaskInfo.Status = status;
            db.Updateable(ScheduledTaskInfo).UpdateColumns(it => new { it.Status }).Where(it => it.Id == taskId).ExecuteCommand();
            return true;
        }

        /// <summary>
        /// 更新Job运行信息 
        /// </summary>
        /// <param name="taskId">Job ID</param>
        /// <param name="lastRunTime">最后运行时间</param>
        /// <param name="nextRunTime">下次运行时间</param>
        public void UpdateScheduledTaskStatus(int taskId, DateTime lastRunTime, DateTime nextRunTime)
        {
            db.Updateable<ScheduledTask>()
                .ReSetValue(it => it.RunCount == (it.RunCount + 1))
                .UpdateColumns(it => new ScheduledTask() { LastRunTime = lastRunTime, NextRunTime = nextRunTime })
                .Where(it => it.Id == taskId)
                .ExecuteCommand();
        }
        #endregion

        #region ScheduledTaskHistoryInfo


        /// <summary>
        /// Job日志详情
        /// </summary>
        /// <param name="taskHistoryId">日志ID</param>
        /// <returns></returns>
        public ScheduledTaskHistory GetScheduledTaskHistoryInfo(int taskHistoryId)
        {
            return db.Queryable<ScheduledTaskHistory>().Where(it => it.Id == taskHistoryId).First();
        }

        /// <summary>
        /// Job日志集合（分页）
        /// </summary>
        /// <param name="parameter">参数集</param>
        /// <returns></returns>
        public PagerModel<ScheduledTaskHistory> GetScheduledTaskHistoryInfoPagerList(PageParameter parameter)
        {
            int TotalRecord = 0;
            List<ScheduledTaskHistory> dataList = null;
            string JobName = parameter.GetParameter("JobName");
            if (!string.IsNullOrWhiteSpace(JobName))
            {
                dataList = db.Queryable<ScheduledTaskHistory>()
                    .Where(it => it.JobName.Contains(JobName))
                     .OrderBy(it => it.CreatedDateTime, OrderByType.Desc)
                     .ToPageList(parameter.currentPageIndex, parameter.rows, ref TotalRecord);
            }
            else
            {
                dataList = db.Queryable<ScheduledTaskHistory>()
                      .OrderBy(it => it.CreatedDateTime, OrderByType.Desc)
                      .ToPageList(parameter.currentPageIndex, parameter.rows, ref TotalRecord);
            }

            PagerModel<ScheduledTaskHistory> pagerModel = new PagerModel<ScheduledTaskHistory>();
            pagerModel.dataList = dataList;
            pagerModel.TotalRecord = TotalRecord;
            pagerModel.CurrentPage = parameter.currentPageIndex;
            pagerModel.CalculateTotalPage(parameter.rows, TotalRecord);
            return pagerModel;
        }

        /// <summary>
        /// Job日志删除
        /// </summary>
        /// <param name="taskHistoryId">日志ID</param>
        /// <returns></returns>
        public bool DeleteScheduledTaskHistory(int taskHistoryId)
        {
            return db.Deleteable<ScheduledTaskHistory>().Where(it => it.Id == taskHistoryId).ExecuteCommand() > 0;
        }

        /// <summary>
        /// Job日志记录
        /// </summary>
        /// <param name="jobHistory">ScheduledTaskHistoryModel</param>
        public void AddScheduledTaskHistory(ScheduledTaskHistory jobHistory)
        {
            db.Insertable(jobHistory).ExecuteCommand();
        }
        #endregion
    }
}
