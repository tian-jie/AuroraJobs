using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Aurora.Jobs.Items
{
    public class CommonService
    {
        static log4net.ILog logger = log4net.LogManager.GetLogger("ScheduledTask");
        /// <summary>
        /// 给企业号发送文字消息
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="Messsage"></param>
        /// <returns></returns>
        public static SendMessageToEmployeeResult SendTextMessage(string EmployeeId, string Messsage, string appid = "0")
        {
            var data = new SendMessageToEmployeeData()
            {
                LillyId = EmployeeId,
                AppId = appid,
                Email = string.Empty,
                Message = Messsage
            };

            var jsonData = JsonConvert.SerializeObject(data);
            logger.Debug("Enterprise WeChat SendTextMessage:" + jsonData);

            var QYEncryptKey = ConfigurationManager.AppSettings["QYEncryptKey"];
            //var QYEncryptKey = _sysConfigService.Repository.Entities.FirstOrDefault(a => a.ConfigName == "QYEncryptKey").ConfigValue;
            var content = AesEncrypt(jsonData, QYEncryptKey);
            jsonData = "encriptedPackage=" + UrlEncode(content);
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);

            var enterpriseWeChatHost = ConfigurationManager.AppSettings["EnterpriseWeChatHost"];

            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(enterpriseWeChatHost + "/CAAdmin/EAService/SendMessageToEmployee"));

            webReq.Method = "POST";
            webReq.ContentType = "application/x-www-form-urlencoded";
            webReq.ContentLength = byteArray.Length;
            Stream newStream = webReq.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();
            HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
            SendMessageToEmployeeResult sendMessageToEmployeeResult = null;
            //responseCode:000; responseData:""; remark:ok 
            //responseCode:1201;responseData:""; remark:At lease one of Lilly ID and Email must exist!
            string result = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                result = sr.ReadToEnd();
                sendMessageToEmployeeResult = JsonConvert.DeserializeObject<SendMessageToEmployeeResult>(result);
                sr.Close();
            }
            response.Close();

            logger.Debug(result);
            return sendMessageToEmployeeResult;

        }


        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }

        public static string AesEncrypt(string toEncrypt, string key)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

    }
}

