﻿using Aurora.Jobs.Core.Business.enums;
using System;

namespace Aurora.Jobs.Core.Business.Info
{
    /// <summary>
    /// Job信息
    /// </summary>
    public class ScheduledTask
    {
        /// <summary>
        /// JobID
        /// </summary>				
        public int Id { get; set; }

        /// <summary>
        /// Job类型
        /// </summary>
        public string JobType { get; set; }

        /// <summary>
        /// Job名称
        /// </summary>				
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>				
        public string Description { get; set; }

        /// <summary>
        /// 程序集名称(所属程序集)
        /// </summary>				
        public string AssemblyName { get; set; }

        /// <summary>
        /// 类名(完整命名空间的类名)
        /// </summary>				
        public string ClassName { get; set; }

        /// <summary>
        /// 参数
        /// </summary>				
        public string JobArgs { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>				
        public string CronExpression { get; set; }

        /// <summary>
        /// Cron表达式描述
        /// </summary>				
        public string CronExpressionDescription { get; set; }

        /// <summary>
        /// 最后运行时间
        /// </summary>				
        public DateTime? LastRunTime { get; set; }

        /// <summary>
        /// 下次运行时间
        /// </summary>				
        public DateTime? NextRunTime { get; set; }

        /// <summary>
        /// 运行次数
        /// </summary>
        public int RunCount { get; set; }

        /// <summary>
        /// 状态  0-停止  1-运行   3-正在启动中...   5-停止中...
        /// </summary>
        public JobStatus Status { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 报警类型
        /// </summary>
        public WarningType WarningType { get; set; }

        /// <summary>
        /// 报警对象，用|隔开，如A|B|C
        /// </summary>
        public String WarningTo { get; set; }

        /// <summary>
        /// 排序
        /// </summary>				
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>				
        public string CreatedByUserId { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>				
        public string CreatedByUserName { get; set; }

        /// <summary>
        /// 创建日期时间
        /// </summary>				
        public System.DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// 最后更新人ID
        /// </summary>				
        public string LastUpdatedByUserId { get; set; }

        /// <summary>
        /// 最后更新人姓名
        /// </summary>				
        public string LastUpdatedByUserName { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdatedDateTime { get; set; }

        /// <summary>
        /// 最后执行结果
        /// </summary>
        public string LastExecutedResult { get; set; }

        /// <summary>
        /// 是否删除 0-未删除   1-已删除
        /// </summary>				
        public int IsDelete { get; set; }
    }
}
