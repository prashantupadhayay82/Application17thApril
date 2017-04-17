using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace Website.Models
{
    public class UtilityManager
    {
        public static void TempDataToViewData(ControllerContext CurrentController)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(CurrentController.Controller.TempData[StringUtility.SuccessMessage])))
                CurrentController.Controller.ViewData[StringUtility.SuccessMessage] = CurrentController.Controller.TempData[StringUtility.SuccessMessage];
            if (!string.IsNullOrEmpty(Convert.ToString(CurrentController.Controller.TempData[StringUtility.ErrorMessage])))
                CurrentController.Controller.ViewData[StringUtility.ErrorMessage] = CurrentController.Controller.TempData[StringUtility.ErrorMessage];
        }

        public static string OperationResponseMessage(string Operation)
        {
            return string.Format(ResourceLayer.ResourceFile.Success_General, Operation);
        }
    }
}