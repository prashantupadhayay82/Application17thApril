using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityLayer
{
    /// <summary>
    /// Enum used through out the application 
    /// </summary>
    public class DBEnum
    {
        /// <summary>
        /// This enum will used to specify database operation type
        /// </summary>
        public enum DBAction
        {
            Insert = 1,
            Update
        }

        public enum DBStatus
        {
            Active = 1,
            Inactive
        }

        public enum ErrorCode
        {
            GeneralError = 1,
            PageSessionExpired,
            LoginSessionExpired,
            AccessDenied,
            InvalidRequest,
            EmailExpired,
            error505,
            error404
        }

    }
}
