using Aurora.Jobs.Core.Business.enums;
using log4net;
using Newtonsoft.Json;
using Quartz;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Aurora.Jobs.Items
{
    //不允许此 Job 并发执行任务（禁止新开线程执行）
    [DisallowConcurrentExecution]
    public sealed class UrlJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(UrlJob));
        string Message = "";

        public async Task Execute(IJobExecutionContext context)
        {
            var warningType = WarningType.None;
            var warningTo = "";
            var warningContent = "";
            var jobName = "";
            var warningMessage = "";
            bool isWarning = false;

            try
            {
                ServicePointManager.Expect100Continue = false;
                jobName = context.JobDetail.JobDataMap["JobName"] as string;
                Version Ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                warningType = (WarningType)context.JobDetail.JobDataMap["WarningType"];
                warningTo = context.JobDetail.JobDataMap["WarningTo"] as string;

                _logger.InfoFormat($"{jobName} Execute begin");
                var jobParameters = context.JobDetail.JobDataMap["Parameters"].ToString();
                var jobParametersObj = JsonConvert.DeserializeObject<UrlParameterObject>(jobParameters);
                var url = ExtractUrl(jobParametersObj.Url);

                context.MergedJobDataMap.Put("url", url);


                //var httpClient = new HttpClient()
                //{
                //    Timeout = new TimeSpan(1, 0, 0)
                //};

                //var response = await httpClient.GetAsync(url);
                //var content = await response.Content.ReadAsStringAsync();


                var content = "";

                // Create a request for the URL. 		
                var request = WebRequest.Create(url);
                request.Timeout = System.Threading.Timeout.Infinite;
                request.Credentials = CredentialCache.DefaultCredentials;
                var response = (HttpWebResponse)request.GetResponse();
                using (var dataStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(dataStream))
                    {
                        // Read the content.
                        content = reader.ReadToEnd();
                        // Cleanup the streams and the response.
                        reader.Close();
                    }
                    dataStream.Close();
                }
                response.Close();

                context.MergedJobDataMap.Put("executedResult", content);
                //context.MergedJobDataMap.Put("isSuccess", content);
                _logger.InfoFormat($"{jobName} Execute finished");

                var isSuccess = false;
                // 执行结果（需要考虑，如果不是标准JSON返回值怎么处理）
                try
                {
                    var notification = JsonConvert.DeserializeObject<Notification>(content);
                    warningContent = "Status:" + notification.Status + ";Message:" + notification.Message;
                    isSuccess = notification.Status.Equals("200");
                }
                catch
                {
                    // 无法解析json格式的
                    _logger.Debug($"{jobName} 不是标准返回值，查找关键字作为成功返回值:{JsonConvert.SerializeObject(jobParametersObj)}");
                    isSuccess = content.Contains(jobParametersObj.SuccessMessageContains);
                    warningContent = content.Substring(0, 100);
                }

                isWarning = ArrangeWarning(warningType, isSuccess, jobName, warningContent, out warningMessage);

                return;
            }
            catch (Exception ex)
            {
                try
                {
                    _logger.Error($"{jobName} 执行过程中发生异常:" + ex.ToString());
                    var innerEx = ex.InnerException;
                    while (innerEx != null)
                    {
                        _logger.Error(innerEx.Message);
                        innerEx = innerEx.InnerException;
                    }

                    if (warningType != WarningType.None)
                    {
                        isWarning = true;
                        warningMessage = $"计划任务执行结果：失败！\r\n\r\n{jobName}\r\n\r\n{warningContent}\r\n\r\nException:\r\n{ex.Message}";
                    }
                    context.MergedJobDataMap.Put("executedResult", ex.Message);
                }
                catch
                {
                    // do nothing while exception in exception
                }
            }
            finally
            {
                try
                {
                    // 执行完成后根据要求发送提醒消息
                    _logger.Debug($"{jobName} 发报警 isWarning={isWarning},warningTo={warningTo}");
                    if (isWarning && !string.IsNullOrEmpty(warningTo))
                    {
                        CommonService.SendTextMessage(warningTo, warningMessage);
                    }

                    _logger.InfoFormat($"{jobName} Execute end ");
                }
                catch
                {
                    // do nothing while exception in exception
                }
            }
        }

        /// <summary>
        /// 整理要返回的warning相关信息
        /// </summary>
        /// <returns><c>true</c>, if warning was arranged, <c>false</c> otherwise.</returns>
        /// <param name="warningType">Warning type.</param>
        /// <param name="isSuccess">If set to <c>true</c> is success.</param>
        /// <param name="jobName">Job name.</param>
        /// <param name="warningContent">Warning content.</param>
        /// <param name="warningMessage">Warning message.</param>
        private bool ArrangeWarning(WarningType warningType, bool isSuccess, string jobName, string warningContent, out string warningMessage)
        {
            var isWarning = false;
            warningMessage = "";
            if (warningType == WarningType.Always)
            {
                if (isSuccess)
                {
                    isWarning = true;
                    warningMessage = string.Format("计划任务执行结果：成功\r\n\r\n{0}\r\n\r\n{1}", jobName, warningContent);
                }
                else
                {
                    isWarning = true;
                    warningMessage = string.Format("计划任务执行结果：失败！\r\n\r\n{0}\r\n\r\n{1}\r\n\r\nException:\r\n{2}", jobName, warningContent, Message);
                }
            }
            else if (warningType == WarningType.OnError)
            {
                if (!isSuccess)
                {
                    isWarning = true;
                    warningMessage = string.Format("计划任务执行结果：失败！\r\n\r\n{0}\r\n\r\n{1}\r\n\r\nException:\r\n{2}", jobName, warningContent, Message);
                }
            }

            return isWarning;
        }

        /// <summary>
        /// 检查返回是否是200，如果不是200则返回false
        /// 非标准的返回字符串通过successMessageContains字段判断，response中包含这些串就是正常返回
        /// </summary>
        /// <param name="responseContent"></param>
        /// <param name="successMessageContains"></param>
        /// <returns></returns>
        public bool CheckForRequestStatus(String responseStatus, string successMessageContains, String responseContent, String responseMessage)
        {
            if (responseStatus.Equals("200") || responseContent.Contains(successMessageContains))
            {
                return true;
            }
            else
            {
                Message = responseMessage;
                return false;
            }
        }

        /// <summary>
        /// 将传进来的URL中的各种标签和转义内容转成真实数据
        /// 比如：url中的日期标签((MonthFirstDay[, yyyy-MM-dd])), ((Today[, yyyy-MM-dd])), ((WeekFirstDay[, yyyy-MM-dd]))
        /// 后面中括号中的格式可以省略，不需要中括号
        /// 例子：
        ///     ((MonthFirstDay, yyyy-MM-dd))
        ///     ((MonthFirstDay))
        ///     ((WeekFirstDay))
        ///     ((Yesterday))
        ///     ((TheDayBeforeYesterday))
        ///     ((Now-5m))
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string ExtractUrl(string url)
        {
            var now = DateTime.Now;
            var monthFirstDay = now.AddDays(-now.Day + 1);
            var today = now;
            var weekFirstDay = now.AddDays(-(int)now.DayOfWeek + 1);
            var yesterday = now.AddDays(-1);
            var theDayBeforeYesterday = now.AddDays(-2);

            var extractedUrl = url;

            string pat = @"\(\((.*?)(([+-]\d+?)([smhd]))*?\)\)";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            var ms = r.Matches(url);
            foreach (Match m in ms)
            {
                var s = m.Value;
                var kv = s.Split(',');
                string key = "";
                string format = "";
                string fullString = m.ToString();

                var text = m.Groups[1].Value.ToLower();
                if (text.Contains("now"))
                {
                    // 如果包含now的话，就应该是这个格式：now+5h, now-4m
                    var number = int.Parse(m.Groups[3].Value);
                    var unit = m.Groups[4].Value;

                    // 转换为秒
                    var seconds = 0;
                    switch (unit)
                    {
                        case "s":
                            seconds = number;
                            break;
                        case "m":
                            seconds = number * 60;
                            break;
                        case "h":
                            seconds = number * 3600;
                            break;
                        case "d":
                            seconds = number * 86400;
                            break;
                        default:
                            seconds = 0;
                            break;
                    }

                    // 转换为需要的时间
                    var neededTime = now.AddSeconds(seconds);
                }
                key = kv[0].Trim('(').Trim();
                if (kv.Length == 2)
                {
                    format = kv[1].Trim(')').Trim();
                }
                else
                {
                    key = key.Trim(')').Trim();
                }
                string value = "";
                switch (key.ToLower())
                {
                    case "monthfirstday":
                        value = monthFirstDay.ToString(string.IsNullOrEmpty(format) ? "yyyy-MM-dd" : format);
                        break;
                    case "today":
                        value = today.ToString(string.IsNullOrEmpty(format) ? "yyyy-MM-dd" : format);
                        break;
                    case "weekfirstday":
                        value = weekFirstDay.ToString(string.IsNullOrEmpty(format) ? "yyyy-MM-dd" : format);
                        break;
                    case "yesterday":
                        value = yesterday.ToString(string.IsNullOrEmpty(format) ? "yyyy-MM-dd" : format);
                        break;
                    case "thedaybeforeyesterday":
                        value = theDayBeforeYesterday.ToString(string.IsNullOrEmpty(format) ? "yyyy-MM-dd" : format);
                        break;
                }

                // 如果存在，则解析，如果有format，就用format样式打印出来
                extractedUrl = extractedUrl.Replace(fullString, value);

            }
            return extractedUrl;
        }
    }

    /// <summary>
    /// UrlParameterObject
    /// </summary>
    public class UrlParameterObject
    {
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 包含什么字符串算成功
        /// </summary>
        public string SuccessMessageContains { get; set; }
    }

    /// <summary>
    /// Notification
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 提醒消息
        /// </summary>
        public string Message { get; set; }
    }
}
