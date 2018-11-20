# Aurora.Jobs
Quartz.net的调度管理,Job任务放置数据库中,通过管理后台进行调度管理

可以动态添加任务（不重启服务）
任务错误重试机制
web端管理

# 安装

1. Copy项目Release 至C盘 Aurora.Jobs
2. 以管理员身份运行CMD
3. 执行命令:cd C:\Aurora.Jobs
4. 执行命令 Aurora.Jobs install
5. 启动服务 Aurora.Jobs start



# 卸载服务 
  1. Aurora.Jobs uninstall
  2. sc delete Aurora.Jobs
