using EstateAgent.Dto.Abstract;
using EstateAgent.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.Dtos
{
    public class UserRoleFilter
    {
        public long? UserId { get; set; }
        public RoleTypes? Role { get; set; }

        


    }
}
