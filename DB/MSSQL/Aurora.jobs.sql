USE [Aurora.Jobs]
GO
/****** Object:  Table [dbo].[ScheduledTask]    Script Date: 08/01/2017 11:49:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ScheduledTask]') AND type in (N'U'))
DROP TABLE [dbo].[ScheduledTask]
GO
/****** Object:  Table [dbo].[ScheduledTaskHistory]    Script Date: 08/01/2017 11:49:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ScheduledTaskHistory]') AND type in (N'U'))
DROP TABLE [dbo].[ScheduledTaskHistory]
GO
/****** Object:  Table [dbo].[ScheduledTaskHistory]    Script Date: 08/01/2017 11:49:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ScheduledTaskHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ScheduledTaskHistory](
	[BackgroundJobLogId] [uniqueidentifier] NOT NULL,
	[BackgroundJobId] [uniqueidentifier] NULL,
	[JobName] [nvarchar](255) NULL,
	[ExecutionTime] [datetime] NULL,
	[ExecutionDuration] [decimal](18, 2) NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[RunLog] [text] NULL,
 CONSTRAINT [PK_BACKGROUNDJOBLOG] PRIMARY KEY NONCLUSTERED 
(
	[BackgroundJobLogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTaskHistory', N'COLUMN',N'BackgroundJobLogId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'记录ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTaskHistory', @level2type=N'COLUMN',@level2name=N'BackgroundJobLogId'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTaskHistory', N'COLUMN',N'BackgroundJobId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'JobId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTaskHistory', @level2type=N'COLUMN',@level2name=N'BackgroundJobId'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTaskHistory', N'COLUMN',N'JobName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTaskHistory', @level2type=N'COLUMN',@level2name=N'JobName'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTaskHistory', N'COLUMN',N'ExecutionTime'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTaskHistory', @level2type=N'COLUMN',@level2name=N'ExecutionTime'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTaskHistory', N'COLUMN',N'ExecutionDuration'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行持续时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTaskHistory', @level2type=N'COLUMN',@level2name=N'ExecutionDuration'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTaskHistory', N'COLUMN',N'CreatedDateTime'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTaskHistory', @level2type=N'COLUMN',@level2name=N'CreatedDateTime'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTaskHistory', N'COLUMN',N'RunLog'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTaskHistory', @level2type=N'COLUMN',@level2name=N'RunLog'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTaskHistory', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'后台任务运行日志' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTaskHistory'
GO
/****** Object:  Table [dbo].[uniqueidentifier]    Script Date: 08/01/2017 11:49:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uniqueidentifier]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ScheduledTask](
	[BackgroundJobId] [uniqueidentifier] NOT NULL,
	[JobType] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NULL,
	[AssemblyName] [nvarchar](255) NULL,
	[ClassName] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[JobArgs] [nvarchar](255) NULL,
	[CronExpression] [nvarchar](255) NULL,
	[CronExpressionDescription] [nvarchar](255) NULL,
	[NextRunTime] [datetime] NULL,
	[LastRunTime] [datetime] NULL,
	[RunCount] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[IsEnabled] [int] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[CreatedByUserId] [nvarchar](40) NULL,
	[CreatedByUserName] [nvarchar](80) NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[LastUpdatedByUserId] [nvarchar](40) NULL,
	[LastUpdatedByUserName] [nvarchar](80) NULL,
	[LastUpdatedDateTime] [datetime] NOT NULL,
	[IsDelete] [int] NOT NULL,
 CONSTRAINT [PK_BACKGROUNDJOB] PRIMARY KEY NONCLUSTERED 
(
	[BackgroundJobId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'BackgroundJobId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'记录ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'BackgroundJobId'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'JobType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'JobType'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'Name'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'Name'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'AssemblyName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'程序集名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'AssemblyName'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'ClassName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'ClassName'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'Description'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'JobArgs'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'参数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'JobArgs'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'CronExpression'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cron表达式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'CronExpression'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'CronExpressionDescription'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cron表达式描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'CronExpressionDescription'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'NextRunTime'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下次运行时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'NextRunTime'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'LastRunTime'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后运行时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'LastRunTime'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'RunCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'总运行次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'RunCount'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'State'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0-运行   2-停止' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'State'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'DisplayOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'CreatedByUserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'CreatedByUserId'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'CreatedByUserName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'CreatedByUserName'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'CreatedDateTime'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'CreatedDateTime'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'LastUpdatedByUserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后更新人ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'LastUpdatedByUserId'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'LastUpdatedByUserName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后更新人姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'LastUpdatedByUserName'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'LastUpdatedDateTime'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'LastUpdatedDateTime'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', N'COLUMN',N'IsDelete'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ScheduledTask', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'后台任务' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ScheduledTask'
GO
INSERT [dbo].[ScheduledTask] ([BackgroundJobId], [JobType], [Name], [AssemblyName], [ClassName], [Description], [JobArgs], [CronExpression], [CronExpressionDescription], [NextRunTime], [LastRunTime], [RunCount], [State], [DisplayOrder], [CreatedByUserId], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserId], [LastUpdatedByUserName], [LastUpdatedDateTime], [IsDelete]) VALUES (N'ec464d8f-d873-4393-bed1-70b3219c2bb2', N'Job管理', N'Job管理器', N'Only.Jobs.exe', N'Only.Jobs.JobItems.ManagerJob', N'负责Job的动态调度', NULL, N'0/3 * * * * ?', N'每隔3秒执行一次', CAST(0x0000A7BC009AFD58 AS DateTime), CAST(0x0000A7BC009AF9D4 AS DateTime), 1848, 1, 0, NULL, NULL, CAST(0x0000A7AA013C0B64 AS DateTime), NULL, NULL, CAST(0x0000A7AB00FE6D26 AS DateTime), 0)
