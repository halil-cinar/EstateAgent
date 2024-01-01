using EstateAgent.Entities.Abstract;
using EstateAgent.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    public class RoleMethodEntity:EntityBase
    {
        public RoleTypes Role { get; set; }

        public MethodTypes Method { get; set; }

        

        

    }
}
