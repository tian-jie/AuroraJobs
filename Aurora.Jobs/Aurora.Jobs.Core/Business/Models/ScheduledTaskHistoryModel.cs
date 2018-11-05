using SqlSugar;
using System;

namespace Aurora.Jobs.Core.Business.Info
{
    /// <summary>
    /// 后台任务执行日志
    /// </summary>
    public class ScheduledTaskHistory
    {
        /// <summary>
        /// JobID
        /// </summary>				
        public int Id { get; set; }

        /// <summary>
        /// JobID
        /// </summary>
        public int ScheduledTaskId { get; set; }

        /// <summary>
        /// Job名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>				
        public DateTime? ExecutedTime { get; set; }

        /// <summary>
        /// 执行持续时长
        /// </summary>				
        public double? ExecutionDuration { get; set; }

        /// <summary>
        /// 创建日期时间
        /// </summary>				
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>				
        public string ExecutedResult { get; set; }

    }
}
