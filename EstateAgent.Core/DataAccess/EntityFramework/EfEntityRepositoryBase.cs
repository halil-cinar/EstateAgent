using EstateAgent.DataAccess;
using EstateAgent.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TContext, TEntity> : IEntityRepository<TEntity>
        where TContext : DbContext, new()
        where TEntity : EntityBase, new()
    {
        public TEntity Add(TEntity entity)
        {
            using(var context=new TContext())
            {
                var entry=context.Entry(entity);
                entry.State = EntityState.Added;
                context.SaveChanges();
                return entry.Entity;
            }
        }

        public TEntity Delete(long id)
        {
            using (var context = new TContext())
            {
                var entities = context.Set<TEntity>().Where(e => e.Id == id);
                context.Set<TEntity>().RemoveRange(entities);
                context.SaveChanges();
                return entities.ToList()[0];
            }
        }

        public TEntity Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                var entry = context.Entry(entity);
                entry.State = EntityState.Deleted;
                context.SaveChanges();
                return entry.Entity;
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().FirstOrDefault(filter);
            }
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null)
        {
            using (var context = new TContext())
            {
                return (filter == null)
                    ? context.Set<TEntity>().ToList()
                    : context.Set<TEntity>().Where(filter).ToList();
            }
        }

        public TEntity GetById(long id)
        {
           return Get(x=>x.Id == id);
        }

        public TEntity Update(TEntity entity)
        {
            using (var context = new TContext())
            {
                var entry = context.Entry(entity);
                entry.State = EntityState.Modified;
                context.SaveChanges();
                return entry.Entity;
            }
        }
    }
}
