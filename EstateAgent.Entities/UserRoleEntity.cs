using EstateAgent.Entities.Abstract;
using EstateAgent.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    [Table("UserRole")]
    public class UserRoleEntity:EntityBase
    {
        public long UserId { get; set; }
        public RoleTypes Role { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }

    }
}
