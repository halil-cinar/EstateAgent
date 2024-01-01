using EstateAgent.Dto.Abstract;
using EstateAgent.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.Dtos
{
    public class RoleMethodDto:DtoBase
    {
        public RoleTypes Role { get; set; }

        public MethodTypes Method { get; set; }
    }
}
