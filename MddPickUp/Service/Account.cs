using MddPickUp.Helpers;
using MddPickUp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace MddPickUp.Service
{
    public static class Account
    {
        public static string id;
        public static string key;
        public static bool islogin;

        public static void Init()
        {
            id = IniHelper.GetKeyValue("main", "id", null, IniHelper.inipath);
            key = IniHelper.GetKeyValue("main", "key", null, IniHelper.inipath);
        }
        public static void Save(string id1, string key1)
        {
            IniHelper.SetKeyValue("main", "id", id1, IniHelper.inipath);
            IniHelper.SetKeyValue("main", "key", key1, IniHelper.inipath);
        }

        public static LoginResult Login(string id1, string key1)
        {
            try
            {
                id = id1;
                key = key1;
            }
            catch (Exception ex)
            {
                return new LoginResult(LoginState.fail, ex.Message);
            }
            
            return new LoginResult(LoginState.success, "登录成功！");
        }

        public static bool ValidateLogin(string id1, string key1)
        {
            Dictionary<string, string> d = Web.PostCommonHeaders();
            d.Add("x-wx-id", id1);
            d.Add("x-wx-skey", key1);
            var res = Web.Get(BasicInfo.BaseUri + "/fanTan/api/init?ver=3.1.5", d);
            if (!res.success) return false;

            LoginDto json;
            try
            {
                json = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginDto>(res.result);
            }
            catch //(Exception ex)
            {
                return false;
            }

            if (json.code != 0)
            {
                return false;
            }

            Application.Current.Resources["nickname"] = json.data.userInfo.nickname;
            Save(id1, key1);
            return true;
        }

        public class LoginResult
        {
            public LoginState state;
            public string result;

            public LoginResult() { }
            public LoginResult(LoginState state, string result)
            {
                this.state = state;
                this.result = result;
            }
        }

        public enum LoginState
        {
            success, fail, captchafail
        }

        public class LoginDto
        {
            public int code;
            public string msg;
            public datadto data;
        }

        public class datadto
        {
            public userinfodto userInfo;
        }

        public class userinfodto
        {
            public string nickname;
        }
    }
}
