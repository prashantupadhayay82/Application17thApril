using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UtilityLayer
{
    /// <summary>
    /// All static entries used through out the application
    /// It also includes Web.config all AppSetting entries
    /// </summary>
    public class StringUtility
    {
        #region AppSetting Entry
        public static readonly string WebsiteURL = ConfigurationManager.AppSettings["WebsiteURL"];
        public static readonly string URLReferrerPath = ConfigurationManager.AppSettings["URLReferrerPath"];
        public static readonly string Exception_AbsolutePath = ConfigurationManager.AppSettings["ExceptionPath"];
        public static readonly string Log_AbsolutePath = ConfigurationManager.AppSettings["LogPath"];
        public static readonly string UploadedFile_AbsolutePath = ConfigurationManager.AppSettings["UploadedFilePath"];
        public static readonly string Exception_FullPath = Utility.AppendBaseVirtualPath(Exception_AbsolutePath);
        public static readonly string Log_FullPath = Utility.AppendBaseVirtualPath(Log_AbsolutePath);
        public static readonly string UploadedFile_FullPath = Utility.AppendBaseVirtualPath(UploadedFile_AbsolutePath);
        public static readonly bool WriteException = Convert.ToBoolean(ConfigurationManager.AppSettings["WriteException"]);
        public static readonly bool SessionBased = Convert.ToBoolean(ConfigurationManager.AppSettings["SessionBased"]);
        public static readonly string AdminUsername = ConfigurationManager.AppSettings["AdminUsername"];
        public static readonly string AdminPassword = ConfigurationManager.AppSettings["AdminPassword"];
        #endregion

        #region Date Format Entry
        public static readonly string MMDDYYYY = ConfigurationManager.AppSettings["MMDDYYYY"];
        public static readonly string MMDDYY = ConfigurationManager.AppSettings["MMDDYY"];
        public static readonly string DDMMYYYY = ConfigurationManager.AppSettings["DDMMYYYY"];
        public static readonly string DDMMYY = ConfigurationManager.AppSettings["DDMMYY"];
        #endregion

        #region Basic configuration Entry
        public const string AdminSession = "AdminSession";
        public const string SuccessMessage = "SuccessMessage";
        public const string ErrorMessage = "ErrorMessage";
        public const string VDPager = "VDPager";
        public const string Current_Page = "Current_Page";
        public const string DefaultDropdownText = "Please Select";
        public static readonly int ItemsPerPage = Convert.ToInt32(ConfigurationManager.AppSettings["ItemsPerPage"]);
        #endregion

        public static string[] DateTimeFormat = new string[]{
        "MM/dd/yyyy HH:mm:ss",
        "M/dd/yyyy HH:mm:ss",
        "MM/d/yyyy HH:mm:ss",
        "M/d/yyyy HH:mm:ss",
        "MM/dd/yyyy",
        "M/dd/yyyy",
        "MM/d/yyyy",
        "M/d/yyyy",
        "dd/MM/yyyy HH:mm:ss",
        "d/MM/yyyy HH:mm:ss",
        "dd/M/yyyy HH:mm:ss",
        "d/M/yyyy HH:mm:ss",
        "dd/MM/yyyy",
        "d/MM/yyyy",
        "dd/M/yyyy",
        "d/M/yyyy"
      };
    }
}