using log4net;
using Quartz;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Aurora.Jobs.Items
{
    //不允许此 Job 并发执行任务（禁止新开线程执行）
    [DisallowConcurrentExecution]
    public sealed class UrlJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(UrlJob));

        public async Task Execute(IJobExecutionContext context)
        {
            Version Ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            _logger.InfoFormat("JobTestA Execute begin Ver." + Ver.ToString());
            try
            {
                var url = "http://www.baidu.com/?t=" + DateTime.Now.Ticks;
                context.MergedJobDataMap.Put("url", url);
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                context.MergedJobDataMap.Put("executedResult", content);
                //context.MergedJobDataMap.Put("isSuccess", content);

                _logger.InfoFormat("JobTestA Executing ...");

                return;
            }
            catch (Exception ex)
            {
                _logger.Error("JobTestA 执行过程中发生异常:" + ex.ToString());
            }
            finally
            {
                _logger.InfoFormat("JobTestA Execute end ");
            }
        }
    }
}
