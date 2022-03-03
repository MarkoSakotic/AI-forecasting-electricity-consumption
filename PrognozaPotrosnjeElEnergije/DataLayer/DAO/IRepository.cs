using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DAO
{
    interface IRepository<TEntity> where TEntity : class
    {
        TEntity FindById(object Id);
        List<TEntity> GetList();
        void Insert(TEntity entuty);
        void Delete(object id);
        void Update(TEntity entityToUpdate);
    }
}
