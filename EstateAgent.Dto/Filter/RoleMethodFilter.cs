using EstateAgent.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.Filter
{
    public class RoleMethodFilter
    {
        public RoleTypes? Role { get; set; }

        public MethodTypes? Method { get; set; }
    }
}
