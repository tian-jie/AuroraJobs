﻿using System;
using System.Collections.Generic;

namespace Aurora.Jobs.Core.Common
{
    public class Utils
    {
        /// <summary>
        /// 字符串转List<Guid> 
        /// </summary>
        /// <param name="strArray">字符串,多个ID以 , 隔开</param>
        /// <returns></returns>
        public static List<Guid> StringToGuidList(string strArray)
        {
            List<System.Guid> list = new List<System.Guid>();
            if (!string.IsNullOrWhiteSpace(strArray))
            {
                string[] arrays = strArray.Split(',');
                foreach (string str in arrays)
                {
                    Guid result;
                    if (Guid.TryParse(str, out result))
                        list.Add(result);
                }
            }
            return list;
        }
        /// <summary>
        /// 字符串转List<int> 
        /// </summary>
        /// <param name="strArray">字符串,多个ID以 , 隔开</param>
        /// <returns></returns>
        public static List<int> StringToIntList(string strArray)
        {
            var list = new List<int>();
            if (!string.IsNullOrWhiteSpace(strArray))
            {
                string[] arrays = strArray.Split(',');
                foreach (string str in arrays)
                {
                    int result;
                    if (int.TryParse(str, out result))
                        list.Add(result);
                }
            }
            return list;
        }
    }
}
