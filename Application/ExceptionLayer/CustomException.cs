using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityLayer;
using ResourceLayer;

namespace ExceptionLayer
{
    /// <summary>
    /// Specify all type of exception here, create user defined exception
    /// Generate text file for all Logs and exception
    /// </summary>
    public class CustomException : Exception
    {
        private Exception _ex;
        public CustomException()
        {
            this._ex = new Exception();
        }
        public override System.Collections.IDictionary Data
        {
            get { return _ex.Data; }
        }
        public override Exception GetBaseException()
        {
            return _ex.GetBaseException();
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string Message
        {
            get { return _ex.Message; }
        }
        public override string StackTrace
        {
            get { return _ex.StackTrace; }
        }
        public override string ToString()
        {
            return _ex.ToString();
        }
        public override bool Equals(object obj)
        {
            return _ex.Equals(obj);
        }
        public CustomException(string strException)
        {
            this._ex = new Exception(strException);
        }
        public CustomException(Exception ex)
        {
            this._ex = ex;
            CustomException.HandleException(this);
        }

        /// <summary>
        /// HandleException function will Create a log file of whole exception.
        /// auther : Shambhu Pasi
        /// </summary>
        /// <param name="e"></param>
        public static void HandleException(Exception ex)
        {
            try
            {
                if (StringUtility.WriteException)
                {
                    FileStream fs = null;
                    StreamWriter sw = null;
                    string strFilePath = StringUtility.Exception_FullPath.Replace("$", DateTime.Now.ToString("dd_MMM_yyyy"));
                    Utility.CreateFolder(strFilePath);
                    if (!File.Exists(strFilePath))
                        fs = File.Create(strFilePath);
                    else
                        sw = File.AppendText(strFilePath);
                    sw = sw == null ? new StreamWriter(fs) : sw;

                    sw.WriteLine("\n************************************** Exception Log Start **************************************");
                    sw.WriteLine("\n****************************************************************************************************");
                    sw.WriteLine("\nDATE : ");
                    sw.WriteLine("\n" + DateTime.Now.ToString());
                    sw.WriteLine("\nMESSAGE : ");
                    sw.WriteLine("\n" + ex.Message);
                    if (ex != null && ex.InnerException != null)
                    {
                        sw.WriteLine("\nINNER EXCEPTION : ");
                        sw.WriteLine("\n" + ex.InnerException.Message);
                    }
                    sw.WriteLine("\nStack Trace:");
                    sw.WriteLine("\n" + ex.StackTrace);
                    sw.WriteLine("\nSource : ");
                    sw.WriteLine("\n" + ex.Source);
                    sw.WriteLine("\n************************************** Exception Log End **************************************");
                    sw.WriteLine("\n****************************************************************************************************");
                    sw.Close();
                    if (fs != null)
                        fs.Close();
                }
            }
            catch { }
        }

        /// <summary>
        /// CreateLog function will Create a log of passed string.
        /// auther : Shambhu Pasi 
        /// </summary>
        /// <param name="strLogString"></param>
        public static void CreateLog(string Content)
        {
            try
            {
                if (StringUtility.WriteException)
                {
                    FileStream fs = null;
                    StreamWriter sw = null;
                    string strFilePath = StringUtility.Log_FullPath.Replace("$", DateTime.Now.ToString("dd_MMM_yyyy"));
                    Utility.CreateFolder(strFilePath);
                    if (!File.Exists(strFilePath))
                        fs = File.Create(strFilePath);
                    else
                        sw = File.AppendText(strFilePath);
                    sw = sw == null ? new StreamWriter(fs) : sw;

                    sw.WriteLine(DateTime.Now.ToString() + " - " + Content);
                    sw.Close();
                    if (fs != null)
                        fs.Close();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }

    public class InvalidUsernamePasswordException : CustomException
    {
        public InvalidUsernamePasswordException()
            : base(ResourceFile.User_Invalid_Username_Password) { }
    }

    public class InvalidPasswordException : CustomException
    {
        public InvalidPasswordException()
            : base(ResourceFile.User_Invalid_Password) { }
    }

    public class RecordNotFoundException : CustomException
    {
        public RecordNotFoundException()
            : base(ResourceFile.Record_Not_Found) { }
    }

    public class UserAlreadyVerifiedException : CustomException
    {
        public UserAlreadyVerifiedException()
            : base(ResourceFile.User_Already_Verified) { }
    }

    public class EmailAlreadyRegisteredException : CustomException
    {
        public EmailAlreadyRegisteredException()
            : base(ResourceFile.User_Email_Already_Registered) { }
    }

    public class GeneralException : CustomException
    {
        public GeneralException()
            : base(ResourceFile.Failure_General) { }
    }

    public class UserEmailNotFoundException : CustomException
    {
        public UserEmailNotFoundException()
            : base(ResourceFile.User_Email_Not_Found) { }
    }

    public class InactiveAccountException : CustomException
    {
        public InactiveAccountException()
            : base(ResourceFile.User_Account_Inactive) { }
    }

    public class UserVerificationPendingException : CustomException
    {
        public UserVerificationPendingException()
            : base(ResourceFile.User_Verification_Pending) { }
    }
}
