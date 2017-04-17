using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;
using System.Collections;
using System.Threading;
using System.Linq;

namespace CustomAuth
{
    public sealed class CustomAuthenticationModule : IHttpModule
    {
        HttpApplication app = null;
        public void Init(HttpApplication httpapp)
        {
            this.app = httpapp;
            app.AuthenticateRequest += new EventHandler(this.OnAuthenticate);
        }

        void OnAuthenticate(object sender, EventArgs e)
        {
            app = (HttpApplication)sender;
            HttpRequest req = app.Request;
            HttpResponse res = app.Response;
            int i = req.Path.LastIndexOf("/");
            string cookieName = AuthUtility.AUTHENTICATION_COOKIE_NAME;
            string cookieTimeout = AuthUtility.AUTHENTICATION_COOKIE_TIMEOUT;
            if (req.Cookies.Count > 0 && req.Cookies[cookieName] != null)
            {
                HttpCookie userCookie = req.Cookies[cookieName];
                if (userCookie != null)
                {
                    string strUserCookieValue = userCookie.Value;
                    if (!string.IsNullOrEmpty(strUserCookieValue))
                    {
                        HttpCookie userDataCookie = req.Cookies[strUserCookieValue];
                        if (userDataCookie != null)
                        {
                            string strUserDataCookieValue = userDataCookie.Value;
                            if (!string.IsNullOrEmpty(strUserDataCookieValue))
                            {
                                userCookie.Expires = DateTime.Now.AddMinutes(int.Parse(cookieTimeout));
                                userDataCookie.Expires = DateTime.Now.AddMinutes(int.Parse(cookieTimeout));
                                res.Cookies.Add(userDataCookie);
                                res.Cookies.Add(userCookie);
                                UserContextModel userContext = (UserContextModel)AuthUtility.Decrypt(strUserDataCookieValue);
                                if (userContext != null)
                                {
                                    CustomIdentity userIdentity = new CustomIdentity { Name = strUserCookieValue, User = userContext };
                                    string[] roles = null;
                                    if (userContext.Roles != null)
                                        roles = userContext.Roles.Select(a => a.role_name).ToArray();
                                    else
                                        roles = new string[] { "admin" };
                                    ArrayList arrRoles = new ArrayList();
                                    arrRoles.InsertRange(0, roles);
                                    CustomPrincipal principal = new CustomPrincipal(userIdentity, arrRoles);
                                    app.Context.User = principal;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //res.Redirect(req.ApplicationPath + loginUrl + "?ReturnUrl=" + req.Path, true);
            }
        }

        public void Dispose() { }
    }
}
