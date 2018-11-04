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
        /// <param name="ScheduledTaskInfo">ScheduledTaskInfo实体</param>
        /// <returns></returns>
        public bool InsertScheduledTask(ScheduledTask ScheduledTaskInfo)
        {
            ScheduledTaskInfo.CreatedDateTime = DateTime.Now;
            ScheduledTaskInfo.LastUpdatedDateTime = DateTime.Now;
            return db.Insertable(ScheduledTaskInfo).ExecuteCommand() > 0;
        }

        /// <summary>
        /// Job修改
        /// </summary>
        /// <param name="ScheduledTaskInfo">ScheduledTaskInfo实体</param>
        /// <returns></returns>
        public bool UpdateScheduledTask(ScheduledTask ScheduledTaskInfo)
        {
            System.Guid ScheduledTaskId = ScheduledTaskInfo.ScheduledTaskId;

            ScheduledTaskInfo.LastUpdatedDateTime = DateTime.Now;
            db.Updateable(ScheduledTaskInfo).IgnoreColumns(it => new { it.LastRunTime, it.NextRunTime, it.RunCount, it.CreatedByUserId, it.CreatedByUserName, it.CreatedDateTime, it.IsDelete }).Where(it => it.ScheduledTaskId == ScheduledTaskId).ExecuteCommand();
            return true;
        }

        /// <summary>
        /// Job删除
        /// </summary>
        /// <param name="ScheduledTaskId">Job ID</param>
        /// <returns></returns>
        public bool DeleteScheduledTask(System.Guid ScheduledTaskId)
        {
            ScheduledTask ScheduledTaskInfo = new ScheduledTask();

            ScheduledTaskInfo.ScheduledTaskId = ScheduledTaskId;
            ScheduledTaskInfo.IsDelete = 1;
            ScheduledTaskInfo.LastUpdatedDateTime = DateTime.Now;
            db.Updateable(ScheduledTaskInfo).UpdateColumns(it => new { it.IsDelete, it.LastUpdatedDateTime }).Where(it => it.ScheduledTaskId == ScheduledTaskId).ExecuteCommand();
            return true;
        }

        /// <summary>
        /// Job详情
        /// </summary>
        /// <param name="ScheduledTaskId">Job ID</param>
        /// <returns></returns>
        public ScheduledTask GetScheduledTaskInfo(System.Guid ScheduledTaskId)
        {
            return db.Queryable<ScheduledTask>().Where(it => it.ScheduledTaskId == ScheduledTaskId).First();
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
        /// <param name="ScheduledTaskId">Job ID</param>
        /// <param name="State">状态</param>
        /// <returns></returns>
        public bool UpdateScheduledTaskState(System.Guid ScheduledTaskId, JobStatus State)
        {
            ScheduledTask ScheduledTaskInfo = new ScheduledTask();
            ScheduledTaskInfo.ScheduledTaskId = ScheduledTaskId;
            ScheduledTaskInfo.Status = State;
            db.Updateable(ScheduledTaskInfo).UpdateColumns(it => new { it.Status }).Where(it => it.ScheduledTaskId == ScheduledTaskId).ExecuteCommand();
            return true;
        }

        /// <summary>
        /// 更新Job运行信息 
        /// </summary>
        /// <param name="ScheduledTaskId">Job ID</param>
        /// <param name="LastRunTime">最后运行时间</param>
        /// <param name="NextRunTime">下次运行时间</param>
        public void UpdateScheduledTaskStatus(System.Guid ScheduledTaskId, DateTime LastRunTime, DateTime NextRunTime)
        {
            db.Updateable<ScheduledTask>()
                .ReSetValue(it => it.RunCount == (it.RunCount + 1))
                .UpdateColumns(it => new ScheduledTask() { LastRunTime = LastRunTime, NextRunTime = NextRunTime })
                .Where(it => it.ScheduledTaskId == ScheduledTaskId)
                .ExecuteCommand();
        }
        #endregion

        #region ScheduledTaskHistoryInfo


        /// <summary>
        /// Job日志详情
        /// </summary>
        /// <param name="ScheduledTaskHistoryId">日志ID</param>
        /// <returns></returns>
        public ScheduledTaskHistory GetScheduledTaskHistoryInfo(System.Guid ScheduledTaskHistoryId)
        {
            return db.Queryable<ScheduledTaskHistory>().Where(it => it.ScheduledTaskHistoryId == ScheduledTaskHistoryId).First();
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
        /// <param name="ScheduledTaskHistoryId">日志ID</param>
        /// <returns></returns>
        public bool DeleteScheduledTaskHistory(System.Guid ScheduledTaskHistoryId)
        {
            return db.Deleteable<ScheduledTaskHistory>().Where(it => it.ScheduledTaskHistoryId == ScheduledTaskHistoryId).ExecuteCommand() > 0;
        }

        /// <summary>
        /// Job日志记录
        /// </summary>
        /// <param name="job">ScheduledTaskHistoryModel</param>
        public void AddScheduledTaskHistory(ScheduledTaskHistory job)
        {
            db.Insertable(job).ExecuteCommand();
        }
        #endregion
    }
}
