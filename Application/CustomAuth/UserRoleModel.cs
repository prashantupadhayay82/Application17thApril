using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomAuth
{
    [Serializable]
    public class UserRoleModel
    {
        public long personaid { get; set; }
        public Nullable<int> roleid { get; set; }
        public string role_name { get; set; }
        public Nullable<byte> status { get; set; }
        public Nullable<int> specificcolumn1 { get; set; }
        public Nullable<long> specificcolumn2 { get; set; }
        public string entities_name { get; set; }
        //public Nullable<byte> regulator_status { get; set; }
    }
}
