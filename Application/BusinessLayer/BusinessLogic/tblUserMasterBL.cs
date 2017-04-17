using CustomAuth;
using DataLayer.DataLogic;
using EntityLayer;
using ExceptionLayer;
using System;
using System.Collections.Generic;
using UtilityLayer;

namespace BusinessLayer.BusinessLogic
{
    public class tblUserMasterBL
    {
        private static readonly Lazy<tblUserMasterBL> _instance = new Lazy<tblUserMasterBL>(() => new tblUserMasterBL());
        public static tblUserMasterBL Instance
        {
            get { return _instance.Value; }
        }
        private tblUserMasterBL() { }

        public UserContextModel ValidateUser(ApplicationDBEntities db, string UserId, string Password)
        {
            UserContextModel model = null;
            if (StringUtility.SessionBased)
            {
                if (UserId.Equals(StringUtility.AdminUsername) && Password.Equals(StringUtility.AdminPassword))
                {
                    model = PrepareUserContext(new tblUserMaster { Id = 1, EmailId = UserId, FirstName = "Admin" });
                }
            }
            else
            {
                Password = Utility.HashPassword(Password);
                model = PrepareUserContext(tblUserMasterDL.Instance.ValidateUser(db, UserId, Password));
            }
            if (model == null)
                throw new InvalidUsernamePasswordException();
            return model;
        }

        public UserContextModel PrepareUserContext(tblUserMaster model)
        {
            UserContextModel result = null;
            if (model != null)
                result = new UserContextModel
                {
                    userid = model.Id,
                    emailid = model.EmailId,
                    firstname = model.FirstName,
                    lastname = model.LastName,
                    Roles = new List<UserRoleModel>()
                };
            return result;
        }

        public tblUserMaster GetDataByPK(ApplicationDBEntities db, long Id)
        {
            tblUserMaster result = tblUserMasterDL.Instance.GetDataByPK(db, Id);
            return result;
        }

        public List<tblUserMaster> GetData(ApplicationDBEntities db, tblUserFilterModel filterModel, int currentPageIndex, out int Total)
        {
            List<tblUserMaster> lstResult = tblUserMasterDL.Instance.GetData(db, filterModel, currentPageIndex, out Total);
            return lstResult;
        }

        public tblUserMaster InsertUpdate(ApplicationDBEntities db, tblUserMaster model, DBEnum.DBAction action)
        {
            model.Password = Utility.HashPassword(model.Password);
            tblUserMaster result = tblUserMasterDL.Instance.InsertUpdate(db, model, action);
            return model;
        }

        public bool Delete(ApplicationDBEntities db, long Id)
        {
            bool Status = tblUserMasterDL.Instance.Delete(db, Id);
            return Status;
        }

        public int UpdateStatus(ApplicationDBEntities db, long ID)
        {
            int Status = tblUserMasterDL.Instance.UpdateStatus(db, ID);
            return Status;
        }
    }
}
