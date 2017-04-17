using CustomAuth;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;

namespace UtilityLayer
{
    /// <summary>
    /// Utility class contains all common methods used through out application
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// Append Base Virtual Path with passed parameter.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static string AppendBaseVirtualPath(string FilePath)
        {
            string strPath = string.Format("{0}\\{1}", HttpContext.Current.Server.MapPath("/"), FilePath);
            return strPath;
        }

        public static bool CreateFolder(string FilePath)
        {
            try
            {
                string folder = Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
            }
            catch (Exception) { return false; }
            return true;
        }

        public static string SaveUploadFile(HttpPostedFileBase fuPostedFile, string strFileName, string strFileAbsolutePath)
        {
            bool FolderStatus = CreateFolder(Utility.AppendBaseVirtualPath(strFileAbsolutePath));
            string strNewFileName = strFileName;
            if (fuPostedFile != null && fuPostedFile.ContentLength > 0)
            {
                strNewFileName = Guid.NewGuid() + "_" + DateTime.Now.ToString("ddmmyyhhmmss") + System.IO.Path.GetExtension(fuPostedFile.FileName);
                if (!string.IsNullOrEmpty(strFileName))
                {
                    string strFullPath = Utility.AppendBaseVirtualPath(strFileAbsolutePath) + strFileName;
                    if (System.IO.File.Exists(strFullPath))
                        System.IO.File.Delete(strFullPath);
                }
                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(fuPostedFile.InputStream))
                {
                    fileData = binaryReader.ReadBytes(fuPostedFile.ContentLength);
                    System.IO.File.WriteAllBytes(Utility.AppendBaseVirtualPath(strFileAbsolutePath) + strNewFileName, fileData);
                }
                strNewFileName = (strFileAbsolutePath + strNewFileName).Replace("\\", "/");
            }
            return strNewFileName;
        }

        public static string HashPassword(string strPassword)
        {
            // Note this password is 'salted' with the username when it is passed
            // to this method, basic dictionary attack would not work
            return FormsAuthentication.HashPasswordForStoringInConfigFile(strPassword, FormsAuthPasswordFormat.MD5.ToString());
        }

        /// <summary>
        /// Generate random string of given length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomText(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return result.ToString();
        }

        /// <summary>
        /// Created By : Shambhu Pasi
        /// Description : This Action will read the file from specified path and return the file content. 
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        public static string ReadFile(string strFilePath)
        {
            string strText = string.Empty;
            StreamReader streamReader = new StreamReader(strFilePath);
            strText = streamReader.ReadToEnd();
            streamReader.Close();
            return strText;
        }

        #region DateTime related
        /// <summary>
        /// Fetch only date value from passed date object
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public static DateTime ConvertToDate(DateTime? Date)
        {
            if (Date == null)
                return DateTime.Now.Date;
            else
                return Convert.ToDateTime(Date).Date;
        }

        /// <summary>
        /// Convert passed date object to specified date string format [currently MMDDYYYY]
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public static string ConvertDateTimeToString(DateTime? Date)
        {
            string result = string.Empty;
            try
            {
                if (Date != null)
                    result = Convert.ToDateTime(Date).ToString(StringUtility.MMDDYYYY, CultureInfo.InvariantCulture);
            }
            catch { }
            return result;
        }

        /// <summary>
        /// Convert passed string to valide datetime format
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static DateTime ConvertStringToDateTime(string strDate)
        {
            try
            {
                if (!string.IsNullOrEmpty(strDate))
                {
                    DateTime dtReturnDate;
                    DateTime.TryParseExact(strDate, StringUtility.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtReturnDate);
                    return dtReturnDate;
                }
            }
            catch { }
            return new DateTime();
        }
        #endregion

        public static bool IsValidImage(string fileName)
        {
            Regex regex = new Regex(@"(.*?)\.(jpg|JPG|jpeg|JPEG|png|PNG|gif|GIF|bmp|BMP)$");
            return regex.IsMatch(fileName);
        }
    }
}
