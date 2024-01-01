using AutoMapper;
using EstateAgent.DataAccess;
using EstateAgent.Entities.Abstract;
using EstateAgent.Entities.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Business
{
    public class ManagerBase<TEntity,TViewEntity>
        where TEntity:EntityBase,new()
        where TViewEntity:EntityBase, new()
    {
        protected BaseEntityValidator<TEntity> Validator { get; private set; }

        protected IMapper Mapper { get; private set; }

        protected IEntityRepository<TEntity> Repository { get; private set; }

        protected IEntityRepository<TViewEntity> ListEntityRepository { get; private set; }

        public ManagerBase(BaseEntityValidator<TEntity> validator, IMapper mapper, IEntityRepository<TEntity> repository, IEntityRepository<TViewEntity> listEntityRepository)
        {
            Validator = validator;
            Mapper = mapper;
            Repository = repository;
            ListEntityRepository = listEntityRepository;
        }


    }
}
