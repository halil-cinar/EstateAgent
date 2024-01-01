using EstateAgent.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.DataAccess
{
    public interface IEntityRepository<T>
        where T : EntityBase,new()
    {
        public T Add(T entity);
        public T Update(T entity);
        public T Delete(long id);
        public T Delete(T entity);

        public T GetById(long id);
        public List<T> GetAll(Expression<Func<T,bool>>? filter=null);
        public T Get(Expression<Func<T, bool>> filter);
    }
}
