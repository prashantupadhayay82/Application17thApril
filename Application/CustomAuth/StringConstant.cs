using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace CustomAuth
{
    public static class StringConstant
    {
        public static string AUTHENTICATION_LOGINURL = ConfigurationManager.AppSettings["CustomAuthentication.LoginUrl"];
        public static string AUTHENTICATION_COOKIE_NAME = ConfigurationManager.AppSettings["CustomAuthentication.CookieName"].ToUpper();
        public static string AUTHENTICATION_COOKIE_TIMEOUT = ConfigurationManager.AppSettings["CustomAuthentication.CookieTimeout"];
    }
}
