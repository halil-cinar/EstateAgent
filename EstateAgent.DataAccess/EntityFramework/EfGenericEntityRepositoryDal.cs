using EstateAgent.Core.DataAccess.EntityFramework;
using EstateAgent.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.DataAccess.EntityFramework
{
    public class EfGenericEntityRepositoryDal<TEntity>:EfEntityRepositoryBase<DatabaseContext,TEntity>,IEntityRepository<TEntity>
        where TEntity : EntityBase,new()
    {

    }
}
