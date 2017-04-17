using System;
using System.Reflection;
using System.Collections;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.Configuration;

namespace CustomAuth
{
    /// <summary>
    /// Provides static methods that supply helper utilities for authenticating identites. 
    /// This class cannot be inherited.
    /// </summary>
    public sealed class CustomAuthentication
    {
        #region static methods
        /// <summary>
        /// Redirects an authenticated user back to the originally requested URL.
        /// </summary>
        /// <param name="identity">CustomIdentity of an authenticated user</param>
        public static void RedirectFromLoginPage(CustomIdentity identity)
        {
            string cookieName = AuthUtility.AUTHENTICATION_COOKIE_NAME;
            if (string.IsNullOrEmpty(cookieName))
            {
                throw new Exception(" CustomAuthentication.CookieName entry not found in appSettings section section of Web.config");
            }
            string cookieTimeout = AuthUtility.AUTHENTICATION_COOKIE_TIMEOUT;
            HttpRequest request = HttpContext.Current.Request;
            HttpResponse response = HttpContext.Current.Response;
            string strEncryption = AuthUtility.Encrypt(identity.User);

            HttpCookie userCookie = new HttpCookie(cookieName, identity.Name);
            HttpCookie userDataCookie = new HttpCookie(identity.Name, strEncryption);

            if (cookieTimeout != null && cookieTimeout.Trim() != String.Empty)
            {
                userCookie.Expires = DateTime.Now.AddMinutes(int.Parse(cookieTimeout));
            }
            response.Cookies.Add(userCookie);
            string returnUrl = Convert.ToString(request["ReturnUrl"]);
            if (!string.IsNullOrEmpty(returnUrl))
                response.Redirect(returnUrl, false);
            else
                response.Redirect("/home.aspx", false);
        }

        /// <summary>
        /// Set Authentication Cookie by encrypting & decrypting cookie value
        /// </summary>
        /// <param name="identity"></param>
        public static void SetAuthCookie(CustomIdentity identity)
        {
            string cookieName = AuthUtility.AUTHENTICATION_COOKIE_NAME;
            if (string.IsNullOrEmpty(cookieName))
            {
                throw new Exception("CustomAuthentication.CookieName entry not found in appSettings section section of Web.config");
            }
            string cookieTimeout = AuthUtility.AUTHENTICATION_COOKIE_TIMEOUT;
            if (!string.IsNullOrEmpty(cookieTimeout))
            {
                HttpRequest request = HttpContext.Current.Request;
                HttpResponse response = HttpContext.Current.Response;
                var strEncryption = AuthUtility.Encrypt(identity.User);

                HttpCookie reqUserKey = request.Cookies[cookieName];
                if (reqUserKey != null && reqUserKey.Expires != DateTime.MinValue)
                {
                    HttpCookie reqDataKey = request.Cookies[reqUserKey.Value];
                    if (reqDataKey != null && reqDataKey.Expires != DateTime.MinValue)
                    {
                        response.Cookies[reqDataKey.Name].Value = strEncryption;
                        response.Cookies[reqDataKey.Name].Expires = DateTime.Now.AddMinutes(int.Parse(cookieTimeout));
                    }
                }
                else
                {
                    HttpCookie userCookie = new HttpCookie(cookieName, identity.Name);
                    HttpCookie userDataCookie = new HttpCookie(identity.Name, strEncryption);
                    userCookie.Expires = DateTime.Now.AddMinutes(int.Parse(cookieTimeout));
                    userDataCookie.Expires = DateTime.Now.AddMinutes(int.Parse(cookieTimeout));

                    response.Cookies.Add(userDataCookie);
                    response.Cookies.Add(userCookie);
                }
            }          
        }

        /// <summary>
        /// Remove Authentication Cookie by setting Cookie expiry time to -10 days
        /// </summary>
        /// <param name="identity"></param>
        public static void RemoveAuthCookie(CustomIdentity identity)
        {
            string cookieName = AuthUtility.AUTHENTICATION_COOKIE_NAME;
            HttpCookie userCookie = new HttpCookie(cookieName);
            if (userCookie != null)
            {
                HttpCookie userDataCookie = new HttpCookie(identity.Name);
                userCookie.Expires = DateTime.Now.AddDays(-10);
                userDataCookie.Expires = DateTime.Now.AddDays(-10);
                HttpContext.Current.Response.Cookies.Add(userCookie);
                HttpContext.Current.Response.Cookies.Add(userDataCookie);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Returns the configured cookie name used for the current application
        /// </summary>
        public string CookieName
        {
            get
            {
                string cookieName = AuthUtility.AUTHENTICATION_COOKIE_NAME;
                if (string.IsNullOrEmpty(cookieName))
                {
                    throw new Exception(" CustomAuthentication.Cookie.Name entry not found in appSettings section section of Web.config");
                }
                return cookieName;
            }
        }
        #endregion
    }
}
