/*
Navicat SQL Server Data Transfer

Source Server         : xxx.xxx.xxx.xxx
Source Server Version : 110000
Source Host           : xxx.xxx.xxx.xxx:1433
Source Database       : scheduledjobs
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 110000
File Encoding         : 65001

Date: 2018-11-05 09:21:24
*/


-- ----------------------------
-- Table structure for ScheduledTask
-- ----------------------------
DROP TABLE [dbo].[ScheduledTask]
GO
CREATE TABLE [dbo].[ScheduledTask] (
[Id] int NOT NULL IDENTITY(1,1) ,
[JobType] nvarchar(50) NULL ,
[Name] nvarchar(255) NULL ,
[AssemblyName] nvarchar(255) NULL ,
[ClassName] nvarchar(255) NULL ,
[Description] nvarchar(255) NULL ,
[JobArgs] nvarchar(2000) NULL ,
[CronExpression] nvarchar(255) NULL ,
[CronExpressionDescription] nvarchar(255) NULL ,
[NextRunTime] datetime NULL ,
[LastRunTime] datetime NULL ,
[RunCount] int NOT NULL ,
[Status] int NOT NULL ,
[DisplayOrder] int NOT NULL ,
[CreatedByUserId] nvarchar(40) NULL ,
[CreatedByUserName] nvarchar(80) NULL ,
[CreatedDateTime] datetime NOT NULL ,
[LastUpdatedByUserId] nvarchar(40) NULL ,
[LastUpdatedByUserName] nvarchar(80) NULL ,
[LastUpdatedDateTime] datetime NOT NULL ,
[IsDelete] int NOT NULL ,
[IsEnabled] bit NULL ,
[LastExecutedResult] nvarchar(2000) NULL 
)


GO

-- ----------------------------
-- Records of ScheduledTask
-- ----------------------------
SET IDENTITY_INSERT [dbo].[ScheduledTask] ON
GO
INSERT INTO [dbo].[ScheduledTask] ([Id], [JobType], [Name], [AssemblyName], [ClassName], [Description], [JobArgs], [CronExpression], [CronExpressionDescription], [NextRunTime], [LastRunTime], [RunCount], [Status], [DisplayOrder], [CreatedByUserId], [CreatedByUserName], [CreatedDateTime], [LastUpdatedByUserId], [LastUpdatedByUserName], [LastUpdatedDateTime], [IsDelete], [IsEnabled], [LastExecutedResult]) VALUES (N'1', N'test', N'百度一下', N'Aurora.Jobs.Items.UrlJobs', N'Aurora.Jobs.Items.UrlJob', N'百度一下', N'{"url":"https://www.baidu.com/t=111"}', N'0/10, * * * * ? *', N'每10秒执行一次', null, null, N'0', N'0', N'1', N'jie.tian', N'Jie Tian', N'2018-11-05 09:20:11.000', N'jie.tian', N'Jie Tian', N'2018-11-05 09:20:35.000', N'0', N'1', null)
GO
GO
SET IDENTITY_INSERT [dbo].[ScheduledTask] OFF
GO

-- ----------------------------
-- Table structure for ScheduledTaskHistory
-- ----------------------------
DROP TABLE [dbo].[ScheduledTaskHistory]
GO
CREATE TABLE [dbo].[ScheduledTaskHistory] (
[Id] int NOT NULL IDENTITY(1,1) ,
[ScheduledTaskId] int NOT NULL ,
[JobName] nvarchar(255) NULL ,
[ExecutedTime] datetime NULL ,
[ExecutionDuration] decimal(18,3) NULL ,
[CreatedDateTime] datetime NOT NULL ,
[ExecutedResult] text NULL 
)


GO

-- ----------------------------
-- Records of ScheduledTaskHistory
-- ----------------------------
SET IDENTITY_INSERT [dbo].[ScheduledTaskHistory] ON
GO
SET IDENTITY_INSERT [dbo].[ScheduledTaskHistory] OFF
GO
