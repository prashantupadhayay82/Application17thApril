using EntityLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using UtilityLayer;

namespace DataLayer
{
    public abstract class DbHelper<TEntity> where TEntity : class
    {
        protected TEntity GetSingle(ApplicationDBEntities db, Expression<Func<TEntity, bool>> condition)
        {
            TEntity entity = db.Set<TEntity>().FirstOrDefault(condition);
            return entity;
        }

        protected List<TEntity> GetMany(ApplicationDBEntities db, Expression<Func<TEntity, bool>> condition)
        {
            return db.Set<TEntity>().Where(condition).ToList();
        }

        protected List<TEntity> GetPagedList(ApplicationDBEntities db, Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, Int64>> _order, int currentPageIndex, out int Total)
        {
            IQueryable<TEntity> lstResult = db.Set<TEntity>().AsQueryable();
            if (condition != null)
                lstResult = lstResult.Where(condition).AsQueryable();
            Total = lstResult.Count();
            lstResult = lstResult.OrderBy(_order).AsQueryable().Skip((currentPageIndex - 1) * StringUtility.ItemsPerPage).Take(StringUtility.ItemsPerPage);
            return lstResult.ToList();
        }

        protected TEntity InsertEntity(ApplicationDBEntities db, TEntity entity)
        {
            db.Set<TEntity>().Add(entity);
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            return entity;
        }

        protected TEntity UpdateEntity(ApplicationDBEntities db, TEntity entity)
        {
            db.Set<TEntity>().Attach(entity);
            db.Entry<TEntity>(entity).State = EntityState.Modified;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            return entity;
        }

        protected bool DeleteEntity(ApplicationDBEntities db, Expression<Func<TEntity, bool>> condition)
        {
            try
            {
                db.Set<TEntity>().RemoveRange(db.Set<TEntity>().Where(condition));
                db.Configuration.ValidateOnSaveEnabled = false;
                return db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}