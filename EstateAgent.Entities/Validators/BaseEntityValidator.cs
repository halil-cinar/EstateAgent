using EstateAgent.Entities.Abstract;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities.Validators
{
    public class BaseEntityValidator<TEntity>:AbstractValidator<TEntity>
        where TEntity : EntityBase, new()
    {
        public BaseEntityValidator()
        {
            RuleFor(x => x.IsDeleted).NotNull();
            RuleFor(x => x.IsDeletable).NotNull();

        }

    }
}
