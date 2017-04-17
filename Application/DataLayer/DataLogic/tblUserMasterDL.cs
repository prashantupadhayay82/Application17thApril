using EntityLayer;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using UtilityLayer;
using System.Linq.Expressions;

namespace DataLayer.DataLogic
{
    public class tblUserMasterDL : DbHelper<tblUserMaster>
    {
        private static readonly Lazy<tblUserMasterDL> _instance = new Lazy<tblUserMasterDL>(() => new tblUserMasterDL());
        public static tblUserMasterDL Instance
        {
            get { return _instance.Value; }
        }
        private tblUserMasterDL() { }

        public tblUserMaster ValidateUser(ApplicationDBEntities db, string UserId, string Password)
        {
            return GetSingle(db, (x => x.EmailId == UserId && x.Password == Password));
        }

        public tblUserMaster GetDataByPK(ApplicationDBEntities db, long Id)
        {
            return GetSingle(db, x => x.Id == Id);
        }

        public List<tblUserMaster> GetData(ApplicationDBEntities db, tblUserFilterModel filterModel, int currentPageIndex, out int Total)
        {
            List<tblUserMaster> result = null;
            if (filterModel == null)
            {
                result = GetPagedList(db, null, _order => _order.Id, currentPageIndex, out Total);
            }
            else
            {
                var predicate = PredicateBuilder.False<tblUserMaster>();
                if (!string.IsNullOrEmpty(filterModel.FirstName))
                    predicate = predicate.Or<tblUserMaster>(x => x.FirstName.Contains(filterModel.FirstName));
                if (!string.IsNullOrEmpty(filterModel.LastName))
                    predicate = predicate.Or<tblUserMaster>(x => x.LastName.Contains(filterModel.LastName));
                //predicate.Compile();
                result = GetPagedList(db, predicate, _order => _order.Id, currentPageIndex, out Total);
            }
            return result;
        }

        public tblUserMaster InsertUpdate(ApplicationDBEntities db, tblUserMaster model, DBEnum.DBAction action)
        {
            model.FirstName = WebUtility.HtmlEncode(model.FirstName);
            model.LastName = WebUtility.HtmlEncode(model.LastName);
            if (action == DBEnum.DBAction.Update)
            {
                return UpdateEntity(db, model);
            }
            else
            {
                return InsertEntity(db, model);
            }
        }

        public bool Delete(ApplicationDBEntities db, long Id)
        {
            return DeleteEntity(db, x => x.Id == Id);
        }

        public int UpdateStatus(ApplicationDBEntities db, long ID)
        {
            tblUserMaster result = (from sel in db.tblUserMasters where sel.Id == ID select sel).FirstOrDefault();
            if (result != null)
            {
                if (result.Status == (int)DBEnum.DBStatus.Active)
                    result.Status = (int)DBEnum.DBStatus.Inactive;
                else
                    result.Status = (int)DBEnum.DBStatus.Active;
                db.SaveChanges();
            }
            return (int)result.Status;
        }
    }
}
