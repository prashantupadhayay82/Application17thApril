using CustomAuth;
using ExceptionLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace Website.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        public BaseController() { }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (UserContext.CurrentUser.User == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                    filterContext.Result = new JavaScriptResult { Script = "window.location.href ='" + Url.Action("LogOn", "Account", new { area = "admin" }) + "';" };
                else
                    filterContext.Result = new RedirectResult(Url.Action("LogOn", "Account", new { area = "admin" }));
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            Exception ex = filterContext.Exception;
            CustomException.HandleException(ex);
            int ErrorCode = (int)DBEnum.ErrorCode.GeneralError;
            if (ex is HttpAntiForgeryException)
            {
                ErrorCode = (int)DBEnum.ErrorCode.PageSessionExpired;
            }
            if (filterContext.HttpContext.Request.IsAjaxRequest())
                filterContext.Result = new JavaScriptResult { Script = "window.location.href ='" + Url.Action("Error", "Account", new { id = ErrorCode, area = "admin" }) + "';" };
            else
                filterContext.Result = new RedirectResult(Url.Action("Error", "Account", new { id = ErrorCode, area = "admin" }));

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
        }
    }
}