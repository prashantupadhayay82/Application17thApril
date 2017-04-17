using Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;
using EntityLayer;
using CustomAuth;
using System.Collections;
using ExceptionLayer;
using BusinessLayer.BusinessLogic;
using ResourceLayer;

namespace Website.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        ApplicationDBEntities db = new ApplicationDBEntities();
        public ActionResult Index()
        {
            return RedirectToAction("LogOn");
        }

        public ActionResult LogOn()
        {
            if (UserContext.CurrentUser.User == null)
                return View();
            else
                return RedirectToAction("Index", "Dashboard", new { area = "admin" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOn(AdminLogOnModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserContextModel userModel = tblUserMasterBL.Instance.ValidateUser(db, model.UserName, model.Password);
                    if (userModel != null)
                    {
                        PrepareUserSession(userModel);
                        return RedirectToAction("Index", "Dashboard", new { area = "admin" });
                    }
                    else
                    {
                        ViewData[StringUtility.ErrorMessage] = ResourceLayer.ResourceFile.User_Invalid_Username_Password;
                    }
                }
                catch (InvalidUsernamePasswordException ex)
                {
                    ViewData[StringUtility.ErrorMessage] = ex.Message;
                }
                catch (Exception ex)
                {
                    ViewData[StringUtility.ErrorMessage] = ex.Message;
                }
            }
            return View(model);
        }

        public ActionResult LogOff()
        {
            CustomAuthentication.RemoveAuthCookie(UserContext.CurrentUser);
            return RedirectToAction("LogOn", "Account", new { area = "admin" });
        }

        public void PrepareUserSession(UserContextModel model)
        {
            ArrayList roles = new ArrayList();
            CustomIdentity userIdentity = new CustomIdentity(model);
            CustomPrincipal principal = new CustomPrincipal(userIdentity, roles);
            HttpContext.User = principal;
            CustomAuthentication.SetAuthCookie(userIdentity);
        }

        public ActionResult Error(int? id)
        {
            switch (id)
            {
                case (int)DBEnum.ErrorCode.GeneralError:
                    ViewData[StringUtility.ErrorMessage] = ResourceFile.Failure_General;
                    break;
                case (int)DBEnum.ErrorCode.LoginSessionExpired:
                    ViewData[StringUtility.ErrorMessage] = ResourceFile.Login_Session_Expired;
                    break;
                case (int)DBEnum.ErrorCode.PageSessionExpired:
                    ViewData[StringUtility.ErrorMessage] = ResourceFile.Page_Session_Expired;
                    break;
                case (int)DBEnum.ErrorCode.InvalidRequest:
                    ViewData[StringUtility.ErrorMessage] = ResourceFile.Invalid_Request;
                    break;
                case (int)DBEnum.ErrorCode.error505:
                    ViewData[StringUtility.ErrorMessage] = ResourceFile.Failure_General;
                    break;
                default:
                    ViewData[StringUtility.ErrorMessage] = ResourceFile.Failure_General;
                    break;
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}