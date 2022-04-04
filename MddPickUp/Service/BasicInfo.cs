using MddPickUp.Helpers;
using MddPickUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace MddPickUp.Service
{
    public static class BasicInfo
    {
        #region fields
        public static string BaseUri = "https://xwsq.wxatech.com";
        public static string groupNo = "74130029";
        //public static string groupNo = "21997229";
        #endregion


        public static string Host
        {
            get {
                Uri uri = new Uri(BasicInfo.BaseUri);
                return uri.Host; 
            }
        }

        private static Timer ti;
        public static bool isstart;
        public static int interval = 1000;

        public static CommonResult ReadBaseUri()
        {
            BaseUri = IniHelper.GetKeyValue("Main", "BaseUri", "", IniHelper.inipath);
            return new CommonResult(true, "");
        }
        public static CommonResult SaveBaseUri()
        {
            IniHelper.SetKeyValue("Main", "BaseUri", BaseUri, IniHelper.inipath);
            return new CommonResult(true, "");
        }

        public static void ReadXnXq()
        {
            var lastsavetime = IniHelper.GetKeyValue("XnXq", "lastsavetime", null, IniHelper.inipath);
            if (lastsavetime == null || lastsavetime == "") 
                return;
            if (Common.GetTimeStampInt64() - long.Parse(lastsavetime) > TimeSpan.FromDays(5).TotalSeconds)
                return;
        }

        public static void SaveXnXq()
        {
            IniHelper.SetKeyValue("XnXq", "lastsavetime", Common.GetTimeStampInt64().ToString(), IniHelper.inipath);
        }
    }
}
