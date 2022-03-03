using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DAO
{
    public abstract class BaseRepo<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public BaseRepo()
        {

        }

        public void Delete(object id)
        {
            using (var db = new WeatherConditionsContainer())
            {
                TEntity entityToDelete = db.Set<TEntity>().Find(id);
                db.Entry(entityToDelete).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public TEntity FindById(object Id)
        {
            using (var db = new WeatherConditionsContainer())
            {
                return db.Set<TEntity>().Find(Id);
            }
        }

        public List<TEntity> GetList()
        {
            using (var db = new WeatherConditionsContainer())
            {
                return db.Set<TEntity>().ToList();
            }
        }

        public void Insert(TEntity entity)
        {
            using (var db = new WeatherConditionsContainer())
            {
                db.Set<TEntity>().Add(entity);
                db.SaveChanges();
            }
        }

        public void Update(TEntity entityToUpdate)
        {
            using (var db = new WeatherConditionsContainer())
            {
                db.Set<TEntity>().Attach(entityToUpdate);
                db.Entry(entityToUpdate).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
