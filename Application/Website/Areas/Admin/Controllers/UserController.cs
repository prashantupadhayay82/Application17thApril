using System;
using System.Collections.Generic;
using System.Web.Mvc;
using UtilityLayer;
using Website.Models;
using ExceptionLayer;
using System.Net;
using ResourceLayer;
using BusinessLayer.BusinessLogic;
using EntityLayer;

namespace Website.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        ApplicationDBEntities db = new ApplicationDBEntities();
        
        public ActionResult Index()
        {
            UtilityManager.TempDataToViewData(this.ControllerContext);
            return View(new UserPageModel { Filter = new tblUserFilterModel(), List = LoadContent(null) });
        }

        [HttpPost]
        public ActionResult PartialIndex(tblUserFilterModel filterModel)
        {
            return PartialView(@"~\Areas\Admin\Views\User\_List.cshtml", LoadContent(filterModel));
        }

        public List<tblUserMaster> LoadContent(tblUserFilterModel filterModel)
        {
            List<tblUserMaster> lstResult = new List<tblUserMaster>();
            try
            {
                int PageNumber = 1;
                int Total = 0;

                if (!string.IsNullOrEmpty(Convert.ToString(TempData[StringUtility.Current_Page])))
                    PageNumber = Convert.ToInt32(TempData[StringUtility.Current_Page]);
                else if (!string.IsNullOrEmpty(Convert.ToString(Request["page"])))
                    PageNumber = Convert.ToInt32(Request["page"]);

                lstResult = tblUserMasterBL.Instance.GetData(db, filterModel,PageNumber, out Total);
                ViewData[StringUtility.VDPager] = new PaginationModel().CreatePager(PageNumber, Total);
                ViewBag.CurrentPage = PageNumber;
            }
            catch (Exception ex)
            {
                ViewData[StringUtility.ErrorMessage] = ResourceFile.Failure_General;
                CustomException.HandleException(ex);
            }
            return lstResult;
        }

        public ActionResult Create()
        {
            tblUserMaster model = new tblUserMaster();
            return View(@"~\Areas\Admin\Views\User\Create.cshtml", model);
        }

        public ActionResult Edit(Int64 Id, int current_page)
        {
            ViewData[StringUtility.Current_Page] = current_page;
            if (Id > 0)
            {
                tblUserMaster model = tblUserMasterBL.Instance.GetDataByPK(db, Id);
                model.FirstName = WebUtility.HtmlDecode(model.FirstName);
                model.LastName = WebUtility.HtmlDecode(model.LastName);
                return View(@"~\Areas\Admin\Views\User\Create.cshtml", model);
            }
            else
            {
                TempData[StringUtility.ErrorMessage] = ResourceFile.Failure_General;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(tblUserMaster model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DBEnum.DBAction action = model.Id == 0 ? DBEnum.DBAction.Insert : DBEnum.DBAction.Update;
                    tblUserMasterBL.Instance.InsertUpdate(db, model, action);
                    if (action == DBEnum.DBAction.Insert)
                        TempData[StringUtility.SuccessMessage] = UtilityManager.OperationResponseMessage(ResourceFile.Insert);
                    else
                        TempData[StringUtility.SuccessMessage] = UtilityManager.OperationResponseMessage(ResourceFile.Update);

                    TempData[StringUtility.Current_Page] = Request[StringUtility.Current_Page];
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewData[StringUtility.ErrorMessage] = ResourceFile.Failure_General;
                    CustomException.HandleException(ex);
                }
            }
            return View(model);
        }

        public ActionResult Delete(Int64 Id)
        {
            try
            {
                bool Status = tblUserMasterBL.Instance.Delete(db, Id);
                return Content("Deleted");
            }
            catch (Exception ex)
            {
                CustomException.HandleException(ex);
            }
            return Content(string.Empty);
        }

        public ActionResult UpdateStatus()
        {
            string strStatus = string.Empty;
            try
            {
                if (RouteData.Values["id"] != null)
                {
                    Int64 Id = Convert.ToInt64(RouteData.Values["id"]);
                    int Status = tblUserMasterBL.Instance.UpdateStatus(db, Id);
                    strStatus = Enum.GetName(typeof(DBEnum.DBStatus), Status);
                }
            }
            catch (Exception ex)
            {
                CustomException.HandleException(ex);
            }
            return Content(strStatus);
        }

        public ActionResult Cancel(int current_page)
        {
            TempData[StringUtility.Current_Page] = current_page;
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            db.Dispose();
        }
    }
}