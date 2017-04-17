using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomAuth
{
    [Serializable]
    public class UserContextModel
    {
        private string _UserCode = Guid.NewGuid().ToString();
        public string UserCode
        {
            get { return _UserCode; }
        }
        public long userid { get; set; }
        public string emailid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string FullName { get { return this.firstname + " " + this.lastname; } }
        public Nullable<byte> status { get; set; }
        public List<UserRoleModel> Roles { get; set; }
        public UserRoleModel CurrentRole { get; set; }
    }
}